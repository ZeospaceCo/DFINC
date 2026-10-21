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

        $('#imgAdd').click(function (e) {
            e.preventDefault();
            Tree.Add();
        });

        $('#imgMod').click(function (e) {
            e.preventDefault();
            Tree.Mod();
        });

        $('#imgDel').click(function (e) {
            e.preventDefault();
            Tree.Del();
        });

        $('#imgFaAdd').click(function (e) {
            e.preventDefault();
            Tree.FavoriteAdd();
        });

        $('#imgFaMod').click(function (e) {
            e.preventDefault();
            Tree.FavoriteMod();
        });

        $('#imgFaDel').click(function (e) {
            e.preventDefault();
            Tree.FavoriteDel();
        });

        //중요표시
        $('#impo').click(function () {
            Tree.GoImpoPage();
        });

        //관리자
        $('#Manager').click(function () {
            Tree.Admin();
        });        

        //공용폴더       
        $('#public').click(function () {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        });

        //개인 폴더
        $('#private').click(function () {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        });

        //공유받은 폴더
        $('#share').click(function () {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        });

        //공유한 폴더
        $('#shared').click(function () {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();

        });

        // 즐겨찾기
        $('#favorite').click(function () {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        });

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
작성일자 : 2011-10-17
작성목적 : 초기화
***************************************************/
Tree.Init = function () {

    if (AccessAuth == "True") {
        $("#Manager").show();
    }
    else {
        $("#Manager").hide();
    }

    Tree.ContentFrame = parent.frmContent;

    if (FolderType == "U" && AccordionActive == "0") {
        $("#userTotal").text(PDisPlayUse);
        $("#userQuota").text(PDisPlayQuota);
        $("#graphBar").width(PPercentCount + "%");
        $("#percent").text(PPercentCount + "%");
    }
    else {
        $("#userTotal").text(DisPlayUse);
        $("#userQuota").text(DisPlayQuota);
        $("#graphBar").width(PercentCount + "%");
        $("#percent").text(PercentCount + "%");
    }

    if (AccordionActive == "4") {

        // 즐겨찾기 폴더 추가, 수정, 삭제일 때 즐겨찾기 최상위리스트를 보여주기 위하여
        Tree.Control = WebHardTree3_tvWebHardTree;

        $("#imgAdd").hide();
        $("#imgMod").hide();
        $("#imgDel").hide();
        $("#imgFaAdd").show();
        $("#imgFaMod").hide();
        $("#imgFaDel").hide();

        ParamArray = {
            'favoriteID': '-5',
            'types': 'FC'
        };

        var listPage = "../Content/List.aspx";

        if (listPage != "" && Tree.ContentFrame) {
            CallEncrypt(CallGetParam());
            Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
        }

        return;
    }

    if (AccordionActive == "0") {
        Tree.Control = WebHardTree4_tvWebHardTree;
    }
    else {
        Tree.Control = UCWebHardTree_tvWebHardTree;
    }

    if (Tree.Control != null) {
        ParentFolderID = Tree.Control.SelectedNode.GetProperty("ParentFolderID");

        RealFolderID = Tree.Control.SelectedNode.ID;
        FolderType = Tree.Control.SelectedNode.GetProperty("Type");
        var childCnt = Tree.Control.SelectedNode.GetProperty("ChildCnt");
        var favoriteFolderID = Tree.Control.SelectedNode.GetProperty("FavoriteFolderID");
        var parentFavoriteID = Tree.Control.SelectedNode.GetProperty("ParentFavoriteID");

        if (((FolderType == "J") || (FolderType == "T") || (FolderType == "S") || (FolderType == "M") || (FolderType == "B")) && (favoriteFolderID == "0")) {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();

        }
        else if (FolderType == "FC") {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").show();

            if (RealFolderID == "-5") {
                $("#imgFaMod").hide();
                $("#imgFaDel").hide();
            }
            else {
                $("#imgFaMod").show();
                if (childCnt == "0") {
                    $("#imgFaDel").show();
                }
                else {
                    $("#imgFaDel").hide();
                }
            }
        }
        else if (favoriteFolderID > 0) {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").show();

        }
        else {
            if (FolderType != "U") {
                if (ParentFolderID == "-1") {
                    $("#imgAdd").show();
                    $("#imgMod").hide();
                    $("#imgDel").hide();
                    $("#imgFaAdd").hide();
                    $("#imgFaMod").hide();
                    $("#imgFaDel").hide();

                }
                else {
                    $("#imgAdd").show();
                    $("#imgMod").show();
                    $("#imgDel").show();
                    $("#imgFaAdd").hide();
                    $("#imgFaMod").hide();
                    $("#imgFaDel").hide();
                }
            }
            else {
                $("#imgAdd").hide();
                $("#imgMod").hide();
                $("#imgDel").hide();
                $("#imgFaAdd").hide();
                $("#imgFaMod").hide();
                $("#imgFaDel").hide();
            }
        }

        Tree.GetFolderUser(RealFolderID, FolderType);
        var listPage = "../Content/List.aspx";

        if (FolderType != "T" && FolderType != "B") {
            if (FolderType == "FC") {
                ParamArray = {
                    'childCnt': childCnt,
                    'parentFolderID': ParentFolderID,
                    'parentFavoriteID': parentFavoriteID,
                    'favoriteID': RealFolderID,
                    'types': FolderType
                };
            }
            else {
                if (favoriteFolderID > 0) {
                    ParamArray = {
                        'folderID': favoriteFolderID,
                        'parentFolderID': ParentFolderID,
                        'parentFavoriteID': parentFavoriteID,
                        'favoriteID': RealFolderID,
                        'types': FolderType
                    };
                }
                else {
                    ParamArray = {
                        'folderID': RealFolderID,
                        'parentFolderID': ParentFolderID,
                        'types': FolderType
                    };
                }
            }

            if (FolderType == "U" && ParentFolderID == -1) {
                listPage = "../Content/Home.aspx";
            }
            else {
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
        }
        else {
            ParamArray = {
                'types': FolderType
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

    /*
    FolderUserName = "";
    if (node.ParentNode) {
        FolderUserName = node.ParentNode.Text;
    }
    */

    FolderType = node.GetProperty("Type");

    if (((FolderType == "J") || (FolderType == "T") || (FolderType == "S") || (FolderType == "M") || (FolderType == "B")) && (favoriteFolderID == "0")) {
        $("#imgAdd").hide();
        $("#imgMod").hide();
        $("#imgDel").hide();
        $("#imgFaAdd").hide();
        $("#imgFaMod").hide();
        $("#imgFaDel").hide();

    }
    else if (FolderType == "FC") {
        $("#imgAdd").hide();
        $("#imgMod").hide();
        $("#imgDel").hide();
        $("#imgFaAdd").show();


        if (RealFolderID == "-5") {
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        }
        else {
            $("#imgFaMod").show();
            var childCnt = node.GetProperty("ChildCnt");
            if (childCnt == "0") {                
                $("#imgFaDel").show();
            }
            else {                
                $("#imgFaDel").hide();
            }
        }
    }
    else if (favoriteFolderID > 0) {
        $("#imgAdd").hide();
        $("#imgMod").hide();
        $("#imgDel").hide();
        $("#imgFaAdd").hide();
        $("#imgFaMod").hide();
        $("#imgFaDel").show();

    }
    else {
        if (FolderType != "U") {            
            if (ParentFolderID == "-1") {
                $("#imgAdd").show();
                $("#imgMod").hide();
                $("#imgDel").hide();
                $("#imgFaAdd").hide();
                $("#imgFaMod").hide();
                $("#imgFaDel").hide();

            }
            else {
                $("#imgAdd").show();
                $("#imgMod").show();
                $("#imgDel").show();
                $("#imgFaAdd").hide();
                $("#imgFaMod").hide();
                $("#imgFaDel").hide();

            }
        }
        else {
            $("#imgAdd").hide();
            $("#imgMod").hide();
            $("#imgDel").hide();
            $("#imgFaAdd").hide();
            $("#imgFaMod").hide();
            $("#imgFaDel").hide();
        }
    }

    var listPage = "../Content/List.aspx";

    if (FolderType == "S" && ParentFolderID == "-1") {
        node.Expand();
        return;
    }
    if (FolderType != "T" && FolderType != "B") {

        if (FolderType == "FC") {
            ListTypeUser = "1";

            ParamArray = {
                'childCnt': childCnt,
                'parentFolderID' : ParentFolderID,
                'parentFavoriteID': parentFavoriteID,
                'favoriteID': RealFolderID,
                'types': FolderType
            };
        }
        else {
            if (favoriteFolderID > 0) {

                Tree.GetFolderUser(favoriteFolderID, FolderType);

                ParamArray = {
                    'folderID': favoriteFolderID,
                    'parentFolderID': ParentFolderID,
                    'parentFavoriteID': parentFavoriteID,
                    'favoriteID': RealFolderID,
                    'types': FolderType
                };
            }
            else {
                Tree.GetFolderUser(RealFolderID, FolderType);

                ParamArray = {
                    'folderID': RealFolderID,
                    'parentFolderID': ParentFolderID,
                    'types': FolderType
                };
            }
        }

        if (FolderType == "U" && ParentFolderID == "-1") {
            listPage = "../Content/Home.aspx";
        }
        else {
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
    }
    else {
        Tree.GetFolderUser(RealFolderID, FolderType);

        ParamArray = {
            'types': FolderType
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

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }

    if (FolderType == "U" || FolderType == "B") {
        var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

        if (favoriteFolderID > 0) {
            ParamArray = {
                'folderID': favoriteFolderID,
                'types': FolderType
            };
        }
        else {
            ParamArray = {
                'folderID': RealFolderID,
                'types': FolderType
            };
        }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Tree.SuccessLoadPublic");

        AjaxService(url, data, obj);
    }
    else if (FolderType == "S") {
        Tree.LoadShare();
    }
    else {
        var obj = eval("Tree.SuccessLoad");
        var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetUseCount";

        AjaxService(url, "", obj);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-12
작성목적 : 공유받은폴더 선택시
**********************************************************************************************/
Tree.LoadShare = function () {
    $("#userTotal").text("∞");
    $("#userQuota").text("∞");
    $("#graphBar").width("0%");
    $("#percent").text("0%");
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
작성자   : 이지원
작성일자 : 2017-06-23
작성목적 : 폴더용량 확인
**********************************************************************************************/
Tree.SuccessLoad = function (data, status) {

    // 트리의 그래픽 바 , 퍼센트 보여주기
    UseCount = data.d;

    var tmpPercentCount = "0%";

    if (UseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((UseCount / (PrivateQuota * 1048576)) * 100);

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

    var tmpUseCount = UseCount;

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
    $("#userQuota").text(DisPlayQuota);
    $("#graphBar").width(tmpPercentCount + "%");
    $("#percent").text(tmpPercentCount + "%");
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : 폴더추가
***************************************************/
Tree.Add = function () {
    var gubun = "add";

    ParamArray = {
        'folderID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Folder/Add.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : 폴더수정
***************************************************/
Tree.Mod = function () {
    var gubun = "mod";

    ParamArray = {
        'folderID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Folder/Add.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-18
작성목적 : 폴더삭제
***************************************************/
Tree.Del = function () {

    var url = "/DeskPlusEIP/WebHard/Ws/Folder/Tree.asmx/GetSubFolderCount";

    ParamArray = {
        'folderID': RealFolderID
    };

    var obj = eval("Tree.SubFolder");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-17
작성목적 : 삭제 가능 여부
***************************************************/
Tree.SubFolder = function (data, status) {
    var count = data.d;

    if (count < 0) {                    // 오류로 인한 삭제 불가
        alert(R_ProcessingError);
    }
    else if (count > 0) {               // 하위폴더 존재로 인한 삭제 불가
        alert(R_DeleteSubFolder);
    }
    else {                              // 삭제
        $("#tbFID").val(RealFolderID);
        if (confirm(R_DeleteFolder)) {
            __doPostBack("LBTNProcess", FolderType);
        }
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-12-28
작성목적 : 즐겨찾기 폴더추가
***************************************************/
Tree.FavoriteAdd = function () {
    var gubun = "add";

    ParamArray = {
        'favoriteID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Content/FavoriteAdd.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-12-28
작성목적 : 즐겨찾기 폴더수정
***************************************************/
Tree.FavoriteMod = function () {
    var gubun = "mod";

    ParamArray = {
        'favoriteID': RealFolderID,
        'gubun': gubun
    };

    var listPage = "../Content/FavoriteAdd.aspx";
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-12-28
작성목적 : 즐겨찾기 폴더삭제
***************************************************/
Tree.FavoriteDel = function () {

    $("#tbFID").val(RealFolderID);
    if (confirm(R_DelFavoriteFolder)) {
        __doPostBack("LBTNProcess", "Favorite");
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2011-12-30
작성목적 : 중요표시 페이지 호출하기
**************************************************************/
Tree.GoImpoPage = function () {

    $("#imgAdd").hide();
    $("#imgMod").hide();
    $("#imgDel").hide();
    $("#imgFaAdd").hide();
    $("#imgFaMod").hide();
    $("#imgFaDel").hide();

    Tree.GetFolderUser("0", "I");

    var listPage = "";

    switch (ListTypeUser) {
        case "1":
            listPage = "../Content/ListImpo.aspx";
            break;
        case "2":
            listPage = "../Content/ListImpoIcon.aspx";
            break;
        case "3":
            listPage = "../Content/ListImpoPreview.aspx";
            break;
        default: listPage = "../Content/ListImpo.aspx";
            break;
    }


    if (listPage != "" && Tree.ContentFrame) {
        Tree.ContentFrame.location.href = listPage;
    }

    //$(".accordion").accordion({ collapsible: true });

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
Tree.SuccessFolderUser = function(data, status) {
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
    else {                              // 삭제
        $("#tbFID").val(RealFolderID);
        if (confirm(R_DeleteFolder)) {
            __doPostBack("LBTNProcess", FolderType);
        }
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-03
작성목적 : 관리자 모드
**********************************************************************************************/
Tree.Admin = function () {
    ParamArray = {
        'accActive': 0
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/WebHard/Folder/TreeAdmin.aspx?p=" + GetEncryptValue;
    if (Tree.TreeFrame) Tree.TreeFrame.location.href = goUrl;
}