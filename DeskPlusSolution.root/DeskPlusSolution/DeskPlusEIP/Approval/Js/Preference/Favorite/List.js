List = {}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 초기화 
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

        $('#ModSeq').click(function (e) {
            e.preventDefault();
            List.ModSeq();
        });

        //부서변경내역
        $('#ModList').click(function (e) {
            e.preventDefault();
            List.ModList();
        });

        //추가 - 사용자
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제 
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.SavePageSize();
            List.LoadData();
        });

        //추가 - 부서
        $('#AddDept').click(function (e) {
            e.preventDefault();
            List.AddDept();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Init();

    });
});

/**************************************************
작성자   : 이문호
작성일자 :2012-05-09
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

    // 변경내역건수에 따른 분기처리. (우선.주석처리함)
    // $("#modifyInfo")[0].style.display = (ModifyCount > 0) ? "" : "none";
    List.LoadData("");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

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


    setTimeout(function () {
        if (gubun == "search") {
            loadBoardSearchData();
            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);   
        }
        else {
            loadBoardData();
            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
    }, 100);

}



/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/List.asmx/GetFavoriteList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount,
        'order': Order
    };
    
    var obj = eval("List.SuccessFavoriteList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);


}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessFavoriteList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/Favorite/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
   
    List.Resource();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/List.asmx/GetFavoriteListCnt";

    //파라미터값 배열에 담기
    ParamArray = {
    };

    var obj = eval("List.SuccessGetPageListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);


}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessGetPageListCnt = function (data, status) {
    var response = data.d;
    TotalItemCount = response;
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {    
    $("#r_number")[0].innerHTML = R_Number;
    $("#r_isModfy")[0].innerHTML = R_IsModfy;
    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_titleName")[0].innerHTML = R_TitleName;
    $("#r_dutyName")[0].innerHTML = R_DutyName;
    $("#r_userName")[0].innerHTML = R_UserName;
    $("#r_addDate")[0].innerHTML = R_AddDate;

    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 자주 지정하는 결재자 추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'gubun': "2"
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);

}
//자주 지정하는 결재자 추가(팝업에서 추가후)
function GetUserDept(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");

        $("#tbxHiddenValue").val(responseStr[0]); // 추가 대상 userid
        $("#tbxHdApprovalDeptID").val(responseStr[2]); // 추가 대상 deptid

        if ($("#tbxHiddenValue").val() != "" && $("#tbxHdApprovalDeptID").val() != "") {
            __doPostBack('lbtnAdd', '');
        }
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 자주 지정하는 결재자 삭제
**********************************************************************************************/
List.Del = function () {

    var favoriteApprovalID = List.CheckedValue();
    
    if (favoriteApprovalID != "") {       
        if (window.confirm(R_ConfirmDeleteFavoritePerson)) {

            ParamArray = {
                'favoriteApprovalID': favoriteApprovalID
            };

            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/List.asmx/DelFavoriteUser";
            var obj = eval("List.SuccessDelUser");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckDeleteFavoritePerson);
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 자주지정하는 결재자 삭제 성공시 페이지 표시
**********************************************************************************************/
List.SuccessDelUser = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("search");
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
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
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();
    
    return allVals;
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Favorite",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Setting'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 순서변경
**********************************************************************************************/
List.ModSeq = function () {

    ParamArray = {};
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Preference/Favorite/ModSequence.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 부서변경내역
**********************************************************************************************/
List.ModList = function () {

    ParamArray = {};
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Preference/Favorite/ListModify.aspx?p=" + GetEncryptValue;
    PopupWin(500, 350, url, "-500", "", true);
}

/**********************************************************************************************
자주 지정하는 결재자 추가 - 부서추가
**********************************************************************************************/
List.AddDept = function () {

    ParamArray = {
        'isCheckBox' : '0'
        , 'callback': 'AddDeptVals'
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);

}
//자주 지정하는 결재자 추가(팝업에서 추가후)
function AddDeptVals(_selIDs, _selValues, _isSub) {     //alert(_selIDs + "=" + _selValues + "=" + _isSub)

    if (_selIDs != "undefined" && _selIDs != "" && _selIDs != null) {

        var issub = (confirm("하위부서를 포함하시겠습니까?")) ? "1" : "0";   // 하위부서포함(1),미포함(0)

        var ids = unescape(_selIDs);            //selIDs    : 367|1|368|378|
        var val = unescape(_selValues);         //selValues : 367|영업부|1|1;1|zeospaceCoKr 영업||3;368|기술부|1|2;378|영업1팀|367|1;

        $("#tbxHdApprovalDeptID").val(ids);     // 추가대상 deptid

        __doPostBack('lbtnAddDept', issub);
    }
}