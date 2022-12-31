window.onload = init;
var authorizedUser = false;
var channels = [];
var subscribeOnChannel = false;
var subscribeOnMailing = false;
var userTopchiks = [];
var userBookmarks = [];
var topchiksQueue = [];
var bookmarksQueue = [];
var subscriptionOnChannelsQueue = [];
var subscriptionOnMailingsQueue = [];
var typeOfPage = "all";
var currentChannelId = null;
var searchText = "";
var currentPageId = null;
var isLoad = false;
var isLoadVisible = false;
var isLoadData = false;
var isLoadDataVisible = false;

function init() {
    var width = $(window).width();
    if (width <= 1007) {
        $('.filtersBlock').fadeOut();
        $('.filtersMenu').fadeIn('fast');
        $('.filtersMenuSpan').fadeIn('fast');
    }
    else {
        $('.mainBlock').css("margin-bottom", "40px");
        $('.filtersMenu').fadeOut('fast');
        $('.filtersMenuSpan').fadeOut('fast');
        $('.menuFullWindow').fadeOut('fast');
        $('.filtersBlock').fadeIn();
    }

    Timer();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetBlogInformation",
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            authorizedUser = jsonData.AuthorizedUser;
            channels = jsonData.BlogInformation.Channels;

            SetChannelsInFilter();

            SetPosts(jsonData.BlogInformation.Posts, jsonData.BlogInformation.PersonalInformation.StatisticsByPost);

            if (jsonData.BlogInformation.PagesCount > 1)
                SetPagesButtons(jsonData.BlogInformation.PagesCount);

            ViewAllData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetChannelsInFilter() {
    var channelsFullWindow = document.getElementById('channelsFullWindow');
    var channelsNotFullWindow = document.getElementById('channelsNotFullWindow');

    for (var i = 0; i < channels.length; i++) {
        var channelBlockFullWindow = document.createElement("div");
        channelBlockFullWindow.className = "channelBlockFullWindow";

        var channelBlock = document.createElement("div");
        channelBlock.className = "channelBlock";
        channelBlock.setAttribute('onclick', 'OpenChannel(' + channels[i].Id + ')');

        var channelColorFullWindow = document.createElement("div");
        channelColorFullWindow.className = "channelColor";
        $(channelColorFullWindow).css("background", channels[i].Color);

        var channelColor = document.createElement("div");
        channelColor.className = "channelColor";
        $(channelColor).css("background", channels[i].Color);

        var channelNameFullWindow = document.createElement("p");
        channelNameFullWindow.className = "channelName channelNameInFilters";
        channelNameFullWindow.innerHTML = channels[i].Name;

        var channelName = document.createElement("p");
        channelName.className = "channelName channelNameInFilters";
        channelName.innerHTML = channels[i].Name;

        channelBlockFullWindow.append(channelColorFullWindow);
        channelBlockFullWindow.append(channelNameFullWindow);

        channelBlock.append(channelColor);
        channelBlock.append(channelName);

        channelsFullWindow.append(channelBlockFullWindow);
        channelsNotFullWindow.append(channelBlock);
    }
}

function SetPosts(posts, personalInformation, isPrepend = false, isNone = false) {
    var postBlock = "";
    if (isPrepend)
        postsBlock = document.querySelector('.pagesButtonsBlock');
    else
        postsBlock = document.getElementById('posts');

    for (var i = 0; i < posts.length; i++) {
        var postBlock = document.createElement("div");
        postBlock.className = "postBlock";
        if (isNone)
            $('.postBlock').css('display', 'none');

        var firstSection = document.createElement("div");
        firstSection.className = "firstSection";

        var channelNameInPost = document.createElement("p");
        channelNameInPost.href = '#' + posts[i].ChannelName;
        channelNameInPost.className = "channelName channelNameInPost";
        channelNameInPost.innerHTML = posts[i].ChannelName;
        channelNameInPost.setAttribute('onclick', 'OpenChannel(' + posts[i].ChannelId + ')');

        var date = document.createElement("p");
        date.className = "date";
        date.innerHTML = posts[i].Date;

        var viewsCountBlock = document.createElement("div");
        viewsCountBlock.className = "viewsCountBlock";
        viewsCountBlock.innerHTML = '<svg viewBox="0 0 24 24" class="statisticIcon"><use xlink:href="#views"></use></svg>';

        var viewsValue = document.createElement("p");
        viewsValue.className = "viewsValue";
        viewsValue.innerHTML = posts[i].ViewsCount;

        viewsCountBlock.append(viewsValue);

        firstSection.append(channelNameInPost);
        firstSection.append(date);
        firstSection.append(viewsCountBlock);

        var secondSection = document.createElement("div");
        secondSection.className = "secondSection";
        secondSection.setAttribute("onclick", "OpenPost('" + posts[i].Key + "')")

        var postDescription = document.createElement("p");
        postDescription.className = "postDescription";
        postDescription.innerHTML = posts[i].Description;

        var postHeaderBlock = document.createElement("div");
        postHeaderBlock.className = "postHeaderBlock";

        if (posts[i].HeaderImageUrl != null && posts[i].HeaderImageUrl != "") {
            var postHeaderImage = document.createElement("div");
            postHeaderImage.className = "postHeaderImage";
            $(postHeaderImage).css('background-image', 'url(' + posts[i].HeaderImageUrl + ')');

            var postHeaderBackColor = document.createElement("div");
            postHeaderBackColor.className = "postHeaderBackColor backColorWithImage";

            var postHeaderNameBlock = document.createElement("div");
            postHeaderNameBlock.className = "postHeaderNameBlock";

            var postHeaderName = document.createElement("p");
            postHeaderName.className = "postHeaderName postHeaderNameWithImage";
            postHeaderName.innerHTML = posts[i].Name;

            postHeaderNameBlock.append(postHeaderName);

            postHeaderBlock.append(postHeaderImage);
            postHeaderBlock.append(postHeaderBackColor);
            postHeaderBlock.append(postHeaderNameBlock);
        }
        else {
            var postHeaderBackColor = document.createElement("div");
            postHeaderBackColor.className = "postHeaderBackColor";

            var postHeaderNameBlock = document.createElement("div");
            postHeaderNameBlock.className = "postHeaderNameBlock";

            var postHeaderName = document.createElement("p");
            postHeaderName.className = "postHeaderName postHeaderNameWithoutImage";
            postHeaderName.innerHTML = posts[i].Name;

            postHeaderNameBlock.append(postHeaderName);

            postHeaderBlock.append(postHeaderBackColor);
            postHeaderBlock.append(postHeaderNameBlock);
        }

        secondSection.append(postDescription);
        secondSection.append(postHeaderBlock);

        var thirdSection = document.createElement("div");
        thirdSection.className = "thirdSection";

        var topchikBlock = document.createElement("div");
        topchikBlock.id = "topchik" + i;
        topchikBlock.className = "topchikBlock";
        if (personalInformation[posts[i].Id]['Topchiks'])
            topchikBlock.classList.add("topchikUser");
        else
            topchikBlock.classList.add("untopchikUser");
        userTopchiks.push(personalInformation[posts[i].Id]['Topchiks']);
        topchikBlock.setAttribute('onclick', 'SendTopchik(' + i + ', ' + posts[i].Id + ')');
        topchikBlock.innerHTML = '<svg viewBox="0 0 64 64" class="topchikIcon"><use xlink:href="#topchik"></use></svg>';

        var statisticValueTopchik = document.createElement("p");
        statisticValueTopchik.id = "topchikValue" + i;
        statisticValueTopchik.className = "statisticValue";
        statisticValueTopchik.innerHTML = posts[i].TopchiksCount;

        if (posts[i].TopchiksCount == 0) {
            topchikBlock.classList.add("noTopchik");
            statisticValueTopchik.classList.add("noValue");
        }
        
        topchikBlock.append(statisticValueTopchik);

        var bookmarksBlock = document.createElement("div");
        bookmarksBlock.className = "bookmarksBlock";
        bookmarksBlock.setAttribute('onclick', 'InBookmarks(' + i + ', ' + posts[i].Id + ')');
        userBookmarks.push(personalInformation[posts[i].Id]['Bookmarks']);
        if (userBookmarks[i])
            bookmarksBlock.innerHTML = '<svg id="bookmarks' + i + '" viewBox="0 0 24 24" class="bookmarksIcon needBookmarks"><use xlink:href="#bookmarks"></use></svg>';
        else
            bookmarksBlock.innerHTML = '<svg id="bookmarks' + i + '" viewBox="0 0 24 24" class="bookmarksIcon notBookmarks"><use xlink:href="#bookmarks"></use></svg>';

        var statisticValueBookmark = document.createElement("p");
        statisticValueBookmark.id = "bookmarkValue" + i;
        statisticValueBookmark.className = "statisticValue";
        statisticValueBookmark.innerHTML = posts[i].BookmarksCount;

        if (posts[i].BookmarksCount == 0)
            statisticValueBookmark.classList.add("noValue");

        if (userBookmarks[i])
            statisticValueBookmark.classList.add("bookmarksValue");

        bookmarksBlock.append(statisticValueBookmark);

        var commentsBlock = document.createElement("div");
        commentsBlock.className = "commentsBlock";
        commentsBlock.innerHTML = '<svg viewBox="0 0 24 24" class="commentsIcon"><use xlink:href="#comments"></use></svg>';

        var statisticValueComment = document.createElement("p");
        statisticValueComment.id = "commentValue" + i;
        statisticValueComment.innerHTML = posts[i].CommentsCount;

        if (posts[i].CommentsCount == 0)
            statisticValueComment.classList.add("noValue");
        else if (posts[i].CommentsCount > 0)
            statisticValueComment.classList.add("statisticValue");

        commentsBlock.append(statisticValueComment);

        thirdSection.append(topchikBlock);
        thirdSection.append(bookmarksBlock);
        thirdSection.append(commentsBlock);

        postBlock.append(firstSection);
        postBlock.append(secondSection);
        postBlock.append(thirdSection);

        if (isPrepend)
            postsBlock.before(postBlock);
        else
            postsBlock.append(postBlock);
    }
}

function OpenSubscriptions() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (typeOfPage == "onChannels")
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = "";
    $('#search').val("");

    $('.postBlock').remove();
    $('.informationBlock').remove();
    $('.aboutChannelBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();

    typeOfPage = "onChannels";

    currentChannelId = null;

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetSubscriptionOnChannels",
        dataTpye: "json",
        success: function (data) {
            var informationBlock = document.createElement("div");
            informationBlock.className = "informationBlock";
            $(informationBlock).css('background', 'white');
            $(informationBlock).css('display', 'none');

            var serviceNameInHeader = document.createElement("p");
            serviceNameInHeader.className = "channelName channelNameInHeader";
            serviceNameInHeader.innerHTML = 'Подписки';

            var separator = document.createElement("div");
            separator.className = "separator";

            var aboutService = document.createElement("p");
            aboutService.className = "aboutChannel";
            aboutService.innerHTML = 'Подписывайтесь на интересные для Вас каналы, чтобы быстрее находить нужные посты.';
            $(aboutService).css('text-align', 'center');

            informationBlock.append(serviceNameInHeader);
            informationBlock.append(separator);
            informationBlock.append(aboutService);

            document.getElementById('posts').append(informationBlock);

            var jsonData = $.parseJSON(data);
            subscriptions = jsonData.Subscriptions;

            var subscriptionsBlock = document.createElement("div");
            subscriptionsBlock.className = "subscriptionsBlock";
            $(subscriptionsBlock).css('display', 'none');

            for (var id in subscriptions) {
                var channelSubscriptionBlock = document.createElement("div");
                channelSubscriptionBlock.id = "onChannel" + id;
                channelSubscriptionBlock.className = "channelSubscriptionBlock";

                var channelWrapper = document.createElement("div");
                channelWrapper.className = "channelWrapper";
                channelWrapper.setAttribute('onclick', 'OpenChannel(' + subscriptions[id].Id + ')');

                var channelColor = document.createElement("div");
                channelColor.className = "channelColor";
                $(channelColor).css('background', subscriptions[id].Color);

                var channelNameInFilters = document.createElement("p");
                channelNameInFilters.className = "channelName channelNameInFilters";
                channelNameInFilters.innerHTML = subscriptions[id].Name;

                var removeSubscription = document.createElement("p");
                removeSubscription.className = "removeSubscription";
                removeSubscription.innerHTML = "✖";
                removeSubscription.setAttribute('onclick', 'DeleteSubscribeOnChannel(' + subscriptions[id].Id + ',' + id + ')');

                channelWrapper.append(channelColor);
                channelWrapper.append(channelNameInFilters);
                channelSubscriptionBlock.append(channelWrapper);
                channelSubscriptionBlock.append(removeSubscription);

                subscriptionsBlock.append(channelSubscriptionBlock);
            }

            document.getElementById('posts').append(subscriptionsBlock);

            var classesView = [];
            classesView.push(".informationBlock");
            classesView.push(".subscriptionsBlock");

            ViewData(classesView);
        },
        error: function (error) {

        }
    });
}

