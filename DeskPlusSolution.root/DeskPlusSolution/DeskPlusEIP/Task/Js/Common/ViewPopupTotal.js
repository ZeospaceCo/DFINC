View = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //답변작성 페이지 보기
        $('#cbtnOpen').click(function (e) {
            e.preventDefault();
            View.OpenReply();
        });

        $('#cbtnOpen_F').click(function (e) {
            e.preventDefault();
            View.OpenReply();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        $('#cbtnPrint_F').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });


        //메인 페이지 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            if (window.opener != null) {
                window.opener.parent.frmTop.Menu.GoParam("TASK");
                window.opener.location.href = "/DeskPlusEIP/Task/Default.aspx";
                self.close();
            }
            else {
                location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            }
        });

        $('#cbtnList_F').click(function (e) {
            e.preventDefault();
            if (window.opener != null) {
                window.opener.parent.frmTop.Menu.GoParam("TASK");
                window.opener.location.href = "/DeskPlusEIP/Task/Default.aspx";
                self.close();
            }
            else {
                location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK";
            }
        });


        //상세보기 열기
        $('#cbtnDetailOpen').click(function (e) {
            e.preventDefault();
            $(".detail").show();
            $('#cbtnDetailClose').show();
            $('#cbtnDetailOpen').hide();

            if (IsBase == "") {
                $(".detailEtc").show();
            }
        });

        //상세보기 닫기
        $('#cbtnDetailClose').click(function (e) {
            e.preventDefault();
            $(".detail").hide();
            $(".detailEtc").hide();
            $('#cbtnDetailClose').hide();
            $('#cbtnDetailOpen').show();

        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = document.body.scrollHeight;
            $("html, body").animate({ scrollTop: pos }, 100);

        });

        //답변작성 페이지 감추기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            $("#Answer_Area").hide();
            $('#cbtnOpen').show();
            $('#cbtnOpen_F').show();
        });

        // 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            View.AttachSwfFile();
        });

        //전자결재 근거문서 추가
        $('#cbtnAddApp').click(function (e) {
            e.preventDefault();
            View.AddApp();
        });

        //답글 추가
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Save();
        });

        setTimeout(function () {

            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            /*
            if (NonActiveX == "T") {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
                var agent = navigator.userAgent.toLowerCase();
                if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

                }
                else {
                    $("#ActiveX").hide();
                }
                $('#ActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "ViewPopupTotal.aspx?" + ClientQueryString;
                    }
                });
            } else {
                $('#NonActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "ViewPopupTotalN.aspx?" + ClientQueryString;
                    }
                });
            }
            */
        }, 1000);


        View.InitResource();
        //답변글 감추기
        $("#Answer_Area").hide();
        $('#cbtnDetailOpen').hide();
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 다국어 처리
**********************************************************************************************/
View.InitResource = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;
   
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseAppListData") != undefined) { $(".NoBaseAppListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/doc_view.gif'); }
     */
}


/**************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 첨부파일 추가
***************************************************/
View.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'TASK',
        'callBackEvent': "View.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
View.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').empty();
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.Resource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 임시파일 삭제 처리
**********************************************************************************************/
function DelSpoolPath(fileName) {

    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').empty();
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        View.Resource();
    }, 100);

    var pos = document.body.scrollHeight;
    $("html, body").animate({ scrollTop: pos }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 전자결재 근거문서 추가
**********************************************************************************************/
View.AddApp = function () {
    ParamArray = {
        'folderName': "dept",
        'baseDocIDs': BaseDocIDs
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Approval/FormFlow/Basic/GetBaseDoc.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachBaseApp", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 답변하기
**********************************************************************************************/
View.Save = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    //전자결재 근거문서
    $("#tbBaseApps").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseTasks").val(BaseTasks);

    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_DoubtStcipt);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_NoContent);
        return;
    }

    //2번 이상 클릭 불가
    BtnCheck = "Y";
    $("#cbtnReply").attr('onclick', '').unbind('click');

    __doPostBack('LBTNProcess', "REPLY");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : 답변하기 보이기
**********************************************************************************************/
View.OpenReply = function () {

    var position = $('#btn_Answer_Area').offset();
    $('html, body').animate({ scrollTop: (position.top) }, 100);

    $("#Answer_Area").show();
    $('#cbtnOpen').hide();
    $('#cbtnOpen_F').hide();
}

