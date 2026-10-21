Static = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        // 최대 제안자(년도)
        $('#ddlMaxYear').change(function (e) {
            e.preventDefault();

            var selStartYearTxt = $("#ddlMaxYear option:selected").text();

            if (selStartYearTxt == "▲" || selStartYearTxt == "▼") {
                Static.LoadMaxYear($('#ddlMaxYear').val());
            }
        });

        // 부서별 제안(년도)
        $('#ddlDeptYear').change(function (e) {
            e.preventDefault();

            var selDeptYearTxt = $("#ddlDeptYear option:selected").text();

            if (selDeptYearTxt == "▲" || selDeptYearTxt == "▼") {
                Static.LoadDeptYear($('#ddlDeptYear').val());
            }
        });

        // 월별 제안(년도)
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                Static.LoadYear($('#ddlYear').val());
            }
        });

        // 최대 제안자(년도) 변경
        $('#ddlMaxYear').change(function () {
            Static.SearchMaxYearMonth();
        });

        // 최대 제안자(월) 변경
        $('#ddlMaxMonth').change(function () {
            Static.SearchMaxYearMonth();
        });

        // 부서별 제안(년도) 변경
        $('#ddlDeptYear').change(function () {
            Static.SearchDeptYearMonth();
        });

        // 부서별 제안(월) 변경
        $('#ddlDeptMonth').change(function () {
            Static.SearchDeptYearMonth();
        });

        // 월별 제안(년도) 변경
        $('#ddlYear').change(function () {
            Static.SearchYear();
        });

        Static.Init();

    });
});


