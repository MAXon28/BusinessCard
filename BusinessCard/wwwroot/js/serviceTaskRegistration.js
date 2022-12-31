window.onload = init;
var isLoad = false;
var isLoadVisible = false;
var needTechnoSpec = false;
var needRate = false;
var prePrice = false;
var preDeadline = false;
var selectRateId = null;

// Жирный (b)
$('body').on('click', '.toolbar-b', function () {
    document.execCommand('bold', false, null);
    return false;
});

// Курсив (i)
$('body').on('click', '.toolbar-i', function () {
    document.execCommand('italic', false, null);
    return false;
});

// Маркированный список (ul)
$('body').on('click', '.toolbar-ul', function () {
    document.execCommand('insertUnorderedList', false, null);
    return false;
});

// Выравнивание текста по левому краю
$('body').on('click', '.toolbar-left', function () {
    document.execCommand('justifyLeft', false, null);
    return false;
});

// Выравнивание текста по центру
$('body').on('click', '.toolbar-center', function () {
    document.execCommand('justifyCenter', false, null);
    return false;
});

// Выравнивание текста по правому краю
$('body').on('click', '.toolbar-right', function () {
    document.execCommand('justifyRight', false, null);
    return false;
});

// Выравнивание по ширине
$('body').on('click', '.toolbar-justify', function () {
    document.execCommand('justifyFull', false, null);
    return false;
});

function init() {
    Timer();
    var serviceId = $(location).attr('href').split("=")[1];
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetServiceRates",
        data: { serviceId },
        success: function (serviceInformation) {
            needTechnoSpec = serviceInformation.needTechnicalSpecification;
            prePrice = serviceInformation.prePrice;
            preDeadline = serviceInformation.preDeadline;

            var serviceRuleUrlText = $('#serviceRuleUrl').text();
            $('#serviceRuleUrl').text(serviceRuleUrlText + ' "' + serviceInformation.serviceName + '"');

            if (serviceInformation.rates.length > 0) {
                needRate = true;

                var ratesBlock = document.querySelector('.rates');

                for (var i = 0; i < serviceInformation.rates.length; i++) {
                    var rateBlock = document.createElement("div");
                    rateBlock.id = serviceInformation.rates[i].rateId;
                    rateBlock.className = "rateCard";

                    var name_priceBlock = document.createElement("div");
                    name_priceBlock.className = "name_price";

                    var name = document.createElement("p");
                    name.className = "name";
                    name.innerHTML = serviceInformation.rates[i].rateName;

                    var price = document.createElement("p");
                    price.className = "price";
                    price.innerHTML = serviceInformation.rates[i].ratePrice;

                    name_priceBlock.append(name);
                    name_priceBlock.append(price);

                    rateBlock.append(name_priceBlock);

                    var conditionsBlock = document.createElement("div");
                    conditionsBlock.className = "conditionsBlock";

                    $.each(serviceInformation.rates[i].conditions, function (rateRow, rateValueInformation) {
                        var conditionBlock = document.createElement("div");
                        conditionBlock.className = "conditionBlock";

                        var conditionValue = document.createElement("span");
                        if (rateValueInformation.isAvailable) {
                            conditionValue.className = "conditionValue plus";
                            conditionValue.innerHTML = "✔";
                        } else {
                            conditionValue.className = "conditionValue minus";
                            conditionValue.innerHTML = "✘";
                        }

                        var condition = document.createElement("p");
                        condition.className = "condition";
                        if (rateValueInformation.value == null)
                            condition.innerHTML = rateRow;
                        else
                            condition.innerHTML = rateRow + ": <strong>" + rateValueInformation.value + "</strong>";

                        conditionBlock.append(conditionValue);
                        conditionBlock.append(condition);

                        conditionsBlock.append(conditionBlock);
                    });

                    rateBlock.append(conditionsBlock);

                    var separator = document.createElement("div");
                    separator.className = "separator";

                    rateBlock.append(separator);

                    var descriptionBlock = document.createElement("div");
                    descriptionBlock.className = "descriptionBlock";

                    var description = document.createElement("p");
                    description.className = "description";
                    description.innerHTML = serviceInformation.rates[i].description;

                    descriptionBlock.append(description);

                    rateBlock.append(descriptionBlock);

                    var selectRate = document.createElement("button");
                    selectRate.id = "selectButton" + serviceInformation.rates[i].rateId;
                    selectRate.className = "selectRate";
                    selectRate.setAttribute("onclick", "SelectRate('" + serviceInformation.rates[i].rateId.toString() + "')");

                    var primary = document.createElement("span");
                    primary.className = "primary text";
                    primary.innerHTML = "Выбрать";

                    var secondary = document.createElement("span");
                    secondary.className = "secondary text";
                    secondary.innerHTML = "Выбрано";

                    selectRate.append(primary);
                    selectRate.append(secondary);

                    rateBlock.append(selectRate);

                    ratesBlock.append(rateBlock);
                }
            }

            ViewForms();

            console.log(28);
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
        isLoad = false;
    };
};

