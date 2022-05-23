window.onload = init;
//window.onload = init;
var isLoad = false;

function init() {
    timer();
    $.ajax({
        type: "GET",
        url: "/MAXonBusinessCard/GetAboutMeData",
        success: function (informationAboutMe) {
            SetBiography(informationAboutMe.Biography);
            SetSkills(informationAboutMe.Skills);
            SetExperience(informationAboutMe.Experience);
            SetEducation(informationAboutMe.Education);
        },
        error: function (error) {
            alert(error);
        }
    });
};

function timer() {
    window.setTimeout(show_preloader, 500);
    function show_preloader() {
        if (!isLoad) {
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.remove('skip');
        }
    };
};

function SetBiography(data) {
    var div = document.getElementById(`textAboutMe`);
    for (i = 0; i < data.length; i++) {
        var biography = document.createElement("p");
        biography.innerHTML = data[i].data;
        div.append(biography);
    }

    isLoad = true;

    var preloader = document.querySelector('.reverse-spinner');
    preloader.classList.add('skip');

    var biographyBlock = document.querySelector('.biographyBlock');
    biographyBlock.classList.remove('skip');
    biographyBlock.classList.add('loaded');
}

function SetSkills(data) {
    var skillsDiv = document.getElementById(`skills`);
    for (i = 0; i < data.length; i++) {
        var skillDiv = document.createElement("div");
        skillDiv.className = "skill";

        var skillNameBlock = document.createElement("div");
        skillNameBlock.className = "skillNameBlock";

        var skillName = document.createElement("p");
        skillName.className = "skillName";
        skillName.innerHTML = data[i].name;
        skillNameBlock.append(skillName);

        var skillPercentOfKnowledgeBlock = document.createElement("div");
        skillPercentOfKnowledgeBlock.className = "skillPercentOfKnowledgeBlock";

        var skillPercentOfKnowledge = document.createElement("p");
        skillPercentOfKnowledge.className = "skillPercentOfKnowledge";
        skillPercentOfKnowledge.innerHTML = data[i].percentOfKnowledge + "%";
        skillPercentOfKnowledgeBlock.append(skillPercentOfKnowledge);

        skillDiv.append(skillNameBlock);
        skillDiv.append(skillPercentOfKnowledgeBlock);

        var skillDescription = document.createElement("p");
        skillDescription.className = "skillDescription";
        skillDescription.innerHTML = data[i].description;
        skillDiv.append(skillDescription);

        skillsDiv.append(skillDiv);
    }

    var skillsBlock = document.querySelector('.skillsBlock');
    skillsBlock.classList.remove('skip');
    skillsBlock.classList.add('loaded');
}

function SetExperience(data) {
    var experienceDiv = document.getElementById(`experience`);
    for (i = 0; i < data.length; i++) {
        var experienceDataDiv = document.createElement("div");
        experienceDataDiv.className = "experienceData";

        var experienceHeader = document.createElement("div");
        experienceHeader.className = "experienceHeader";

        var period = document.createElement("p");
        period.className = "period";
        period.innerHTML = data[i].startDate + " - " + data[i].endDate;

        experienceHeader.append(period);
        experienceDataDiv.append(experienceHeader);

        var experienceSeparator = document.createElement("div");
        experienceSeparator.className = "experienceSeparator";
        experienceDataDiv.append(experienceSeparator);

        var experienceInformation = document.createElement("div");
        experienceInformation.className = "experienceInformation";

        var work = document.createElement("p");
        work.className = "work";
        work.innerHTML = data[i].company + "/" + data[i].position;

        var workDescription = document.createElement("p");
        workDescription.className = "workDescription";
        workDescription.innerHTML = data[i].description;

        experienceInformation.append(work);
        experienceInformation.append(workDescription);
        experienceDataDiv.append(experienceInformation);

        experienceDiv.append(experienceDataDiv);
    }

    var experienceBlock = document.querySelector('.experienceBlock');
    experienceBlock.classList.remove('skip');
    experienceBlock.classList.add('loaded');
}

function SetEducation(data) {
    var ulEducation = document.getElementById(`items`);
    for (i = 0; i < data.length; i++) {
        var liItem = document.createElement("li");
        liItem.className = "item";

        var topDiv = document.createElement("div");
        topDiv.className = "top";

        var circleDiv = document.createElement("div");
        circleDiv.className = "circle";

        var titleDiv = document.createElement("div");
        titleDiv.className = "title";
        titleDiv.innerHTML = data[i].organization;

        topDiv.append(circleDiv);
        topDiv.append(titleDiv);
        liItem.append(topDiv);

        var educationPeriodDiv = document.createElement("div");
        educationPeriodDiv.className = "educationPeriod";
        educationPeriodDiv.innerHTML = data[i].startDate + " - " + data[i].endDate;

        var educationDescriptionDiv = document.createElement("div");
        educationDescriptionDiv.className = "educationDescription";
        educationDescriptionDiv.innerHTML = data[i].description;

        liItem.append(educationPeriodDiv);
        liItem.append(educationDescriptionDiv);

        ulEducation.append(liItem);
    }

    var educationBlock = document.querySelector('.educationBlock');
    educationBlock.classList.remove('skip');
    educationBlock.classList.add('loaded');
}