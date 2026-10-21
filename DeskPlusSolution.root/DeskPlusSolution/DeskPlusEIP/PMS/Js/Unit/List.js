List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //작성하기
        $('#cbtnWrite').click(function (e) {
            e.preventDefault();
            List.cbsWrite_OnClick();
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

        //날짜검색
        $("#cbtnOrder").click(function (e) {
            e.preventDefault();

            ParamArray = {
                'projectID': projectID
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/PMS/Unit/MoveUpDown.aspx?p=" + GetEncryptValue;
            PopupWin(400, 400, url, "", "", true);
        });

        //검색어입력
        $("#tbSearch").attr("placeholder", R_PlaceholderText);

        Init();
    });
});

/**********************************************************************************************
[작성페이지이동]
**********************************************************************************************/
List.cbsWrite_OnClick = function () {
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'pageListUrl': "List.aspx",
        'projectTemplateID': id,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'status': Status,
        'pageNo': pageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[검색]
**********************************************************************************************/
List.Search = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlFieldSearch").val();
    SearchValue = $("#tbSearch").val().securityText();
    $("#tbSearch").val(SearchValue);

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
[읽기페이지 이동하기]
**********************************************************************************************/
List.View = function (id) {
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'rqProjectID': rqProjectID,
        'unitID': id,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'status': Status,
        'pageNo': pageNo
    };

    CallEncrypt(CallGetParam());
    var url = "View.aspx?p=" + GetEncryptValue;
    self.location.href = url;
}