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

    //수정
    $('#cbtnMod').click(function (e) {
        e.preventDefault();
        Tree.Mod();
    });

    //삭제
    $('#cbtnDel').click(function (e) {
        e.preventDefault();
        Tree.Del();
    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {

    if (CategoryID > 0) {

        ParamArray = {
            'categoryID': CategoryID,
            'gubun': 'mod'
        };

        CallEncrypt(CallGetParam());
        parent.frmRight.location.href = "List.aspx?p=" + GetEncryptValue;
    }
    else {
        
        $('#cbtnMod').hide();
        $('#cbtnDel').hide();

        //파라미터값 배열에 담기
//        ParamArray = {
//            'categoryID': CategoryID,
//            'gubun': 'add'
//        };
//        CallEncrypt(CallGetParam());

//        parent.frmRight.location.href = "AddFolder.aspx?p=" + GetEncryptValue;
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
        'categoryID': CategoryID,
        'gubun': 'add'
    };
    CallEncrypt(CallGetParam());

    parent.frmRight.location.href = "AddFolder.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 수정
**********************************************************************************************/
Tree.Mod = function () {

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'gubun': 'mod'
    };
    CallEncrypt(CallGetParam());

    parent.frmRight.location.href = "AddFolder.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 삭제
**********************************************************************************************/
Tree.Del = function () {

    if (CategoryID > 0) {

        if (window.confirm(R_ConfirmDelete)) {

            var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Tree.asmx/DelPortletCategory";

            //파라미터값 배열에 담기
            ParamArray = {
                'companyID': CompanyID,
                'categoryID': CategoryID
            };

            var obj = eval("Tree.SuccessDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-11-21 
작성목적 : 처리 후, 삭제
**********************************************************************************************/
Tree.SuccessDel = function (data, status) {
    if (data.d == true) {
        location = "Tree.aspx";
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 메뉴 관리 선택
***************************************************/
function PortletNodeSelect_onNodeSelect(node) {

    $('#cbtnMod').show();
    $('#cbtnDel').show();

    CategoryID = node.ID;   

    if (CategoryID == 0) {
        $('#cbtnMod').hide();
        $('#cbtnDel').hide();

        //파라미터값 배열에 담기
        ParamArray = {
            'categoryID': '0',
            'gubun': 'add'
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "AddFolder.aspx?p=" + GetEncryptValue;
    }
    else if (CategoryID > 0) {        

        //파라미터값 배열에 담기
        ParamArray = {
            'categoryID': CategoryID
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

