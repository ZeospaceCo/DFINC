List = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {

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

        // 추가 
        $('#cbtnActive').click(function (e) { e.preventDefault(); List.Add(); });

        // 삭제 
        $('#cbtnDel').click(function (e) { e.preventDefault(); List.Del(); });

        //페이지 카운트
        $('#ddlListCount').change(function () { List.PageSize(); List.SavePageSize(); });

        // 전체내역조회
        $('#cbtnTotal').click(function (e) { e.preventDefault(); List.Total(); });

        List.Init();

    });
});


/**************************************************************
List.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
List.Init = function () {

    List.LoadData("");
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    DeptID = node.ID;

    CurrentPage = 1; //1페이지로 이동처리  
    List.LoadData("search");

    //jtemplate 출력
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
List.PageSize = function () {
    RecordPerPage = $('#ddlListCount').val();
    CurrentPage = 1; //1페이지로 이동처리 
    List.LoadData("search");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
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
            loadSearchData();

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
        else {
            loadData();

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

    var url = "/DeskPlusEIP/Approval/Ws/Admin/FolderAuth/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'type': Type,
        'deptID': DeptID,
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
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/FolderAuth/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    List.Resource();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/FolderAuth/List.asmx/GetPageListCnt";

    //파라미터값 배열에 담기
    ParamArray = {
        'type': Type,
        'deptID': DeptID
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

    $("#r_targetDept")[0].innerHTML = R_TargetDept;
    $("#r_targetUser")[0].innerHTML = R_TargetUser;

    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 :  추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'gubun': '2',
        'callback': 'GetAddUserList'
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 사용자 추가하기
**********************************************************************************************/
function GetAddUserList(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");

        var userList = responseStr[0];

        List.UserAdd(userList);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 사용자 추가(웹서비스 호출)
**********************************************************************************************/
List.UserAdd = function (userList) {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/FolderAuth/List.asmx/AddUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'userList': userList,
        'deptID': DeptID,
        'type': Type
    };

    var obj = eval("List.SuccessAddUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 수발자,결재자 저장하기(웹서비스 호출 성공)
**********************************************************************************************/
List.SuccessAddUser = function (data, status) {

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
작성자   : shine
작성일자 : 2012-05-10
작성목적 : 수발자,결재자 리스트 사용자 삭제
***************************************************/
List.Del = function () {
    var authIDList = "";
    authIDList = List.CheckedValue();

    var url = "/DeskPlusEIP/Approval/Ws/Admin/FolderAuth/List.asmx/DelUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'authIDList': authIDList
    };

    var obj = eval("List.SuccessDelUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 :  사용자 삭제 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessDelUser = function (data, status) {
    if (data.d == true) {
        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("search");
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 :  지정 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessAction = function (data, status) {

    if (data.d == true) {
        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("search");
    }
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function (_type) {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Admin_FolderAuth",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성목적 : [전체내역조회]
***************************************************/
List.Total = function () {

    ParamArray = {'p': 'p'};
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Admin/FolderAuth/TotalList.aspx?p=" + GetEncryptValue;
    PopupWin(600, 380, url, "-500", "", true);
}

