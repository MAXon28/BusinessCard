window.onload = init;
var projectId = null;
var isLoad = false;
var isLoadVisible = false;
var isLoadReviews = false;
var isLoadReviewsVisible = false;
var isLoadReview = false;
var isLoadReviewVisible = false;
var openReviews = false;
var sliders = [];
var reviews = [];
var reviewData = null;
var pagesCount;
var reviewsDataByPages = new Object();
var currentPageId = "page1";
var rating = 1;

function init() {
    Timer();
    projectId = $(location).attr('href').split("=")[1];
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetProject?projectId=" + projectId,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (projectData) {
            var mainBlock = document.querySelector('.mainBlock');

            var nameBlock = document.createElement("div");
            nameBlock.className = "nameBlock";

            if (projectData.icon != null) {
                var projectIcon = document.createElement("img");
                projectIcon.className = "projectIcon";
                projectIcon.src = projectData.icon;
                nameBlock.append(projectIcon);
            }

            var projectName = document.createElement("p");
            projectName.className = "projectName";
            projectName.innerHTML = projectData.name;
            nameBlock.append(projectName);

            mainBlock.append(nameBlock);

            var typeCategoryBlock = document.createElement("div");
            typeCategoryBlock.className = "typeCategoryBlock";

            var typeName = document.createElement("p");
            typeName.className = "text type";
            typeName.innerHTML = projectData.type;
            $(typeName).css('text-align', 'cetner');
            typeCategoryBlock.append(typeName);

            var categoryName = document.createElement("p");
            categoryName.className = "text category";
            categoryName.innerHTML = projectData.category;
            $(categoryName).css('text=align', 'cetner');
            typeCategoryBlock.append(categoryName);

            mainBlock.append(typeCategoryBlock);

            if (projectData.clickType != null) {
                var clicksBlock = document.createElement("div");
                clicksBlock.className = "clicksBlock";

                var clicksCountBlock = document.createElement("div");
                clicksCountBlock.className = "clicksCountBlock";

                var clickCount = document.createElement("p");
                clickCount.className = "value";
                clickCount.innerHTML = projectData.clicksCount;

                var typeName = document.createElement("p");
                typeName.className = "text";
                typeName.innerHTML = projectData.clickType.typeName;

                clicksCountBlock.append(clickCount);
                clicksCountBlock.append(typeName);

                var clickProject = document.createElement("a");
                clickProject.className = "click";
                clickProject.innerHTML = projectData.clickType.actionName;
                if (projectData.clickType.actionName == "Скачать")
                    clickProject.setAttribute("download", "");
                clickProject.href = projectData.projectUrl;

                clicksBlock.append(clicksCountBlock);
                clicksBlock.append(clickProject);

                mainBlock.append(clicksBlock);
            }

            $(mainBlock).append('<div class="ratingInformationBlock"><div class="ratingBlock"><div class="ratingValueBlock"><p class="value" style="margin-right: 3px;">' + projectData.reviewInformation.avgRating + '</p><label class="rating__item" style="color: gold; margin-left: 3px"><svg class="rating__star"><use xlink:href="#star"></use></svg></label></div><p class="text">Среднее</p></div><div class="separator vertical"></div><div class="countReviewsBlock"><p class="value">' + projectData.reviewInformation.reviewsCount + '</p><p class="text">Оценки</p></div></div>');
            pagesCount = projectData.reviewInformation.reviewsPagesCount;

            var creationDate = document.createElement("p");
            creationDate.className = "text date";
            creationDate.innerHTML = projectData.creationDate;
            $(creationDate).css('text-align', 'cetner');
            mainBlock.append(creationDate);

            if (projectData.images != null && projectData.images.length > 0)
                SetScreenshots(projectData.images);

            if (projectData.videoUrl != null && projectData.videoUrl != "") {
                var video = document.getElementById(`video`);

                var videoUrlBlock = document.createElement("div");
                videoUrlBlock.className = "urlBlock videoUrlBlock";

                var videoInformationText = document.createElement("p");
                videoInformationText.className = "informationText";
                videoInformationText.innerHTML = "Демонстрация работы приложения: ";
                $(videoInformationText).css('margin', '12px 5px');
                $(videoInformationText).css('text-align', 'center');

                var videoUrl = document.createElement("a");
                videoUrl.className = "url";
                videoUrl.href = projectData.videoUrl;
                videoUrl.innerHTML = projectData.name + " (видео)";

                videoInformationText.append(videoUrl);

                videoUrlBlock.append(videoInformationText);

                video.append(videoUrlBlock);

                video.classList.remove("skip");
            }

            if (projectData.codeLevel != null && projectData.codeUrl != null && projectData.codeUrl != "") {
                var code = document.getElementById(`code`);

                var codeBlock = document.createElement("div");
                codeBlock.className = "codeBlock";

                var circle = document.createElement("div");
                circle.className = "circle per-" + projectData.codeLevel.percentage;

                var inner = document.createElement("div");
                inner.className = "inner";
                inner.innerHTML = projectData.codeLevel.percentage + "%";

                circle.append(inner);

                var annotation = document.createElement("p");
                annotation.className = "informationText";
                annotation.innerHTML = projectData.codeLevel.annotation;
                $(annotation).css('text-align', 'center');

                codeBlock.append(circle);
                codeBlock.append(annotation);

                var codeUrlBlock = document.createElement("div");
                codeUrlBlock.className = "urlBlock codeUrlBlock";

                var codeInformationText = document.createElement("p");
                codeInformationText.className = "informationText";
                codeInformationText.innerHTML = "Ссылка на код: ";
                $(codeInformationText).css('margin', '12px 5px');
                $(codeInformationText).css('text-align', 'center');

                var codeUrl = document.createElement("a");
                codeUrl.className = "url";
                codeUrl.href = projectData.codeUrl;
                codeUrl.innerHTML = projectData.name + " (код)";

                codeInformationText.append(codeUrl);

                codeUrlBlock.append(codeInformationText);

                code.append(codeBlock);
                code.append(codeUrlBlock);

                code.classList.remove("skip");
            }

            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.informationText').html(projectData.reviewInformation.avgRating);
            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.additionalText').html("Оценок: " + projectData.reviewInformation.reviewsCount);

            $('#fiveStars').find('.animated-progress').find('span').css('width', projectData.reviewInformation.ratingStatistic.excellentCount + "%");
            $('#fourStars').find('.animated-progress').find('span').css('width', projectData.reviewInformation.ratingStatistic.goodCount + "%");
            $('#threeStars').find('.animated-progress').find('span').css('width', projectData.reviewInformation.ratingStatistic.notBadCount + "%");
            $('#twoStars').find('.animated-progress').find('span').css('width', projectData.reviewInformation.ratingStatistic.badCount + "%");
            $('#oneStars').find('.animated-progress').find('span').css('width', projectData.reviewInformation.ratingStatistic.terriblyCount + "%");

            if (projectData.reviewInformation.reviewsCount > 0) {
                SetReviewData(projectData.reviewInformation.review, 1);
                reviews.push(1);

                if (projectData.reviewInformation.reviewsCount > 1) {
                    var turningAroundButtonReview = document.createElement("p");
                    turningAroundButtonReview.id = "turningAroundButtonReview";
                    turningAroundButtonReview.className = "turningAroundButton";
                    turningAroundButtonReview.innerHTML = "Посмотреть все";
                    turningAroundButtonReview.setAttribute("onclick", "SetReviews()");

                    document.getElementById(`reviews`).append(turningAroundButtonReview);
                }

                reviewData = projectData.reviewInformation.review;
            }

            document.getElementById(`reviews`).classList.remove("skip");

            var information = document.getElementById(`information`);

            var descriptionText = document.createElement("p");
            descriptionText.id = "description";
            descriptionText.className = "informationText descriptionText";

            var spanText = document.createElement("span");
            spanText.innerHTML = projectData.description;

            descriptionText.append(spanText);

            var turningAroundDescription = document.createElement("p");
            turningAroundDescription.id = "turningAroundDescription";
            turningAroundDescription.className = "turningAroundButton";
            turningAroundDescription.setAttribute("onclick", "ViewFullDescription()");
            turningAroundDescription.innerHTML = "Развернуть";

            information.append(descriptionText);
            information.append(turningAroundDescription);

            if (projectData.technicalRequirements != null) {
                var technicalRequirements = document.getElementById(`technicalRequirement`);

                $.each(projectData.technicalRequirements, function (key, value) {
                    var technicalRequirementValue = document.createElement("p");
                    technicalRequirementValue.className = "informationText";
                    technicalRequirementValue.innerHTML = "<b>" + key + ":</b> " + value;

                    technicalRequirements.append(technicalRequirementValue);
                });

                technicalRequirements.classList.remove("skip");
            }

            ViewData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function Timer() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            var preloader = document.getElementById(`dataSpinner`);
            preloader.classList.remove('delete');
        }
        isLoad = false;
    };
};

