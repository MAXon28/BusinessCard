window.onload = init;
var role = "";

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
    $.ajax({
        async: true,
        type: "GET",
        url: "/Account/GetSmallUserData",
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            userData = jsonData.UserData;

            $('.skillPercentOfKnowledge').text(userData.UserAbbreviation);
            $('#userName').text(userData.Surname + " " + userData.Name);
            $('#userRole').text(userData.RoleName);
            role = userData.RoleName;

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            document.querySelector('.userCard').style.display = "";
            window.setTimeout(show_user_card, 107);
            function show_user_card() {
                document.querySelector('.userCard').style.opacity = '1';
            };

            document.querySelector('.workCard').style.display = "";
            window.setTimeout(show_word_card, 526);
            function show_word_card() {
                document.querySelector('.workCard').style.opacity = '1';
            };

            document.querySelector('.applicationCard').style.display = "";
            window.setTimeout(show_application_card, 328);
            function show_application_card() {
                document.querySelector('.applicationCard').style.opacity = '1';
            };

            GetVacanciesData();
            GetTasksData();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GetVacanciesData() {
    var isLoadVacanciesStatistic = false;
    var isLoadVacanciesStatisticVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadVacanciesStatistic) {
            isLoadVacanciesStatisticVisible = true;
            $('#preloaderVacancies').fadeIn();
        }
        isLoad = false;
    };
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetVacanciesStatistic",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            vacanciesStatistic = jsonData.VacanciesStatistic;

            document.getElementById('vacanciesCount').innerHTML = "Количество полученных вакансий: <span>" + vacanciesStatistic.VacanciesCount + "</span>";
            document.getElementById('newVacanciesCount').innerHTML = "+" + vacanciesStatistic.VacanciesNotViewedCount;

            isLoadVacanciesStatistic = true;
            if (isLoadVacanciesStatisticVisible) {
                $('#preloaderVacancies').fadeOut();
            }

            $('#vacanciesCount').fadeIn();
            if (vacanciesStatistic.VacanciesNotViewedCount > 0)
                $('#workIndicator').fadeIn();

        },
        error: function (error) {
            alert(error);
        }
    });
}

function GetTasksData() {
    var isLoadUserTasks = false;
    var isLoadUserTasksVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadUserTasks) {
            isLoadUserTasksVisible = true;
            $('#preloaderTasks').fadeIn();
        }
        isLoad = false;
    };
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonTask/GetTasksStatistic",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            tasksStatistic = jsonData.TasksStatistic;

            document.getElementById('processTasksCount').innerHTML = "Количество задач в работе: <span>" + tasksStatistic.ProcessCount + "</span>";
            document.getElementById('doneTasksCount').innerHTML = "Количество выполненных задач: <span>" + tasksStatistic.DoneCount + "</span>";
            document.getElementById('rejectedTasksCount').innerHTML = "Количество отклонённых задач: <span>" + tasksStatistic.RejectedCount + "</span>";
            document.getElementById('newTasksCount').innerHTML = "+" + tasksStatistic.UnreadCount;

            isLoadUserTasks = true;
            if (isLoadUserTasksVisible) {
                $('#preloaderTasks').fadeOut();
            }

            $('#processTasksCount').fadeIn();
            $('#doneTasksCount').fadeIn();
            $('#rejectedTasksCount').fadeIn();
            if (tasksStatistic.UnreadCount > 0)
                $('#applicationIndicator').fadeIn();

        },
        error: function (error) {
            alert(error);
        }
    });
}

function GoToDetail() {
    if (role == "Администратор")
        $(location).attr('/', 'Account', 'Detail');
    else
        $(location).attr('href', 'MAXon28Profile');
}