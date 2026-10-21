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
    $('#DelCatID').click(function (e) {
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

    if (SYSCodeID > 0) {

        $("#DelCatID").show();

        ParamArray = {
            'sysCodeID': SYSCodeID,
            'parentCodeID': CodeID
        };

        CallEncrypt(CallGetParam());
        parent.frmRight.location = "List.aspx?p=" + GetEncryptValue;
    }
    else {
        $("#DelCatID").hide();
        //parent.frmRight.location = "AddCategory.aspx";
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
Tree.Del = function () {

    if (window.confirm(R_ConfirmDeleteSubCode)) {
        
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/Tree.asmx/DelSysCode";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'sysCodeList': SYSCodeID
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
작성목적 : 성공시 , 이벤트
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
작성목적 : 부서 선택
***************************************************/
function SysCodeNodeSelect_onNodeSelect(node) {

    SYSCodeID = node.ID;
    CodeID = node.GetProperty("CodeID");

    if (CodeID == "0") {
        $("#DelCatID").hide();
        parent.frmRight.location.href = "AddCategory.aspx";
    }
    else {
        $("#DelCatID").show();

        //파라미터값 배열에 담기
        ParamArray = {
            'sysCodeID': SYSCodeID,
            'parentCodeID': CodeID
        };
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

        parent.frmRight.location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

