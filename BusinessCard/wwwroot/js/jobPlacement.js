var isLoad = false;

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

document.querySelector(".inputSalary").addEventListener("keypress", function (evt) {
	if (evt.which != 8 && evt.which != 0 && evt.which < 48 || evt.which > 57) {
		evt.preventDefault();
	}
});

function CreateVacancy() {
	event.preventDefault();
	var url = $(this).attr('href');
	window.history.replaceState("object or string", "Title", url);

	$("#nameError").fadeOut();
	$("#companyError").fadeOut();
	$("#positionError").fadeOut();
	$("#scheduleError").fadeOut();
	$("#contactsError").fadeOut();


	var name = $('#name').val();
	if (name == null || name == "" || !name.trim()) {
		$('#nameError').show('slow');
		$('#name').focus();
		return;
    }

	var company = $('#company').val();
	if (company == null || company == "" || !company.trim()) {
		$('#companyError').show('slow');
		$('#company').focus();
		return;
	}

	var position = $('#position').val();
	if (position == null || position == "" || !position.trim()) {
		$('#positionError').show('slow');
		$('#position').focus();
		return;
	}

	var schedule = $('#schedule').val();
	if (schedule == null || schedule == "" || !schedule.trim()) {
		$('#scheduleError').show('slow');
		$('#schedule').focus();
		return;
	}

	var contacts = $('#contacts').val();
	if (contacts == null || contacts == "" || !contacts.trim()) {
		$('#contactsError').show('slow');
		$('#contacts').focus();
		return;
	}

	var address = $('#address').val();
	var salaryFrom = $('#salaryFrom').val();
	var salaryTo = $('#salaryTo').val();
	var currency = $('#currency').val();
	var description = $('#description').html();

	var newVacancy = {
		Name: name,
		Company: company,
		Position: position,
		Schedule: schedule,
		Contacts: contacts,
		Address: address,
		SalaryFrom: salaryFrom,
		SalaryTo: salaryTo,
		Currency: currency,
		Description: description
	}

	$("#form").fadeOut();

	timer();
	$.ajax({
		type: "POST",
		url: "/MAXonWork/CreateVacancy",
		contentType: 'application/json',
		data: JSON.stringify(newVacancy),
		success: function (result) {
			isLoad = true;

			var preloader = document.querySelector('.reverse-spinner');
			preloader.classList.add('skip');

			if (result) {
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
		error: function (error) {
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

function timer() {
	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoad) {
			var preloader = document.querySelector('.reverse-spinner');
			preloader.classList.remove('skip');
		}
	};
};