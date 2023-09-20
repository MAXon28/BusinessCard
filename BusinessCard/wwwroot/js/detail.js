window.onload = init;
var currentSurname = "";
var currentName = "";
var currentMiddleName = "";
var currentEmail = "";
var currentPhoneNumber = "";

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
        url: "/Account/GetProfileData",
        contentType: 'application/json',
        dataTpye: "json",
        success: function (data) {
            var jsonData = $.parseJSON(data);
            userData = jsonData.UserData;

			currentSurname = userData.Surname;
			$('#surname').val(currentSurname);
			currentName = userData.Name;
			$('#name').val(currentName);
			currentMiddleName = userData.MiddleName;
			$('#middleName').val(currentMiddleName);
			currentEmail = userData.Email;
			$('#login').val(currentEmail);
			currentPhoneNumber = userData.PhoneNumber;
			$('#phoneNumber').val(currentPhoneNumber);

            isLoad = true;
            if (isLoadVisible) {
                $('.reverse-spinner').fadeOut();
            }

            $('.container').fadeIn();
        },
        error: function (error) {
            alert(error);
        }
    });
}

function SaveNewUserData() {
	$("#surnameError").fadeOut();
	$("#nameError").fadeOut();
	$('#loginError').fadeOut();
	$('#loginErrorFromServer').fadeOut();
	$('#phoneNumberError').fadeOut();
	$('#phoneNumberErrorFromServer').fadeOut();
	$("#registrationError").fadeOut();
	$('#registrationSuccess').fadeOut();

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

	var email = $('#login').val();
	if (email == null || email == "" || !email.trim()) {
		$('#loginError').show('slow');
		$('#login').focus();
		return;
	}

	var phoneNumber = $('#phoneNumber').val();
	if (phoneNumber == null || phoneNumber == "" || !phoneNumber.trim()) {
		$('#phoneNumberError').show('slow');
		$('#phoneNumber').focus();
		return;
	}

	if (currentSurname == surname && currentName == name && currentMiddleName == middleName && currentEmail == email && currentPhoneNumber == phoneNumber)
		return;

	var user = {
		Surname: surname,
		Name: name,
		MiddleName: middleName,
		Email: email,
		PhoneNumber: phoneNumber
	}

	var isUpdateUserProfile = false;
	var isUpdateUserProfileVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isUpdateUserProfile) {
			isUpdateUserProfileVisible = true;
			$('.btn-saveData').fadeOut();
			$(".inputsData").append('<div id="preloaderUpdateProfile" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isUpdateUserProfile = false;
	};

	$.ajax({
		type: "POST",
		url: "/Account/UpdateProfile",
		contentType: 'application/json',
		data: JSON.stringify(user),
		success: function (result) {

			if (result) {
				currentSurname = surname;
				currentName = name;
				currentMiddleName = middleName;
				currentEmail = email;
				$("#surnameError").fadeOut();
				$("#nameError").fadeOut();
				$('#loginError').fadeOut();
				$('#loginErrorFromServer').fadeOut();
				$("#registrationError").fadeOut();
				$('#registrationSuccess').show('slow');
			}
			else {
				$('#registrationError').text('Не удалось обновить данные профиля. Пожалуйста, повторите попытку.');
				$('#registrationError').show('slow');
			}

			isUpdateUserProfile = true;
			if (isUpdateUserProfileVisible) {
				$("#preloaderUpdateProfile").remove();
				$('.btn-saveData').fadeIn();
			}
		},
		error: function (error) {
			switch (error.responseJSON.TypeOfError) {
				case 'UNIQUE_VALUE':
					$('#registrationError').text(error.responseJSON.ErrorMessage);
					$('#registrationError').show('slow');
					$('#login').focus();
					break;
				case 'EMAIL':
					$('#loginErrorFromServer').text(error.responseJSON.ErrorMessage);
					$('#loginErrorFromServer').show('slow');
					$('#login').focus();
					break;
				case 'PHONE':
					$('#phoneNumberErrorFromServer').text(error.responseJSON.ErrorMessage);
					$('#phoneNumberErrorFromServer').show('slow');
					$('#phoneNumber').focus();
				default:
					$('#registrationError').text(error.responseJSON.ErrorMessage);
					$('#registrationError').show('slow');
			}

			isUpdateUserProfile = true;
			if (isUpdateUserProfileVisible) {
				$("#preloaderUpdateProfile").remove();
				$('.btn-saveData').fadeIn();
			}
		}
	});
}

