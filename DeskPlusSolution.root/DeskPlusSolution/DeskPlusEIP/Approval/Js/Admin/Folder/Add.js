Add = {}

$(function () {
    $(document).ready(function () {

        //event
        $("#ctbFolderName").keyup(function (e) { Add.SetMultiLang(); });

        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); Add.Add(); });
        // [닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [추가]
***************************************************/
Add.Add = function () {

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbFolderName", R_Msg_ErrInputFolder, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnSubmit', "");
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더명입력 시, 다국어처리
***************************************************/
Add.SetMultiLang = function () {
    var objid = "ctbName" + PreLang.replace("-", "").toUpperCase();
    $("#" + objid).val($("#ctbFolderName").val());
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : submit 후처리
***************************************************/
Add.SubmitClose = function () {
    parent.document.location.href = "/DeskPlusEIP/Approval/Admin/Folder/List.aspx";
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 폴더명입력evt
***************************************************/
Add.SetMultiLang = function () {
    var objid = "ctbName" + PreLang.replace("-", "").toUpperCase();
    $("#"+objid).val($("#ctbFolderName").val());
}