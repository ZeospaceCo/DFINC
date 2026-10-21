TotalDownLoad = {}

/**************************************************
작성자   : 이현수
작성일자 : 2008-09-12
작성목적 : 파일 다운로드
***************************************************/
TotalDownLoad.DownLoad = function() {

    //파일이 있는지 확인
    if (FileList == "") {
        alert(MsgNotFoundFile);
        return;
    }

    var attachArr = FileList.split("|");
    var attachFile = "";
    var attachFileName = "";
    var attachFilePath = "";
    var localPath = Form1.DPUpload.GetDownLoadClickFolderPath();

    //다운로드 할 폴더를 선택했는지 확인
    if (localPath == "") {
        alert(MsgNotFoundPath);
        return false;
    }

    Form1.DPUpload.GetFileBrowserIndex = 0;
    Form1.DPUpload.SetInitializeDown();
    Form1.DPUpload.SetServer = UploadHost;
    
    for (i = 0; i < attachArr.length; i++) {
        attachFile = attachArr[i].split(":");
        attachFileName = attachFile[0];
        attachFilePath = attachFile[1];

        Form1.DPUpload.SetServerFullPath = attachFilePath;
        Form1.DPUpload.SetLocalFullPath = localPath + attachFileName;

        //alert(Form1.DPUpload.SetServerFullPath + "  :  " + Form1.DPUpload.SetLocalFullPath);
    }

    try {
        var result = Form1.DPUpload.RunDownload();
        if (result == "0") {

        } else {
            alert(rtnVal);
        }
    }
    catch (e) {
        alert(e);
    }
}


/**************************************************
 작성자   : 이현수
 작성일자 : 2008-08-11
 작성목적 : 초기화
***************************************************/
TotalDownLoad.Init = function() {
    setTimeout("TotalDownLoad.InitDpUpload();", 300);
    setTimeout("TotalDownLoad.LoadFileList();", 300);
    __addEventListener(document, "click", TotalDownLoad.click);
 }
 
/**************************************************
 작성자   : 이현수
 작성일자 : 2008-08-11
 작성목적 : 업로드 초기화
***************************************************/
 TotalDownLoad.InitDpUpload = function() {
     Form1.DPUpload.SetTitleDesktop = MsgUploadAttachFileDesktop;
     Form1.DPUpload.SetTitleMyDocuments = MsgUploadAttachFileMyDocuments;
     Form1.DPUpload.SetTitleMyComputer = MsgUploadAttachFileMyComputer;
     Form1.DPUpload.SetTitleMyNetworkPlaces = MsgUploadAttachFileMyNetworkPlaces;
     Form1.DPUpload.SetStartFolder = "z";
     //Form1.DPUpload.RunCreateInit();
     try {
         setTimeout("Form1.DPUpload.RunCreateInit();", 300);
     } catch (e) {
         alert(e);
     }
 }

/**************************************************
작성자   : 이현수
작성일자 : 2008-09-12
작성목적 : 파일 리스트 가져오기
***************************************************/
 TotalDownLoad.LoadFileList = function () {
     var attachArr = FileList.split("|");
     var attachFile = "";
     var attachFileName = "";
     var attachFilePath = "";
     var shtml = "";

     for (i = 0; i < attachArr.length; i++) {
         attachFile = attachArr[i].split(":");
         attachFileName = attachFile[0];
         attachFilePath = attachFile[1];
         shtml += "<span action='attach' class='attach btnG' attachFile='" + attachFile + "' CONTENTEDITABLE=false id='" + attachFilePath + "' name='P' >" + attachFileName + "</span>";

     }
     
     $("#divFileList").html(shtml);
 }



/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
TotalDownLoad.click = function(e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": TotalDownLoad.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;
/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
TotalDownLoad.ShowAttachContextMenu = function(e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var attachFile = el.getAttribute("attachFile");
    var fileID = el.getAttribute("id");

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();
    oItem = oContextMenuCtrl.addItem(MsgDelFile, TotalDownLoad.Del, "");
    oItem.txt.setAttribute("arg", fileID, 0);
    
    oContextMenuCtrl.show(e);
}

TotalDownLoad.Del = function (e) {

    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        var tmpFileList = "";
        var obj = document.getElementById(arg);
        if (obj.name == "P") {
            var attachArr = FileList.split("|");
            var attachFile = "";
            var attachFileName = "";
            var attachFilePath = "";

            for (i = 0; i < attachArr.length; i++) {
                attachFile = attachArr[i].split(":");
                attachFileName = attachFile[0];
                attachFilePath = attachFile[1];

                if (attachFilePath != document.getElementById(arg).id) {
                    tmpFileList += attachArr[i] + "|";
                }
            }

            FileList = tmpFileList.substr(0, tmpFileList.length - 1);
            var objR = document.getElementById(obj.id);
            objR.removeNode(true);
        }
    }
}

$(function () {
    $(document).ready(function () {
        TotalDownLoad.Init();

        //확인
        $('#CbtnSubmit').click(function (e) {
            e.preventDefault();
            TotalDownLoad.DownLoad();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });
    });
});
