View = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'order': Order,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());
            location = "List.aspx?p=" + GetEncryptValue;
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'order': Order,
                'listID': ListID,
                'groupID': GroupID,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());

            if ($.browser.msie) {
                location = "Mod.aspx?p=" + GetEncryptValue;
            } else {
                location = "ModN.aspx?p=" + GetEncryptValue;
            }
        });

        //답글 추가
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Save("REPLY");
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            if (window.confirm(R_ApplyDelete)) {
                //2번 이상 클릭 불가
                $("#cbtnDel").attr('onclick', '').unbind('click');
                __doPostBack('LBTNProcess', "DEL");
            }
        });

        //재요청
        $('#cbtnReCall').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'order': Order,
                'listID': ListID,
                'groupID': GroupID,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());

            if ($.browser.msie) {
                location = "AddRecall.aspx?p=" + GetEncryptValue;
            } else {
                location = "AddRecallN.aspx?p=" + GetEncryptValue;
            }
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //전자결재 근거문서 추가
        $('#cbtnDocAdd').click(function (e) {
            e.preventDefault();
            View.DocAdd();
        });

        // 업무협업 근거문서 추가
        $('#cbtnCoopAdd').click(function (e) {
            e.preventDefault();
            View.CoopAdd();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            View.AttachSwfFile();
        });

        //답변작성 페이지 보기
        $('#cbtnOpen').click(function (e) {
            e.preventDefault();

            var position = $('#cbtnOpen').offset();
            $('html, body').animate({ scrollTop: position.top }, 100);

            $("#Answer_Area").show();
            $('#cbtnOpen').hide();
        });

        //답변작성 페이지 감추기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            $("#Answer_Area").hide();
            $('#cbtnOpen').show();
        });

        //전달하기
        $('#cbtnForward').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'gubun': "2"
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Coop/Common/GetUserList.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });

        //완료취소
        $('#cbtnCompCancel').click(function (e) {
            e.preventDefault();
            View.CompCancel();
        });

        setTimeout(function () {

            if (NonActiveX == "T") {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
                $('#ActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "View.aspx?" + ClientQueryString;
                    }
                });
            } else {
                $('#NonActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "ViewN.aspx?" + ClientQueryString;
                    }
                });
            }
        }, 1000);

        View.GetSpoolFileList();
        View.Resource();
        //답변글 감추기
        $("#Answer_Area").hide();
        View.ReLoadTree();
    });
});

/**************************************************
좌측 메뉴 reload
***************************************************/
View.ReLoadTree = function () {
    ParamArray = {
        'targetPage': "Request,Content/Request/List.aspx",
        'itemID': ItemID,
        'reLoad': '1'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Coop/Category/Tree.aspx?p=" + GetEncryptValue;
    parent.frmLeft.location = url;
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
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Coop/Ws/Content/Request/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Request/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
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
        var obj = eval("View.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachFileID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': ListID,
            'attachID': p_attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessFileList");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/View.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessFileList = function (data, status) {
    if (data.d == true) {
        View.GetAttachFileList();
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 근거문서 추가
**********************************************************************************************/
View.DocAdd = function () {
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
    WindowObjectReference = window.open(url, "AttachBaseDoc", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
업무협업 근거문서 추가
**********************************************************************************************/
View.CoopAdd = function () {
    ParamArray = {
        'folderName': "dept",
        'coopBaseIDs': CoopBaseIDs
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Coop/Content/Request/GetListBase.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachCoopBaseDoc", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($("#j_spool_screenshot")[0] != undefined) {
        $("#j_spool_screenshot")[0].innerHTML = R_ScreenShot;
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoContent); }
    if ($(".NoBaseCoopListData") != undefined) { $(".NoBaseCoopListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/doc_view.gif'); }
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

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
View.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'COOP',
        'callBackEvent': "View.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());
    //var url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?p=" + GetEncryptValue;
    //PopupWinOpen(680, 510, url, "", "", true);

    //    var url;
    //    if ($.browser.msie) {
    //        url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx";
    //    } else {
    //        url = "/DeskPlusEIP/Upload/SilverUpload/Main.aspx";
    //    }

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 답변글 - 수정
**********************************************************************************************/
View.Mod = function (p_listID, p_groupID) {
    ParamArray = {
        'listCount': ListCount,
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'order': Order,
        'listID': p_listID,
        'groupID': p_groupID,
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());

    if ($.browser.msie) {
        location = "ModReply.aspx?p=" + GetEncryptValue;
    } else {
        location = "ModReplyN.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 답변 글 삭제
**********************************************************************************************/
View.Del = function (p_listID, p_groupID) {
    if (window.confirm(R_DelReply)) {
        ParamArray = {
            'listID': p_listID,
            'groupID': p_groupID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDel");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/View.asmx/Del";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 답변 글 삭제
**********************************************************************************************/
View.SuccessDel = function (data, status) {
    if (data.d == true) {
        ParamArray = {
            'listCount': ListCount,
            'currentPage': CurrentPage,
            'searchStatus': SearchStatus,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'searchDateValue': SearchDateValue,
            'order': Order,
            'listID': ListID,
            'groupID': GroupID,
            'itemID': ItemID,
            'itemSubID': ItemSubID
        };

        CallEncrypt(CallGetParam());

        if ($.browser.msie) {
            location = "View.aspx?p=" + GetEncryptValue;
        } else {
            location = "ViewN.aspx?p=" + GetEncryptValue;
        }
    }
}

/**********************************************************************************************
[저장]
**********************************************************************************************/
View.Save = function (p_gubun) {
    //    var validator = new Validator($("#form1"));
    //    validator.checkRequired("tbxTitle", R_InputTitle, true);
    //    if (!validator.validate()) return;

    //전자결재 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseCoopIDs").val(CoopBaseIDs);

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
    $("#cbtnReply").attr('onclick', '').unbind('click');

    __doPostBack('LBTNProcess', p_gubun);
}

/**************************************************
작성자   :
작성일자 : 심재운
작성목적 : 사용자 모달창 띄우기
***************************************************/
function GetDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);

        var tmpArr = response.split("‡");
        var idArr = tmpArr[0].split(",");

        ParamArray = {
            'groupID': GroupID,
            'userList': idArr
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.GetDept");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Receive/ViewComplete.asmx/AddForward";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 전달사용자 받아서 처리하기
**********************************************************************************************/
View.GetDept = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
    }
    else {
        alert(R_MSG_FAIL);
    }
}

/**********************************************************************************************
[완료취소]
**********************************************************************************************/
View.CompCancel = function () {
    if (window.confirm("'처리진행' 으로 상태 변경됩니다. \n정말로 완료 취소하시겠습니까?")) {
        //2번 이상 클릭 불가
        $("#cbtnCancel").attr('onclick', '').unbind('click');
        __doPostBack('LBTNProcess', 'COMPCANCEL');
    }
}