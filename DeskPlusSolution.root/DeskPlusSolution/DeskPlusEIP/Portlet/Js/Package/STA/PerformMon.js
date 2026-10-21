$(document).ready(function () {
    var portletDom = performMon_tabPortletID + "_" + performMon_portletID;

    // 검색 아이콘
    if ($(".IconSearch") != undefined) { $(".IconSearch").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    //관리자 여부
    if (performMon_IsAuth != "True") {
        $("#ptConfig_" + portletDom).remove();
    }
    //최소화 여부
    if (performMon_IsMini == "1") {
        $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
        $("#ptContent_" + portletDom).fadeOut("slow");
    }

    //포틀릿 최소화/최대화
    $("#ptSlide_" + portletDom).on("click", function (e) {
        if ($("#hdnTabSlide_" + portletDom).val() == "1") {
            //최대화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
            $("#ptContent_" + portletDom).fadeIn("slow");
            $("#hdnTabSlide_" + portletDom).val("0");
        }
        else {
            //최소화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
            $("#ptContent_" + portletDom).fadeOut("slow");
            $("#hdnTabSlide_" + portletDom).val("1");
        }
    });


    //포틀릿 설정
    $("#ptConfig_" + portletDom).on("click", function (e) {
        $("#ptListConfig_" + portletDom).slideToggle(600);
    });

    // 포틀릿 삭제
    $("#cbtnPDel_" + portletDom).click(function (e) {
        e.preventDefault();

        Main.DelTabPortlet(portletDom);
        if (PortletAuth == "True") {
            $("#portlet_" + portletDom).fadeOut("slow");
        }
    });

    //setTimeout(function () {
        performMon_LoadData();
    //}, 1);

});

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function performMon_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(performMon_tabPortletID, performMon_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }

        case "max":
            {
                Main.ModMinMaxPortlet(performMon_tabPortletID, performMon_portletID, "0");
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
function performMon_LoadData() {
    var portletDom = performMon_tabPortletID + "_" + performMon_portletID;
    var count = 24;

    if ($("#ptContent_" + portletDom).width() < 300) {
        count = 5;
    }

    var data = JSON.stringify({
        _machineName: machinName,
        _count: count
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetPerform",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            performMon_GetPerformData(response);
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


function performMon_GetPerformData(response) {

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
                    label: "CPU %",
                    backgroundColor: "rgba(255,99,132,0.2)",
                    borderColor: "rgba(255,99,132,1)",
                    pointColor: "rgba(255,99,132,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(255,99,132,1)",
                    borderWidth : 1,
                    data: listCPU
                },
                {
                    label: "Memory %",
                    backgroundColor: "rgba(54, 162, 235,0.2)",
                    borderColor: "rgba(54, 162, 235,1)",
                    pointColor: "rgba(54, 162, 235,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(54, 162, 235,1)",
                    borderWidth: 1,
                    data: listMEM
                }
            ]
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
}