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
        $('#cbtnReset').click(function (e) {
            e.preventDefault();
            List.ResetSearch();
        });
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

        List.SetActiveStatusFilter(SearchStatus);

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModListUser();  
        });

        List.InitializeMonthPicker();
        if (List.SettingSearch() !== false) {
            List.GetStatusSummary();
            List.LoadData("");
        }

        __addEventListener(document, "click", List.click);

        $("#zeoModalContact").draggable({ disabled: false });
    });
});

/**************************************************/
List.ResetSearch = function () {
    $('#ddlItem, #ddlComplete, #ddlStatus').val('');
    List.CloseMonthPicker(false);
    $('#startDate').val(DefaultSearchStartMonth);
    $('#endDate').val(DefaultSearchMonth);
    $('#ctbSearch').val('');
    List.Search();
};

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

    $("#ddlStatus").val(status);
    List.Search();
}

/**************************************************/
List.SetActiveStatusFilter = function (status) {
    if (status == null) status = "";

    $(".edu-summary-count").each(function () {
        var itemStatus = $(this).attr("data-status");
        // 검색 버튼으로 적용한 진행상태와 같은 상단 항목만 표시합니다.
        var isActive = itemStatus == status;
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
    $('#phList').processTemplate(List.TemplateRows(data.d));
    $('#loading').html("");
}

List.TemplateRows = function (data) {
    var rows = [];
    $.each(data || [], function (_, source) {
        if (!source) return;
        var row = $.extend({}, source);
        row.CompleteUserID = List.PositiveNumber(source.CompleteUserID);
        row.HasCompleteUserInfo = row.CompleteUserID > 0 && $.trim(String(source.CompleteUserName || "")) != "";
        // 기존 필드는 유지하고 완료자 이름과 완료일만 템플릿 사본에서 한 번 인코딩합니다.
        row.CompleteUserName = row.HasCompleteUserInfo ? List.EncodeHtml(source.CompleteUserName) : "";
        row.CompleteDateStr = List.EncodeHtml(source.CompleteDateStr);
        rows.push(row);
    });
    return rows;
}

List.EncodeHtml = function (value) {
    return String(value == null ? "" : value).replace(/&/g, "&amp;").replace(/</g, "&lt;")
        .replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#39;");
}

List.PositiveNumber = function (value) {
    if (!/^[0-9]+$/.test(String(value))) return 0;
    var number = Number(value);
    return number > 0 && number <= 2147483647 ? number : 0;
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

    // 진행상태를 제외한 검색조건으로 상태별 건수와 진행률을 집계합니다.
    ParamArray = {
        'searchItem': SearchItem,
        'searchStatus': '',
        'searchComplete': SearchComplete,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue
    };

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
    $(".edu-summary-count.progressing").attr("aria-label", "진행 " + progressCount + "건 검색");
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
        "상태 분포: 전체 " + totalCount + "건, 대기 " + waitCount + "건, 진행 " + progressCount +
        "건, 완료 " + completeCount + "건, 지연 " + delayCount + "건");
}



/****************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = $(el).closest("[action]").attr("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e, false); break;
        case "certificate": List.ShowAttachContextMenu(e, true); break;
        case "accountInfo":
            var accountEvent = e || window.event;
            if (accountEvent.preventDefault) accountEvent.preventDefault();
            else accountEvent.returnValue = false;
            var userID = List.PositiveNumber($(el).closest("[action]").attr("data-user-id"));
            if (userID) AccountInfo(userID);
            break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/***************************************************/
List.ShowAttachContextMenu = function (e, isCertificate) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = null;

    var evt = new __event(e);
    var listID = $(evt.target).closest("[ListID]").attr("ListID");
    if (!listID) return;

    var url = isCertificate
        ? "/DeskPlusEIP/Education/Ws/Content/List.asmx/GetCertificateFileList"
        : "/DeskPlusEIP/Education/Ws/Admin/View.asmx/GetAttachFileList";
    var files = null;

    ParamArray = { 'listID': listID };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, function (response) {
        if (response && $.isArray(response.d)) files = response.d;
    });

    // 요청이 실패하면 이전 파일 메뉴를 다시 표시하지 않습니다.
    if (files == null) return;
    if (files.length == 0) {
        alert(isCertificate ? "등록된 첨부파일이 없습니다." : "공지첨부이 없습니다.");
        return;
    }

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();
    var itemCount = 0;
    $.each(files, function (index, file) {
        if (!file) return;
        var downloadUrl = isCertificate ? file.DownloadUrl
            : (file.FilePath ? "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + encodeURIComponent(file.FilePath) : "");
        if (!downloadUrl) return;

        var item = oContextMenuCtrl.addItem("", List.DownLoad, "");
        $(item.txt).text(file.FileName || "");
        item.txt.setAttribute("arg", downloadUrl);
        itemCount++;
    });

    if (itemCount > 0) oContextMenuCtrl.show(e);
}

/***************************************************/
List.DownLoad = function (e) {
    var evt = new __event(e);
    var downloadUrl = $(evt.target).closest("[arg]").attr("arg");
    if (downloadUrl) window.open(downloadUrl);
}


