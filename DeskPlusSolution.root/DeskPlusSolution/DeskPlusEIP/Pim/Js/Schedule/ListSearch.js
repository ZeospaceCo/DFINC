ListSearch = {}

$(function () {
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListSearch.SettingPara();

        }
    }

    $(document).ready(function () {

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

        //공유선택        
        $('#ddlSearchKind').change(function (e) {
            e.preventDefault();
            ListSearch.SearchComboSelect($('#ddlSearchKind').val());

        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {

                e.preventDefault();
                ListSearch.SettingPara();
            }
        });

        //엑셀다운로드
        $('#CbtnExcel').click(function (e) {
            e.preventDefault();
            ListSearch.ExcelDown();
        });

        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {

                e.preventDefault();
                ListSearch.SettingPara();
            }
        });

        //나의일정
        $('#CbtnSchedule').click(function (e) {
            e.preventDefault();
            ListSearch.MoveSchedulePage();
        });


        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        $("#startDate").val(StartDate);
        $("#endDate").val(EndDate);

        if (DatePeriod == "U")
            $("#spanDate").show();

        //ListSearch.GetShareList();
        //ListSearch.GetSearchList();

        $('#selPageDistance').change(function (e) {
            e.preventDefault();
            ListSearch.SettingPara();
        });

        ListSearch.SettingPara();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


ListSearch.ExcelDown = function () {

    ParamArray = {
        'StartDate': StartDate,
        'EndDate': EndDate,
        'SearchValue': SerachValue,
        'SearchKind': $("#ddlValue").val()
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    //var url = "ListExcel.aspx";
    //var param = "p=" + GetEncryptValue;
    //WindowObjectReference = PopupWinOpen(url, param, "Repeat", "300", "300")

    $("#hdexcel").val(GetEncryptValue);
    __doPostBack('lbtnexcel', "");
}


ListSearch.MoveSchedulePage = function () {
    
    document.location.href = "ScheduleMain.aspx";
}


ListSearch.SearchComboSelect = function (arg) {
    if (arg == "U") {
        $("#spanDate").show();
    }
    else {
        $("#spanDate").hide();
    }

    $("#startDate").val("");
    $("#endDate").val("");
}



/******************************
작성자   : 김광일
작성일자 :  2011-11-02
작성목적 : 검색 
*******************************/
ListSearch.GetSearchList = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListSearch.asmx/GetALLList";
    ParamArray = {        
        'StartDate': StartDate,
        'EndDate': EndDate,
        'PageSize': PageSize,
        'PageSizePageNo': PageSizePageNo,
        'SearchValue': SerachValue,
        'SearchKind': $("#ddlValue").val()
    };
    

    var obj = eval("ListSearch.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/******************************
작성자   : 김광일
작성일자 :  2011-11-02
작성목적 : 검색 
*******************************/
ListSearch.SuccessList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Schedule/JTListSearch.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListSearch.Resource();
}


ListSearch.Resource = function () {
    $("#r_name")[0].innerHTML = col1;
    $("#r_kind")[0].innerHTML = col2;
    $("#r_date")[0].innerHTML = col3;
    $("#r_title")[0].innerHTML = "" + col4 + "";
    $("#r_time")[0].innerHTML = col5;    
    $("#divt1").text(TextEmpty);
}

/*웹서비스*/
ListSearch.GetShareList = function () {
    
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListSearch.asmx/GetOpenUserComboList";
    ParamArray = {
        'companyID': CompanyID,
        'UserId': UserId
    };
    
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListSearch.SuccessShareList");

    AjaxService(url, data, obj);
}

ListSearch.SuccessShareList = function (data, status) {
    

    $("select[id='ddlPerson']").find('option').each(function () {
        $(this).remove();

    });

    $("#ddlPerson").get(0).options[0] = new Option(myperson, "P");
    //$("#ddlPerson").get(0).options[1] = new Option(AllSchedule, "M");

    $.each(data.d, function (index, list) {
        
        $("#ddlPerson").get(0).options[index + 1] = new Option(list.Name, list.UserID);
    });

    $("#ddlPerson").val(inquryPerson);
}


ListSearch.SettingPara = function () {
    DatePeriod = $("#ddlSearchKind").val();

    switch (DatePeriod) {
        case "0":
            StartDate = "0";
            EndDate = "";
            break;
        case "WEEK":
            StartDate = "W";
            EndDate = "";
            break;
        case "MON":
            StartDate = "M";
            EndDate = "";
            break;
        case "U":
            StartDate = $("#startDate").val();
            EndDate = $("#endDate").val();
            if (StartDate.length == 0) {
                alert(InputSearchDay);        //
                return;
            }
            if (EndDate.length == 0) {
                alert(InputSearchDay);    //검색일을 입력하세요
                return;
            }

            var dtResponse = CompareDT(StartDate, EndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return;
            }
            break;
    }

    SerachValue = $("#ctbSearch").val();
    PageSize = $("#selPageDistance").val();
    PageSizePageNo = CurrentPage;
    RecordPerPage = $("#selPageDistance").val();

    

    ListSearch.GetSearchListCnt();

    ListSearch.GetSearchList();

}


/******************************
작성자   : 김광일
작성일자 :  2011-11-02
작성목적 : 카운트 
*******************************/
ListSearch.GetSearchListCnt = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListSearch.asmx/GetALLListCnt";
    ParamArray = {
        'StartDate': StartDate,
        'EndDate': EndDate,
        'PageSize': PageSize,
        'PageSizePageNo': PageSizePageNo,
        'SearchValue': SerachValue,
        'SearchKind': $("#ddlValue").val()
    };

    var obj = eval("ListSearch.SuccessListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/******************************
작성자   : 김광일
작성일자 :  2011-11-02
작성목적 : 검색 
*******************************/
ListSearch.SuccessListCnt = function (data, status) {
  
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

}


ListSearch.SearchButton = function () {
    CurrentPage = 1;

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListSearch.SettingPara();
        }
    }
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

    ListSearch.SettingPara();
}


ListSearch.MovePage = function (arg, kind) {
     
    var url = "";
    if (kind == 8) {
        url = "/DeskPlusEIP/Pim/Todo/View.aspx?p=";
        ParamArray = {
            's_Distance': inquryPerson,
            's_inquryPerson': inquryPerson,
            's_DatePeriod': DatePeriod,
            's_SearchKind': SearchKind,
            's_SerachValue': SerachValue,
            's_StartDate': StartDate,
            's_EndDate': EndDate,            
            's_PageSize': PageSize,
            's_CurrentPage': CurrentPage,
            'Page': 'ListSearch.aspx',
            'TodoID': arg
        };
    }
    else {
        url = "/DeskPlusEIP/Pim/Schedule/View.aspx?p=";
        ParamArray = {
            's_Distance': inquryPerson,
            's_inquryPerson': inquryPerson,
            's_DatePeriod': DatePeriod,
            's_SearchKind': SearchKind,
            's_SerachValue': SerachValue,
            's_StartDate': StartDate,
            's_EndDate': EndDate,
            's_UserId': '',
            's_PageSize': PageSize,
            's_CurrentPage': CurrentPage,
            'Page': 'ListSearch.aspx',
            'DisplayID': arg
        };
    }
    CallEncrypt(CallGetParam()); 
    document.location.href=url+GetEncryptValue;
}

 