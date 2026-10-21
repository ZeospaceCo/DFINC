Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //추가
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //수정
        $('#CbtnMod').click(function (e) {
            e.preventDefault();
            Add.Edit();
        });

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            Add.Del();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBMultiName').attr("disabled", true);
    });
});

/**********************************************************************************************
[추가]
**********************************************************************************************/
Add.Add = function () {
    var folderID = $("#tbxFolderID").val();
    var folderName = $("#tbxFolderName").val();

    if (folderName != "") {
        $('#CTBMultiName').attr("disabled", false);
        __doPostBack('lbtnSubmit', 'add');
    }
    else {
        alert(FOLDER_Register_MSGName);
        $("#tbxFolderName").focus();
    }
}

/**********************************************************************************************
[수정]
**********************************************************************************************/
Add.Edit = function () {
    $('#CTBMultiName').attr("disabled", false);

    var folderID = $("#tbxFolderID").val();
    var folderName = $("#tbxFolderName").val();

    if (folderID != "" && folderName != "") {
        __doPostBack('lbtnSubmit', 'edit');
    }
}

/**********************************************************************************************
[삭제]
**********************************************************************************************/
Add.Del = function () {
    var folderID = $("#tbxFolderID").val();
    if (folderID != "") {
        __doPostBack('lbtnSubmit', 'del');
    }
}

/**********************************************************************************************
[폴더트리]
**********************************************************************************************/
function FolderScript_OnNodeSelect(node) {
    if (node.ID == "-1") {
        $("#tbxFolderID").val("0");
    }
    else {
        $("#tbxFolderID").val(node.ID);
    }

    $("#CTBMultiName").val(node.Value.split("|")[4]);
    $("#tbxFolderName").val(node.Value.split("|")[5]);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-18
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiName").val(multiNames);
}