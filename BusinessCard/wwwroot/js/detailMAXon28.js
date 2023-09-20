window.onload = init;
var currentSurname = "";
var currentName = "";
var currentMiddleName = "";
var currentEmail = "";
var currentPhoneNumber = "";
var currentFacts = null;
var currentBiography = "";
var currentSkills = null;
var currentAllExperience = null;
var currentAllEducation = null;
var needWork = null;
var currentResume = null;
var mainDataByServices = null;
var currentConditions = [];
var currentCalculatedValues = [];
var calculatedValueVariants = null;
var currentRates = [];

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
			$('.adminButtons').fadeIn();
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

function FactsAboutMe() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("padding", "22px 0");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "factsAboutMe";
	inputsData.className = "inputData";

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (currentFacts == null) {
		var isLoadFacts = false;
		var isLoadFactsVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadFacts) {
				isLoadFactsVisible = true;
				$("#factsAboutMe").append('<div id="preloaderFacts" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadFacts = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonBusinessCard/GetMainFacts",
			dataTpye: "json",
			success: function (facts) {

				currentFacts = facts;

				isLoadFacts = true;
				if (isLoadFactsVisible) {
					$("#preloaderFacts").remove();
				}
				SetUpdateFactsForm();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateFactsForm();
}

function SetUpdateFactsForm() {
	var inputsData = document.getElementById('factsAboutMe');

	for (var i = 0; i < currentFacts.length; i++) {
		$(inputsData).append('<div class="inputbox" style="margin-bottom: 12px;"><input id="fact' + i + '" class="input" required="required" autocomplete="off"><span class="placeholder">Факт №' + parseInt(i + 1) + '</span><p id="currentFactError' + i + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');
		var input = $('#fact' + i);
		input.val(currentFacts[i].text);

		var formButton = document.createElement("button");
		formButton.id = "btn" + i;
		formButton.className = "btn-saveData";
		formButton.setAttribute("onclick", "UpdateFactsAboutMe(" + i + ")");
		$(formButton).css("background", "#87CEFA");
		$(formButton).css("margin-top", "0px");
		$(formButton).css("margin-bottom", "27px");
		formButton.innerHTML = 'Сохранить';

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateFactsAboutMe" + i;
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Факты успешно обновлены!';

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateFactsAboutMe" + i;
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить факты! Попробуйте ещё раз.';

		inputsData.append(formButton);
		inputsData.append(updateSuccess);
		inputsData.append(updateError);
	}
}

function UpdateFactsAboutMe(index) {
	$("#errorUpdateFactsAboutMe" + index).fadeOut();

	$("#currentFactError" + index).fadeOut();

	var newFact = $("#fact" + index).val();
	if (newFact == null || newFact == "") {
		$("#currentFactError" + index).fadeIn();
		return;
	}

	if (newFact == currentFacts[index].text)
		return;

	var isLoadUpdateFacts = false;
	var isLoadUpdateFactsVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateFacts) {
			isLoadUpdateFactsVisible = true;
			$('#btn' + index).fadeOut();
			$("#factsAboutMe").append('<div id="preloaderUpdateFacts" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateFacts = false;
	};

	var fact = {
		Id: currentFacts[index].id,
		Text: newFact
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/UpdateFact",
		data: { fact },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentFacts[index].text = newFact;
				$('#successUpdateFactsAboutMe' + index).show('slow');
				window.setTimeout(close_fact_update_form, 3001);
				function close_fact_update_form() {
					$('#successUpdateFactsAboutMe' + index).fadeOut();
				};
			}
			else {
				$('#errorUpdateFactsAboutMe' + index).show('slow');
			}

			isLoadUpdateFacts = true;
			if (isLoadUpdateFactsVisible) {
				$("#preloaderUpdateFacts").remove();
				$('#btn' + index).fadeIn();
			}
		},
		error: function (error) {
			alert(error);
			$('#errorUpdateFactsAboutMe' + index).show('slow');

			isLoadUpdateFacts = true;
			if (isLoadUpdateFactsVisible) {
				$("#preloaderUpdateFacts").remove();
				$('.btn-saveData').fadeIn();
			}
		}
	});
}

function Biography() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "biography";
	inputsData.className = "inputData";

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (currentBiography == "") {
		var isLoadBiography = false;
		var isLoadBiographyVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadBiography) {
				isLoadBiographyVisible = true;
				$("#biography").append('<div id="preloaderBiography" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadBiography = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonBusinessCard/GetBiography",
			dataTpye: "json",
			success: function (biography) {

				currentBiography = biography;

				isLoadBiography = true;
				if (isLoadBiographyVisible) {
					$("#preloaderBiography").remove();
				}
				SetUpdateBiographyForm();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateBiographyForm();
}

function SetUpdateBiographyForm() {
	var inputsData = document.getElementById('biography');

	var editor = document.createElement("div");
	editor.className = "editor";
	editor.setAttribute("contenteditable", "true");
	editor.setAttribute("data-placeholder", "Биография");
	editor.innerHTML = currentBiography;

	var formButton = document.createElement("button");
	formButton.className = "btn-saveData";
	formButton.setAttribute("onclick", "UpdateBiography()");
	$(formButton).css("background", "#87CEFA");
	formButton.innerHTML = 'Сохранить';

	var updateSuccess = document.createElement("p");
	updateSuccess.id = "successUpdateBiography"
	updateSuccess.className = "successMessage";
	updateSuccess.innerHTML = 'Биография успешно обновлена!';

	var updateError = document.createElement("p");
	updateError.id = "errorUpdateBiography"
	updateError.className = "errorMessage";
	updateError.innerHTML = 'Не удалось обновить биографию! Попробуйте ещё раз.';

	inputsData.append(editor);
	inputsData.append(formButton);
	inputsData.append(updateSuccess);
	inputsData.append(updateError);
}

function UpdateBiography() {
	$("#errorUpdateBiography").fadeOut();

	var biography = $('.editor').text();

	if (biography == null || biography == "") {
		return;
    }

	var isLoadUpdateBiography = false;
	var isLoadUpdateBiographyVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateBiography) {
			isLoadUpdateBiographyVisible = true;
			$('.btn-saveData').fadeOut();
			$("#inputsPassword").append('<div id="preloaderUpdateBiography" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateBiography = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/UpdateBiography",
		data: { biography },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentBiography = biography;
				$('#successUpdateBiography').show('slow');
				window.setTimeout(close_biography_update_form, 3001);
				function close_biography_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateBiography').show('slow');
			}

			isLoadUpdateBiography = true;
			if (isLoadUpdateBiographyVisible) {
				$("#preloaderUpdateBiography").remove();

				if (!result)
					$('.btn-saveData').fadeIn();
			}
		},
		error: function (error) {
			alert(error);
			$('#errorUpdateBiography').show('slow');

			isLoadUpdateBiography = true;
			if (isLoadUpdateBiographyVisible) {
				$("#preloaderUpdateBiography").remove();
				$('.btn-saveData').fadeIn();
			}
		}
	});
}

