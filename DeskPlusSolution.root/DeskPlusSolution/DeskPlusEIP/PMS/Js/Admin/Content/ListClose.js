ListClose = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //거부
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            ListClose.Reject();
        });

        //승인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ListClose.Approval();
        });

        //검색
        $('#cbtnFindit').click(function (e) {
            e.preventDefault();
            ListClose.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#tbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListClose.Search();
            }
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListClose.RegDate();
        });

        //검색어입력
        $("#tbSearch").attr("placeholder", R_PlaceholderText);

        Init();
    });
});

/**********************************************************************************************
[검색]
**********************************************************************************************/
ListClose.Search = function () {
    SearchTerm = $("#ddlSearchTerm").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
        $("#tbSearchTermValue").val(SearchTermValue);
    }

    var searchValue = $("#tbSearch").val().securityText();

    $("#tbSearch").val(searchValue);
    __doPostBack('lbtnSearch', '');
}

/**********************************************************************************************
[엔터]
**********************************************************************************************/
ListClose.checkMods = function (evt, theForm) {
    if (evt.keyCode == 13)
        return false;
}

/**********************************************************************************************
[초기화]
**********************************************************************************************/
function Init() {
    $("#startEndDate").hide(); //기간

    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListClose.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**********************************************************************************************
[승인]
**********************************************************************************************/
ListClose.Approval = function () {
    var selectedItems = SelectedItems();
    var rowSplit = selectedItems.split("");

    if (rowSplit.length > 1) {
        if (window.confirm(Admin_ListClose_IsReceipt)) {
            __doPostBack('lbtnApproval', '');
        }
    }
    else {
        alert(R_ConfirmProject);
        return;
    }
}

/**********************************************************************************************
[거절]
**********************************************************************************************/
ListClose.Reject = function () {
    var selectedItems = SelectedItems();
    var rowSplit = selectedItems.split("");

    if (rowSplit.length > 1) {
        if (window.confirm(Admin_ListClose_IsReject)) {
            __doPostBack('lbtnReject', '');
        }
    }
    else {
        alert(R_RejectProject);
        return;
    }
}