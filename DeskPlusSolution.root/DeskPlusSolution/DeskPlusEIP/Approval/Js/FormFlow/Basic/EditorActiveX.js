EditorActiveX = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        EditorActiveX.Init();
    });
});

EditorActiveX.Init = function () {

    if ($("#EDITOR", parent.document) != null) {
        var w = $("#EDITOR", parent.document)[0].style.width;
        var h = $("#EDITOR", parent.document)[0].style.height;

        // 에디터의 높이를 부모값의 높이로 강제지정.(100%로 주는경우, 반만 보여지는 버그처리.)
        if ($("#EDITOR", parent.document)[0].parentNode.height != null) {
            if ($("#EDITOR", parent.document)[0].parentNode.height != "") {
                h = $("#EDITOR", parent.document)[0].parentNode.height;
            }
        }

        $("#twe").attr("weight", w);
        $("#twe").attr("height", h);
    }
}

EditorActiveX.ControlInit = function () {

    if ($("#htaEditorHTML", parent.parent.document).val() != "") {
        form1.twe.bodyValue = TemplateHeader + $("#htaEditorHTML", parent.parent.document).val() + TemplateFooter;
    } else {
        form1.twe.HtmlValue = TemplateHeader + TemplateFooter;
    }
}
EditorActiveX.IsSecurity = function () {
    return EditorSecurity($("#twe")[0]);
}
EditorActiveX.GetHtml = function () {
    var strHtml = $("#twe")[0].HtmlValue;
    if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
        strHtml = strHtml.replace(".js\"", "\"");
        strHtml = strHtml.replace('=\"xss:expression', "");

        $("#twe")[0].HtmlValue = strHtml;
        alert(R_Msg_ErrCont);
    }
    return $("#twe")[0].HtmlValue;
}
EditorActiveX.GetText = function () {
    return $("#twe")[0].TextValue;
}


// 사용하지 않음
EditorActiveX.DPUploadEditor = function () {

    var files = $("#twe")[0].GetLocalFiles();

    if (files != "")  // 본문에 이미지가 첨부되어 있을대
    {
        $("#DPUpload")[0].SetPostFilePathInit(); //저장된 파일 관련 내용 초기화

        var filesList = files.split(";");
        $.each(filesList, function (i, o) {
            if (o.substr(0, 2) != "\\\\") {
                $("#DPUpload")[0].SetPostFilePath(o);
            }
        });

        //AppendFileCount - 파일이 있는 지 갯수 , GetPostFilePathCount - 파일 경로가 있는 지 갯수
        if (($("#DPUpload")[0].AppendFileCount() + $("#DPUpload")[0].GetPostFilePathCount()) > 0) {
            $("#DPUpload")[0].SetFileNotUpload = 0;
            $("#DPUpload")[0].Host = UploadHost;
            $("#DPUpload")[0].Port = UploadPort;
            $("#DPUpload")[0].PostAcceptor = UploadPath + "?path=" + UploadPath + "&cID=" + CompanyID + "&uID=" + UserID;

            $("#DPUpload")[0].Post();
            var rtnVal = $("#DPUpload")[0].ServerRespones;
            if (rtnVal == "") {
                $("#twe")[0].ChangeLocalFilesUrl(UploadPath + "/");
            } else {
                alert(rtnVal);
            }
        }
    }
}
