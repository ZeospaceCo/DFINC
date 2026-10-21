Move = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Move.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.List.Search(); parent.disablePopup(); } else { self.close(); }
        });
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 반영
***************************************************/
Move.Confirm = function () {
    
    if (ParentCategoryID > 0) {
        ParamArray = {
            'companyID': CompanyID,
            'categoryID': ParentCategoryID,
            'portletID': PortletID
        };

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Move.asmx/ModMovePortlet";
        var obj = eval("Move.SuccessConfirm");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_CheckMoveFolder);
    }

}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 순서 성공시 페이지 표시
**********************************************************************************************/
Move.SuccessConfirm = function (data, status) {

    if (data.d == true) {
        alert(R_ConfirmMove);

        if (parent.disablePopup) { parent.List.Search(); parent.disablePopup(); } else { self.close(); }

    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 폴더 선택
***************************************************/
function PortletNodeSelect_onNodeSelect(node) {
    ParentCategoryID = node.ID;
    $("#ctbName").val("");
    if (ParentCategoryID > 0) {
        $("#ctbName").val(node.Text);
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

