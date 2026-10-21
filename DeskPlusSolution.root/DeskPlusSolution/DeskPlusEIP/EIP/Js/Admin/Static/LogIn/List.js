List = {}

$(function () {
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", 
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

        /*
        $("#ddlSearchTerm").hide();
        $("#startEndDate").hide();

        $('#ddlSearchField').change(function (e) {
            e.preventDefault();
            List.VisibleSearch();
        });

        $('#ddlSearchTerm').change(function (e) {
            e.preventDefault();
            List.RegDate();
        });
        */

        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            List.Excel();
        });

        $('#ddlListCount').change(function () {
            List.ModPageSize();
        });

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        List.LoadData("");
    });
});

/***************************************************
List.VisibleSearch = function () {

    var sField = $("#ddlSearchField").val();

    if (sField == "StartTimeYMD" || sField == "EndTimeYMD") {
        $("#ddlSearchTerm").show();
    }
    else {
        $("#ddlSearchTerm").hide();     
        $("#startEndDate").hide();
    }
}

/**************************************************
List.RegDate = function () {
    var sTerm = $("#ddlSearchTerm").val();

    if (sTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    }
    else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}
*/

/**************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);


    var loadBoardData = function () {
        List.GetPageList();
    }

    var loadBoardSearchData = function () {
        List.GetCount(); 
        List.GetPageList(); 

        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", 
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
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}

/***********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Static/LogIn/List.asmx/GetListCount";

    //SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************/
List.GetPageList = function () {    

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Static/LogIn/List.asmx/GetStaticLog";

    //SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/***********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#plShareUser').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/Static/LogIn/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plShareUser').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/***********************************************************************************************/
List.Resource = function () {

    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_title")[0].innerHTML = R_TitleName;
    $("#r_name")[0].innerHTML = R_UserName
    $("#r_ip")[0].innerHTML = R_IPAddress;
    $("#r_startDate")[0].innerHTML = R_StartDate;
    $("#r_endDate")[0].innerHTML = R_EndDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/*********************************************************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  

    List.LoadData("search");
}

/**************************************************/
List.SettingSearch = function () {

    //SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    //if (SearchTerm == "period") {
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
    //}

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/****************************************************/
List.ModPageSize = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    List.Search();

}


/**********************************************************************************************/
List.Excel = function () {
    //SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "ExcelDown.aspx?p=" + GetEncryptValue;
}