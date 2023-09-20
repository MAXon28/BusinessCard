window.onload = init;
var typeOfStatus = "ALL";
var searchText = "";
var lastTaskId = -1;
var packagesCount = 1;
var currentPackage = 1;
var tasksId = [];

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
        url: "/MAXonTask/GetUserTaskHistory?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var tasksHistory = jsonData.TasksHistory;
            var tasksBlock = document.querySelector('.tasksBlock');
            var tasksCount = tasksHistory.TasksCount;
            if (tasksCount == 1)
                tasksBlock.classList.add('oneTaskBlock');
            else if (tasksCount == 2)
                tasksBlock.classList.add('twoTasksBlock');
            else
                tasksBlock.classList.add('manyTasksBlock');
            packagesCount = tasksHistory.TasksPackageCount;

            SetTasks(tasksHistory.Tasks);

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            $('.filterBlock').fadeIn();

            ViewTasks();

            $('html, body').animate({ scrollTop: $(".tasksBlock").offset().top }, 228);
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

function SelectStatus(selectTypeOfStatus) {
    if (typeOfStatus != selectTypeOfStatus) {
        $('.viewElse').fadeOut();

        typeOfStatus = selectTypeOfStatus;
        lastTaskId = -1;
        packagesCount = 1;
        currentPackage = 1;

        $("div").remove(".taskBlock");

        var isLoadStatus = false;
        var isLoadStatusVisible = false;

        window.setTimeout(show_preloader, 428);
        function show_preloader() {
            if (!isLoadStatus) {
                isLoadStatusVisible = true;
                $('.reverse-spinner').fadeIn();
            }
            isLoadStatus = false;
        };
        $('html, body').animate({ scrollTop: $(".mainBlock").offset().top }, 228);
        var requestParameters = GetRequestParameters(true);
        $.ajax({
            async: true,
            type: "GET",
            url: "/MAXonTask/GetUserTaskHistory?" + requestParameters,
            contentType: 'application/json',
            dataTpye: "json",
            traditional: true,
            success: function (data) {
                var jsonData = $.parseJSON(data);
                var tasksHistory = jsonData.TasksHistory;
                var tasksBlock = document.querySelector('.tasksBlock');
                tasksBlock.classList.remove('oneTaskBlock');
                tasksBlock.classList.remove('twoTasksBlock');
                tasksBlock.classList.remove('manyTasksBlock');
                var tasksCount = tasksHistory.TasksCount;
                if (tasksCount == 1)
                    tasksBlock.classList.add('oneTaskBlock');
                else if (tasksCount == 2)
                    tasksBlock.classList.add('twoTasksBlock');
                else
                    tasksBlock.classList.add('manyTasksBlock');
                packagesCount = tasksHistory.TasksPackageCount;

                SetTasks(tasksHistory.Tasks);

                isLoadStatus = true;
                if (isLoadStatusVisible) {
                    $('.reverse-spinner').fadeOut();
                }

                ViewTasks();

                $('html, body').animate({ scrollTop: $(".tasksBlock").offset().top }, 228);
            },
            error: function (error) {
                alert(error);
                isLoadStatus = true;
                if (isLoadStatusVisible) {
                    $('.reverse-spinner').fadeOut();
                }
            }
        });
    }
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

    lastTaskId = -1;
    packagesCount = 1;
    currentPackage = 1;

    searchText = $('#search').val();

    $("div").remove(".taskBlock");

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
        url: "/MAXonTask/GetUserTaskHistory?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var tasksHistory = jsonData.TasksHistory;
            var tasksBlock = document.querySelector('.tasksBlock');
            tasksBlock.classList.remove('oneTaskBlock');
            tasksBlock.classList.remove('twoTasksBlock');
            tasksBlock.classList.remove('manyTasksBlock');
            var tasksCount = tasksHistory.TasksCount;
            if (tasksCount == 1)
                tasksBlock.classList.add('oneTaskBlock');
            else if (tasksCount == 2)
                tasksBlock.classList.add('twoTasksBlock');
            else
                tasksBlock.classList.add('manyTasksBlock');
            packagesCount = tasksHistory.TasksPackageCount;

            SetTasks(tasksHistory.Tasks);

            isLoadSearch = true;
            if (isLoadSearchVisible) {
                $('.reverse-spinner').fadeOut();
            }

            ViewTasks();

            $('html, body').animate({ scrollTop: $(".tasksBlock").offset().top }, 228);
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
        url: "/MAXonTask/GetUserTaskHistory?" + requestParameters,
        contentType: 'application/json',
        dataTpye: "json",
        traditional: true,
        success: function (data) {
            currentPackage += 1;

            var jsonData = $.parseJSON(data);
            var tasksHistory = jsonData.TasksHistory;

            SetTasks(tasksHistory.Tasks);

            isLoadElse = true;
            if (isLoadElseVisible) {
                $('#elsePreloader').fadeOut();
            }

            ViewTasks();

            //$('html, body').animate({ scrollTop: $(".tasksBlock").offset().top }, 228);
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

function GetRequestParameters(needUpdatePackagesCount) {
    var requestParameters = "";
    requestParameters += "LastTaskId=" + lastTaskId;
    requestParameters += "&TypeOfStatus=" + typeOfStatus;
    requestParameters += "&SearchText=" + searchText;
    requestParameters += "&NeedPackagesCount=" + needUpdatePackagesCount;
    return requestParameters;
}

function SetTasks(tasks) {
    var tasksBlock = document.querySelector('.tasksBlock');
    for (var i = 0; i < tasks.length; i++) {
        var taskBlock = document.createElement("div");
        taskBlock.id = "task" + tasks[i].Id;
        taskBlock.className = "taskBlock";
        taskBlock.setAttribute("onclick", 'GoToTask("' + tasks[i].Url + '")');
        $(taskBlock).css('display', 'none');

        var taskReceipt = document.createElement("p");
        taskReceipt.className = "taskReceipt";
        taskReceipt.innerHTML = tasks[i].Receipt;

        var taskName = document.createElement("p");
        taskName.className = "taskName";
        taskName.innerHTML = tasks[i].Name;

        var sectionBlock = document.createElement("div");
        sectionBlock.className = "sectionBlock";

        var leftBlock = document.createElement("div");
        leftBlock.className = "detailBlock rightMargin";

        var taskCreationDate = document.createElement("p");
        taskCreationDate.className = "taskDate";
        taskCreationDate.innerHTML = 'Создано: <span style="color: white">' + tasks[i].CreationDate + '</span>';

        var taskUpdateDate = document.createElement("p");
        taskUpdateDate.className = "taskDate";
        taskUpdateDate.innerHTML = 'Обновлено: <span style="color: white">' + tasks[i].UpdateDate + '</span>';

        var taskStatus = document.createElement("p");
        taskStatus.className = "taskStatus";
        switch (tasks[i].StatusDetail.StatusType) {
            case 'PROCESS':
                taskStatus.innerHTML = 'Статус задачи: <span class="processStatus">' + tasks[i].StatusDetail.StatusText + '</span>';
                break;
            case 'DONE':
                taskStatus.innerHTML = 'Статус задачи: <span class="doneStatus">' + tasks[i].StatusDetail.StatusText + '</span>';
                break;
            case 'REJECTED':
                taskStatus.innerHTML = 'Статус задачи: <span class="rejectedStatus">' + tasks[i].StatusDetail.StatusText + '</span>';
                break;
        }

        leftBlock.append(taskCreationDate);
        leftBlock.append(taskUpdateDate);
        leftBlock.append(taskStatus);

        var rightBlock = document.createElement("div");
        rightBlock.className = "detailBlock rightBlock";

        var taskPrice = document.createElement("p");
        taskPrice.className = "taskPrice";
        var price = tasks[i].Price;
        if (price > -1)
            taskPrice.innerHTML = price + ' ₽';
        else
            taskPrice.innerHTML = '—';

        rightBlock.append(taskPrice);

        sectionBlock.append(leftBlock);
        sectionBlock.append(rightBlock);

        if (tasks[i].UnreadRecordsCount > 0) {
            var indicator = document.createElement("div");
            indicator.className = "indicator";

            var count = document.createElement("div");
            count.className = "count";
            count.innerHTML = "+" + tasks[i].UnreadRecordsCount;

            indicator.append(count);

            taskBlock.append(indicator);
        }

        taskBlock.append(taskReceipt);
        taskBlock.append(taskName);
        taskBlock.append(sectionBlock);

        tasksId.push(taskBlock.id);

        if (i + 1 == tasks.length)
            lastTaskId = tasks[i].Id;

        tasksBlock.append(taskBlock);
    }

    if (currentPackage < packagesCount)
        $('.viewElse').fadeIn();
}

function ViewTasks() {
    for (var i = 0; i < tasksId.length; i++)
        $('#' + tasksId[i]).fadeIn();
    tasksId = [];
}

function GoToTask(taskUrl) {
    var url = "Task/" + taskUrl;
    $(location).attr('href', url);
}