function Skills() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "skills";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (currentSkills == null) {
		var isLoadSkills = false;
		var isLoadSkillsVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadSkills) {
				isLoadSkillsVisible = true;
				$("#skills").append('<div id="preloaderSkills" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadSkills = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonBusinessCard/GetSkills",
			dataTpye: "json",
			success: function (skills) {

				currentSkills = skills;

				isLoadSkills = true;
				if (isLoadSkillsVisible) {
					$("#preloaderSkills").remove();
				}
				SetUpdateSkillsForm();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateSkillsForm();
}

function SetUpdateSkillsForm() {
	var inputsData = document.getElementById('skills');

	for (var i = 0; i < currentSkills.length; i++) {
		if (currentSkills[i] == null)
			continue;

		var formButton = document.createElement("button");
		formButton.id = "btn" + i;
		formButton.className = "btn-saveData";
		formButton.setAttribute("onclick", "Skill(" + i + ")");
		$(formButton).css("background", "#211F30");
		$(formButton).css("color", "#87CEFA");
		$(formButton).css("margin-top", "12px");
		$(formButton).css("margin-bottom", "12px");
		formButton.innerHTML = currentSkills[i].name;

		inputsData.append(formButton);
	}

	var formButton = document.createElement("button");
	formButton.className = "btn-saveData";
	formButton.setAttribute("onclick", "SkillFormForAdd()");
	$(formButton).css("background", "#87CEFA");
	$(formButton).css("margin-top", "12px");
	$(formButton).css("margin-bottom", "12px");
	formButton.innerHTML = "Добавить навык";
	inputsData.append(formButton);
}

function SkillFormForAdd() {
	$("#skills").fadeOut();

	window.setTimeout(show_skill, 501);
	function show_skill() {
		var inputsData = document.createElement("div");
		inputsData.id = "skill";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="skillName" class="input" required="required" autocomplete="off"><span class="placeholder">Наименование</span><p id="skillNameError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="skillPercent" class="input" required="required" autocomplete="off"><span class="placeholder">Уровень знаний</span><p id="skillPercentError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var editor = document.createElement("div");
		editor.id = "skillDescription";
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		$(inputsData).append(editor);
		$(inputsData).append('<p id="skillDescriptionError" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonSave = document.createElement("button");
		formButtonSave.id = "btnAdd";
		formButtonSave.className = "btn-saveData";
		formButtonSave.setAttribute("onclick", "AddSkill()");
		formButtonSave.innerHTML = "Добавить";
		$(formButtonSave).css("background", "#87CEFA");
		$(inputsData).append(formButtonSave);

		var saveSuccess = document.createElement("p");
		saveSuccess.id = "successAddSkill";
		$(saveSuccess).css("margin-top", "0px");
		$(saveSuccess).css("margin-bottom", "12px");
		saveSuccess.className = "successMessage";
		saveSuccess.innerHTML = 'Навык успешно добавлен!';
		$(inputsData).append(saveSuccess);

		var saveError = document.createElement("p");
		saveError.id = "errorAddSkill";
		$(saveError).css("margin-top", "0px");
		$(saveError).css("margin-bottom", "12px");
		saveError.className = "errorMessage";
		saveError.innerHTML = 'Не удалось добавить навык! Попробуйте ещё раз.';
		$(inputsData).append(saveError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);
	};
}

function Skill(index) {
	$("#skills").fadeOut();

	window.setTimeout(show_skill, 501);
	function show_skill() {
		var inputsData = document.createElement("div");
		inputsData.id = "skill";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="skillName' + index + '" class="input" required="required" autocomplete="off" value="' + currentSkills[index].name + '"><span class="placeholder">Наименование</span><p id="skillNameError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="skillPercent' + index + '" class="input" required="required" autocomplete="off" value="' + currentSkills[index].percentOfKnowledge + '"><span class="placeholder">Уровень знаний</span><p id="skillPercentError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var editor = document.createElement("div");
		editor.id = "skillDescription" + index;
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		editor.innerHTML = currentSkills[index].description;
		$(inputsData).append(editor);
		$(inputsData).append('<p id="skillDescriptionError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "UpdateSkill(" + index + ")");
		formButtonUpdate.innerHTML = "Обновить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(inputsData).append(formButtonUpdate);

		var formButtonDelete = document.createElement("button");
		formButtonDelete.id = "btnDelete";
		formButtonDelete.className = "btn-saveData";
		formButtonDelete.setAttribute("onclick", "DeleteSkill(" + index + ")");
		formButtonDelete.innerHTML = "Удалить";
		$(inputsData).append(formButtonDelete);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateSkill";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Навык успешно обновлён!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateSkill";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить навык! Попробуйте ещё раз.';
		$(inputsData).append(updateError);

		var deleteSuccess = document.createElement("p");
		deleteSuccess.id = "successDeleteSkill";
		$(deleteSuccess).css("margin-top", "0px");
		$(deleteSuccess).css("margin-bottom", "12px");
		deleteSuccess.className = "successMessage";
		deleteSuccess.innerHTML = 'Навык успешно удалён!';
		$(inputsData).append(deleteSuccess);

		var deleteError = document.createElement("p");
		deleteError.id = "errorDeleteSkill";
		$(deleteError).css("margin-top", "0px");
		$(deleteError).css("margin-bottom", "12px");
		deleteError.className = "errorMessage";
		deleteError.innerHTML = 'Не удалось удалить навык! Попробуйте ещё раз.';
		$(inputsData).append(deleteError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);
	};
}

function AddSkill() {
	$("#errorUpdateSkill").fadeOut();

	$("#skillNameError").fadeOut();
	$("#skillPercentError").fadeOut();
	$("#skillDescriptionError").fadeOut();

	var newSkillName = $("#skillName").val();
	var newSkillPercent = $("#skillPercent").val();
	var newSkillDescription = $("#skillDescription").text();

	if (newSkillName == null || newSkillName == "") {
		$("#skillNameError").fadeIn();
		return;
	}

	if (newSkillPercent == null || newSkillPercent == "") {
		$("#skillPercentError").fadeIn();
		return;
	}

	if (newSkillDescription == null || newSkillDescription == "") {
		$("#skillDescriptionError").fadeIn();
		return;
	}

	var skill = {
		Name: newSkillName,
		PercentOfKnowledge: newSkillPercent,
		Description: newSkillDescription
	};

	var isLoadAddSkill = false;
	var isLoadAddSkillVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddSkill) {
			isLoadAddSkillVisible = true;
			$('#btnAdd').fadeOut();
			$("#skill").append('<div id="preloaderAddSkill" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddSkill = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/AddSkill",
		data: { skill },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentSkills = null;
				$('#successAddSkill').show('slow');
				window.setTimeout(close_skill_add_form, 3001);
				function close_skill_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddSkill').show('slow');
			}

			isLoadAddSkill = true;
			if (isLoadAddSkillVisible) {
				$("#preloaderAddSkill").remove();

				if (!result)
					$('#btnAdd').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function UpdateSkill(index) {
	$("#errorUpdateSkill").fadeOut();

	$("#skillNameError" + index).fadeOut();
	$("#skillPercentError" + index).fadeOut();
	$("#skillDescriptionError" + index).fadeOut();

	var newSkillName = $("#skillName" + index).val();
	var newSkillPercent = $("#skillPercent" + index).val();
	var newSkillDescription = $("#skillDescription" + index).text();

	if (newSkillName == null || newSkillName == "") {
		$("#skillNameError" + index).fadeIn();
		return;
	}

	if (newSkillPercent == null || newSkillPercent == "") {
		$("#skillPercentError" + index).fadeIn();
		return;
	}

	if (newSkillDescription == null || newSkillDescription == "") {
		$("#skillDescriptionError" + index).fadeIn();
		return;
	}

	if (newSkillName == currentSkills[index].name && newSkillPercent == currentSkills[index].percentOfKnowledge && newSkillDescription == currentSkills[index].description)
		return;

	var skill = {
		Id: currentSkills[index].id,
		Name: newSkillName,
		PercentOfKnowledge: newSkillPercent,
		Description: newSkillDescription
	};

	var isLoadUpdateSkill = false;
	var isLoadUpdateSkillVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateSkill) {
			isLoadUpdateSkillVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#skill").append('<div id="preloaderUpdateSkill" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateSkill = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/UpdateSkill",
		data: { skill },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentSkills[index].name = skill.Name;
				currentSkills[index].percentOfKnowledge = skill.PercentOfKnowledge;
				currentSkills[index].description = skill.Description;
				$('#successUpdateSkill').show('slow');
				window.setTimeout(close_skill_update_form, 3001);
				function close_skill_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateSkill').show('slow');
			}

			isLoadUpdateSkill = true;
			if (isLoadUpdateSkillVisible) {
				$("#preloaderUpdateSkill").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function DeleteSkill(index) {
	$("#errorDeleteSkill").fadeOut();

	var isLoadDeleteSkill = false;
	var isLoadDeleteSkillVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadDeleteSkill) {
			isLoadDeleteSkillVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#skill").append('<div id="preloaderDeleteSkill" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadDeleteSkill = false;
	};

	var skillId = currentSkills[index].id;
	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/DeleteSkill",
		data: { skillId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentSkills[index] = null;
				$('#successDeleteSkill').show('slow');
				window.setTimeout(close_skill_delete_form, 3001);
				function close_skill_delete_form() {
					CloseForm();
				};
			}
			else {
				$('#errorDeleteSkill').show('slow');
			}

			isLoadDeleteSkill = true;
			if (isLoadDeleteSkillVisible) {
				$("#preloaderDeleteSkill").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
                }
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function AllExperience() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "allExperience";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (currentAllExperience == null) {
		var isLoadAllExperience = false;
		var isLoadAllExperienceVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadAllExperience) {
				isLoadAllExperienceVisible = true;
				$("#allExperience").append('<div id="preloaderAllExperience" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadAllExperience = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonBusinessCard/GetExperience",
			dataTpye: "json",
			success: function (allExperience) {

				currentAllExperience = allExperience;

				isLoadAllExperience = true;
				if (isLoadAllExperienceVisible) {
					$("#preloaderAllExperience").remove();
				}
				SetUpdateAllExperienceForm();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateAllExperienceForm();
}

function SetUpdateAllExperienceForm() {
	var inputsData = document.getElementById('allExperience');

	for (var i = 0; i < currentAllExperience.length; i++) {
		if (currentAllExperience[i] == null)
			continue;

		var formButton = document.createElement("button");
		formButton.id = "btn" + i;
		formButton.className = "btn-saveData";
		formButton.setAttribute("onclick", "Experience(" + i + ")");
		$(formButton).css("background", "#211F30");
		$(formButton).css("color", "#87CEFA");
		$(formButton).css("margin-top", "12px");
		$(formButton).css("margin-bottom", "12px");
		formButton.innerHTML = currentAllExperience[i].company;

		inputsData.append(formButton);
	}

	var formButton = document.createElement("button");
	formButton.className = "btn-saveData";
	formButton.setAttribute("onclick", "ExperienceFormForAdd()");
	$(formButton).css("background", "#87CEFA");
	$(formButton).css("margin-top", "12px");
	$(formButton).css("margin-bottom", "12px");
	formButton.innerHTML = "Добавить опыт работы";
	inputsData.append(formButton);
}

function ExperienceFormForAdd() {
	$("#allExperience").fadeOut();

	window.setTimeout(show_experience, 501);
	function show_experience() {
		var inputsData = document.createElement("div");
		inputsData.id = "experience";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="company" class="input" required="required" autocomplete="off"><span class="placeholder">Компания</span><p id="companyError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="position" class="input" required="required" autocomplete="off"><span class="placeholder">Должность</span><p id="positionError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="fromDate" class="input" required="required" autocomplete="off"><span class="placeholder">Начало работы</span><p id="fromDateError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="toDate" class="input" required="required" autocomplete="off"><span class="placeholder">Окончание работы</span></div>');

		var editor = document.createElement("div");
		editor.id = "experienceDescription";
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		$(inputsData).append(editor);
		$(inputsData).append('<p id="experienceDescriptionError" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonSave = document.createElement("button");
		formButtonSave.id = "btnAdd";
		formButtonSave.className = "btn-saveData";
		formButtonSave.setAttribute("onclick", "AddExperience()");
		formButtonSave.innerHTML = "Добавить";
		$(formButtonSave).css("background", "#87CEFA");
		$(inputsData).append(formButtonSave);

		var saveSuccess = document.createElement("p");
		saveSuccess.id = "successAddExperience";
		$(saveSuccess).css("margin-top", "0px");
		$(saveSuccess).css("margin-bottom", "12px");
		saveSuccess.className = "successMessage";
		saveSuccess.innerHTML = 'Опыт успешно добавлен!';
		$(inputsData).append(saveSuccess);

		var saveError = document.createElement("p");
		saveError.id = "errorAddExperience";
		$(saveError).css("margin-top", "0px");
		$(saveError).css("margin-bottom", "12px");
		saveError.className = "errorMessage";
		saveError.innerHTML = 'Не удалось добавить опыт! Попробуйте ещё раз.';
		$(inputsData).append(saveError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);
	};
}

function Experience(index) {
	$("#allExperience").fadeOut();

	window.setTimeout(show_experience, 501);
	function show_experience() {
		var inputsData = document.createElement("div");
		inputsData.id = "experience";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="company' + index + '" class="input" required="required" autocomplete="off"><span class="placeholder">Компания</span><p id="companyError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="position' + index + '" class="input" required="required" autocomplete="off" value="' + currentAllExperience[index].position + '"><span class="placeholder">Должность</span><p id="positionError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="fromDate' + index + '" class="input" required="required" autocomplete="off" value="' + currentAllExperience[index].fromDate + '"><span class="placeholder">Начало работы</span><p id="fromDateError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="toDate' + index + '" class="input" required="required" autocomplete="off" value="' + currentAllExperience[index].toDate + '"><span class="placeholder">Окончание работы</span></div>');

		var editor = document.createElement("div");
		editor.id = "experienceDescription" + index;
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		editor.innerHTML = currentAllExperience[index].description;
		$(inputsData).append(editor);
		$(inputsData).append('<p id="experienceDescriptionError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "UpdateExperience(" + index + ")");
		formButtonUpdate.innerHTML = "Обновить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(inputsData).append(formButtonUpdate);

		var formButtonDelete = document.createElement("button");
		formButtonDelete.id = "btnDelete";
		formButtonDelete.className = "btn-saveData";
		formButtonDelete.setAttribute("onclick", "DeleteExperience(" + index + ")");
		formButtonDelete.innerHTML = "Удалить";
		$(inputsData).append(formButtonDelete);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateExperience";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Опыт успешно обновлён!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateExperience";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить опыт! Попробуйте ещё раз.';
		$(inputsData).append(updateError);

		var deleteSuccess = document.createElement("p");
		deleteSuccess.id = "successDeleteExperience";
		$(deleteSuccess).css("margin-top", "0px");
		$(deleteSuccess).css("margin-bottom", "12px");
		deleteSuccess.className = "successMessage";
		deleteSuccess.innerHTML = 'Опыт успешно удалён!';
		$(inputsData).append(deleteSuccess);

		var deleteError = document.createElement("p");
		deleteError.id = "errorDeleteExperience";
		$(deleteError).css("margin-top", "0px");
		$(deleteError).css("margin-bottom", "12px");
		deleteError.className = "errorMessage";
		deleteError.innerHTML = 'Не удалось удалить опыт! Попробуйте ещё раз.';
		$(inputsData).append(deleteError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);

		var input = $('#company' + index);
		input.val(currentAllExperience[index].company);
	};
}

function AddExperience() {
	$("#errorAddExperience").fadeOut();

	$("#companyError").fadeOut();
	$("#positionError").fadeOut();
	$("#fromDateError").fadeOut();
	$("#experienceDescriptionError").fadeOut();

	var newCompany = $("#company").val();
	var newPosition = $("#position").val();
	var newFromDate = $("#fromDate").val();
	var newToDate = $("#toDate").val();
	var newExperienceDescription = $("#experienceDescription").text();

	if (newCompany == null || newCompany == "") {
		$("#companyError").fadeIn();
		return;
	}

	if (newPosition == null || newPosition == "") {
		$("#positionError").fadeIn();
		return;
	}

	if (newFromDate == null || newFromDate == "") {
		$("#fromDateError").fadeIn();
		return;
	}

	if (newExperienceDescription == null || newExperienceDescription == "") {
		$("#experienceDescriptionError").fadeIn();
		return;
	}

	var experience = {
		Company: newCompany,
		Position: newPosition,
		Description: newExperienceDescription,
		FromDate: newFromDate,
		ToDate: newToDate
	};

	var isLoadAddExperience = false;
	var isLoadAddExperienceVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddExperience) {
			isLoadAddExperienceVisible = true;
			$('#btnAdd').fadeOut();
			$("#experience").append('<div id="preloaderAddSExperience" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddExperience = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/AddExperience",
		data: { experience },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllExperience = null;
				$('#successAddExperience').show('slow');
				window.setTimeout(close_experience_add_form, 3001);
				function close_experience_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddExperience').show('slow');
			}

			isLoadAddExperience = true;
			if (isLoadAddExperienceVisible) {
				$("#preloaderAddSExperience").remove();

				if (!result)
					$('#btnAdd').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function UpdateExperience(index) {
	$("#errorUpdateExperience").fadeOut();

	$("#companyError" + index).fadeOut();
	$("#positionError" + index).fadeOut();
	$("#fromDateError" + index).fadeOut();
	$("#experienceDescriptionError" + index).fadeOut();

	var newCompany = $("#company" + index).val();
	var newPosition = $("#position" + index).val();
	var newFromDate = $("#fromDate" + index).val();
	var newToDate = $("#toDate" + index).val();
	if (newToDate == "по настоящее время")
		newToDate = null;
	var newExperienceDescription = $("#experienceDescription" + index).text();

	if (newCompany == null || newCompany == "") {
		$("#companyError" + index).fadeIn();
		return;
	}

	if (newPosition == null || newPosition == "") {
		$("#positionError" + index).fadeIn();
		return;
	}

	if (newFromDate == null || newFromDate == "") {
		$("#fromDateError" + index).fadeIn();
		return;
	}

	if (newExperienceDescription == null || newExperienceDescription == "") {
		$("#experienceDescriptionError" + index).fadeIn();
		return;
	}

	if (newCompany == currentAllExperience[index].company
		&& newPosition == currentAllExperience[index].position
		&& newFromDate == currentAllExperience[index].fromDate
		&& newToDate == currentAllExperience[index].toDate
		&& newExperienceDescription == currentAllExperience[index].description)
		return;

	var experience = {
		Id: currentAllExperience[index].id,
		Company: newCompany,
		Position: newPosition,
		Description: newExperienceDescription,
		FromDate: newFromDate,
		ToDate: newToDate
	};

	var isLoadUpdateExperience = false;
	var isLoadUpdateExperienceVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateExperience) {
			isLoadUpdateExperienceVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#experience").append('<div id="preloaderUpdateExperience" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateExperience = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/UpdateExperience",
		data: { experience },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllExperience[index].company = experience.Company;
				currentAllExperience[index].position = experience.Position;
				currentAllExperience[index].description = experience.Description;
				currentAllExperience[index].fromDate = experience.FromDate;
				if (experience.ToDate == null)
					currentAllExperience[index].toDate = "по настоящее время";
				else
					currentAllExperience[index].toDate = experience.ToDate;
				$('#successUpdateExperience').show('slow');
				window.setTimeout(close_experience_update_form, 3001);
				function close_experience_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateExperience').show('slow');
			}

			isLoadUpdateExperience = true;
			if (isLoadUpdateExperienceVisible) {
				$("#preloaderUpdateExperience").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function DeleteExperience(index) {
	$("#errorDeleteExperience").fadeOut();

	var isLoadDeleteExperience = false;
	var isLoadDeleteExperienceVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadDeleteExperience) {
			isLoadDeleteExperienceVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#experience").append('<div id="preloaderDeleteExperience" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadDeleteExperience = false;
	};

	var experienceId = currentAllExperience[index].id;
	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/DeleteExperience",
		data: { experienceId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllExperience[index] = null;
				$('#successDeleteExperience').show('slow');
				window.setTimeout(close_experience_delete_form, 3001);
				function close_experience_delete_form() {
					CloseForm();
				};
			}
			else {
				$('#errorDeleteExperience').show('slow');
			}

			isLoadDeleteExperience = true;
			if (isLoadDeleteExperienceVisible) {
				$("#preloaderDeleteExperience").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function AllEducation() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "allEducation";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (currentAllEducation == null) {
		var isLoadAllEducation = false;
		var isLoadAllEducationVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadAllEducation) {
				isLoadAllEducationVisible = true;
				$("#allEducation").append('<div id="preloaderAllEducation" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadAllEducation = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonBusinessCard/GetEducation",
			dataTpye: "json",
			success: function (allEducation) {

				currentAllEducation = allEducation;

				isLoadAllEducation = true;
				if (isLoadAllEducationVisible) {
					$("#preloaderAllEducation").remove();
				}
				SetUpdateAllEducationForm();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateAllEducationForm();
}

function SetUpdateAllEducationForm() {
	var inputsData = document.getElementById('allEducation');

	for (var i = 0; i < currentAllEducation.length; i++) {
		if (currentAllEducation[i] == null)
			continue;

		var formButton = document.createElement("button");
		formButton.id = "btn" + i;
		formButton.className = "btn-saveData";
		formButton.setAttribute("onclick", "Education(" + i + ")");
		$(formButton).css("background", "#211F30");
		$(formButton).css("color", "#87CEFA");
		$(formButton).css("margin-top", "12px");
		$(formButton).css("margin-bottom", "12px");
		formButton.innerHTML = currentAllEducation[i].organization;

		inputsData.append(formButton);
	}

	var formButton = document.createElement("button");
	formButton.className = "btn-saveData";
	formButton.setAttribute("onclick", "EducationFormForAdd()");
	$(formButton).css("background", "#87CEFA");
	$(formButton).css("margin-top", "12px");
	$(formButton).css("margin-bottom", "12px");
	formButton.innerHTML = "Добавить обучение";
	inputsData.append(formButton);
}

function EducationFormForAdd() {
	$("#allEducation").fadeOut();

	window.setTimeout(show_education, 501);
	function show_education() {
		var inputsData = document.createElement("div");
		inputsData.id = "education";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="organization" class="input" required="required" autocomplete="off"><span class="placeholder">Место учёбы</span><p id="organizationError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="fromDate" class="input" required="required" autocomplete="off"><span class="placeholder">Начало учёбы</span><p id="fromDateError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="toDate" class="input" required="required" autocomplete="off"><span class="placeholder">Окончание учёбы</span></div>');

		var editor = document.createElement("div");
		editor.id = "educationDescription";
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		$(inputsData).append(editor);
		$(inputsData).append('<p id="educationDescriptionError" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonSave = document.createElement("button");
		formButtonSave.id = "btnAdd";
		formButtonSave.className = "btn-saveData";
		formButtonSave.setAttribute("onclick", "AddEducation()");
		formButtonSave.innerHTML = "Добавить";
		$(formButtonSave).css("background", "#87CEFA");
		$(inputsData).append(formButtonSave);

		var saveSuccess = document.createElement("p");
		saveSuccess.id = "successAddEducation";
		$(saveSuccess).css("margin-top", "0px");
		$(saveSuccess).css("margin-bottom", "12px");
		saveSuccess.className = "successMessage";
		saveSuccess.innerHTML = 'Обучение успешно добавлено!';
		$(inputsData).append(saveSuccess);

		var saveError = document.createElement("p");
		saveError.id = "errorAddEducation";
		$(saveError).css("margin-top", "0px");
		$(saveError).css("margin-bottom", "12px");
		saveError.className = "errorMessage";
		saveError.innerHTML = 'Не удалось добавить обучение! Попробуйте ещё раз.';
		$(inputsData).append(saveError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);
	};
}

function Education(index) {
	$("#allEducation").fadeOut();

	window.setTimeout(show_education, 501);
	function show_education() {
		var inputsData = document.createElement("div");
		inputsData.id = "education";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="organization' + index + '" class="input" required="required" autocomplete="off"><span class="placeholder">Место учёбы</span><p id="organizationError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="fromDate' + index + '" class="input" required="required" autocomplete="off" value="' + currentAllEducation[index].fromDate + '"><span class="placeholder">Начало учёбы</span><p id="fromDateError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="toDate' + index + '" class="input" required="required" autocomplete="off" value="' + currentAllEducation[index].toDate + '"><span class="placeholder">Окончание учёбы</span></div>');

		var editor = document.createElement("div");
		editor.id = "educationDescription" + index;
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		editor.innerHTML = currentAllEducation[index].description;
		$(inputsData).append(editor);
		$(inputsData).append('<p id="educationDescriptionError' + index + '" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "UpdateEducation(" + index + ")");
		formButtonUpdate.innerHTML = "Обновить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(inputsData).append(formButtonUpdate);

		var formButtonDelete = document.createElement("button");
		formButtonDelete.id = "btnDelete";
		formButtonDelete.className = "btn-saveData";
		formButtonDelete.setAttribute("onclick", "DeleteEducation(" + index + ")");
		formButtonDelete.innerHTML = "Удалить";
		$(inputsData).append(formButtonDelete);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateEducation";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Обучение успешно обновлено!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateEducation";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить обучение! Попробуйте ещё раз.';
		$(inputsData).append(updateError);

		var deleteSuccess = document.createElement("p");
		deleteSuccess.id = "successDeleteEducation";
		$(deleteSuccess).css("margin-top", "0px");
		$(deleteSuccess).css("margin-bottom", "12px");
		deleteSuccess.className = "successMessage";
		deleteSuccess.innerHTML = 'Обуение успешно удалено!';
		$(inputsData).append(deleteSuccess);

		var deleteError = document.createElement("p");
		deleteError.id = "errorDeleteEducation";
		$(deleteError).css("margin-top", "0px");
		$(deleteError).css("margin-bottom", "12px");
		deleteError.className = "errorMessage";
		deleteError.innerHTML = 'Не удалось удалить обучение! Попробуйте ещё раз.';
		$(inputsData).append(deleteError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);

		var input = $('#organization' + index);
		input.val(currentAllEducation[index].organization);
	};
}

function AddEducation() {
	$("#errorAddEducation").fadeOut();

	$("#organizationError").fadeOut();
	$("#fromDateError").fadeOut();
	$("#educationDescriptionError").fadeOut();

	var newOrganization = $("#organization").val();
	var newFromDate = $("#fromDate").val();
	var newToDate = $("#toDate").val();
	var newEducationDescription = $("#educationDescription").text();

	if (newOrganization == null || newOrganization == "") {
		$("#organizationError").fadeIn();
		return;
	}

	if (newFromDate == null || newFromDate == "") {
		$("#fromDateError").fadeIn();
		return;
	}

	if (newEducationDescription == null || newEducationDescription == "") {
		$("#educationDescriptionError").fadeIn();
		return;
	}

	var education = {
		Organization: newOrganization,
		Description: newEducationDescription,
		FromDate: newFromDate,
		ToDate: newToDate
	};

	var isLoadAddEducation = false;
	var isLoadAddEducationVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddEducation) {
			isLoadAddEducationVisible = true;
			$('#btnAdd').fadeOut();
			$("#education").append('<div id="preloaderAddEducation" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddEducation = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/AddEducation",
		data: { education },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllEducation = null;
				$('#successAddEducation').show('slow');
				window.setTimeout(close_education_add_form, 3001);
				function close_education_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddEducation').show('slow');
			}

			isLoadAddEducation = true;
			if (isLoadAddEducationVisible) {
				$("#preloaderAddEducation").remove();

				if (!result)
					$('#btnAdd').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function UpdateEducation(index) {
	$("#errorUpdateEducation").fadeOut();

	$("#organizationError" + index).fadeOut();
	$("#fromDateError" + index).fadeOut();
	$("#educationDescriptionError" + index).fadeOut();

	var newOrganization = $("#organization" + index).val();
	var newFromDate = $("#fromDate" + index).val();
	var newToDate = $("#toDate" + index).val();
	var newEducationDescription = $("#educationDescription" + index).text();
	var newToDate = $("#toDate" + index).val();
	if (newToDate == "по настоящее время")
		newToDate = null;

	if (newOrganization == null || newOrganization == "") {
		$("#organizationError").fadeIn();
		return;
	}

	if (newFromDate == null || newFromDate == "") {
		$("#fromDateError" + index).fadeIn();
		return;
	}

	if (newEducationDescription == null || newEducationDescription == "") {
		$("#educationDescriptionError" + index).fadeIn();
		return;
	}

	if (newOrganization == currentAllEducation[index].organization
		&& newFromDate == currentAllEducation[index].fromDate
		&& newToDate == currentAllEducation[index].toDate
		&& newEducationDescription == currentAllEducation[index].description)
		return;

	var education = {
		Id: currentAllEducation[index].id,
		Organization: newOrganization,
		Description: newEducationDescription,
		FromDate: newFromDate,
		ToDate: newToDate
	};

	var isLoadUpdateEducation = false;
	var isLoadUpdateEducationVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateEducation) {
			isLoadUpdateEducationVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#education").append('<div id="preloaderUpdateEducation" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateEducation = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/UpdateEducation",
		data: { education },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllEducation[index].organization = education.Organization;
				currentAllEducation[index].description = education.Description;
				currentAllEducation[index].fromDate = education.FromDate;
				if (education.ToDate == null)
					currentAllEducation[index].toDate = "по настоящее время";
				else
					currentAllEducation[index].toDate = education.ToDate;
				$('#successUpdateEducation').show('slow');
				window.setTimeout(close_education_update_form, 3001);
				function close_education_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateEducation').show('slow');
			}

			isLoadUpdateEducation = true;
			if (isLoadUpdateEducationVisible) {
				$("#preloaderUpdateEducation").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function DeleteEducation(index) {
	$("#errorDeleteEducation").fadeOut();

	var isLoadDeleteEducation = false;
	var isLoadDeleteEducationVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadDeleteEducation) {
			isLoadDeleteEducationVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#education").append('<div id="preloaderDeleteEducation" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadDeleteEducation = false;
	};

	var educationId = currentAllEducation[index].id;
	$.ajax({
		type: "POST",
		url: "/MAXonBusinessCard/DeleteEducation",
		data: { educationId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentAllEducation[index] = null;
				$('#successDeleteEducation').show('slow');
				window.setTimeout(close_education_delete_form, 3001);
				function close_education_delete_form() {
					CloseForm();
				};
			}
			else {
				$('#errorDeleteEducation').show('slow');
			}

			isLoadDeleteEducation = true;
			if (isLoadDeleteEducationVisible) {
				$("#preloaderDeleteEducation").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function ResumeInfo() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "resumeInfo";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (needWork == null) {
		var isLoadResumeInfo = false;
		var isLoadResumeInfoVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadResumeInfo) {
				isLoadResumeInfoVisible = true;
				$("#resumeInfo").append('<div id="preloaderResumeInfo" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadResumeInfo = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonWork/GetResume",
			dataTpye: "json",
			success: function (resume) {
				if (resume == null) {
					needWork = false;
				}
				else {
					needWork = true;
					currentResume = resume;
                }

				isLoadResumeInfo = true;
				if (isLoadResumeInfoVisible) {
					$("#preloaderResumeInfo").remove();
				}
				SetUpdateResumeInfo();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetUpdateResumeInfo();
}

function SetUpdateResumeInfo() {
	var inputsData = document.getElementById('resumeInfo');

	if (needWork) {
		var formButtonUpdateResume = document.createElement("button");
		formButtonUpdateResume.className = "btn-saveData";
		formButtonUpdateResume.setAttribute("onclick", "Resume()");
		$(formButtonUpdateResume).css("background", "#211F30");
		$(formButtonUpdateResume).css("color", "#87CEFA");
		$(formButtonUpdateResume).css("margin-top", "12px");
		$(formButtonUpdateResume).css("margin-bottom", "12px");
		formButtonUpdateResume.innerHTML = "Обновить резюме";
		inputsData.append(formButtonUpdateResume);
    }

	var formButtonUpdateNeedWork = document.createElement("button");
	formButtonUpdateNeedWork.className = "btn-saveData";
	formButtonUpdateNeedWork.setAttribute("onclick", "NeedWork()");
	$(formButtonUpdateNeedWork).css("background", "#211F30");
	$(formButtonUpdateNeedWork).css("color", "#87CEFA");
	$(formButtonUpdateNeedWork).css("margin-top", "12px");
	$(formButtonUpdateNeedWork).css("margin-bottom", "12px");
	if (needWork)
		formButtonUpdateNeedWork.innerHTML = "Скрыть резюме";
	else
		formButtonUpdateNeedWork.innerHTML = "Открыть резюме";
	inputsData.append(formButtonUpdateNeedWork);
}

function Resume() {
	$("#resumeInfo").fadeOut();

	window.setTimeout(show_resume, 501);
	function show_resume() {
		var inputsData = document.createElement("div");
		inputsData.id = "resume";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="position" class="input" required="required" autocomplete="off"value="' + currentResume.position + '"><span class="placeholder">Позиция</span><p id="positionError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="salary" class="input" required="required" autocomplete="off" value="' + currentResume.salary + '"><span class="placeholder">Зарплата</span><p id="salaryError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="schedule" class="input" required="required" autocomplete="off" value="' + currentResume.schedule + '"><span class="placeholder">Формат работы</span><p id="scheduleError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="technologyStack" class="input" required="required" autocomplete="off"><span class="placeholder">Технологический стек</span><p id="technologyStackError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "UpdateResume()");
		formButtonUpdate.innerHTML = "Подтвердить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(formButtonUpdate).css("margin-top", "0px");
		$(inputsData).append(formButtonUpdate);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateResume";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Резюме успешно обновлено!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateResume";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить резюме!';
		$(inputsData).append(updateError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);

		var input = $('#technologyStack');
		input.val(currentResume.technologyStack);
	}
}

function UpdateResume() {
	$("#errorUpdateResume").fadeOut();

	$("#positionError").fadeOut();
	$("#salaryError").fadeOut();
	$("#scheduleError").fadeOut();
	$("#technologyStackError").fadeOut();

	var newPosition = $("#position").val();
	var newSalary = $("#salary").val();
	var newSchedule = $("#schedule").val();
	var newTechnologyStack = $("#technologyStack").val();

	if (newPosition == null || newPosition == "") {
		$("#positionError").fadeIn();
		return;
	}

	if (newSalary == null || newSalary == "") {
		$("#salaryError").fadeIn();
		return;
	}

	if (newSchedule == null || newSchedule == "") {
		$("#scheduleError").fadeIn();
		return;
	}

	if (newTechnologyStack == null || newTechnologyStack == "") {
		$("#technologyStackError").fadeIn();
		return;
	}

	if (newPosition == currentResume.position
		&& newSalary == currentResume.salary
		&& newSchedule == currentResume.schedule
		&& newTechnologyStack == currentResume.technologyStack)
		return;

	var resume = {
		Id: currentResume.id,
		Position: newPosition,
		Salary: newSalary,
		Schedule: newSchedule,
		TechnologyStack: newTechnologyStack
	};

	var isLoadUpdateResume = false;
	var isLoadUpdateResumeVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateResume) {
			isLoadUpdateResumeVisible = true;
			$('#btnUpdate').fadeOut();
			$("#education").append('<div id="preloaderUpdateResume" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateResume = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonWork/UpdateResume",
		data: { resume },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentResume = resume;
				$('#successUpdateResume').show('slow');
				window.setTimeout(close_resume_update_form, 3001);
				function close_resume_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateResume').show('slow');
			}

			isLoadUpdateResume = true;
			if (isLoadUpdateResumeVisible) {
				$("#preloaderUpdateResume").remove();

				if (!result) {
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function NeedWork() {
	$("#resumeInfo").fadeOut();

	window.setTimeout(show_need_work, 501);
	function show_need_work() {
		var inputsData = document.createElement("div");
		inputsData.id = "needWork";
		inputsData.className = "inputData";

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "RewriteNeedWork()");
		formButtonUpdate.innerHTML = "Подтвердить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(formButtonUpdate).css("margin-top", "0px");
		$(inputsData).append(formButtonUpdate);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateNeedWork";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		if (needWork)
			updateSuccess.innerHTML = 'Резюме успешно скрыто!';
		else
			updateSuccess.innerHTML = 'Резюме успешно открыто!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateNeedWork";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		if (needWork)
			updateError.innerHTML = 'Не удалось скрыть резюме!';
		else
			updateError.innerHTML = 'Не удаллсь открыть резюме!';
		$(inputsData).append(updateError);

		$('.center').append(inputsData);
	}
}

function RewriteNeedWork() {
	var value = !needWork;

	$("#errorUpdateNeedWork").fadeOut();

	var isLoadUpdateNeedWork = false;
	var isLoadUpdateNeedWorkVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateNeedWork) {
			isLoadUpdateNeedWorkVisible = true;
			$('#btnUpdate').fadeOut();
			$("#needWork").append('<div id="preloaderUpdateNeedWork" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateNeedWork = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonWork/UpdateFlagForNeedWork",
		data: { value },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				needWork = value;
				$('#successUpdateNeedWork').show('slow');
				window.setTimeout(close_need_work_update_form, 3001);
				function close_need_work_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateNeedWork').show('slow');
			}

			isLoadUpdateNeedWork = true;
			if (isLoadUpdateNeedWorkVisible) {
				$("#preloaderUpdateNeedWork").remove();

				if (!result) {
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function ServicesMenu() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "servicesMenu";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);
	html.append(fullWindow);

	if (mainDataByServices == null) {
		var isLoadDataByServices = false;
		var isLoadDataByServicesVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadDataByServices) {
				isLoadDataByServicesVisible = true;
				$("#servicesMenu").append('<div id="preloaderServicesMenu" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadDataByServices = false;
		};

		$.ajax({
			type: "GET",
			url: "/MAXonService/GetAllServices",
			dataTpye: "json",
			success: function (services) {
				mainDataByServices = services;

				isLoadDataByServices = true;
				if (isLoadDataByServicesVisible) {
					$("#preloaderServicesMenu").remove();
				}
				SetServicesMenu();
			},
			error: function (error) {
				alert(error);

				CloseForm();
			}
		});

		return;
	}

	SetServicesMenu();
}

function SetServicesMenu() {
	var inputsData = document.getElementById('servicesMenu');

	for (var i = 0; i < mainDataByServices.length; i++) {
		var formButtonService = document.createElement("button");
		formButtonService.className = "btn-saveData";
		formButtonService.setAttribute("onclick", "ServiceMenu(" + i + ")");
		$(formButtonService).css("background", "#211F30");
		$(formButtonService).css("color", "#87CEFA");
		$(formButtonService).css("margin-top", "12px");
		$(formButtonService).css("margin-bottom", "12px");
		formButtonService.innerHTML = mainDataByServices[i].name;
		inputsData.append(formButtonService);
	}

	var formButtonService = document.createElement("button");
	formButtonService.className = "btn-saveData";
	formButtonService.setAttribute("onclick", "CreateService()");
	$(formButtonService).css("background", "#211F30");
	$(formButtonService).css("color", "#E29930");
	$(formButtonService).css("margin-top", "12px");
	$(formButtonService).css("margin-bottom", "12px");
	formButtonService.innerHTML = "Создать сервис";
	inputsData.append(formButtonService);
}

function ServiceMenu(index) {
	$("#servicesMenu").fadeOut();

	window.setTimeout(show_service_menu, 501);
	function show_service_menu() {
		var inputsData = document.createElement("div");
		inputsData.id = "serviceMenu";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		var formButtonServices = document.createElement("button");
		formButtonServices.className = "btn-saveData";
		formButtonServices.setAttribute("onclick", "UpdateServiceForm(" + index + ")");
		$(formButtonServices).css("background", "#211F30");
		$(formButtonServices).css("color", "#87CEFA");
		$(formButtonServices).css("margin-top", "12px");
		$(formButtonServices).css("margin-bottom", "12px");
		formButtonServices.innerHTML = "Данные";
		inputsData.append(formButtonServices);

		var formButtonServicesConditions = document.createElement("button");
		formButtonServicesConditions.className = "btn-saveData";
		formButtonServicesConditions.setAttribute("onclick", "UpdateConditionsForm(" + index + ")");
		$(formButtonServicesConditions).css("background", "#211F30");
		$(formButtonServicesConditions).css("color", "#87CEFA");
		$(formButtonServicesConditions).css("margin-top", "12px");
		$(formButtonServicesConditions).css("margin-bottom", "12px");
		formButtonServicesConditions.innerHTML = "Условия";
		inputsData.append(formButtonServicesConditions);

		var formButtonServicesConditions = document.createElement("button");
		formButtonServicesConditions.className = "btn-saveData";
		formButtonServicesConditions.setAttribute("onclick", "UpdateRatesForm(" + index + ")");
		$(formButtonServicesConditions).css("background", "#211F30");
		$(formButtonServicesConditions).css("color", "#87CEFA");
		$(formButtonServicesConditions).css("margin-top", "12px");
		$(formButtonServicesConditions).css("margin-bottom", "12px");
		formButtonServicesConditions.innerHTML = "Тарифы";
		inputsData.append(formButtonServicesConditions);

		$('.center').append(inputsData);
	}
}

function Update(index) {
	$("#serviceMenu").fadeOut();

	window.setTimeout(show_update_service_form, 501);
	function show_update_service_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "updateServiceForm";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="nameService" class="input" required="required" autocomplete="off"><span class="placeholder">Наименование</span><p id="nameServiceError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="shortDescriptFirst" class="input" required="required" autocomplete="off"><span class="placeholder">Краткое описание №1</span><p id="shortDescriptFirstError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="shortDescriptSecond" class="input" required="required" autocomplete="off"><span class="placeholder">Краткое описание №2</span><p id="shortDescriptSecondError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="shortDescriptThird" class="input" required="required" autocomplete="off"><span class="placeholder">Краткое описание №3</span><p id="shortDescriptThirdError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="price" class="input" required="required" autocomplete="off"><span class="placeholder">Цена</span><p id="priceError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var editor = document.createElement("div");
		editor.id = "fullDescript";
		editor.className = "editor";
		editor.setAttribute("contenteditable", "true");
		editor.setAttribute("data-placeholder", "Описание");
		editor.innerHTML = mainDataByServices[index].description;
		$(inputsData).append(editor);
		$(inputsData).append('<p id="fullDescriptError" class="errorMessage">Это поле является обязательным для заполнения.</p>');

		var formButtonServices = document.createElement("button");
		formButtonServices.className = "btn-saveData";
		formButtonServices.setAttribute("onclick", "UpdateServiceForm(" + index + ")");
		$(formButtonServices).css("background", "#211F30");
		$(formButtonServices).css("color", "#87CEFA");
		$(formButtonServices).css("margin-top", "12px");
		$(formButtonServices).css("margin-bottom", "12px");
		formButtonServices.innerHTML = "Данные";
		inputsData.append(formButtonServices);

		var formButtonServicesConditions = document.createElement("button");
		formButtonServicesConditions.className = "btn-saveData";
		formButtonServicesConditions.setAttribute("onclick", "UpdateConditionsForm(" + index + ")");
		$(formButtonServicesConditions).css("background", "#211F30");
		$(formButtonServicesConditions).css("color", "#87CEFA");
		$(formButtonServicesConditions).css("margin-top", "12px");
		$(formButtonServicesConditions).css("margin-bottom", "12px");
		formButtonServicesConditions.innerHTML = "Условия";
		inputsData.append(formButtonServicesConditions);

		var formButtonServicesConditions = document.createElement("button");
		formButtonServicesConditions.className = "btn-saveData";
		formButtonServicesConditions.setAttribute("onclick", "UpdateRatesForm(" + index + ")");
		$(formButtonServicesConditions).css("background", "#211F30");
		$(formButtonServicesConditions).css("color", "#87CEFA");
		$(formButtonServicesConditions).css("margin-top", "12px");
		$(formButtonServicesConditions).css("margin-bottom", "12px");
		formButtonServicesConditions.innerHTML = "Тарифы";
		inputsData.append(formButtonServicesConditions);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);

		$('#nameService').val(mainDataByServices[index].name);

		$('#shortDescriptFirst').val(mainDataByServices[index].shortDescriptions[0]);
		$('#shortDescriptSecond').val(mainDataByServices[index].shortDescriptions[1]);
		$('#shortDescriptThird').val(mainDataByServices[index].shortDescriptions[2]);

		$('#price').val(mainDataByServices[index].price);
	}
}

function CreateService() {
	var url = "/MAXonService/ServiceCreator";
	$(location).attr('href', url);
}

function UpdateServiceForm(index) {
	var url = "/MAXonService/ServiceUpdater/" + mainDataByServices[index].id; 
	$(location).attr('href', url);
}

function UpdateConditionsForm(index) {
	$("#serviceMenu").fadeOut();

	window.setTimeout(show_update_service_conditions_form, 501);
	function show_update_service_conditions_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "updateServiceConditionsForm";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		$('.fullWindow.container').css("width", "471px");
		$('.center').append(inputsData);

		if (currentConditions[index] == undefined) {
			var isLoadServiceConditions = false;
			var isLoadServiceConditionsVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadServiceConditions) {
					isLoadServiceConditionsVisible = true;
					$("#updateServiceConditionsForm").append('<div id="preloaderConditions" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadServiceConditions = false;
			};

			var serviceId = mainDataByServices[index].id;

			$.ajax({
				type: "GET",
				url: "/ServiceConditions/GetServiceConditions",
				data: { serviceId },
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					currentConditions[index] = data;

					isLoadServiceConditions = true;
					if (isLoadServiceConditionsVisible) {
						$("#preloaderConditions").remove();
					}
					SetServiceConditions(index);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetServiceConditions(index);
	}
}

function SetServiceConditions(index) {
	var inputsData = document.getElementById('updateServiceConditionsForm');

	for (var i = 0; i < currentConditions[index].length; i++) {
		var formButtonServiceCondition = document.createElement("button");
		formButtonServiceCondition.className = "btn-saveData";
		formButtonServiceCondition.setAttribute("onclick", "Condition(" + index + ", " + i + ")");
		$(formButtonServiceCondition).css("background", "#211F30");
		$(formButtonServiceCondition).css("color", "#87CEFA");
		$(formButtonServiceCondition).css("margin-top", "12px");
		$(formButtonServiceCondition).css("margin-bottom", "12px");
		formButtonServiceCondition.innerHTML = currentConditions[index][i].text;
		inputsData.append(formButtonServiceCondition);
	}

	var formButtonServiceCondition = document.createElement("button");
	formButtonServiceCondition.className = "btn-saveData";
	formButtonServiceCondition.setAttribute("onclick", "ConditionFormForAdd(" + mainDataByServices[index].id + ")");
	$(formButtonServiceCondition).css("background", "#211F30");
	$(formButtonServiceCondition).css("color", "#E29930");
	$(formButtonServiceCondition).css("margin-top", "12px");
	$(formButtonServiceCondition).css("margin-bottom", "12px");
	formButtonServiceCondition.innerHTML = "Добваить условие";
	inputsData.append(formButtonServiceCondition);

	var formButtonServiceCondition = document.createElement("button");
	formButtonServiceCondition.className = "btn-saveData";
	formButtonServiceCondition.setAttribute("onclick", "ServiceCalculatedValuesForm(" + mainDataByServices[index].id + ")");
	$(formButtonServiceCondition).css("background", "#211F30");
	$(formButtonServiceCondition).css("color", "#E29930");
	$(formButtonServiceCondition).css("margin-top", "12px");
	$(formButtonServiceCondition).css("margin-bottom", "12px");
	formButtonServiceCondition.innerHTML = "Вычисляемые значения";
	inputsData.append(formButtonServiceCondition);
}

function ConditionFormForAdd(serviceId) {
	$("#updateServiceConditionsForm").fadeOut();

	window.setTimeout(show_condition, 501);
	function show_condition() {
		var inputsData = document.createElement("div");
		inputsData.id = "condition";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="conditionText" class="input" required="required" autocomplete="off"><span class="placeholder">Условие</span><p id="conditionError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var formButtonSave = document.createElement("button");
		formButtonSave.id = "btnAdd";
		formButtonSave.className = "btn-saveData";
		formButtonSave.setAttribute("onclick", "AddCondition(" + serviceId + ")");
		formButtonSave.innerHTML = "Добавить";
		$(formButtonSave).css("background", "#87CEFA");
		$(inputsData).append(formButtonSave);

		var saveSuccess = document.createElement("p");
		saveSuccess.id = "successAddCondition";
		$(saveSuccess).css("margin-top", "0px");
		$(saveSuccess).css("margin-bottom", "12px");
		saveSuccess.className = "successMessage";
		saveSuccess.innerHTML = 'Условие успешно добавлено!';
		$(inputsData).append(saveSuccess);

		var saveError = document.createElement("p");
		saveError.id = "errorAddCondition";
		$(saveError).css("margin-top", "0px");
		$(saveError).css("margin-bottom", "12px");
		saveError.className = "errorMessage";
		saveError.innerHTML = 'Не удалось добавить условие! Попробуйте ещё раз.';
		$(inputsData).append(saveError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);
	};
}

function Condition(index, secondIndex) {
	$("#updateServiceConditionsForm").fadeOut();

	window.setTimeout(show_condition, 501);
	function show_condition() {
		var inputsData = document.createElement("div");
		inputsData.id = "condition";
		inputsData.className = "inputData";

		$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="conditionText" class="input" required="required" autocomplete="off"><span class="placeholder">Условие</span><p id="conditionError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

		var formButtonUpdate = document.createElement("button");
		formButtonUpdate.id = "btnUpdate";
		formButtonUpdate.className = "btn-saveData";
		formButtonUpdate.setAttribute("onclick", "UpdateCondition(" + index + ", " + secondIndex + ")");
		formButtonUpdate.innerHTML = "Обновить";
		$(formButtonUpdate).css("background", "#87CEFA");
		$(inputsData).append(formButtonUpdate);

		var formButtonDelete = document.createElement("button");
		formButtonDelete.id = "btnDelete";
		formButtonDelete.className = "btn-saveData";
		formButtonDelete.setAttribute("onclick", "DeleteCondition(" + index + ", " + secondIndex + ")");
		formButtonDelete.innerHTML = "Удалить";
		$(inputsData).append(formButtonDelete);

		var updateSuccess = document.createElement("p");
		updateSuccess.id = "successUpdateCondition";
		$(updateSuccess).css("margin-top", "0px");
		$(updateSuccess).css("margin-bottom", "12px");
		updateSuccess.className = "successMessage";
		updateSuccess.innerHTML = 'Условие успешно обновлено!';
		$(inputsData).append(updateSuccess);

		var updateError = document.createElement("p");
		updateError.id = "errorUpdateCondition";
		$(updateError).css("margin-top", "0px");
		$(updateError).css("margin-bottom", "12px");
		updateError.className = "errorMessage";
		updateError.innerHTML = 'Не удалось обновить условие! Попробуйте ещё раз.';
		$(inputsData).append(updateError);

		var deleteSuccess = document.createElement("p");
		deleteSuccess.id = "successDeleteCondition";
		$(deleteSuccess).css("margin-top", "0px");
		$(deleteSuccess).css("margin-bottom", "12px");
		deleteSuccess.className = "successMessage";
		deleteSuccess.innerHTML = 'Условие успешно удалено!';
		$(inputsData).append(deleteSuccess);

		var deleteError = document.createElement("p");
		deleteError.id = "errorDeleteCondition";
		$(deleteError).css("margin-top", "0px");
		$(deleteError).css("margin-bottom", "12px");
		deleteError.className = "errorMessage";
		deleteError.innerHTML = 'Не удалось удалить условие! Попробуйте ещё раз.';
		$(inputsData).append(deleteError);

		$('.container').css("width", "auto");

		$('.center').append(inputsData);

		var input = $('#conditionText');
		input.val(currentConditions[index][secondIndex].text);
	};
}

function AddCondition(serviceId) {
	$("#errorAddCondition").fadeOut();

	$("#conditionError").fadeOut();

	var newCondition = $("#conditionText").val();

	if (newCondition == null || newCondition == "") {
		$("#conditionError").fadeIn();
		return;
	}

	var condition = {
		Text: newCondition,
		ServiceId: serviceId
	};

	var isLoadAddCondition = false;
	var isLoadAddConditionVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddCondition) {
			isLoadAddConditionVisible = true;
			$('#btnAdd').fadeOut();
			$("#condition").append('<div id="preloaderAddCondition" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddCondition = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceConditions/AddCondition",
		data: { condition },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentConditions = [];
				$('#successAddCondition').show('slow');
				window.setTimeout(close_condition_add_form, 3001);
				function close_condition_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddCondition').show('slow');
			}

			isLoadAddCondition = true;
			if (isLoadAddConditionVisible) {
				$("#preloaderAddCondition").remove();

				if (!result)
					$('#btnAdd').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function UpdateCondition(index, secondIndex) {
	$("#errorUpdateCondition").fadeOut();

	$("#conditionError").fadeOut();

	var newCondition = $("#conditionText").val();

	if (newCondition == null || newCondition == "") {
		$("#conditionError").fadeIn();
		return;
	}

	if (newCondition == currentConditions[index][secondIndex].text)
		return;

	var condition = {
		Id: currentConditions[index][secondIndex].id,
		Text: newCondition,
		ServiceId: currentConditions[index][secondIndex].serviceId
	};

	var isLoadUpdateCondition = false;
	var isLoadUpdateConditionVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateCondition) {
			isLoadUpdateConditionVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#condition").append('<div id="preloaderUpdateCondition" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateCondition = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceConditions/UpdateCondition",
		data: { condition },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentConditions[index][secondIndex].text = condition.Text;
				$('#successUpdateCondition').show('slow');
				window.setTimeout(close_condition_update_form, 3001);
				function close_condition_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateCondition').show('slow');
			}

			isLoadUpdateCondition = true;
			if (isLoadUpdateConditionVisible) {
				$("#preloaderUpdateCondition").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function DeleteCondition(index, secondIndex) {
	$("#errorDeleteCondition").fadeOut();

	var isLoadDeleteCondition = false;
	var isLoadDeleteConditionVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadDeleteCondition) {
			isLoadDeleteConditionVisible = true;
			$('#btnDelete').fadeOut();
			$('#btnUpdate').fadeOut();
			$("#condition").append('<div id="preloaderDeleteCondition" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadDeleteCondition = false;
	};

	var conditionId = currentConditions[index][secondIndex].id;
	$.ajax({
		type: "POST",
		url: "/ServiceConditions/DeleteCondition",
		data: { conditionId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentConditions[index] = undefined;
				$('#successDeleteCondition').show('slow');
				window.setTimeout(close_condition_delete_form, 3001);
				function close_condition_delete_form() {
					CloseForm();
				};
			}
			else {
				$('#errorDeleteCondition').show('slow');
			}

			isLoadDeleteCondition = true;
			if (isLoadDeleteConditionVisible) {
				$("#preloaderDeleteCondition").remove();

				if (!result) {
					$('#btnDelete').fadeIn();
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function ServiceCalculatedValuesForm(serviceId) {
	$("#updateServiceConditionsForm").fadeOut();

	window.setTimeout(show_calculated_values, 501);
	function show_calculated_values() {
		var inputsData = document.createElement("div");
		inputsData.id = "updateServiceCalculatedValuesForm";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		$('.fullWindow.container').css("width", "471px");
		$('.center').append(inputsData);

		if (currentCalculatedValues[serviceId] == undefined) {
			var isLoadCalculatedValues = false;
			var isLoadCalculatedValuesVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadCalculatedValues) {
					isLoadCalculatedValuesVisible = true;
					$("#updateServiceCalculatedValuesForm").append('<div id="preloaderServiceCalculatedValues" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadCalculatedValues = false;
			};

			$.ajax({
				type: "GET",
				url: "/ServiceConditions/GetServiceCalculatedValues",
				data: { serviceId },
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					currentCalculatedValues[serviceId] = data;

					isLoadCalculatedValues = true;
					if (isLoadCalculatedValuesVisible) {
						$("#preloaderServiceCalculatedValues").remove();
					}
					SetServicCalculatedValues(serviceId);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetServicCalculatedValues(serviceId);
    }
}

function SetServicCalculatedValues(serviceId) {
	var inputsData = document.getElementById('updateServiceCalculatedValuesForm');

	for (var i = 0; i < currentCalculatedValues[serviceId].length; i++) {
		var formButtonServiceCalculatedValue = document.createElement("button");
		formButtonServiceCalculatedValue.className = "btn-saveData";
		formButtonServiceCalculatedValue.setAttribute("onclick", "CalculatedValue(" + serviceId + ", " + i + ")");
		$(formButtonServiceCalculatedValue).css("background", "#211F30");
		$(formButtonServiceCalculatedValue).css("color", "#87CEFA");
		$(formButtonServiceCalculatedValue).css("margin-top", "12px");
		$(formButtonServiceCalculatedValue).css("margin-bottom", "12px");
		formButtonServiceCalculatedValue.innerHTML = currentCalculatedValues[serviceId][i].value + " (" + currentCalculatedValues[serviceId][i].number + ")";
		inputsData.append(formButtonServiceCalculatedValue);
	}

	var formButtonServiceCalculatedValue = document.createElement("button");
	formButtonServiceCalculatedValue.className = "btn-saveData";
	formButtonServiceCalculatedValue.setAttribute("onclick", "CalculatedValueFormForAdd(" + serviceId + ")");
	$(formButtonServiceCalculatedValue).css("background", "#211F30");
	$(formButtonServiceCalculatedValue).css("color", "#E29930");
	$(formButtonServiceCalculatedValue).css("margin-top", "12px");
	$(formButtonServiceCalculatedValue).css("margin-bottom", "12px");
	formButtonServiceCalculatedValue.innerHTML = "Добваить вычисляемое значение";
	inputsData.append(formButtonServiceCalculatedValue);
}

function CalculatedValueFormForAdd(serviceId) {
	$("#updateServiceCalculatedValuesForm").fadeOut();
	window.setTimeout(show_calculated_value_form_add, 501);
	function show_calculated_value_form_add() {
		var inputsData = document.createElement("div");
		inputsData.id = "calculatedValue";
		inputsData.className = "inputData";

		$('.fullWindow.container').css("width", "471px");
		$('.center').append(inputsData);

		if (calculatedValueVariants == null) {
			var isLoadCalculatedValueVariants = false;
			var isLoadCalculatedValueVariantsVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadCalculatedValueVariants) {
					isLoadCalculatedValueVariantsVisible = true;
					$("#calculatedValue").append('<div id="preloaderCalculatedValue" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadCalculatedValueVariants = false;
			};

			$.ajax({
				type: "GET",
				url: "/ServiceConditions/GetCalculatedValues",
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					calculatedValueVariants = data;

					isLoadCalculatedValueVariants = true;
					if (isLoadCalculatedValueVariantsVisible) {
						$("#preloaderCalculatedValue").remove();
					}
					SetServicCalculatedValueForm(serviceId);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetServicCalculatedValueForm(serviceId);
	}
}

function SetServicCalculatedValueForm(serviceId) {
	var inputsData = document.getElementById('calculatedValue');

	$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="numberValue" class="input" required="required" autocomplete="off"><span class="placeholder">Количество</span><p id="numberValueError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');

	var calculatedValueVariantsBlock = document.createElement("div");
	calculatedValueVariantsBlock.className = "calculatedValueVariantsBlock";

	var valueVariants = document.createElement("select");
	valueVariants.className = "valueVariants";

	for (var i = 0; i < calculatedValueVariants.length; i++) {
		var processInSelector = document.createElement("option");
		processInSelector.className = "processInSelector";
		$(processInSelector).val(calculatedValueVariants[i].id);
		processInSelector.innerHTML = calculatedValueVariants[i].value;
		$(valueVariants).append(processInSelector);
	}

	$(calculatedValueVariantsBlock).append(valueVariants);
	$(inputsData).append(calculatedValueVariantsBlock);

	var formButtonAdd = document.createElement("button");
	formButtonAdd.id = "btnAdd";
	formButtonAdd.className = "btn-saveData";
	formButtonAdd.setAttribute("onclick", "AddCalculatedValue(" + serviceId + ")");
	formButtonAdd.innerHTML = "Добавить";
	$(formButtonAdd).css("background", "#87CEFA");
	$(inputsData).append(formButtonAdd);

	var addSuccess = document.createElement("p");
	addSuccess.id = "successAddCalculatedValue";
	$(addSuccess).css("margin-top", "0px");
	$(addSuccess).css("margin-bottom", "12px");
	addSuccess.className = "successMessage";
	addSuccess.innerHTML = 'Вычисляемое значение успешно добавлено!';
	$(inputsData).append(addSuccess);

	var addError = document.createElement("p");
	addError.id = "errorAddCalculatedValue";
	$(addError).css("margin-top", "0px");
	$(addError).css("margin-bottom", "12px");
	addError.className = "errorMessage";
	addError.innerHTML = 'Не удалось добавить вычисляемое значение! Попробуйте ещё раз.';
	$(inputsData).append(addError);

	$('.container').css("width", "auto");

	$('.center').append(inputsData);
}

function AddCalculatedValue(serviceId) {
	$("#errorAddCalculatedValue").fadeOut();

	$("#numberValueError").fadeOut();

	var numberValue = $("#numberValue").val();

	if (numberValue == null || numberValue == "") {
		$("#numberValueError").fadeIn();
		return;
	}

	var calculatedValueId = $('.valueVariants').val();

	var serviceCalculatedValue = {
		Number: numberValue,
		ServiceId: serviceId,
		CalculatedValueId: calculatedValueId
	};

	var isLoadAddCalculatedValue = false;
	var isLoadAddCalculatedValueVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddCalculatedValue) {
			isLoadAddCalculatedValueVisible = true;
			$('#btnAdd').fadeOut();
			$("#calculatedValue").append('<div id="preloaderAddCalculatedValue" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddCalculatedValue = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceConditions/AddServiceCalculatedValue",
		data: { serviceCalculatedValue },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentCalculatedValues = [];
				$('#successAddCalculatedValue').show('slow');
				window.setTimeout(close_calculated_value_add_form, 3001);
				function close_calculated_value_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddCalculatedValue').show('slow');
			}

			isLoadAddCalculatedValue = true;
			if (isLoadAddCalculatedValueVisible) {
				$("#preloaderAddCalculatedValue").remove();

				if (!result)
					$('#btnAdd').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function CalculatedValue(serviceId, index) {
	$("#updateServiceCalculatedValuesForm").fadeOut();
	window.setTimeout(show_calculated_value_form, 501);
	function show_calculated_value_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "calculatedValue";
		inputsData.className = "inputData";

		$('.fullWindow.container').css("width", "471px");
		$('.center').append(inputsData);

		if (calculatedValueVariants == null) {
			var isLoadCalculatedValueVariants = false;
			var isLoadCalculatedValueVariantsVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadCalculatedValueVariants) {
					isLoadCalculatedValueVariantsVisible = true;
					$("#calculatedValue").append('<div id="preloaderCalculatedValue" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadCalculatedValueVariants = false;
			};

			$.ajax({
				type: "GET",
				url: "/ServiceConditions/GetCalculatedValues",
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					calculatedValueVariants = data;

					isLoadCalculatedValueVariants = true;
					if (isLoadCalculatedValueVariantsVisible) {
						$("#preloaderCalculatedValue").remove();
					}
					SetServicCalculatedValueFormForExistingValue(serviceId, index);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetServicCalculatedValueFormForExistingValue(serviceId, index);
	}
}

function SetServicCalculatedValueFormForExistingValue(serviceId, index) {
	var inputsData = document.getElementById('calculatedValue');

	$(inputsData).append('<div class="inputbox" style="margin-bottom: 22px;"><input id="numberValue" class="input" required="required" autocomplete="off"><span class="placeholder">Количество</span><p id="numberValueError" class="errorMessage">Это поле является обязательным для заполнения.</p></div>');
	$('#numberValue').val(currentCalculatedValues[serviceId][index].number);

	var calculatedValueVariantsBlock = document.createElement("div");
	calculatedValueVariantsBlock.className = "calculatedValueVariantsBlock";

	var valueVariants = document.createElement("select");
	valueVariants.className = "valueVariants";

	for (var i = 0; i < calculatedValueVariants.length; i++) {
		var processInSelector = document.createElement("option");
		processInSelector.className = "processInSelector";
		$(processInSelector).val(calculatedValueVariants[i].id);
		processInSelector.innerHTML = calculatedValueVariants[i].value;
		$(valueVariants).append(processInSelector);
	}

	$(calculatedValueVariantsBlock).append(valueVariants);
	$(inputsData).append(calculatedValueVariantsBlock);

	$('.valueVariants option[value=' + currentCalculatedValues[serviceId][index].calculatedValueId + ']').prop('selected', true);

	var formButtonUpdate = document.createElement("button");
	formButtonUpdate.id = "btnUpdate";
	formButtonUpdate.className = "btn-saveData";
	formButtonUpdate.setAttribute("onclick", "UpdateCalculatedValue(" + serviceId + ", " + index + ")");
	formButtonUpdate.innerHTML = "Обновить";
	$(formButtonUpdate).css("background", "#87CEFA");
	$(inputsData).append(formButtonUpdate);

	var formButtonDelete = document.createElement("button");
	formButtonDelete.id = "btnDelete";
	formButtonDelete.className = "btn-saveData";
	formButtonDelete.setAttribute("onclick", "DeleteCalculatedValue(" + serviceId + ", " + index + ")");
	formButtonDelete.innerHTML = "Удалить";
	$(formButtonDelete).css("background", "#E29930");
	$(inputsData).append(formButtonDelete);

	var updateSuccess = document.createElement("p");
	updateSuccess.id = "successUpdateCalculatedValue";
	$(updateSuccess).css("margin-top", "0px");
	$(updateSuccess).css("margin-bottom", "12px");
	updateSuccess.className = "successMessage";
	updateSuccess.innerHTML = 'Вычисляемое значение успешно обновлено!';
	$(inputsData).append(updateSuccess);

	var updateError = document.createElement("p");
	updateError.id = "errorUpdateCalculatedValue";
	$(updateError).css("margin-top", "0px");
	$(updateError).css("margin-bottom", "12px");
	updateError.className = "errorMessage";
	updateError.innerHTML = 'Не удалось обновить вычисляемое значение! Попробуйте ещё раз.';
	$(inputsData).append(updateError);

	var deleteSuccess = document.createElement("p");
	deleteSuccess.id = "successDeleteCalculatedValue";
	$(deleteSuccess).css("margin-top", "0px");
	$(deleteSuccess).css("margin-bottom", "12px");
	deleteSuccess.className = "successMessage";
	deleteSuccess.innerHTML = 'Вычисляемое значение успешно удалено!';
	$(inputsData).append(deleteSuccess);

	var deleteError = document.createElement("p");
	deleteError.id = "errorDeleteCalculatedValue";
	$(deleteError).css("margin-top", "0px");
	$(deleteError).css("margin-bottom", "12px");
	deleteError.className = "errorMessage";
	deleteError.innerHTML = 'Не удалось удалить вычисляемое значение! Попробуйте ещё раз.';
	$(inputsData).append(deleteError);

	$('.container').css("width", "auto");

	$('.center').append(inputsData);
}

function UpdateCalculatedValue(serviceId, index) {
	$("#errorUpdateCalculatedValue").fadeOut();

	$("#numberValueError").fadeOut();

	var numberValue = $("#numberValue").val();

	if (numberValue == null || numberValue == "") {
		$("#numberValueError").fadeIn();
		return;
	}

	var calculatedValueId = $('.valueVariants').val();

	if (currentCalculatedValues[serviceId][index].number == numberValue && currentCalculatedValues[serviceId][index].calculatedValueId == calculatedValueId)
		return;

	var serviceCalculatedValue = {
		Id: currentCalculatedValues[serviceId][index].id,
		Number: numberValue,
		ServiceId: serviceId,
		CalculatedValueId: calculatedValueId
	};

	var isLoadUpdateCalculatedValue = false;
	var isLoadUpdateCalculatedValueVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateCalculatedValue) {
			isLoadUpdateCalculatedValueVisible = true;
			$('#btnUpdate').fadeOut();
			$('#btnDelete').fadeOut();
			$("#calculatedValue").append('<div id="preloaderUpdateCalculatedValue" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateCalculatedValue = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceConditions/UpdateServiceCalculatedValue",
		data: { serviceCalculatedValue },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentCalculatedValues[serviceId][index].number = serviceCalculatedValue.Number;
				currentCalculatedValues[serviceId][index].calculatedValueId = serviceCalculatedValue.CalculatedValueId;
				currentCalculatedValues[serviceId][index].value = $('.valueVariants option:selected').text();
				$('#successUpdateCalculatedValue').show('slow');
				window.setTimeout(close_calculated_value_form, 3001);
				function close_calculated_value_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateCalculatedValue').show('slow');
			}

			isLoadUpdateCalculatedValue = true;
			if (isLoadUpdateCalculatedValueVisible) {
				$("#preloaderUpdateCalculatedValue").remove();

				if (!result) {
					$('#btnUpdate').fadeIn();
					$('#btnDelete').fadeIn();
                }
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function DeleteCalculatedValue(serviceId, index) {
	$("#errorDeleteCalculatedValue").fadeOut();

	var isLoadDeleteCalculatedValue = false;
	var isLoadDeleteCalculatedValueVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadDeleteCalculatedValue) {
			isLoadDeleteCalculatedValueVisible = true;
			$('#btnUpdate').fadeOut();
			$('#btnDelete').fadeOut();
			$("#calculatedValue").append('<div id="preloaderDeleteCalculatedValue" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadDeleteCalculatedValue = false;
	};

	var serviceCalculatedValueId = currentCalculatedValues[serviceId][index].id;

	$.ajax({
		type: "POST",
		url: "/ServiceConditions/DeleteServiceCalculatedValue",
		data: { serviceCalculatedValueId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentCalculatedValues[serviceId].splice(index, 1);
				$('#successDeleteCalculatedValue').show('slow');
				window.setTimeout(close_calculated_value_form, 3001);
				function close_calculated_value_form() {
					CloseForm();
				};
			}
			else {
				$('#errorDeleteCalculatedValue').show('slow');
			}

			isLoadDeleteCalculatedValue = true;
			if (isLoadDeleteCalculatedValueVisible) {
				$("#preloaderDeleteCalculatedValue").remove();

				if (!result) {
					$('#btnUpdate').fadeIn();
					$('#btnDelete').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function UpdateRatesForm(index) {
	$("#serviceMenu").fadeOut();

	window.setTimeout(show_update_service_rates_form, 501);
	function show_update_service_rates_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "updateServiceRatesForm";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		$('.fullWindow.container').css("width", "471px");
		$('.center').append(inputsData);

		if (currentRates[index] == undefined) {
			var isLoadServiceRates = false;
			var isLoadServiceRatesVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadServiceRates) {
					isLoadServiceRatesVisible = true;
					$("#updateServiceRatesForm").append('<div id="preloaderRates" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadServiceRates = false;
			};

			var serviceId = mainDataByServices[index].id;

			$.ajax({
				type: "GET",
				url: "/ServiceRates/GetServiceRates",
				data: { serviceId },
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					currentRates[serviceId] = data;

					isLoadServiceRates = true;
					if (isLoadServiceRatesVisible) {
						$("#preloaderRates").remove();
					}
					SetServiceRates(serviceId);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetServiceRates(serviceId);
	}
}

function SetServiceRates(serviceId) {
	var inputsData = document.getElementById('updateServiceRatesForm');

	for (var i = 0; i < currentRates[serviceId].length; i++) {
		var formButtonServiceRate = document.createElement("button");
		formButtonServiceRate.className = "btn-saveData";
		formButtonServiceRate.setAttribute("onclick", "Rate(" + currentRates[serviceId][i].id + ", " + serviceId + ", " + i + ")");
		$(formButtonServiceRate).css("background", "#211F30");
		$(formButtonServiceRate).css("color", "#87CEFA");
		$(formButtonServiceRate).css("margin-top", "12px");
		$(formButtonServiceRate).css("margin-bottom", "12px");
		formButtonServiceRate.innerHTML = currentRates[serviceId][i].name;
		inputsData.append(formButtonServiceRate);
	}

	var formButtonServiceRate = document.createElement("button");
	formButtonServiceRate.className = "btn-saveData";
	formButtonServiceRate.setAttribute("onclick", "AddRate(" + serviceId + ")");
	$(formButtonServiceRate).css("background", "#211F30");
	$(formButtonServiceRate).css("color", "#E29930");
	$(formButtonServiceRate).css("margin-top", "12px");
	$(formButtonServiceRate).css("margin-bottom", "12px");
	formButtonServiceRate.innerHTML = "Добавить тариф";
	inputsData.append(formButtonServiceRate);
}

function Rate(rateId, serviceId, index) {
	$("#updateServiceRatesForm").fadeOut();

	window.setTimeout(show_rate_form, 501);
	function show_rate_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "rateForm";
		inputsData.className = "inputData";
		$(inputsData).css("align-items", "stretch");

		var formButtonServiceRate = document.createElement("button");
		formButtonServiceRate.className = "btn-saveData";
		formButtonServiceRate.setAttribute("onclick", "UpdateRate(" + rateId + ")");
		$(formButtonServiceRate).css("background", "#211F30");
		$(formButtonServiceRate).css("color", "#87CEFA");
		$(formButtonServiceRate).css("margin-top", "12px");
		$(formButtonServiceRate).css("margin-bottom", "12px");
		formButtonServiceRate.innerHTML = "Тариф";
		inputsData.append(formButtonServiceRate);

		var formButtonServiceRate = document.createElement("button");
		formButtonServiceRate.className = "btn-saveData";
		formButtonServiceRate.setAttribute("onclick", "UpdateRateCalculatedValueForm(" + rateId + ", " + serviceId + ", " + index + ")");
		$(formButtonServiceRate).css("background", "#211F30");
		$(formButtonServiceRate).css("color", "#87CEFA");
		$(formButtonServiceRate).css("margin-top", "12px");
		$(formButtonServiceRate).css("margin-bottom", "12px");
		formButtonServiceRate.innerHTML = "Ограничение по тарифу";
		inputsData.append(formButtonServiceRate);

		$('.center').append(inputsData);
	}
}

function AddRate(serviceId) {
	var url = "/MAXonService/RateCreatorForService/" + serviceId; 
	$(location).attr('href', url);
}

function UpdateRate(rateId) {
	var url = "/MAXonService/RateUpdater/" + rateId;
	$(location).attr('href', url);
}

function UpdateRateCalculatedValueForm(rateId, serviceId, index) {
	$("#rateForm").fadeOut();

	window.setTimeout(show_update_rate_calculated_value_form, 501);
	function show_update_rate_calculated_value_form() {
		var inputsData = document.createElement("div");
		inputsData.id = "updateRateCalculatedValueForm";
		inputsData.className = "inputData";

		$('.center').append(inputsData);

		if (currentCalculatedValues[serviceId] == undefined) {
			var isLoadCalculatedValues = false;
			var isLoadCalculatedValuesVisible = false;

			window.setTimeout(show_preloader, 428);
			function show_preloader() {
				if (!isLoadCalculatedValues) {
					isLoadCalculatedValuesVisible = true;
					$("#updateServiceCalculatedValuesForm").append('<div id="preloaderServiceCalculatedValues" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
				}
				isLoadCalculatedValues = false;
			};

			$.ajax({
				type: "GET",
				url: "/ServiceConditions/GetServiceCalculatedValues",
				data: { serviceId },
				contentType: 'application/json',
				dataTpye: "json",
				success: function (data) {
					currentCalculatedValues[serviceId] = data;

					isLoadCalculatedValues = true;
					if (isLoadCalculatedValuesVisible) {
						$("#preloaderServiceCalculatedValues").remove();
					}
					SetRateCalculatedValue(rateId, serviceId, index);
				},
				error: function (error) {
					alert(error);

					CloseForm();
				}
			});

			return;
		}

		SetRateCalculatedValue(rateId, serviceId, index);
	}
}

function SetRateCalculatedValue(rateId, serviceId, index) {
	var inputsData = document.getElementById('updateRateCalculatedValueForm');

	var calculatedValueVariantsBlock = document.createElement("div");
	calculatedValueVariantsBlock.className = "calculatedValueVariantsBlock";

	var valueVariants = document.createElement("select");
	valueVariants.className = "valueVariants";

	var selectCalculatedValueId = 0;

	var processInSelector = document.createElement("option");
	processInSelector.className = "processInSelector";
	$(processInSelector).val(0);
	processInSelector.innerHTML = "Не выбрано";
	$(valueVariants).append(processInSelector);

	for (var i = 0; i < currentCalculatedValues[serviceId].length; i++) {
		var processInSelector = document.createElement("option");
		processInSelector.className = "processInSelector";
		$(processInSelector).val(currentCalculatedValues[serviceId][i].id);
		processInSelector.innerHTML = currentCalculatedValues[serviceId][i].value + " " + currentCalculatedValues[serviceId][i].number;
		$(valueVariants).append(processInSelector);

		if (currentRates[serviceId][index].serviceCounterId == currentCalculatedValues[serviceId][i].id)
			selectCalculatedValueId = currentCalculatedValues[serviceId][i].id;
	}

	$(calculatedValueVariantsBlock).append(valueVariants);
	$(inputsData).append(calculatedValueVariantsBlock);

	$('.valueVariants option[value=' + selectCalculatedValueId + ']').prop('selected', true);

	var formButtonUpdate = document.createElement("button");
	formButtonUpdate.id = "btnUpdate";
	formButtonUpdate.className = "btn-saveData";
	formButtonUpdate.setAttribute("onclick", "UpdateRateCalculatedValue(" + rateId + ", " + serviceId + ", " + index + ")");
	formButtonUpdate.innerHTML = "Обновить";
	$(formButtonUpdate).css("background", "#87CEFA");
	$(inputsData).append(formButtonUpdate);

	var updateSuccess = document.createElement("p");
	updateSuccess.id = "successUpdateRateCalculatedValue";
	$(updateSuccess).css("margin-top", "0px");
	$(updateSuccess).css("margin-bottom", "12px");
	updateSuccess.className = "successMessage";
	updateSuccess.innerHTML = 'Вычисляемое значение тарифа успешно обновлено!';
	$(inputsData).append(updateSuccess);

	var updateError = document.createElement("p");
	updateError.id = "errorUpdateRateCalculatedValue";
	$(updateError).css("margin-top", "0px");
	$(updateError).css("margin-bottom", "12px");
	updateError.className = "errorMessage";
	updateError.innerHTML = 'Не удалось обновить вычисляемое значение тарифа! Попробуйте ещё раз.';
	$(inputsData).append(updateError);
}

function UpdateRateCalculatedValue(rateId, serviceId, index) {
	$("#errorUpdateRateCalculatedValue").fadeOut();

	var serviceCounterId = $('.valueVariants').val();

	if (serviceCounterId == 0)
		serviceCounterId = null;

	if (currentRates[serviceId][index].serviceCounterId == serviceCounterId)
		return;

	var isLoadUpdateRateCalculatedValue = false;
	var isLoadUpdateRateCalculatedValueVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateRateCalculatedValue) {
			isLoadUpdateRateCalculatedValueVisible = true;
			$('#btnUpdate').fadeOut();
			$("#calculatedValue").append('<div id="preloaderUpdateRateCalculatedValue" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateRateCalculatedValue = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceRates/UpdateRateCalculatedValue",
		data: { rateId, serviceCounterId },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				currentRates[serviceId][index].serviceCounterId = serviceCounterId;
				$('#successUpdateRateCalculatedValue').show('slow');
				window.setTimeout(close_update_rate_calculated_value_form, 3001);
				function close_update_rate_calculated_value_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateRateCalculatedValue').show('slow');
			}

			isLoadUpdateRateCalculatedValue = true;
			if (isLoadUpdateRateCalculatedValueVisible) {
				$("#preloaderUpdateRateCalculatedValue").remove();

				if (!result) {
					$('#btnUpdate').fadeIn();
				}
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function ProjectMenu() {
	var html = document.querySelector('html');

	var fullWindow = document.createElement("div");
	fullWindow.className = "fullWindow";

	var skipButton = document.createElement("p");
	skipButton.className = "skipButton";
	skipButton.innerHTML = "×";
	skipButton.setAttribute("onclick", "CloseForm()");

	var container = document.createElement("div");
	container.className = "container";
	$(container).css("padding", "22px 0");
	$(container).css("width", "328px");

	var center = document.createElement("div");
	center.className = "center";

	var inputsData = document.createElement("div");
	inputsData.id = "projectsMenu";
	inputsData.className = "inputData";
	$(inputsData).css("align-items", "stretch");

	var formButtonProjects = document.createElement("button");
	formButtonProjects.className = "btn-saveData";
	formButtonProjects.setAttribute("onclick", "Projects()");
	formButtonProjects.innerHTML = "Проекты";
	$(formButtonProjects).css("margin-top", "0");
	$(formButtonProjects).css("color", "#87CEFA");
	$(formButtonProjects).css("background", "#211F30");
	$(inputsData).append(formButtonProjects);

	var formButtonProjectTypes = document.createElement("button");
	formButtonProjectTypes.className = "btn-saveData";
	formButtonProjectTypes.setAttribute("onclick", "UpdateProjectTypesForm()");
	formButtonProjectTypes.innerHTML = "Типы проектов";
	$(formButtonProjectTypes).css("color", "#87CEFA");
	$(formButtonProjectTypes).css("background", "#211F30");
	$(inputsData).append(formButtonProjectTypes);

	var formButtonProjectCategories = document.createElement("button");
	formButtonProjectCategories.className = "btn-saveData";
	formButtonProjectCategories.setAttribute("onclick", "UpdateProjectCategoriesForm()");
	formButtonProjectCategories.innerHTML = "Категории проектов";
	$(formButtonProjectCategories).css("color", "#87CEFA");
	$(formButtonProjectCategories).css("background", "#211F30");
	$(inputsData).append(formButtonProjectCategories);

	var formButtonProjectCompatibilities = document.createElement("button");
	formButtonProjectCompatibilities.className = "btn-saveData";
	formButtonProjectCompatibilities.setAttribute("onclick", "UpdateProjectCompatibilitiesForm()");
	formButtonProjectCompatibilities.innerHTML = "Совместимости проектов";
	$(formButtonProjectCompatibilities).css("color", "#87CEFA");
	$(formButtonProjectCompatibilities).css("background", "#211F30");
	$(inputsData).append(formButtonProjectCompatibilities);

	var formButtonProjectCodeLevels = document.createElement("button");
	formButtonProjectCodeLevels.className = "btn-saveData";
	formButtonProjectCodeLevels.setAttribute("onclick", "UpdateProjectCodeLevelsForm()");
	formButtonProjectCodeLevels.innerHTML = "Уровни написания кода";
	$(formButtonProjectCodeLevels).css("color", "#87CEFA");
	$(formButtonProjectCodeLevels).css("background", "#211F30");
	$(inputsData).append(formButtonProjectCodeLevels);

	center.append(inputsData);

	container.append(center);

	fullWindow.append(skipButton);
	fullWindow.append(container);

	html.append(fullWindow);
}

function UpdateProjectTypesForm() {

}

function CloseForm() {
	document.querySelector('.fullWindow').remove();
}