$(document).ready(function () {
    var portletDom = chartPie_tabPortletID + "_" + chartPie_portletID;

    // 검색 아이콘
    if ($(".IconSearch") != undefined) { $(".IconSearch").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    //관리자 여부
    if (chartPie_IsAuth != "True") {
        $("#basicMenu_" + portletDom + " .del").remove();
    }
    //최소화 여부
    if (chartPie_IsMini == "1") {
        $("#ptContent_" + portletDom).fadeOut("slow");
    }
    $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {
        var pID = el[0].id.replace('ptConfig_', '');
        chartPie_ContextMenuWork(pID, action, el, pos);
    });

    //setTimeout(function () {
        chartPie_LoadData();
    //}, 1);

});

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function chartPie_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(chartPie_tabPortletID, chartPie_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }

        case "max":
            {
                Main.ModMinMaxPortlet(chartPie_tabPortletID, chartPie_portletID, "0");
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
function chartPie_LoadData() {
    var portletDom = chartPie_tabPortletID + "_" + chartPie_portletID;
    var data;
    
    //SP 호출일 경우
    if (chartPie_dataType == "S") {
        data = JSON.stringify({
            _dataURL: chartPie_dataURL,
            _pCount: piePCount
        });
    } else { // 커스트마이징

    }

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Portlet/Ws/Package/Chart/Pie.asmx/Get",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            chartPie_Get(response);
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


function chartPie_Get(response) {
    var portletDom = chartPie_tabPortletID + "_" + chartPie_portletID;
    var cData = response.d;
    
    var chartLabels = cData[0];
    var chartData = cData[1];

    var ctx = document.getElementById("chartPie_" + portletDom);
    var myChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: chartLabels,
            datasets: [
                {
                    backgroundColor: poolColors(piePCount),
                    data: chartData
                }
            ]
        },
        options: {

        }
    });
}

function poolColors(a) {
    var pool = [];
    for (i = 0; i < a; i++) {
        pool.push(dynamicColors());
    }
    return pool;
}

function dynamicColors() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgba(" + r + "," + g + "," + b + ",0.3)";
}