window.onload = init;
var serviceId = null;
var conditions = null;

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
		url: "/ServiceConditions/GetServiceConditions",
		data: { serviceId },
		contentType: 'application/json',
		dataTpye: "json",
		success: function (data) {
			conditions = data;

			var conditionsBlock = document.querySelector('.conditionsBlock');

			for (var i = 0; i < data.length; i++) {
				var conditionBlock = document.createElement("div");
				conditionBlock.className = "conditionBlock";

				var booleanBlock = document.createElement("div");
				booleanBlock.className = "booleanBlock";
				$(booleanBlock).css("margin", "0");

				var checkboxBlock = document.createElement("div");
				checkboxBlock.className = "checkboxBlock";

				$(checkboxBlock).append('<div class="checkbox"><input class="custom-checkbox" type="checkbox" id="condition' + data[i].id + '"><label for="condition' + data[i].id + '"></label></div>');

				var checkboxText = document.createElement("p");
				checkboxText.innerHTML = data[i].text;
				$(checkboxBlock).append(checkboxText);

				$(booleanBlock).append(checkboxBlock);

				$(conditionBlock).append(booleanBlock);

				var inputbox = document.createElement("div");
				inputbox.className = "inputbox";
				$(inputbox).css("margin", "0");

				var input = document.createElement("input");
				input.id = "conditionText" + data[i].id;
				input.className = "input";
				input.setAttribute("required", "required");
				input.setAttribute("autocomplete", "off");
				$(input).css("margin", "0");

				$(inputbox).append(input);

				$(conditionBlock).append(inputbox);

				$(conditionsBlock).append(conditionBlock);
			}

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

function AddRate() {
	$("#errorAddRate").fadeOut();

	$("#nameRateError").fadeOut();
	$("#descriptionRateError").fadeOut();
	$("#priceError").fadeOut();

	var nameRate = $("#nameRate").val();
	var descriptionRate = $("#descriptionRate").text();
	var price = +$("#price").val();

	if (nameRate == null || nameRate == "") {
		$('html, body').animate({ scrollTop: $('#nameRate').offset().top }, 1);
		$("#nameRateError").fadeIn();
		return;
	}

	if (descriptionRate == null || descriptionRate == "") {
		$('html, body').animate({ scrollTop: $('#descriptionRate').offset().top }, 1);
		$("#descriptionRateError").fadeIn();
		return;
	}

	if (price == null || price == "") {
		$('html, body').animate({ scrollTop: $('#price').offset().top }, 1);
		$("#priceError").fadeIn();
		return;
	}

	var concretePrice = $('#concretePrice').is(':checked');
	var isPublic = $('#isPublic').is(':checked');

	var conditionsForSave = [];
	for (var i = 0; i < conditions.length; i++) {
		var conditionForSave = {
			ConditionId: conditions[i].id,
			ConditionValueText: $("#conditionText" + conditions[i].id).text(),
			IsAvailable: $('#condition' + conditions[i].id).is(':checked')
		};
		conditionsForSave.push(conditionForSave);
	}

	var rate = {
		Name: nameRate,
		Description: descriptionRate,
		Price: price,
		IsSpecificPrice: concretePrice,
		IsPublic: isPublic,
		ServiceId: serviceId,
		Conditions: conditionsForSave
	};

	var isLoadAddRate = false;
	var isLoadAddRateVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadAddRate) {
			isLoadAddRateVisible = true;
			$('.btn-saveData').fadeOut();
			$(".inputsData").append('<div id="preloaderAddRate" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadAddRate = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceRates/AddRate",
		data: { rate },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				$('#successAddRate').show('slow');
				window.setTimeout(close_rate_add_form, 3001);
				function close_rate_add_form() {
					CloseForm();
				};
			}
			else {
				$('#errorAddRate').show('slow');
			}

			isLoadAddRate = true;
			if (isLoadAddRateVisible) {
				$("#preloaderAddRate").remove();

				if (!result)
					$('.btn-saveData').fadeIn();
			}
		},
		error: function (error) {
			alert(error);

			CloseForm();
		}
	});
}

function CloseForm() {
	var url = "/MAXonTeam/MAXon28Profile";
	$(location).attr('href', url);
}