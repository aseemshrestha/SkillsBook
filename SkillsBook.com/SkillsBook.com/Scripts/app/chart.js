var _title,_theme;

function setTitle(title) {
    _title = title;
}

function getTitle() {
    return _title;
}

function setTheme(theme) {
    _theme = theme;
}

function getTheme() {
    return _theme;
}

function loadChart() {
    var chart = new CanvasJS.Chart("chartContainer",
   {
       title: {
           text: ""
       },
       animationEnabled: true,
       theme: "theme2",
       data: [
           {
               type: "column",
               indexLabelFontFamily: "Garamond",
               indexLabelFontSize: 10,
               startAngle: 0,
               indexLabelFontColor: "dimgrey",
               indexLabelLineColor: "darkgrey",
               toolTipContent: "{y} %",

               dataPoints: [
                   { y: 67.34, indexLabel: "Views{y}" },
                   { y: 28.6, indexLabel: "Likes" },
                   { y: 16.78, indexLabel: "Comments" },
                   { y: 50.84, indexLabel: "Followers" }
               ]
           }
       ]
   });

    chart.render();
}

//$(document).ready(function () {
   
//	
