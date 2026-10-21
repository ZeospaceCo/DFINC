List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add()
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del()
        });

        //검색
        $('#cbtnFindit').click(function (e) {
            e.preventDefault();
            List.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#tbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //검색어입력
        $("#tbSearch").attr("placeholder", R_PlaceholderText);

        Init();
    });
});

/**********************************************************************************************
[검색]
**********************************************************************************************/
List.Search = function () {
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
List.checkMods = function (evt, theForm) {
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

/**********************************************************************************************
[검색필드 선택]
**********************************************************************************************/
function ddlFieldSearch_OnChange() {
    $("#tbSearch").val("");

    if ($("#ddlFieldSearch").val() != "RegisterDate") {
        $("#plDate").css("display", "none");
        $("#tbSearch").attr("readonly", false);
    }
    else {
        $("#plDate").css("display", "");
        $("#tbSearch").attr("readonly", true);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
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
[추가]
**********************************************************************************************/
List.Add = function () {
    ParamArray = {
        'gubun': "2"
    };

    CallEncrypt(CallGetParam());

    dialogURL = "../../Module/Popup/GetDept.aspx?p=" + GetEncryptValue;

    PopupWin(350, 530, url, "", "", true);
    /*
    if ($.browser.msie) {
        var rtnVal = window.showModalDialog(dialogURL, window, "dialogWidth:350px; dialogHeight:530px; status: off; center: Yes; help: No; resizable: No; status: No; scroll:No");
        GetDept(rtnVal);
    } else {
        PopupWin(350, 530, url, "", "", true);
    }
    */
}

function GetDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡");

        $("#tbxHiddenValue").val(responseStr[0]);

        if ($("#tbxHiddenValue").val() != "") {
            __doPostBack('lbtnWrite', '');
        }
    }
}

/**********************************************************************************************
[삭제]
**********************************************************************************************/
List.Del = function () {
    var selectedItems = SelectedItems();
    var rowSplit = selectedItems.split("");

    if (rowSplit.length > 1) {
        if (window.confirm(Member_List_ChargerDel)) {
            __doPostBack('lbtnDel', '');
        }
    }
    else {
        alert(Member_List_ChargerSelectDel);
    }
}