FolderAdd = {}

$(function () {
    $(document).ready(function () {
        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); FolderAdd.Add(); });
        // [닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        $("#ctbFolderName").focus();
    });
});

/**************************************************
작성자   :
작성일자 :
작성목적 : [추가]
***************************************************/
FolderAdd.Add = function () {
    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbFolderName", R_Msg_ErrInputFolder, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnSubmit', "");
}

/**************************************************
작성자   :
작성일자 :
작성목적 : submit 후처리
***************************************************/
FolderAdd.SubmitClose = function () {
    parent.document.location.href = "/DeskPlusEIP/Report/Content/FolderList.aspx";
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}