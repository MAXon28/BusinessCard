window.onload = init;
var authorizedUser = false;
var taskUrl = "";
var taskId = -1;
var serviceId = -1;
var rating = 0;

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
            authorizedUser = jsonData.IsAuthorizedUser;

            taskId = taskInfo.Data.Id;
            serviceId = taskInfo.Data.ServiceId;

            var taskData = taskInfo.Data;

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
            switch (taskData.StatusDetail.StatusType) {
                case 'PROCESS':
                    taskStatus.classList.add('processStatus');
                    break;
                case 'DONE':
                    taskStatus.classList.add('doneStatus');
                    $('.addTaskReview').fadeIn();
                    break;
                case 'REJECTED':
                    taskStatus.classList.add('rejectedStatus');
            }

            if (taskData.TechnicalSpecification != null && taskData.TechnicalSpecification != "") {
                $('.technicalSpecificationText').append(taskData.TechnicalSpecification);
                $('#technicalSpecification').fadeIn();
            }

            if (taskData.Price != -1) {
                var priceStr = taskData.Price.toString() + " ₽";
                document.querySelector('.fixedPrice').innerHTML = priceStr;
            }
            else {
                document.querySelector('.fixedPrice').innerHTML = "Цена устанавливается";
            }

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

function CreateTaskReview() {
    if (!authorizedUser) {
        OpenInformationBlock();
        return;
    }

    rating = 1;

    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "reviewCreation";
    fullWindow.className = "fullWindow";

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseForm()");

    var center = document.createElement("div");
    center.className = "center";

    var editor = document.createElement("div");
    editor.className = "editor";
    editor.setAttribute("contenteditable", "true");
    editor.setAttribute("data-placeholder", "Оставьте отзыв о приложении");

    var formButton = document.createElement("button");
    formButton.className = "formButton";
    formButton.setAttribute("onclick", "SendReview()");
    formButton.innerHTML = '<p>Отправить</p>';

    var saveReviewSuccess = document.createElement("p");
    saveReviewSuccess.id = "saveReviewSuccess";
    saveReviewSuccess.className = "successMessage";
    saveReviewSuccess.innerHTML = 'Отзыв успешно сохранён! Спасибо за обратную связь!';

    var saveReviewError = document.createElement("p");
    saveReviewError.id = "saveReviewError";
    saveReviewError.className = "errorMessage";
    saveReviewError.innerHTML = 'Ошибка при отправке отзыва!';

    center.innerHTML = '<div class="newRating"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc1" checked=""><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc2"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc3"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc4"><input type="radio" name="rating-star" class="rating__control screen-reader" id="rc5"><label for="rc1" class="newItem" onclick="SetRating(1)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">1</span></label><label for="rc2" class="newItem" onclick="SetRating(2)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">2</span></label><label for="rc3" class="newItem" onclick="SetRating(3)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">3</span></label><label for="rc4" class="newItem" onclick="SetRating(4)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">4</span></label><label for="rc5" class="newItem" onclick="SetRating(5)"><svg class="rating__star"><use xlink:href="#star"></use></svg><span class="screen-reader">5</span></label></div>';
    center.append(editor);
    center.append(formButton);
    center.append(saveReviewSuccess);
    center.append(saveReviewError);

    fullWindow.append(skipButton);
    fullWindow.append(center);
    html.append(fullWindow);
}

function SetRating(newRating) {
    rating = newRating;
}

function SendReview() {
    $('#saveReviewError').fadeOut();

    $('.formButton').fadeOut();

    var isLoadSaveReview = false;
    var isLoadSaveReviewVisible = false;

    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoadSaveReview) {
            isLoadSaveReviewVisible = true;
            $('.formButton').fadeOut();
            $(".center").append('<div id="preloaderSaveData" class="preloader loaded" style="width: 28px; height: 28px; margin-bottom: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px; margin-top: 12px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
        }
        isLoadSaveReview = false;
    };

    var text = $('.editor')[0].innerText;

    $.ajax({
        async: true,
        type: "POST",
        url: "/TaskReview/AddReview",
        data: { taskId, taskUrl, rating, text, serviceId },
        dataTpye: "json",
        success: function (recordText) {
            $('.addTaskReview').fadeOut();

            $('#saveReviewSuccess').show('slow');

            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, false, recordText, 'Пользователь', 'Сейчас');
            taskRecords.scrollTop = taskRecords.scrollHeight;

            window.setTimeout(close_counter_update_form, 3001);
            function close_counter_update_form() {
                CloseForm();
            };
            isLoadSaveReview = true;
            if (isLoadSaveReviewVisible) {
                $('#preloaderSaveData').fadeOut();
            }
        },
        error: function (error) {
            alert(error);
            $('#saveReviewError').show('slow');
            isLoadSaveReview = true;
            if (isLoadSaveReviewVisible) {
                $('#preloaderSaveData').fadeOut();
                $('.formButton').fadeIn();
            }
        }
    });
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

function CloseForm() {
    document.getElementById(`reviewCreation`).remove();
}

function OpenInformationBlock() {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.className = "fullWindowReview";

    var center = document.createElement("div");
    center.className = "centerReview";

    var requirement = document.createElement("p");
    requirement.className = "requirementReview";
    requirement.innerHTML = '<a href="/Account/Login">Войдите</a> на платформу MAXon28, чтобы оставлять отзывы';
    center.append(requirement);

    var skipButton = document.createElement("p");
    skipButton.className = "skipButtonReview";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseInformationBlock()");

    center.append(skipButton);

    fullWindow.append(center);
    html.append(fullWindow);
}

function CloseInformationBlock() {
    $('.fullWindowReview').remove();
}

function DownloadDoneTaskFile() {
    $.ajax({
        async: true,
        type: "POST",
        url: "/MAXonTask/NoticeDwonloadDoneTaskFile",
        data: { taskId, taskUrl },
        dataTpye: "json",
        success: function (recordText) {
            var taskRecords = document.querySelector('.recordsBlock');
            AddRecord(taskRecords, false, recordText, 'Пользователь', 'Сейчас');
            taskRecords.scrollTop = taskRecords.scrollHeight;
        },
        error: function (error) {
            alert(error);
        }
    });
}