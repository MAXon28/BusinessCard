window.onload = init;
var userTopchiks = [];
var userBookmarks = [];

function init() {
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
}

function OpenSubscriptions() {

}

function SendTopchik(postNumber) {
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