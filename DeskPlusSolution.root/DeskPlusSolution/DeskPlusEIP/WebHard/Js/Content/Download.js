Download = {}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Download.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        __addEventListener(window, "load", Download.Init);

    });
});

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : ActiveX로드
***************************************************/
Download.Init = function () {
    setTimeout("Download.InitDpUpload();", 300);
    setTimeout("Download.LoadFileList();", 300);
    __addEventListener(document, "click", Download.Click);
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : ActiveX로드
***************************************************/
Download.InitDpUpload = function () {
    form1.DPUpload.SetTitleDesktop = R_UploadDesktop;
    form1.DPUpload.SetTitleMyDocuments = R_UploadDocuments;
    form1.DPUpload.SetTitleMyComputer = R_UploadComputer;
    form1.DPUpload.SetTitleMyNetworkPlaces = R_UploadNetworkPlaces;
    form1.DPUpload.SetStartFolder = "z";
    //Form1.DPUpload.RunCreateInit();
    try {
        setTimeout("form1.DPUpload.RunCreateInit();", 300);
    } catch (e) {
        alert(e);
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 파일 리스트 가져오기
***************************************************/
Download.LoadFileList = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/Download.asmx/GetFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileID': FileList,
        'order': Order    
    };

    var obj = eval("Download.SuccessFileList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-09
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Download.SuccessFileList = function (data, status) {

    var shtml = "";
    $("#divFileList").empty();
    $("#divFileList").removeClass("on");

    for (var i = 0; i < data.d.length; i++) {
        shtml = "<span action='attach' class='attach btnG' id='" + data.d[i].FileID + "' CONTENTEDITABLE=false FilePath='" + data.d[i].FilePath + "' >" + data.d[i].FileName + "</span>";
        AttachFileName += data.d[i].FileName + ":";
        AttachFilePath += data.d[i].FilePath + ":";
        $("#divFileList").append(shtml);
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 클릭 이벤트 분기
***************************************************/
Download.Click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Download.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Download.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var fileID = el.getAttribute("id");
    var filePath = el.getAttribute("FilePath");

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();
    oItem = oContextMenuCtrl.addItem(R_DelFile, Download.Del, "");
    oItem.txt.setAttribute("arg", fileID, 0);
    oItem.txt.setAttribute("filePath", filePath, 0);

    oContextMenuCtrl.show(e);
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 첨부파일 리스트 삭제
***************************************************/
Download.Del = function (e) {
    var evt = new __event(e);
    var el = evt.target;
    if (el) {

        var tmpFileList = "";
        var tmpFileName = "";
        var tmpFilePath = "";
        var attachArr = FileList.split(",");
        var attachFileNameArr = AttachFileName.split(":");
        var attachFilePathArr = AttachFilePath.split(":");


        for (i = 0; i < attachArr.length - 1; i++) {
            if (attachArr[i] != el.getAttribute("arg")) {
                tmpFileList += attachArr[i] + ",";
                tmpFileName += attachFileNameArr[i] + ":";
                tmpFilePath += attachFilePathArr[i] + ":";
            }
        }

        FileList = tmpFileList;
        AttachFileName = tmpFileName;        
        AttachFilePath = tmpFilePath;

        Download.LoadFileList();
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2011-11-04
작성목적 : 파일 다운로드
***************************************************/
Download.Confirm = function () {

    //파일이 있는지 확인
    if (FileList == "") {
        alert(R_NotFoundFile);
        return;
    }

    var attachArr = FileList.split(",");
    var attachFileNameArr = AttachFileName.split(":");
    var attachFilePathArr = AttachFilePath.split(":");
    var localPath = form1.DPUpload.GetDownLoadClickFolderPath();

    //다운로드 할 폴더를 선택했는지 확인
    if (localPath == "") {
        alert(R_NotFoundPath);
        return false;
    }


    form1.DPUpload.GetFileBrowserIndex = 0;
    form1.DPUpload.SetInitializeDown();
    form1.DPUpload.SetServer = UploadHost;

    for (i = 0; i < attachArr.length - 1; i++) {
        form1.DPUpload.SetServerFullPath = attachFilePathArr[i];
        form1.DPUpload.SetLocalFullPath = localPath + attachFileNameArr[i];
    }

    try {
        var result = form1.DPUpload.RunDownload();

        if (result == "0") {
            alert(R_CompleteConfirm);
        } else {
            alert(result);
        }
    }
    catch (e) {
        alert(e);
    }
}

