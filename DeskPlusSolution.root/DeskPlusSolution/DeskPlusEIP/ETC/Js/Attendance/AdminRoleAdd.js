AdminRoleAdd = {}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        if (parent.SelectUser.length > 0) {
            var arSel = parent.SelectUser.split('|');
            if (arSel.length > 0) {
                UserID = arSel[1];
                $("#txtUser").val(arSel[0]);
                AdminRoleAdd.GetViewList();
            }
        }

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            AdminRoleAdd.KeyEvents();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            parent.AdminRole.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            parent.AdminRole.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //사용자별 권한부서 추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            AdminRoleAdd.RoleAdd();
        });

        //트리검색(검색버튼)
        $('#CbtnDeptUserSearch').click(function (e) {
            e.preventDefault();
            SearchDeptUserList(1);
        });

        //트리검색(검색취소)
        $('#CbtnTreeView').click(function (e) {
            e.preventDefault();
            SearchDeptUserList(2);
        });

    });
});


/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 리소스 
***************************************************/
AdminRoleAdd.Resource = function () {
    $(".imgDel").attr("src", ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif");
    $("#r_DeptName").text(R_DeptName);
    $("#r_HoliDel").text(R_HoliDel);
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 엔터키이벤트
***************************************************/
AdminRoleAdd.KeyEvents = function () {
    if (event.keyCode == 13) {
        SearchDeptUserList(1);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 조직도 검색
***************************************************/
function SearchDeptUserList(param) {
    var searchName = $("#DeptUserTreeScript1_tbDeptUser").val().securityText();
    $("#DeptUserTreeScript1_tbDeptUser").val(searchName);

    var params = searchName + "" + param + "" + CompanyID;
    DeptUserTreeScript1_CallBack1.Callback(params);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 조직도 선택시
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    $("#tvSelectedID").empty();
    $("#tvSelectedValue").empty();

    var selIDs = $("#tvSelectedID").val();
    var selValues = $("#tvSelectedValue").val();
    var selID = node.ID;
    var selValue = node.Value;

    if (node.Checked) {


        selIDs = selIDs.replace(selID + "|", "");
        selIDs += selID + "|";


        selValues = selValues.replace(selValue + ";", "");
        selValues += selValue + ";";
    } else {
        selIDs = selIDs.replace(selID + "|", "");
        selValues = selValues.replace(selValue + ";", "");
    }

    $("#tvSelectedID").val(selIDs);

    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    // type(M) | userID | userName | parentDeptID | titleID | titleName | loginID | mail | Phone | officephone | DutyName
    $("#tvSelectedValue").val(selValues);

}
/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유 추가 
***************************************************/
AdminRoleAdd.RoleAdd = function () {

    UserList = "";
    var arSelValue = $("#tvSelectedValue").val().split(';');

    for (var i = 0; i < arSelValue.length; i++) {
        var arFor = arSelValue[i].split('|');
        if (arFor.length > 0) {
            if ($("#G_" + arFor[0]).html() == null) {
                UserList += "G_" + arFor[0] + "|";
            }
        }

    }

    AdminRoleAdd.AdminRoleAddList();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 리스트 가져오기
**********************************************************************************************/
AdminRoleAdd.GetViewList = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/GetRoleList";

    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };
    CallEncrypt(CallGetParam());


    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminRoleAdd.SuccessRoleList");


    AjaxService(url, data, obj);
}

AdminRoleAdd.SuccessRoleList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTAdminRoleAdd.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    AdminRoleAdd.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자추가
**********************************************************************************************/
AdminRoleAdd.AdminRoleAddList = function () {
    var isLowRank = 0;

    if ($('#cbAuthSubApply').is(':checked') == true) {
        isLowRank = 1;
    }
    else {
        isLowRank = 0;
    }

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/AddUserList";

    ParamArray = {
        'addUserID': UserID,
        'userList': UserList,
        'isLowRank': isLowRank
    };
    CallEncrypt(CallGetParam());


    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminRoleAdd.SuccessAdminRoleAddList");


    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자추가 성공시 리스트
**********************************************************************************************/
AdminRoleAdd.SuccessAdminRoleAddList = function (data, status) {

    AdminRoleAdd.GetViewList();
    
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자 삭제
**********************************************************************************************/
AdminRoleAdd.DelUserList = function (arg) {

    var GroupID = 0;

    if (arg.length > 0) {

        if (!confirm(R_MSG_DELSUCCESS))
            return;

        var arArg = arg.split('_');

        GroupID = arArg[1];

        var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/DelUserList";
        ParamArray = {
            'CompanyID': CompanyID,
            'UserID': UserID,
            'GroupID': GroupID
        };
        
        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("AdminRoleAdd.SuccessDel");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자 삭제 성공시 리스트
**********************************************************************************************/
AdminRoleAdd.SuccessDel = function (data, status) {

    AdminRoleAdd.GetViewList();
}

