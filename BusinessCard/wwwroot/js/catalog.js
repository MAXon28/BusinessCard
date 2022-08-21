window.onload = init;
var authorizedUser = false;
var channels = [];
var subscribeOnChannel = false;
var subscriveOnMailing = false;
var userTopchiks = [];
var userBookmarks = [];

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

            userTopchiks.push(false);
            document.getElementById('topchik1').classList.add("untopchikUser");

            userBookmarks.push(false);
            document.getElementById('bookmarks1').classList.add("notBookmarks");

            userTopchiks.push(false);
            document.getElementById('topchik2').classList.add("untopchikUser");
            document.getElementById('topchik2').classList.add("noTopchik");
            document.getElementById('topchikValue2').classList.add("noValue");

            userBookmarks.push(false);
            document.getElementById('bookmarks2').classList.add("notBookmarks");
            document.getElementById('bookmarksValue2').classList.add("noValue");

            userTopchiks.push(false);
            document.getElementById('topchik3').classList.add("untopchikUser");
            document.getElementById('topchik3').classList.add("noTopchik");
            document.getElementById('topchikValue3').classList.add("noValue");

            userBookmarks.push(false);
            document.getElementById('bookmarks3').classList.add("notBookmarks");

            userTopchiks.push(false);
            document.getElementById('topchik4').classList.add("untopchikUser");

            userBookmarks.push(false);
            document.getElementById('bookmarks4').classList.add("notBookmarks");
            document.getElementById('bookmarksValue4').classList.add("noValue");
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

function OpenSubscriptions() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }
}

function OpenMailings() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }
}

function OpenBookmarks() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }
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

function SendTopchik(postNumber) {
    if (!authorizedUser) {
        OpenInformationBlock("topchik");
        return;
    }

    if (userTopchiks[postNumber - 1]) {
        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue' + postNumber).innerHTML) - 1;
        $("#topchikValue" + postNumber).html(Math.round(newValue));
        if (newValue == 0) {
            document.getElementById('topchik' + postNumber).classList.add("noTopchik");
            document.getElementById('topchikValue' + postNumber).classList.add("noValue");
        }
        document.getElementById('topchik' + postNumber).classList.remove("topchikUser");
        document.getElementById('topchik' + postNumber).classList.add("untopchikUser");
        userTopchiks[postNumber - 1] = false;
    }
    else {
        var newValue = 0;
        newValue = parseInt(document.getElementById('topchikValue' + postNumber).innerHTML) + 1;
        $("#topchikValue" + postNumber).html(Math.round(newValue));
        if (newValue == 1) {
            document.getElementById('topchik' + postNumber).classList.remove("noTopchik");
            document.getElementById('topchikValue' + postNumber).classList.remove("noValue");
        }
        document.getElementById('topchik' + postNumber).classList.remove("untopchikUser");
        document.getElementById('topchik' + postNumber).classList.add("topchikUser");
        userTopchiks[postNumber - 1] = true;
    }
}

function InBookmarks(postNumber) {
    if (!authorizedUser) {
        OpenInformationBlock("bookmarks");
        return;
    }

    if (userBookmarks[postNumber - 1]) {
        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarksValue' + postNumber).innerHTML) - 1;
        $("#bookmarksValue" + postNumber).html(Math.round(newValue));
        if (newValue == 0)
            document.getElementById('bookmarksValue' + postNumber).classList.add("noValue");
        document.getElementById('bookmarks' + postNumber).classList.remove("needBookmarks");
        document.getElementById('bookmarks' + postNumber).classList.add("notBookmarks");
        document.getElementById('bookmarksValue' + postNumber).classList.remove("bookmarksValue");
        userBookmarks[postNumber - 1] = false;
    }
    else {
        var newValue = 0;
        newValue = parseInt(document.getElementById('bookmarksValue' + postNumber).innerHTML) + 1;
        $("#bookmarksValue" + postNumber).html(Math.round(newValue));
        if (newValue == 1)
            document.getElementById('bookmarksValue' + postNumber).classList.remove("noValue");
        document.getElementById('bookmarks' + postNumber).classList.remove("notBookmarks");
        document.getElementById('bookmarks' + postNumber).classList.add("needBookmarks");
        document.getElementById('bookmarksValue' + postNumber).classList.add("bookmarksValue");
        userBookmarks[postNumber - 1] = true;
    }
}

function SubscribeOnChannel() {
    /*if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }*/

    if (subscribeOnChannel) {
        document.querySelector('.subscriptionChannel').classList.remove("subscribeButton");
        document.querySelector('.subscriptionChannel').classList.add("notSubscribeOnChannelButton");
        subscribeOnChannel = false;
    }
    else {
        document.querySelector('.subscriptionChannel').classList.remove("notSubscribeOnChannelButton");
        document.querySelector('.subscriptionChannel').classList.add("subscribeButton");
        subscribeOnChannel = true;
    }
}

function SubscribeOnMailing() {
    if (!authorizedUser) {
        OpenInformationBlock("services");
        return;
    }

    if (subscribeOnChannel) {
        document.querySelector('.subscriptionMailing').classList.remove("subscribeButton");
        document.querySelector('.subscriptionMailing').classList.add("notSubscribeOnMailingButton");
        subscribeOnChannel = false;
    }
    else {
        document.querySelector('.subscriptionMailing').classList.remove("notSubscribeOnMailingButton");
        document.querySelector('.subscriptionMailing').classList.add("subscribeButton");
        subscribeOnChannel = true;
    }
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