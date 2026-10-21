UnitSeq = {}

$(function () {
    $(document).ready(function () {
        
        //[확인]
        $("#ulWidgets").sortable();

        //[닫기]
        $("#cbtnClose").click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
        
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
            var obj = eval("UnitSeq.ScsModSeq");
            var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitSeq.asmx/ModSeq";

            AjaxService(url, data, obj);
        });


    });
});


/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 항목순서변경
**********************************************************************************************/
UnitSeq.ScsModSeq = function (data, status) {
    if (data.d == true) { alert(R_Msg_Success); }
    if (parent.disablePopup) { parent.disablePopup(); parent.UnitAdd.Reload(); } else { opener.UnitAdd.Reload(); self.close(); }
}
