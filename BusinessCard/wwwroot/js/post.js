window.onload = init;
var authorizedUser = false;
var userName = "";
var postId = null;
var commentsCount = 0;
var viewCommentsCount = 0;
var lastBranchId = 0;
var isLoad = false;
var isLoadVisible = false;
var isLoadComments = false;
var isLoadCommentsVisible = false;
var isLoadNextComments = false;
var isLoadNextCommentsVisible = false;
var isLoadNextCommentsInBranch = false;
var isLoadNextCommentsInBranchVisible = false;
var userTopchik = null;
var userBookmark = null;
var topchikQueue = [];
var bookmarkQueue = [];
var branchesNotView = [];
var textInDeletedComment = "Комментарий удалён пользователем";
var newCommentsInBranches = new Map();

function init() {
    Timer();
    hrefParts = $(location).attr('href').split("/");
    postKey = hrefParts[hrefParts.length - 1];
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetPostInformation?postKey=" + postKey,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            authorizedUser = jsonData.AuthorizedUser;
            userName = jsonData.UserName;
            var postInformation = jsonData.PostInformation;
            var postData = postInformation.Post;
            var postDetails = postInformation.PostDetails;
            var personalInformation = postInformation.PersonalInformation.StatisticsByPost;
            postId = postData.Id;



            var channelName = document.querySelector('.channelName');
            channelName.innerHTML = postData.ChannelName;
            var date = document.querySelector('.date');
            date.innerHTML = postData.Date;
            var viewsValue = document.querySelector('.viewsValue');
            viewsValue.innerHTML = postData.ViewsCount;



            var postHeader = document.querySelector('.postHeader');
            postHeader.innerHTML = postData.Name;



            var bookmarksBlock = document.createElement("div");
            bookmarksBlock.className = "bookmarksBlock";
            bookmarksBlock.setAttribute('onclick', 'InBookmarks()');
            userBookmark = personalInformation[postId]['Bookmarks'];
            if (userBookmark)
                bookmarksBlock.innerHTML = '<svg viewBox="0 0 24 24" id="bookmarksTop" class="bookmarksIcon needBookmarks"><use xlink:href="#bookmarks"></use></svg>';
            else
                bookmarksBlock.innerHTML = '<svg viewBox="0 0 24 24" id="bookmarksTop" class="bookmarksIcon notBookmarks"><use xlink:href="#bookmarks"></use></svg>';

            var statisticValueBookmark = document.createElement("p");
            statisticValueBookmark.id = "bookmarkValueTop";
            statisticValueBookmark.className = "statisticValue";
            statisticValueBookmark.innerHTML = postData.BookmarksCount;

            if (postData.BookmarksCount == 0)
                statisticValueBookmark.classList.add("noValue");

            if (userBookmark)
                statisticValueBookmark.classList.add("bookmarksValue");

            bookmarksBlock.append(statisticValueBookmark);

            var commentsBlock = document.createElement("div");
            commentsBlock.className = "commentsCountBlock";
            commentsBlock.innerHTML = '<svg viewBox="0 0 24 24" class="commentsIcon"><use xlink:href="#comments"></use></svg>';
            commentsBlock.setAttribute('onclick', 'GoToComments()');

            var statisticValueComment = document.createElement("p");
            statisticValueComment.id = "commentValue";
            statisticValueComment.innerHTML = postData.CommentsCount;

            if (postData.CommentsCount == 0)
                statisticValueComment.classList.add("noValue");
            else if (postData.CommentsCount > 0)
                statisticValueComment.classList.add("statisticValue");

            commentsBlock.append(statisticValueComment);

            var thirdSection = document.querySelector('.thirdSection');
            thirdSection.append(bookmarksBlock);
            thirdSection.append(commentsBlock);



            var fourthSection = document.querySelector('.fourthSection');
            for (var i = 0; i < postDetails.length; i++) {
                switch (postDetails[i].DetailType) {
                    case "Header":
                        SetHeader(fourthSection, postDetails[i].Data);
                        break;
                    case "Text":
                        SetText(fourthSection, postDetails[i].Data);
                        break;
                    case "Code":
                        SetCode(fourthSection, postDetails[i].Data, postDetails[i].Description);
                        break;
                    case "Image":
                        SetImage(fourthSection, postDetails[i].Data, postDetails[i].Description);
                        break;
                }
            }



            var fifthSection = document.querySelector('.fifthSection');
            var topchikBlock = document.createElement("div");
            topchikBlock.id = "topchikBlock";
            topchikBlock.className = "topchikBlock";
            userTopchik = personalInformation[postId]['Topchiks'];;
            if (userTopchik)
                topchikBlock.classList.add("topchikUser");
            else
                topchikBlock.classList.add("untopchikUser");
            topchikBlock.setAttribute('onclick', 'SendTopchik()');
            topchikBlock.innerHTML = '<svg viewBox="0 0 64 64" class="topchikIcon"><use xlink:href="#topchik"></use></svg>';

            var statisticValueTopchik = document.createElement("p");
            statisticValueTopchik.id = "topchikValue";
            statisticValueTopchik.className = "statisticValue";
            statisticValueTopchik.innerHTML = postData.TopchiksCount;

            if (postData.TopchiksCount == 0) {
                topchikBlock.classList.add("noTopchik");
                statisticValueTopchik.classList.add("noValue");
            }

            topchikBlock.append(statisticValueTopchik);

            var bookmarksBlock2 = document.createElement("div");
            bookmarksBlock2.className = "bookmarksBlock";
            bookmarksBlock2.setAttribute('onclick', 'InBookmarks()');
            if (userBookmark)
                bookmarksBlock2.innerHTML = '<svg viewBox="0 0 24 24" id="bookmarksBottom" class="bookmarksIcon needBookmarks"><use xlink:href="#bookmarks"></use></svg>';
            else
                bookmarksBlock2.innerHTML = '<svg viewBox="0 0 24 24" id="bookmarksBottom" class="bookmarksIcon notBookmarks"><use xlink:href="#bookmarks"></use></svg>';

            var statisticValueBookmark2 = document.createElement("p");
            statisticValueBookmark2.id = "bookmarkValueBottom";
            statisticValueBookmark2.className = "statisticValue";
            statisticValueBookmark2.innerHTML = postData.BookmarksCount;

            if (postData.BookmarksCount == 0)
                statisticValueBookmark2.classList.add("noValue");

            if (userBookmark)
                statisticValueBookmark2.classList.add("bookmarksValue");

            bookmarksBlock2.append(statisticValueBookmark2);

            fifthSection.append(topchikBlock);
            fifthSection.append(bookmarksBlock2);



            var commentsHeader = document.querySelector('.commentsHeader');
            commentsHeader.innerHTML = "Комментарии (" + postData.CommentsCount + ")";
            commentsCount = postData.CommentsCount;

            ViewPostData();

            if (!authorizedUser)
                $('#answerBlock').remove();

            SetFirstCommentsPack();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function Timer() {
    isLoad = false;
    isLoadVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            var preloader = document.getElementById(`postDataSpinner`);
            $(preloader).fadeIn();
        }
        isLoad = false;
    };
};

