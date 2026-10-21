GetDept = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetDept.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            GetDept.Close();
        });
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
GetDept.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인 
***************************************************/
GetDept.Confirm = function () {

    if (RqDeptID == 0) { alert(R_Msg_Dept); return; }

    if (opener.$("#ctbDeptID") && opener.$("#ctbDeptNm")) {
        opener.$("#ctbDeptID").val(RqDeptID);
        opener.$("#ctbDeptNm").val(RqDeptNm);
    }

    GetDept.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 부서선택
***************************************************/
DeptNodeSelect_onNodeSelect = function(node) {

    RqDeptID = node.ID;
    RqDeptNm = node.Text;
    //DeptID = node.ID;
    //ParentDeptID = node.GetProperty("ParentDeptID");
}