function OpenMailings() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (typeOfPage == "onMailings")
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = "";
    $('#search').val("");

    $('.postBlock').remove();
    $('.informationBlock').remove();
    $('.aboutChannelBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();

    typeOfPage = "onMailings"

    currentChannelId = null;

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetSubscriptionOnMailings",
        dataTpye: "json",
        success: function (data) {
            var informationBlock = document.createElement("div");
            informationBlock.className = "informationBlock";
            $(informationBlock).css('background', 'white');
            $(informationBlock).css('display', 'none');

            var serviceNameInHeader = document.createElement("p");
            serviceNameInHeader.className = "channelName channelNameInHeader";
            serviceNameInHeader.innerHTML = 'Рассылки';

            var separator = document.createElement("div");
            separator.className = "separator";

            var aboutService = document.createElement("p");
            aboutService.className = "aboutChannel";
            aboutService.innerHTML = 'Подписывайтесь на рассылки, чтобы получать новости о публикациях на почту.';
            $(aboutService).css('text-align', 'center');

            informationBlock.append(serviceNameInHeader);
            informationBlock.append(separator);
            informationBlock.append(aboutService);

            document.getElementById('posts').append(informationBlock);

            var jsonData = $.parseJSON(data);
            subscriptions = jsonData.Subscriptions;

            var subscriptionsBlock = document.createElement("div");
            subscriptionsBlock.className = "subscriptionsBlock";
            $(subscriptionsBlock).css('display', 'none');

            for (var id in subscriptions) {
                var channelSubscriptionBlock = document.createElement("div");
                channelSubscriptionBlock.id = "onMailing" + id;
                channelSubscriptionBlock.className = "channelSubscriptionBlock";

                var channelWrapper = document.createElement("div");
                channelWrapper.className = "channelWrapper";
                channelWrapper.setAttribute('onclick', 'OpenChannel(' + subscriptions[id].Id + ')');

                var channelColor = document.createElement("div");
                channelColor.className = "channelColor";
                $(channelColor).css('background', subscriptions[id].Color);

                var channelNameInFilters = document.createElement("p");
                channelNameInFilters.className = "channelName channelNameInFilters";
                channelNameInFilters.innerHTML = subscriptions[id].Name;

                var removeSubscription = document.createElement("p");
                removeSubscription.className = "removeSubscription";
                removeSubscription.innerHTML = "✖";
                removeSubscription.setAttribute('onclick', 'DeleteSubscribeOnMailing(' + subscriptions[id].Id + ',' + id + ')');

                channelWrapper.append(channelColor);
                channelWrapper.append(channelNameInFilters);
                channelSubscriptionBlock.append(channelWrapper);
                channelSubscriptionBlock.append(removeSubscription);

                subscriptionsBlock.append(channelSubscriptionBlock);
            }

            document.getElementById('posts').append(subscriptionsBlock);

            var classesView = [];
            classesView.push(".informationBlock");
            classesView.push(".subscriptionsBlock");

            ViewData(classesView);
        },
        error: function (error) {

        }
    });
}

