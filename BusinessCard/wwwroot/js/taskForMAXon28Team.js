window.onload = init;
var taskUrl = "";
var taskId = -1;
var statusText = "";
var taskStatusesData = [];

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
    taskUrl = hrefParts[hrefParts.length - 1];

    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonTask/GetTask?taskUrl=" + taskUrl,
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            taskInfo = jsonData.TaskInfo;

            var taskData = taskInfo.Data;

            taskId = taskInfo.Data.Id;

            document.querySelector('.taskReceipt').innerHTML = taskData.Receipt;
            document.querySelector('.taskName').innerHTML = taskData.Name;

            $('#service').find('.taskData').html(taskData.ServiceName);

            if (taskData.RateName != null && taskData.RateName != "") {
                $('#rate').find('.taskData').html(taskData.RateName);
                $('#rate').fadeIn();
            }

            if (taskData.TechnicalSpecificationFilePath != null && taskData.TechnicalSpecificationFilePath != "") {
                $('#technicalSpecificationFile').find('a').attr('href', taskData.TechnicalSpecificationFilePath);
                $('#technicalSpecificationFile').find('a').attr('download', taskData.TechnicalSpecificationFileNameForUser);
                $('#technicalSpecificationFile').fadeIn();
            }

            if (taskData.DoneTaskFilePath != null && taskData.DoneTaskFilePath != "") {
                $('#doneTaskFile').find('a').attr('href', taskData.DoneTaskFilePath);
                $('#doneTaskFile').find('a').attr('download', taskData.DoneTaskFileNameForUser);
                $('#doneTaskFile').fadeIn();
            }

            if (taskData.SuggestedPrice != null && taskData.SuggestedPrice != "") {
                $('#suggestedPrice').find('.taskData').html(taskData.SuggestedPrice);
                $('#suggestedPrice').fadeIn();
            }

            if (taskData.Deadline != "") {
                $('#deadline').find('.taskData').html(taskData.Deadline);
                $('#deadline').fadeIn();
            }

            $('#creationDate').find('.taskData').html(taskData.CreationDate);

            if (taskData.UpdateDate != "" && taskData.UpdateDate != taskData.CreationDate) {
                $('#updateDate').find('.taskData').html(taskData.UpdateDate);
                $('#updateDate').fadeIn();
            }

            var taskStatus = document.querySelector('.taskStatus');
            taskStatus.innerHTML = taskData.StatusDetail.StatusText;
            statusText = taskData.StatusDetail.StatusText;
            switch (taskData.StatusDetail.StatusType) {
                case 'PROCESS':
                    taskStatus.classList.add('processStatus');
                    break;
                case 'DONE':
                    taskStatus.classList.add('doneStatus');
                    break;
                case 'REJECTED':
                    taskStatus.classList.add('rejectedStatus');
            }

            if (taskData.StatusDetail.StatusType == 'PROCESS' && taskData.TechnicalSpecification != null && taskData.TechnicalSpecification != "") {
                $('.technicalSpecificationText').append(taskData.TechnicalSpecification);
                $('#technicalSpecification').fadeIn();
            }

            if (taskData.Price != -1) {
                var priceStr = taskData.Price.toString() + " ₽";
                document.querySelector('.fixedPrice').innerHTML = priceStr;
            }
            else {
                document.querySelector('.fixedPrice').innerHTML = "Цена устанавливается";
                if (taskData.StatusDetail.StatusType == "PROCESS")
                    $('.addPrice').fadeIn();
            }

            if (taskData.StatusDetail.StatusType == "PROCESS")
                $('.updateStatus').fadeIn();

            if ((taskData.DoneTaskFilePath == null || taskData.DoneTaskFilePath == "") && taskData.StatusDetail.StatusType == "PROCESS")
                $('.addDoneTaskFile').fadeIn();

            if (taskData.StatusDetail.StatusType == "PROCESS" && taskInfo.HaveCounter)
                $('.addTaskCounterRecord').fadeIn();

            var personalInfo = taskInfo.PersonalInfo;
            document.getElementById('name').innerHTML = personalInfo.FullName;
            document.getElementById('email').innerHTML = personalInfo.UserEmail;
            document.getElementById('phoneNumber').innerHTML = personalInfo.UserPhoneNumber;
            document.getElementById('connection').innerHTML = personalInfo.Connection;

            var taskRecords = document.querySelector('.recordsBlock');
            var records = taskInfo.Records;
            for (var i = 0; i < records.length; i++) {
                AddRecord(taskRecords, records[i].IsMAXonTeam, records[i].Text, records[i].Role, records[i].CreationDate);
            }

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            $('.taskInformation').fadeIn();
            $('.taskRecords').fadeIn();

            $('html, body').animate({ scrollTop: $(".taskInformation").offset().top }, 228);
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

