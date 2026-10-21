List = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 
**********************************************************************************************/

$(function () {

    $('#ddlListCount').val(RecordPerPage);

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

			CurrentPage = 1; //1페이지로 이동처리 
			if (CompanyID == "0") CompanyID = CurrentCompanyID;

            //List.Search();
			if(DeptID == "0") { List.LoadData("search"); } else {List.LoadData("");};
        });

        //검색입력필드 엔터 이벤트 기술
        $("#CbtnSearch").keydown(function (e) {
			DeptID = "0";
            List.Search();
        });

        //검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
			DeptID = "0";
            List.Search();
        });

        //입력필드 엔터 이벤트 기술
        $('input').keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
				DeptID = "0";
                List.Search();
            }
        });       

        List.LoadData("");

    });
});



/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadAccountData = function () {
        List.GetTreeListCount();
        List.GetTreePageList();		
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadAccountSearchData = function () {
        List.GetSearchUserchListCount(); //검색 총 카운트 가져오기
        List.GetSearchUserList(); //검색 리스트 가져오기

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

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadAccountSearchData();
        }
        else {
            loadAccountData();
        }
    }, 100);
}


/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {
    CurrentPage = 1; //1페이지로 이동처리 

    if (CompanyID == "0") CompanyID = CurrentCompanyID;

    List.LoadData("search");
}




/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#r_photo")[0] != undefined) { $("#r_photo")[0].innerHTML = R_MyPhoto; }
    if ($("#r_name")[0] != undefined) { $("#r_name")[0].innerHTML = R_Private_AccountInfo_ltUserName; }
    if ($("#r_deptname")[0] != undefined) { $("#r_deptname")[0].innerHTML = R_Admin_UserManager_GridHeader2; }
    if ($("#r_titledutyname")[0] != undefined) { $("#r_titledutyname")[0].innerHTML = R_TitleDutyName; }
    if ($("#r_mobile")[0] != undefined) { $("#r_mobile")[0].innerHTML = R_MobilePhone; }
    if ($("#r_officephone")[0] != undefined) { $("#r_officephone")[0].innerHTML = R_AllPhoneNumber; }
    if ($("#r_mail")[0] != undefined) { $("#r_mail")[0].innerHTML = R_Private_AccountInfo_ltMail; }
    if ($("#NoListData")[0] != undefined) { $("#NoListData")[0].innerHTML = R_NoContent; }
}



/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 트리에서 호출한 값으로 리스트 카운트 호출
**********************************************************************************************/
List.GetTreeListCount = function () {

    var url = "/DeskPlusEIP/Account/Ws/Search/List.asmx/GetTreeListCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'selownerID': UserID,
        'seldeptID': DeptID,
        'types': Types
    };

    var obj = eval("List.SuccessPageListCount");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 리스트 검색으로 인한 카운트 호출
**********************************************************************************************/
List.GetSearchUserchListCount = function () {

    // 이름
    var name = $("#ctbName").val().securityText();
    //$("#ctbName").val(name);

    // 부서명
    var deptName = $("#ctbDeptName").val().securityText();
    //$("#ctbDeptName").val(deptName);


    ParamArray = {
        'companyID': CompanyID,
        'name': $("#ctbName").val(),
        'mobilePhone': $("#cbtMobile").val(),
        'deptName': $("#ctbDeptName").val(),
        'officePhone': $("#ctbOfficePhone").val(),
        'title': $("#ctbTitle").val(),
        'mail': $("#ctbMail").val(),
        'andOr': $("#Condition").val()
    };


    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageListCount");
    var url = "/DeskPlusEIP/Account/Ws/Search/List.asmx/GetSearchUserchListCount";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 트리에서 호출하여 리스트 바인딩 리스트 호출(트리)
**********************************************************************************************/
List.GetTreePageList = function () {
    var url = "/DeskPlusEIP/Account/Ws/Search/List.asmx/GetTreePageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'selownerID': UserID,
        'seldeptID': DeptID,
        'types': Types,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 리스트 검색으로 인한 리스트 호출
**********************************************************************************************/
List.GetSearchUserList = function () {



    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'companyID': CompanyID,
        'name': $("#ctbName").val(),
        'mobilePhone': $("#cbtMobile").val(),
        'deptName': $("#ctbDeptName").val(),
        'officePhone': $("#ctbOfficePhone").val(),
        'title': $("#ctbTitle").val(),
        'mail': $("#ctbMail").val(),
        'order': Order,
        'andOr': $("#Condition").val()

    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageList");
    var url = "/DeskPlusEIP/Account/Ws/Search/List.asmx/GetSearchUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시 인사 카운트 표시
**********************************************************************************************/
List.SuccessPageListCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //List.Resource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시 인사 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Search/JTSearch.htm?seq=' + Guids(), null, { filter_data: false });

    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 사람에게 댓글쓰기 팝업
**********************************************************************************************/
List.PopupShortReplyList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    //var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx";
    //var param = "p=" + GetEncryptValue;
    //WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "800", "500");

    var url = "/DeskPlusEIP/SNS/Common/MyTalkGroup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "700", "520");
}