/****************************************************/
List.SettingSearch = function () {

    SearchItem = $("#ddlItem").val();
    SearchStatus = $("#ddlStatus").val();
    SearchComplete = $("#ddlComplete").val();
    SearchDateField = "StartDate";
    SearchField = "Title";

    var period = List.MonthDateRange($.trim($("#startDate").val()), $.trim($("#endDate").val()));
    if (!period) {
        alert("조회 시작월과 종료월을 올바르게 선택해 주세요.");
        return false;
    }
    SearchDateValue = period.startDate + "," + period.endDate;

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

/**********************************************************************************************/
List.MonthNumber = function (value) {
    var match = /^(\d{4})-(\d{2})$/.exec(String(value));
    if (!match || +match[1] < 1900 || +match[1] > 9999 || +match[2] < 1 || +match[2] > 12) return 0;
    return +match[1] * 100 + +match[2];
};

List.MonthDateRange = function (start, end) {
    var startNumber = List.MonthNumber(start), endNumber = List.MonthNumber(end);
    if (!startNumber || !endNumber || startNumber > endNumber) return null;
    var lastDay = new Date(Math.floor(endNumber / 100), endNumber % 100, 0).getDate();
    // 기존 목록/건수/요약 조회 규약에 맞게 선택한 월 전체를 조회합니다.
    return { startDate: start + "-01", endDate: end + "-" + lastDay };
};

List.InitializeMonthPicker = function () {
    $('.edu-month-input, .edu-month-trigger').click(function () {
        List.OpenMonthPicker($(this).attr('data-month-input') || this.id, this);
    });
    $('.edu-month-input').keydown(function (e) {
        if (e.keyCode == 13 || e.keyCode == 32 || e.keyCode == 40) {
            e.preventDefault(); List.OpenMonthPicker(this.id, this);
        }
    });
    $('#eduMonthPreviousYear').click(function () { List.ChangeMonthYear(-1); });
    $('#eduMonthNextYear').click(function () { List.ChangeMonthYear(1); });
    $('#eduMonthPicker').on('click', '[data-month]', function () {
        var value = $(this).attr('data-month');
        if (!List.MonthPicker || !List.MonthNumber(value)) return;
        $('#' + List.MonthPicker.inputID).val(value).trigger('change');
        List.CloseMonthPicker(true);
    }).keydown(function (e) {
        if (e.keyCode == 27) { e.preventDefault(); List.CloseMonthPicker(true); }
    });
    $(document).on('click.eduMonthPicker focusin.eduMonthPicker', function (e) {
        if (!$(e.target).closest('#eduMonthPicker, .edu-month-input, .edu-month-trigger').length)
            List.CloseMonthPicker(false);
    });
    $(window).on('resize.eduMonthPicker scroll.eduMonthPicker', function () { List.CloseMonthPicker(false); });
};

List.OpenMonthPicker = function (inputID, trigger) {
    if (inputID != 'startDate' && inputID != 'endDate') return;
    if (List.MonthPicker && List.MonthPicker.inputID == inputID) { List.CloseMonthPicker(true); return; }
    List.CloseMonthPicker(false);
    var value = $('#' + inputID).val(), year = +(List.MonthNumber(value) ? value : DefaultSearchMonth).substring(0, 4);
    List.MonthPicker = { inputID: inputID, trigger: trigger, year: year };
    List.RenderMonthPicker();
    $('#' + inputID).attr('aria-expanded', 'true');
    $('.edu-month-trigger[data-month-input="' + inputID + '"]').attr('aria-expanded', 'true');
    var picker = $('#eduMonthPicker').show(), bounds = document.getElementById(inputID).getBoundingClientRect();
    var left = Math.max(8, Math.min(bounds.left, $(window).width() - picker.outerWidth() - 8));
    var top = bounds.bottom + 5;
    if (top + picker.outerHeight() > $(window).height() - 8) top = Math.max(8, bounds.top - picker.outerHeight() - 5);
    picker.css({ left: left, top: top });
    List.FocusMonthPicker();
};

List.RenderMonthPicker = function () {
    var state = List.MonthPicker, selected = $('#' + state.inputID).val(), html = '';
    $('#eduMonthPickerTitle').text(state.year + '년');
    // 관리자 목록과 동일하게 현재 월에 따른 선택 제한은 두지 않습니다.
    $('#eduMonthPreviousYear').prop('disabled', state.year <= 1900);
    $('#eduMonthNextYear').prop('disabled', state.year >= 9999);
    for (var month = 1; month <= 12; month++) {
        var value = state.year + '-' + (month < 10 ? '0' : '') + month;
        var active = value == selected, current = value == DefaultSearchMonth;
        html += '<button type="button" data-month="' + value + '" class="' + (active ? 'is-selected ' : '')
            + (current ? 'is-current' : '') + '" aria-label="' + state.year + '년 ' + month + '월" aria-pressed="'
            + (active ? 'true' : 'false') + '">' + month + '월</button>';
    }
    $('#eduMonthPicker .edu-month-grid').html(html);
};

List.FocusMonthPicker = function () {
    var selected = $('#eduMonthPicker .is-selected:enabled');
    (selected.length ? selected : $('#eduMonthPicker [data-month]:enabled').first()).focus();
};

List.ChangeMonthYear = function (direction) {
    if (!List.MonthPicker || (direction != -1 && direction != 1)) return;
    var year = List.MonthPicker.year + direction;
    if (year < 1900 || year > 9999) return;
    List.MonthPicker.year = year;
    List.RenderMonthPicker();
    List.FocusMonthPicker();
};

List.CloseMonthPicker = function (restoreFocus) {
    var state = List.MonthPicker;
    if (!state) return;
    List.MonthPicker = null;
    $('#eduMonthPicker').hide();
    $('.edu-month-input, .edu-month-trigger').attr('aria-expanded', 'false');
    if (restoreFocus && state.trigger) $(state.trigger).focus();
};

