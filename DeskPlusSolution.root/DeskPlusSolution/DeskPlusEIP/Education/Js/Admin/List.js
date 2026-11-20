List = {};
List.Targets = [];
List.CourseFilter = { id: 0, title: "" };
List.AppliedSearch = null;
List.DashBoardSearch = "";
List.DataLoaded = false;
List.Loading = false;
List.ConfirmationSaving = false;

var SearchItem = "";
var SearchComplete = "";
var SearchStatus = "";
var SearchConfirm = "";
var SearchListID = 0;
var SearchDateField = "StartDate";
var SearchDateValue = "";
var SearchTarget = "A";
var SearchTargetIDs = "";
var SearchUserField = "Name";
var SearchUserValue = "";
var SearchTitle = "";
var SearchEmployment = "";
var oContextMenuCtrl = null;

/**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다.
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (List.Loading || List.ConfirmationSaving) return;
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    };

    $(document).ready(function () {

        List.RestoreSearch(InitialSearch || {});
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

        $("#ctbSearchUser, #ctbSearchTitle").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        $("#form1").submit(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 검색
        $('#cbtnReset').click(function (e) {
            e.preventDefault();
            List.ResetSearch();
        });
        $("#cbtnSearch").click(function (e) {
            e.preventDefault();
            List.Search();
        });


        // 대상 선택
        $("#ddlSearchTarget").change(function () {
            List.Targets = [];
            List.DrawTargets();
        });
        $("#cbtnSelectTargets").click(function (e) {
            e.preventDefault();
            List.OpenTargets();
        });
        // 페이지 카운트
        $("#ddlListCount").change(function () {
            List.ModListUser();
        });

        // 엑셀 다운로드
        $("#cbtnExcel").attr("title", "현재 검색조건에 해당하는 전체 내역을 다운로드합니다.").click(function (e) {
            e.preventDefault();
            List.ExcelDown();
        });

        $("#phList").on("change", "input[action='confirmRow']", function () {
            List.SetConfirmation(this);
        });
        $("#cbtnDashBoard").click(function (e) {
            if (List.ConfirmationSaving) e.preventDefault();
        });
        $(window).on("beforeunload.eduConfirmation", function () {
            if (List.ConfirmationSaving) return "확인여부를 저장하고 있습니다. 잠시 후 이동해 주세요.";
        });

        List.InitializeMonthPicker();

        __addEventListener(document, "click", List.click);
        $("#zeoModalContact").draggable({ disabled: false });

        if (List.SettingSearch()) {
            List.LoadData("");
        }
        else {
            $("#phList").text("검색조건을 확인한 후 검색해 주세요.");
        }
    });
});

/**********************************************************************************************/
List.ResetSearch = function () {
    if (List.Loading || List.ConfirmationSaving) return;
    List.CloseMonthPicker(false);
    $('#startDate').val(DefaultSearchStartMonth);
    $('#endDate').val(DefaultSearchMonth);
    $('#ddlItem, #ddlEmployment, #ddlComplete, #ddlStatus, #ddlConfirm').val('');
    $('#ddlSearchUserField').val('Name');
    $('#ctbSearchUser, #ctbSearchTitle').val('');
    $('#ddlSearchTarget').val('A');
    List.Targets = [];
    List.CourseFilter = { id: 0, title: "" };
    List.DrawTargets();
    List.Search();
};

List.Search = function () {

    if (List.Loading || List.ConfirmationSaving) return;
    List.CloseMonthPicker(false);

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    List.LoadData("search");
};

