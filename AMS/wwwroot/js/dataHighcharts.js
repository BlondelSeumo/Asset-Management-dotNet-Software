//PIE
$(document).ready(function () {
    var titleMessage = "Total Asset: ";
    $.ajax({
        type: "GET",
        url: "/Dashboard/GetPieChartData",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var keys = Object.keys(result);
            var weeklydata = new Array();
            var totalspent = 0.0;
            for (var i = 0; i < keys.length; i++) {
                var arrL = new Array();
                arrL.push(keys[i]);
                arrL.push(result[keys[i]]);
                totalspent += result[keys[i]];
                weeklydata.push(arrL);
            }
            createPIECharts(weeklydata, titleMessage, totalspent.toFixed(0));
        }
    })
})

function createPIECharts(sum, titleText, totalspent) {
    Highcharts.chart('containerPIE', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            //text: 'Browser market shares in January, 2018'
            text: titleText + ' ' + totalspent
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            name: 'Brands',
            colorByPoint: true,
            data: sum,
        }]
    });
}