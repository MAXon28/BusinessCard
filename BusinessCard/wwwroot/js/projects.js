window.onload = init;
var isLoad = false;
var isLoadVisible = false;
var currentPageId = "page1";
var isSmall = false;
var canThrowOff = false;

$(window).resize(function () {
    var width = $(window).width();
    if (!isSmall && width <= 943) {
        $('#filters').css({ "display": "none" });
        document.querySelector('.setFiltersBlock').classList.remove("skip");

        if (canThrowOff) {
            $('#throwOffSmallView').css({ "display": "block" });
            $('#throwOff').css({ "display": "none" });
        }

        $('#filterButtonsBlock').css({ "display": "none" });
        $('.skipFiltersButton').css({ "display": "block" });

        isSmall = true;

        $('html, body').animate({ scrollTop: $("#projects").offset().top }, 228);

        return;
    }

    if (isSmall && width > 943) {
        $('#filters').css({ "display": "block" });
        $('#thirdSection').css({ "display": "flex" });
        document.querySelector('.setFiltersBlock').classList.add("skip");

        if (canThrowOff) {
            $('#throwOff').css({ "display": "block" });
        }

        $('#filterButtonsBlock').css({ "display": "none" });
        $('.skipFiltersButton').css({ "display": "none" });

        isSmall = false;

        $('html, body').animate({ scrollTop: $("#projects").offset().top }, 228);

        return;
    }
});

var applyFilters = {
    ProjectName: "",
    SortType: 1,
    ProjectTypes: [],
    ProjectCategories: [],
    ProjectCompatibilities: []
};

var notApplyFilters = {
    ProjectName: "",
    SortType: 1,
    ProjectTypes: [],
    ProjectCategories: [],
    ProjectCompatibilities: []
};

let timer = null;

class DynamicTimer {

    constructor(func, delay) {
        this.callback = func
        this.triggerTime = delay
        this.timer = 0
        this.updateTimer()
    }

    updateTimer() {
        clearTimeout(this.timer)
        let delay = this.triggerTime
        this.timer = setTimeout(this.callback, delay)
        return this
    }

    addTime(delay) {
        this.triggerTime = delay
        this.updateTimer()
        return this
    }
}

window.onpopstate = function(e){
    if(e.state){
        document.getElementById("content").innerHTML = e.state.html;
        document.title = e.state.pageTitle;
    }
};

