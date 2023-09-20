var card = document.querySelector('.card');
var flipButtonFront = document.querySelector('.container_corner_front');
var flipButtonBack = document.querySelector('.container_corner_back');
var divBack = document.querySelector('.preloader');

var isGetBack = false;

flipButtonFront.addEventListener('click', function () {
    card.classList.toggle('is-flipped');

    if (!isGetBack) {
        isGetBack = true;

        var backDiv = document.getElementById(`back`);

        divBack.classList.add('loaded_hiding');
        divBack.classList.remove('loaded');

        $.ajax({
            async: true,
            type: "GET",
            url: "/MAXonBusinessCard/GetMainFacts",
            success: function (mainFacts) {
                for (i = 0; i < mainFacts.length; i++) {
                    var p = document.createElement("p");
                    p.className = "fact";
                    p.innerHTML = "- " + mainFacts[i].text;
                    backDiv.append(p);
                }

                divBack.classList.add('loaded');
                divBack.classList.remove('loaded_hiding');
            },
            error: function (error) {
                alert(error);
            }
        });
    }
});

flipButtonBack.addEventListener('click', function () {
    card.classList.toggle('is-flipped');
});