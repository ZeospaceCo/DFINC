List = {}

$(function () {

    List.GetPerform(SelTab);
    List.GetDisk(SelTab);
    List.GetDiskMByte(SelTab);
});



List.SetTab = function (_id) {

/*
    switch (_id.toUpperCase()) {
        case "PerformMonS":
            $("#PerformMonS").attr("class", "tab_on");
            $("#PerformMonF").attr("class", "");
            $("#MPerformMonS").attr("class", "");
            $("#MPerformMonF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        default:
            $("#PerformMonS").attr("class", "tab_on");
            $("#PerformMonF").attr("class", "");
            $("#MPerformMonS").attr("class", "");
            $("#MPerformMonF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
    }
*/
}

List.GetPerform = function (_data) {
    var data = JSON.stringify({
        _machineName: _data,
        _count: 24
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetPerform",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            List.GetPerformData(response);
        },
        error: function () {
            /*
            if (count == 0) {
            //alert("Load ERROR");
            setTimeout(function () {
            Main.LoadPortlet(column_ID, portletPath, portletID);
            }, 100);
            }
            else {
            jQuery(column_ID).append('Load ERROR');
            }

            count += 1;
            */
        }

    });
}

List.GetPerformData = function (response) {

    var cData = response.d;
    var chartLabels = cData[0];
    var listCPU = cData[1];
    var listMEM = cData[2];

    var ctx = document.getElementById("lineChart");
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: chartLabels,
            datasets: [
                {
                    label: "CPU 사용량 %",
                    backgroundColor: "rgba(255,99,132,0.2)",
                    borderColor: "rgba(255,99,132,0.5)",
                    pointColor: "rgba(255,99,132,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(255,99,132,1)",
                    data: listCPU
                },
                {
                    label: "최대 메모리 사용량 %",
                    backgroundColor: "rgba(54, 162, 235,0.2)",
                    borderColor: "rgba(54, 162, 235,0.5)",
                    pointColor: "rgba(54, 162, 235,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(54, 162, 235,1)",
                    data: listMEM
                }
            ]
        },
        options: {
            scaleShowGridLines: true,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            bezierCurve: true,
            bezierCurveTension: 0.4,
            pointDot: true,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 2,
            datasetFill: true,
            responsive: true,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        min: 0,
                        max: 100
                    }
                }]
            }
        }
    });
/*
    var lineData = {
        labels: chartLabels,
        datasets: [
            {
                label: "CPU",
                fillColor: "rgba(220,220,220,0.5)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: listCPU
            },
            {
                label: "Memory",
                fillColor: "rgba(26,179,148,0.5)",
                strokeColor: "rgba(26,179,148,0.7)",
                pointColor: "rgba(26,179,148,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(26,179,148,1)",
                data: listMEM
            }
        ]
    };

    var lineOptions = {
        scaleShowGridLines: true,
        scaleGridLineColor: "rgba(0,0,0,.05)",
        scaleGridLineWidth: 1,
        bezierCurve: true,
        bezierCurveTension: 0.4,
        pointDot: true,
        pointDotRadius: 4,
        pointDotStrokeWidth: 1,
        pointHitDetectionRadius: 20,
        datasetStroke: true,
        datasetStrokeWidth: 2,
        datasetFill: true,
        responsive: true,
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    min: 0,
                    max: 100
                }
            }]
        }

    };
    var ctx = document.getElementById("lineChart").getContext("2d");
    var myNewChart = new Chart(ctx).Line(lineData, lineOptions);
*/
}

List.GetDisk = function (_data) {
    var data = JSON.stringify({
        _machineName: _data
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetDisk",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            List.GetDiskData(response);
        },
        error: function () {
            /*
            if (count == 0) {
            //alert("Load ERROR");
            setTimeout(function () {
            Main.LoadPortlet(column_ID, portletPath, portletID);
            }, 100);
            }
            else {
            jQuery(column_ID).append('Load ERROR');
            }

            count += 1;
            */
        }

    });
}

List.GetDiskData = function (response) {

    var cData = response.d;
    var chartLabels = cData[0];
    var listDisk = cData[1];


    var ctx = document.getElementById("barChart");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: chartLabels,
            datasets: [{
                label: "논리 디스크의 여유공간 %",
                backgroundColor: "rgba(54, 162, 235,0.5)",
                borderColor: "rgba(54, 162, 235,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: listDisk
            }]
        },
        options: {
            responsive: true,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        min: 0,
                        max: 100
                    }
                }]
            }
        }
});

/*
    var barData = {
        labels: chartLabels,
        datasets: [
            {
                label: "Disk",
                fillColor: "rgba(220,220,220,0.5)",
                strokeColor: "rgba(220,220,220,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: listDisk
            }
        ]
    };

    var barOptions = {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    min: 0,
                    max: 100
                }
            }]
        }
    };
    var ctx = document.getElementById("barChart").getContext("2d");
    var myNewChart = new Chart(ctx).Bar(barData, barOptions);
*/
}

List.GetDiskMByte = function (_data) {
    var data = JSON.stringify({
        _machineName: _data
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetDiskMByte",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
                var cData = response.d;
                var freeMBytes = cData[2];//.toString();
                document.getElementById("diskMbytes").innerHTML = freeMBytes.join(",");
                //$("diskMbytes").innerHTML(freeMBytes);
                
                
        },
        error: function () {
            /*
            if (count == 0) {
            //alert("Load ERROR");
            setTimeout(function () {
            Main.LoadPortlet(column_ID, portletPath, portletID);
            }, 100);
            }
            else {
            jQuery(column_ID).append('Load ERROR');
            }

            count += 1;
            */
        }

    });
}
