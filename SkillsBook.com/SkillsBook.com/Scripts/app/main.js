$(document).ready(function () {
    var curl = $(location).attr('href');
    if ((/AddThread/i.test(curl) || (/DisplayThread/i.test(curl)|| (/Question/i.test(curl))))) {
        initTinyMce();

    }
  
    //loadChart();

    var forms = {
        logon: $("#frmLogOn"),
        register: $("#frmRegister"),
        thread: $("#frmThread"),
        classified:$("#frmClassified")
    }
    var data = {
        uname: $("#uname"),
        email: $("#email"),
        title: $("#title"),
        tagsG: $("#tagsG"),
        content: $("#con"),
        tagsA: $("#tagsA"),
        editor: $(".mceNoEditor"),
        check: "/Account/Check?usernameOrEmail=",
        like: $("#like_"),
        watch: $("#watch_"),

    }
    var search = {
        searchResults: $("#searchResults"),
        btnGo1: $("#btnGo1")
    }

    var viewModel = function () {
        var self = this;
        self.email = ko.observable(data.email.val());
        self.validationErrors = ko.observable();
        self.password = ko.observable();
        self.confirmPassword = ko.observable();
        self.username = ko.observable(data.uname.val());
        self.isThread = ko.observable();
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        self.announcement = ko.observable(data.editor.text());
        self.maxCharacter = ko.observable(200);
        self.tagsA = ko.observable(data.tagsA.val());
        self.title = ko.observable(data.title.val());
        self.content = ko.observable(data.content.val());
        self.tagsG = ko.observable(data.tagsG.val());
        self.tagsArray = ko.observableArray(['Accommodation', 'Entertainment', 'Event', 'Education', 'Help', 'Intimacy', 'Immigration', 'Jobs',
            'Lets Talk', 'Love', 'News', 'Need a partner', 'WTF'

        ]);
        self.availableOptions = ko.observableArray(['Housing', 'BuySell', 'Jobs']);
        self.classifiedChosen = ko.observable();

       // self.tagsClassifiedArray = ko.observableArray(['Housing', 'Buy/Sell', 'Jobs']);
        self.temp = ko.observableArray();
        self.temp1 = ko.observableArray();
        self.owntag = ko.observable();
        self.likes = ko.observable('<b><font color=#3b599>Like</font></b>');
        self.liked = ko.observable(' <b><font color=#3498db> Liked </font></b> ');
        self.isAuthenticated = ko.observable();
        self.totalLikes = ko.observable();
        self.watch = ko.observable(' Watch This Thread');
        self.watched = ko.observable(' <b><font color=#3498db> Watched </font></b> ');
        self.comment = ko.observable();
        //self.commentThreadId = ko.observable();
        //self.commetSubmitArray = ko.observableArray(['Friend','Well Wisher','Secret Admirer','Anonymous']);
        var counter = 0;

        (data.uname).on('blur', function () {
            var row = ko.dataFor(this);
            var url = data.check + encodeURIComponent(row.username());
            $.post(url, function (callback, status) {
                if (status != "success") {
                    alert("OOps! Something went wrong. Please try again later");
                    return;
                }
                if (callback.UsernameExists == "true")
                    self.validationErrors(callback.Message);
                else
                    self.validationErrors("Username is Available");
            });
        });

        (data.email).on('blur', function () {
            var row = ko.dataFor(this);
            var that = row;
            var url = data.check + encodeURIComponent(that.email());
            $.post(url, function (callback, status) {
                if (status != "success") {
                    alert("* OOps! Something went wrong. Please try again later");
                    return;
                }
                if (callback.EmailExists == "true")
                    self.validationErrors(callback.Message);
                else {
                    if (!re.test(that.email())) {
                        self.validationErrors("<span>* Please enter valid email.</span>");
                        return;
                    }
                    self.validationErrors("Email is Available");
                }
            });
        });

        self.likedByPopUp = function (threadId) {
            var url = $('#liked-by').data('url');
            var likes = '';
            $.get(url + '?threadId=' + threadId, function (callback) {
                $.each(callback.Result, function (i, v) {
                    likes = likes + '' + v.LikedBy + '<br />';
                });
                $("#close_" + threadId).show();
                $("#show_" + threadId).show().html('<small>Liked By:</small><br />' + '<small>' + likes + '</small>');
            });
        }
        self.tagsA.subscribe(function () {
            self.maxCharacter(200);
            if (self.tagsA().indexOf(',') > 0) {
                return self.maxCharacter(self.tagsA().length - 1);
            };
            return self.tagsA();
        });

        self.commentedByPopUp = function (threadId) {
            var url = $('#commented-by').data('url');
            var comments = '';
            $.get(url + '?threadId=' + threadId, function (callback) {
                $.each(callback.Result, function (i, v) {
                    comments = comments + '' + v.CommentedBy + '<br />';
                });
                $("#close_" + threadId).show();
                $("#show_" + threadId).show().html('<small>Commented By:</small><br />' + '<small>' + comments + '</small>');
            });
        }

        $("#fpbtn").on("click", function () {
            var val = $("#unameEmail").val();
            var url = $("#rp").data('url');
            $.post(url + "?email=" + encodeURIComponent(val), function (callback) {
                $("#mess").text(callback.Message).show();
                return;
            });
        });
        self.resetPass = function () {
            var val = $("#unameEmail").val();
            var url = $("#rp").data('url');
            $.post(url + "?email=" + encodeURIComponent(val), function (callback) {
                $("#mess").text(callback.Message).show();
            });
        }

        self.viewedByPopUp = function (threadId) {
            var url = $('#viewed-by').data('url');
            var views = '';
            $.get(url + '?threadId=' + threadId, function (callback) {
                $.each(callback.Result, function (i, v) {
                    views = views + '' + v.ViewedBy + '<br />';
                });
                $("#close_" + threadId).show();
                $("#show_" + threadId).show().html('<small>Viewed By:</small><br />' + '<small>' + views + '</small>');
            });
        }
        self.watchedByPopUp = function (threadId) {
            var url = $('#watched-by').data('url');
            var watch = '';
            $.get(url + '?threadId=' + threadId, function (callback) {
                $.each(callback.Result, function (i, v) {
                    watch = watch + '' + v.WatchedBy + '<br />';
                });
                $("#close_" + threadId).show();
                $("#show_" + threadId).show().html('<small>Watched By:</small><br />' + '<small>' + watch + '</small>');
            });
        }


        self.hidelikedByPopUp = function (threadId) {
            $("#show_" + threadId).hide();
            $("#close_" + threadId).hide();
        }

        self.displayThread = function (threadId) {
            var isliked = $("#like_" + threadId).attr("data-liked");
            var iswatched = $("#watch_" + threadId).attr("data-watched");
            window.location.href = 'Page/DisplayThread?threadId=' + threadId + "&isLiked=" + isliked + "&isWatched=" + iswatched;

        }

        self.updateLikes = function (threadId, id) {
            var url = $("#likeUrl").data('url');
            if (url == undefined)
                url = 'Page/Like';
           // alert(url);
            $.post(url + '?threadId=' + threadId, function (callback, status) {
                if (!callback.IsAuthenticated) {
                    $("#loginMsg1_" + threadId).show();
                }
                else if (callback.Result == "Success") {
                    $("#likefig_" + threadId).text(callback.totalLikes);
                    $("#like_" + threadId).hide();
                    $("#likeh_" + threadId).show();
                    $("#like_" + threadId).attr("data-liked", 'true');
                    return;
                }
            });
        }
        self.updateWatch = function (threadId, id) {
            var url = $("#watchUrl").data('url');
            if (url == undefined)
                url = 'Page/Watch';
            $.post(url + "?threadId=" + threadId, function (callback, status) {
                if (!callback.IsAuthenticated) {
                    $("#loginMsg1_" + threadId).show();
                }
                else if (callback.Result == "Success") {
                    $("#watchfig_" + threadId).text(callback.totalWatch);
                    $("#watch_" + threadId).hide();
                    $("#watchh_" + threadId).show();
                    $("#watch_" + threadId).attr("data-watched", 'true');
                    return;
                }
            });
        }


        self.isThread.subscribe(function (newValue) {
            if (newValue == "generalThread") {
                data.tagsA.removeAttr('value');
                data.tagsA.removeAttr('placeholder');
                data.editor.removeAttr('value');
                data.editor.removeAttr('placeholder');
                data.editor.val('');
                self.tagsA('');
                self.temp([]);
            }
            else if (newValue == "classified") {
                var housing = $("#housing");
                var common = $("#common");
                var bs = $("#buysell");
                var job = $("#job");
                var loc = $("#loc");

                self.classifiedChosen.subscribe(function (newv) {
                    if (newv == undefined) {
                        bs.hide(); job.hide();
                        housing.hide(); common.hide();loc.hide();
                    }
                    else if (newv == "Housing") {
                        bs.hide(); job.hide();
                        housing.slideDown(); common.slideDown();loc.slideDown();
                    }
                    else if (newv == "BuySell") {
                        housing.hide(); job.hide();
                        bs.slideDown(); common.slideDown(); loc.slideDown();
                    }
                    else if (newv == "Jobs") {
                        housing.hide();bs.hide();
                        common.hide();job.slideDown();loc.slideDown();
                    }
                });
            }
            else {
                data.tagsG.removeAttr('value');
                data.tagsG.removeAttr('placeholder');
                data.title.removeAttr('value');
                data.title.removeAttr('placeholder');
                data.title.val('');
                $('#content_ifr').contents().find("body").html('');
                self.tagsG('');
                self.temp1([]);
            }
        });

        self.createTags = function () {
            if (self.owntag().indexOf(',') > 0) {
                var items = self.owntag().split(',');
                $.each(items, function (i) {
                    if ($.inArray(items[i], self.tagsArray()) == -1) {
                        self.tagsArray.push(items[i]);
                        self.owntag('');
                    }
                });
            } else {
                if ($.inArray(self.owntag(), self.tagsArray()) == -1) {
                    self.tagsArray.push(self.owntag());
                    self.owntag('');
                }
            }
        }

        self.removeTags = function (item) {
            self.tagsArray.remove(item);

        }
        self.populateTagField = function (tag) {
            //not adding more than one now - changed the idea
            if (self.tagsA().length > 0 || self.tagsG().length > 0) {
                return;
            }
            if (self.isThread() == 'announcement') {
                if ($.inArray(tag, self.temp()) == -1) {
                    self.temp.push(tag);
                    self.tagsA(self.temp());

                }
            } else {
                if ($.inArray(tag, self.temp1()) == -1) {
                    self.temp1.push(tag);
                    self.tagsG(self.temp1().toString());

                }
            }
        }

        self.resetTags = function () {
            if (self.isThread() == 'announcement') {
                self.tagsA('');
                self.temp([]);
            } else {
                self.tagsG('');
                self.temp1([]);
            }
        }

        self.toSignUp = function () {
            window.location.href = "/Account/SignUp";
        }

        self.toAddThread = function () {
            window.location.href = "/Page/AddThread";
        }

        self.LogOn = function () {
            forms.logon.submit();
        }

        self.count = ko.computed(function () {
            var countNum = self.maxCharacter() - self.announcement().length;
            return countNum;
        });

        self.validateSignUp = function () {
            if (self.username() == undefined || self.email() == undefined || self.password() == undefined || self.confirmPassword() == undefined) {
                self.validationErrors("<span>* All fields are required.</span>");
                return;
            }
            if (!re.test(self.email())) {
                self.validationErrors("<span>* Please enter valid email.</span>");
                return;
            }
            if (self.password() !== self.confirmPassword()) {
                self.validationErrors("<span> * Password and Confirm Password do not match.</span>");
                return;
            }
            forms.register.submit();
        }

        self.validateAddThread = function () {
            if (self.username() == undefined || self.username() == ''
                || self.email() == undefined || self.email() == ''
                || self.announcement() == ""
                || self.tagsA() == '') {
                self.validationErrors("<span>* All fields are required.</span>");
                return;
            }
            if (!re.test(self.email())) {
                self.validationErrors("<span>* Please enter valid email.</span>");
                return;
            }

            forms.thread.submit();
        }
        self.validateClassifiedThread = function () {
            var title = $("#title_classified").val();
            var lc = $("#location_city").val();
            var ls = $("#location_state").val();
            var lcn = $("#location_country").val();
        
           if (self.username() == undefined || self.email() == undefined  || title == '' || lc =='' ||ls==''||lcn=='') {
                self.validationErrors("<span>* Username, Email, Title and Location Fields are required.</span>");
                return;
            }
            if (!re.test(self.email())) {
                self.validationErrors("<span>* Please enter valid email.</span>");
                return;
            }

            if ($("#housing").css('display') !== 'none') {
                if ($("#housing-select").val() == '') {
                    self.validationErrors("<span>* Please select one of the item from housing dropdown.</span>");
                    return;
                }
            }
            if ($("#buysell").css('display') !== 'none') {
                if ($("#buysell-select").val() == '') {
                    self.validationErrors("<span>* Please select one of the item from buy/sell dropdown.</span>");
                    return;
                }
            }
          
            forms.thread.submit();

        }
        self.validateGeneralThread = function () {
          
            if (self.username() == undefined || self.username() == ''
                ||self.email() == undefined || self.email() == ''
                || self.title() == undefined || self.title() == ''
                || tinymce.get('content').getContent() == '' || self.tagsG() == undefined) {
                self.validationErrors("<span>* All fields are required.</span>");
                return;
            }
            if (!re.test(self.email())) {
                self.validationErrors("<span>* Please enter valid email.</span>");
                return;
            }
            forms.thread.submit();
        }
        self.isAuthenticated = function () {
            var cookie = getCookie("LogOnCookie");
            if (cookie == "") {
                return false;
            }
            return true;
        }

        self.showLoginMsg = function (threadId) {
            if (!self.isAuthenticated()) {
                $("#loginMsg1_" + threadId).show();
            }
        }

        self.hideLoginMsg = function (threadId) {
            $("#loginMsg1_" + threadId).hide();
        }

        self.loadMoreThreads = function () {
            var url = $("#reqUrl").data('url');
            var block = $("#_psize").val();
            var p = parseInt($("#_psizeConst").val());
            var that = this;
            var moreThreads = $("#morethreads");
            $.get(url + '?size=' + block, function (page) {
                if (page.length == 0) {
                    moreThreads.text("No more threads available.");
                    // return;
                }
                $("#_psize").val(parseInt(block) + p);
                moreThreads.append(page);
            }).then(function () {
                ko.applyBindings(that);
            });
        }

        self.loadMoreCategoryThreads = function () {
            var url = $("#reqUrl").data('url');
            var block = $("#_psize").val();
            var p = parseInt($("#_psizeConst").val());

            $.get(url + '?type=' + $("#tagType").val() + '&category=' + encodeURIComponent($("#tagCategory").val()) + '&size=' + block, function (page) {
                console.log("page", page);
                if (page.length == 0) {
                    $("#moreThread").parent().hide();
                    return;
                }
                $("#_psize").val(parseInt(block) + p);
                $("#morethreads").append(page);
            });
        }

        self.loadMorePostByUser = function () {
            var morepost = $("#moreposts");
            var uname = $("#uname").val();
            var url = $("#morePosts").data('url');
            var loadmore = $("#showmore").data('url');
            var pval = $("#psize").val();

            $.get(url + '?username=' + uname + "&size=" + pval, function (callback, status) {
                $("#psize").val(parseInt(callback.length) + parseInt(pval));
                if (pval == $("#totalPost").val()) {
                    $(".btn-lg").hide();
                    return;
                }
                if (callback.length > 0) {
                    $.each(callback, function (i, v) {
                        if (v.Title == null)
                            v.Title = '';
                        if (v.announcement == null)
                            v.announcement = '';
                        var str = moment(v.SubmittedOn).format("MM/DD/YYYY hh:mm:ss a"); //json string
                        morepost.append("<ul><li class='clearfix'><a href=" + loadmore + "?threadId=" + v.ThreadId + ">" + v.Title + "</a>" +
                            "<a href=" + loadmore + "?threadId=" + v.ThreadId + ">" + v.Announcement + "</a><br />" + str.toString() +
                            "<br /><small>" + v.Views + " views" + " | " + v.Likes + " likes" + " | " + v.Responses + " comments" + "</small></li></ul>");
                    });
                }
            });
        }

        self.submitComment = function () {
            var threadId = $("#commentThreadId").val();
            var commentbox = tinymce.get('contentComment').getContent();
            var url = $("#commentForm").attr('action');
            if (commentbox == '') {
                self.comment('Comment cannot be empty.');
                return;
            }
            $.post(url + '?comment=' + commentbox + '&threadId=' + threadId, function (callback, status) {
                //  $("#totalComments").text(callback.TotalCount);
                // tinyMCE.activeEditor.setContent("");
                //  $('html, body').animate({ scrollTop: $('#totalComments').offset().top }, 2000);
                // alert(callback.Comments);
                location.reload(true);

            });
        }

        customHandlers();
    }

    window.ko.applyBindings(new viewModel());


    if (getCookie("LogOnCookie") != "") {
        $.post('/Page/GetLikedThreads', function (callback, status) {
            if (callback.LikedThreads.indexOf(',') != -1) {
                var threads = callback.LikedThreads.split(',');
                $.each(threads, function (i, v) {
                    $("#like_" + v).hide();
                    $("#likeh_" + v).show();
                    $("#like_" + v).attr("data-liked", 'true');
                });
            } else {
                $("#like_" + callback.LikedThreads).hide();
                $("#like_" + callback.LikedThreads).show();
                $("#like_" + callback.LikedThreads).attr("data-liked", 'true');
            }
            if (callback.WatchedThreads.indexOf(',') != -1) {
                var threadsW = callback.WatchedThreads.split(',');
                $.each(threadsW, function (i, v) {
                    $("#watch_" + v).hide();
                    $("#watchh_" + v).show();
                    $("#watch_" + v).attr("data-watched", 'true');
                });
            } else {
                $("#watch_" + callback.WatchedThreads).hide();
                $("#watchh_" + callback.WatchedThreads).show();
                $("#watch_" + callback.WatchedThreads).attr("data-watched", 'true');
            }

        });
    }

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
    //custom bindings
    function customHandlers() {
        window.ko.bindingHandlers.limitCharacters = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                element.value = element.value.substr(0, valueAccessor());
                allBindingsAccessor().value(element.value.substr(0, valueAccessor()));
            }
        };
        window.ko.bindingHandlers.fadeVisible = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var value = valueAccessor();
                $(element).toggle(ko.utils.unwrapObservable(value));
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = valueAccessor();
                ko.utils.unwrapObservable(value) ? $(element).delay("slow").fadeIn() : $(element).delay("slow").fadeOut();
            }
        };
    }
    Array.prototype.uniq = function () {
        return this.filter(
            function (a) { return !this[a] ? this[a] = true : false; }, {}
        );
    }
    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }


});