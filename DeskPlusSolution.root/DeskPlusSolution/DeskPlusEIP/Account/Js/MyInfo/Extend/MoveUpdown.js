MoveUpdown = {};


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
                'companyID': CompanyID,
                'gubun': Gubun,
                'sortCatList': sortCatList
            };

            var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/MoveUpdown.asmx/ModExtendOrder";
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
        alert(R_CompleteCategoryOrders);

        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }


        switch (Gubun) {
            case "family":
                parent.FamilyInfo.LoadData();
                break;
            case "career":
                parent.CareerInfo.LoadData();
                break;
            case "education":
                parent.EduInfo.LoadData();
                break;
            case "reward":
                parent.RewardInfo.LoadData();
                break;
            case "scholar":
                parent.ScholarInfo.LoadData();
                break;
        }
    }
}



