AddFolderAuth = {}

$(function () {
    $(document).ready(function () {

        //[확인]
        $('#CbtnConfirm').click(function (e) { e.preventDefault(); AddFolderAuth.Confirm(); });

        //[닫기]
        $('#CbtnClose').click(function (e) { e.preventDefault(); AddFolderAuth.Close(); });
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
AddFolderAuth.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인 
***************************************************/
AddFolderAuth.Confirm = function () {

    if (RqDeptID == 0) { alert(R_Msg_Dept); return; }

    if (opener.$("#ctbDeptID") && opener.$("#ctbDeptNm")) {
        opener.$("#ctbDeptID").val(RqDeptID);
        opener.$("#ctbDeptNm").val(RqDeptNm);
    }

    AddFolderAuth.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 부서선택
***************************************************/
DeptNodeSelect_onNodeSelect = function (node) {

    RqDeptID = node.ID;
    RqDeptNm = node.Text;
    //DeptID = node.ID;
    //ParentDeptID = node.GetProperty("ParentDeptID");
}