/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
Static.Init = function () {

    Static.LoadMaxYear(StartYears);
    Static.LoadDeptYear(StartYears);
    Static.LoadYear(StartYears);

    // 사업부별 제안현황
    Static.SearchDeptYearMonth();
    // 월별 제안현황
    Static.SearchYear();
    // 최대 제안자
    Static.SearchMaxYearMonth();
    

    __addEventListener(document, "click", Static.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 최대 제안자 년도 위아래 체크하기
***************************************************/
Static.LoadMaxYear = function (p_year) {

    $('#ddlMaxYear').empty();
    $('#ddlMaxYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlMaxYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlMaxYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlMaxYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlMaxYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 사업부별 년도 위아래 체크하기
***************************************************/
Static.LoadDeptYear = function (p_year) {

    $('#ddlDeptYear').empty();
    $('#ddlDeptYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlDeptYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlDeptYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlDeptYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlDeptYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 월별 년도 위아래 체크하기
***************************************************/
Static.LoadYear = function (p_year) {

    $('#ddlYear').empty();
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 최대 제안자 검색 (웹서비스 호출)
***************************************************/
Static.SearchMaxYearMonth = function () {
    MaxYear = $('#ddlMaxYear').val();
    MaxMonth = $('#ddlMaxMonth').val();
    
    var url = "/DeskPlusEIP/Proposal/Ws/Content/Statistics.asmx/GetMaxUserInfo";

    ParamArray = {
        'maxYear': MaxYear,
        'maxMonth': MaxMonth
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Static.SuccessMaxYearMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 최대 제안자 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Static.SuccessMaxYearMonth = function (data, status) {
    if (data.d.length > 0) {
        var tmpInfo = data.d.split(',');
        var mylink = "<a onclick=\"AccountInfo('" + tmpInfo[1] + "');return false;\" style='cursor:pointer'>" + tmpInfo[0] + "</a>";
        $("#userInfo").html(mylink + "(" + tmpInfo[2] + ")");
    }
    else {
        $("#userInfo").text("없음 (0건)");
    }

    Static.MonthlyProposal();
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 우수제안소개 (웹서비스 호출)
***************************************************/
Static.MonthlyProposal = function () {

    MaxYear = $('#ddlMaxYear').val();
    MaxMonth = $('#ddlMaxMonth').val();

    var url = "/DeskPlusEIP/Proposal/Ws/Content/Statistics.asmx/GetMaxScoreInfo";

    ParamArray = {
        'maxYear': MaxYear,
        'maxMonth': MaxMonth
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Static.SuccessMonthlyProposal");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 우수제안소개 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Static.SuccessMonthlyProposal = function (data, status) {

    var report = "";

    if (data.d.length > 0) {
        var usrID = data.d[0].UserID;
        var name = data.d[0].UserName;
        var title = data.d[0].Title;
        var afterCont = data.d[0].AfterCont;

        var mylink = "<a onclick=\"AccountInfo('" + usrID + "');return false;\" style='cursor:pointer'>" + name + "</a>";

        report = "<table class='table_typeE'>"
        + "<caption>우수제안소개</caption><colgroup><col width='100px' /><col /></colgroup>"
        + "<tr><th>제안자</th><td style='text-align:left;border-bottom:1px solid #ddd !important'>" + mylink + "</td></tr>"
        + "<tr><th>제안제목</th><td style='text-align:left;border-bottom:1px solid #ddd !important'>" + title + "</td></tr>"
        + "<tr style='height:180px'><th>제안내용</th><td style='text-align:left;border-bottom:1px solid #ddd !important'>" + afterCont + "</td></tr></table>";

        /*
        report = "<table class='table_typeC'>"
        + "<caption>우수제안소개</caption><colgroup><col width='100px' /><col /></colgroup>"
        + "<tr><th>제안자</th><td style='border-bottom: 1px solid rgb(210,210,210)'>" + mylink + "</td></tr>"
        + "<tr><th>제안제목</th><td style='border-bottom: 1px solid rgb(210,210,210)'>" + title + "</td></tr>"
        + "<tr style='height:180px'><th>제안내용</th><td>" + afterCont + "</td></tr></table>";
        */
        $("#monthlyReport").html(report);
    }
    else {
        report = "<table class='table_typeC'>"
        + "<caption>우수제안소개</caption><colgroup><col width='100px' /><col /></colgroup>"
        + "<tr><th>제안자</th><td style='text-align:left;border-bottom:1px solid #ddd !important'>우수 제안자가 없습니다.</td></tr>"
        + "<tr><th>제안제목</th><td style='text-align:left;border-bottom:1px solid #ddd !important'>&nbsp;</td></tr>"
        + "<tr style='height:180px'><th>제안내용</th><td>&nbsp;</td></tr></table>";

        /*
        report = "<table class='table_typeC'>"
        + "<caption>우수제안소개</caption><colgroup><col width='100px' /><col /></colgroup>"
        + "<tr><th>제안자</th><td style='border-bottom: 1px solid rgb(210,210,210)'>우수 제안자가 없습니다.</td></tr>"
        + "<tr><th>제안제목</th><td style='border-bottom: 1px solid rgb(210,210,210)'>&nbsp;</td></tr>"
        + "<tr style='height:180px'><th>제안내용</th><td>&nbsp;</td></tr></table>";
        */
        $("#monthlyReport").html(report);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 사업부별 검색 (웹서비스 호출)
***************************************************/
Static.SearchDeptYearMonth = function () {
    MaxYear = $('#ddlDeptYear').val();
    MaxMonth = $('#ddlDeptMonth').val();

    var url = "/DeskPlusEIP/Proposal/Ws/Content/Statistics.asmx/GetDeptInfo";

    ParamArray = {
        'maxYear': MaxYear,
        'maxMonth': MaxMonth
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Static.SuccessDeptYearMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 사업부별 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Static.SuccessDeptYearMonth = function (data, status) {
    $('#phDeptList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTStatistics.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phDeptList').processTemplate(data.d);  
}


/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 월별 제안현황 검색(웹서비스 호출)
***************************************************/
Static.SearchYear = function () {
    MaxYear = $('#ddlYear').val();

    var url = "/DeskPlusEIP/Proposal/Ws/Content/Statistics.asmx/GetYearInfo";

    ParamArray = {
        'maxYear': MaxYear
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Static.SuccessSearchYear");

    AjaxService(url, data, obj);

}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 월별 제안현황 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Static.SuccessSearchYear = function (data, status) {
    $('#phMonthList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTMonthList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMonthList').processTemplate(data.d);
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Static.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Static.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Static.ShowDelContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값


    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem("삭제", "Write.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}