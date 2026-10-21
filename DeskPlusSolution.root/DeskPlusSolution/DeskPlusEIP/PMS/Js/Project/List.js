List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //관리자도구
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            List.Admin();
        });

        //폴더관리
        $('#cbtnFolder').click(function (e) {
            e.preventDefault();
            List.Folder();
        });

        //글올리기
        $('#cbtnWrite').click(function (e) {
            e.preventDefault();
            List.Register();
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

/**************************************************
작성자   :
작성일자 :
작성목적 : [관리자]버튼
***************************************************/
List.Admin = function () {
    var url = "/DeskPlusEIP/PMS/Admin/Default.aspx";
    parent.location.href = url;
}

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
[프로젝트 추가]
**********************************************************************************************/
List.Register = function () {
    ParamArray = {
        'folderID': FolderID
    };

    CallEncrypt(CallGetParam());
    self.location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[폴더 추가]
**********************************************************************************************/
List.Folder = function () {
    //url = "Admin/Folder/Add.aspx";
    //window.open(url, "FolderRegister", "toolbar=no,width=200,height=300,directories=no,status=no,scrollbars=no,resize=no,menubar=no");

    var url = "../Admin/Folder/Add.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Register", "500", "400");
}

List.Opinion = function (projectID) {
    ParamArray = {
        'projectID': projectID
    };

    CallEncrypt(CallGetParam());

    var url = "Opinion.aspx?p=" + GetEncryptValue;

    PopupWin(400, 200, url, "50", "", true);
}