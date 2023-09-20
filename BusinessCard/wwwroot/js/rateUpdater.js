window.onload = init;
var rateId = null;
var rateInfo = null;

function init() {
	hrefParts = $(location).attr('href').split("/");
	rateId = hrefParts[hrefParts.length - 1];

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
		url: "/ServiceRates/GetRate",
		data: { rateId },
		contentType: 'application/json',
		dataTpye: "json",
		success: function (data) {
			rateInfo = data;

			$('#nameRate').val(rateInfo.name);
			$('#descriptionRate').text(rateInfo.description);
			$('#price').val(rateInfo.price);
			$('#concretePrice').prop('checked', rateInfo.isSpecificPrice);
			$('#isPublic').prop('checked', rateInfo.isPublic);

			var conditionsBlock = document.querySelector('.conditionsBlock');

			for (var i = 0; i < data.conditions.length; i++) {
				var conditionBlock = document.createElement("div");
				conditionBlock.className = "conditionBlock";

				var booleanBlock = document.createElement("div");
				booleanBlock.className = "booleanBlock";
				$(booleanBlock).css("margin", "0");

				var checkboxBlock = document.createElement("div");
				checkboxBlock.className = "checkboxBlock";

				$(checkboxBlock).append('<div class="checkbox"><input class="custom-checkbox" type="checkbox" id="condition' + data.conditions[i].conditionValueId + '"><label for="condition' + data.conditions[i].conditionValueId + '"></label></div>');

				var checkboxText = document.createElement("p");
				checkboxText.innerHTML = data.conditions[i].conditionValue;
				$(checkboxBlock).append(checkboxText);

				$(booleanBlock).append(checkboxBlock);

				$(conditionBlock).append(booleanBlock);

				var inputbox = document.createElement("div");
				inputbox.className = "inputbox";
				$(inputbox).css("margin", "0");

				var input = document.createElement("input");
				input.id = "conditionText" + data.conditions[i].conditionValueId;
				input.className = "input";
				input.setAttribute("required", "required");
				input.setAttribute("autocomplete", "off");
				$(input).css("margin", "0");
				$(input).val(data.conditions[i].conditionValueText);

				$(inputbox).append(input);

				$(conditionBlock).append(inputbox);

				$(conditionsBlock).append(conditionBlock);

				$('#condition' + data.conditions[i].conditionValueId).prop('checked', data.conditions[i].isAvailable);
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

function UpdateRate() {
	$("#errorUpdateRate").fadeOut();

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

	var updateType = 0;

	if (rateInfo.name != nameRate
		|| rateInfo.description != descriptionRate
		|| rateInfo.price != price
		|| rateInfo.isSpecificPrice != concretePrice
		|| rateInfo.isPublic != isPublic)
		updateType = 1;

	var conditionsForSave = [];
	for (var i = 0; i < rateInfo.conditions.length; i++) {
		var conditionIsAvailable = $('#condition' + rateInfo.conditions[i].conditionValueId).is(':checked');
		var conditionValueText = "";
		if (conditionIsAvailable)
			conditionValueText = $("#conditionText" + rateInfo.conditions[i].conditionValueId).val();

		if (conditionIsAvailable != rateInfo.conditions[i].isAvailable || conditionValueText != rateInfo.conditions[i].conditionValueText) {
			var conditionForSave = {
				ConditionValueId: rateInfo.conditions[i].conditionValueId,
				ConditionValueText: conditionValueText,
				IsAvailable: conditionIsAvailable
			};
			conditionsForSave.push(conditionForSave);
        }
	}

	if (conditionsForSave.length > 0) {
		if (updateType == 0)
			updateType = 2;
		else
			updateType = 3;
	}

	if (updateType == 0)
		return;

	var rate = null;
	if (updateType == 1)
		rate = {
			Id: rateId,
			Name: nameRate,
			Description: descriptionRate,
			Price: price,
			IsSpecificPrice: concretePrice,
			IsPublic: isPublic
		};
	else if (updateType == 2)
		rate = {
			Id: rateId,
			Conditions: conditionsForSave
		};
	else
		rate = {
			Id: rateId,
			Name: nameRate,
			Description: descriptionRate,
			Price: price,
			IsSpecificPrice: concretePrice,
			IsPublic: isPublic,
			Conditions: conditionsForSave
		};

	var isLoadUpdateRate = false;
	var isLoadUpdateRateVisible = false;

	window.setTimeout(show_preloader, 428);
	function show_preloader() {
		if (!isLoadUpdateRate) {
			isLoadUpdateRateVisible = true;
			$('.btn-saveData').fadeOut();
			$(".inputsData").append('<div id="preloaderUpdateRate" class="preloader loaded" style="margin-top: 28px; width: 28px; height: 28px;"><svg class="preloader__image" style="width: 28px; height: 28px;" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor"d="M304 48c0 26.51-21.49 48-48 48s-48-21.49-48-48 21.49-48 48-48 48 21.49 48 48zm-48 368c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zm208-208c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48zM96 256c0-26.51-21.49-48-48-48S0 229.49 0 256s21.49 48 48 48 48-21.49 48-48zm12.922 99.078c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.491-48-48-48zm294.156 0c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48c0-26.509-21.49-48-48-48zM108.922 60.922c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.491-48-48-48z"></path></svg></div>');
		}
		isLoadUpdateRate = false;
	};

	$.ajax({
		type: "POST",
		url: "/ServiceRates/UpdateRate",
		data: { rate, updateType },
		dataTpye: "json",
		success: function (result) {

			if (result) {
				$('#successUpdateRate').show('slow');
				window.setTimeout(close_rate_update_form, 3001);
				function close_rate_update_form() {
					CloseForm();
				};
			}
			else {
				$('#errorUpdateRate').show('slow');
			}

			isLoadUpdateRate = true;
			if (isLoadUpdateRateVisible) {
				$("#preloaderUpdateRate").remove();

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