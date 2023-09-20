window.onload = init;
//var typeOfStatus = "ALL";
var searchText = "";
var lastVacancyId = -1;
var packagesCount = 1;
var currentPackage = 1;
var vacanciesId = [];

function init() {
    var isLoad = false;
    var isLoadVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            isLoadVisible = true;
            $('.reverse-spinner').fadeIn();
        }
        isLoad = false;
    };
    var requestParameters = GetRequestParameters(true);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetVacanciesInfo?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var vacanciesBlock = document.querySelector('.vacanciesBlock');
            var vacanciesCount = jsonData.VacanciesCount;
            if (vacanciesCount == 1)
                vacanciesBlock.classList.add('oneVacancyBlock');
            else if (vacanciesCount == 2)
                vacanciesBlock.classList.add('twoVacanciesBlock');
            else
                vacanciesBlock.classList.add('manyVacanciesBlock');
            packagesCount = jsonData.PackagesCount;

            SetVacancies(jsonData.VacanciesInfo);

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            $('.filterBlock').fadeIn();

            ViewVacancies();

            $('html, body').animate({ scrollTop: $(".vacanciesBlock").offset().top }, 228);
        },
        error: function (error) {
            alert(error);
            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }
        }
    });
}

$(document).ready(function () {
    $('#search').keydown(function (e) {
        if (e.keyCode === 13)
            SearchTasks();
    });
});

function SearchTasks() {
    if ($('#search').val() == searchText)
        return;

    $('.viewElse').fadeOut();

    lastVacancyId = -1;
    packagesCount = 1;
    currentPackage = 1;

    searchText = $('#search').val();

    $("div").remove(".vacancyBlock");

    var isLoadSearch = false;
    var isLoadSearchVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadSearch) {
            isLoadSearchVisible = true;
            $('.reverse-spinner').fadeIn();
        }
        isLoadSearch = false;
    };
    $('html, body').animate({ scrollTop: $(".mainBlock").offset().top }, 228);
    var requestParameters = GetRequestParameters(true);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetVacanciesInfo?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            var vacanciesBlock = document.querySelector('.vacanciesBlock');
            vacanciesBlock.classList.remove('oneVacancyBlock');
            vacanciesBlock.classList.remove('twoVacanciesBlock');
            vacanciesBlock.classList.remove('manyVacanciesBlock');
            var jsonData = $.parseJSON(data);
            var vacanciesCount = jsonData.VacanciesCount;
            if (vacanciesCount == 1)
                vacanciesBlock.classList.add('oneVacancyBlock');
            else if (vacanciesCount == 2)
                vacanciesBlock.classList.add('twoVacanciesBlock');
            else
                vacanciesBlock.classList.add('manyVacanciesBlock');
            packagesCount = jsonData.PackagesCount;

            SetVacancies(jsonData.VacanciesInfo);

            isLoadSearch = true;
            if (isLoadSearchVisible) {
                $('.reverse-spinner').fadeOut();
            }

            ViewVacancies();

            $('html, body').animate({ scrollTop: $(".vacanciesBlock").offset().top }, 228);
        },
        error: function (error) {
            alert(error);
            isLoadSearch = true;
            if (isLoadSearchVisible) {
                $('.reverse-spinner').fadeOut();
            }
        }
    });
}

function ViewElse() {
    $('.viewElse').fadeOut();

    var isLoadElse = false;
    var isLoadElseVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadElse) {
            isLoadElseVisible = true;
            $('#elsePreloader').fadeIn();
        }
        isLoadElse = false;
    };
    //$('html, body').animate({ scrollTop: $(".mainBlock").offset().top }, 228);
    var requestParameters = GetRequestParameters(false);
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetVacanciesInfo?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            currentPackage += 1;

            SetVacancies($.parseJSON(data).VacanciesInfo);

            isLoadElse = true;
            if (isLoadElseVisible) {
                $('#elsePreloader').fadeOut();
            }

            ViewVacancies();
        },
        error: function (error) {
            alert(error);
            isLoadElse = true;
            if (isLoadElseVisible) {
                $('#elsePreloader').fadeOut();
            }
        }
    });
}

function GetRequestParameters(needUpdatePackagesCount) {
    var requestParameters = "";
    requestParameters += "LastVacancyId=" + lastVacancyId;
    requestParameters += "&SearchText=" + searchText;
    requestParameters += "&NeedPackagesCount=" + needUpdatePackagesCount;
    return requestParameters;
}

function SetVacancies(vacancies) {
    var vacanciesBlock = document.querySelector('.vacanciesBlock');
    for (var i = 0; i < vacancies.length; i++) {
        var vacancyBlock = document.createElement("div");
        vacancyBlock.id = "vacancy" + vacancies[i].Id;
        vacancyBlock.className = "vacancyBlock";
        vacancyBlock.setAttribute("onclick", 'GoToVacancy("' + vacancies[i].Id + '")');
        $(vacancyBlock).css('display', 'none');

        var companyName = document.createElement("p");
        companyName.className = "companyName";
        companyName.innerHTML = vacancies[i].Company;

        var vacancyName = document.createElement("p");
        vacancyName.className = "vacancyName";
        vacancyName.innerHTML = vacancies[i].Name;

        var salary = document.createElement("p");
        salary.className = "salary";
        salary.innerHTML = vacancies[i].Salary;

        var creationDate = document.createElement("p");
        creationDate.className = "creationDate";
        creationDate.innerHTML = vacancies[i].CreationDate;

        vacancyBlock.append(companyName);
        vacancyBlock.append(vacancyName);
        vacancyBlock.append(salary);
        vacancyBlock.append(creationDate);

        if (vacancies[i].ViewedByMAXon28Team == false) {
            var indicator = document.createElement("div");
            indicator.className = "indicator";

            var text = document.createElement("div");
            text.className = "text";
            text.innerHTML = "Новая";

            indicator.append(text);

            vacancyBlock.append(indicator);
        }

        vacanciesId.push(vacancyBlock.id);

        if (i + 1 == vacancies.length)
            lastVacancyId = vacancies[i].Id;

        vacanciesBlock.append(vacancyBlock);
    }

    if (currentPackage < packagesCount)
        $('.viewElse').fadeIn();
}

function ViewVacancies() {
    for (var i = 0; i < vacanciesId.length; i++)
        $('#' + vacanciesId[i]).fadeIn();
    vacanciesId = [];
}

function GoToVacancy(vacancyUrl) {
    var url = "Vacancy/" + vacancyUrl;
    $(location).attr('href', url);
}