function UpdateStatus() {
    taskStatusesData = [];

    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "addData";
    fullWindow.className = "fullWindow";

    html.append(fullWindow);

    var isLoadStatuses = false;
    var isLoadStatusesVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadStatuses) {
            isLoadStatusesVisible = true;
            fullWindow.innerHTML = '<div id="taskStatusesLoader" style="display: none; width: 71px; margin-top: 307px;" class="preloader"><svg style="width: 71px; height: 71px; color: #87CEFA" class="preloader__image" role="img" xmlns="http://www.w3.org/2000/svg" viewBox ="0 0 512 512"><path fill="currentColor" d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>';
            $('#taskStatusesLoader').fadeIn();
        }
        isLoadStatuses = false;
    };

    $.ajax({
        async: true,
        type: "GET",
        url: "/TaskStatus/GetTaskStatuses",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            statuses = jsonData.Statuses;

            var htmlCode = '<div class="statusesBlock"><select id="statuses" class="toolbar-font">';
            for (var i = 0; i < statuses.length; i++) {
                var statusClass = "";
                taskStatusesData.push(statuses[i].Id);
                switch (statuses[i].StatusType) {
                    case 'PROCESS':
                        taskStatusesData[statuses[i].Id] = 'PROCESS';
                        statusClass = 'processInSelector';
                        break;
                    case 'DONE':
                        taskStatusesData[statuses[i].Id] = 'DONE';
                        statusClass = 'doneStatus';
                        break;
                    case 'REJECTED':
                        taskStatusesData[statuses[i].Id] = 'REJECTED';
                        statusClass = 'rejectedStatus';
                }
                var selected = "";
                if (statuses[i].StatusText == statusText)
                    selected = "selected";
                htmlCode += '<option class="' + statusClass + '" value="' + statuses[i].Id + '" ' + selected + '>' + statuses[i].StatusText + '</option>';
            }
            htmlCode += '</select></div>';

            var skipButton = document.createElement("p");
            skipButton.className = "skipButton";
            skipButton.innerHTML = "×";
            skipButton.setAttribute("onclick", "CloseForm()");

            var container = document.createElement("div");
            container.className = "container";

            var frame = document.createElement("div");
            frame.className = "frameFullWindow";

            var inputsData = document.createElement("div");
            inputsData.id = "inputs";
            inputsData.className = "inputsData";

            var formButton = document.createElement("button");
            formButton.id = "btnSaveTaskStatus";
            formButton.className = "btn-saveData";
            formButton.setAttribute("onclick", "SaveTaskStatus()");
            formButton.innerHTML = 'Сохранить';

            var updateDataSuccess = document.createElement("p");
            updateDataSuccess.id = "updateDataSuccess";
            updateDataSuccess.className = "successMessage";
            updateDataSuccess.innerHTML = 'Статус задачи обновлён!';

            var updateDataError = document.createElement("p");
            updateDataError.id = "updateDataError";
            updateDataError.className = "errorMessage";
            updateDataError.innerHTML = 'Ошибка при обновлении статуса задачи!';

            inputsData.innerHTML = htmlCode;
            inputsData.append(formButton);
            inputsData.append(updateDataSuccess);
            inputsData.append(updateDataError);

            frame.append(inputsData);

            container.append(frame);

            isLoadStatuses = true;
            if (isLoadStatusesVisible) {
                $('#taskStatusesLoader').fadeOut();
            }

            fullWindow.append(skipButton);
            fullWindow.append(container);
        },
        error: function (error) {
            alert(error);
            isLoadStatuses = true;
            if (isLoadStatusesVisible) {
                $('#taskStatusesLoader').fadeOut();
            }
            CloseForm();
        }
    });
}