/**********************************************************************************************/
List.LoadData = function (gubun) {

    if (!List.AppliedSearch || List.Loading || List.ConfirmationSaving) return;
    List.CloseMonthPicker(false);
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    // 첫 목록 및 페이지 이동 시 건수와 목록을 가져온다.
    var loadEDUData = function () {
        List.GetCount();
        // 공통 AjaxService는 동기 호출이므로 건수 조회 성공 후 목록을 조회한다.
        if (List.CountLoaded) List.GetPageList();
    };

    // 검색 시 페이지 카운트와 목록을 다시 가져온다.
    var loadEDUSearchData = function () {
        RecordPerPage = $("#ddlListCount").val();
        List.AppliedSearch.pageSize = RecordPerPage;
        List.GetCount();
        if (List.CountLoaded) List.GetPageList();
    };

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다.
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (List.Loading || List.ConfirmationSaving) return;
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    };

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    List.Loading = true;
    List.DataLoaded = false;
    List.CountLoaded = false;
    $("#loading").html(loadingPath);

    setTimeout(function () {
        try {
            if (gubun == "search") {
                loadEDUSearchData();
            }
            else {
                loadEDUData();
            }
        }
        finally {
            List.Loading = false;
            $("#loading").html("");

            if (!List.DataLoaded) {
                TotalItemCount = 0;
                CurrentPage = 1;
                $("#phList").text("목록을 불러오지 못했습니다. 검색을 다시 실행해 주세요.");
            }

            // 조회 결과에 맞춘 현재 페이지로 공통 페이저를 초기화한다.
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

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(pageCount ? CurrentPage + " / " + pageCount : "0 / 0");
        }
    }, 100);
};

/**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Education/Ws/Admin/List.asmx/GetPageList";

    // 파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchItem': SearchItem,
        'searchComplete': SearchComplete,
        'searchStatus': SearchStatus,
        'searchConfirm': SearchConfirm,
        'searchListID': SearchListID,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue,
        'searchTarget': SearchTarget,
        'targetIDs': SearchTargetIDs,
        'searchUserField': SearchUserField,
        'searchUserValue': SearchUserValue,
        'searchTitle': SearchTitle,
        'searchEmployment': SearchEmployment
    };

    var obj = List.SuccessPageList;
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};

/**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    if (!data || !$.isArray(data.d)) return;

    // 서버는 원문을 반환한다. 템플릿용 사본만 한 번 인코딩한다.
    var rows = List.TemplateRows(data.d);
    $("#phList").setTemplateURL("/DeskPlusEIP/Education/Jt/Admin/JTList.htm?seq=" + Guids(), null, { filter_data: false });
    $("#phList").processTemplate(rows);
    $("#loading").html("");

    List.DataLoaded = true;
};

/**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Education/Ws/Admin/List.asmx/GetListCount";
    ParamArray = {
        'searchItem': SearchItem,
        'searchComplete': SearchComplete,
        'searchStatus': SearchStatus,
        'searchConfirm': SearchConfirm,
        'searchListID': SearchListID,
        'searchDateField': SearchDateField,
        'searchDateValue': SearchDateValue,
        'searchTarget': SearchTarget,
        'targetIDs': SearchTargetIDs,
        'searchUserField': SearchUserField,
        'searchUserValue': SearchUserValue,
        'searchTitle': SearchTitle,
        'searchEmployment': SearchEmployment
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = List.SuccessCount;
    AjaxService(url, data, obj);
};

/**********************************************************************************************/
List.SuccessCount = function (data, status) {

    if (!data || data.d == null || data.d === "" || !isFinite(Number(data.d)) || Number(data.d) < 0) return;

    TotalItemCount = parseInt(data.d, 10);
    var totalPageCount = Math.ceil(TotalItemCount / RecordPerPage);
    CurrentPage = totalPageCount ? Math.max(1, Math.min(parseInt(CurrentPage, 10) || 1, totalPageCount)) : 1;
    List.CountLoaded = true;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(pageCount ? CurrentPage + " / " + pageCount : "0 / 0");
};