function OpenBookmarks() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (typeOfPage == "bookmark")
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = "";
    $('#search').val("");

    typeOfPage = "bookmark";

    $('.postBlock').remove();
    $('.informationBlock').remove();
    $('.aboutChannelBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();

    currentChannelId = null;

    var requestParameters = GetRequestParameters(1, "", true);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetPosts?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var informationBlock = document.createElement("div");
            informationBlock.className = "informationBlock";
            $(informationBlock).css('background', 'white');
            $(informationBlock).css('display', 'none');

            var serviceNameInHeader = document.createElement("p");
            serviceNameInHeader.className = "channelName channelNameInHeader";
            serviceNameInHeader.innerHTML = 'Закладки';

            var separator = document.createElement("div");
            separator.className = "separator";

            var aboutService = document.createElement("p");
            aboutService.className = "aboutChannel";
            aboutService.innerHTML = 'Сохраняйте полезные для Вас посты в закладки для более быстрого доступа к ним.';
            $(aboutService).css('text-align', 'center');

            informationBlock.append(serviceNameInHeader);
            informationBlock.append(separator);
            informationBlock.append(aboutService);

            document.getElementById('dataSpinner').before(informationBlock);

            var jsonData = $.parseJSON(data);
            bookmarks = jsonData.PostsInformation;

            SetPosts(bookmarks.Posts, bookmarks.PersonalInformation.StatisticsByPost, false, true);

            if (bookmarks.PagesCount > 1)
                SetPagesButtons(bookmarks.PagesCount, true);

            var classesView = [];
            classesView.push(".informationBlock");
            classesView.push(".postBlock");
            if (bookmarks.PagesCount > 1)
                classesView.push(".pagesButtonsBlock");

            ViewData(classesView);
        },
        error: function (error) {

        }
    });
}

