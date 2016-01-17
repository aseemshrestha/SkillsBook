var index = 1;
$(document).ready(function () {
    var account = {
        myAccUrl: $("#myaccurl").data('url'),
        commentUrl: $("#commentsurl").data('url'),
        likeUrl: $("#likesurl").data('url'),
        viewUrl: $("#viewsurl").data('url'),
        watchUrl: $("#watchurl").data('url'),
        classiUrl: $("#classifiedurl").data('url'),
        questionUrl:$("#questionurl").data('url'),
        pagination: $("#pagination"),
        tabs: $("#tabs-post")
    }
    $("#pagination ul li a").on("click", function () {
        index = $(this).attr('id');
        $("#currpage").val(index);
        var tab = account.tabs.find('.active').attr('id');
        switch (tab) {
            case 'tab-post':
                $.get(account.myAccUrl + "?page=" + index, function (page) {
                    $("body").empty().append(page);
                });
                break;
            case 'tab-comment':
                index = $(this).attr('id');
                $("#tab-comment").trigger('click');
                break;
            case 'tab-likes':
                index = $(this).attr('id');
                $("#tab-likes").trigger('click');
                break;
            case 'tab-views':
                index = $(this).attr('id');
                $("#tab-views").trigger('click');
                break;
            case 'tab-watch':
                index = $(this).attr('id');
                $("#tab-watch").trigger('click');
                break;
            case 'tab-classifieds':
                index = $(this).attr('id');
                $("#tab-classifieds").trigger('click');
                break;
            case 'tab-questions':
                index = $(this).attr('id');
                $("#tab-questions").trigger('click');
                break;
        }
    });

    $("#tab-post").on("click", function () {
        $.get(account.myAccUrl, function (page) {
            $("body").empty().append(page);
        });
    });

    $("#tab-comment").on("click", function () {
        $.get(account.commentUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
            $("#lblcc").text('(' + $("#totalcountComments").val() + ')');
        });
    });

    $("#tab-likes").on("click", function () {
        $.get(account.likeUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
            $("#lbllc").text('(' + $("#totalcountLikes").val() + ')');
        });
    });

    $("#tab-views").on("click", function () {
        $.get(account.viewUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
            $("#lblcv").text('(' + $("#totalcountViews").val() + ')');
        });
    });

    $("#tab-watch").on("click", function () {
        $.get(account.watchUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
            $("#lblcw").text('(' + $("#totalcountWatch").val() + ')');
        });
    });
    $("#tab-classifieds").on("click", function () {
        $.get(account.classiUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
            $("#message").text("Note: If you switch status to closed, it won't be visible to others.");
        });
    });
    $("#tab-questions").on("click", function () {
        $.get(account.questionUrl + "?page=" + index, function (page) {
            $("#callback").empty().append(page);
            account.pagination.html('');
            account.tabs.html('');
           
        });
    });
    $("#updatePass").on("click", function() {
        $("#frmupdatepass").slideToggle();
    });

    $("#frmupdatepass").submit(function (e) {
        var formdata = {
            'old': $('#oldpass').val(),
            'newpass': $('#newpass').val(),
            'confirmnew': $('#confirmnewpass').val()
        }
        var url = $("#passupdate").data('url');
        $.ajax({
            type: 'POST', 
            url: url, 
            data: formdata, 
            dataType: 'json', 
            encode: true
        })
        .done(function (data) {
            console.log(data.Result);
            if(data.Result=="success")
                $("#error").text(data.Msg);
            if(data.Result == "failed")
                $("#error").text(data.Msg);
           
        });
        e.preventDefault();
    });

    $("#feedbackform").submit(function (e) {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!re.test($('#email').val())) {
            $("#callbackMessage").text("Email looks invalid. Please try again.");
            return;
        }
        var rUrl = $("#returnUrl").data('url');
        var formdata = {
            'uname': $('#uname').val(),
            'email': $('#email').val(),
            'subject': $('#subject').val(),
            'message': $('#message').val()
        }
        var url = $("#feedbackUrl").data('url');
        $.ajax({
            type: 'POST',
            url: url,
            data: formdata,
            dataType: 'json',
            encode: true
        })
       .done(function (data) {
         //  console.log(data.Result);
            if (data.Result == "success") {
                $("#callbackMessage").text(data.Message +" and redirecting you to home page..");
                setTimeout(function () {
                    window.location.href = rUrl;
                }, 2000);
            }
            if (data.Result == "vf") {
                $("#callbackMessage").text(data.Message);
            }
            if (data.Result == "failed") {
                $("#callbackMessage").text(data.Message);
            }
            
        });
        e.preventDefault();
    });
});

function unlike(threadId, like) {
    var url = $("#unlike-url").data('url') + "?threadId=" + threadId + "&currentLike=" + like;
     $.get(url, function (data) {
        if (data.Result == "success") {
            index = $("#currpage").val();
            $("#tab-likes").trigger("click");
        } else {
            alert("Opps!Something went wrong.");
        }
    });
}

function deleteThread(threadId,e) {
    var url = $("#delete-thread").data('url');
    var deleteNow = $("#delete_" + threadId);
    $.get(url+"?threadId="+threadId, function (data) {
        if (data.Result == "Y") {
            deleteNow.removeAttr('href').text(data.Message);
            location.reload();
           // return;
        }
        deleteNow.removeAttr('href').text(data.Message);
    });
}

function switchStatus(cid, status) {
    var url = $("#statusswitchurl").data('url') + "?cid=" + cid + "&status=" + status;
    $.get(url, function (data) {
        if (data.Result == "success") {
            index = $("#currpage").val();
            $("#tab-classifieds").trigger("click");
        } else {
            alert("Opps!Something went wrong.");
        }
    });
}