function init() {
    Timer();

    if ($(window).width() <= 943) {
        $('#filters').css({ "display": "none" });
        document.querySelector('.setFiltersBlock').classList.remove("skip");
        if (canThrowOff) {
            $('#throwOffSmallView').css({ "display": "block" });
            $('#throwOff').css({ "display": "none" });
        }
        $('#filterButtonsBlock').css({ "display": "none" });
        $('.skipFiltersButton').css({ "display": "block" });
        isSmall = true;
    }

    //location.hash = "SortType=1";

    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetAllProjectsData?SortType=1",
        contentType: 'application/json',
        dataTpye: "json",
        success: function (projectsData) {
            SetProjects(projectsData.projects);

            var typesBlock = document.getElementById(`appTypes`);
            for (var i = 0; i < projectsData.filters.projectTypes.length; i++) {
                var checkboxBlock = document.createElement("div");
                checkboxBlock.className = "checkboxBlock";

                var checkbox = document.createElement("div");
                checkbox.className = "checkbox";

                var input = document.createElement("input");
                input.className = "custom-checkbox";
                input.setAttribute("value", projectsData.filters.projectTypes[i].id);
                input.id = projectsData.filters.projectTypes[i].processedValue;
                input.setAttribute("type", 'checkbox');

                var label = document.createElement("label");
                label.className = "filter";
                label.setAttribute("for", projectsData.filters.projectTypes[i].processedValue);
                $(label).click(function () {
                    CheckTypeFilter(this);
                    if (CanApply())
                        CheckFilter(this);
                    else
                        UncheckFilter();
                });

                checkbox.append(input);
                checkbox.append(label);

                var filterText = document.createElement("p");
                filterText.innerHTML = projectsData.filters.projectTypes[i].value;

                checkboxBlock.append(checkbox);
                checkboxBlock.append(filterText);

                typesBlock.append(checkboxBlock);
            }

            var categoriesBlock = document.getElementById(`appCategories`);
            for (var i = 0; i < projectsData.filters.projectCategories.length; i++) {
                var categoryBlock = document.createElement("li");

                var input = document.createElement("input");
                input.id = projectsData.filters.projectCategories[i].processedValue;
                input.setAttribute("type", 'checkbox');
                input.setAttribute("value", projectsData.filters.projectCategories[i].id);

                var label = document.createElement("label");
                label.className = "filter";
                label.setAttribute("for", projectsData.filters.projectCategories[i].processedValue);
                $(label).click(function () {
                    CheckCategoryFilter(this);
                    if (CanApply())
                        CheckFilter(this);
                    else
                        UncheckFilter();
                });
                label.innerHTML = projectsData.filters.projectCategories[i].value;

                categoryBlock.append(input);
                categoryBlock.append(label);

                categoriesBlock.append(categoryBlock);
            }

            var compatibilitiesBlock = document.getElementById(`appCompatibilities`);
            for (var compatibilityType in projectsData.filters.projectCompatibilities) {
                var subtitle = document.createElement("p");
                $(subtitle).css({ "font-family": "system-ui", "font-weight": "400", "margin-top": "12px" });
                subtitle.innerHTML = compatibilityType;
                compatibilitiesBlock.append(subtitle);

                for (var i = 0; i < projectsData.filters.projectCompatibilities[compatibilityType].length; i++) {
                    var checkboxBlock = document.createElement("div");
                    checkboxBlock.className = "checkboxBlock";

                    var checkbox = document.createElement("div");
                    checkbox.className = "checkbox";

                    var input = document.createElement("input");
                    input.className = "custom-checkbox";
                    input.setAttribute("value", projectsData.filters.projectCompatibilities[compatibilityType][i].id);
                    input.id = projectsData.filters.projectCompatibilities[compatibilityType][i].processedValue;
                    input.setAttribute("type", 'checkbox');

                    var label = document.createElement("label");
                    label.className = "filter";
                    label.setAttribute("for", projectsData.filters.projectCompatibilities[compatibilityType][i].processedValue);
                    $(label).click(function () {
                        CheckCompatibilityFilter(this);
                        if (CanApply())
                            CheckFilter(this);
                        else
                            UncheckFilter();
                    });

                    checkbox.append(input);
                    checkbox.append(label);

                    var filterText = document.createElement("p");
                    filterText.innerHTML = projectsData.filters.projectCompatibilities[compatibilityType][i].value;

                    checkboxBlock.append(checkbox);
                    checkboxBlock.append(filterText);

                    compatibilitiesBlock.append(checkboxBlock);
                }
            }

            SetGeneralInformation('', projectsData.generalInformation);
            SetGeneralInformation('SmallView', projectsData.generalInformation);

            if (projectsData.generalInformation.pagesCount > 1)
                SetPagesButtons(projectsData.generalInformation.pagesCount);

            ViewForms();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SetGeneralInformation(id, generalInformation) {
    var projectsCountBlock = document.getElementById(`projectsCount` + id);
    var projectsCounter = document.createElement("p");
    projectsCounter.className = "counter projectsCounter";
    projectsCounter.innerHTML = generalInformation.projectsCount.count;
    var projectsCounterName = document.createElement("p");
    projectsCounterName.className = "counterName";
    projectsCounterName.innerHTML = generalInformation.projectsCount.text;
    projectsCountBlock.append(projectsCounter);
    projectsCountBlock.append(projectsCounterName);

    var downloadsCountBlock = document.getElementById(`downloadsCount` + id);
    var downloadsCounter = document.createElement("p");
    downloadsCounter.className = "counter downloadsCounter";
    downloadsCounter.innerHTML = generalInformation.downloadsCount.count;
    var downloadsCounterName = document.createElement("p");
    downloadsCounterName.className = "counterName";
    downloadsCounterName.innerHTML = generalInformation.downloadsCount.text;
    downloadsCountBlock.append(downloadsCounter);
    downloadsCountBlock.append(downloadsCounterName);

    var ratingBlock = document.getElementById(`rating` + id);
    var rating = document.createElement("p");
    rating.className = "counter rating";
    rating.innerHTML = generalInformation.avgRating;
    var ratingName = document.createElement("p");
    ratingName.className = "counterName";
    ratingName.innerHTML = "Рейтинг проектов";
    ratingBlock.append(rating);
    ratingBlock.append(ratingName);
}

function ViewForms() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('skip');
        }

        var searchBlock = document.querySelector('.searchBlock');
        searchBlock.classList.remove('skip');

        var filters = document.getElementById(`filters`);
        filters.classList.remove('skip');
        if ($(window).width() <= 943) {
            $(filters).css({ "display": "none" });
            $('#filterButtonsBlock').css({ "display": "none" });
            $('.skipFiltersButton').css({ "display": "block" });
            isSmall = true;
        }

        var sortBlock = document.querySelector('.sortBlock');
        sortBlock.classList.remove('skip');

        var myProjectsBlock = document.querySelector('.myProjectsBlock');
        myProjectsBlock.classList.remove('skip');

        var generalInformation = document.getElementById(`generalInformation`);
        generalInformation.classList.remove('skip');

        var pagesButtonsBlock = document.getElementById(`pagesButtons`);
        pagesButtonsBlock.classList.remove('skip');
        pagesButtonsBlock.classList.add('viewButtons');

        $('html, body').animate({ scrollTop: $("#projects").offset().top }, 228);
    }, 228);

}

