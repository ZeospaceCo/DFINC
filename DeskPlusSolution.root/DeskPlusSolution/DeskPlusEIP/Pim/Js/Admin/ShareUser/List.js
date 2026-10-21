List = {}

var WebServiceUrl = '/DeskPlusEIP/Pim/Ws/Admin/ShareUser/List.asmx/';




$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            PageSizePageNo = CurrentPage;
            List.LoadData();
        }
    }


    //검색입력필드 엔터 이벤트 기술
    $("#ctbSearch").keydown(function (e) {
        if (e.keyCode == 13) {

            e.preventDefault();
            List.Search();
        }
    });

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


        //검색 
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();

            List.Search();
        });

        //공유자추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //공유자 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        $('#ddlListCount').change(function (e) {
            e.preventDefault();
            List.Search();
        });

        ///나의일정
        $('#CbtnMySchedule').click(function (e) {
            e.preventDefault();
            parent.document.location.href = "/DeskPlusEIP/Pim/Default.aspx";
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.GetPageListCnt();
        List.LoadData();
    });
});



/**************************************************
작성자   : 
작성일자 :
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);
    
    //List.GetPageListCnt();
    List.GetPageList();
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Admin/ShareUser/List.asmx/GetAdminShareList";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'NAME': Name,
        'KIND': Kind,
        'PageSize': PageSize,
        'PageSizePageNo': PageSizePageNo
    };
    
    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

 

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#plShareUser').setTemplateURL('/DeskPlusEIP/Pim/Jt/Admin/ShareUser/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plShareUser').processTemplate(data.d);
    $('#loading').html("");
    List.Resource();
}


List.Resource = function () {

    $("#div1").text(col1);
    $("#div2").text(col2);
    $("#div3").text(col3);
    $("#div4").text(col4);
    $("#div5").text(TextEmpty);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 카운트
**********************************************************************************************/
List.GetPageListCnt = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Admin/ShareUser/List.asmx/GetAdminShareListCnt";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'NAME': Name,
        'KIND': Kind
    };

    var obj = eval("List.SuccessPageListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

List.SuccessPageListCnt = function (data, status) {
    TotalItemCount = data.d;
    RecordPerPage = PageSize;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);

    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}



/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 검색
**********************************************************************************************/
List.Search = function () {
    Name = $("#ctbSearch").val();
    PageSize = $("#ddlListCount").val();
    List.GetPageListCnt();
    List.LoadData();
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 공유자 추가
**********************************************************************************************/
List.Add = function () {
    SelectUser = "";
    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "Add.aspx?p=" + GetEncryptValue;
    PopupWin(800, 600, url, "", "", true);
    
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 공유자 삭제
**********************************************************************************************/
List.Del = function () {

}


List.SelectUserPop = function (arg) {
    SelectUser = arg;
    
    
    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "Add.aspx?p=" + GetEncryptValue;
    PopupWin(700, 560, url, "", "", true);
}