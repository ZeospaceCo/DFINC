Move = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

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


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 반영
***************************************************/
Move.Confirm = function () {

    if (DeptID == 0) {
        alert(R_CheckMoveBaseDept);
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'deptID': DeptID,
        'userList': UserList
    };

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Move.asmx/ModBaseDept";
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

        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function DeptNodeSelect_onNodeSelect(node) {
    DeptID = node.ID;
    $("#folderName").val(node.Text);
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