$(document).ready(function () {
    $('#search').keydown(function (e) {
        if (e.keyCode === 13)
            SearchProjects();
    });
});

function SearchProjects() {
    notApplyFilters.ProjectName = $('#search').val();

    if (applyFilters.ProjectName != notApplyFilters.ProjectName) {
        currentPageId = "page1";

        applyFilters.ProjectName = notApplyFilters.ProjectName;

        UncheckFilter();

        isLoad = false;
        Timer();

        CheckWidth();

        $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

        $("div").remove(".myProjectBlock");
        $("p").remove(".pageButton");

        var requestParameters = GetRequestParameters(1, true);
        $.ajax({
            async: true,
            type: "GET",
            url: "/MAXonStore/GetProjects?" + requestParameters,
            contentType: 'application/json',
            dataTpye: "json",
            traditional: true,
            success: function (projectsInformation) {
                SetProjects(projectsInformation.projects);

                if (projectsInformation.pagesCountByCurrentFilters > 1)
                    SetPagesButtons(projectsInformation.pagesCountByCurrentFilters);

                ViewData();
            },
            error: function (error) {
                alert(error);
            }
        });
    }
}

$('.select__item').on('click', function () {
    var sort = $(this).val();

    notApplyFilters.SortType = sort;

    if (applyFilters.SortType != sort) {

        if (currentPageId != "page1") {
            var currentPageButton = document.getElementById(currentPageId);
            currentPageButton.classList.remove("active");

            var pageButton = document.getElementById(`page1`);
            pageButton.classList.add("active");
        }

        applyFilters.SortType = sort;

        UncheckFilter();

        isLoad = false;
        Timer();

        $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

        $("div").remove(".myProjectBlock");
        var requestParameters = GetRequestParameters(1, false);
        $.ajax({
            async: true,
            type: "GET",
            url: "/MAXonStore/GetProjects?" + requestParameters,
            contentType: 'application/json',
            dataTpye: "json",
            traditional: true,
            success: function (projects) {
                SetProjects(projects);

                ViewData();

                currentPageId = "page1";
            },
            error: function (error) {
                alert(error);
            }
        });
    }
});

jQuery(($) => {
    $('.select').on('click', '.select__head', function () {
        if ($(this).hasClass('open')) {
            $(this).removeClass('open');
            $(this).next().fadeOut();
        } else {
            $('.select__head').removeClass('open');
            $('.select__list').fadeOut();
            $(this).addClass('open');
            $(this).next().fadeIn();
        }
    });

    $('.select').on('click', '.select__item', function () {
        $('.select__head').removeClass('open');
        $(this).parent().fadeOut();
        $(this).parent().prev().text($(this).text());
        $(this).parent().prev().prev().val($(this).text());
    });

    $(document).click(function (e) {
        if (!$(e.target).closest('.select').length) {
            $('.select__head').removeClass('open');
            $('.select__list').fadeOut();
        }
    });
});

function CheckTypeFilter(element) {
    checkboxId = $(element).attr('for');

    if (!($('#' + checkboxId).prop('checked')))
        notApplyFilters.ProjectTypes.push($('#' + checkboxId).val());
    else
        notApplyFilters.ProjectTypes.splice(notApplyFilters.ProjectTypes.indexOf($('#' + checkboxId).val()), 1);
}

function CheckCategoryFilter(element) {
    checkboxId = $(element).attr('for');

    if (!($('#' + checkboxId).prop('checked')))
        notApplyFilters.ProjectCategories.push($('#' + checkboxId).val());
    else
        notApplyFilters.ProjectCategories.splice(notApplyFilters.ProjectCategories.indexOf($('#' + checkboxId).val()), 1);
}