function SaveTaskStatus() {
    $('#updateDataError').fadeOut();

    var statusId = $('#statuses').val();
    var status = $('#statuses option:selected').text();

    if (status == statusText) {
        CloseForm();
        return;
    }

    $('#btnSaveTaskStatus').fadeOut();

    var isLoadNewStatuses = false;
    var isLoadNewStatusesVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadNewStatuses) {
            isLoadNewStatusesVisible = true;
            $('.btn-saveData').fadeOut();
            $(".inputsData").append('<div id="preloaderUpdateData" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px; margin-top: 12px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadNewStatuses = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonTask/UpdateTaskStatus",
        data: { taskId, taskUrl, statusId, status },
        dataTpye: "json",
        success: function (recordText) {
            $('#updateDataSuccess').show('slow');
            statusText = status;

            var typeOfStatus = taskStatusesData[statusId];
            var taskStatus = document.querySelector('.taskStatus');
            taskStatus.classList.remove('processStatus');
            taskStatus.classList.remove('doneStatus');
            taskStatus.classList.remove('rejectedStatus');
            taskStatus.innerHTML = statusText;
            switch (typeOfStatus) {
                case 'PROCESS':
                    taskStatus.classList.add('processStatus');
                    break;
                case 'DONE':
                    $('.updateStatus').fadeOut();
                    $('.addDoneTaskFile').fadeOut();
                    taskStatus.classList.add('doneStatus');
                    break;
                case 'REJECTED':        
                    $('.updateStatus').fadeOut();
                    $('.addDoneTaskFile').fadeOut();
                    taskStatus.classList.add('rejectedStatus');
            }

            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, true, recordText, 'Команда MAXon28', 'Сейчас');

            window.setTimeout(close_status_update_form, 3001);
            function close_status_update_form() {
                CloseForm();
            };
            isLoadNewStatuses = true;
            if (isLoadNewStatusesVisible) {
                $('#preloaderUpdateData').fadeOut();
            }
        },
        error: function (error) {
            alert(error);
            $('#updateDataError').show('slow');
            isLoadStatuses = true;
            if (isLoadNewStatusesVisible) {
                $('#preloaderUpdateData').fadeOut();
                $('.btn-saveData').fadeIn();
            }
            CloseForm();
        }
    });
}

function AddPrice() {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "addData";
    fullWindow.className = "fullWindow";

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseForm()");

    var container = document.createElement("div");
    container.className = "container";

    var frame = document.createElement("div");
    frame.className = "frameFullWindow";

    var inputsData = document.createElement("div");
    inputsData.id = "inputs";
    inputsData.className = "inputsData";

    var formButton = document.createElement("button");
    formButton.id = "btnSavePrice";
    formButton.className = "btn-saveData";
    formButton.setAttribute("onclick", "SavePrice()");
    formButton.innerHTML = 'Сохранить';

    var updateDataSuccess = document.createElement("p");
    updateDataSuccess.id = "updateDataSuccess";
    updateDataSuccess.className = "successMessage";
    updateDataSuccess.innerHTML = 'Цена успешно установлена!';

    var updateDataError = document.createElement("p");
    updateDataError.id = "updateDataError";
    updateDataError.className = "errorMessage";
    updateDataError.innerHTML = 'Ошибка при установлении цены!';

    inputsData.innerHTML = '<div class="inputbox"><input id="price" class="input" type="number" required="required" autocomplete="off"><span class="placeholder">Цена</span></div>';
    inputsData.append(formButton);
    inputsData.append(updateDataSuccess);
    inputsData.append(updateDataError);

    frame.append(inputsData);

    container.append(frame);

    fullWindow.append(skipButton);
    fullWindow.append(container);
    html.append(fullWindow);
}

function SavePrice() {
    $('#updateDataError').fadeOut();

    var priceStr = $('#price').val();

    if (priceStr == "") {
        CloseForm();
        return;
    }

    var price = parseInt(priceStr);

    $('#btnSavePrice').fadeOut();

    var isLoadPrice = false;
    var isLoadPriceVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadPrice) {
            isLoadPriceVisible = true;
            $('.btn-saveData').fadeOut();
            $(".inputsData").append('<div id="preloaderUpdateData" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px; margin-bottom: 12px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadPrice = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonTask/AddTaskPrice",
        data: { taskId, taskUrl, price },
        dataTpye: "json",
        success: function (recordText) {
            $('#updateDataSuccess').show('slow');

            document.querySelector('.fixedPrice').innerHTML = price + ' ₽';

            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, true, recordText, 'Команда MAXon28', 'Сейчас');

            $('.addPrice').fadeOut();

            window.setTimeout(close_price_addition_form, 3001);
            function close_price_addition_form() {
                CloseForm();
            };
            isLoadPrice = true;
            if (isLoadPriceVisible) {
                $('#preloaderUpdateData').fadeOut();
            }
        },
        error: function (error) {
            alert(error);
            $('#updateDataError').show('slow');
            isLoadPrice = true;
            if (isLoadPriceVisible) {
                $('#preloaderUpdateData').fadeOut();
                $('.btn-saveData').fadeIn();
            }
            CloseForm();
        }
    });
}

