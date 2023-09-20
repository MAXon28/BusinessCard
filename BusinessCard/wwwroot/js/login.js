window.onload = init;
var isSignIn = true;
var isLongSingUpBlock = false;

function init() { }

$(function () {
	$(".btn").click(function () {
		if (isSignIn && isLongSingUpBlock)
			$('.frameж').css({ "height": "831px" });
		if (!isSignIn)
			$('.frame').removeAttr('style');
		isSignIn = !isSignIn;
		$(".form-signin").toggleClass("form-signin-left");
		$(".form-signup").toggleClass("form-signup-left");
		$(".frame").toggleClass("frame-long");
		$(".signup-inactive").toggleClass("signup-active");
		$(".signin-active").toggleClass("signin-inactive");
		$(".forgot").toggleClass("forgot-left");
		$(this).removeClass("idle").addClass("active");
	});
});

$(function () {
	$(".btn-signup").click(function () {
		event.preventDefault();
		var url = $(this).attr('href');
		window.history.replaceState("object or string", "Title", url);

		isLongSingUpBlock = false;

		$('.frame').removeAttr('style');
		$("#surnameError").fadeOut();
		$("#nameError").fadeOut();
		$('#emailRegistrationError').fadeOut();
		$('#phoneNumberRegistrationError').fadeOut();
		$("#passwordRegistrationError").fadeOut();
		$('#emailRegistrationErrorFromServer').fadeOut();
		$('#phoneNumberRegistrationErrorFromServer').fadeOut();
		$("#passwordRegistrationErrorFromServer").fadeOut();
		$("#repeatPasswordError").fadeOut();
		$("#privacyPolicyError").fadeOut();
		$("#registrationError").fadeOut();

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

		var emailRegistration = $('#emailRegistration').val();
		if (emailRegistration == null || emailRegistration == "" || !emailRegistration.trim()) {
			$('#emailRegistrationError').show('slow');
			$('#emailRegistration').focus();
			return;
		}

		var phoneNumberRegistration = $('#phoneNumberRegistration').val();
		if (phoneNumberRegistration == null || phoneNumberRegistration == "" || !phoneNumberRegistration.trim()) {
			$('#phoneNumberRegistrationError').show('slow');
			$('#phoneNumberRegistration').focus();
			return;
		}

		var passwordRegistration = $('#passwordRegistration').val();
		if (passwordRegistration == null || passwordRegistration == "" || !passwordRegistration.trim()) {
			$('#passwordRegistrationError').show('slow');
			$('#passwordRegistration').focus();
			return;
		}

		var repeatPassword = $('#repeatPassword').val();
		if (repeatPassword == null || repeatPassword == "" || !repeatPassword.trim()) {
			$('#repeatPasswordError').show('slow');
			$('#repeatPassword').focus();
			return;
		}

		if (!$('#privacyPolicy').is(':checked')) {
			$('#privacyPolicyError').show('slow');
			return;
		}

		if (passwordRegistration != repeatPassword) {
			$('#repeatPasswordError').show('slow');
			$('#repeatPassword').focus();
			return;
		}

		var user = {
			Surname: surname,
			Name: name,
			MiddleName: middleName,
			Email: emailRegistration,
			PhoneNumber: phoneNumberRegistration,
			Password: passwordRegistration
		}

		var isLoadRegistration = false;
		var isLoadRegistrationVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadRegistration) {
				isLoadRegistrationVisible = true;
				$('.btn-signup').fadeOut();
				$(".form-signup").append('<div id="preloaderSignup" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadRegistration = false;
		};

		$.ajax({
			type: "POST",
			url: "/Account/Registration",
			contentType: 'application/json',
			data: JSON.stringify(user),
			success: function (result) {

				if (result) {
					$('.inputbox').remove();
					$('.ruleBlock').remove();
					$('#mainPreloader').fadeIn();
					$('#registrationSuccess').show('slow');
					window.setTimeout(redirect_to_main_page, 3001);
					function redirect_to_main_page() {
						$(location).attr('href', "/");
					};
				}
				else {
					$('#registrationError').text('Не удалось зарегистрироваться. Пожалуйста, повторите попытку.');
					$('#registrationError').show('slow');
                }

				isLoadRegistration = true;
				if (isLoadRegistrationVisible) {
					$("#preloaderSignup").remove();

					if (!result)
						$('.btn-signup').fadeIn();
				}
			},
			error: function (error) {
				switch (error.responseJSON.TypeOfError) {
					case 'UNIQUE_VALUE':
						$('#registrationError').text(error.responseJSON.ErrorMessage);
						$('#registrationError').show('slow');
						$('#emailRegistration').focus();
						break;
					case 'EMAIL':
						$('#emailRegistrationErrorFromServer').text(error.responseJSON.ErrorMessage);
						$('#emailRegistrationErrorFromServer').show('slow');
						$('#emailRegistration').focus();
						break;
					case 'PASSWORD':
						isLongSingUpBlock = true;
						$('.frame-long').css({ "height": "831px" });
						$('#passwordRegistrationErrorFromServer').text(error.responseJSON.ErrorMessage);
						$('#passwordRegistrationErrorFromServer').show('slow');
						$('#passwordRegistration').focus();
						break;
					case 'PHONE':
						$('#phoneNumberRegistrationErrorFromServer').text(error.responseJSON.ErrorMessage);
						$('#phoneNumberRegistrationErrorFromServer').show('slow');
						$('#phoneNumberRegistration').focus();
					default:
						$('#registrationError').text(error.responseJSON.ErrorMessage);
						$('#registrationError').show('slow');
				}

				isLoadRegistration = true;
				if (isLoadRegistrationVisible) {
					$("#preloaderSignup").remove();
					$('.btn-signup').fadeIn();
				}
			}
		});
	});
});

$(function () {
	$(".btn-signin").click(function () {
		event.preventDefault();
		var url = $(this).attr('href');
		window.history.replaceState("object or string", "Title", url);

		$("#emailError").fadeOut();
		$("#passwordError").fadeOut();
		$('#loginError').fadeOut();

		var email = $('#email').val();
		if (email == null || email == "" || !email.trim()) {
			$('#emailError').show('slow');
			$('#email').focus();
			return;
		}

		var password = $('#password').val();
		if (password == null || password == "" || !password.trim()) {
			$('#passwordError').show('slow');
			$('#password').focus();
			return;
		}

		var isLoadLogin = false;
		var isLoadLoginVisible = false;

		window.setTimeout(show_preloader, 428);
		function show_preloader() {
			if (!isLoadLogin) {
				isLoadLoginVisible = true;
				$('.btn-signin').fadeOut();
				$(".form-signin").append('<div id="preloaderSignin" class="preloader loaded" style="width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
			}
			isLoadLogin = false;
		};

		var params = 'email=' + email + '&password=' + password;
		$.ajax({
			type: "GET",
			url: "/Account/Authenticate?" + params,
			contentType: 'application/json',
			dataTpye: "json",
			success: function (result) {

				if (result)
					$(location).attr('href', '/');
				else
					$('#loginError').show('slow');

				isLoadLogin = true;
				if (isLoadLoginVisible) {
					$("#preloaderSignin").remove();

					if (!result)
						$('.btn-signin').fadeIn();
				}
			},
			error: function (error) {
				alert("Неопределённая ошибка");
			}
		});
	});
});