function GetAllChannels() {
    if (typeOfPage == "allChannels")
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = "";
    $('#search').val("");

    $('.postBlock').remove();
    $('.informationBlock').remove();
    $('.aboutChannelBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();

    typeOfPage = "allChannels";

    currentChannelId = null;

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetChannels",
        dataTpye: "json",
        success: function (data) {
            var informationBlock = document.createElement("div");
            informationBlock.className = "informationBlock";
            $(informationBlock).css('background', 'white');
            $(informationBlock).css('display', 'none');

            var serviceNameInHeader = document.createElement("p");
            serviceNameInHeader.className = "channelName channelNameInHeader";
            serviceNameInHeader.innerHTML = 'Каналы';

            var separator = document.createElement("div");
            separator.className = "separator";

            var aboutService = document.createElement("p");
            aboutService.className = "aboutChannel";
            aboutService.innerHTML = 'По каналам сможете выбрать интересующие тематики и быстрее найти нужные посты.';
            $(aboutService).css('text-align', 'center');

            informationBlock.append(serviceNameInHeader);
            informationBlock.append(separator);
            informationBlock.append(aboutService);

            document.getElementById('posts').append(informationBlock);

            var jsonData = $.parseJSON(data);
            channelsInformation = jsonData.ChannelsInformation;

            var subscriptionsBlock = document.createElement("div");
            subscriptionsBlock.className = "subscriptionsBlock";
            $(subscriptionsBlock).css('display', 'none');

            for (var index in channelsInformation) {
                var channelSubscriptionBlock = document.createElement("div");
                channelSubscriptionBlock.className = "channelSubscriptionBlock";
                $(channelSubscriptionBlock).css('cursor', 'pointer');
                channelSubscriptionBlock.setAttribute('onclick', 'OpenChannel(' + channelsInformation[index].Id + ')');

                var channelColor = document.createElement("div");
                channelColor.className = "channelColor";
                $(channelColor).css('background', channelsInformation[index].Color);
                $(channelColor).css('margin', '0 12px 0 0');

                var channelNameInFilters = document.createElement("p");
                channelNameInFilters.className = "channelName channelNameInFilters";
                channelNameInFilters.innerHTML = channelsInformation[index].Name;

                channelSubscriptionBlock.append(channelColor);
                channelSubscriptionBlock.append(channelNameInFilters);

                subscriptionsBlock.append(channelSubscriptionBlock);
            }

            document.getElementById('posts').append(subscriptionsBlock);

            var classesView = [];
            classesView.push(".informationBlock");
            classesView.push(".subscriptionsBlock");

            ViewData(classesView);
        },
        error: function (error) {

        }
    });
}

