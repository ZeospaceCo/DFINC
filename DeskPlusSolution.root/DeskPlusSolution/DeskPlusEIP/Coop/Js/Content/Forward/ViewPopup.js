View = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //메인 페이지이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=coop";
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        View.Init();
        View.Resource();
    });
});

/**********************************************************************************************
초기화
**********************************************************************************************/
View.Init = function () {
    //수정모드일경우
    if (ForwardListID != "0") {
        View.GetAttachFileList();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
    //$("#j_spool_fileName")[0].innerHTML = R_FileName;
    //$("#j_spool_fileType")[0].innerHTML = R_FileType;
    //$("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    //$("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($("#j_spool_screenshot")[0] != undefined) {
        $("#j_spool_screenshot")[0].innerHTML = R_ScreenShot;
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoBaseListData); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/icon_search.gif'); }
}

/**********************************************************************************************
## 첨부파일 관련 시작
**********************************************************************************************/

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
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'forwardListID': ForwardListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Coop/Ws/Content/Forward/ViewPopup.asmx/GetForwardAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Forward/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);
}