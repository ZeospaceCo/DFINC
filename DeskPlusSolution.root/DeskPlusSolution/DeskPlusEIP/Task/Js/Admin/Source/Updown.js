Updown = {}

$(function () {
    $(document).ready(function () {
        $("#ulWidgets").sortable();

        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#cbtnConfirm").click(function (e) {           
            e.preventDefault();

            var list = $("#ulWidgets li");

            var sourceList = "";

            for (var i = 0; i < list.length; i++) {
                sourceList += list[i].id;
                sourceList += ",";
            }           

            ParamArray = {
                'sourceList': sourceList
            };
            
            var url = "/DeskPlusEIP/Task/Ws/Admin/Source/Updown.asmx/SourceOrder";
            var obj = eval("Updown.SuccessOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);

        });

    });

});


Updown.SuccessOrder = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);

        if (parent.disablePopup) { parent.disablePopup(); parent.List.GetPageList(); parent.List.Resource(); } else { opener.List.GetPageList(); parent.List.Resource(); self.close(); }
    }
}