function OpenChannel(channelId) {
    if (typeOfPage == "channel" && currentChannelId == channelId)
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = "";
    $('#search').val("");

    typeOfPage = "channel";

    $('.postBlock').remove();
    $('.informationBlock').remove();
    $('.aboutChannelBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();

    currentChannelId = channelId;

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetChannelInformation",
        data: { channelId },
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            channelInformation = jsonData.ChannelInformation;

            var informationBlock = document.createElement("div");
            informationBlock.className = "informationBlock";
            $(informationBlock).css('background-image', ' linear-gradient(' + channelInformation.Channel.Color + ', white 67%)');
            $(informationBlock).css('display', 'none');

            var channelNameInHeader = document.createElement("p");
            channelNameInHeader.className = "channelName channelNameInHeader";
            channelNameInHeader.innerHTML = channelInformation.Channel.Name;

            var subscriptionsVariantBlock = document.createElement("div");
            subscriptionsVariantBlock.className = "subscriptionsVariantBlock";

            var subscriptionChannel = document.createElement("button");
            subscriptionChannel.setAttribute('onclick', 'SubscribeOnChannel()');
            if (channelInformation.PersonalInformation.SubscriptionsDictionary['OnChannel']) {
                subscriptionChannel.className = "subscriptionButton subscriptionChannel subscribeButton";
                subscribeOnChannel = true;
            }
            else {
                subscriptionChannel.className = "subscriptionButton subscriptionChannel notSubscribeOnChannelButton";
                subscribeOnChannel = false;
            }

            var subscriptionMailing = document.createElement("button");
            subscriptionMailing.setAttribute('onclick', 'SubscribeOnMailing()');
            if (channelInformation.PersonalInformation.SubscriptionsDictionary['OnMailing']) {
                subscriptionMailing.className = "subscriptionButton subscriptionMailing subscribeButton";
                subscribeOnMailing = true;
            }
            else {
                subscriptionMailing.className = "subscriptionButton subscriptionMailing notSubscribeOnMailingButton";
                subscribeOnMailing = false;
            }

            subscriptionsVariantBlock.append(subscriptionChannel);
            subscriptionsVariantBlock.append(subscriptionMailing);

            var separator = document.createElement("div");
            separator.className = "separator";

            var menuBlock = document.createElement("div");
            menuBlock.className = "menuBlock";

            var menuValueBlog = document.createElement("p");
            menuValueBlog.id = "blogMenu";
            menuValueBlog.className = "menuValue active";
            menuValueBlog.innerHTML = 'Блог';
            menuValueBlog.setAttribute('onclick', 'OpenBlog()');

            var menuValueAboutChannel = document.createElement("p");
            menuValueAboutChannel.id = "aboutChannelMenu";
            menuValueAboutChannel.className = "menuValue";
            menuValueAboutChannel.innerHTML = 'О канале';
            menuValueAboutChannel.setAttribute('onclick', 'OpenAboutChannel()');

            menuBlock.append(menuValueBlog);
            menuBlock.append(menuValueAboutChannel);

            informationBlock.append(channelNameInHeader);
            informationBlock.append(subscriptionsVariantBlock);
            informationBlock.append(separator);
            informationBlock.append(menuBlock);

            document.getElementById('dataSpinner').before(informationBlock);

            var aboutChannelBlock = document.createElement("div");
            aboutChannelBlock.className = "aboutChannelBlock";
            $(aboutChannelBlock).css('display', 'none');

            var aboutChannel = document.createElement("p");
            aboutChannel.className = "aboutChannel";
            aboutChannel.innerHTML = channelInformation.Channel.Description;

            aboutChannelBlock.append(aboutChannel);

            document.getElementById('posts').append(aboutChannelBlock);

            SetPosts(channelInformation.Posts, channelInformation.PersonalInformation.StatisticsByPost, false, true);

            if (channelInformation.PagesCount > 1)
                SetPagesButtons(channelInformation.PagesCount, true);

            var classesView = [];
            classesView.push(".informationBlock");
            classesView.push(".postBlock");
            if (channelInformation.PagesCount > 1)
                classesView.push(".pagesButtonsBlock");

            ViewData(classesView);
        },
        error: function (error) {

        }
    });
}

function OpenBlog() {
    document.getElementById('aboutChannelMenu').classList.remove('active');
    $('.aboutChannelBlock').fadeOut();
    document.getElementById('blogMenu').classList.add('active');
    $('.postBlock').fadeIn();
    $('.pagesButtonsBlock').fadeIn();
}

function OpenAboutChannel() {
    document.getElementById('blogMenu').classList.remove('active');
    $('.postBlock').fadeOut();
    $('.pagesButtonsBlock').fadeOut();
    document.getElementById('aboutChannelMenu').classList.add('active');
    $('.aboutChannelBlock').fadeIn();
}

$(window).resize(function () {
    var width = $(window).width();
    if (width <= 1007) {
        $('.filtersBlock').fadeOut();
        $('.filtersMenu').fadeIn('fast');
        $('.filtersMenuSpan').fadeIn('fast');
    }
    else {
        $('.mainBlock').css("margin-bottom", "40px");
        $('.filtersMenu').fadeOut('fast');
        $('.filtersMenuSpan').fadeOut('fast');
        $('.menuFullWindow').fadeOut('fast');
        $('.filtersBlock').fadeIn();
    }
});

function OpenMenu() {
    $('.mainBlock').css("margin-bottom", "12px");
    $('.menuFullWindow').fadeIn('fast');
}

