Question = {
    settings: {
        form: $("#frmQuestion"),
        btnAsk: $("#askBtn"),
        messageDiv: $("#messageDiv"),
        username: $("#uname"),
        email: $("#email"),
        category: $("#category"),
        question: $("#contentq"),
        btnAsnwer: $("#answerBtn"),
        formAnswer: $("#frmanswer"),
        re: /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i
    },
    onSubmitAnswer: function (e) {
        var answer = $("#answer").val();
        if (answer == '' || answer == undefined) {
            alert("Answer is required. Thank you.");
            return false;
        }
        this.settings.btnAsnwer.text("Processing...");
        this.settings.formAnswer.submit();
        return false;
    },
    onSubmitForm: function (e) {
        var that = this;
        this.settings.form.on("submit", function () {
            if (that.settings.username.val() == '' || that.settings.username.val() == undefined ||
                that.settings.email.val() == '' || that.settings.email.val() == undefined ||
                that.settings.category.val() == '' || that.settings.category.val() == undefined ||
                that.settings.question.val() == null || that.settings.question.val() == '' || that.settings.question.val() == undefined) {
                that.settings.messageDiv.text(" All fields are required.");
                return false;
            }

            if (that.settings.category.val() == '' || that.settings.category.val() == undefined) {
                that.settings.messageDiv.text(" Please select one category.");
                return false;
            }

            if (that.settings.question.val().indexOf('?') < 1) {
                that.settings.messageDiv.text(" You shoud be asking a question. Your statement doesn't look like a question.");
                return false;
            }

            this.settings.form.submit();
            e.preventDefault();
            return false;
            //
        });
        /* if (question.slice(-1) != '?') {
                that.settings.messageDiv.text("Please ask a question not just a regular statement.");
                return;
            }*/


        // e.preventDefault();
        /*  if (that.settings.username.val() == "" || that.settings.email.val() == "" || that.settings.category.val() == "") {
                that.settings.messageDiv("All fields are required.");
                return;
            }*/


    },
    rateAnswer: function (answerId, rating) {
        var url = $("#ra").data('url') + "?answerId=" + answerId + "&rating=" + rating;
        $.get(url, function (response) {
            if (response.Result == "success"){
                if (rating == 0) {
                    $("#uy_" + answerId).text(response.Count);
                    $("#btnUseful_" + answerId).hide();
                    $("#btnUsefulh_" + answerId).show();
                }
                else if (rating == 1) {
                    $("#usw_" + answerId).text(response.Count);
                    $("#btnSw_" + answerId).hide();
                    $("#btnSwh_" + answerId).show();
                }
                else if (rating == 2) {
                    $("#un_" + answerId).text(response.Count);
                    $("#btnNu_" + answerId).hide();
                    $("#btnNuh_" + answerId).show();
                }
            } else {
                alert("Oops! something wrong. Please try again later");
                return;
            }
        });


    }
}

