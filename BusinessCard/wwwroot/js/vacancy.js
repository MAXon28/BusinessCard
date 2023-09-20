window.onload = init;

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

    hrefParts = $(location).attr('href').split("/");
    vacancyId = hrefParts[hrefParts.length - 1];

    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetVacancy?id=" + vacancyId,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            vacancy = jsonData.Vacancy;

            document.getElementById('vacancyName').innerHTML = vacancy.Name;
            document.getElementById('vacancySalary').innerHTML = vacancy.Salary;
            document.getElementById('vacancyDate').innerHTML = vacancy.CreationDate;
            document.getElementById('company').innerHTML = vacancy.Company;
            document.getElementById('contact').innerHTML = vacancy.Contact;
            document.getElementById('position').innerHTML = vacancy.Position;
            document.getElementById('workFormat').innerHTML = vacancy.WorkFormat;
            if (vacancy.Description != null && vacancy.Description != "") {
                document.getElementById('vacancyDescription').innerHTML = vacancy.Description;
                $('.descriptionBlock').fadeIn();
            }

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            $('.vacancyBlock').fadeIn();

            $('html, body').animate({ scrollTop: $(".vacancyBlock").offset().top }, 228);
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