function CloseMenu() {
    $('.menuFullWindow').fadeOut('fast');
    $('.mainBlock').css("margin-bottom", "40px");
}

function SendTopchik(postNumber, postId) {
    if (!authorizedUser) {
        OpenInformationBlock("topchik");
        return;
    }

    if (userTopchiks[postNumber]) {
        if (topchiksQueue[postId] == undefined) {
            topchiksQueue[postId] = [];
            topchiksQueue[postId].push(false);
            SendTopchikToServer(postId, false);
        }
        else {
            SetTopchikCondition(postId, false);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue' + postNumber).innerHTML) - 1;
        $("#topchikValue" + postNumber).html(Math.round(newValue));
        if (newValue == 0) {
            document.getElementById('topchik' + postNumber).classList.add("noTopchik");
            document.getElementById('topchikValue' + postNumber).classList.add("noValue");
        }
        document.getElementById('topchik' + postNumber).classList.remove("topchikUser");
        document.getElementById('topchik' + postNumber).classList.add("untopchikUser");
        userTopchiks[postNumber] = false;
    }
    else {
        if (topchiksQueue[postId] == undefined) {
            topchiksQueue[postId] = [];
            topchiksQueue[postId].push(true);
            SendTopchikToServer(postId, true);
        }
        else {
            SetTopchikCondition(postId, true);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue' + postNumber).innerHTML) + 1;
        $("#topchikValue" + postNumber).html(Math.round(newValue));
        if (newValue == 1) {
            document.getElementById('topchik' + postNumber).classList.remove("noTopchik");
            document.getElementById('topchikValue' + postNumber).classList.remove("noValue");
        }
        document.getElementById('topchik' + postNumber).classList.remove("untopchikUser");
        document.getElementById('topchik' + postNumber).classList.add("topchikUser");
        userTopchiks[postNumber] = true;
    }
}

function SendTopchikToServer(postId, isTopchik) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/SendTopchik",
        data: { postId, isTopchik },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (topchiksQueue[postId].length == 1) {
                    delete topchiksQueue[postId];
                }
                else {
                    topchiksQueue[postId].splice(topchiksQueue[postId].indexOf(isTopchik), 1);
                    SendTopchikToServer(postId, topchiksQueue[postId][0]);
                }
            }

        },
        error: function (error) {
            
        }
    });
}

function SetTopchikCondition(postId, condition) {
    if (topchiksQueue[postId].length == 1 && topchiksQueue[postId][0] != condition)
        topchiksQueue[postId].push(condition);
    else if (topchiksQueue[postId].length == 2 && topchiksQueue[postId][1] != condition)
        topchiksQueue[postId].pop();
}

function InBookmarks(postNumber, postId) {
    if (!authorizedUser) {
        OpenInformationBlock("bookmarks");
        return;
    }

    if (userBookmarks[postNumber]) {
        if (bookmarksQueue[postId] == undefined) {
            bookmarksQueue[postId] = [];
            bookmarksQueue[postId].push(false);
            InBookmarksToServer(postId, false);
        }
        else {
            SetBookmarkCondition(postId, false);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarkValue' + postNumber).innerHTML) - 1;
        $("#bookmarkValue" + postNumber).html(Math.round(newValue));
        if (newValue == 0)
            document.getElementById('bookmarkValue' + postNumber).classList.add("noValue");
        document.getElementById('bookmarks' + postNumber).classList.remove("needBookmarks");
        document.getElementById('bookmarks' + postNumber).classList.add("notBookmarks");
        document.getElementById('bookmarkValue' + postNumber).classList.remove("bookmarksValue");
        userBookmarks[postNumber] = false;
    }
    else {
        if (bookmarksQueue[postId] == undefined) {
            bookmarksQueue[postId] = [];
            bookmarksQueue[postId].push(true);
            InBookmarksToServer(postId, true);
        }
        else {
            SetBookmarkCondition(postId, true);
        }

        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarkValue' + postNumber).innerHTML) + 1;
        $("#bookmarkValue" + postNumber).html(Math.round(newValue));
        if (newValue == 1)
            document.getElementById('bookmarkValue' + postNumber).classList.remove("noValue");
        document.getElementById('bookmarks' + postNumber).classList.remove("notBookmarks");
        document.getElementById('bookmarks' + postNumber).classList.add("needBookmarks");
        document.getElementById('bookmarkValue' + postNumber).classList.add("bookmarksValue");
        userBookmarks[postNumber] = true;
    }
}

function InBookmarksToServer(postId, inBookmark) {
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/SendPostInBookmark",
        data: { postId, inBookmark },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (bookmarksQueue[postId].length == 1) {
                    delete bookmarksQueue[postId];
                }
                else {
                    bookmarksQueue[postId].splice(bookmarksQueue[postId].indexOf(inBookmark), 1);
                    SendTopchikToServer(postId, bookmarksQueue[postId][0]);
                }
            }

        },
        error: function (error) {

        }
    });
}

function SetBookmarkCondition(postId, condition) {
    if (bookmarksQueue[postId].length == 1 && bookmarksQueue[postId][0] != condition)
        bookmarksQueue[postId].push(condition);
    else if (bookmarksQueue[postId].length == 2 && bookmarksQueue[postId][1] != condition)
        bookmarksQueue[postId].pop();
}

