AddMenu = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddMenu.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        /*
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover",
            active: AccordionActive,
            overTop: 30
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });
        */

    });
});


/**************************************************
추가
***************************************************/
AddMenu.Confirm = function () {

    //게시판 용
    if (Modules == "Board") {

        ParamArray = {
            'menuID': MenuID,
            'categoryID': CategoryID
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/EIP/Ws/Portal/AddMenu.asmx/AddMenuByBoard";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("AddMenu.SuccessCreateMenu");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 메뉴 생성하기
**********************************************************************************************/
AddMenu.SuccessCreateMenu = function (data, status) {
    if (data.d == true) {
        alert(R_Complete);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 메뉴 관리 선택
***************************************************/
function MenuNodeSelect_onNodeSelect(node) {    
    MenuID = node.ID;
    $("#portletName").text(node.Text);
}




/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}