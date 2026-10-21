Tree = {};
Tree.TreeFrame = parent.frmLeft;
Tree.ContentFrame = parent.frmContent;

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //완료 프로젝트
        $("#CompletePRJ").click(function (e) {
            e.preventDefault();

            ParamArray = {
                'status': "3"
            };

            var url = "../Project/JList.aspx";
            if (url != "" && Tree.ContentFrame) {
                CallEncrypt(CallGetParam());
                Tree.ContentFrame.location.href = url + "?p=" + GetEncryptValue;
            }
        });

        //폴더추가
        $('#folderAdd').click(function (e) {
            e.preventDefault();
            Tree.Add();
        });

        //폴더수정
        $('#folderMod').click(function (e) {
            e.preventDefault();
            Tree.Mod();
        });

        //폴더삭제
        $('#folderDel').click(function (e) {
            e.preventDefault();
            Tree.Del();
        });

        /*
        ParamArray = {
            'status': "0"
        };

        var url = "../Project/JList.aspx";
        if (url != "" && Tree.ContentFrame) {
            CallEncrypt(CallGetParam());
            Tree.ContentFrame.location.href = url + "?p=" + GetEncryptValue;
        }
        */

        ResetButton();
    });
});

/**********************************************************************************************
[프로젝트관리 트리]
**********************************************************************************************/
function tvProjectTree_OnNodeSelect(node) {
    ResetButton();
    RealFolderID = node.ID;

    if (node.ID == "-1") {
        var url = "../Project/JList.aspx";
        Tree.ContentFrame.location.href = url
    }
    else if (node.ID == "-3") {
        var url = "../Project/JList.aspx";
        Tree.ContentFrame.location.href = url

        if (AccessAuth == "True") {
            $("#imgAdd").show();
        }
    }
    else {
        var nodeValue = node.Value.split("|");

        if (nodeValue[0] == "f") {
            ParamArray = {
                'folderID': node.ID
            };

            var url = "../Project/JList.aspx";
            if (url != "" && Tree.ContentFrame) {
                CallEncrypt(CallGetParam());
                Tree.ContentFrame.location.href = url + "?p=" + GetEncryptValue;
            }

            if (AccessAuth == "True") {
                $("#imgAdd").show();
                $("#imgMod").show();
                $("#imgDel").show();
            }
        }
        else if (nodeValue[0] == "p") {
            ParamArray = {
                'projectID': node.ID
            };

            var url = "../Project/Default.aspx";
            CallEncrypt(CallGetParam());
            parent.location.href = url + "?p=" + GetEncryptValue;
        }
    }
}

/**********************************************************************************************
폴더 추가 버튼 감추기
**********************************************************************************************/
function ResetButton() {
    $("#imgAdd").hide();
    $("#imgMod").hide();
    $("#imgDel").hide();
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
    var url = "/DeskPlusEIP/PMS/Ws/Folder/Tree.asmx/GetSubFolderCount";

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