View = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        // 닫기 
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //이미지 보기(첨부파일)
        $('#cbtnShowImage').click(function (e) {
            e.preventDefault();
            View.ShowImage();
        });

        //이미지 보기(첨부파일)
        $('#cbtnShowPImage').click(function (e) {
            e.preventDefault();
            View.ShowImage();
        });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); View.ZIPDownLoad(); });

        View.LoadData();
    });
});


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
View.LoadData = function () {

    View.GetAttachFileList();
    View.GetBaseDocList();

    View.Resource();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 근거문서 보기
**********************************************************************************************/
View.ViewBaseDoc = function (_baseDocumnetID) {
    
    ParamArray = { 'documentID': _baseDocumnetID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
    var param = "p=" + GetEncryptValue;
	WindowObjectReference = PopupWinOpen(url, param, "ViewBaseDoc", "1200", "800");
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 다국어
***************************************************/
View.Resource = function () {

    if ($("#j_selectall").length > 0) {
        $("#j_selectall").html(SelectAll);
    }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 이미지 보기 (첨부파일)
**********************************************************************************************/
View.ShowImage = function () {
    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 첨부 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {

    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/CirDocUser.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    if (data.d.length == 0) {
        $("#download").css("display", "none");
        $("#zipline1").css("display", "none");
        $("#zipline2").css("display", "none");
        $("#cbxAll").css("display", "none");
        $("#j_selectall").css("display", "none");
    }

    if ($('#cbtnShowImage').length == 0) {
        $("#zipline2").css("display", "none");
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 근거문서  리스트 호출
**********************************************************************************************/
View.GetBaseDocList = function () {

    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/CirDocUser.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 근거문서 리스트 보여주기
**********************************************************************************************/
View.SuccessBaseDoc = function (data, status) {

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTBaseDoc.htm?seq=' + Guids());
    $('#placeholder_baseDoc').processTemplate(data.d); 
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 근거문서 보기
**********************************************************************************************/
View.ViewBaseDoc = function (_baseDocumnetID) {

    ParamArray = { 'documentID': _baseDocumnetID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
    var param = "p=" + GetEncryptValue;
	WindowObjectReference = PopupWinOpen(url, param, "ViewBaseDoc", "1200", "800");
}

/**********************************************************************************************
ZIP선택다운로드
**********************************************************************************************/
View.ZIPDownLoad = function () {

    if (CirDocumentID != "") {

        var fileIDList = "";
        fileIDList = View.CheckedValue();

        if (fileIDList != "") {
            ParamArray = {
                'module': 'APPROVALCIR'
                , 'listID': CirDocumentID
                , 'fileID': fileIDList
            };
            CallEncrypt(CallGetParam());

            var attachFilePath = "/DeskPlusEIP/Common/Zip/CreateZipFileSelect.aspx?p=" + GetEncryptValue;
            window.open(attachFilePath);
        }
        else {
            alert(R_MSG_SelectDownLoad);
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 선택된 값 가져오기
***************************************************/
View.CheckedValue = function () {
    var allVals = $('.cbxFile:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   :
작성일자 : 
작성목적 : 체크박스(전체)
***************************************************/
View.checkedAllValue = function () {
    if ($("#cbxAll").is(":checked")) {
        $('.cbxFile').attr("checked", true);
    }
    else {
        $('.cbxFile').attr("checked", false);
    }
}