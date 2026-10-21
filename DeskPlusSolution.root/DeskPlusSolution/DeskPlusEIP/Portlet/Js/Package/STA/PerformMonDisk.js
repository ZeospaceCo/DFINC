$(document).ready(function () {
    var portletDom = performMonDisk_tabPortletID + "_" + performMonDisk_portletID;

    // 검색 아이콘
    if ($(".IconSearch") != undefined) { $(".IconSearch").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    //관리자 여부
    if (performMonDisk_IsAuth != "True") {
        $("#ptConfig_" + portletDom).remove();
    }
    //최소화 여부
    if (performMonDisk_IsMini == "1") {
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
        performMonDisk_LoadData();
        performMonDisk_GetDiskMByte();
    //}, 1);

});

/**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function performMonDisk_ContextMenuWork(id, action, el, pos) {
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
                Main.ModMinMaxPortlet(performMonDisk_tabPortletID, performMonDisk_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }

        case "max":
            {
                Main.ModMinMaxPortlet(performMonDisk_tabPortletID, performMonDisk_portletID, "0");
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
function performMonDisk_LoadData() {
    var data = JSON.stringify({
        _machineName: machinName
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetDisk",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            performMonDisk_GetPerformData(response);
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


function performMonDisk_GetPerformData(response) {
    var cData = response.d;
    var chartLabels = cData[0];
    var listDisk = cData[1];


    var ctx = document.getElementById("barChart");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: chartLabels,
            datasets: [{
                label: "Free disk space %",
                backgroundColor: "rgba(54, 162, 235,0.5)",
                borderColor: "rgba(54, 162, 235,1)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                borderWidth: 1,
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
}


function performMonDisk_GetDiskMByte() {
    var data = JSON.stringify({
        _machineName: machinName
    });

    $.ajax({
        type: "POST",
        url: "/DeskPlusEIP/Statistics/Ws/Content/PerformMon/List.asmx/GetDiskMByte",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var cData = response.d;
            var freeMBytes = cData[2]; //.toString();
            var freeMBStr = freeMBytes.join(",");

            document.getElementById("diskMbytes").innerHTML = freeMBStr.substring(freeMBStr.indexOf("C:"));
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