function SetScreenshots(images) {
    var screenshots = document.getElementById(`screenshots`);
    var html = document.querySelector('html');

    var slider = document.createElement("div");
    slider.className = "slider";

    var slider__wrapper = document.createElement("div");
    slider__wrapper.className = "slider__wrapper";

    var slider__items = document.createElement("div");
    slider__items.className = "slider__items";

    slider__wrapper.append(slider__items);

    slider.append(slider__wrapper);


    var sliderFullWindow = document.createElement("div");
    sliderFullWindow.className = "slider";

    var slider__wrapperFullWindow = document.createElement("div");
    slider__wrapperFullWindow.className = "slider__wrapper";

    var slider__itemsFullWindow = document.createElement("div");
    slider__itemsFullWindow.className = "slider__items";

    slider__wrapperFullWindow.append(slider__itemsFullWindow);

    sliderFullWindow.append(slider__wrapperFullWindow);

    if (images.length > 1) {
        $(slider).append('<a href="#" class="slider__control" data-slide="prev" style="border-radius: 5px 0 0 5px;"></a>');
        $(slider).append('<a href="#" class="slider__control" data-slide="next" style="border-radius: 0 5px 5px 0;"></a>');

        $(sliderFullWindow).append('<a href="#" class="slider__control" data-slide="prev" style="border-radius: 5px 0 0 5px;"></a>');
        $(sliderFullWindow).append('<a href="#" class="slider__control" data-slide="next" style="border-radius: 0 5px 5px 0;"></a>');
    }

    for (var i = 0; i < images.length; i++) {
        var sliderItem = document.createElement("div");
        sliderItem.className = "slider__item";

        var imageBlock = document.createElement("div");
        imageBlock.className = "imageBlock";

        var image = document.createElement("img");
        image.className = "image";
        image.src = "/" + images[i].path;
        image.setAttribute("onclick", "OpenFullScreen(" + (i) + ")");
        imageBlock.append(image);

        var imageDescription = document.createElement("p");
        imageDescription.className = "imageDescription";
        imageDescription.innerHTML = images[i].description;
        imageBlock.append(imageDescription);

        var sliderItemFullWindow = document.createElement("div");
        sliderItemFullWindow.className = "slider__item";
        sliderItemFullWindow.id = i;
        $(sliderItemFullWindow).css('flex', '0 0 100%');
        $(sliderItemFullWindow).css('width', '100%');

        var imageBlockFullWindow = document.createElement("div");
        imageBlockFullWindow.className = "imageBlock";
        $(imageBlockFullWindow).css('width', '655px');

        var imageFullWindow = document.createElement("img");
        imageFullWindow.className = "image";
        imageFullWindow.src = "/" + images[i].path;
        imageBlockFullWindow.append(imageFullWindow);

        var imageDescriptionFullWindow = document.createElement("p");
        imageDescriptionFullWindow.className = "imageDescription";
        imageDescriptionFullWindow.innerHTML = images[i].description;
        imageBlockFullWindow.append(imageDescriptionFullWindow);

        sliderItem.append(imageBlock);
        sliderItemFullWindow.append(imageBlockFullWindow);

        slider__items.append(sliderItem);
        slider__itemsFullWindow.append(sliderItemFullWindow);
    }

    var images = document.createElement("div");
    images.className = "images";
    images.append(slider);

    var fullWindow = document.createElement("div");
    fullWindow.className = "fullWindow";
    fullWindow.classList.add("skip");

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseFullScreen()");

    fullWindow.append(skipButton);
    fullWindow.append(sliderFullWindow);

    screenshots.append(images);
    html.append(fullWindow);
}

