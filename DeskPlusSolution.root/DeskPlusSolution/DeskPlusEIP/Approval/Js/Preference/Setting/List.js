List = {}

$(function () {

    $(document).ready(function () {

        // [추가]
        $('#cbtConfirm').click(function (e) { e.preventDefault(); List.Confirm(); });
    });
});


/**************************************************
[확인]
***************************************************/
List.Confirm = function () {

    var vals = "N";
    var menuType = "OLD";
    
    var objs = document.getElementsByName("rblUsePwd");
    for (var i = 0; i < objs.length; i++) {
        if (objs[i].checked) {
            vals = $(objs[i]).val();
        }
    }

    var menuObjs = document.getElementsByName("rblApprovalStatusMenuType");
    for (var j = 0; j < menuObjs.length; j++) {
        if (menuObjs[j].checked) {
            menuType = $(menuObjs[j]).val();
        }
    }

    ParamArray = { 'ispwd': vals, 'approvalStatusMenuType': menuType };
    var url = "/DeskPlusEIP/Approval/Ws/Preference/Setting/List.asmx/Mod";
    var obj = eval("List.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
List.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_MSG_SUCCESS);
    } else {
        alert(R_MSG_FAIL);
    }
}
