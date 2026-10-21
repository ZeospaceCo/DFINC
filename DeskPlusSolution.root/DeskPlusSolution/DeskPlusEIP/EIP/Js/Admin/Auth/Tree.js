Tree = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        var accordionIdx = 0;
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            overTop: 0,
            active: accordionIdx
            //event: "mouseover",
        });

        Tree.Init();
    });

    //삭제
    $('#cbtnDel').click(function (e) {
        e.preventDefault();
        Tree.Del();
    });

    //수정
    $('#cbtnMod').click(function (e) {
        e.preventDefault();
        Tree.Mod();
    });

    //적용
    $('#cbtnApply').click(function (e) {
        e.preventDefault();
        Tree.Apply();
    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {

    if (RoleID > 0) {

        $("#cbtnDel").show();

        ParamArray = {
            'roleID': RoleID,
            'gubun': Gubun
        };

        CallEncrypt(CallGetParam());
        parent.frmRight.location.href = "List.aspx?p=" + GetEncryptValue;
    }
    else {
        $("#cbtnMod").hide();
        $("#cbtnDel").hide();
        //parent.frmRight.location.href = "Add.aspx";
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 메뉴 및 권한, IP 들 초기화하기
**********************************************************************************************/
Tree.Apply = function () {
    
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/Tree.asmx/ResetMenuList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Tree.SuccessApply");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
Tree.SuccessApply = function (data, status) {
    if (data.d == true) {
        alert(R_ProcessingApply);
    }
    else {
        alert(R_ProcessingErrorApply);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 수정
**********************************************************************************************/
Tree.Mod = function () {

    //파라미터값 배열에 담기
    ParamArray = {
        'roleID': RoleID,
        'gubun': Gubun
    };
    CallEncrypt(CallGetParam());

    parent.frmRight.location.href = "Add.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
Tree.Del = function () {

    if (window.confirm(R_ConfirmDeleteAuth)) {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/Tree.asmx/DelRole";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'roleID': RoleID,
            'gubun': Gubun
        };       

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Tree.SuccessDel");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
Tree.SuccessDel = function (data, status) {
    if (data.d == true) {
        location.href = "Tree.aspx";
    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 권한관리 선택
***************************************************/
function RoleNodeSelect_onNodeSelect(node) {

    RoleID = node.ID;
    Gubun = node.GetProperty("Gubun");

    if (RoleID < 0) {
        $("#cbtnDel").hide();
        $('#cbtnMod').hide();

        //파라미터값 배열에 담기
        ParamArray = {
            'roleID': RoleID,           
            'gubun': Gubun
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "Add.aspx?p=" + GetEncryptValue;
    }
    else if (RoleID > 0) {
        $("#cbtnDel").show();
        $('#cbtnMod').show();

        //파라미터값 배열에 담기
        ParamArray = {
            'roleID': RoleID,
            'gubun': Gubun
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

