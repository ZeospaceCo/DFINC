$(document).ready(function () {
    var portletDom = chartBar_tabPortletID + "_" + chartBar_portletID;

    // 검색 아이콘
    if ($(".IconSearch") != undefined) { $(".IconSearch").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    //관리자 여부
    if (chartBar_IsAuth != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();
    }
    //최소화 여부
    if (chartBar_IsMini == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }
    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {
        var pID = el[0].id.replace('ptConfig_', '');
        chartBar_ContextMenuWork(pID, action, el, pos);
    });

    //setTimeout(function () {
        chartBar_LoadData();
    //}, 1);

});

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function chartBar_ContextMenuWork(id, action, el, pos) {
    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                if (PortletAuth == "True") {
                    $("#portlet_" + id).fadeOut("slow");
                }
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(chartBar_tabPortletID, chartBar_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }

        case "max":
            {
                Main.ModMinMaxPortlet(chartBar_tabPortletID, chartBar_portletID, "0");
                $("#ptContent_" + id).fadeIn("slow");                
                break;
            }

        case "info":
            {

                break;
            }
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2017-01-17
작성목적 : 리스트 가져오기
***************************************************/
function chartBar_LoadData() {
    var portletDom = chartBar_tabPortletID + "_" + chartBar_portletID;

    if ($("#ptContent_" + portletDom).width() < 300) {
        pCount = 5;
    }

    var data;
    
    //SP 호출일 경우
    if (chartBar_dataType == "S") {
        data = JSON.stringify({
            _dataURL: chartBar_dataURL,
            _fName: fName,
            _fValue: fValue,
            _pCount: pCount
        });
    } else { // 커스트마이징

    }

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Portlet/Ws/Package/Chart/Bar.asmx/Get",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            chartBar_Get(response);
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


function chartBar_Get(response) {
    var portletDom = chartBar_tabPortletID + "_" + chartBar_portletID;
    var cData = response.d;

    var chartLabels = cData[0];
    var list1 = cData[1];
    var list2 = cData[2];
    var list3 = cData[3];

    var ctx = document.getElementById("chartBar_" + portletDom);
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: chartLabels,
            datasets: [
                {
                    label: fValue.split('|')[2],
                    backgroundColor: "rgba(170,170,170,0.2)",
                    borderColor: "rgba(170,170,170,1)",
                    pointColor: "rgba(170,170,170,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(170,170,170,1)",
                    borderWidth : 1,
                    data: list1
                },
                {
                    label: fValue.split('|')[1],
                    backgroundColor: "rgba(255,99,132,0.2)",
                    borderColor: "rgba(255,99,132,1)",
                    pointColor: "rgba(255,99,132,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(255,99,132,1)",
                    borderWidth : 1,
                    data: list2
                },
                {
                    label: fValue.split('|')[0],
                    backgroundColor: "rgba(54, 162, 235,0.2)",
                    borderColor: "rgba(54, 162, 235,1)",
                    pointColor: "rgba(54, 162, 235,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(54, 162, 235,1)",
                    borderWidth: 1,
                    data: list3
                }
            ]
        },
        options: {

            responsive: true,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        //min: 0,
                        //max: 100
                    }
                }]
            }
        }
    });
}