function SubscribeOnChannel() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (subscribeOnChannel) {
        if (subscriptionOnChannelsQueue[currentChannelId] == undefined) {
            subscriptionOnChannelsQueue[currentChannelId] = [];
            subscriptionOnChannelsQueue[currentChannelId].push(false);
            SubscribeOnChannelToServer(currentChannelId, false);
        }
        else {
            SetSubscriptionChannelCondition(currentChannelId, false);
        }

        document.querySelector('.subscriptionChannel').classList.remove("subscribeButton");
        document.querySelector('.subscriptionChannel').classList.add("notSubscribeOnChannelButton");
        subscribeOnChannel = false;
    }
    else {
        if (subscriptionOnChannelsQueue[currentChannelId] == undefined) {
            subscriptionOnChannelsQueue[currentChannelId] = [];
            subscriptionOnChannelsQueue[currentChannelId].push(true);
            SubscribeOnChannelToServer(currentChannelId, true);
        }
        else {
            SetSubscriptionChannelCondition(currentChannelId, true);
        }

        document.querySelector('.subscriptionChannel').classList.remove("notSubscribeOnChannelButton");
        document.querySelector('.subscriptionChannel').classList.add("subscribeButton");
        subscribeOnChannel = true;
    }
}

function SubscribeOnChannelToServer(channelId, isSubscribe) {
    var subscribeType = "channels";

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/Subscribe",
        data: { channelId, isSubscribe, subscribeType },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (subscriptionOnChannelsQueue[channelId].length == 1) {
                    delete subscriptionOnChannelsQueue[channelId];
                }
                else {
                    subscriptionOnChannelsQueue[channelId].splice(subscriptionOnChannelsQueue[channelId].indexOf(isSubscribe), 1);
                    SubscribeOnChannelToServer(channelId, subscriptionOnChannelsQueue[channelId][0]);
                }
            }

        },
        error: function (error) {

        }
    });
}

function SetSubscriptionChannelCondition(channelId, condition) {
    if (subscriptionOnChannelsQueue[channelId].length == 1 && subscriptionOnChannelsQueue[channelId][0] != condition)
        subscriptionOnChannelsQueue[channelId].push(condition);
    else if (subscriptionOnChannelsQueue[channelId].length == 2 && subscriptionOnChannelsQueue[channelId][1] != condition)
        subscriptionOnChannelsQueue[channelId].pop();
}

function SubscribeOnMailing() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (subscribeOnMailing) {
        if (subscriptionOnMailingsQueue[currentChannelId] == undefined) {
            subscriptionOnMailingsQueue[currentChannelId] = [];
            subscriptionOnMailingsQueue[currentChannelId].push(false);
            SubscribeOnMailingToServer(currentChannelId, false);
        }
        else {
            SetSubscriptionMailingCondition(currentChannelId, false);
        }

        document.querySelector('.subscriptionMailing').classList.remove("subscribeButton");
        document.querySelector('.subscriptionMailing').classList.add("notSubscribeOnMailingButton");
        subscribeOnMailing = false;
    }
    else {
        if (subscriptionOnMailingsQueue[currentChannelId] == undefined) {
            subscriptionOnMailingsQueue[currentChannelId] = [];
            subscriptionOnMailingsQueue[currentChannelId].push(true);
            SubscribeOnMailingToServer(currentChannelId, true);
        }
        else {
            SetSubscriptionMailingCondition(currentChannelId, true);
        }

        document.querySelector('.subscriptionMailing').classList.remove("notSubscribeOnMailingButton");
        document.querySelector('.subscriptionMailing').classList.add("subscribeButton");
        subscribeOnMailing = true;
    }
}

function SubscribeOnMailingToServer(channelId, isSubscribe) {
    var subscribeType = "mailings";

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonBlog/Subscribe",
        data: { channelId, isSubscribe, subscribeType },
        dataTpye: "json",
        success: function (result) {
            if (result) {
                if (subscriptionOnMailingsQueue[channelId].length == 1) {
                    delete subscriptionOnMailingsQueue[channelId];
                }
                else {
                    subscriptionOnMailingsQueue[channelId].splice(subscriptionOnMailingsQueue[channelId].indexOf(isSubscribe), 1);
                    SubscribeOnMailingToServer(channelId, subscriptionOnMailingsQueue[channelId][0]);
                }
            }

        },
        error: function (error) {

        }
    });
}

function SetSubscriptionMailingCondition(channelId, condition) {
    if (subscriptionOnMailingsQueue[channelId].length == 1 && subscriptionOnMailingsQueue[channelId][0] != condition)
        subscriptionOnMailingsQueue[channelId].push(condition);
    else if (subscriptionOnMailingsQueue[channelId].length == 2 && subscriptionOnMailingsQueue[channelId][1] != condition)
        subscriptionOnMailingsQueue[channelId].pop();
}

function DeleteSubscribeOnChannel(channelId, id) {
    SubscribeOnChannelToServer(channelId, false);
    $('#onChannel' + id).fadeOut();
    //$('#onChannel' + id).remove();
}