function AddDoneTaskFile() {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "addData";
    fullWindow.className = "fullWindow";

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseForm()");

    var container = document.createElement("div");
    container.className = "container";

    var frame = document.createElement("div");
    frame.className = "frameFullWindow";

    var inputsData = document.createElement("div");
    inputsData.id = "inputs";
    inputsData.className = "inputsData";

    var formButton = document.createElement("button");
    formButton.id = "btnSaveFile";
    formButton.className = "btn-saveData";
    formButton.setAttribute("onclick", "SaveFile()");
    formButton.innerHTML = 'Сохранить';

    var updateDataSuccess = document.createElement("p");
    updateDataSuccess.id = "updateDataSuccess";
    updateDataSuccess.className = "successMessage";
    updateDataSuccess.innerHTML = 'Файл успешно добавлен!';

    var updateDataError = document.createElement("p");
    updateDataError.id = "updateDataError";
    updateDataError.className = "errorMessage";
    updateDataError.innerHTML = 'Ошибка при добавлении файла!';

    inputsData.innerHTML = '<div class="field__wrapper"><input id="file" type="file" name="file" class="field__file" style="display: none" accept=".jpg,.jpeg,.png,.doc,.docx,.txt,.pdf,.zip,.rar,.7z"><label class="field__file-wrapper" for="file"><p class="field__file-fake">Прикрепите файл задачи</p><p class="field__file-button">Прикрепить</p></label></div>';
    inputsData.append(formButton);
    inputsData.append(updateDataSuccess);
    inputsData.append(updateDataError);

    frame.append(inputsData);

    container.append(frame);

    fullWindow.append(skipButton);
    fullWindow.append(container);
    html.append(fullWindow);

    SetSelectFiles();
}

function SetSelectFiles() {
    let fields = document.querySelectorAll('.field__file');
    Array.prototype.forEach.call(fields, function (input) {
        let label = input.nextElementSibling,
            labelVal = label.querySelector('.field__file-fake').innerText;

        input.addEventListener('change', function (e) {
            let countFiles = '';
            if (this.files && this.files.length >= 1)
                countFiles = this.files.length;

            if (countFiles) {
                var file = document.getElementById('file').value;

                file = file.split('\\').pop();

                label.querySelector('.field__file-fake').innerText = file;
                $('.field__file-fake').css('color', '#211f30');
            } else {
                label.querySelector('.field__file-fake').innerText = labelVal;
                $('.field__file-fake').css('color', '#e0e0e0');
            }
        });
    });
}

function SaveFile() {
    var files = document.getElementById("file").files;
    if (files.length == 0)
        return;

    $('#updateDataError').fadeOut();

    $('#btnSaveFile').fadeOut();

    var isLoadFile = false;
    var isLoadFileVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadFile) {
            isLoadFileVisible = true;
            $('.btn-saveData').fadeOut();
            $(".inputsData").append('<div id="preloaderUpdateData" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px; margin-top: 12px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadFile = false;
    };

    let data = new FormData();
    data.append("taskId", taskId);
    data.append("taskUrl", taskUrl);
    data.append("file", files[0]);
    $.ajax({
        type: "POST",
        url: "/MAXonTask/AddDoneTaskFile",
        data: data,
        contentType: false,
        processData: false,
        success: function (data) {
            var jsonData = $.parseJSON(data);
            taskInfo = jsonData.TaskInfo;

            $('#updateDataSuccess').show('slow');

            $('#doneTaskFile').find('a').attr('href', jsonData.File);
            $('#doneTaskFile').fadeIn();

            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, true, jsonData.RecordText, 'Команда MAXon28', 'Сейчас');

            $('.addDoneTaskFile').fadeOut();

            window.setTimeout(close_file_addition_form, 3001);
            function close_file_addition_form() {
                CloseForm();
            };
            isLoadFile = true;
            if (isLoadFileVisible) {
                $('#preloaderUpdateData').fadeOut();
            }
        },
        error: function (error) {
            alert(error);
            $('#updateDataError').show('slow');
            isLoadFile = true;
            if (isLoadFileVisible) {
                $('#preloaderUpdateData').fadeOut();
                $('.btn-saveData').fadeIn();
            }
            CloseForm();
        }
    });
}