function ReplacePassword() {
    var html = document.querySelector('html');

    var fullWindow = document.createElement("div");
    fullWindow.id = "replacePasswordBlock";
    fullWindow.className = "fullWindow";

    var skipButton = document.createElement("p");
    skipButton.className = "skipButton";
    skipButton.innerHTML = "×";
    skipButton.setAttribute("onclick", "CloseReplacePassword()");

    var container = document.createElement("div");
    container.className = "container";

    var frame = document.createElement("div");
    frame.className = "frameFullWindow";

	var inputsData = document.createElement("form");
	inputsData.id = "inputsPassword";
	inputsData.className = "inputsData";
	inputsData.setAttribute("action", "");
	inputsData.setAttribute("method", "post");
	inputsData.setAttribute("name", "form");

	var formButton = document.createElement("button");
	formButton.id = "btnSavePassword";
    formButton.className = "btn-saveData";
    formButton.setAttribute("onclick", "SaveNewPassword()");
	formButton.innerHTML = 'Сохранить';

	var updatePasswordSuccess = document.createElement("p");
	updatePasswordSuccess.id = "updatePasswordSuccess";
	updatePasswordSuccess.className = "successMessage";
	updatePasswordSuccess.innerHTML = 'Пароль успешно обновлён!';

	var updatePasswordError = document.createElement("p");
	updatePasswordError.id = "updatePasswordError";
	updatePasswordError.className = "errorMessage";

	inputsData.innerHTML = '<div class="inputbox"><input id="currentPassword" class="input" type="password" required="required" autocomplete="off"><span class="placeholder">Текущий пароль</span><p id="currentPasswordError" class="errorMessage">Это поле является обязательным для заполнения.</p><p id="currentPasswordFromServer" class="errorMessage"></p></div><div class="inputbox"><input id="newPassword" class="input" type="password" required="required" autocomplete="off"><span class="placeholder">Новый пароль</span><p id="newPasswordError" class="errorMessage">Это поле является обязательным для заполнения.</p><p id="newPasswordFromServer" class="errorMessage"></p></div><div class="inputbox"><input id="repeatPassword" class="input" type="password" required="required" autocomplete="off"><span class="placeholder">Пароль ещё раз</span><p id="repeatPasswordError" class="errorMessage">Пароль повторно введён неверно.</p></div>';
	inputsData.append(formButton);
	inputsData.append(updatePasswordSuccess);
	inputsData.append(updatePasswordError);

    frame.append(inputsData);

    container.append(frame);

    fullWindow.append(skipButton);
    fullWindow.append(container);
    html.append(fullWindow);
}

function SaveNewPassword() {
	event.preventDefault();
	var url = $(this).attr('href');
	window.history.replaceState("object or string", "Title", url);

	$("#currentPasswordError").fadeOut();
	$("#currentPasswordFromServer").fadeOut();
	$("#newPasswordError").fadeOut();
	$("#newPasswordFromServer").fadeOut();
	$("#repeatPasswordError").fadeOut();
	$("#updatePasswordError").fadeOut();

	var currentPassword = $('#currentPassword').val();
	if (currentPassword == null || currentPassword == "" || !currentPassword.trim()) {
		$('#currentPasswordError').show('slow');
		$('#currentPassword').focus();
		return;
	}

	var newPassword = $('#newPassword').val();
	if (newPassword == null || newPassword == "" || !newPassword.trim()) {
		$('#newPasswordError').show('slow');
		$('#newPassword').focus();
		return;
	}

	var repeatPassword = $('#repeatPassword').val();
	if (repeatPassword == null || repeatPassword == "" || !repeatPassword.trim()) {
		$('#repeatPassword').focus();
		return;
	}

	if (newPassword != repeatPassword) {
		$('#repeatPasswordError').show('slow');
		$('#repeatPassword').focus();
		return;
	}

	var isLoadNewPassword = false;
	var isLoadNewPasswordVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadNewPassword) {
			isLoadNewPasswordVisible = true;
			$('#btnSavePassword').fadeOut();
			$("#inputsPassword").append('<div id="preloaderUpdatePassword" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadNewPassword = false;
	};

	$.ajax({
		type: "POST",
		url: "/Account/UpdatePassword",
		data: { currentPassword, newPassword },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				$('#updatePasswordSuccess').show('slow');
				window.setTimeout(close_password_update_form, 3001);
				function close_password_update_form() {
					CloseReplacePassword();
				};
			}
			else {
				$('#currentPasswordFromServer').text('Не удалось обновить пароль. Текущий пароль введён неверно.');
				$('#currentPasswordFromServer').show('slow');
				$('#currentPassword').focus();
			}

			isLoadNewPassword = true;
			if (isLoadNewPasswordVisible) {
				$("#preloaderUpdatePassword").remove();

				if (!result)
					$('#btnSavePassword').fadeIn();
			}
		},
		error: function (error) {
			switch (error.responseJSON.TypeOfError) {
				case 'PASSWORD':
					$('#newPasswordFromServer').text(error.responseJSON.ErrorMessage);
					$('#newPasswordFromServer').show('slow');
					$('#newPassword').focus();
					break;
				default:
					$('#updatePasswordError').text(error.responseJSON.ErrorMessage);
					$('#updatePasswordError').show('slow');
			}

			isLoadNewPassword = true;
			if (isLoadNewPasswordVisible) {
				$("#preloaderUpdatePassword").remove();
				$('#btnSavePassword').fadeIn();
			}
		}
	});
}

function CloseReplacePassword() {
    document.getElementById(`replacePasswordBlock`).remove();
}