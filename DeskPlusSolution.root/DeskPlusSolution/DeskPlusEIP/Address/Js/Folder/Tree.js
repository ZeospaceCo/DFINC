Tree = {};
Tree.TreeFrame = parent.frmLeft;

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;



/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        Tree.Init();

        $('#folderAdd').click(function (e) {
            e.preventDefault();
            Tree.PrivateFolderAdd();
        });

        $('#folderModify').click(function (e) {
            e.preventDefault();
            Tree.PrivateFolderMod();
        });

        $('#folderDelete').click(function (e) {
            e.preventDefault();
            Tree.PrivateFolderDel();
        });

        $('#GoImpo').click(function (e) {
            e.preventDefault();
            Tree.GoImpo();
        });

        /*
        $(".accordion").accordion({
            heightStyle: "fill",
            autoHeight: false,
            //fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        $(".menusub:eq(" + AccordionActive + ")").attr("style", "display:");
        */

        $(".accordion").accordion({
            //heightStyle: "fill",
            //autoHeight: false,
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
        });

        var h = $(".menusub:eq(" + AccordionActive + ")").height();
        $(".menusub:eq(" + AccordionActive + ")").attr("style", "display:;height:" + h + "px;");

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //alert($(".accordion").height());
    });
});



/**************************************************
초기화
***************************************************/
Tree.Init = function () {
    Tree.ContentFrame = parent.frmContent;
    Tree.Control = UCAddressTree_tvAddressTree;

    $("#folderAdd").hide();
    $("#folderModify").hide();
    $("#folderDelete").hide();

    ParamArray = {
        'categoryID': "0",
        'types': FolderType,
        'rTypes': RealFolderType
    };

    var listPage = "../Content/List.aspx";

    if (RealFolderType == "PG") {
        AccordionActive = 2;
    } else if (RealFolderType == "FG") {
        AccordionActive = 3;
    }

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    Tree.SelectedNodeID = node.ID; //고유키
    $("#folderAdd").hide();
    $("#folderModify").hide();
    $("#folderDelete").hide();


    FolderCategoryID = node.GetProperty("CategoryID");
    RealFolderCategoryID = node.GetProperty("CategoryID");
    FolderType = node.GetProperty("Type");
    RealFolderType = node.GetProperty("Type");

    //즐겨찾기 일경우,
    if (FolderType == "FG" && FolderCategoryID != "0") {
        var etc = node.GetProperty("Etc");
        var selVal = etc.split("|");
        FolderCategoryID = selVal[0];
        FolderType = selVal[1];
    }

    //즐겨찾기 루트에서 추가, 개인주소록 루트에서 추가
    if (FolderCategoryID == "0" && FolderType == "FG") {
        $("#folderAdd").show();
        $("#folderModify").hide();
        $("#folderDelete").hide();
    }

    //즐겨찾기 하위그룹에서 추가, 개인주소록 하위그룹에서 추가
    if (FolderCategoryID != "0" && (FolderType == "PG" || FolderType == "FG")) {
        $("#folderAdd").show();
        $("#folderModify").show();
        $("#folderDelete").show();
    }

    // 즐겨찾기에서 추가한 그룹가 아닌 즐겨찾기 이외의 그룹에서 그룹즐겨찾기를 했을 경우의 추가
    if (RealFolderType == "FG" && RealFolderCategoryID != "0" && FolderType != "FG") {
        $("#folderAdd").hide();
        $("#folderModify").hide();
        $("#folderDelete").show();
    }

    // 개인주소록 루트, XML옵션에 따른 추가버튼 사용 유무
    if (FolderCategoryID == "0" && FolderType == "PG") {
        switch (UseAddRootPrivateCategory) {
            case "Y":
                $("#folderAdd").show();
                $("#folderModify").hide();
                $("#folderDelete").hide();
                break;
            case "N":
                $("#folderAdd").hide();
                $("#folderModify").hide();
                $("#folderDelete").hide();
                break;
            default:
                $("#folderAdd").hide();
                $("#folderModify").hide();
                $("#folderDelete").hide();
                break;
        }
    }

    Tree.GetCategoryUser(RealFolderCategoryID, FolderType); //사용자 설정 카테고리 정보    

    var listPage = "../Content/List.aspx";
    if (ListTypeUser == "2") {
        listPage = "../Content/ListBlog.aspx";
    }

    if (listPage != "" && Tree.ContentFrame) {

        ParamArray = {
            'categoryID': FolderCategoryID,
            'rTypes': RealFolderType,
            'types': FolderType
        };

        CallEncrypt(CallGetParam());

        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}


/**************************************************************
사용자별 카테고리 정보 가져오기
**************************************************************/
Tree.GetCategoryUser = function (_categoryID, _types) {
    var url = "/DeskPlusEIP/Address/Ws/Folder/Tree.asmx/GetCategoryUser";

    //    ParamArray = {
    //        'categoryID': RealFolderCategoryID,
    //        'types': FolderType
    //    };

    ParamArray = {
        'categoryID': _categoryID,
        'types': _types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SuccessCatUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
function SuccessCatUser(data, status) {
    var catUser = data.d;
    if (data.d != "") {
        ListTypeUser = catUser.split(":")[0]; //리스트형식
    }
}


/**************************************************
추가
***************************************************/
Tree.PrivateFolderAdd = function () {
    var listPage = "../Folder/Add.aspx";
    var gubun = "add";

    if (FolderType == "FG") {
        listPage = "../Folder/Add.aspx";
    }

    ParamArray = {
        'categoryID': RealFolderCategoryID,
        'types': FolderType,
        'gubun': gubun
    };


    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
수정
***************************************************/
Tree.PrivateFolderMod = function () {

    var listPage = "../Folder/Add.aspx";
    var gubun = "mod";

    if (FolderType == "FG") {
        listPage = "../Folder/Add.aspx";
    }

    ParamArray = {
        'categoryID': RealFolderCategoryID,
        'types': FolderType,
        'gubun': gubun
    };

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
삭제
***************************************************/
Tree.PrivateFolderDel = function () {

    var url = "";

    ParamArray = {
        'categoryID': RealFolderCategoryID
    };

    if (RealFolderType == "PG") {
        url = "/DeskPlusEIP/Address/Ws/Folder/Tree.asmx/GetPrivateCategoryCount";
    }
    else if (RealFolderType == "FG") {
        url = "/DeskPlusEIP/Address/Ws/Folder/Tree.asmx/GetFavorCategoryCount";
    }

    var obj = eval("Tree.SubCategory");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
삭제 가능 여부
***************************************************/
Tree.SubCategory = function (data, status) {
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
            __doPostBack("LBTNProcess", RealFolderType);
        }
    }
}

/**************************************************************
작성자 : 
작성일자 : 2011-10-17
작성목적 : 중요표시 페이지 호출하기
**************************************************************/
Tree.GoImpo = function () {

    $("#folderAdd").hide();
    $("#folderModify").hide();
    $("#folderDelete").hide();

    Tree.GetCategoryUser("0", "IG");

    var listPage = "../Content/ListImpo.aspx";
    if (ListTypeUser == "2") {
        listPage = "../Content/ListBlogImpo.aspx";
    }

    if (listPage != "" && Tree.ContentFrame) {
        Tree.ContentFrame.location.href = listPage;
    }

    //$(".accordion").accordion({ collapsible: true });

}