function CheckCompatibilityFilter(element) {
    checkboxId = $(element).attr('for');

    if (!($('#' + checkboxId).prop('checked')))
        notApplyFilters.ProjectCompatibilities.push($('#' + checkboxId).val());
    else
        notApplyFilters.ProjectCompatibilities.splice(notApplyFilters.ProjectCompatibilities.indexOf($('#' + checkboxId).val()), 1);
}

function CanApply() {
    if (applyFilters.ProjectTypes.length > notApplyFilters.ProjectTypes.length)
        return true;

    if (applyFilters.ProjectCategories.length > notApplyFilters.ProjectCategories.length)
        return true;

    if (applyFilters.ProjectCompatibilities.length > notApplyFilters.ProjectCompatibilities.length)
        return true;

    for (var i = 0; i < notApplyFilters.ProjectTypes.length; i++)
        if (applyFilters.ProjectTypes.indexOf(notApplyFilters.ProjectTypes[i]) == -1)
            return true;

    for (var i = 0; i < notApplyFilters.ProjectCategories.length; i++)
        if (applyFilters.ProjectCategories.indexOf(notApplyFilters.ProjectCategories[i]) == -1)
            return true;

    for (var i = 0; i < notApplyFilters.ProjectCompatibilities.length; i++)
        if (applyFilters.ProjectCompatibilities.indexOf(notApplyFilters.ProjectCompatibilities[i]) == -1)
            return true;

    return false;
}

function CheckFilter(element) {
    if (isSmall) {
        $('#filterButtonsBlock').css({ "display": "flex" });
        return;
    }

    var top = $(element).offset().top - $('.filtersBlock').offset().top + 40;
    var apply = document.getElementById(`apply`);
    $(apply).css('top', top);

    $(apply).show();
    if (timer == null)
        timer = new DynamicTimer(function () {
            $(apply).hide();
        }, 7000);
    else
        timer.addTime(7000);
}

function UncheckFilter() {
    var apply = document.getElementById(`apply`);
    $(apply).hide();
}