/**********************************************************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = $(evt.target).closest("[action]");
    var action = el.attr("action");
    if ((List.Loading || List.ConfirmationSaving) && action) {
        var blockedEvent = e || window.event;
        if (blockedEvent.preventDefault) blockedEvent.preventDefault();
        else blockedEvent.returnValue = false;
        return;
    }

    switch (action) {
        case "attach":
            List.ShowAttachContextMenu(e, false);
            break;
        case "certificate":
            List.ShowAttachContextMenu(e, true);
            break;
        case "accountInfo":
            var accountEvent = e || window.event;
            if (accountEvent.preventDefault) accountEvent.preventDefault();
            else accountEvent.returnValue = false;
            var accountUserID = List.PositiveNumber(el.attr("data-user-id"));
            if (accountUserID) AccountInfo(accountUserID);
            break;
        case "view":
            var nativeEvent = e || window.event;
            if (nativeEvent.preventDefault) {
                nativeEvent.preventDefault();
            }
            else {
                nativeEvent.returnValue = false;
            }
            var viewRow = el.closest("[ListID]");
            List.View(List.PositiveNumber(viewRow.attr("ListID")), List.PositiveNumber(viewRow.attr("UserID")));
            break;
        case "confirmRow":
            // 확인 저장은 키보드 조작도 처리하는 change 이벤트에서만 실행한다.
            break;
        default:
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e);
            break;
    }
};

/**********************************************************************************************/
List.ShowAttachContextMenu = function (e, isCertificate) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    oContextMenuCtrl = null;

    var evt = new __event(e);
    var row = $(evt.target).closest("[ListID]");
    var listID = List.PositiveNumber(row.attr("ListID"));
    var userID = List.PositiveNumber(row.attr("UserID"));
    if (!listID || !userID) return;

    var url = isCertificate ? "/DeskPlusEIP/Education/Ws/Admin/List.asmx/GetCertificateFileList"
        : "/DeskPlusEIP/Education/Ws/Admin/View.asmx/GetAttachFileList";
    ParamArray = {
        'listID': listID
    };
    if (isCertificate) ParamArray.userID = userID;

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var files = null;
    AjaxService(url, data, function (response) {
        if (response && $.isArray(response.d)) files = response.d;
    });

    if (files == null) return;
    if (!files.length) {
        alert(isCertificate ? "등록된 첨부파일이 없습니다." : "공지첨부이 없습니다.");
        return;
    }

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var count = 0;
    $.each(files, function (_, file) {
        if (!file) return;
        var downloadUrl = isCertificate ? file.DownloadUrl
            : (file.FilePath ? "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + encodeURIComponent(file.FilePath) : "");
        if (!downloadUrl) return;

        var item = oContextMenuCtrl.addItem("", List.DownLoad, "");
        $(item.txt).text(file.FileName || "");
        item.txt.setAttribute("arg", downloadUrl);
        count++;
    });

    if (count) oContextMenuCtrl.show(e);
};

/**********************************************************************************************/
List.DownLoad = function (e) {
    var evt = new __event(e);
    var downloadUrl = $(evt.target).closest("[arg]").attr("arg");
    if (downloadUrl) window.open(downloadUrl);
};

/**********************************************************************************************/
List.SettingSearch = function () {

    var period = List.MonthDateRange($.trim($("#startDate").val()), $.trim($("#endDate").val()));
    if (!period) {
        alert("시작 월과 종료 월을 올바르게 선택해 주세요.");
        return false;
    }

    var status = $("#ddlStatus").val();
    if (status !== "" && status !== "1" && status !== "2" && status !== "3" && status !== "4") {
        alert("진행상태를 올바르게 선택해 주세요.");
        return false;
    }

    var confirm = $("#ddlConfirm").val();
    if (confirm !== "" && confirm !== "Y" && confirm !== "N") {
        alert("확인여부를 올바르게 선택해 주세요.");
        return false;
    }

    var target = $("#ddlSearchTarget").val();
    var ids = [], names = [];
    $.each(List.Targets, function (_, item) {
        ids.push(item.id);
        names.push(item.name);
    });

    if (target != "A" && !ids.length) {
        alert("검색할 사용자 또는 부서를 선택해 주세요.");
        return false;
    }
    if (target == "G" && ids.length != 1) {
        alert("부서는 한 개만 선택해 주세요.");
        return false;
    }

    var title = $.trim($("#ctbSearchTitle").val());
    // 과정명을 수정하거나 지우면 특정 내역고정을 해제하고 입력한 과정명으로 검색합니다.
    if (List.CourseFilter.id && title != $.trim(List.CourseFilter.title))
        List.CourseFilter = { id: 0, title: "" };

    SearchItem = $("#ddlItem").val() || "";
    SearchComplete = $("#ddlComplete").val() || "";
    SearchStatus = status;
    SearchConfirm = confirm;
    SearchListID = List.PositiveNumber(List.CourseFilter.id);
    SearchDateField = "StartDate";
    SearchDateValue = period.startDate + "," + period.endDate;
    SearchTarget = target;
    SearchTargetIDs = target == "A" ? "" : ids.join(",");
    SearchUserField = $("#ddlSearchUserField").val();
    SearchUserValue = $.trim($("#ctbSearchUser").val());
    SearchTitle = title;
    SearchEmployment = $("#ddlEmployment").val() || "";

    // 적용한 검색조건을 보관하여 검색 전 입력값 수정이 페이지 이동/엑셀에 반영되지 않게 한다.
    List.AppliedSearch = {
        searchItem: SearchItem,
        searchComplete: SearchComplete,
        searchStatus: SearchStatus,
        searchConfirm: SearchConfirm,
        searchListID: SearchListID,
        searchDateField: SearchDateField,
        startDate: period.startDate,
        endDate: period.endDate,
        searchTarget: SearchTarget,
        targetIDs: SearchTargetIDs,
        targetNames: SearchTarget == "A" ? "" : names.join(", "),
        searchUserField: SearchUserField,
        searchUserValue: SearchUserValue,
        searchTitle: SearchTitle,
        searchEmployment: SearchEmployment,
        pageSize: RecordPerPage
    };

    return true;
};

