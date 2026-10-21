CopyMove = {}
CopyMove.ContentFrame = parent.parent.frmContent;
CopyMove.TreeFrame = parent.parent.frmLeft;

/**************************************************
작성자   : famo23
작성일자 : 2012-01-20
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        if (Gubun == "COPY") {
            setTimeout("CopyMove.Copy();", 300);            
        }
        else if (Gubun == "MOVE") {
            setTimeout("CopyMove.Move();", 300);
        }

    });
});

/**************************************************
작성자 : famo23
작성일자 : 2012-01-20
작성목적 : 복사
***************************************************/
CopyMove.Copy = function () {

    //alert(FolderID + "/" + FolderID + "/" + RealFolderID + "/" + FileList + "/" + UID + "/" + Order + "/" + Types);

    ParamArray = {
        'folderID': FolderID,
        'realFolderID': RealFolderID,
        'fileList': FileList,
        'uID': UID,
        'order': Order,
        'types': Types
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("CopyMove.SuccessMoveCopy");
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/CopyMove.asmx/CopyFile";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : famo23
작성일자 : 2012-01-20
작성목적 : 이동
***************************************************/
CopyMove.Move = function () {   

    ParamArray = {
        'folderID': FolderID,
        'realFolderID': RealFolderID,
        'fileList': FileList,
        'uID': UID,
        'order': Order,
        'types': Types
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("CopyMove.SuccessMoveCopy");
    var url = "/DeskPlusEIP/WebHard/Ws/Folder/CopyMove.asmx/MoveFile";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-20
작성목적 : 파일 이동, 복사 (웹서비스 값 받기) -> 총 사용 용량 가져오기
**********************************************************************************************/
CopyMove.SuccessMoveCopy = function (data, status) {

    var rtn = data.d

    if (rtn == "1") {
        if (Types == "U") {
            var url = "/DeskPlusEIP/WebHard/Ws/Content/List.asmx/GetPublicUseCount";

            ParamArray = {
                'folderID': FolderID,
                'types': Types
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("CopyMove.SuccessLoadPublic");

            AjaxService(url, data, obj);
        }
        else {
            var obj = eval("CopyMove.SuccessLoad");
            var url = "/DeskPlusEIP/WebHard/Ws/Folder/CopyMove.asmx/GetUseCount";

            AjaxService(url, "", obj);
        }
    }
    else if (rtn == "2") {
        //파일용량
        alert(R_PublicFileCount);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (rtn == "3") {
        //폴더용량
        alert(R_PublicFolderCount);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-20
작성목적 : 파일 이동 및 복사 한 후에 페이지 로드
**********************************************************************************************/
CopyMove.SuccessLoad = function (data, status) {

    alert(R_CompleteConfirm);

    // 트리의 그래픽 바 , 퍼센트 보여주기
    var tmpUseCount = data.d;
    var tmpPercentCount = "0";

    if (tmpUseCount == "0") {
        tmpPercentCount = "0";
    }
    else if (PrivateQuota == "0") {
        tmpPercentCount = "0";
    }
    else {
        tmpPercentCount = ((tmpUseCount / (PrivateQuota * 1048576)) * 100);

        if ((0 < tmpPercentCount) && (tmpPercentCount < 1)) {
            tmpPercentCount = "0.1";
        }
        else {
            tmpPercentCount = tmpPercentCount.toFixed(1);
        }
    }

    if (tmpPercentCount > 100) {
        tmpPercentCount = 100;
    }

    // Format 맞추어서 사용량 보여 주기 
    if (tmpUseCount < 1024) {
        if (tmpUseCount == 0) {
            tmpUseCount = "0 B";
        }
        else {
            tmpUseCount = tmpUseCount.toFixed(1) + "B";
        }
    }
    else if (tmpUseCount < 1024 * 1024) {
        tmpUseCount = (tmpUseCount / 1024).toFixed(1) + "KB";
    }
    else if (tmpUseCount < 1024 * 1024 * 1024) {
        tmpUseCount = (tmpUseCount / (1024 * 1024)).toFixed(1) + "MB";
    }
    else {
        tmpUseCount = (tmpUseCount / (1024 * 1024 * 1024)).toFixed(1) + "GB";
    }

    CopyMove.TreeFrame.$("#userTotal").text(tmpUseCount);
    CopyMove.TreeFrame.$("#percent").text(tmpPercentCount + "%");
    CopyMove.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    if (ListType == "1") {
        CopyMove.ContentFrame.List.Search();
    }
    else if (ListType == "2") {
        CopyMove.ContentFrame.ListIcon.Search();
    }
    else if (ListType == "3") {
        CopyMove.ContentFrame.ListPreview.Search();
    }
    else {
        CopyMove.ContentFrame.List.Search();
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-07-01
작성목적 : 폴더용량 확인
**********************************************************************************************/
CopyMove.SuccessLoadPublic = function (data, status) {

    alert(R_CompleteConfirm);

    var folderData = data.d;
    var PUseCount = 0;
    var PPrivateQuota = 0;
    var PDisPlayQuota = 0;

    PUseCount = folderData.split("|")[1];
    PPrivateQuota = folderData.split("|")[0];

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

    CopyMove.TreeFrame.$("#userTotal").text(tmpUseCount);
    CopyMove.TreeFrame.$("#percent").text(tmpPercentCount + "%");
    CopyMove.TreeFrame.$("#graphBar").width(tmpPercentCount + "%");

    // 페이지 리스트 호출 
    if (ListType == "1") {
        CopyMove.ContentFrame.List.Search();
    }
    else if (ListType == "2") {
        CopyMove.ContentFrame.ListIcon.Search();
    }
    else if (ListType == "3") {
        CopyMove.ContentFrame.ListPreview.Search();
    }
    else {
        CopyMove.ContentFrame.List.Search();
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