function SetHeader(fourthSection, headerData) {
    var header = document.createElement("p");
    header.className = "text";
    header.innerHTML = '<b>' + headerData + '</b>';
    fourthSection.append(header);
}

function SetText(fourthSection, textData) {
    var text = document.createElement("p");
    text.className = "text";
    text.innerHTML = textData;
    fourthSection.append(text);
}

function SetCode(fourthSection, codeData, description) {
    var codeBlock = document.createElement("div");
    codeBlock.className = "codeBlock";

    var headerBlock = document.createElement("div");
    headerBlock.className = "headerBlock";
    var codeDescription = document.createElement("p");
    codeDescription.className = "codeDescription";
    codeDescription.innerHTML = description;
    headerBlock.append(codeDescription);
    codeBlock.append(headerBlock);

    var separatorInCode = document.createElement("div");
    separatorInCode.className = "separatorInCode";
    codeBlock.append(separatorInCode);

    var code = document.createElement("code");
    code.innerHTML = codeData;
    codeBlock.append(code);

    fourthSection.append(codeBlock);
}

function SetImage(fourthSection, imageData, description) {
    var imageBlock = document.createElement("div");
    imageBlock.className = "imageBlock";

    var image = document.createElement("img");
    image.setAttribute("src", imageData);
    imageBlock.append(image);

    var imageDescription = document.createElement("p");
    imageDescription.innerHTML = description;
    imageBlock.append(imageDescription);

    fourthSection.append(imageBlock);
}

function ViewPostData() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.getElementById(`postDataSpinner`);
            $(preloader).fadeOut();
        }

        $('.postBlock').fadeIn();
        $('.commentsBlock').fadeIn();

        $('html, body').animate({ scrollTop: $(".postBlock").offset().top }, 228);
    }, 228);
}