function DeleteSubscribeOnMailing(channelId, id) {
    SubscribeOnMailingToServer(channelId, false);
    $('#onMailing' + id).fadeOut();
    $('#onMailing' + id).remove();
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
        requirement.innerHTML = '<a href="/Account/Login">Войдите</a> на платформу MAXon28';

        var requirementDescription = document.createElement("p");
        requirementDescription.className = "requirementDescription";
        requirementDescription.innerHTML = 'Вы сможете подписываться на каналы, на их рассылки и сохранять статьи';

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

$(document).ready(function () {
    $('#search').keydown(function (e) {
        if (e.keyCode === 13)
            SearchPosts();
    });
});

function SearchPosts() {
    if ($('#search').val() == searchText)
        return;

    userTopchiks = [];
    userBookmarks = [];

    searchText = $('#search').val();

    currentPageId = "page1";

    /*isLoad = false;
    Timer();*/

    $('.postBlock').remove();
    $('.subscriptionsBlock').remove();
    $('.pagesButtonsBlock').remove();
    if (typeOfPage == "onChannels" || typeOfPage == "onMailings" || typeOfPage == "allChannels") {
        typeOfPage = "all";
        $('.informationBlock').remove();
        $('.aboutChannelBlock').remove();
    }

    if (typeOfPage == "channel") {
        document.getElementById('aboutChannelMenu').classList.remove('active');
        $('.aboutChannelBlock').fadeOut();
        document.getElementById('blogMenu').classList.add('active');
    }

    //$('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

    var requestParameters = GetRequestParameters(1, searchText, true);

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetPosts?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            var jsonData = $.parseJSON(data);
            postsInformation = jsonData.PostsInformation;

            if (postsInformation.Posts.length > 0) {
                SetPosts(postsInformation.Posts, postsInformation.PersonalInformation.StatisticsByPost, false, true);

                if (postsInformation.PagesCount > 1)
                    SetPagesButtons(postsInformation.PagesCount, true);
            }

            var classesView = [];
            classesView.push(".postBlock");
            if (postsInformation.PagesCount > 1)
                classesView.push(".pagesButtonsBlock");

            ViewData(classesView);
        },
    });
}

function SetPagesButtons(pagesCount, isNone = false) {
    currentPageId = "page1";

    var posts = document.getElementById(`posts`);

    var pagesButtonsBlock = document.createElement("div");
    pagesButtonsBlock.className = "pagesButtonsBlock";
    if (isNone)
        $(pagesButtonsBlock).css('display', 'none');

    pageNumber = 1;
    while (pageNumber <= pagesCount) {
        var pageButton = document.createElement("p");
        if (pageNumber == 1) {
            pageButton.className = "pageButton activeButton";
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

    posts.append(pagesButtonsBlock);
}

function OpenNewPage(pageButton) {
    if (pageButton.id == currentPageId)
        return;

    userTopchiks = [];
    userBookmarks = [];

    var currentPageButton = document.getElementById(currentPageId);
    currentPageButton.classList.remove("activeButton");
    pageButton.classList.add("activeButton");
    currentPageId = pageButton.id;

    //isLoad = false;
    //Timer();

    //$('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

    var pageNumber = $(pageButton).val();

    $('.postBlock').remove();
    var requestParameters = GetRequestParameters(pageNumber, searchText, false);

    TimerData();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetPosts?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            var jsonData = $.parseJSON(data);

            SetPosts(jsonData.PostsInformation.Posts, jsonData.PostsInformation.PersonalInformation.StatisticsByPost, true, true);

            var classesView = [];
            classesView.push(".postBlock");

            ViewData(classesView);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GetRequestParameters(pageNumber, searchText, needUpdatePagesCount) {
    var requestParameters = "";

    if (currentChannelId != null)
        requestParameters += "ChannelId=" + currentChannelId;

    if (requestParameters == "")
        requestParameters += "PostsPackageNumber=" + pageNumber;
    else
        requestParameters += "&PostsPackageNumber=" + pageNumber;

    if (searchText.length > 0)
        requestParameters += "&SearchText=" + searchText;

    requestParameters += "&TypeOfRequest=" + typeOfPage;

    requestParameters += "&NeedPagesCount=" + needUpdatePagesCount;

    return requestParameters;
}

function Timer() {
    isLoad = false;
    isLoadVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            var preloader = document.getElementById(`allDataSpinner`);
            $(preloader).fadeIn();
        }
        isLoad = false;
    };
};

function ViewAllData() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.getElementById(`allDataSpinner`);
            $(preloader).fadeOut();
        }

        $('.mainBlock').fadeIn();

        $('html, body').animate({ scrollTop: $("#posts").offset().top }, 228);
    }, 228);
}

function TimerData() {
    isLoadData = false;
    isLoadDataVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadData) {
            isLoadDataVisible = true;
            var preloader = document.getElementById(`dataSpinner`);
            $(preloader).fadeIn();
        }
        isLoadData = false;
    };
};

function ViewData(classesView) {
    isLoadData = true;

    setTimeout(function () {
        if (isLoadDataVisible) {
            var preloader = document.getElementById(`dataSpinner`);
            $(preloader).fadeOut();
        }

        for (var i = 0; i < classesView.length; i++)
            $(classesView[i]).fadeIn();

        $('html, body').animate({ scrollTop: $("#posts").offset().top }, 228);
    }, 228);
}

function OpenPost(postKey) {
    var url = "Post/" + postKey;
    $(location).attr('href', url);
}