RoleAdminAdd = {}

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
                RoleAdminAdd.GetViewList();
            }
        }

        //검색입력필드 엔터 이벤트 기술
        $("#DeptUserTreeScript1_tbDeptUser").keydown(function (event) {
            RoleAdminAdd.KeyEvents();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            parent.RoleAdmin.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            parent.RoleAdmin.Search();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //사용자별 권한부서 추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            RoleAdminAdd.RoleAdd();
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
RoleAdminAdd.Resource = function () {
    $(".imgDel").text(R_Del);
    $("#r_DeptName").text(R_DeptName);
    $("#r_Del").text(R_Del);
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 엔터키이벤트
***************************************************/
RoleAdminAdd.KeyEvents = function () {
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
RoleAdminAdd.RoleAdd = function () {

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

    RoleAdminAdd.RoleAdminAddList();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 리스트 가져오기
**********************************************************************************************/
RoleAdminAdd.GetViewList = function () {

    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/GetRoleList";

    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };
    CallEncrypt(CallGetParam());


    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RoleAdminAdd.SuccessRoleList");


    AjaxService(url, data, obj);
}

RoleAdminAdd.SuccessRoleList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Admin/Content/JTRoleAdminAdd.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    RoleAdminAdd.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자추가
**********************************************************************************************/
RoleAdminAdd.RoleAdminAddList = function () {
    var isRowRank = 0;

    if ($('#cbAuthSubApply').is(':checked') == true) {
        isRowRank = 1;
    }
    else {
        isRowRank = 0;
    }

    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/AddUserList";

    ParamArray = {
        'addUserID': UserID,
        'userList': UserList,
        'isRowRank': isRowRank
    };
    CallEncrypt(CallGetParam());


    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RoleAdminAdd.SuccessRoleAdminAddList");


    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자추가 성공시 리스트
**********************************************************************************************/
RoleAdminAdd.SuccessRoleAdminAddList = function (data, status) {

    RoleAdminAdd.GetViewList();

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자 삭제
**********************************************************************************************/
RoleAdminAdd.DelUserList = function (arg) {

    var GroupID = 0;

    if (arg.length > 0) {

        if (!confirm(R_Msg_Del))
            return;

        var arArg = arg.split('_');

        GroupID = arArg[1];

        var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/DelUserList";
        ParamArray = {
            'CompanyID': CompanyID,
            'UserID': UserID,
            'GroupID': GroupID
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("RoleAdminAdd.SuccessDel");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-08
작성목적 : 공유자 삭제 성공시 리스트
**********************************************************************************************/
RoleAdminAdd.SuccessDel = function (data, status) {

    RoleAdminAdd.GetViewList();
}