function SetFirstCommentsPack() {
    TimerComments();
    var params = 'postId=' + postId + '&commentsCount=' + commentsCount;
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetFirstCommentsInformation?" + params,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var branches = jsonData.CommentsInformation.Branches;
            SetComments(branches);
            if (viewCommentsCount < commentsCount) {
                var nextBranchesButton = document.createElement("p");
                nextBranchesButton.id = "nextBranchesButton";
                nextBranchesButton.className = "nextCommentsButton";
                $(nextBranchesButton).css({ "margin": "0 28px 28px 28px" });
                nextBranchesButton.innerHTML = "Показать ещё комментарии (" + (commentsCount - viewCommentsCount) + ")";
                nextBranchesButton.setAttribute("onclick", "SetNextCommentsPack()");
                document.querySelector('.commentsBlock').append(nextBranchesButton);
            }
            ViewCommentsData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetNextCommentsPack() {
    TimerNextComments();
    $("#nextBranchesButton").remove();
    var params = 'postId=' + postId + '&lastBranchId=' + lastBranchId;
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetCommentsInformation?" + params,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var branches = jsonData.CommentsInformation.Branches;
            SetComments(branches, true);
            if (viewCommentsCount < commentsCount) {
                var nextBranchesButton = document.createElement("p");
                nextBranchesButton.id = "nextBranchesButton";
                nextBranchesButton.className = "nextCommentsButton";
                $(nextBranchesButton).css({ "margin": "0 28px 28px 28px" });
                nextBranchesButton.innerHTML = "Показать ещё обсуждения (" + (commentsCount - viewCommentsCount) + ")";
                nextBranchesButton.setAttribute("onclick", "SetNextCommentsPack()");
                document.querySelector('.commentsBlock').append(nextBranchesButton);
            }
            ViewNextCommentsData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetNextAllCommentsPack(commentId) {
    TimerNextComments();
    $("#nextBranchesButton").remove();
    var params = 'postId=' + postId + '&lastBranchId=' + lastBranchId + '&allNextComments=' + true;
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetCommentsInformation?" + params,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var branches = jsonData.CommentsInformation.Branches;
            SetComments(branches, true);
            ViewNextCommentsData(commentId);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function TimerComments() {
    isLoadComments = false;
    isLoadCommentsVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadComments) {
            isLoadCommentsVisible = true;
            var preloader = document.getElementById(`commentsDataSpinner`);
            $(preloader).fadeIn();
        }
        isLoadComments = false;
    };
};

function TimerNextComments() {
    isLoadNextComments = false;
    isLoadNextCommentsVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadNextComments) {
            isLoadNextCommentsVisible = true;
            $("#branch" + lastBranchId).after('<div id="branchesPreloader" class="preloader loaded" style="width: 40px; height: 81px;"><svg class="preloader__image" style="width: 40px; height: 40px; margin-top: 21px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadNextComments = false;
    };
};

function SetComments(branches, stopView = false) {
    branchesNotView = [];
    var lastCommentId = 0;
    var commentsWrapper = document.querySelector('.commentsWrapper');
    for (var i = 0; i < branches.length; i++) {
        var commentsBranch = document.createElement("div");
        commentsBranch.id = "branch" + branches[i].Id;
        commentsBranch.className = "commentsBranch";
        if (stopView) {
            $(commentsBranch).css({ "display": "none" });
            branchesNotView.push(commentsBranch.id);
        }
        var comments = branches[i].Comments;
        for (var j = 0; j < comments.length; j++) {
            var commentBlock = document.createElement("div");
            commentBlock.id = "comment" + comments[j].Id;
            commentBlock.className = "commentBlock";
            if (j > 0)
                $(commentBlock).css({ "margin-left": "28px" });

            var dataBlock = document.createElement("div");
            dataBlock.className = "dataBlock";
            if (!comments[j].IsDeleted) {
                var commentator = document.createElement("p");
                commentator.className = "commentator";
                commentator.innerHTML = comments[j].UserName;
                var time = document.createElement("p");
                time.className = "time";
                time.innerHTML = comments[j].Time;
                dataBlock.append(commentator);
                dataBlock.append(time);
                if (j > 0) {
                    var asnwerToAnswer = document.createElement("div");
                    asnwerToAnswer.className = "answer-to-answer";
                    asnwerToAnswer.innerHTML = '<svg viewBox="0 0 24 24" class="answer-to-answer" onclick="GoToMainComment(' + comments[j].CommentId + ')"><use xlink:href="#answerToAnswer"></use></svg>';
                    dataBlock.append(asnwerToAnswer);
                }
                if (comments[j].BelongsUser) {
                    var deleteCommentButton = document.createElement("p");
                    deleteCommentButton.id = "deleteButton" + comments[j].Id;
                    deleteCommentButton.className = "deleteCommentButton";
                    deleteCommentButton.innerHTML = "×";
                    deleteCommentButton.setAttribute("onclick", "DeleteComment(" + comments[j].Id + ")");
                    dataBlock.append(deleteCommentButton);
                }
            }
            else {
                var time = document.createElement("p");
                time.className = "time";
                time.innerHTML = comments[j].Time;
                $(time).css({ "margin-left": "0" });
                dataBlock.append(time);
                if (j > 0) {
                    var asnwerToAnswer = document.createElement("div");
                    asnwerToAnswer.className = "answer-to-answer";
                    asnwerToAnswer.innerHTML = '<svg viewBox="0 0 24 24" class="answer-to-answer" onclick="GoToMainComment(' + comments[j].CommentId + ')"><use xlink:href="#answerToAnswer"></use></svg>';
                    dataBlock.append(asnwerToAnswer);
                }
            }

            var comment = document.createElement("p");
            comment.className = "comment";
            if (comments[j].IsDeleted) {
                comment.innerHTML = textInDeletedComment;
                $(comment).css({ "font-style": "italic" });
            }
            else {
                comment.innerHTML = comments[j].Text;
            }

            commentBlock.append(dataBlock);
            commentBlock.append(comment);
            if (!comments[j].IsDeleted) {
                var answer = document.createElement("p");
                answer.className = "answer";
                answer.innerHTML = "Ответить";
                answer.setAttribute("onclick", "GetAnswer('" + comments[j].UserName + ", ', " + branches[i].Id + ", " + comments[j].Id + ")");
                commentBlock.append(answer);
            }

            commentsBranch.append(commentBlock);

            lastCommentId = comments[j].Id;
        }
        if (branches[i].CommentsCount > 7) {
            var nextCommentsButton = document.createElement("p");
            nextCommentsButton.id = "nextCommentsButton" + branches[i].Id;
            nextCommentsButton.className = "nextCommentsButton";
            $(nextCommentsButton).css({ "margin-left": "28px" });
            nextCommentsButton.innerHTML = "Показать ещё ответы (" + (branches[i].CommentsCount - 7) + ")";
            nextCommentsButton.setAttribute("onclick", "SetNextCommentsInBranch(" + branches[i].Id + ", " + lastCommentId + ")");
            commentsBranch.append(nextCommentsButton);
        }

        viewCommentsCount += branches[i].CommentsCount;
        lastBranchId = branches[i].Id;
        commentsWrapper.append(commentsBranch);
    }
}

function GoToMainComment(commentId) {
    $('html, body').animate({ scrollTop: $('#comment' + commentId).offset().top }, 228);
    $('#comment' + commentId).css("background", "rgba(211, 211, 211, .22)");
    window.setTimeout(whiteBackground, 2601);
    function whiteBackground() {
        $('#comment' + commentId).css("background", "white");
    };
}

function GetAnswer(nameForAnswer, branchId, commentId) {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    $('.inBranch').remove();

    var answerBlock = document.createElement("div");
    answerBlock.id = "answerBlock" + commentId;
    answerBlock.className = "answerBlock inBranch";
    $(answerBlock).css('display', 'none');
    $(answerBlock).css('margin', '0');
    $(answerBlock).css('margin-top', '12px');
    $(answerBlock).css('margin-left', '28px');

    var answerCreator = document.createElement("div");
    answerCreator.className = "answerCreator";
    answerCreator.id = "userAnswerForComment";
    answerCreator.setAttribute('contenteditable', true);
    answerCreator.setAttribute('data-placeholder', 'Написать комментарий...');
    answerCreator.innerText = nameForAnswer;

    answerBlock.append(answerCreator);

    var sendBlock = document.createElement("div");
    sendBlock.className = "sendBlock";
    sendBlock.innerHTML = '<svg viewBox="0 0 24 24" class="sender"><use xlink:href="#send"></use></svg>';
    sendBlock.setAttribute("onclick", "SetNewComment(" + branchId + ", " + commentId + ")");

    answerBlock.append(sendBlock);

    document.getElementById('comment' + commentId).after(answerBlock);

    $(answerBlock).fadeIn();
    $(answerCreator).focus();

    var tmp = $('<span />').appendTo($(answerCreator)),
        node = tmp.get(0),
        range = null,
        sel = null;

    if (document.selection) {
        range = document.body.createTextRange();
        range.moveToElementText(node);
        range.select();
    } else if (window.getSelection) {
        range = document.createRange();
        range.selectNode(node);
        sel = window.getSelection();
        sel.removeAllRanges();
        sel.addRange(range);
    }

    answerCreator.removeChild(answerCreator.lastElementChild);
}

function ViewCommentsData() {
    isLoadComments = true;

    setTimeout(function () {
        if (isLoadCommentsVisible) {
            var preloader = document.getElementById(`commentsDataSpinner`);
            $(preloader).fadeOut();
        }

        $('.commentsWrapper').fadeIn();
    }, 228);
}

function ViewNextCommentsData(commentId = -1) {
    isLoadNextComments = true;

    setTimeout(function () {
        if (isLoadNextCommentsVisible) {
            $('#branchesPreloader').remove();
        }

        for (var i = 0; i < branchesNotView.length; i++) {
            $('#' + branchesNotView[i]).fadeIn();
        }

        if (commentId != -1)
            GoToMainComment(commentId);
    }, 228);
}

function SetNextCommentsInBranch(branchId, lastCommentId) {
    var isLoadNextCommentsInBranch = false;
    var isLoadNextCommentsInBranchVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadNextCommentsInBranch) {
            isLoadNextCommentsInBranchVisible = true;
            $("#branch" + branchId).append('<div id="comment' + lastCommentId + 'Preloader" class="preloader loaded" style="width: 40px; height: 81px; "><svg class="preloader__image" style="width: 40px; height: 40px; margin-top: 21px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadNextCommentsInBranch = false;
    };
    $("#nextCommentsButton" + branchId).remove();
    var params = 'postId=' + postId + '&branchId=' + branchId + '&lastCommentId=' + lastCommentId;
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetCommentsByBranch?" + params,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var comments = jsonData.Comments;

            for (var i = 0; i < comments.length; i++) {
                var commentBlock = document.createElement("div");
                commentBlock.id = "comment" + comments[i].Id;
                commentBlock.className = "commentBlock";
                $(commentBlock).css({ "margin-left": "28px" });

                var dataBlock = document.createElement("div");
                dataBlock.className = "dataBlock";
                if (!comments[i].IsDeleted) {
                    var commentator = document.createElement("p");
                    commentator.className = "commentator";
                    commentator.innerHTML = comments[i].UserName;
                    var time = document.createElement("p");
                    time.className = "time";
                    time.innerHTML = comments[i].Time;
                    var asnwerToAnswer = document.createElement("div");
                    asnwerToAnswer.className = "answer-to-answer";
                    asnwerToAnswer.innerHTML = '<svg viewBox="0 0 24 24" class="answer-to-answer" onclick="GoToMainComment(' + comments[i].CommentId + ')"><use xlink:href="#answerToAnswer"></use></svg>';
                    dataBlock.append(commentator);
                    dataBlock.append(time);
                    dataBlock.append(asnwerToAnswer);
                    if (comments[i].BelongsUser) {
                        var deleteCommentButton = document.createElement("p");
                        deleteCommentButton.id = "deleteButton" + comments[i].Id;
                        deleteCommentButton.className = "deleteCommentButton";
                        deleteCommentButton.innerHTML = "×";
                        deleteCommentButton.setAttribute("onclick", "DeleteComment(" + comments[i].Id + ")");
                        dataBlock.append(deleteCommentButton);
                    }
                }
                else {
                    var time = document.createElement("p");
                    time.className = "time";
                    time.innerHTML = comments[i].Time;
                    $(time).css({ "margin-left": "0" });
                    var asnwerToAnswer = document.createElement("div");
                    asnwerToAnswer.className = "answer-to-answer";
                    asnwerToAnswer.innerHTML = '<svg viewBox="0 0 24 24" class="answer-to-answer" onclick="GoToMainComment(' + comments[i].CommentId + ')"><use xlink:href="#answerToAnswer"></use></svg>';
                    dataBlock.append(time);
                    dataBlock.append(asnwerToAnswer);
                }

                var comment = document.createElement("p");
                comment.className = "comment";
                comment.innerHTML = comments[i].Text;
                if (comments[i].IsDeleted) {
                    comment.innerHTML = textInDeletedComment;
                    $(comment).css({ "font-style": "italic" });
                }
                else {
                    comment.innerHTML = comments[i].Text;
                }

                commentBlock.append(dataBlock);
                commentBlock.append(comment);
                if (!comments[i].IsDeleted) {
                    var answer = document.createElement("p");
                    answer.className = "answer";
                    answer.innerHTML = "Ответить";
                    answer.setAttribute("onclick", "GetAnswer('" + comments[i].UserName + ", ', " + branchId + ", " + comments[i].Id + ")");
                    commentBlock.append(answer);
                }

                $('#branch' + branchId).append(commentBlock);
            }
            isLoadNextCommentsInBranch = true;
            if (isLoadNextCommentsInBranchVisible) {
                $('#comment' + lastCommentId + 'Preloader').remove();
            }

            if (newCommentsInBranches.has(branchId)) {
                GoToMainComment(newCommentsInBranches.get(branchId));
                newCommentsInBranches.delete(branchId);
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetNewCommentInNewBranch() {
    if ($("#answerBlock").find('.answerCreator').text() == "")
        return;

    var newComment = {
        Text: $("#answerBlock").find('.answerCreator').text(),
        PostId: postId,
        BranchId: 0,
        CommentId: 0
    }

    var isLoadCreateComment = false;
    var isLoadCreateCommentVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadCreateComment) {
            isLoadCreateCommentVisible = true;
            $("#answerBlock").find('.sendBlock').fadeOut();
            $("#answerBlock").append('<div id="createAfterComment" class="preloader loaded" style="width: 20px; height: 20px; margin-left: 12px;"><svg class="preloader__image" style="width: 20px; height: 20px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadCreateComment = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/CreateComment",
        contentType: 'application/json',
        data: JSON.stringify(newComment),
        traditional: true,
        success: function (result) {
            var jsonData = $.parseJSON(result);
            var newBranchId = jsonData.BranchId;
            var newCommentId = jsonData.CommentId;

            if ($(".commentsWrapper").find('#nextBranchesButton') != undefined) {
                SetNextAllCommentsPack(newCommentId);
            }
            else {
                var commentsBranch = document.createElement("div");
                commentsBranch.id = "branch" + newBranchId;
                commentsBranch.className = "commentsBranch";

                var commentBlock = document.createElement("div");
                commentBlock.id = "comment" + newCommentId;
                commentBlock.className = "commentBlock";

                var dataBlock = document.createElement("div");
                dataBlock.className = "dataBlock";

                var commentator = document.createElement("p");
                commentator.className = "commentator";
                commentator.innerHTML = userName;
                var time = document.createElement("p");
                time.className = "time";
                time.innerHTML = "Сегодня";
                var deleteCommentButton = document.createElement("p");
                deleteCommentButton.id = "deleteButton" + newCommentId;
                deleteCommentButton.className = "deleteCommentButton";
                deleteCommentButton.innerHTML = "×";
                deleteCommentButton.setAttribute("onclick", "DeleteComment(" + newCommentId + ")");
                dataBlock.append(commentator);
                dataBlock.append(time);
                dataBlock.append(deleteCommentButton);

                var comment = document.createElement("p");
                comment.className = "comment";
                comment.innerHTML = $("#answerBlock").find('.answerCreator').text();

                var answer = document.createElement("p");
                answer.className = "answer";
                answer.innerHTML = "Ответить";
                answer.setAttribute("onclick", "GetAnswer('" + userName + ", ', " + newBranchId + ", " + newCommentId + ")");

                commentBlock.append(dataBlock);
                commentBlock.append(comment);
                commentBlock.append(answer);

                commentsBranch.append(commentBlock);

                if ($('.commentsWrapper').find('#branch' + newBranchId) != undefined)
                    $('.commentsWrapper').find('#branch' + newBranchId).remove();

                $('.commentsWrapper').append(commentsBranch);

                GoToMainComment(newCommentId);
            }

            $("#answerBlock").find('.answerCreator').text("");

            commentsCount += 1;
            $('.commentsHeader').text("Комментарии (" + commentsCount + ")");
            $('#commentValue').text(commentsCount);
            var commentValue = document.getElementById('commentValue');
            commentValue.classList.remove("noValue");
            commentValue.classList.add("statisticValue");

            isLoadCreateComment = true;
            if (isLoadCreateCommentVisible) {
                $("#createAfterComment").remove();
            }

            $("#answerBlock").find('.sendBlock').fadeIn();
        },
        error: function (error) {
            alert(error);
        }
    });
}

async function SetNewComment(branchId, commentId) {
    if ($("#answerBlock" + commentId).find('.answerCreator').text() == "")
        return;

    var newComment = {
        Text: $("#answerBlock" + commentId).find('.answerCreator').text(),
        PostId: postId,
        BranchId: branchId,
        CommentId: commentId
    }

    var isLoadCreateComment = false;
    var isLoadCreateCommentVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadCreateComment) {
            isLoadCreateCommentVisible = true;
            $("#answerBlock" + commentId).find('.sendBlock').fadeOut();
            $("#answerBlock" + commentId).append('<div id="createAfterComment' + commentId + '" class="preloader loaded" style="width: 20px; height: 20px; margin-left: 12px;"><svg class="preloader__image" style="width: 20px; height: 20px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadCreateComment = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/CreateComment",
        contentType: 'application/json',
        data: JSON.stringify(newComment),
        traditional: true,
        success: function (result) {
            var jsonData = $.parseJSON(result);
            var newCommentId = jsonData.CommentId;

            if ($("#branch" + branchId).find('.nextCommentsButton') != undefined) {
                newCommentsInBranches.set(branchId, newCommentId);
                $("#branch" + branchId).find('.nextCommentsButton').trigger('click');
            }
            else {
                var commentBlock = document.createElement("div");
                commentBlock.id = "comment" + newCommentId;
                commentBlock.className = "commentBlock";
                $(commentBlock).css({ "margin-left": "28px" });

                var dataBlock = document.createElement("div");
                dataBlock.className = "dataBlock";

                var commentator = document.createElement("p");
                commentator.className = "commentator";
                commentator.innerHTML = userName;
                var time = document.createElement("p");
                time.className = "time";
                time.innerHTML = "Сегодня";
                var asnwerToAnswer = document.createElement("div");
                asnwerToAnswer.className = "answer-to-answer";
                asnwerToAnswer.innerHTML = '<svg viewBox="0 0 24 24" class="answer-to-answer" onclick="GoToMainComment(' + commentId + ')"><use xlink:href="#answerToAnswer"></use></svg>';
                var deleteCommentButton = document.createElement("p");
                deleteCommentButton.id = "deleteButton" + newCommentId;
                deleteCommentButton.className = "deleteCommentButton";
                deleteCommentButton.innerHTML = "×";
                deleteCommentButton.setAttribute("onclick", "DeleteComment(" + newCommentId + ")");
                dataBlock.append(commentator);
                dataBlock.append(time);
                dataBlock.append(asnwerToAnswer);
                dataBlock.append(deleteCommentButton);

                var comment = document.createElement("p");
                comment.className = "comment";
                comment.innerHTML = $("#answerBlock" + commentId).find('.answerCreator').text();

                var answer = document.createElement("p");
                answer.className = "answer";
                answer.innerHTML = "Ответить";
                answer.setAttribute("onclick", "GetAnswer('" + userName + ", ', " + branchId + ", " + newCommentId + ")");

                commentBlock.append(dataBlock);
                commentBlock.append(comment);
                commentBlock.append(answer);

                $("#branch" + branchId).append(commentBlock);

                GoToMainComment(newCommentId);
            }

            $("#answerBlock" + commentId).remove();

            commentsCount += 1;
            $('.commentsHeader').text("Комментарии (" + commentsCount + ")");
            $('#commentValue').text(commentsCount);
            var commentValue = document.getElementById('commentValue');
            commentValue.classList.remove("noValue");
            commentValue.classList.add("statisticValue");

            isLoadDeleteComment = true;
            if (isLoadCreateCommentVisible) {
                $("#createAfterComment" + commentId).remove();
            }

            loadForNewComment = false;
        },
        error: function (error) {
            alert(error);
        }
    });
}

function DeleteComment(commentId) {
    var isLoadDeleteComment = false;
    var isLoadDeleteCommentVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadDeleteComment) {
            isLoadDeleteCommentVisible = true;
            $("#deleteButton" + commentId).fadeOut();
            $("#comment" + commentId).find('.dataBlock').append('<div id="deleteComment' + commentId + '" class="preloader loaded" style="width: 20px; height: 20px; margin-left: 40px;"><svg class="preloader__image" style="width: 20px; height: 20px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadDeleteComment = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/DeleteComment",
        data: { commentId },
        dataTpye: "json",
        traditional: true,
        success: function (result) {
            if (result) {
                $("#deleteButton" + commentId).remove();
                $("#comment" + commentId).find('.dataBlock').find('.commentator').fadeOut();
                $("#comment" + commentId).find('.dataBlock').find('.time').css({ "margin-left": "0" });
                $("#comment" + commentId).find('.answer').fadeOut();
                $("#comment" + commentId).find('.comment').text(textInDeletedComment);
                $("#comment" + commentId).find('.comment').css({ "font-style": "italic" });
            }
            else {
                $("#deleteButton" + commentId).fadeIn();
            }
            isLoadDeleteComment = true;
            if (isLoadDeleteCommentVisible) {
                $("#deleteComment" + commentId).remove();
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SendTopchik() {
    if (!authorizedUser) {
        OpenInformationBlock("topchik");
        return;
    }

    if (userTopchik) {
        if (topchikQueue.length == 0) {
            topchikQueue.push(false);
            SendTopchikToServer(false);
        }
        else {
            SetTopchikCondition(false);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue').innerHTML) - 1;
        $("#topchikValue").html(Math.round(newValue));
        if (newValue == 0) {
            document.getElementById('topchikBlock').classList.add("noTopchik");
            document.getElementById('topchikValue').classList.add("noValue");
        }
        document.getElementById('topchikBlock').classList.remove("topchikUser");
        document.getElementById('topchikBlock').classList.add("untopchikUser");
        userTopchik = false;
    }
    else {
        if (topchikQueue.length == 0) {
            topchikQueue.push(true);
            SendTopchikToServer(true);
        }
        else {
            SetTopchikCondition(true);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue').innerHTML) + 1;
        $("#topchikValue").html(Math.round(newValue));
        if (newValue == 1) {
            document.getElementById('topchikBlock').classList.remove("noTopchik");
            document.getElementById('topchikValue').classList.remove("noValue");
        }
        document.getElementById('topchikBlock').classList.remove("untopchikUser");
        document.getElementById('topchikBlock').classList.add("topchikUser");
        userTopchik = true;
    }
}

function SendTopchikToServer(isTopchik) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/SendTopchik",
        data: { postId, isTopchik },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (topchikQueue.length != 1) {
                    topchikQueue.splice(topchikQueue.indexOf(isTopchik), 1);
                    SendTopchikToServer(topchikQueue[0]);
                }
            }

        },
        error: function (error) {

        }
    });
}

function SetTopchikCondition(condition) {
    if (topchikQueue.length == 1 && topchikQueue[0] != condition)
        topchikQueue.push(condition);
    else if (topchikQueue.length == 2 && topchikQueue[1] != condition)
        topchikQueue.pop();
}

function InBookmarks() {
    if (!authorizedUser) {
        OpenInformationBlock("bookmarks");
        return;
    }

    if (userBookmark) {
        if (bookmarkQueue.length == 0) {
            bookmarkQueue.push(false);
            InBookmarksToServer(false);
        }
        else {
            SetBookmarkCondition(false);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarkValueTop').innerHTML) - 1;
        $("#bookmarkValueTop").html(Math.round(newValue));
        $("#bookmarkValueBottom").html(Math.round(newValue));
        if (newValue == 0) {
            document.getElementById('bookmarkValueTop').classList.add("noValue");
            document.getElementById('bookmarkValueBottom').classList.add("noValue");
        }
        document.getElementById('bookmarksTop').classList.remove("needBookmarks");
        document.getElementById('bookmarksTop').classList.add("notBookmarks");
        document.getElementById('bookmarksBottom').classList.remove("needBookmarks");
        document.getElementById('bookmarksBottom').classList.add("notBookmarks");
        document.getElementById('bookmarkValueTop').classList.remove("bookmarksValue");
        document.getElementById('bookmarkValueBottom').classList.remove("bookmarksValue");
        userBookmark = false;
    }
    else {
        if (bookmarkQueue.length == 0) {
            bookmarkQueue.push(true);
            InBookmarksToServer(true);
        }
        else {
            SetBookmarkCondition(true);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarkValueTop').innerHTML) + 1;
        $("#bookmarkValueTop").html(Math.round(newValue));
        $("#bookmarkValueBottom").html(Math.round(newValue));
        if (newValue == 1) {
            document.getElementById('bookmarkValueTop').classList.remove("noValue");
            document.getElementById('bookmarkValueBottom').classList.remove("noValue");
        }
        document.getElementById('bookmarksTop').classList.remove("notBookmarks");
        document.getElementById('bookmarksTop').classList.add("needBookmarks");
        document.getElementById('bookmarksBottom').classList.remove("notBookmarks");
        document.getElementById('bookmarksBottom').classList.add("needBookmarks");
        document.getElementById('bookmarkValueTop').classList.add("bookmarksValue");
        document.getElementById('bookmarkValueBottom').classList.add("bookmarksValue");
        userBookmark = true;
    }
}

function InBookmarksToServer(inBookmark) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/SendPostInBookmark",
        data: { postId, inBookmark },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (bookmarkQueue.length != 1) {
                    bookmarkQueue.splice(bookmarkQueue.indexOf(inBookmark), 1);
                    SendTopchikToServer(bookmarkQueue[0]);
                }
            }

        },
        error: function (error) {

        }
    });
}

