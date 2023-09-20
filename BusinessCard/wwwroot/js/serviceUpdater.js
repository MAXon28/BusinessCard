window.onload = init;
var serviceId = null;
var serviceInfo = null;

function init() {
    hrefParts = $(location).attr('href').split("/");
    serviceId = hrefParts[hrefParts.length - 1];

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
		url: "/MAXonService/GetServiceInfo",
		data: { serviceId },
		contentType: 'application/json',
		dataTpye: "json",
		success: function (data) {
			serviceInfo = data;

			$('#nameService').val(serviceInfo.name);
			$('#shortDescriptFirst').val(serviceInfo.shortDescriptions[0].text);
			$('#shortDescriptSecond').val(serviceInfo.shortDescriptions[1].text);
			$('#shortDescriptThird').val(serviceInfo.shortDescriptions[2].text);
			$('#price').val(serviceInfo.price);
			$('#fullDescript').text(serviceInfo.description);
			$('#concretePrice').prop('checked', serviceInfo.concretePrice);
			$('#needTechnicalSpecification').prop('checked', serviceInfo.needTechnicalSpecification);
			$('#prePrice').prop('checked', serviceInfo.prePrice);
			$('#preDeadline').prop('checked', serviceInfo.preDeadline);
			$('#isPublic').prop('checked', serviceInfo.isPublic);

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

function UpdateServiceData() {
	$("#errorUpdateService").fadeOut();

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
	var price = +$("#price").val();
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

	var updateType = 0;

	if (serviceInfo.name != nameService
		|| serviceInfo.description != fullDescript
		|| serviceInfo.price != price
		|| serviceInfo.concretePrice != concretePrice
		|| serviceInfo.needTechnicalSpecification != needTechnicalSpecification
		|| serviceInfo.prePrice != prePrice
		|| serviceInfo.preDeadline != preDeadline
		|| serviceInfo.isPublic != isPublic)
		updateType = 1;

	if (serviceInfo.shortDescriptions[0].text != shortDescriptFirst
		|| serviceInfo.shortDescriptions[1].text != shortDescriptSecond
		|| serviceInfo.shortDescriptions[2].text != shortDescriptThird) {
		if (updateType == 0)
			updateType = 2;
		else
			updateType = 3;
	}

	if (updateType == 0)
		return;

	var shortDescriptions = [];
	if (updateType != 1) {
		shortDescriptions.push({
			Id: serviceInfo.shortDescriptions[0].id,
			Text: shortDescriptFirst
		});
		shortDescriptions.push({
			Id: serviceInfo.shortDescriptions[1].id,
			Text: shortDescriptSecond
		});
		shortDescriptions.push({
			Id: serviceInfo.shortDescriptions[2].id,
			Text: shortDescriptThird
		});
	}

	var service = {
		Id: serviceId,
		Name: nameService,
		Description: fullDescript,
		Price: price,
		ConcretePrice: concretePrice,
		NeedTechnicalSpecification: needTechnicalSpecification,
		PrePrice: prePrice,
		PreDeadline: preDeadline,
		IsPublic: isPublic,
		ShortDescriptions: shortDescriptions
	};

	var isLoadUpdateService = false;
	var isLoadUpdateServiceVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateService) {
			isLoadUpdateServiceVisible = true;
			$('.btn-saveData').fadeOut();
			$(".inputsData").append('<div id="preloaderUpdateService" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateService = false;
	};

	$.ajax({
		type: "POST",
		url: "/MAXonService/UpdateService",
		data: { service, updateType },
		dataTpye: "json",
		success: function (result) {
			if (result) {
				$('#successUpdateService').show('slow');
				window.setTimeout(close_update_service_form, 3001);
				function close_update_service_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateService').show('slow');
			}

			isLoadUpdateService = true;
			if (isLoadUpdateServiceVisible) {
				$("#preloaderUpdateService").remove();

				if (!result) {
					$('.btn-saveData').fadeIn();
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