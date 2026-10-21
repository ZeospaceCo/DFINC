List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData();
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
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Init();
    });
});

/**********************************************************************************************
[초기화]
**********************************************************************************************/
List.Init = function () {
    RecordPerPage = $('#ddlListCount').val();
    List.LoadData();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**********************************************************************************************
[검색]
**********************************************************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    List.LoadData();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData();
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

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardSearchData();
    }, 100);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Related/JList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': $("#ddlSearchField").val(),
        'searchValue': $("#ctbSearch").val(),
        'order': Order,
        'projectID': ProjectID
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/PMS/Jt/Related/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {
    $("#r_header0")[0].innerHTML = R_List_ProjectName;
    $("#r_header1")[0].innerHTML = R_ProjectList_Header1;
    $("#r_header2")[0].innerHTML = R_ProjectList_Header2;
    $("#r_header3")[0].innerHTML = R_ProjectList_Header3;
    $("#r_header4")[0].innerHTML = R_ProjectList_Header4;
    $("#r_header5")[0].innerHTML = R_ProjectList_Header5;
    $("#r_header6")[0].innerHTML = R_ProjectList_Header6;
    $("#r_header7")[0].innerHTML = R_ProjectList_Header7;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Related/JList.asmx/GetListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': $("#ddlSearchField").val(),
        'searchValue': $("#ctbSearch").val(),
        'order': Order,
        'projectID': ProjectID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        //사용자만 가져온다.
        if ($(this).val().indexOf("_2") != -1) {
            return $(this).val();
        }
    }).get();

    return allVals;
}

/**************************************************
작성자   :
작성일자 : 심재운
작성목적 : 사용자 추가
***************************************************/
List.Add = function () {
    ParamArray = {
        'projectID': ProjectID
    };

    CallEncrypt(CallGetParam());
    var url = "ProjectList.aspx?p=" + GetEncryptValue;
    PopupWin(1000, 500, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
List.Success = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        List.Search();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 프로젝트 보기
**********************************************************************************************/
List.View = function (p_linkProjectID) {
    ParamArray = {
        'companyID': CompanyID,
        'projectID': p_linkProjectID,
        'rqProjectID': ProjectID
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    var url = "../Project/Default.aspx?" + param;

    //var param = "companyID=" + CompanyID + "&projectID=" + p_linkProjectID + "&rqProjectID=" + ProjectID;

    window.open(url, "openWin", "width=1024px,height=500px,scrollbars=yes,status=no,toolbar=no,resizable=1,location=no,menu=no");
}