Move = {}


Move.Control = null;
Move.InitNodeID = 0;
Move.ContentFrame = null;


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Move.Init();

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Move.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});


/**************************************************************
Move.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Move.Init = function () {

    Move.ContentFrame = parent.frmContent;
    Move.Control = UCDeptTreeScript1_DeptTreeScript1;
    Move.InitNodeID = 0;
    Move.SelectedNodeID = 0;    
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 반영
***************************************************/
Move.Confirm = function () {

    if (DeptID == ParentDeptID) {
        alert(R_NotApplySameUpper);
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'deptID': DeptID,
        'parentDeptID': ParentDeptID
    };
    
    var url = "/DeskPlusEIP/Account/Ws/Admin/Dept/Move.asmx/ModParentDeptID";
    var obj = eval("Move.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 순서 성공시 페이지 표시
**********************************************************************************************/
Move.SuccessConfirm = function (data, status) {

    if (data.d == true) {
        alert(R_SUCCESS);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GoTree(); } else { opener.List.GoTree(); self.close(); }
        
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {

    Move.SelectedNodeID = node.ID;
    ParentDeptID = node.ID;
    $("#folderName").val(node.Text);
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

