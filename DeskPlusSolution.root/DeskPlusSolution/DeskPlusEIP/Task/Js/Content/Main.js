Main = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-07
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        // 기간
        $("#ddlYear").change(function (e) {
            e.preventDefault();
            Main.ChangeYear();
        });

        Main.Init();

    });
});

/**************************************************
작성자   : famo23
작성일자 : 2015-05-07
작성목적 : 초기화 하기
***************************************************/
Main.Init = function () {

    $("#dvChart").width($("#line").width() - 10);
    Main.GenerateChart(Graph1, Graph2);

    $("#spTot").html(CntTot);
    $("#spI").html(CntI);
    $("#spD").html(CntD);
    $("#spC").html(CntC);
    $("#spBeforeI").html(CntBeforeI);
    $("#spBeforeD").html(CntBeforeD);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-08
작성목적 : 차트그리기
***************************************************/
var yunit = 3;     //  pixel/1단위 (1개당 높이)
Main.GenerateChart = function (_HorXString, _VerYString) {

    var arrX = new Array();
    arrX = _HorXString.split("|");

    var arrY = new Array();
    arrY = _VerYString.split("|");

    var cont = "";
    cont += "<table id='tblgraph' class='tabstat'>";
    cont += "<tr>";

    // Y좌표
    cont += "<td style='vertical-align:bottom;'>";
    cont += "<table cellpadding=0 cellspacing=0 border=0 width=100%>";

    for (var j = TopCnt; j > 50; j -= 10) {
        cont += "<tr>";
        cont += "<td class='y' height=30>" + j + "</td>";
        cont += "</tr>";
    }

    cont += "<tr>";
    cont += "<td class='y' height=30>50</td>";
    cont += "</tr>";
    cont += "<tr>";
    cont += "<td class='y' height=30>40</td>";
    cont += "</tr>";
    cont += "<tr>";
    cont += "<td class='y' height=30>30</td>";
    cont += "</tr>";
    cont += "<tr>";
    cont += "<td class='y' height=30>20</td>";
    cont += "</tr>";
    cont += "<tr>";
    cont += "<td class='y' height=30>10</td>";
    cont += "</tr>";
    cont += "</table>";
    cont += "</td>";

    // X좌표-Graph
    if (_VerYString != "") {

        for (var i = 0; i < arrY.length; i++) {

            var item = arrY[i].split(",");

            cont += "<td class='gap'>";
            cont += "<table cellpadding=0 cellspacing=0 border=0 style='width:50px;'>";
            cont += "<tr title='" + R_TotalCnt + " : " + item[0] + "'>";
            cont += "<td class='x'>" + item[1] + "<div class='I' style='height:" + parseInt(item[1]) * yunit + "px;'></div></td>";
            cont += "<td class='x'>" + item[2] + "<div class='D' style='height:" + parseInt(item[2]) * yunit + "px;'></div></td>";
            cont += "<td class='x'>" + item[3] + "<div class='C' style='height:" + parseInt(item[3]) * yunit + "px;'></div></td>";

            cont += "</tr>";
            cont += "</table>";
            cont += "</td>";
        }
        cont += "</tr>";
    }

    if (_HorXString != "") {
        cont += "<tr>";
        cont += "<td class='txt'>&nbsp;</td>";

        // X좌표-설명
        for (var i = 0; i < arrX.length; i++) {
            cont += "<td class='txt' style='width:50px;'>" + arrX[i] + "</td>";
        }

        cont += "</tr>";
    }
    cont += "</table>";

    document.getElementById("dvChart").innerHTML = cont;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-08
작성목적 : 리스트 이동
**********************************************************************************************/
Main.GoList = function (p_itemID) {

    ParamArray = {
        'cabinetID': "Total",
        'itemID': p_itemID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
    parent.frmLeft.location = url;

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-08
작성목적 : 리스트 이동
**********************************************************************************************/
Main.ChangeYear = function () {

    var year = $("#ddlYear").val();

    ParamArray = {
        'year': year
    };
    CallEncrypt(CallGetParam());

    location = "Main.aspx?p=" + GetEncryptValue;

}