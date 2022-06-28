window.onload = init;
var sliders = [];
var reviews = [];

function init() {
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetProject?projectId=6",
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

            var typeName = document.createElement("p");
            typeName.className = "text type";
            typeName.innerHTML = projectData.type;
            $(typeName).css('text-align', 'cetner');
            mainBlock.append(typeName);

            var categoryName = document.createElement("p");
            categoryName.className = "text category";
            categoryName.innerHTML = projectData.category;
            $(categoryName).css('text=align', 'cetner');
            mainBlock.append(categoryName);

            if (projectData.clickType != null) {
                var downloadsCountBlock = document.createElement("div");
                downloadsCountBlock.className = "downloadsCountBlock";

                var clickCount = document.createElement("p");
                clickCount.className = "value";
                clickCount.innerHTML = projectData.clicksCount;

                var typeName = document.createElement("p");
                typeName.className = "text";
                typeName.innerHTML = projectData.clickType.typeName;

                downloadsCountBlock.append(clickCount);
                downloadsCountBlock.append(typeName);

                var clickProject = document.createElement("a");
                clickProject.className = "download";
                clickProject.innerHTML = projectData.clickType.actionName;
                if (projectData.clickType.actionName == "Скачать")
                    clickProject.setAttribute("download", "");
                clickProject.href = projectData.projectUrl;

                mainBlock.append(downloadsCountBlock);
                mainBlock.append(clickProject);
            }

            $(mainBlock).append('<div class="ratingInformationBlock"><div class="ratingBlock"><div class="ratingValueBlock"><p class="value" style="margin-right: 3px;">' + projectData.avgRating + '</p><label class="rating__item" style="color: gold; margin-left: 3px"><svg class="rating__star"><use xlink:href="#star"></use></svg></label></div><p class="text">Среднее</p></div><div class="separator vertical"></div><div class="countReviewsBlock"><p class="value">' + projectData.reviewsCount + '</p><p class="text">Оценки</p></div></div>');

            var creationDate = document.createElement("p");
            creationDate.className = "text date";
            creationDate.innerHTML = projectData.creationDate;
            $(creationDate).css('text-align', 'cetner');
            mainBlock.append(creationDate);

            if (projectData.images != null && projectData.images.length > 0) {
                SetScreenshots(projectData.images);
                document.getElementById(`screenshots`).classList.remove("skip");

                var elms = document.querySelectorAll('.slider');

                for (var i = 0, len = elms.length; i < len; i++) {
                    sliders.push(new ChiefSlider(elms[i], {
                        loop: false,
                        swipe: false,
                        refresh: true
                    }));
                }

                document.querySelector('.imagesFullWindow').classList.add("skip");
            }

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

            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.informationText').html(projectData.avgRating);
            $('#reviews').find('.reviewStatisticBlock').find('.generalStatisticBlock').find('.additionalText').html("Оценок: " + projectData.reviewsCount);

            $('#fiveStars').find('.animated-progress').find('span').css('width', projectData.ratingStatistic.excellentCount + "%");
            $('#fourStars').find('.animated-progress').find('span').css('width', projectData.ratingStatistic.goodCount + "%");
            $('#threeStars').find('.animated-progress').find('span').css('width', projectData.ratingStatistic.notBadCount + "%");
            $('#twoStars').find('.animated-progress').find('span').css('width', projectData.ratingStatistic.badCount + "%");
            $('#oneStars').find('.animated-progress').find('span').css('width', projectData.ratingStatistic.terriblyCount + "%");

            if (projectData.reviewsCount > 0) {
                var reviewBlock = document.createElement("div");
                reviewBlock.className = "reviewBlock";

                var reviewerName = document.createElement("p");
                reviewerName.className = "informationText";
                $(reviewerName).css('margin', '0 0 12px 0');
                $(reviewerName).css('font-size', '22px');
                $(reviewerName).css('line-height', 'normal');
                reviewerName.innerHTML = projectData.review.userName;

                var joinBlock = document.createElement("div");
                joinBlock.className = "joinBlock";

                var rating = document.createElement("div");
                rating.className = "rating";

                var firstStar = document.createElement("label");
                firstStar.className = "rating__item";
                $(firstStar).css('margin', '0 5px 0 0');
                firstStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (projectData.review.rating > 0)
                    firstStar.style.color = '#FFD700';

                var secondStar = document.createElement("label");
                secondStar.className = "rating__item";
                $(secondStar).css('margin', '0 5px');
                secondStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (projectData.review.rating > 1)
                    secondStar.style.color = '#FFD700';

                var thirdStar = document.createElement("label");
                thirdStar.className = "rating__item";
                $(thirdStar).css('margin', '0 5px');
                thirdStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (projectData.review.rating > 2)
                    thirdStar.style.color = '#FFD700';

                var fourthStar = document.createElement("label");
                fourthStar.className = "rating__item";
                $(fourthStar).css('margin', '0 5px');
                fourthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (projectData.review.rating > 3)
                    fourthStar.style.color = '#FFD700';

                var fifthStar = document.createElement("label");
                fifthStar.className = "rating__item";
                $(fifthStar).css('margin', '0 5px');
                fifthStar.innerHTML = '<svg class="rating__star"><use xlink:href="#star"></use></svg>';

                if (projectData.review.rating > 4)
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
                date.innerHTML = projectData.review.date;

                joinBlock.append(rating);
                joinBlock.append(date);

                var text = document.createElement("p");
                text.id = "review1";
                text.className = "informationText reviewText";
                $(text).css('margin', '7px 0 12px 0');
                $(text).css('font-size', '22px');
                text.innerHTML = "<span>" + projectData.review.text + "</span>";

                var turningAroundReview = document.createElement("p");
                turningAroundReview.id = "turningAroundReview1";
                turningAroundReview.className = "turningAroundButton";
                $(turningAroundReview).css('margin', '0 0 12px 0');
                turningAroundReview.setAttribute("onclick", "ViewFullReview(1)");
                turningAroundReview.innerHTML = "Весь отзыв";

                var separator = document.createElement("div");
                separator.className = "separator horizontal";
                $(separator).css('width', '5%');
                $(separator).css('height', '1px');

                reviewBlock.append(reviewerName);
                reviewBlock.append(joinBlock);
                reviewBlock.append(text);
                reviewBlock.append(turningAroundReview);
                reviewBlock.append(separator);

                document.getElementById(`reviews`).append(reviewBlock);

                if ($('#review1').find('span:first-child').height() <= 340)
                    $('#turningAroundReview1').css("display", "none");
                else
                    $('#turningAroundReview1').css("display", "block");
                reviews.push(1);

                if (projectData.reviewsCount > 1) {
                    var turningAroundButtonReview = document.createElement("p");
                    turningAroundButtonReview.className = "turningAroundButton";
                    turningAroundButtonReview.innerHTML = "Посмотреть все";

                    document.getElementById(`reviews`).append(turningAroundButtonReview);
                }
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

            if ($('#description').find('span:first-child').height() <= 528)
                $('#turningAroundDescription').css("display", "none");
            else
                $('#turningAroundDescription').css("display", "block");

            information.classList.remove("skip");

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
        },
        error: function (error) {
            alert(error);
        }
    });

    /*var html = document.querySelector('html');

    var imagesFullWindow = document.createElement("div");
    imagesFullWindow.className = "imagesFullWindow";

    var htmlCode = $('div.images').html();

    imagesFullWindow.innerHTML = htmlCode;
    var a
    html.append(imagesFullWindow);*/
}

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

    var imagesFullWindow = document.createElement("div");
    imagesFullWindow.className = "imagesFullWindow";
    imagesFullWindow.classList.add("skip");

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseFullScreen()");

    imagesFullWindow.append(skipButton);
    imagesFullWindow.append(sliderFullWindow);

    screenshots.append(images);
    html.append(imagesFullWindow);
}

