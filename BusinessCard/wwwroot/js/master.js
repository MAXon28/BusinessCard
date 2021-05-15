window.onload = init;
var windowWidth = $(window).width();

function init() {
    console.log(windowWidth);
};

$(window).resize(function () {
    windowWidth = $(window).width();
    console.log(windowWidth);
});

$(".rightNav").on("mousedown", function () {
    return false;
});

$(".leftNav").on("mousedown", function () {
    return false;
});