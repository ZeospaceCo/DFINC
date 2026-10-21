Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        Add.GetList();
    });
});
// 양식리스트 가져오기
Add.GetList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Preference/FavoriteForm/Add.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {};
    var obj = eval("Add.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Add.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/FavoriteForm/JTAdd.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    Add.Resource();
}
Add.Resource = function () {

    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}
// 문서작성
Add.Submit = function (_formID) {

    //var nonActiveX = ($.browser.msie) ? "F" : "T";      // activeX(F) , nonActiveX(T)
    var nonActiveX = "T";      // activeX(F) , nonActiveX(T)
    var tmpval = $("#ddlAdditionDept", parent.parent.frmLeft.document).val().split("|");

    var url = "/DeskPlusEIP/Approval/FormFlow/Basic/Add.aspx";
    ParamArray = {
        'formID': _formID
            , 'deptID': tmpval[0]
            , 'deptName': tmpval[1].replace("&", "zeo$")
            , 'titleID': tmpval[2]
            , 'titleName': tmpval[3]
            , 'dutyID': tmpval[4]
            , 'dutyName': tmpval[5]
            , 'rootDeptID': tmpval[6]
            , 'rootDeptName': tmpval[7].replace("&", "zeo$")
            , 'nonActiveX': nonActiveX
    };

    CallEncrypt(CallGetParam());
    parent.location.href = url + "?p=" + GetEncryptValue;
}