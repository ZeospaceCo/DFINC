MoveUpdown = {};


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {        

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            parent.disablePopup();
        });


        $("#ulWidgets").sortable();

        $("#cbtnConfirm").click(function (e) {
            
            e.preventDefault();
            var items = $("#ulWidgets li");
            var sortList = "";

            for (var i = 0; i < items.length; i++) {
                sortList += items[i].id;
                sortList += ",";
            }

            //alert(sortCatList);

            ParamArray = {
                'sortList': sortList
            };
            
            var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/MoveUpdown.asmx/ModOrderSysCode";
            var obj = eval("MoveUpdown.SuccessModOrder");
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
MoveUpdown.SuccessModOrder = function (data, status) {

    if (data.d == true) {
        alert(R_ConfirmOrder);
        parent.disablePopup();
        parent.List.Search();
    }
}
