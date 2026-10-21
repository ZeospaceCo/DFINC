Tree = {};
Tree.TreeFrame = parent.frmLeft;

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;


/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Tree.Init();

        //폴더추가
        $('#imgUAdd').click(function (e) {
            e.preventDefault();
            Tree.PublicAdd();
        });

        //폴더수정
        $('#imgUMod').click(function (e) {
            e.preventDefault();
            Tree.PublicMod();
        });

        //촐더삭제
        $('#imgUDel').click(function (e) {
            e.preventDefault();
            Tree.PublicDel();
        });

        //폴더관리       
        $('#public').click(function () {
            $("#imgUAdd").hide();
            $("#imgUMod").hide();
            $("#imgUDel").hide();
        });

        //폴더리시트
        $('#publicList').click(function () {
            $("#imgUAdd").hide();
            $("#imgUMod").hide();
            $("#imgUDel").hide();
        });
        
    });
});

// 메뉴 Accordion 초기화
$(function () {
    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
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
작성일자 : 2011-10-17
작성목적 : 초기화
***************************************************/
Tree.Init = function () {

    Tree.ContentFrame = parent.frmContent;

    $("#userTotal").text(PDisPlayUse);
    $("#userQuota").text(PDisPlayQuota);
    $("#graphBar").width(PPercentCount + "%");
    $("#percent").text(PPercentCount + "%");

    Tree.Control = WebHardTreeA_tvWebHardTree;

    if (Tree.Control != null) {
        ParentFolderID = Tree.Control.SelectedNode.GetProperty("ParentFolderID");

        RealFolderID = Tree.Control.SelectedNode.ID;
        FolderType = Tree.Control.SelectedNode.GetProperty("Type");
        var childCnt = Tree.Control.SelectedNode.GetProperty("ChildCnt");
        var favoriteFolderID = Tree.Control.SelectedNode.GetProperty("FavoriteFolderID");
        var parentFavoriteID = Tree.Control.SelectedNode.GetProperty("ParentFavoriteID");

        if (FolderType == "A") {
            if (ParentFolderID == "-1") {
                $("#imgUMod").hide();
                $("#imgUDel").hide();
                $("#imgUAdd").show();
            }
            else {
                $("#imgUAdd").show();
                $("#imgUMod").show();
                $("#imgUDel").show();
            }
        }

        var listPage = "";

        if (FolderType == "A") {

            var gubun = "";

            if (ParentFolderID == "-1") {
                gubun = "add";

            }
            else {
                gubun = "mod";
            }

            ParamArray = {
                'folderID': RealFolderID,
                'gubun': gubun
            };

            listPage = "../Public/Add.aspx";
        }
        else {
            Tree.GetFolderUser(RealFolderID, FolderType);
            listPage = "../Content/List.aspx";

            if (FolderType != "B") {

                ParamArray = {
                    'folderID': RealFolderID,
                    'parentFolderID': ParentFolderID,
                    'types': FolderType,
                    'padmin': "1"
                };

                switch (ListTypeUser) {
                    case "1":
                        listPage = "../Content/List.aspx";
                        break;
                    case "2":
                        listPage = "../Content/ListIcon.aspx";
                        break;
                    case "3":
                        listPage = "../Content/ListPreview.aspx";
                        break;
                    default: listPage = "../Content/List.aspx";
                        break;
                }
            }
            else {
                ParamArray = {
                    'types': "B"
                };

                switch (ListTypeUser) {
                    case "1":
                        listPage = "../Trash/List.aspx";
                        break;
                    case "2":
                        listPage = "../Trash/ListIcon.aspx";
                        break;
                    case "3":
                        listPage = "../Trash/ListPreview.aspx";
                        break;
                    default: listPage = "../Trash/List.aspx";
                        break;
                }
            }
        }


        if (listPage != "" && Tree.ContentFrame) {
            CallEncrypt(CallGetParam());
            Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    ParentFolderID = node.GetProperty("ParentFolderID");
    RealFolderID = node.ID;
    var childCnt = node.GetProperty("ChildCnt");
    var favoriteFolderID = node.GetProperty("FavoriteFolderID");
    var parentFavoriteID = node.GetProperty("ParentFavoriteID");

    FolderType = node.GetProperty("Type");

    if (FolderType == "A") {
        if (ParentFolderID == "-1") {
            $("#imgUMod").hide();
            $("#imgUDel").hide();
            $("#imgUAdd").show();
        }
        else {
            $("#imgUAdd").show();
            $("#imgUMod").show();
            $("#imgUDel").show();
        }
    }

    var listPage = "";

    if (FolderType == "A") {

        var gubun = "";

        if (ParentFolderID == "-1") {
            gubun = "add";

        }
        else {
            gubun = "mod";
        }

        ParamArray = {
            'folderID': RealFolderID,
            'gubun': gubun
        };

        listPage = "../Public/Add.aspx";
    }
    else {
        listPage = "../Content/List.aspx";

        if (FolderType != "B") {

            Tree.GetFolderUser(RealFolderID, FolderType);

            ParamArray = {
                'folderID': RealFolderID,
                'parentFolderID': ParentFolderID,
                'types': FolderType,
                'padmin': "1"
            };

            switch (ListTypeUser) {
                case "1":
                    listPage = "../Content/List.aspx";
                    break;
                case "2":
                    listPage = "../Content/ListIcon.aspx";
                    break;
                case "3":
                    listPage = "../Content/ListPreview.aspx";
                    break;
                default: listPage = "../Content/List.aspx";
                    break;
            }


        }
        else {
            Tree.GetFolderUser(RealFolderID, FolderType);

            ParamArray = {
                'types': "B"
            };

            switch (ListTypeUser) {
                case "1":
                    listPage = "../Trash/List.aspx";
                    break;
                case "2":
                    listPage = "../Trash/ListIcon.aspx";
                    break;
                case "3":
                    listPage = "../Trash/ListPreview.aspx";
                    break;
                default: listPage = "../Trash/List.aspx";
                    break;
            }
        }
    }

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }

    var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

    ParamArray = {
        'folderID': RealFolderID,
        'types': FolderType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Tree.SuccessLoadPublic");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-23
작성목적 : 폴더용량 확인
**********************************************************************************************/
Tree.SuccessLoadPublic = function (data, status) {
    var folderData = data.d;

    if (FolderType == "B") {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = 0;
    }
    else {
        PUseCount = folderData.split("|")[1];
        PPrivateQuota = folderData.split("|")[0];

        if (PPrivateQuota == "") {
            PPrivateQuota = 0;
        }
    }

    if (PPrivateQuota == "0" || PPrivateQuota == "") {
        PDisPlayQuota = "∞";
    }
    else {
        //용량에 맞춰서 보여주기
        if (PPrivateQuota < 1024) {
            PDisPlayQuota = (parseFloat(PPrivateQuota)).toFixed(1) + "KB";
        }
        else if (PPrivateQuota < 1024 * 1024) {
            PDisPlayQuota = (parseFloat(PPrivateQuota) / (1024)).toFixed(1) + "MB";
        }
        else {
            PDisPlayQuota = (parseFloat(PPrivateQuota) / (1024 * 1024)).toFixed(1) + "GB";
        }
    }

    var tmpPercentCount = "0%";

    if (PUseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PPrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((PUseCount / (PPrivateQuota * 1024)) * 100);

        if ((0 < tmpPercentCount) && (tmpPercentCount < 1)) {
            tmpPercentCount = "0.1";
        }
        else {
            tmpPercentCount = parseFloat(tmpPercentCount).toFixed(1);
        }
    }

    if (tmpPercentCount > 100) {
        tmpPercentCount = 100;
    }

    var tmpUseCount = PUseCount;

    // Format 맞추어서 사용량 보여 주기 
    if (tmpUseCount < 1024) {
        if (tmpUseCount == 0) {
            tmpUseCount = "0 B";
        }
        else {
            tmpUseCount = parseFloat(tmpUseCount).toFixed(1) + "B";
        }
    }
    else if (tmpUseCount < 1024 * 1024) {
        tmpUseCount = (parseFloat(tmpUseCount) / 1024).toFixed(1) + "KB";
    }
    else if (tmpUseCount < 1024 * 1024 * 1024) {
        tmpUseCount = (parseFloat(tmpUseCount) / (1024 * 1024)).toFixed(1) + "MB";
    }
    else {
        tmpUseCount = (parseFloat(tmpUseCount) / (1024 * 1024 * 1024)).toFixed(1) + "GB";
    }

    $("#userTotal").text(tmpUseCount);
    $("#userQuota").text(PDisPlayQuota);
    $("#graphBar").width(tmpPercentCount + "%");
    $("#percent").text(tmpPercentCount + "%");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-12
작성목적 : 사용자별 폴더 정보 가져오기 (웹서비스 호출)
**********************************************************************************************/
Tree.GetFolderUser = function (p_folderID, p_types) {
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/Tree.asmx/GetFolderUser";

    if (p_types == "M") {
        p_types = "P";
    }

    ParamArray = {
        'folderID': p_folderID,
        'types': p_types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Tree.SuccessFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-12
작성목적 : 사용자별 폴더 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Tree.SuccessFolderUser = function (data, status) {
    var folderUser = data.d;
    if (data.d != "") {
        ListTypeUser = folderUser.split(":")[0]; //리스트형식
    }
}


/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 공용폴더추가
***************************************************/
Tree.PublicAdd = function () {

    var gubun = "add";

    ParamArray = {
        'folderID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Public/Add.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 공용 폴더수정
***************************************************/
Tree.PublicMod = function () {
    var gubun = "mod";

    ParamArray = {
        'folderID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Public/Add.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 공용 폴더삭제
***************************************************/
Tree.PublicDel = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Folder/Tree.asmx/GetSubFolderCount";

    ParamArray = {
        'folderID': RealFolderID
    };

    var obj = eval("Tree.PublicSubFolder");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : 이지원
작성일자 : 2017-06-19
작성목적 : 공용 삭제 가능 여부
***************************************************/
Tree.PublicSubFolder = function (data, status) {
    var count = data.d;

    if (count < 0) {                    // 오류로 인한 삭제 불가
        alert(R_ProcessingError);
    }
    else if (count > 0) {               // 하위폴더 존재로 인한 삭제 불가
        alert(R_DeleteSubFolder);
    }
    else {
        // 삭제
        $("#tbFID").val(RealFolderID);
        if (confirm(R_DeleteFolder)) {
            __doPostBack("LBTNProcess", FolderType);
        }
    }
}