function OpenFullScreen(id) {
    sliders[1].refresh();

    if ($('html div.imagesFullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active').length == 0)
        sliders[1].next();

    if ($('html div.imagesFullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id == id)
        document.querySelector('.imagesFullWindow').classList.remove("skip");

    if ($('html div.imagesFullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id < id)
        while ($('html div.imagesFullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id != id)
            sliders[1].next();
    else
        while ($('html div.imagesFullWindow div.slider div.slider__wrapper div.slider__items div.slider__item_active')[0].id != id)
            sliders[1].prev();

    document.querySelector('.imagesFullWindow').classList.remove("skip");
}

function CloseFullScreen() {
    document.querySelector('.imagesFullWindow').classList.add("skip");
}

$(window).resize(function () {
    $.each(reviews, function (index, review) {
        if ($('#review' + review).find('span:first-child').height() <= 340)
            $('#turningAroundReview' + review).css("display", "none");
        else
            $('#turningAroundReview' + review).css("display", "block");
    });

    if ($('#description').find('span:first-child').height() <= 528)
        $('#turningAroundDescription').css("display", "none");
    else
        $('#turningAroundDescription').css("display", "block");
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
        $('#turningAroundReview' + reviewNumber).text("Свернуть");
    }
    else {
        $('#review' + reviewNumber).css('maxHeight', 340);
        $('#turningAroundReview' + reviewNumber).text("Весь отзыв");
    }
}