function ViewData() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('delete');
        }

        document.getElementById(`data`).classList.remove('delete');

        document.getElementById(`screenshots`).classList.remove('skip');

        var elms = document.querySelectorAll('.slider');

        for (var i = 0, len = elms.length; i < len; i++) {
            sliders.push(new ChiefSlider(elms[i], {
                loop: false,
                swipe: false,
                refresh: true
            }));
        }

        if (document.querySelector('.fullWindow') != undefined)
            document.querySelector('.fullWindow').classList.add("skip");

        document.getElementById(`video`).classList.remove('skip');
        document.getElementById(`code`).classList.remove('skip');
        document.getElementById(`reviews`).classList.remove('skip');

        if ($('#review1').find('span:first-child').height() <= 340) {
            $('#turningAroundReview1').css("display", "none");
        }
        else {
            $('.reviewText').toggleClass('hide');
            $('#turningAroundReview1').css("display", "block");
        }

        if ($('#description').find('span:first-child').height() <= 528)
            $('#turningAroundDescription').css("display", "none");
        else
            $('#turningAroundDescription').css("display", "block");

        document.getElementById(`information`).classList.remove('skip');
        document.getElementById(`technicalRequirement`).classList.remove('skip');

        $('html, body').animate({ scrollTop: $("#data").offset().top }, 228);
    }, 228);

}

