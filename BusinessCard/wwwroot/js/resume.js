window.onload = init;
window.scrollTo(0, 0);
var isLoad = false;
var usePreloader = false;

function init() {
    timer();
    $.ajax({
        async: true,
        type: "GET",
        url: "/MAXonWork/GetResume",
        success: function (resume) {
            if (resume != null) {
                SetPosition(resume.position);
                SetSalary(resume.salary);
                SetSchedule(resume.schedule);
                SetTechnologyStack(resume.technologyStack);
            } else {
                isLoad = true;
                ViewNotResume();
            }
        },
        error: function (error) {
            isLoad = true;
            alert(error);
        }
    });
};

function timer() {
    window.setTimeout(show_preloader, 428);
    function show_preloader() {
        if (!isLoad) {
            usePreloader = true;
            var preloader = document.querySelector('.reverse-spinner');
            preloader.classList.remove('skip');
        }
    };
};

function SetPosition(data) {
    var position = document.querySelector('.positionData');
    position.innerHTML = data;

    isLoad = true;
}

function SetSalary(data) {
    var salary = document.querySelector('.salaryData');
    salary.innerHTML = data;

    isLoad = true;
}

function SetSchedule(data) {
    var schedule = document.querySelector('.scheduleData');
    schedule.innerHTML = data;

    isLoad = true;
}

function SetTechnologyStack(data) {
    var technologyStack = document.querySelector('.technologyStackData');
    technologyStack.innerHTML = data;

    isLoad = true;

    ViewResume();
}

function ViewResume() {
    if (usePreloader) {
        var preloader = document.querySelector('.reverse-spinner');
        preloader.classList.add('skip');
    }

    var resume1 = document.querySelector('.resume1');
    resume1.classList.remove('skip');
    resume1.classList.add('loaded');

    var resume2 = document.querySelector('.resume2');
    resume2.classList.remove('skip');
    resume2.classList.add('loaded');

    var responseToResume = document.querySelector('.responseToResume');
    responseToResume.classList.remove('skip');
    responseToResume.classList.add('loadedResponseToResume');

    setTimeout(function () {
        var position = document.querySelector('.positionBox');
        position.classList.add('data_effect');

        var salary = document.querySelector('.salaryBox');
        salary.classList.add('data_effect');

        var schedule = document.querySelector('.scheduleBox');
        schedule.classList.add('data_effect');

        var technologyStack = document.querySelector('.technologyStackBox');
        technologyStack.classList.add('data_effect');

        responseToResume.classList.add('data_effect');

        $('html, body').animate({ scrollTop: $(".header").offset().top }, 228);
    }, 228);
}

function ViewNotResume() {
    var preloader = document.querySelector('.reverse-spinner');
    preloader.classList.add('skip');

    var notResume = document.querySelector('.notResume');
    notResume.classList.remove('skip');
    notResume.classList.add('contents');

    setTimeout(function () {
        var header = document.querySelector('.header');
        header.classList.add('no_data_effect');

        notResume.classList.add('data_effect');
    }, 228);
}