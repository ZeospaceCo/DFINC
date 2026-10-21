AddPortlet = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddPortlet.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


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

        //사용자이면, 공용포틀릿 비활성화
        if (IsAdmin == "0") {
            $("#commonMenu").hide();
            $("#commonMenuSub").hide();
        }

    });
});


/**************************************************
추가
***************************************************/
AddPortlet.Confirm = function () {

    if (CatID == "" || CatID == "0") {
        alert(R_CheckPortletGroup);
        return;
    }

    var url = "/DeskPlusEIP/EIP/Ws/Portal/AddPortlet.asmx/Add";
    var gubun = (Gubun == "COMMON") ? "1" : "0";

    ParamArray = {
        'categoryID': CatID,
        'titleName': TitleName,
        'modules': Modules,
        'dataUrl': DataUrl,
        'isAdmin': gubun,
        'template': Template
    };

    var obj = eval("AddPortlet.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
즐겨찾기 폴더에 추가
***************************************************/
AddPortlet.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_Complete);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
공용 포틀릿카테고리 트리 onNodeSelect 트리 노드 선택
***************************************************/
function CommonPortletTree_onNodeSelect(node) {
    $("#portletName").text("");
    Gubun = "COMMON";
    CatID = node.ID;
    if (CatID != "0") {        
        $("#portletName").text(node.Text);
    } else {
        alert(R_CheckPortletGroup);
    }
    
}


/**************************************************
개인 포틀릿카테고리 트리 onNodeSelect 트리 노드 선택
***************************************************/
function PrivatePortletTree_onNodeSelect(node) {
    $("#portletName").text("");
    Gubun = "PRIVATE";
    CatID = node.ID;
    if (CatID != "0") {
        $("#portletName").text(node.Text);
    } else {
        alert(R_CheckPortletGroup);
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}