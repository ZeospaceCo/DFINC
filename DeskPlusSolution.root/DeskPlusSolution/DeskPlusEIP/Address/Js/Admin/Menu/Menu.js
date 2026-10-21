Menu = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide'
            //event: "mouseover",

        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        $('#imgAdd').click(function (e) {
            e.preventDefault();
            Menu.FolderAdd();
        });

        $('#imgDel').click(function (e) {
            e.preventDefault();
            Menu.FolderDel();
        });

        $('#configSetting').click(function () {
            $("#imgAdd").hide();
            $("#imgDel").hide();         
            parent.frmContent.location.href = "../Config/Default.aspx"; ;
        });

        Menu.Init();
    });
});


/**************************************************
초기화
***************************************************/
Menu.Init = function () {
    
    if (UseAddRootCommonCategory == "Y") {
        $("#imgAdd").show();
    }
    else {
        $("#imgAdd").hide();
    }
    $("#imgDel").hide();

    var listPage = "/DeskPlusEIP/Address/Admin/Folder/Add.aspx";
    var gubun = "add";

    ParamArray = {
        'categoryID': RealFolderCategoryID,
        'types': FolderType,
        'gubun': gubun
    };

    if (listPage != "" && parent.frmContent) {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************
    그룹관리 선택
***************************************************/
function onNodeSelect(node) {
    
    $("#imgAdd").hide();    
    $("#imgDel").hide();

    var listPage = "/DeskPlusEIP/Address/Admin/Folder/Add.aspx";
    FolderCategoryID = node.GetProperty("CategoryID");
    RealFolderCategoryID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");
    RealFolderType = node.GetProperty("Type");

    //공용주소록 추가,수정,삭제버튼 활성화
    if (FolderCategoryID != "0" && FolderType == "CG") {
        $("#imgAdd").show();
        $("#imgDel").show();
    }
    else if (FolderCategoryID == "0" && FolderType == "CG") {
        // XML옵션에 따라 추가버튼 유무
        switch (UseAddRootCommonCategory) {
            case "Y":
                $("#imgAdd").show();
                break;
            case "N":
                $("#imgAdd").hide();
                break;
            default:
                $("#imgAdd").hide();
                break;
        }
    }

    var gubun = "mod";

    //상위 공용주소록은 추가용으로 한다.
    if (RealFolderCategoryID == 0 && FolderType == "CG") {
        listPage = "/DeskPlusEIP/Address/Admin/Folder/Add.aspx";
        gubun = "add";
    }

    ParamArray = {
        'categoryID': RealFolderCategoryID,
        'types': FolderType,
        'gubun': gubun
    };

    if (listPage != "" && parent.frmContent) {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
    그룹관리 선택 : onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
    그룹 추가
***************************************************/
Menu.FolderAdd = function () {
    var listPage = "/DeskPlusEIP/Address/Admin/Folder/Add.aspx";
    var gubun = "add";

    ParamArray = {
        'categoryID': RealFolderCategoryID,
        'types': FolderType,
        'gubun': gubun
    };

    if (listPage != "" && parent.frmContent) {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************
    그룹 삭제
***************************************************/
Menu.FolderDel = function () {

    if (RealFolderCategoryID != "0") {

        var url = "";

        ParamArray = {
            'categoryID': RealFolderCategoryID
        };

        if (RealFolderType == "CG") {
            url = "/DeskPlusEIP/Address/Ws/Admin/Menu/Menu.asmx/GetCommonCategoryPCount";

        }

        var obj = eval("Menu.SubCategory");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
}

/**************************************************
삭제 가능 여부
***************************************************/
Menu.SubCategory = function (data, status) {
    var count = data.d;

    if (count < 0) {                    // 오류로 인한 삭제 불가
        alert(R_ProcessingError);
    }
    else if (count > 0) {               // 하위카테고리 존재로 인한 삭제 불가
        alert(R_DeleteSubFolder);
    }
    else {                              // 삭제
        $("#tbCID").val(RealFolderCategoryID);
        if (window.confirm(R_DeleteFolder)) {
            __doPostBack("LBTNProcess", "CG");
        }
    }
}


/**************************************************
    공용주소록 관리 선택
***************************************************/
function onNodeSelectList(node) {
    
    $("#imgAdd").hide();    
    $("#imgDel").hide();

    var listPage = "/DeskPlusEIP/Address/Admin/Content/ListBlog.aspx";
    
    ParamArray = {
        'categoryID': node.GetProperty("CategoryID"),
        'types': node.GetProperty("Type")
    };

    if (listPage != "" && parent.frmContent) {
        //즐겨찾기 그룹만 이동안되도록 조치
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
    공용주소록 관리 선택 : onCallbackCompleteList 
**************************************************************/
function onCallbackCompleteList(node) {

}

