$(document).ready(function () {
    var options = {
       searchResults : $("#searchResults"),
       btnGo1 : $("#btnGo1"),
       searchTerm: $("#term_search"),
       ajaxLoader: $("#ajaxloader").data('ajaxloader'),
       url:$("#urlSearch1").data('url')
    };
   

    function searchByUsername() {
        options.searchResults.empty();
        var fullUrl = options.url + "?term=" + encodeURIComponent(options.searchTerm.val());
        options.searchResults.html('<center><img src = ' + options.ajaxLoader + '></center>');
        $.get(fullUrl, function(page) {
            if (page.length == 0) {
                options.searchResults.empty();
                options.searchResults.append("No Results Found.");
                return;
            }
            options.searchResults.empty();
            options.searchResults.append(page);
        });

    }

    

    $("ul li a").on("click", function () {
        var id = $(this).attr('id');
        var results = $("#resultpage");
        var fullUrl = options.url + "?term=" + options.searchTerm.val() + "&page=" + id;
        $.get(fullUrl, function(page) {
            results.empty();
            results.append(page);
        });
    });

    options.btnGo1.on("click", function () {
        searchByUsername();
    });

    options.searchTerm.keydown(function (event) {
        var keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == 13) {
            options.btnGo1.trigger('click');
        }
    });
   
});