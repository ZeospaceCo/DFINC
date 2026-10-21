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
            //List.LoadData("");
            List.GoPageLoad();
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

        //부서변경내역
        $('#ModList').click(function (e) {
            e.preventDefault();
            List.ModList();
        });

        //대결자 추가 
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //대결자 삭제 
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val()
            List.SavePageSize();
            //List.LoadData();
            List.GoPageLoad();
        });

        List.Init();

    });
});


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {   

    List.LoadData("");
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadProxyData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadProxySearchData = function () {
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
        // 삭제시에 페이지 총카운트 갯수 변화때문에 구분
        if (gubun == "search") {
            loadProxySearchData();

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
        else {
            loadProxyData();

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
    }, 100);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);


}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/Proxy/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    List.Resource();    
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/GetPageListCnt";

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

    $("#r_photo")[0].innerHTML = R_Photo;
    $("#r_name")[0].innerHTML = R_UserName;
    //$("#r_mail")[0].innerHTML = R_Mail;
    //$("#r_mobile")[0].innerHTML = "핸드폰";
    //$("#r_cphone")[0].innerHTML = R_Cphone;
    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_sta")[0].innerHTML = R_StateInfo;
    //$("#r_state")[0].innerHTML = R_State;
    $("#r_proxydate")[0].innerHTML = R_ProxyDate;
    $("#r_proxytype")[0].innerHTML = R_ProxyType;
    $("#r_isModfy")[0].innerHTML = R_IsModfy;

    //if ($(".r_allday") != undefined) { $(".r_allday").text("(" + R_AllDay + ")"); }

    //if ($(".spStart") != undefined) { $(".spStart").text(R_Msg_Resource05); } //대결중지
    //if ($(".spEnd") != undefined) { $(".spEnd").text(R_Msg_Resource04); } //대결시작
    if ($(".spIng") != undefined) { $(".spIng").text(R_Msg_Resource06); } //진행
    //if ($(".spClosing") != undefined) { $(".spClosing").text(R_Msg_Resource07); } //미진행

    if ($(".spCall") != undefined) { $(".spCall").text(R_Msg_Resource08); } //대기
    if ($(".spCallEnd") != undefined) { $(".spCallEnd").text(R_Msg_Resource09); } //종료
    
    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }

    if ($(".BtnCheck") != undefined) { $(".BtnCheck").attr('src', ImagePath + '/Icon/CheckY.png'); }
    if ($(".BtnUncheck") != undefined) { $(".BtnUncheck").attr('src', ImagePath + '/Icon/CheckG.png'); }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 대결자 추가
**********************************************************************************************/
List.Add = function () {

    /*
    ParamArray = {
        'gubun': "2"
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 517, url, "-500", "", true);
    */

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
    };

    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;

}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 대결자 추가 (팝업에서 선택 후)
**********************************************************************************************/
function GetUserDept(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");
        
        $("#tbxHiddenValue").val(responseStr[0]);
        $("#tbxHiddenDeptValue").val(responseStr[2]);

        if ($("#tbxHiddenValue").val() != "") {
            __doPostBack('lbtnAdd', '');
        }
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 대결자 삭제
**********************************************************************************************/
List.Del = function () {

    var proxyID = List.CheckedValue();

    if (proxyID != "") {
        if (window.confirm(R_ConfirmDeleteProxyPerson)) {

            ParamArray = {
                'proxyID': proxyID
            };

            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/DelProxyUser";
            var obj = eval("List.SuccessDelProxyUser");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckDeleteProxyPerson);
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 대결자 삭제 성공시 페이지 표시
**********************************************************************************************/
List.SuccessDelProxyUser = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리  
        //List.LoadData("search");
        List.GoPageLoad();
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
작성일자 : 2012-05-16
작성목적 : 대결시작 버튼 클릭
***************************************************/
List.ProxyStart = function (_proxyID, _proxyUserID, _proxyDeptID) {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/ProxyAction";

    //파라미터값 배열에 담기
    ParamArray = {
        'proxyID': _proxyID,
        'ProxyUserID': _proxyUserID,
        'ProxyDeptID': _proxyDeptID,
        'isProxy': 'Y'
    };

    //ProxyCnt = 1;
    var obj = eval("List.SuccessProxyAction");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-16
작성목적 : 대결중지 버튼 클릭
***************************************************/
List.ProxyStop = function (_proxyID, _proxyUserID, _proxyDeptID) {
    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/ProxyAction";

    //파라미터값 배열에 담기
    ParamArray = {
        'proxyID': _proxyID,
        'ProxyUserID': _proxyUserID,
        'ProxyDeptID': _proxyDeptID,
        'isProxy': 'N'
    };
    //ProxyCnt = 0;
    var obj = eval("List.SuccessProxyAction");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 대결 지정 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessProxyAction = function (data, status) {

    if (data.d == true) {
        List.LoadData("search");
    }
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Proxy",
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
작성목적 : 부서변경내역
**********************************************************************************************/
List.ModList = function () {

    ParamArray = {};
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Preference/Proxy/ListModify.aspx?p=" + GetEncryptValue;
    PopupWin(500, 350, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 대결자 정보 보기
**********************************************************************************************/
List.View = function (_proxyID) {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'targetProxyID': _proxyID
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