/**********************************************************************************************/
List.View = function (_listID, _userID) {

    if (List.Loading || List.ConfirmationSaving || !List.PositiveNumber(_listID) || !List.PositiveNumber(_userID) || !List.AppliedSearch) return;

    ParamArray = {
        'listID': _listID,
        'userID': _userID,
        'returnPage': "List",
        'dashBoardSearch': List.DashBoardSearch,
        'currentPage': CurrentPage,
        'pageSize': RecordPerPage,
        'searchItem': SearchItem,
        'searchComplete': SearchComplete,
        'searchStatus': SearchStatus,
        'searchConfirm': SearchConfirm,
        'searchListID': SearchListID,
        'searchDateField': SearchDateField,
        'startDate': List.AppliedSearch.startDate,
        'endDate': List.AppliedSearch.endDate,
        'searchTarget': SearchTarget,
        'targetIDs': SearchTargetIDs,
        'targetNames': List.AppliedSearch.targetNames,
        'searchUserField': SearchUserField,
        'searchUserValue': SearchUserValue,
        'searchTitle': SearchTitle,
        'searchEmployment': SearchEmployment
    };

    CallEncrypt(CallGetParam());
    window.location.href = "/DeskPlusEIP/Education/Admin/View.aspx?p=" + encodeURIComponent(GetEncryptValue);
};