function Apply() {
    UncheckFilter();

    currentPageId = "page1";

    applyFilters = JSON.parse(JSON.stringify(notApplyFilters));

    if (applyFilters.ProjectTypes.length != 0 || applyFilters.ProjectCategories.length != 0 || applyFilters.ProjectCompatibilities.length != 0) {
        canThrowOff = true;

        if (!isSmall)
            $('#throwOff').css({ "display": "block" });
        else
            $('#throwOffSmallView').css({ "display": "block" });
    }
    else {
        if (canThrowOff) {
            $('#throwOff').css({ "display": "none" });
            $('#throwOffSmallView').css({ "display": "none" });
            canThrowOff = false;
        }
    }

    CheckWidth();

    $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

    $("div").remove(".myProjectBlock");
    $("p").remove(".pageButton");

    isLoad = false;
    Timer();
    var requestParameters = GetRequestParameters(1, true);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetProjects?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (projectsInformation) {
            SetProjects(projectsInformation.projects);

            if (projectsInformation.pagesCountByCurrentFilters > 1)
                SetPagesButtons(projectsInformation.pagesCountByCurrentFilters);

            ViewData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GetRequestParameters(pageNumber, needUpdatePagesCount) {
    var requestParameters = "";

    if (applyFilters.ProjectName != "")
        requestParameters += "ProjectName=" + applyFilters.ProjectName;

    if (requestParameters == "")
        requestParameters += "SortType=" + applyFilters.SortType;
    else
        requestParameters += "&SortType=" + applyFilters.SortType;

    if (applyFilters.ProjectTypes.length > 0) {
        for (var i = 0; i < applyFilters.ProjectTypes.length; i++) {
            if (i != 0 || requestParameters != "")
                requestParameters += "&ProjectTypes=" + applyFilters.ProjectTypes[i];
            else
                requestParameters += "ProjectTypes=" + applyFilters.ProjectTypes[i];
        }
    }

    if (applyFilters.ProjectCategories.length > 0) {
        for (var i = 0; i < applyFilters.ProjectCategories.length; i++) {
            if (i != 0 || requestParameters != "")
                requestParameters += "&ProjectCategories=" + applyFilters.ProjectCategories[i];
            else
                requestParameters += "ProjectCategories=" + applyFilters.ProjectCategories[i];
        }
    }

    if (applyFilters.ProjectCompatibilities.length > 0) {
        for (var i = 0; i < applyFilters.ProjectCompatibilities.length; i++) {
            if (i != 0 || requestParameters != "")
                requestParameters += "&ProjectCompatibilities=" + applyFilters.ProjectCompatibilities[i];
            else
                requestParameters += "ProjectCompatibilities=" + applyFilters.ProjectCompatibilities[i];
        }
    }

    if (requestParameters == "")
        requestParameters += "pageNumber=" + pageNumber;
    else
        requestParameters += "&pageNumber=" + pageNumber;

    requestParameters += "&needUpdatePagesCount=" + needUpdatePagesCount;

    return requestParameters;
}

function SetProjects(projects) {
    var projectsBlock = document.getElementById(`projects`);
    for (var i = 0; i < projects.length; i++) {
        var myProjectBlock = document.createElement("div");
        myProjectBlock.className = "myProjectBlock";
        myProjectBlock.setAttribute("onclick", "OpenProject('" + projects[i].id.toString() + "')");;
        if (i == 0)
            $(myProjectBlock).css({ "margin-top": "19px" });

        var leftPartWrapper = document.createElement("div");
        leftPartWrapper.className = "leftPartWrapper";

        var leftPartBlock = document.createElement("div");
        leftPartBlock.className = "leftPartBlock";

        if (projects[i].icon != null)
            leftPartBlock.innerHTML = '<img class="projectIcon" src="' + projects[i].icon + '">';

        var projectName = document.createElement("p");
        projectName.className = "projectName";
        projectName.innerHTML = projects[i].name;
        leftPartBlock.append(projectName);

        leftPartWrapper.append(leftPartBlock);

        myProjectBlock.append(leftPartWrapper);

        var rightPartWrapper = document.createElement("div");
        rightPartWrapper.className = "rightPartWrapper";

        var rightPartBlock = document.createElement("div");
        rightPartBlock.className = "rightPartBlock";

        var projectType = document.createElement("p");
        projectType.className = "projectType";
        projectType.innerHTML = projects[i].type;
        rightPartBlock.append(projectType);

        var projectCategory = document.createElement("p");
        projectCategory.className = "projectCategory";
        projectCategory.innerHTML = projects[i].category;
        rightPartBlock.append(projectCategory);

        var projectCompatibilitiesBlock = document.createElement("div");
        projectCompatibilitiesBlock.className = "projectCompatibilitiesBlock";

        var htmlCode = "";
        for (var j = 0; j < projects[i].compatibilities.length; j++)
            htmlCode += '<svg class="projectCompatibility"><use xlink:href="#' + projects[i].compatibilities[j] + 'Icon"></use></svg>';
        projectCompatibilitiesBlock.innerHTML = htmlCode;
        rightPartBlock.append(projectCompatibilitiesBlock);

        var reviewsBlock = document.createElement("div");
        reviewsBlock.className = "reviewsBlock";

        var rating = document.createElement("div");
        rating.className = "rating";

        SetFullStar(rating, projects[i].reviewInformation.avgRating);
        var counter = 1;
        while (counter < projects[i].reviewInformation.avgRating) {
            if (projects[i].reviewInformation.avgRating - counter >= 1)
                SetFullStar(rating, projects[i].reviewInformation.avgRating);
            else
                SetPartStar(rating, parseFloat((projects[i].reviewInformation.avgRating - counter).toFixed(1)) * 10, projects[i].reviewInformation.avgRating);

            counter += 1;
        }

        while (counter < 5) {
            SetZeroStar(rating, projects[i].reviewInformation.avgRating);
            counter++;
        }
        reviewsBlock.append(rating);

        var reviewsCount = document.createElement("p");
        reviewsCount.className = "reviewsCount";
        reviewsCount.innerHTML = projects[i].reviewInformation.reviewsCount;
        reviewsBlock.append(reviewsCount);

        rightPartBlock.append(reviewsBlock);

        rightPartWrapper.append(rightPartBlock);

        myProjectBlock.append(rightPartWrapper);

        projectsBlock.append(myProjectBlock);
    }
}

function SetFullStar(rating, ratingValue) {
    var ratingItem = document.createElement("label");
    ratingItem.className = "rating__item";
    $(ratingItem).css({ "color": "gold" });
    ratingItem.innerHTML = '<svg class="rating__star" onmousemove="showTooltip(evt, ' + ratingValue + ');" onmouseout="hideTooltip();"><use xlink:href="#star"></use></svg>';
    rating.append(ratingItem);
}

function SetPartStar(rating, part, ratingValue) {
    var ratingItem = document.createElement("label");
    ratingItem.className = "rating__item";
    ratingItem.innerHTML = '<svg class="rating__star" onmousemove="showTooltip(evt, ' + ratingValue + ');" onmouseout="hideTooltip();"><use xlink:href="#star" fill="url(#' + part + ')"></use></svg>';
    rating.append(ratingItem);
}

function SetZeroStar(rating, ratingValue) {
    var ratingItem = document.createElement("label");
    ratingItem.className = "rating__item";
    ratingItem.innerHTML = '<svg class="rating__star" onmousemove="showTooltip(evt, ' + ratingValue + ');" onmouseout="hideTooltip();"><use xlink:href="#star"></use></svg>';
    rating.append(ratingItem);
}

function SetPagesButtons(pagesCount) {
    var thirdSection = document.getElementById(`thirdSection`);

    var pagesButtonsBlock = document.getElementById(`pagesButtons`);

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
    if (pageButton.id == currentPageId)
        return;

    var currentPageButton = document.getElementById(currentPageId);
    currentPageButton.classList.remove("active");
    pageButton.classList.add("active");
    currentPageId = pageButton.id;

    UncheckFilter();

    isLoad = false;
    Timer();

    $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

    var pageNumber = $(pageButton).val();

    $("div").remove(".myProjectBlock");
    var requestParameters = GetRequestParameters(pageNumber, false);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetProjects?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (projects) {
            SetProjects(projects);

            ViewData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function OpenFilters() {
    $('#filters').css({ "display": "block" });
    $('#thirdSection').css({ "display": "none" });
    $('html, body').animate({ scrollTop: $("#filters").offset().top }, 228);
    document.querySelector('.setFiltersBlock').classList.add("skip");
}

function ThrowOff() {
    notApplyFilters.ProjectTypes = [];
    notApplyFilters.ProjectCategories = [];
    notApplyFilters.ProjectCompatibilities = [];

    UncheckFilter();

    isLoad = false;
    Timer();

    $('body input:checkbox').prop('checked', false);

    $('#throwOff').css({ "display": "none" });
    $('#throwOffSmallView').css({ "display": "none" });
    canThrowOff = false;

    currentPageId = "page1";

    applyFilters = JSON.parse(JSON.stringify(notApplyFilters));

    CheckWidth();

    $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);

    $("div").remove(".myProjectBlock");
    $("p").remove(".pageButton");
    var requestParameters = GetRequestParameters(1, true);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonStore/GetProjects?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (projectsInformation) {
            SetProjects(projectsInformation.projects);

            if (projectsInformation.pagesCountByCurrentFilters > 1)
                SetPagesButtons(projectsInformation.pagesCountByCurrentFilters);

            ViewData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function ViewData() {
    isLoad = true;

    if (isLoadVisible) {
        var preloader = document.querySelector('.reverse-spinner');
        preloader.classList.add('skip');
        isLoadVisible = false;
    }
}

function Timer() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.remove('skip');
        }
        isLoad = false;
    };
};

function CheckWidth() {
    if (isSmall) {
        $('#filters').css({ "display": "none" });
        $('#thirdSection').css({ "display": "block" });
        $('html, body').animate({ scrollTop: $("#filters").offset().top }, 228);
        document.querySelector('.setFiltersBlock').classList.remove("skip");
    }
};

function showTooltip(evt, text) {
    let tooltip = document.getElementById(`tooltip`);
    tooltip.innerHTML = text;
    tooltip.style.display = "block";
    tooltip.style.left = evt.pageX + 10 + 'px';
    tooltip.style.top = evt.pageY - 227 + 'px';
}

function hideTooltip() {
    var tooltip = document.getElementById(`tooltip`);
    tooltip.style.display = "none";
}

function OpenProject(projectId) {
    var url = "Project?projectId=" + projectId;
    $(location).attr('href', url);
}

function CloseFilters() {
    $('#filters').css({ "display": "none" });
    $('#thirdSection').css({ "display": "flex" });
    $('html, body').animate({ scrollTop: $("#thirdSection").offset().top }, 228);
    document.querySelector('.setFiltersBlock').classList.remove("skip");
}