function CreateTaskCounterRecord() {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "addData";
    fullWindow.className = "fullWindow";

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseForm()");

    var container = document.createElement("div");
    container.className = "container";

    var frame = document.createElement("div");
    frame.className = "frameFullWindow";

    var inputsData = document.createElement("div");
    inputsData.id = "inputs";
    inputsData.className = "inputsData";

    var formButton = document.createElement("button");
    formButton.id = "btnUpdateCounter";
    formButton.className = "btn-saveData";
    formButton.setAttribute("onclick", "UpdateCounter()");
    formButton.innerHTML = 'Подтвердить';

    var updateDataSuccess = document.createElement("p");
    updateDataSuccess.id = "updateDataSuccess";
    updateDataSuccess.className = "successMessage";
    updateDataSuccess.innerHTML = 'Счётчик успешно обновлён!';

    var updateDataError = document.createElement("p");
    updateDataError.id = "updateDataError";
    updateDataError.className = "errorMessage";
    updateDataError.innerHTML = 'Ошибка при обновлении счётчика!';

    inputsData.append(formButton);
    inputsData.append(updateDataSuccess);
    inputsData.append(updateDataError);

    frame.append(inputsData);

    container.append(frame);

    fullWindow.append(skipButton);
    fullWindow.append(container);
    html.append(fullWindow);
}

function UpdateCounter() {
    $('#updateDataError').fadeOut();

    $('#btnUpdateCounter').fadeOut();

    var isLoadUpdateCounter = false;
    var isLoadUpdateCounterVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadUpdateCounter) {
            isLoadUpdateCounterVisible = true;
            $('.btn-saveData').fadeOut();
            $(".inputsData").append('<div id="preloaderUpdateData" class="preloader loaded" style="width: 28px; height: 28px; margin-bottom: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px; margin-top: 12px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadUpdateCounter = false;
    };

    $.ajax({
        async: true,
        type: "POST",
        url: "/TaskCounter/UpdateTaskCounter",
        data: { taskId, taskUrl },
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            var updateInfo = jsonData.UpdateInfo;
            if (updateInfo.IsFinalCount)
                $('.addTaskCounterRecord').fadeOut();

            var recordText = updateInfo.Record;

            $('#updateDataSuccess').show('slow');

            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, true, recordText, 'Команда MAXon28', 'Сейчас');

            window.setTimeout(close_counter_update_form, 3001);
            function close_counter_update_form() {
                CloseForm();
            };
            isLoadUpdateCounter = true;
            if (isLoadUpdateCounterVisible) {
                $('#preloaderUpdateData').fadeOut();
            }
        },
        error: function (error) {
            alert(error);
            $('#updateDataError').show('slow');
            isLoadUpdateCounter = true;
            if (isLoadUpdateCounterVisible) {
                $('#preloaderUpdateData').fadeOut();
                $('.btn-saveData').fadeIn();
            }
            CloseForm();
        }
    });
}

function CloseForm() {
    document.getElementById(`addData`).remove();
}

function AddRecord(taskRecords, isMAXonTeam, text, role, creationDate) {
    var recordBlock = document.createElement("div");
    recordBlock.className = "recordBlock";

    var record = document.createElement("div");
    if (isMAXonTeam)
        record.className = "record";
    else
        record.className = "record right";

    var recordText = document.createElement("p");
    if (isMAXonTeam)
        recordText.className = "recordText admin";
    else
        recordText.className = "recordText user";
    recordText.innerHTML = text;

    record.append(recordText);

    var recordDetail = document.createElement("div");
    if (isMAXonTeam)
        recordDetail.className = "recordDetail";
    else
        recordDetail.className = "recordDetail rightDetail";

    var recordAuthor = document.createElement("p");
    recordAuthor.className = "recordAuthor";
    recordAuthor.innerHTML = role;

    var recordCreationDate = document.createElement("p");
    recordCreationDate.className = "recordCreationDate";
    recordCreationDate.innerHTML = creationDate;

    recordDetail.append(recordAuthor);
    recordDetail.append(recordCreationDate);

    recordBlock.append(record);
    recordBlock.append(recordDetail);

    taskRecords.append(recordBlock);
}