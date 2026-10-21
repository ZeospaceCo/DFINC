List = {}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.Search();
        });

        //작성하기
        $('#cbtnAdd').click(function (e) {
            List.Add();
        });

        List.LoadData("");
    });
});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {
    CurrentPage = 1; //1페이지로 이동처리
    List.LoadData("search");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 작성하기
***************************************************/
List.Add = function () {
    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    //$('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 웹서비스  카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/PopUp/List.asmx/GetListCount";

    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/PopUp/List.asmx/GetPageList";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/ETC/Jt/PopUp/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    //$('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {

    $("#r_title")[0].innerHTML = R_Title;
    $("#r_register")[0].innerHTML = R_Register;
    $("#r_startdate")[0].innerHTML = R_Startdate;
    $("#r_enddate")[0].innerHTML = R_Enddate;
    $("#r_preview")[0].innerHTML = R_Preview;    

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }    
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 미리보기
**********************************************************************************************/
List.View = function (p_managerID, p_hieght, p_width) {
    //window.open("Preview.aspx?popID=" + _popID);
    window.open("/DeskPlusEIP/Preview.aspx?managerID=" + p_managerID, "", "left=0, top=0, scrollbars=yes, width=" + p_width + "," + "height=" + p_hieght);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 수정하기
**********************************************************************************************/
List.Mod = function (p_managerID) {

    ParamArray = {
        'managerID': p_managerID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage
    };

    CallEncrypt(CallGetParam());
    location.href = "ModN.aspx?p=" + GetEncryptValue;
}