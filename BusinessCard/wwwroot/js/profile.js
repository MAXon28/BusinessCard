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

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            document.querySelector('.userCard').style.display = "";
            window.setTimeout(show_user_card, 107);
            function show_user_card() {
                document.querySelector('.userCard').style.opacity = '1';
            };

            document.querySelector('.applicationCard').style.display = "";
            window.setTimeout(show_application_card, 328);
            function show_application_card() {
                document.querySelector('.applicationCard').style.opacity = '1';
            };

            document.querySelector('.blogCard').style.display = "";
            window.setTimeout(show_blog_card, 526);
            function show_blog_card() {
                document.querySelector('.blogCard').style.opacity = '1';
            };

            GetUserTasks(userData.Id);
            GetUserDataInBlog(userData.Id);
        },
        error: function (error) {
            alert(error);
        }
    });
}

function GetUserTasks(userId) {
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
        url: "/MAXonTask/GetUserTasksStatistic",
        data: { userId },
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

function GetUserDataInBlog(userId) {
    var isLoadUserInBlog = false;
    var isLoadUserInBlogVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadUserInBlog) {
            isLoadUserInBlogVisible = true;
            $('#preloaderBlog').fadeIn();
        }
        isLoad = false;
    };
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonBlog/GetUserStatistic",
        data: { userId },
        success: function (data) {
            var jsonData = $.parseJSON(data);
            blogStatistic = jsonData.BlogStatistic;

            document.getElementById('channelSubscriptions').innerHTML = "Количество подписок на каналы: <span>" + blogStatistic.ChannelSubscriptionsCount + "</span>";
            document.getElementById('mailingSubscriptions').innerHTML = "Количество подписок на рассылки: <span>" + blogStatistic.MailingSubscriptionsCount + "</span>";
            document.getElementById('bookmarks').innerHTML = "Количество постов в закладках: <span>" + blogStatistic.BookmarksCount + "</span>";

            isLoadUserInBlog = true;
            if (isLoadUserInBlogVisible) {
                $('#preloaderBlog').fadeOut();
            }

            $('#channelSubscriptions').fadeIn();
            $('#mailingSubscriptions').fadeIn();
            $('#bookmarks').fadeIn();

        },
        error: function (error) {
            alert(error);
        }
    });
}

function GoToDetail() {
    $(location).attr('href', 'Detail');
}

function GoToBlog() {
    $(location).attr('/', 'MAXonBlog', 'Posts');
}