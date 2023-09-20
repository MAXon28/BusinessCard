window.onload = init;

function init() {
    $('.container').fadeIn();
}

function AddService() {
	$("#errorAddService").fadeOut();

	$("#nameServiceError").fadeOut();
	$("#shortDescriptFirstError").fadeOut();
	$("#shortDescriptSecondError").fadeOut();
	$("#shortDescriptThirdError").fadeOut();
	$("#priceError").fadeOut();
	$("#fullDescriptError").fadeOut();

	var nameService = $("#nameService").val();
	var shortDescriptFirst = $("#shortDescriptFirst").val();
	var shortDescriptSecond = $("#shortDescriptSecond").val();
	var shortDescriptThird = $("#shortDescriptThird").val();
	var price = $("#price").val();
	var fullDescript = $("#fullDescript").text();
	var concretePrice = $('#concretePrice').is(':checked');
	var needTechnicalSpecification = $('#needTechnicalSpecification').is(':checked');
	var prePrice = $('#prePrice').is(':checked');
	var preDeadline = $('#preDeadline').is(':checked');
	var isPublic = $('#isPublic').is(':checked');

	if (nameService == null || nameService == "") {
		$('html, body').animate({ scrollTop: $('#nameService').offset().top }, 1);
		$("#nameServiceError").fadeIn();
		return;
	}

	if (shortDescriptFirst == null || shortDescriptFirst == "") {
		$('html, body').animate({ scrollTop: $('#shortDescriptFirst').offset().top }, 1);
		$("#shortDescriptFirstError").fadeIn();
		return;
	}

	if (shortDescriptSecond == null || shortDescriptSecond == "") {
		$('html, body').animate({ scrollTop: $('#shortDescriptSecond').offset().top }, 1);
		$("#shortDescriptSecondError").fadeIn();
		return;
	}

	if (shortDescriptThird == null || shortDescriptThird == "") {
		$('html, body').animate({ scrollTop: $('#shortDescriptThird').offset().top }, 1);
		$("#shortDescriptThirdError").fadeIn();
		return;
	}

	if (price == null || price == "") {
		$('html, body').animate({ scrollTop: $('#price').offset().top }, 1);
		$("#priceError").fadeIn();
		return;
	}

	if (fullDescript == null || fullDescript == "") {
		$('html, body').animate({ scrollTop: $('#fullDescript').offset().top }, 1);
		$("#fullDescriptError").fadeIn();
		return;
	}

	var shortDescriptions = [];
	shortDescriptions.push({
		Text: shortDescriptFirst
	});
	shortDescriptions.push({
		Text: shortDescriptSecond
	});
	shortDescriptions.push({
		Text: shortDescriptThird
	});

	var service = {
		Name: nameService,
		Description: fullDescript,
		shortDescriptions: shortDescriptions,
		Price: price,
		ConcretePrice: concretePrice,
		NeedTechnicalSpecification: needTechnicalSpecification,
		PrePrice: prePrice,
		PreDeadline: preDeadline,
		IsPublic: isPublic
	};

	var isLoadAddService = false;
	var isLoadAddServiceVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddService) {
			isLoadAddServiceVisible = true;
			$('.btn-saveData').fadeOut();
			$(".inputsData").append('<div id="preloaderAddService" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddService = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonService/AddService",
		data: { service },
		dataTpye: "json",
		success: function (result) {
			if (result) {
				$('#successAddService').show('slow');
				window.setTimeout(close_add_service_form, 3001);
				function close_add_service_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddService').show('slow');
			}

			isLoadAddService = true;
			if (isLoadAddServiceVisible) {
				$("#preloaderAddService").remove();

				if (!result) {
					$('.btn-saveData').fadeOut();
				}
			}
			
		},
		error: function (error) {
			alert(error);
		}
	});
}

function CloseForm() {
	var url = "/MAXonTeam/MAXon28Profile";
	$(location).attr('href', url);
}