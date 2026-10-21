ListR = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListR.LoadData("");
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
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            ListR.Add();
        });

        // 삭제 
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            ListR.Del();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListR.PageSize();
            ListR.SavePageSize(Type);
        });

        ListR.Init();

    });
});


/**************************************************************
ListR.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
ListR.Init = function () {

    ListR.LoadData("");
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    DeptID = node.ID;
    
    CurrentPage = 1; //1페이지로 이동처리  
    ListR.LoadData("search");

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
ListR.PageSize = function () {
    RecordPerPage = $('#ddlListCount').val();
    CurrentPage = 1; //1페이지로 이동처리 
    ListR.LoadData("search");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
ListR.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        ListR.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
        ListR.GetCount(); //검색 총 카운트 가져오기
        ListR.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListR.LoadData("search");
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
ListR.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'accountType': Type,
        'deptID': DeptID,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount
    };

    var obj = eval("ListR.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);


}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListR.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/DocContact/JTListR.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    ListR.Resource();
    ListR.GetActiveCnt();

    // 지정 버튼 처리
    $(".Start").hide();

    if (ActiveCnt == "0") {
        $(".Start").show();
    }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListR.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/GetPageListCnt";

    //파라미터값 배열에 담기
    ParamArray = {
        'accountType': Type,
        'deptID': DeptID
    };

    var obj = eval("ListR.SuccessGetPageListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListR.SuccessGetPageListCnt = function (data, status) {
    var response = data.d;
    TotalItemCount = response;
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListR.Resource = function () {

    $("#r_photo")[0].innerHTML = R_Photo;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_isactive")[0].innerHTML = R_IsActive;

    if ($(".NoListData") != undefined) { $(".NoListData").html(R_NoContent); }
    if ($(".activeN") != undefined) { $(".activeN").html(R_Clear); }
    if ($(".activeY") != undefined) { $(".activeY").html(R_SetUp); }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 :  추가
**********************************************************************************************/
ListR.Add = function () {
    
    ParamArray = {
        'deptID': DeptID,
        'callback' : "GetUserByDept"
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Account/Common/UserListByDept.aspx?p=" + GetEncryptValue;
    PopupWin(800, 600, url, "-500", "", true);

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 수발자,결재자 저장하기
**********************************************************************************************/
function GetUserByDept(response) {
    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        ListR.UserAdd(rtnVal);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 수발자,결재자 저장하기(웹서비스 호출)
**********************************************************************************************/
ListR.UserAdd = function (userList) {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/AddUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'userList': userList,
        'deptID': DeptID,
        'isActive': '0',
        'accountType': Type
    };

    var obj = eval("ListR.SuccessAddUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 수발자,결재자 저장하기(웹서비스 호출 성공)
**********************************************************************************************/
ListR.SuccessAddUser = function (data, status) { 

    if(data.d == true) {
        CurrentPage = 1; //1페이지로 이동처리  
        ListR.LoadData("search");
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
ListR.Checked = function () {

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
ListR.CheckedValue = function () {
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
ListR.Del = function () {
    var accIDList = "";
    accIDList = ListR.CheckedValue();

    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/DelUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'accIDList': accIDList
    };

    var obj = eval("ListR.SuccessDelUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 :  사용자 삭제 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListR.SuccessDelUser = function (data, status) {
    if (data.d == true) {
        CurrentPage = 1; //1페이지로 이동처리  
        ListR.LoadData("search");
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-16
작성목적 : 시작 버튼 클릭
***************************************************/
ListR.Start = function (_userID, _deptID) {
    
    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/Action";

    //파라미터값 배열에 담기
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID,
        'isActive': '1',
        'accountType': Type
    };

    //ActiveCnt = 1;
    var obj = eval("ListR.SuccessAction");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-16
작성목적 : 중지 버튼 클릭
***************************************************/
ListR.Stop = function (_userID, _deptID) {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/Action";

    //파라미터값 배열에 담기
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID,
        'isActive': '0',
        'accountType': Type
    };
    //ActiveCnt = 0;

    var obj = eval("ListR.SuccessAction");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 :  지정 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListR.SuccessAction = function (data, status) {

    if (data.d == true) {
        CurrentPage = 1; //1페이지로 이동처리  
        ListR.LoadData("search");
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-17
작성목적 :  지정 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListR.GetActiveCnt = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/DocContact/ListR.asmx/GetActiveCnt";

    //파라미터값 배열에 담기
    ParamArray = {
        'deptID': DeptID,
        'accountType': Type
    };

    var obj = eval("ListR.SuccessGetActiveCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);


}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-17
작성목적 :  지정 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListR.SuccessGetActiveCnt = function (data, status) {
    var response = data.d;
    ActiveCnt = response;
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
ListR.SavePageSize = function (_type) {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    if (_type == "S") {
        ParamArray = {
            'folderID': "Admin_Approver",
            'pageSize': $("#ddlListCount").val(),
            'types': 'Admin'
        };
    }
    else if (_type == "R") {
       
        ParamArray = {
            'folderID': "Admin_Rcver",
            'pageSize': $("#ddlListCount").val(),
            'types': 'Admin'
        };
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListR.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
ListR.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


