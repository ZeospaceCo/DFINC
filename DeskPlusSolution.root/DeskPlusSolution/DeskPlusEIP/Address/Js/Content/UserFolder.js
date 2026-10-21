UserFolder = {};


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {        

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        $("#ulWidgets").sortable();

        $("#cbtnConfirm").click(function (e) {
            
            e.preventDefault();
            var items = $("#ulWidgets li");
            var sortCatList = "";

            for (var i = 0; i < items.length; i++) {
                sortCatList += items[i].id;
                sortCatList += ",";
            }

            //alert(sortCatList);

            ParamArray = {
                'sortCatList': sortCatList,
                'types': Types
            };


            var url = "/DeskPlusEIP/Address/Ws/Content/UserFolder.asmx/ModOrderCategory";
            var obj = eval("UserFolder.SuccessModOrder");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        });

    });
});


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 주소록 카테고리 순서 성공시 페이지 표시
**********************************************************************************************/
UserFolder.SuccessModOrder = function (data, status) {

    if (data.d == true) {
        alert(R_ChangeCategoryOrder);

//        parent.disablePopup();
//        parent.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx";

        if (parent.disablePopup) { parent.disablePopup(); parent.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx"; } else { opener.parent.frmLeft.location.href = "/DeskPlusEIP/Address/Folder/Tree.aspx"; self.close(); }
    }
}
