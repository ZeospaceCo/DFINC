List = {}

/**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {

        RecordPerPage = $("#ddlListCount").val();

        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 진행상태 요약 카운트 클릭 검색
        $(".edu-summary-count").click(function () {
            List.FilterByStatus($(this).attr("data-status"));
        }).keydown(function (e) {
            if (e.keyCode == 13 || e.keyCode == 32) {
                e.preventDefault();
                $(this).click();
            }
        });

        List.SetActiveStatusFilter($("#ddlStatus").val());

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModListUser();  
        });

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
        $(".clsDate").keydown(function (e) { return false; });

        List.GetStatusSummary();
        List.LoadData("");

        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", List.click);

        $("#zeoModalContact").draggable({ disabled: false });
    });
});

/**************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     
    List.SetActiveStatusFilter(SearchStatus);

    List.GetStatusSummary();
    List.LoadData("search");
}

/**************************************************/
List.FilterByStatus = function (status) {
    if (status == null) status = "";

    $("#ddlItem, #ddlComplete").val("");
    $("#ddlSearchDateField").val("StartDate");
    $("#ddlSearchField").val("Title");
    $("#startDate, #endDate").datepick("setDate", null).val("");
    $("#ctbSearch").val("");
    $("#ddlStatus").val(status);
    List.Search();
}

/**************************************************/
List.SetActiveStatusFilter = function (status) {
    if (status == null) status = "";

    $(".edu-summary-count").each(function () {
        var isActive = $(this).attr("data-status") == status;
        $(this)
            .toggleClass("filter-active", isActive)
            .attr("aria-pressed", isActive ? "true" : "false");
    });
}

/**************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadEDUData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadEDUSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

        RecordPerPage = $("#ddlListCount").val();

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadEDUSearchData();
        }
        else {
            loadEDUData();
        }
    }, 100);
}


/*******************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Education/Ws/Content/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchItem': SearchItem,
        'searchStatus': SearchStatus,
        'searchComplete': SearchComplete,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Education/Jt/Content/JTList.htm?seq=' + Guids(), null, { filter_data: false });    
    $('#phList').processTemplate(data.d);
    $('#loading').html("");
}

/*********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Education/Ws/Content/List.asmx/GetListCount";
    ParamArray = {
        'searchItem': SearchItem,
        'searchStatus': SearchStatus,
        'searchComplete': SearchComplete,
        'searchField': SearchField,
        'searchValue': SearchValue,        
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/*********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/*********************************************************************************************/
List.GetStatusSummary = function () {
    var url = "/DeskPlusEIP/Education/Ws/Content/List.asmx/GetStatusSummary";
    var requestNo = (List.StatusSummaryRequestNo || 0) + 1;
    List.StatusSummaryRequestNo = requestNo;

    // 상단 현황은 목록 검색조건과 관계없이 현재 사용자의 전체 교육을 집계합니다.
    ParamArray = {};

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, function (response, status) {
        if (requestNo == List.StatusSummaryRequestNo) {
            List.SuccessStatusSummary(response, status);
        }
    });
}


