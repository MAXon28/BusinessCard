window.onload = init;
var isLoad = false;
var isLoadVisible = false;
var isLoadAboutService = false;
var descriptions = new Map();

function init() {
    Timer();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetServices",
        success: function (services) {
            for (var i = 0; i < services.length; i++) {
                SetService(services[i]);
            }

            ViewServices();
        },
        error: function (error) {
            alert(error);
        }
    });
};

function Timer() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.remove('skip');
        }
    };
};

function SetService(service) {
    var servicesDiv = document.getElementById(`services`);

    var serviceDiv = document.createElement("div");
    serviceDiv.id = service.id;
    serviceDiv.className = "service";

    var serviceNameDiv = document.createElement("div");
    serviceNameDiv.className = "serviceNameBlock";

    var serviceName = document.createElement("p");
    serviceName.className = "serviceName";
    serviceName.innerHTML = service.name;
    serviceNameDiv.append(serviceName);

    var shortDescriptionDiv = document.createElement("div");
    shortDescriptionDiv.className = "shortDescriptionBlock";

    for (var i = 0; i < service.shortDescriptions.length; i++) {
        var shortDescription = document.createElement("p");
        shortDescription.className = "shortDescription";
        shortDescription.innerHTML = service.shortDescriptions[i];
        shortDescriptionDiv.append(shortDescription);

        if (i + 1 >= service.shortDescriptions.length)
            continue;

        var separation = document.createElement("div");
        separation.className = "separation";
        shortDescriptionDiv.append(separation);
    }

    var priceDiv = document.createElement("div");
    priceDiv.className = "priceBlock";

    var price = document.createElement("p");
    price.className = "price";
    price.innerHTML = service.price;
    priceDiv.append(price);

    var buttonsDiv = document.createElement("div");
    buttonsDiv.className = "buttonsGroup";

    var createTaskButton = document.createElement("a");
    createTaskButton.id = "ServiceButton";
    createTaskButton.className = "createTaskButton";
    createTaskButton.innerHTML = "Оформить заявку";
    createTaskButton.href = 'ServiceTaskRegistration?serviceId=' + service.id;
    buttonsDiv.append(createTaskButton);

    var learnMoreButton = document.createElement("button");
    learnMoreButton.className = "learnMoreButton";
    learnMoreButton.innerHTML = "Подробнее";
    learnMoreButton.setAttribute("onclick", "LoadMoreDetailed(" + service.id + ")");
    buttonsDiv.append(learnMoreButton);

    serviceDiv.append(serviceNameDiv);
    serviceDiv.append(shortDescriptionDiv);
    serviceDiv.append(priceDiv);
    serviceDiv.append(buttonsDiv);

    servicesDiv.append(serviceDiv);
}

function ViewServices() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('skip');
        }

        var servicesDiv = document.getElementById(`services`);
        servicesDiv.classList.remove('skip');
        servicesDiv.classList.add('visible');

        $('html, body').animate({ scrollTop: $(".serviceHeader").offset().top }, 228);
    }, 228);

}