function SetBookmarkCondition(condition) {
    if (bookmarkQueue.length == 1 && bookmarkQueue[0] != condition)
        bookmarkQueue.push(condition);
    else if (bookmarkQueue.length == 2 && bookmarkQueue[1] != condition)
        bookmarkQueue.pop();
}

function OpenInformationBlock(typeOfInformation) {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.className = "fullWindow";

    var center = document.createElement("div");
    center.className = "center";

    if (typeOfInformation == "services") {
        var requirement = document.createElement("p");
        requirement.className = "requirement";
        requirement.innerHTML = '<a href="/Account/Login">Войдите</a> на платформу MAXon28, чтобы оставлять комментарии';

        var requirementDescription = document.createElement("p");
        requirementDescription.className = "requirementDescription";
        requirementDescription.innerHTML = 'Вы сможете оставлять комментарии и отвечать на другие';

        center.append(requirement);
        center.append(requirementDescription);
    }
    else if (typeOfInformation == "topchik") {
        var requirement = document.createElement("p");
        requirement.className = "requirement";
        requirement.innerHTML = '<a href="/Account/Login">Войдите</a>, чтобы оценить';
        center.append(requirement);
    }
    else if (typeOfInformation == "bookmarks") {
        var requirement = document.createElement("p");
        requirement.className = "requirement";
        requirement.innerHTML = '<a href="/Account/Login">Войдите</a>, чтобы сохранить статью в закладках';

        var requirementDescription = document.createElement("p");
        requirementDescription.className = "requirementDescription";
        requirementDescription.innerHTML = 'Закладки хранятся в Вашем профиле и видны только Вам';

        center.append(requirement);
        center.append(requirementDescription);
    }

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseInformationBlock()");

    center.append(skipButton);

    fullWindow.append(center);
    html.append(fullWindow);
}

function CloseInformationBlock() {
    $('.fullWindow').remove();
}

function GoToComments() {
    $('html, body').animate({ scrollTop: $('.commentsBlock').offset().top }, 228);
}