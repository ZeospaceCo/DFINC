FolderOrder = {}

$(function () {
    $(document).ready(function () {
        $("#ulWidgets").sortable();

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) {
            e.preventDefault();
            var items = $("#ulWidgets li");
            var sortlist = "";

            for (var i = 0; i < items.length; i++) {
                sortlist += items[i].id + ",";
            }

            ParamArray = { 'sortlist': sortlist };
            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("FolderOrder.Success");
            var url = "/DeskPlusEIP/Report/Ws/Content/FolderOrder.asmx/ModOrder";

            AjaxService(url, data, obj);
        });
    });
});

/**********************************************************************************************
작성자   : withsuki
작성일자 :
작성목적 : 항목순서변경
**********************************************************************************************/
FolderOrder.Success = function (data, status) {
    if (data.d == true) {
        alert(R_Msg_Success);
        parent.document.location.href = "/DeskPlusEIP/Report/Content/FolderList.aspx";
    }
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}