function ViewForms() {
    isLoad = true;

    setTimeout(function () {
        if (isLoadVisible) {
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('skip');
        }

        var scrollId = "#taskRegistration";

        if (!prePrice) {
            var salary = document.getElementById(`salary`);
            salary.classList.add('skip');
        }

        if (!preDeadline) {
            var deadlineBlock = document.getElementById(`deadlineBlock`);
            deadlineBlock.classList.add('skip');
        }

        if (needRate) {
            var ratesBlock = document.querySelector('.ratesBlock');
            ratesBlock.classList.remove('skip');

            scrollId = "#rates";
        }

        if (needTechnoSpec) {
            var technicalSpecificationBlock = document.querySelector('.technicalSpecificationBlock');
            technicalSpecificationBlock.classList.remove('skip');

            scrollId = "#technicalSpecification";
        }

        var taskRegistrationBlock = document.querySelector('.taskRegistrationBlock');
        taskRegistrationBlock.classList.remove('skip');

        $('html, body').animate({ scrollTop: $(scrollId).offset().top }, 228);
    }, 228);

}

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

function SelectRate(rateId) {
    selectRateId = rateId;
    $(".selectRate").prop("disabled", false);
    $("#selectButton" + rateId).prop("disabled", true);
}

function CreateServiceTask() {
    event.preventDefault();
    var url = $(this).attr('href');
    window.history.replaceState("object or string", "Title", url);

    var ratesErrorBlock = document.querySelector(".ratesErrorBlock");
    ratesErrorBlock.classList.add("skip");

    var ratesError = document.querySelector(".ratesError");
    $(ratesError).animate({ opacity: 1 }, 1);

    $("#surnameError").fadeOut();
    $("#nameError").fadeOut();
    $("#middleNameError").fadeOut();
    $("#phoneNumberError").fadeOut();
    $("#emailError").fadeOut();
    $("#serviceRuleError").fadeOut();
    $("#privacyPolicyError").fadeOut();

    if (needRate && selectRateId == null) {
        $('html, body').animate({ scrollTop: $('#ratesBlock').offset().top }, 1);
        ratesErrorBlock.classList.remove("skip");
        $(ratesError).animate({ opacity: 0 }, 7000);
        return;
    }

    var surname = $('#surname').val();
    if (surname == null || surname == "" || !surname.trim()) {
        $('#surnameError').show('slow');
        $('#surname').focus();
        return;
    }

    var name = $('#name').val();
    if (name == null || name == "" || !name.trim()) {
        $('#nameError').show('slow');
        $('#name').focus();
        return;
    }


    var middleName = $('#middleName').val();
    if (middleName == null || middleName == "" || !middleName.trim()) {
        $('#middleNameError').show('slow');
        $('#middleName').focus();
        return;
    }

    var phoneNumber = $('#phoneNumber').val();
    if (phoneNumber == null || phoneNumber == "" || !phoneNumber.trim()) {
        $('#phoneNumberError').show('slow');
        $('#phoneNumber').focus();
        return;
    }

    var email = $('#email').val();
    if (email == null || email == "" || !email.trim()) {
        $('#emailError').show('slow');
        $('#email').focus();
        return;
    }

    if (!$('#serviceRule').is(':checked')) {
        $('#serviceRuleError').show('slow');
        return;
    }

    if (!$('#privacyPolicy').is(':checked')) {
        $('#privacyPolicyError').show('slow');
        return;
    }

    var connection = $('#connection').val();

    var deadline = null;
    if ($('#deadline').val() != "")
        deadline = $('#deadline').val();

    var suggestedPrice = null;
    if ($('#salaryTo').val() != "") {
        suggestedPrice = $('#salaryTo').val() + $('#currency').val();
    }

    var technicalSpecification = null;

    if (needTechnoSpec)
        if ($('#description').html() != "")
            technicalSpecification = $('#description').html();

    var newTask = {
        UserSurname: surname,
        UserName: name,
        UserMiddleName: middleName,
        UserPhoneNumber: phoneNumber,
        UserEmail: email,
        Connection: connection,
        Deadline: deadline,
        SuggestedPrice: suggestedPrice,
        TechnicalSpecification: technicalSpecification,
        RateId: selectRateId,
        ServiceId: parseInt($(location).attr('href').split("=")[1])
    }

    let data = new FormData();

    data.append("data", JSON.stringify(newTask));

    var file = document.getElementById("file").files[0];
    data.append("file", file);

    var technicalSpecificationBlock = document.querySelector('.technicalSpecificationBlock');
    technicalSpecificationBlock.classList.add('skip');

    var ratesBlock = document.querySelector('.ratesBlock');
    ratesBlock.classList.add('skip');

    var taskRegistrationBlock = document.querySelector('.taskRegistrationBlock');
    taskRegistrationBlock.classList.add('skip');

    Timer();
    $.ajax({
        type: "POST",
        url: "/MAXonWork/AddTask",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            isLoad = true;

            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('skip');

            if (result != null) {
                var registrationNumber = document.getElementById(`registrationNumber`);
                registrationNumber.innerHTML = result.taskNumber;
                registrationNumber.href = 'Task/' + result.taskAddress;

                var success = document.querySelector('.success');
                success.classList.remove('skip');
                success.classList.add('loaded');

                setTimeout(function () {
                    success.classList.add('data_effect');
                }, 28);
            }
            else {
                var error = document.querySelector('.error');
                error.classList.remove('skip');
                error.classList.add('loaded');

                setTimeout(function () {
                    error.classList.add('data_effect');
                }, 28);
            }
        },
        error: function () {
            isLoad = true;
 
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.add('skip');
 
            var error = document.querySelector('.error');
            error.classList.remove('skip');
            error.classList.add('loaded');
 
            setTimeout(function () {
                error.classList.add('data_effect');
            }, 28);
        }
    });
}