/*********************************************************************************************/
List.SuccessStatusSummary = function (data, status) {
    var summary = data.d || {};
    var totalCount = parseInt(summary.TotalCount, 10) || 0;
    var waitCount = parseInt(summary.WaitCount, 10) || 0;
    var progressCount = parseInt(summary.ProgressCount, 10) || 0;
    var completeCount = parseInt(summary.CompleteCount, 10) || 0;
    var delayCount = parseInt(summary.DelayCount, 10) || 0;
    var progressRate = totalCount > 0 ? Math.round((completeCount * 100) / totalCount) : 0;

    progressRate = Math.max(0, Math.min(100, progressRate));

    $("#summaryTotalText, #summaryTotalCount, #summaryDonutTotalCount").text(totalCount);
    $("#summaryCompleteText, #summaryCompleteCount").text(completeCount);
    $("#summaryWaitCount").text(waitCount);
    $("#summaryProgressCount").text(progressCount);
    $("#summaryDelayCount").text(delayCount);
    $("#summaryRateText").text(progressRate);
    $("#educationProgressRate").text(progressRate + "%");
    $("#educationProgressBar")
        .css("width", progressRate + "%")
        .attr("aria-valuenow", progressRate);

    $(".edu-summary-count.total").attr("aria-label", "전체 " + totalCount + "건 검색");
    $(".edu-summary-count.wait").attr("aria-label", "대기 " + waitCount + "건 검색");
    $(".edu-summary-count.progressing").attr("aria-label", "진행중 " + progressCount + "건 검색");
    $(".edu-summary-count.complete").attr("aria-label", "완료 " + completeCount + "건 검색");
    $(".edu-summary-count.delay").attr("aria-label", "지연 " + delayCount + "건 검색");

    List.DrawStatusDonut(totalCount, waitCount, progressCount, completeCount, delayCount);
}


/*********************************************************************************************/
List.DrawStatusDonut = function (totalCount, waitCount, progressCount, completeCount, delayCount) {
    var circumference = 2 * Math.PI * 37;
    var offset = 0;
    var segments = [
        { selector: "#educationDonutWait", count: waitCount },
        { selector: "#educationDonutProgress", count: progressCount },
        { selector: "#educationDonutComplete", count: completeCount },
        { selector: "#educationDonutDelay", count: delayCount }
    ];

    $.each(segments, function (index, segment) {
        var segmentLength = totalCount > 0 ? (segment.count / totalCount) * circumference : 0;
        segmentLength = Math.max(0, Math.min(circumference - offset, segmentLength));
        var gapLength = Math.max(0, circumference - segmentLength);

        $(segment.selector)
            .attr("stroke-dasharray", segmentLength.toFixed(3) + " " + gapLength.toFixed(3))
            .attr("stroke-dashoffset", (-offset).toFixed(3));

        offset += segmentLength;
    });

    $("#educationStatusDonut").attr("aria-label",
        "교육 상태 분포: 전체 " + totalCount + "건, 대기 " + waitCount + "건, 진행중 " + progressCount +
        "건, 완료 " + completeCount + "건, 지연 " + delayCount + "건");
}



/****************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.DownLoad(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/***************************************************/
List.DownLoad = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var attachFilePath = el.getAttribute("FilePath");
    attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

    window.open(attachFilePath);

}


/****************************************************/
List.SettingSearch = function () {

    SearchItem = $("#ddlItem").val();
    SearchStatus = $("#ddlStatus").val();
    SearchComplete = $("#ddlComplete").val();
    SearchDateField = $("#ddlSearchDateField").val();
    SearchField = $("#ddlSearchField").val();

    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();

    if ((getStartDate != "" && getEndDate == "") || (getStartDate == "" && getEndDate != "")) {
        alert("날짜를 둘다 입력해주세요");
        return false;
    }
    else {

        if (getStartDate == "" && getEndDate == "") {
            SearchDateField = "";
            SearchDateValue = "";
        }
        else {

            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert("날짜 설정이 잘못 되었습니다");
                return false;
            }

            SearchDateValue = getStartDate + "," + getEndDate;
        }
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/********************************************************************************************/
List.View = function (_listID) {

    ParamArray = {
        'listID': _listID,
        'currentPage': CurrentPage,
        'searchItem': SearchItem,
        'searchStatus': SearchStatus,
        'searchComplete': SearchComplete,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue
    };

    CallEncrypt(CallGetParam());

    location = "View.aspx?p=" + GetEncryptValue;
}


/*********************************************************************************************/
List.ModListUser = function () {

    var url = "/DeskPlusEIP/Education/Ws/Content/List.asmx/ModListUser";
    ParamArray = {
        'isType': "UL",
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModListUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************/
List.SuccessModListUser = function (data, status) {
    if (data.d == true) {
    }
}

