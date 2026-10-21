ListTotal = {}



/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;

            ListTotal.LoadData();
        }
    }


    $(document).ready(function () {
        
        
        //검색입력필드 엔터 이벤트 기술




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
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);



        //조회대상선택
        $('#CbtnTarget').click(function (e) {
            e.preventDefault();
            ListTotal.CallSearchObject();
            //
        });
        //엑셀로보기
        $('#CbtnExcel').click(function (e) {
            e.preventDefault();

        });
        //인쇄
        $('#CbtnPrin').click(function (e) {
            e.preventDefault();

        });

        //검색
        $('#btnsearch').click(function (e) {
            e.preventDefault();

            _Searchkey = "";
            _SearchValue = "";

            CurrentPage = 1;
            RecordPerPage =
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


            ListTotal.LoadData();
        });

        ///나의일정
        $('#CbtnMySchedule').click(function (e) {
            e.preventDefault();
            document.location.href = "ScheduleMain.aspx";
        });


        //공유선택        
        $('#ddlSearchKind').change(function (e) {
            e.preventDefault();
            ListTotal.SearchComboSelect($('#ddlSearchKind').val());


        });



        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);

        if (_Searchkey.length > 0) {

            OpenType = _Searchkey;
            $("#hdShareList").val(_SearchValue);
            var arrayid = _SearchValue.split(',');

            var strHtml = "";

            for (var i = 0; i < arrayid.length - 1; i++) {
                var arrayName = arrayid[i].split('$');

                if (arrayName[0].length > 0) {
                    strHtml += arrayName[2] + "&nbsp;<a href='#' onclick='javascript:ListTotal.ShareUserClick(this)' value='" + arrayid[i] + "' ><font color='red'>삭제</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                //strHtml += "<a href='#' onclick='javascript:Add.ShareUserClick(this)' value='" + arrayid[i] + "' >" + arrayName[2] + "</a><a href='#' >삭제</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ";

            }

            $("#divShare").html(strHtml);
            ListTotal.LoadData();
        }

    });
});


/*********************
작성자   : 김광일
작성일자 :  2011-9-21
작성목적 : 기간선택
*******************************/
ListTotal.SearchComboSelect = function (arg) {
    if (arg == "U") {
        $("#spanDate").show();
    }
    else {
        $("#spanDate").hide();
    }
}



/*********************
작성자   : 김광일
작성일자 :  2011-9-21
작성목적 : 조회대상선택
*******************************/
ListTotal.CallSearchObject = function () {
    
    var url = "/DeskPlusEIP/Pim/Common/MemberShareUser.aspx";

    PopupWin(800, 500, url, "-100", "100", true);
    //$("#zeoModalContact").draggable({ disabled: false });
}


/**********************************************************************************************
작성자   : 
작성일자 : 검색전 세팅
작성목적 : 
**********************************************************************************************/
ListTotal.LoadData = function () {
 
    StartDate = '';
    EndDate = '';
    //CurrentPage = '1';

    UserList = $("#hdShareList").val();
    PageSize = $('#selPageDistance').val();
    RecordPerPage = PageSize;

    if (UserList.length == 0) {
        alert(UserSelect); //UserSelect
        return;
    }

    if ($('#ddlSearchKind').val() == "U") {
        StartDate = $("#startDate").val();
        EndDate = $("#endDate").val();
    }
    else {
        StartDate = $('#ddlSearchKind').val();
    }


    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {

        ListTotal.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {

        ListTotal.GetPageListCnt(); //검색 총 카운트 가져오기
        ListTotal.GetPageList(); //검색 리스트 가져오기
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {

        loadBoardSearchData();
    }, 100);
}
  

/**********************************************************************************************
작성자   : DANIEL  
작성일자 : 2011년 8월 16일
작성목적 : 리스트 가져오기
**********************************************************************************************/
ListTotal.GetPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListTotal.asmx/GetUserScheduleList";
    
    ParamArray = {
        'UserID': UserID,
        'CompanyID': CompanyID,
        'OpenType': OpenType,
        'UserList': UserList,
        'PageSize': PageSize,
        'PageSizePageNo': CurrentPage,
        'StartDate': StartDate,
        'EndDate': EndDate,
        'title': title
    };


    var obj = eval("ListTotal.SuccessUserList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}



ListTotal.SuccessUserList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Schedule/JTListTotal.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    //alert(data.d);
    ListTotal.Resource();
}


ListTotal.Resource = function () {
    $("#r_name")[0].innerHTML = col1;
    $("#r_kind")[0].innerHTML = col2;
    $("#r_date")[0].innerHTML = col3;
    $("#r_title")[0].innerHTML = "" + col4 + "";
    $("#divt1").text(TextEmpty);

}


/**********************************************************************************************
작성자   : DANIEL  
작성일자 : 2011년 8월 16일
작성목적 : 갯수 가져오기
**********************************************************************************************/
ListTotal.GetPageListCnt = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListTotal.asmx/GetUserScheduleListCnt";

    ParamArray = {
        'UserID': UserID,
        'CompanyID': CompanyID,
        'OpenType': OpenType,
        'UserList': UserList,        
        'StartDate': StartDate,
        'EndDate': EndDate,
        'title': title
    };

    var obj = eval("ListTotal.SuccessListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}



ListTotal.SuccessListCnt = function (data, status) {

    TotalItemCount = data.d;
    
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

 


/*********************
작성자:김광일
작성일:2011년 9월 19일
용도: 공유리스트 관리
**********************/
ListTotal.ShareUserClick = function (obj) {

    var strtemp = $("#hdShareList").val().replace(obj.value + ",", '');

    var arrayid = strtemp.split(',');

    var strHtml = "";

    for (var i = 0; i < arrayid.length - 1; i++) {
        var arrayName = arrayid[i].split('$');

        if (arrayName[0].length > 0) {
            strHtml += arrayName[2] + "&nbsp;<a href='#' onclick='javascript:Add.ShareUserClick(this)' value='" + arrayid[i] + "' ><font color='red'>삭제</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
    }
    $("#hdShareList").val(strtemp);

    $("#divShare").html(strHtml);

}


//페이지이동
ListTotal.MovePage = function (arg, kind) {
    

    var url = "";
    if (kind == 8) {
        url = "/DeskPlusEIP/Pim/Todo/View.aspx?p=";
        ParamArray = {
            //'s_Distance': $("#divShare").html(),       
            's_SearchKind': OpenType, //opentype
            's_SerachValue': UserList, //userlist
            's_StartDate': StartDate,
            's_EndDate': EndDate,
            's_PageSize': PageSize,
            's_CurrentPage': CurrentPage,
            'Page': 'ListTotal.aspx',
            'TodoID': arg
        };
    }
    else {
        url = "/DeskPlusEIP/Pim/Schedule/View.aspx?p=";
        ParamArray = {
            //'s_Distance': $("#divShare").html(),
            's_SearchKind': OpenType, //opentype
            's_SerachValue': UserList, //userlist
            's_StartDate': StartDate,
            's_EndDate': EndDate,
            's_PageSize': PageSize,
            's_CurrentPage': CurrentPage,
            'Page': 'ListTotal.aspx',
            'ScheduleID': arg
        };
    }
    CallEncrypt(CallGetParam());
    document.location.href = url + GetEncryptValue;
}
