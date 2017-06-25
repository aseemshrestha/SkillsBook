
var quiz;
var QuizWidget = {
    settings: {
        playQuiz: $("#playQuizBtn"),
        submitQuiz: $("#submitQuizBtn"),
        submitQuizUrl: $("#submitQuizUrl").data('url'), //url for view page
        quizForm: $("#quizForm"),
        createQuiz: $("#createQuizBtn"), //submits the quiz
        createQuizUrl: $("#createQuizUrl").data('ur'),
        playQuizBtn: $("#playQuizBtn"),
        playQuizUrl: $("#playQuizUrl").data('url'),
        quizRadio: $("#quizCheckForm input"),
        checkQuiz: $("#checkQuiz").data('url')

    },

    init: function () {
        quiz = this.settings;
        this.bindUIActions();
    },

    bindUIActions: function () {
        quiz.submitQuiz.on("click", function () {
            QuizWidget.getQuizSubmitPage();
        });

        quiz.createQuiz.on("click", function () {
            QuizWidget.submitQuizPage();
        });

        quiz.playQuizBtn.on("click", function () {
            QuizWidget.getQuizPage();
        });
        quiz.quizRadio.on("change", function(e) {
            QuizWidget.triggerQuizAnswer(e);
        });

    },

    getQuizSubmitPage: function () {
        window.location.href = quiz.submitQuizUrl;
    },
    getQuizPage: function () {
        window.location.href = quiz.playQuizUrl;
    },
    triggerQuizAnswer: function (e) {
        var qId = e.target.id;
        var value = e.target.value;
        $.getJSON(quiz.checkQuiz + "?qId=" + qId + '&a=' + encodeURIComponent(value), function (response) {
           // console.log(response.Count);
            $("#ta_"+qId).text(response.Count);
            if (response.Result == 'match') {
                $("#r_"+qId).show();
                $("#w_" + qId).hide();
            } else {
                $("#w_"+qId).show();
                $("#r_"+qId).hide();
            }
        });

    },
    submitQuizPage: function () {
        var data = quiz.quizForm.serializeArray();
        var isValid = false;
        $.each(data, function (i, field) {
            if (field.value == '') {
                $('html, body').animate({ scrollTop: "0px" }, 800);
                $("#notify").text("All fields are required.");
                return false;
            }
            else {
                isValid = true;
                return isValid;
            }

        });
        if (isValid) {
            if (data[4].value === data[5].value || data[4].value === data[6].value || data[4].value === data[7].value
                || data[5].value === data[6].value || data[5].value === data[7].value || data[6].value === data[7].value
                ) {
                $('html, body').animate({ scrollTop: "0px" }, 800);
                $("#notify").text("You cannot have the same answers.").css('color','red');
                return false;
            }
            var thatForm = quiz.quizForm;
            $.post(quiz.createQuizUrl,
                {
                    username: data[0].value, email: data[1].value,
                    question: data[2].value, category: data[3].value,
                    answer1: data[4].value,
                    answer2: data[5].value,
                    answer3: data[6].value,
                    answer4: data[7].value,
                    rightanswer: data[8].value

                },
            function (callback, status) {
                if (callback.Result == 'success') {
                    $('html, body').animate({ scrollTop: "0px" }, 800);
                    $("#notify").text("You have successfull created a quiz. You may want to go to quiz section to view the quiz or create another one.").css('color','green');
                    thatForm.reset();
                }
            });
        }
    }
};

(function () {
    QuizWidget.init();
})();