/**********************************************************************************************/
List.ModListUser = function () {

    if (List.Loading || List.ConfirmationSaving) {
        $("#ddlListCount").val(String(RecordPerPage));
        return;
    }

    var url = "/DeskPlusEIP/Education/Ws/Admin/List.asmx/ModListUser";
    ParamArray = {
        'isType': "AL",
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = List.SuccessModListUser;
    AjaxService(url, data, obj);

    // 페이지 크기 저장 결과와 관계없이 선택한 크기로 목록을 다시 조회한다.
    RecordPerPage = $("#ddlListCount").val();
    CurrentPage = 1;
    if (List.AppliedSearch) {
        List.AppliedSearch.pageSize = RecordPerPage;
        List.LoadData("search");
    }
};

/**********************************************************************************************/
List.SuccessModListUser = function (data, status) {
    // 목록 재조회는 ModListUser에서 처리한다.
};

/**********************************************************************************************/
List.ExcelDown = function () {

    if (List.ConfirmationSaving) return;
    if (List.Loading || !List.DataLoaded || !List.AppliedSearch || !TotalItemCount) {
        alert("다운로드할 내역이 없습니다.");
        return;
    }

    ParamArray = {
        'pageSize': RecordPerPage,
        'searchItem': SearchItem,
        'searchComplete': SearchComplete,
        'searchStatus': SearchStatus,
        'searchConfirm': SearchConfirm,
        'searchListID': SearchListID,
        'searchDateField': SearchDateField,
        'startDate': List.AppliedSearch.startDate,
        'endDate': List.AppliedSearch.endDate,
        'searchTarget': SearchTarget,
        'targetIDs': SearchTargetIDs,
        'targetNames': List.AppliedSearch.targetNames,
        'searchUserField': SearchUserField,
        'searchUserValue': SearchUserValue,
        'searchTitle': SearchTitle,
        'searchEmployment': SearchEmployment
    };

    CallEncrypt(CallGetParam());
    $("#exportParameters").val(GetEncryptValue);
    document.getElementById("educationExportForm").submit();
};

/**********************************************************************************************/
List.RestoreSearch = function (search) {
    List.Targets = [];
    List.DashBoardSearch = search.dashBoardSearch || "";
    List.CourseFilter = { id: List.PositiveNumber(search.searchListID), title: String(search.searchListTitle || search.searchTitle || "") };

    List.SetSelect("#ddlItem", search.searchItem || "", "");
    List.SetSelect("#ddlSearchTarget", search.searchTarget || "A", "A");
    List.SetSelect("#ddlSearchUserField", search.searchUserField || "Name", "Name");
    List.SetSelect("#ddlEmployment", search.searchEmployment == null ? "" : String(search.searchEmployment), "");
    List.SetSelect("#ddlComplete", search.searchComplete == null ? "" : String(search.searchComplete), "");
    List.SetSelect("#ddlStatus", search.searchStatus == null ? "" : String(search.searchStatus), "");
    List.SetSelect("#ddlConfirm", search.searchConfirm == null ? "" : String(search.searchConfirm), "");
    List.SetSelect("#ddlListCount", String(search.pageSize || RecordPerPage), String(RecordPerPage));

    RecordPerPage = $("#ddlListCount").val();
    CurrentPage = List.PositiveNumber(search.currentPage || "1") || 1;
    var hasPeriod = $.trim(search.startDate || "") != "" || $.trim(search.endDate || "") != "";
    $("#startDate").val(hasPeriod ? List.SearchMonth(search.startDate) : DefaultSearchStartMonth);
    $("#endDate").val(hasPeriod ? List.SearchMonth(search.endDate) : DefaultSearchMonth);
    $("#ctbSearchUser").val(search.searchUserValue || "");
    $("#ctbSearchTitle").val(List.CourseFilter.id ? List.CourseFilter.title : search.searchTitle || "");

    var seen = {};
    if ($("#ddlSearchTarget").val() != "A"
        && ($("#ddlSearchTarget").val() != "G" || String(search.targetIDs || "").split(",").length == 1)) {
        $.each(String(search.targetIDs || "").split(","), function (_, value) {
            var id = List.PositiveNumber(value);
            if (id && !seen[id] && List.Targets.length < 1000) {
                seen[id] = true;
                List.Targets.push({ id: id, name: search.targetNames || ("선택 ID " + id) });
            }
        });
    }

    List.DrawTargets();
};

/**********************************************************************************************/
List.SetSelect = function (id, value, defaultValue) {
    var control = $(id);
    control.val(value);
    if (control.val() == null) control.val(defaultValue);
};

/**********************************************************************************************/
List.DrawTargets = function () {

    var targetType = $("#ddlSearchTarget").val();
    var all = targetType == "A";
    var names = [];

    $("#cbtnSelectTargets")
        .toggleClass("edu-disabled", all)
        .attr("aria-disabled", all ? "true" : "false");

    if (all) List.Targets = [];
    $.each(List.Targets, function (_, target) {
        names.push(target.name);
    });

    $("#targetNames").val(names.join(", "))
        .attr("placeholder", targetType == "G" ? "부서를 선택해 주세요." : "사용자를 선택해 주세요.")
        .attr("title", names.join(", "))
        .toggle(!all);
};

/**********************************************************************************************/
List.OpenTargets = function () {

    if ($("#ddlSearchTarget").val() == "A") return;

    ParamArray = {
        'callBackEvent': "List.SetTargets",
        'targetType': $("#ddlSearchTarget").val(),
        'singleDepartment': $("#ddlSearchTarget").val() == "G" ? "1" : "0"
    };

    CallEncrypt(CallGetParam());
    PopupWin(800, 485, "GetUserDept.aspx?p=" + encodeURIComponent(GetEncryptValue), "-500", "", true);
};

/**********************************************************************************************/
List.SetTargets = function (value) {

    if (!value || value == "undefined") return false;

    var targetType = $("#ddlSearchTarget").val();
    var selected = [];
    var seen = {};
    var invalid = false;

    // 조직도 원문에 unescape를 적용하지 않아 이름에 포함된 % 문자도 그대로 표시한다.
    $.each(String(value).split(";"), function (_, entry) {
        if (!entry) return;
        var parts = entry.split("$");
        var id = List.PositiveNumber(parts[1] || "");
        if (parts.length < 4 || parts[0] != targetType || !id) {
            invalid = true;
            return;
        }

        if (!seen[id]) {
            seen[id] = true;
            selected.push({ id: id, name: parts.slice(3).join("$") || ("선택 ID " + id) });
        }
    });

    if (invalid) {
        alert(targetType == "G" ? "부서만 선택해 주세요. 사용자는 선택 목록에서 삭제해 주세요." : "사용자만 선택해 주세요. 부서는 선택 목록에서 삭제해 주세요.");
        return false;
    }
    if (targetType == "G" && selected.length != 1) {
        alert("부서는 한 개만 선택해 주세요.");
        return false;
    }
    if (!selected.length || selected.length > 1000) {
        alert("대상을 1개 이상, 1,000개 이하로 선택해 주세요.");
        return false;
    }

    List.Targets = selected;
    List.DrawTargets();
    return true;
};

/**********************************************************************************************/
List.SetConfirmationBusy = function (saving) {
    $("#phList").attr("aria-busy", saving ? "true" : "false");
    $("#phList input[action='confirmRow']").prop("disabled", saving);
};

/**********************************************************************************************/
List.SetConfirmation = function (checkbox) {

    var control = $(checkbox);
    var previous = control.attr("data-confirmed") === "Y";
    var desired = checkbox.checked;
    if (List.Loading || List.ConfirmationSaving) {
        checkbox.checked = previous;
        return;
    }
    if (desired === previous) return;

    var row = control.closest("[ListID]");
    var listID = List.PositiveNumber(row.attr("ListID"));
    var userID = List.PositiveNumber(row.attr("UserID"));
    if (!listID || !userID || !List.AppliedSearch) {
        checkbox.checked = previous;
        alert("확인할 대상자를 확인할 수 없습니다. 목록을 다시 조회해 주세요.");
        return;
    }

    List.ConfirmationSaving = true;
    List.SetConfirmationBusy(true);
    var finished = false;
    var fail = function () {
        if (finished) return;
        finished = true;
        checkbox.checked = previous;
        List.ConfirmationSaving = false;
        List.SetConfirmationBusy(false);
        alert("확인여부를 저장하지 못했습니다. 목록을 다시 조회한 후 확인해 주세요.");
    };

    try {
        ParamArray = { listID: listID, userID: userID, confirmYN: desired ? "Y" : "N" };
        CallEncrypt(CallGetParam());
        if (!GetEncryptValue) {
            fail();
            return;
        }
        // 공통 AjaxService는 오류 콜백을 지정할 수 없어 저장 실패 시 체크 복구를 직접 처리한다.
        $.ajax({
            type: "POST",
            url: "/DeskPlusEIP/Education/Ws/Admin/List.asmx/SetConfirmation",
            async: true,
            timeout: 30000,
            data: JSON.stringify({ p: GetEncryptValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var saved = response && response.d;
                if (!saved || List.PositiveNumber(saved.ListID) !== listID || List.PositiveNumber(saved.UserID) !== userID
                    || saved.IsConfirmed !== desired) {
                    fail();
                    return;
                }
                if (finished) return;
                finished = true;
                control.attr("data-confirmed", desired ? "Y" : "N");
                checkbox.checked = desired;
                List.ConfirmationSaving = false;
                List.SetConfirmationBusy(false);
                // 화면의 미적용 입력값이 아닌 적용된 검색조건으로 건수/목록을 다시 조회한다.
                // 확인/미확인 검색 중 행이 빠지면 기존 건수 조회 로직에서 페이지도 보정한다.
                List.LoadData("");
            },
            error: fail
        });
    }
    catch (error) {
        fail();
    }
};

/**********************************************************************************************/
List.TemplateRows = function (data) {

    var rows = [];
    var textFields = [
        "ItemName", "WorkplaceName", "DeptName", "LogInID", "Name", "Title", "DateStr", "EmploymentStr", "StatusStr",
        "CompleteYN", "CompleteUserName", "CompleteDateStr", "WriteDateStr", "WriteUserName", "ConfirmUserName", "ConfirmDateStr"
    ];

    $.each(data, function (_, source) {
        if (!source) return;
        var listID = List.PositiveNumber(source.ListID);
        var userID = List.PositiveNumber(source.UserID);
        if (!listID || !userID) return;

        var row = {
            ListID: listID,
            UserID: userID,
            CompleteUserID: source.IsComplete === true ? List.PositiveNumber(source.CompleteUserID) : 0,
            WriteUserID: List.PositiveNumber(source.WriteUserID),
            IsComplete: source.IsComplete === true,
            IsConfirmed: source.IsConfirmed === true,
            ConfirmUserID: source.IsConfirmed === true ? List.PositiveNumber(source.ConfirmUserID) : 0,
            FileCount: List.PositiveNumber(source.FileCount),
            CertificateCount: List.PositiveNumber(source.CertificateCount)
        };

        $.each(textFields, function (_, name) {
            row[name] = List.EncodeHtml(source[name]);
        });
        row.HasUserInfo = $.trim(String(source.Name || "")) != "";
        row.HasCompleteUserInfo = row.CompleteUserID > 0 && $.trim(String(source.CompleteUserName || "")) != "";
        row.HasWriteUserInfo = row.WriteUserID > 0 && $.trim(String(source.WriteUserName || "")) != "";
        row.HasConfirmUserInfo = row.ConfirmUserID > 0 && $.trim(String(source.ConfirmUserName || "")) != "";
        if (!row.HasCompleteUserInfo) row.CompleteUserName = "";
        if (!row.HasConfirmUserInfo) row.ConfirmUserName = "";
        if (!row.IsConfirmed) row.ConfirmDateStr = "";
        row.ConfirmationLabel = List.EncodeHtml((source.Name || "") + " " + (source.Title || "") + " 관리자 확인");
        rows.push(row);
    });

    return rows;
};

/**********************************************************************************************/
List.EncodeHtml = function (value) {
    return String(value == null ? "" : value).replace(/&/g, "&amp;").replace(/</g, "&lt;")
        .replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#39;");
};

/**********************************************************************************************/
List.PositiveNumber = function (value) {
    if (!/^[0-9]+$/.test(String(value))) return 0;
    var number = Number(value);
    return number > 0 && number <= 2147483647 ? number : 0;
};

/**********************************************************************************************/
List.LocalDateNumber = function (value) {

    if (!value) return 0;

    var match = /^(\d{4})[-\/.](\d{1,2})[-\/.](\d{1,2})$/.exec(value);
    var year;
    var month;
    var day;
    if (match) {
        year = +match[1];
        month = +match[2];
        day = +match[3];
    }
    else {
        match = /^(\d{1,2})[-\/.](\d{1,2})[-\/.](\d{4})$/.exec(value);
        if (!match) return 0;

        var language = String(PreLangReal || PreLang || "").toLowerCase();
        var monthFirst = language == "en" || language == "en-us";
        year = +match[3];
        month = +(monthFirst ? match[1] : match[2]);
        day = +(monthFirst ? match[2] : match[1]);
    }

    var date = new Date(year, month - 1, day);
    return year >= 1900 && year <= 9999 && date.getFullYear() == year && date.getMonth() == month - 1 && date.getDate() == day
        ? year * 10000 + month * 100 + day : 0;
};

/**********************************************************************************************/
List.MonthNumber = function (value) {
    var match = /^(\d{4})-(\d{2})$/.exec(String(value));
    if (!match || +match[1] < 1900 || +match[1] > 9999 || +match[2] < 1 || +match[2] > 12) return 0;
    return +match[1] * 100 + +match[2];
};

List.SearchMonth = function (value) {
    value = $.trim(value || "");
    if (List.MonthNumber(value)) return value;
    // 보기 화면의 기존 복귀 주소는 일 단위 날짜를 전달할 수 있으므로 월로 복원합니다.
    var dateNumber = List.LocalDateNumber(value);
    if (!dateNumber) return value;
    var monthNumber = Math.floor(dateNumber / 100);
    return Math.floor(monthNumber / 100) + "-" + (monthNumber % 100 < 10 ? "0" : "") + monthNumber % 100;
};

List.MonthDateRange = function (start, end) {
    var startNumber = List.MonthNumber(start), endNumber = List.MonthNumber(end);
    if (!startNumber || !endNumber || startNumber > endNumber) return null;
    var lastDay = new Date(Math.floor(endNumber / 100), endNumber % 100, 0).getDate();
    // 목록/건수/엑셀의 기존 날짜 검색 규약은 유지하고 월의 첫날과 말일을 전달합니다.
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
    // 전체현황과 동일하게 현재 월에 따른 선택 제한은 두지 않습니다.
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