function LoadMoreDetailed(id) {
    var reviews = document.getElementById(`reviews`);
    reviews.classList.add('skip');
    document.querySelector('.container').innerHTML = "";

    var moreDetailed = document.getElementById(`moreDetailed`);
    moreDetailed.classList.remove('skip');

    $('#moreDetailed').find('.informationBlock').find('.serviceName').text($('#' + id).find('.serviceNameBlock').find('.serviceName').html());
    $('#moreDetailed').find('.informationBlock').find('.right_informationBlock').find('.price').text($('#' + id).find('.priceBlock').find('.price').html());
    document.getElementById("serviceTaskButton").href = 'ServiceTaskRegistration?serviceId=' + id;

    document.getElementById("buttonDescription").setAttribute("onclick", "ViewMoreDetailed(" + id + ")");
    document.getElementById("buttonReviews").setAttribute("onclick", "ViewReviews(" + id + ")");

    $('.aboutService').scrollTop();

    $('html, body').animate({ scrollTop: $(".aboutService").offset().top }, 528);

    if (descriptions.size > 0 && typeof descriptions.get(id) !== "undefined") {
        var description = document.getElementById(`description`);
        description.classList.remove('skip');
        description.classList.add('visible');
        $('#description').find('p').text(descriptions.get(id));
        return;
    }

    TimerPreloader();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetFullDescription",
        data: { id },
        success: function (fullDescription) {
            var preloader = document.querySelector('.preloader');
            preloader.classList.add('loaded');
            preloader.classList.remove('loaded_hiding');

            $('#description').find('p').text(fullDescription);

            descriptions.set(id, fullDescription);

            var description = document.getElementById(`description`);
            description.classList.remove('skip');
            description.classList.add('visible');

            isLoadAboutService = true;

            setTimeout(function () {
                description.classList.add('visibleOpacity');
            }, 128);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function ViewMoreDetailed(id) {
    var reviews = document.getElementById(`reviews`);
    reviews.classList.add('skip');

    var description = document.getElementById(`description`);
    description.classList.remove('skip');

    if ($('#moreDetailed').find('.informationBlock').find('.serviceName').html() != "")
        return;

    LoadMoreDetailed(id);
}

function ViewReviews(id) {
    var description = document.getElementById(`description`);
    description.classList.add('skip');

    var reviews = document.getElementById(`reviews`);
    reviews.classList.remove('skip');

    var length = $('div.slider__items div.slider__item').length;

    if (length > 0)
        return;

    isLoadAboutService = false;
    TimerPreloader();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetReviews",
        data: { id },
        success: function (reviews) {
            var preloader = document.querySelector('.preloader');
            preloader.classList.add('loaded');
            preloader.classList.remove('loaded_hiding');

            var slider = document.createElement("div");
            slider.className = "slider";

            var slider__wrapper = document.createElement("div");
            slider__wrapper.className = "slider__wrapper";

            var slider__items = document.createElement("div");
            slider__items.className = "slider__items";
            slider__items.id = "items";

            slider__wrapper.append(slider__items);

            slider.append(slider__wrapper);
            $(slider).append('<a href="#" class="slider__control" data-slide="prev" onclick="SlideReviews()"></a>');
            $(slider).append('<a href="#" class="slider__control" data-slide="next" onclick="SlideReviews()"></a>');

            document.getElementById(`reviews`).append(slider);

            for (var i = 0; i < reviews.length; i++) {
                var sliderItem = document.createElement("div");
                sliderItem.className = "slider__item";

                var reviewBlock = document.createElement("div");
                reviewBlock.className = "reviewBlock";

                var rating = document.createElement("div");
                rating.className = "rating";

                var firstStar = document.createElement("label");
                firstStar.className = "rating__item";
                firstStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (reviews[i].rating > 0)
                    firstStar.style.color = '#FFD700';

                var secondStar = document.createElement("label");
                secondStar.className = "rating__item";
                secondStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (reviews[i].rating > 1)
                    secondStar.style.color = '#FFD700';

                var thirdStar = document.createElement("label");
                thirdStar.className = "rating__item";
                thirdStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (reviews[i].rating > 2)
                    thirdStar.style.color = '#FFD700';

                var fourthStar = document.createElement("label");
                fourthStar.className = "rating__item";
                fourthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (reviews[i].rating > 3)
                    fourthStar.style.color = '#FFD700';

                var fifthStar = document.createElement("label");
                fifthStar.className = "rating__item";
                fifthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (reviews[i].rating > 4)
                    fifthStar.style.color = '#FFD700';

                rating.append(firstStar);
                rating.append(secondStar);
                rating.append(thirdStar);
                rating.append(fourthStar);
                rating.append(fifthStar);

                reviewBlock.append(rating);

                var name = document.createElement("p");
                name.className = "name";
                name.innerHTML = reviews[i].userName;
                reviewBlock.append(name);

                var quote = document.createElement("p");
                quote.className = "u-icon u-icon-circle u-icon-1";
                quote.innerHTML = '<svg class="u-svg-link" preserveAspectRatio="xMidYMin slice" viewBox="0 0 95.333 95.332"><use href="#svg-e21a"></use></svg><svg class="u-svg-content" viewBox="0 0 95.333 95.332" x="0px" y="0px" id="svg-e21a" style="enable-background:new 0 0 95.333 95.332;"><g><g><path d="M30.512,43.939c-2.348-0.676-4.696-1.019-6.98-1.019c-3.527,0-6.47,0.806-8.752,1.793    c2.2-8.054,7.485-21.951,18.013-23.516c0.975-0.145,1.774-0.85,2.04-1.799l2.301-8.23c0.194-0.696,0.079-1.441-0.318-2.045    s-1.035-1.007-1.75-1.105c-0.777-0.106-1.569-0.16-2.354-0.16c-12.637,0-25.152,13.19-30.433,32.076    c-3.1,11.08-4.009,27.738,3.627,38.223c4.273,5.867,10.507,9,18.529,9.313c0.033,0.001,0.065,0.002,0.098,0.002    c9.898,0,18.675-6.666,21.345-16.209c1.595-5.705,0.874-11.688-2.032-16.851C40.971,49.307,36.236,45.586,30.512,43.939z"></path><path d="M92.471,54.413c-2.875-5.106-7.61-8.827-13.334-10.474c-2.348-0.676-4.696-1.019-6.979-1.019    c-3.527,0-6.471,0.806-8.753,1.793c2.2-8.054,7.485-21.951,18.014-23.516c0.975-0.145,1.773-0.85,2.04-1.799l2.301-8.23    c0.194-0.696,0.079-1.441-0.318-2.045c-0.396-0.604-1.034-1.007-1.75-1.105c-0.776-0.106-1.568-0.16-2.354-0.16    c-12.637,0-25.152,13.19-30.434,32.076c-3.099,11.08-4.008,27.738,3.629,38.225c4.272,5.866,10.507,9,18.528,9.312    c0.033,0.001,0.065,0.002,0.099,0.002c9.897,0,18.675-6.666,21.345-16.209C96.098,65.559,95.376,59.575,92.471,54.413z"></path></g></g></svg>';;
                reviewBlock.append(quote);

                var review = document.createElement("p");
                review.id = "review" + (i + 1);
                review.className = "review";
                review.innerHTML = reviews[i].text;
                reviewBlock.append(review);

                var turningAroundButton = document.createElement("button");
                turningAroundButton.id = "review" + (i + 1) + "Button";
                turningAroundButton.className = "turningAroundButton";
                turningAroundButton.innerHTML = "Развернуть";
                turningAroundButton.setAttribute("onclick", "ViewFullReview(" + (i + 1) + ")");
                $(review).after(turningAroundButton);


                var date = document.createElement("p");
                date.className = "date";
                date.innerHTML = reviews[i].date;
                reviewBlock.append(date);

                sliderItem.append(reviewBlock);

                document.getElementById(`items`).append(sliderItem);

                var orig = $(review).html();
                var wraped = '<span>' + orig + '</span>';
                $(review).html(wraped);
                if ($(review).find('span:first-child').height() > 228)
                    $(turningAroundButton).css("visibility", "visible");
            }

            var elms = document.querySelectorAll('.slider');

            for (var i = 0, len = elms.length; i < len; i++) {
                // инициализация elms[i] в качестве слайдера
                new ChiefSlider(elms[i], {
                    loop: true,
                    swipe: false,
                    refresh: true
                });
            }

            isLoadAboutService = true;

            setTimeout(function () {
                //description.classList.add('visibleOpacity');
            }, 128);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function TimerPreloader() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadAboutService) {
            var description = document.getElementById(`description`);
            description.classList.remove('visible');
            description.classList.remove('visibleOpacity');
            description.classList.add('skip');

            var preloader = document.querySelector('.preloader');
            preloader.classList.add('loaded_hiding');
            preloader.classList.remove('loaded');
        }
    };
    isLoadAboutService = false;
};

$(window).resize(function () {
    windowWidth = $(window).width();
    console.log(windowWidth);

    $('.slider__item').each(function () {
        if ($(this).find('.review').find('span:first-child').height() > 228)
            $(this).find('.turningAroundButton').css("visibility", "visible");
        else
            $(this).find('.turningAroundButton').css("visibility", "hidden");
    });
});

function ViewFullReview(index) {
    if ($('#review' + index + 'Button').text() == "Развернуть") {
        $('#review' + index).height("auto");
        $('#review' + index).css('maxHeight','none');
        $('#review' + index + 'Button').text("Свернуть");
    }
    else {
        //$('#' + id).height(228);
        $('#review' + index).css('maxHeight', 228);
        $('#review' + index + 'Button').text("Развернуть");
    }
}

function SlideReviews() {
    $('.slider__item').each(function () {
        if ($(this).attr('class').indexOf("slider__item_active") != -1) {
            var id = $(this).find('.reviewBlock').find('.review').attr('id');
            $('#' + id).css('maxHeight', 228);
            $('#' + id + 'Button').text("Развернуть");
        }
    });
}