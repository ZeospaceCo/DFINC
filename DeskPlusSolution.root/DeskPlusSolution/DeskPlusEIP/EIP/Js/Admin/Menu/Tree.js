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

    //추가
    $('#cbtnAdd').click(function (e) {
        e.preventDefault();
        Tree.Add();
    });    

    //삭제
    $('#cbtnDel').click(function (e) {
        e.preventDefault();
        Tree.Del();
    });

    //적용
    $('#cbtnApply').click(function (e) {
        e.preventDefault();
        Tree.Apply();
    });

    //메뉴아이콘설정
    $('#cbtnSetting').click(function (e) {
        e.preventDefault();
        Tree.IconSetting();
    });

});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {

    if (MenuID > 0) {

        $("#cbtnDel").show();

        ParamArray = {
            'menuID': MenuID
        };

        CallEncrypt(CallGetParam());
        parent.frmRight.location.href = "Mod.aspx?p=" + GetEncryptValue;
    }
    else {
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
    
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Tree.asmx/ResetMenuList";

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
작성목적 : 삭제
**********************************************************************************************/
Tree.Del = function () {

    if (window.confirm(R_ConfirmDeleteMenu)) {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Tree.asmx/DelMenu";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuID': MenuID
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
    if (data.d == "1") {
        location.href = "Tree.aspx";
    }
    else if (data.d == "0") {
        alert(R_ExistSubMenu);
    }
    else if (data.d == "-1") {
        alert(R_NotConfirmUserAccessAuth);
    }
    else {
        alert(R_ErrorProcessing);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 추가
**********************************************************************************************/
Tree.Add = function () {

    //파라미터값 배열에 담기
    ParamArray = {
        'menuID': MenuID
    };
    CallEncrypt(CallGetParam());

    parent.frmRight.location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 메뉴 관리 선택
***************************************************/
function MenuNodeSelect_onNodeSelect(node) {

    MenuID = node.ID;
    $("#cbtnAdd").show();

    var levels = node.GetProperty("Levels");    
    if (levels == "1") {
        $("#cbtnAdd").hide();
    }

    if (MenuID == 0) {
        $("#cbtnDel").hide();

        //파라미터값 배열에 담기
        ParamArray = {
            'menuID': MenuID
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "Add.aspx?p=" + GetEncryptValue;
    }
    else if (MenuID > 0) {
        $("#cbtnDel").show();

        //파라미터값 배열에 담기
        ParamArray = {
            'menuID': MenuID
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "Mod.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-10-08
작성목적 : 메뉴아이콘설정
**********************************************************************************************/
Tree.IconSetting = function () {

    var MsgIcon = "";
    var TmpIconYN = "";
    if (IconYN == "N") {
        MsgIcon = R_MenuSetting1;
        TmpIconYN = "Y";
    }
    else {
        MsgIcon = R_MenuSetting2;
        TmpIconYN = "N";
    }

    if (window.confirm(MsgIcon)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Tree.asmx/IconSetting";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'userID': UserID,
            'iconYN': TmpIconYN
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Tree.SuccessSetting");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-10-08
작성목적 : 성공시 , 삭제
**********************************************************************************************/
Tree.SuccessSetting = function (data, status) {
    if (data.d == true) {
        alert(R_ProcessingApply);

        if (IconYN == "Y") {
            IconYN = "N";
        }
        else {
            IconYN = "Y";
        }
    }
    else {
        alert(R_ProcessingErrorApply);
    }
}