function OpenFullScreen(id) {
    sliders[1].refresh();

    if ($('html div.fullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active').length == 0)
        sliders[1].next();

    if ($('html div.fullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id == id)
        document.querySelector('.fullWindow').classList.remove("skip");

    if ($('html div.fullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id < id)
        while ($('html div.fullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id != id)
            sliders[1].next();
    else
        while ($('html div.fullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id != id)
            sliders[1].prev();

    document.querySelector('.fullWindow').classList.remove("skip");
}

function CloseFullScreen() {
    document.querySelector('.fullWindow').classList.add("skip");
}

$(window).resize(function () {
    $.each(reviews, function (index, review) {
        if ($('#review' + review).find('span:first-child').height() <= 340)
            $('#turningAroundReview' + review).css("display", "none");
        else
            $('#turningAroundReview' + review).css("display", "block");
    });

    if (!openReviews) {
        if ($('#description').find('span:first-child').height() <= 528)
            $('#turningAroundDescription').css("display", "none");
        else
            $('#turningAroundDescription').css("display", "block");
    }
});

function ViewFullDescription() {
    if ($('#turningAroundDescription').text() == "Развернуть") {
        $('#description').height("auto");
        $('#description').css('maxHeight', 'none');
        $('#turningAroundDescription').text("Свернуть");
    }
    else {
        $('#description').css('maxHeight', 528);
        $('#turningAroundDescription').text("Развернуть");
    }
}

function ViewFullReview(reviewNumber) {
    if ($('#turningAroundReview' + reviewNumber).text() == "Весь отзыв") {
        $('#review' + reviewNumber).height("auto");
        $('#review' + reviewNumber).css('maxHeight', 'none');
        $('#review' + reviewNumber).toggleClass('hide');
        $('#turningAroundReview' + reviewNumber).text("Свернуть");
    }
    else {
        $('#review' + reviewNumber).css('maxHeight', 340);
        $('#review' + reviewNumber).toggleClass('hide');
        $('#turningAroundReview' + reviewNumber).text("Весь отзыв");
    }
}

function SetReviews() {
    openReviews = true;
    $('#screenshots').fadeOut('fast');
    $('#video').fadeOut('fast');
    $('#code').fadeOut('fast');
    $('#information').fadeOut('fast');
    $('#technicalRequirement').fadeOut('fast');
    $('.reviewStatisticBlock').fadeOut();
    $('.createReview').fadeOut();
    $('.reviewBlock').remove();
    $('#reviews').find('.turningAroundButton').fadeOut();
    $('html, body').animate({ scrollTop: $("#data").offset().top }, 228);

    if (reviewsDataByPages[1] != undefined) {
        SetReviewsData(reviewsDataByPages[1], true, false);
        if (pagesCount > 1)
            SetPagesButtons();
        return;
    }

    TimerReview();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetReviews?projectId=" + projectId + "&pageNumber=1",
        contentType: 'application/json',
        dataTpye: "json",
        success: function (reviews) {
            reviewsDataByPages[1] = reviews;
            SetReviewsData(reviews, true, false);
            if (pagesCount > 1)
                SetPagesButtons();
            ViewReviews();

            if (NeedUpdateReviewStatistic(reviewsDataByPages[1][0]))
                SetNewReviewStatistic();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetReviewsData(reviewsData, needSetSkipButton, beforeButtons) {
    reviews = [];

    $.each(reviewsData, function (index, data) {
        SetReviewData(data, index, true, beforeButtons);
        reviews.push(index);
    });

    if (needSetSkipButton) {
        var skipReviewButton = document.createElement("p");
        skipReviewButton.className = "skipReviewButton";
        skipReviewButton.innerHTML = "×";
        skipReviewButton.setAttribute("onclick", "CloseReviews()");
        document.getElementById(`reviews`).append(skipReviewButton);
    }
}

function SetPagesButtons() {
    currentPageId = "page1";

    var thirdSection = document.getElementById(`reviews`);

    var pagesButtonsBlock = document.createElement("div");
    pagesButtonsBlock.className = "pagesButtonsBlock";

    pageNumber = 1;
    while (pageNumber <= pagesCount) {
        var pageButton = document.createElement("p");
        if (pageNumber == 1) {
            pageButton.className = "pageButton active";
            $(pageButton).css({ "border-left": "0" });
        }
        else if (pageNumber == pagesCount) {
            pageButton.className = "pageButton";
            $(pageButton).css({ "border-right": "0" });
        }
        else
            pageButton.className = "pageButton";
        pageButton.innerHTML = pageNumber;
        pageButton.value = pageNumber;
        pageButton.id = "page" + pageNumber;
        $(pageButton).click(function () {
            OpenNewPage(this);
        });

        pagesButtonsBlock.append(pageButton);

        pageNumber++;
    }

    thirdSection.append(pagesButtonsBlock);
}

function OpenNewPage(pageButton) {
    $('.reviewBlock').remove();

    $('html, body').animate({ scrollTop: $("#data").offset().top }, 228);

    var currentPageButton = document.getElementById(currentPageId);
    currentPageButton.classList.remove("active");
    pageButton.classList.add("active");
    currentPageId = pageButton.id;

    if (reviewsDataByPages[$(pageButton).val()] != undefined) {
        SetReviewsData(reviewsDataByPages[$(pageButton).val()], false, true);
        return;
    }

    TimerReview();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetReviews?projectId=" + projectId + "&pageNumber=" + $(pageButton).val(),
        contentType: 'application/json',
        dataTpye: "json",
        success: function (reviews) {
            reviewsDataByPages[$(pageButton).val()] = reviews;
            SetReviewsData(reviews, false, true);
            ViewReviews();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function TimerReview() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadReviews) {
            isLoadReviewsVisible = true;
            var preloader = document.getElementById(`reviewSpinner`);
            preloader.classList.remove('delete');
        }
        isLoadReviews = false;
    };
};

function ViewReviews() {
    isLoadReviews = true;

    setTimeout(function () {
        if (isLoadReviewsVisible) {
            var preloader = document.getElementById(`reviewSpinner`);
            preloader.classList.add('delete');
        }

        $('html, body').animate({ scrollTop: $("#data").offset().top }, 228);
    }, 228);
}

function CloseReviews() {
    openReviews = false;
    reviews = [];
    $('#screenshots').fadeIn('fast');
    $('#video').fadeIn('fast');
    $('#code').fadeIn('fast');
    $('#information').fadeIn('fast');
    $('#technicalRequirement').fadeIn('fast');
    $('.reviewStatisticBlock').fadeIn();
    $('.createReview').fadeIn();
    $('.reviewBlock').remove();
    $('.skipReviewButton').remove();
    $('.pagesButtonsBlock').remove();
    $('#reviews').find('.turningAroundButton').fadeIn();
    SetReviewData(reviewData, 1, false);
    $('html, body').animate({ scrollTop: $("#data").offset().top }, 228);
}

function SetReviewData(reviewInformation, index, isAppend = true, beforeButtons = false) {
    var reviewBlock = document.createElement("div");
    reviewBlock.className = "reviewBlock";
    if (index == 0) {
        $(reviewBlock).css('margin-top', '28px');
    }

    var reviewerName = document.createElement("p");
    reviewerName.className = "informationText";
    $(reviewerName).css('margin', '0 0 12px 0');
    $(reviewerName).css('font-size', '22px');
    $(reviewerName).css('line-height', 'normal');
    $(reviewerName).css('font-weight', 'bold');
    reviewerName.innerHTML = reviewInformation.userName;
    reviewBlock.append(reviewerName);

    var joinBlock = document.createElement("div");
    joinBlock.className = "joinBlock";

    var rating = document.createElement("div");
    rating.className = "rating";

    var firstStar = document.createElement("label");
    firstStar.className = "rating__item";
    $(firstStar).css('margin', '0 5px 0 0');
    firstStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

    if (reviewInformation.rating > 0)
        firstStar.style.color = '#FFD700';

    var secondStar = document.createElement("label");
    secondStar.className = "rating__item";
    $(secondStar).css('margin', '0 5px');
    secondStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

    if (reviewInformation.rating > 1)
        secondStar.style.color = '#FFD700';

    var thirdStar = document.createElement("label");
    thirdStar.className = "rating__item";
    $(thirdStar).css('margin', '0 5px');
    thirdStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

    if (reviewInformation.rating > 2)
        thirdStar.style.color = '#FFD700';

    var fourthStar = document.createElement("label");
    fourthStar.className = "rating__item";
    $(fourthStar).css('margin', '0 5px');
    fourthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

    if (reviewInformation.rating > 3)
        fourthStar.style.color = '#FFD700';

    var fifthStar = document.createElement("label");
    fifthStar.className = "rating__item";
    $(fifthStar).css('margin', '0 5px');
    fifthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

    if (reviewInformation.rating > 4)
        fifthStar.style.color = '#FFD700';

    rating.append(firstStar);
    rating.append(secondStar);
    rating.append(thirdStar);
    rating.append(fourthStar);
    rating.append(fifthStar);

    var date = document.createElement("p");
    date.className = "informationText";
    $(date).css('margin', '0 0 0 28px');
    $(date).css('font-size', '22px');
    $(date).css('color', 'cdcac7');
    $(date).css('line-height', 'normal');
    date.innerHTML = reviewInformation.date;

    joinBlock.append(rating);
    joinBlock.append(date);
    reviewBlock.append(joinBlock);

    if (reviewInformation.text != null && reviewInformation.text != "") {
        var text = document.createElement("p");
        text.id = "review" + index;
        text.className = "informationText reviewText";
        $(text).css('margin', '7px 0 12px 0');
        $(text).css('font-size', '22px');
        text.innerHTML = "<span>" + reviewInformation.text + "</span>";
        reviewBlock.append(text);

        var turningAroundReview = document.createElement("p");
        turningAroundReview.id = "turningAroundReview" + index;
        turningAroundReview.className = "turningAroundButton";
        $(turningAroundReview).css('margin', '0 0 12px 0');
        turningAroundReview.setAttribute("onclick", "ViewFullReview(" + index + ")");
        turningAroundReview.innerHTML = "Весь отзыв";
        reviewBlock.append(turningAroundReview);
    }

    var separator = document.createElement("div");
    separator.className = "separator horizontal";
    $(separator).css('width', '5%');
    $(separator).css('height', '1px');
    if (reviewInformation.text == null || reviewInformation.text == "")
        $(separator).css('margin-top', '12px');
    reviewBlock.append(separator);  
    
    if (beforeButtons)
        $(".pagesButtonsBlock").before($(reviewBlock));
    else if (isAppend)
        document.getElementById(`reviews`).append(reviewBlock);
    else
        $("#turningAroundButtonReview").before($(reviewBlock));

    if ($('#review' + index).find('span:first-child').height() <= 340) {
        $('#turningAroundReview' + index).css("display", "none");
    }
    else {
        $(text).toggleClass('hide');
        $('#turningAroundReview' + index).css("display", "block");
    }

    reviews.push(index);
}

function NeedUpdateReviewStatistic(newReview) {
    if (reviewData.userName != newReview.userName) {
        reviewData = newReview;
        return true;
    }

    if (reviewData.rating != newReview.rating) {
        reviewData = newReview;
        return true;
    }

    if (reviewData.text != newReview.text) {
        reviewData = newReview;
        return true;
    }

    if (reviewData.date != newReview.date) {
        reviewData = newReview;
        return true;
    }

    return false;
}

function SetNewReviewStatistic() {
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetReviewsStatistic?projectId=" + projectId,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (reviewStatistic) {
            $('.ratingInformationBlock').find('.ratingBlock').find('.ratingValueBlock').find('.value').html(reviewStatistic.avgRating);
            $('.ratingInformationBlock').find('.countReviewsBlock').find('.value').html(reviewStatistic.reviewsCount);

            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.informationText').html(reviewStatistic.avgRating);
            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.additionalText').html("Оценок: " + reviewStatistic.reviewsCount);

            $('#fiveStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.excellentCount + "%");
            $('#fourStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.goodCount + "%");
            $('#threeStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.notBadCount + "%");
            $('#twoStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.badCount + "%");
            $('#oneStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.terriblyCount + "%");

            pagesCount = reviewStatistic.reviewsPagesCount;

            if (openReviews) {
                $('.pagesButtonsBlock').remove();

                if (pagesCount > 1)
                    SetPagesButtons();
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function CreateReview() {
    rating = 1;

    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "reviewCreation";
    fullWindow.className = "fullWindow";
    //fullWindow.classList.add("skip");

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseReviewCreation()");

    var center = document.createElement("div");
    center.className = "center";

    var inputBox = document.createElement("div");
    inputBox.className = "inputBox";

    var input = document.createElement("input");
    input.className = "input";
    input.setAttribute("type", "text");
    input.setAttribute("required", "required");
    input.setAttribute("autocomplete", "off");

    var placeholder = document.createElement("span");
    placeholder.className = "placeholder";
    placeholder.innerHTML = "Имя";

    var errorMessage = document.createElement("p");
    errorMessage.className = "errorMessage";
    errorMessage.innerHTML = "Это поле является обязательным для заполнения.";

    inputBox.append(input);
    inputBox.append(placeholder);
    inputBox.append(errorMessage);

    var editor = document.createElement("div");
    editor.className = "editor";
    editor.setAttribute("contenteditable", "true");
    editor.setAttribute("data-placeholder", "Оставьте отзыв о приложении");

    var textRule = document.createElement("p");
    textRule.className = "textRule";
    textRule.innerHTML = 'Отправляя отзыв, Вы подтверждаете, что ознакомились c <a href="/BusinessCard/ServiceRule" target="_blank">политикой конфиденциальности</a>.';

    var formButton = document.createElement("button");
    formButton.className = "formButton";
    formButton.setAttribute("onclick", "SendReview()");
    formButton.innerHTML = '<p>Отправить</p>';

    var preloader = document.createElement("div");
    preloader.id = "creationReviewSpinner";
    preloader.className = "preloaderReview delete";
    preloader.innerHTML = '<svg class="preloader__imageReview" viewBox="0 0 512 512"><use xlink:href="#preloaderCreationReview"></use></svg>';

    center.innerHTML = '<div class="newRating"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc1" checked=""><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc2"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc3"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc4"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc5"><label for="rc1" class="newItem" onclick="SetRating(1)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">1</span></label><label for="rc2" class="newItem" onclick="SetRating(2)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">2</span></label><label for="rc3" class="newItem" onclick="SetRating(3)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">3</span></label><label for="rc4" class="newItem" onclick="SetRating(4)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">4</span></label><label for="rc5" class="newItem" onclick="SetRating(5)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">5</span></label></div>';
    center.append(inputBox);
    center.append(editor);
    center.append(textRule);
    center.append(formButton);
    center.append(preloader);

    fullWindow.append(skipButton);
    fullWindow.append(center);
    html.append(fullWindow);
}

function CloseReviewCreation() {
    document.getElementById(`reviewCreation`).remove();
}

function SetRating(newRating) {
    rating = newRating;
}

function SendReview() {
    $('#serverError').remove();
    $('.errorMessage').fadeOut();

    var userName = $('.input').val();
    if (userName == null || userName == "" || !userName.trim()) {
        $('.errorMessage').show('slow');
        $('.input').focus();
        return;
    }

    var reviewText = $('.editor')[0].innerText;

    TimerCreationReview();
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonStore/CreateReview",
        data: { projectId, userName, rating, reviewText },
        dataTpye: "json",
        success: function (reviewStatistic) {
            reviewData.userName = userName;
            reviewData.rating = rating;
            reviewData.text = reviewText;
            reviewData.date = "Сегодня";

            $('.ratingInformationBlock').find('.ratingBlock').find('.ratingValueBlock').find('.value').html(reviewStatistic.avgRating);
            $('.ratingInformationBlock').find('.countReviewsBlock').find('.value').html(reviewStatistic.reviewsCount);

            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.informationText').html(reviewStatistic.avgRating);
            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.additionalText').html("Оценок: " + reviewStatistic.reviewsCount);

            $('#fiveStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.excellentCount + "%");
            $('#fourStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.goodCount + "%");
            $('#threeStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.notBadCount + "%");
            $('#twoStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.badCount + "%");
            $('#oneStars').find('.animated-progress').find('span').css('width', reviewStatistic.ratingStatistic.terriblyCount + "%");

            pagesCount = reviewStatistic.reviewsPagesCount;

            $('.reviewBlock').remove();
            SetReviewData(reviewData, 0, false);

            ViewReview();
        },
        error: function (error) {
            var errorMessage = document.createElement("p");
            errorMessage.id = "serverError";
            errorMessage.className = "errorMessage";
            errorMessage.innerHTML = error.responseJSON.ErrorMessage;
            $(errorMessage).css("display", "block");
            $(errorMessage).css("margin-bottom", "0");
            document.querySelector('.center').append(errorMessage);
            ViewReview(false);
            $(document.querySelector('.formButton')).css("display", "flex");
        }
    });
}

function TimerCreationReview() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadReview) {
            isLoadReviewVisible = true;
            var preloader = document.getElementById(`creationReviewSpinner`);
            preloader.classList.remove('delete');

            $(document.querySelector('.formButton')).css("display", "none");
        }
        isLoadReview = false;
    };
};

function ViewReview() {
    isLoadReview = true;

    setTimeout(function (createReview = true) {
        if (isLoadReviewVisible) {
            var preloader = document.getElementById(`creationReviewSpinner`);
            preloader.classList.add('delete');
        }

        if (createReview) {
            $('.newRating').remove();
            $('.inputBox').remove();
            $('.editor').remove();
            $('.textRule').remove();
            $('.formButton').remove();

            var thanksForReview = document.createElement("p");
            thanksForReview.className = "thanksForReview";
            thanksForReview.innerHTML = "Отзыв отправлен. Спасибо большое!";
            document.querySelector('.center').append(thanksForReview);

            window.setTimeout(show_thanksForReview, 128);

            function show_thanksForReview() {
                thanksForReview.classList.add('thanksForReviewView');
            };
        }
    }, 228);
}