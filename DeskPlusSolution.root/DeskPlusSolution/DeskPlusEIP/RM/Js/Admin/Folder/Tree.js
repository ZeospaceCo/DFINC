Tree = {};
Tree.TreeFrame = parent.frmLeft;
Tree.ContentFrame = parent.frmContent;
Tree.Control = null;

/**************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Tree.Init();

        //폴더 추가
        $('#imgAdd').click(function (e) {
            e.preventDefault();
            Tree.Add();
        });

        //폴더 수정
        $('#imgMod').click(function (e) {
            e.preventDefault();
            Tree.Mod();
        });

        //폴더 삭제
        $('#imgDel').click(function (e) {
            e.preventDefault();
            Tree.Del();
        });

        //QRCode
        $('#qrcode').click(function () {
            Tree.GoQRCoeList();
        });

        //QRCode
        $('#qrcodesub').click(function () {
            Tree.GoQRCoeList();
        });

        //승인 대기 예약
        $('#AdminReserve').click(function (e) {
            Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable0List.aspx";
        });

        //승인 대기 예약
        $('#Usable0Reserve').click(function (e) {
            Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable0List.aspx";
        });

        // 승인 완료 예약
        $('#Usable1Reserve').click(function (e) {
            Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable1List.aspx";
        });

        // 승인 거부 예약
        $('#Usable2Reserve').click(function (e) {
            Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Admin/Reserve/Usable2List.aspx";
        });

        $("#imgAdd").show();
        $("#imgMod").hide();
        $("#imgDel").hide();
    });
});

// 메뉴 Accordion 초기화
$(function () {
    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
        //event: "mouseover"
        active: AccordionActive,
        overTop: 55
    });

    $("#accordionResizer").resizable({
        minHeight: 0,
        resize: function () {
            $("#accordion").accordion("resize");
        }
    });
});

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 초기화
***************************************************/
Tree.Init = function () {
    Tree.Control = tvFolderTree;

    if (Tree.Control != null) {
        ParentCategoryID = Tree.Control.SelectedNode.GetProperty("ParentCategoryID");
        RealCategoryID = Tree.Control.SelectedNode.ID;

        var listPage = "../Content/List.aspx";

        ParamArray = {
            'parentCategoryID': ParentCategoryID,
            'categoryID': RealCategoryID
        };

        if (listPage != "" && Tree.ContentFrame) {
            CallEncrypt(CallGetParam());
            Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    ParentCategoryID = node.GetProperty("ParentCategoryID");
    RealCategoryID = node.ID;

    if (RealCategoryID == 0) {
        $("#imgAdd").show();
        $("#imgMod").hide();
        $("#imgDel").hide();
    }
    else {
        $("#imgAdd").show();
        $("#imgMod").show();
        $("#imgDel").show();
    }

    ParamArray = {
        'parentCategoryID': ParentCategoryID,
        'categoryID': RealCategoryID
    };

    if (Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = "../Content/List.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 폴더 추가
**************************************************************/
Tree.Add = function () {
    var gubun = "add";

    ParamArray = {
        'categoryID': RealCategoryID,
        'gubun': gubun
    };

    if (Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = "Add.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 폴더 수정
**************************************************************/
Tree.Mod = function () {
    var gubun = "mod";

    ParamArray = {
        'categoryID': RealCategoryID,
        'gubun': gubun
    };

    if (Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = "Add.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 폴더삭제
***************************************************/
Tree.Del = function () {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Folder/Tree.asmx/GetSubFolderCount";

    ParamArray = {
        'categoryID': RealCategoryID
    };

    var obj = eval("Tree.SubFolder");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-07
작성목적 : 삭제 가능 여부
***************************************************/
Tree.SubFolder = function (data, status) {
    var tmpcount = data.d;
    var arrCount = tmpcount.split('^');

    if (arrCount[0] > 0) {               // 하위폴더 존재로 인한 삭제 불가
        alert(R_DeleteSubFolder);
    }
    else {
        if (arrCount[1] > 0) {
            alert(R_DeleteNoFolder);     // 해당 폴더에 자원이 있어서 삭제 불가
        }
        else {
            $("#tbCID").val(RealCategoryID);
            if (confirm(R_DeleteFolder)) {
                __doPostBack("LBTNProcess", '');
            }
        }
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-03-08
작성목적 : QRCode List
**************************************************************/
Tree.GoQRCoeList = function () {
    $("#imgAdd").hide();
    $("#imgMod").hide();
    $("#imgDel").hide();

    Tree.ContentFrame.location.href = "../Content/ListQRCode.aspx";
}