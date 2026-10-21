View = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //파일백업
        $('#cbtnfileBackUp').click(function (e) {
            e.preventDefault();
            View.FileBackUp();
        });

        $('#cbtnfileBackUp_F').click(function (e) {
            e.preventDefault();
            View.FileBackUp();
        });

        //조회현황 클릭
        $('#cbtnReadList').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        $('#cbtnReadList_F').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        //전달확인 클릭
        $('#cbtnForwardList').click(function (e) {
            e.preventDefault();
            View.ForwardList();
        });

        $('#cbtnForwardList_F').click(function (e) {
            e.preventDefault();
            View.ForwardList();
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

        //답변작성 페이지 보기
        $('#cbtnOpen').click(function (e) {
            e.preventDefault();
            View.OpenReply();
        });

        $('#cbtnOpen_F').click(function (e) {
            e.preventDefault();
            View.OpenReply();
        });

        //완료취소
        $('#cbtnCompCancel').click(function (e) {
            e.preventDefault();
            View.CompCancel();
        });

        $('#cbtnCompCancel_F').click(function (e) {
            e.preventDefault();
            View.CompCancel();
        });

        //완료화면 이동
        $('#cbtnComplete').click(function (e) {
            e.preventDefault();
            View.Complete();
        });

        $('#cbtnComplete_F').click(function (e) {
            e.preventDefault();
            View.Complete();
        });

        //전달하기
        $('#cbtnForward').click(function (e) {
            e.preventDefault();
            View.Forward();
        });

        $('#cbtnForward_F').click(function (e) {
            e.preventDefault();
            View.Forward();
        });

        //변경 저장하기
        $('#cbtnIngDetail').click(function (e) {
            e.preventDefault();
            View.IngDetail();
        });

        $('#cbtnIngDetail_F').click(function (e) {
            e.preventDefault();
            View.IngDetail();
        });

        //접수화면
        $('#cbtnAccept').click(function (e) {
            e.preventDefault();
            View.Accept();
        });

        $('#cbtnAccept_F').click(function (e) {
            e.preventDefault();
            View.Accept();
        });

        //재검토
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            View.Reject();
        });

        $('#cbtnReject_F').click(function (e) {
            e.preventDefault();
            View.Reject();
        });

        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        $('#cbtnList_F').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        //답글 추가
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Save();
        });

        //전자결재 근거문서 추가
        $('#cbtnAddApp').click(function (e) {
            e.preventDefault();
            View.AddApp();
        });

        // 업무협업 근거문서 추가
        $('#cbtnAddTask').click(function (e) {
            e.preventDefault();
            View.AddTask();
        });

        // 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            View.AttachSwfFile();
        });

        //답변작성 페이지 감추기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            $("#Answer_Area").hide();
            $('#cbtnOpen').show();
            $('#cbtnOpen_F').show();
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

        //수신자추가
        $('#cbtnReceive').click(function (e) {
            e.preventDefault();

            EleDom = "InputTO";

            ParamArray = {
                'callBackEvent': "SetUserDept",
                'gubun': "2"
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Task/Common/GetUserDept.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });

        //참조자추가
        $('#cbtnReference').click(function (e) {
            e.preventDefault();

            EleDom = "InputCC";

            ParamArray = {
                'callBackEvent': "SetUserDept"
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Task/Common/GetUserDept.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = document.body.scrollHeight;
            $("html, body").animate({ scrollTop: pos }, 100);

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
            */
        }, 1000);

        __addEventListener(document, "click", View.click);

        View.InitResource();
        //답변글 감추기
        $("#Answer_Area").hide();
        $('#cbtnDetailOpen').hide();
    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
View.InitResource = function () {

    if ((Status == "2") || (Status == "3") || (Status == "6")) {
        $("#contextReceive").show();
        $("#contextReference").show();
        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        if (ReceiveName != "") {
            sHtml = View.SetUserDeptDiv(ReceiveName, "TO");
            var targetObj = document.getElementById("InputTO");
            targetObj.innerHTML = sHtml;
        }

        if (ReferenceName != "") {
            sHtml = View.SetUserDeptDiv(ReferenceName, "CC");
            var targetObj = document.getElementById("InputCC");
            targetObj.innerHTML = sHtml;
        }
    }
    else {
        $("#contextReceive").hide();
        $("#contextReference").hide();
        $("#ctbStartDate").hide();
        $("#ctbEndDate").hide();
    }

    
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");

    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseAppListData") != undefined) { $(".NoBaseAppListData").text(R_NoContent); }
    if ($(".NoBaseTaskListData") != undefined) { $(".NoBaseTaskListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
작성목적 : 업무협업 근거문서 추가
**********************************************************************************************/
View.AddTask = function () {
    ParamArray = {
        'baseTasks': BaseTasks,
        'groupID': GroupID
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Task/Common/BaseTaskList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachBaseTask", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
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
작성일자 : 2015-04-20
작성목적 : 답변글수정
**********************************************************************************************/
View.Mod = function (p_listID, p_groupID) {
    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'listID': p_listID,
        'groupID': p_groupID,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isType': IsType,
        'impoUserYN': ImpoUserYN,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());

    location = "/DeskPlusEIP/Task/Common/ModListN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 답변글 삭제 (웹서비스 호출)
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
        var url = "/DeskPlusEIP/Task/Ws/Content/Receive/View.asmx/Del";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 답변글 삭제 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessDel = function (data, status) {
    if (data.d == true) {
        ParamArray = {
            'currentPage': CurrentPage,
            'searchStatus': SearchStatus,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'searchDateValue': SearchDateValue,
            'listID': ListID,
            'groupID': GroupID,
            'itemID': ItemID,
            'itemSubID': ItemSubID,
            'isType': IsType,
            'impoUserYN': ImpoUserYN,
            'isGubun': IsGubun
        };

        CallEncrypt(CallGetParam());

        location = "ViewN.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 완료취소
**********************************************************************************************/
View.CompCancel = function () {
    if (window.confirm(R_MsgCompCancel)) {
        //2번 이상 클릭 불가
        $("#cbtnCancel").attr('onclick', '').unbind('click');
        __doPostBack('LBTNProcess', 'COMPCANCEL');
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
View.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": View.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
View.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "View.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 부서/사용자 삭제
***************************************************/
View.DelItem = function (id) {
    $("#" + id).remove();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
View.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 사용자 추가값 가져오기 
***************************************************/
function SetUserDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        var targetObj = document.getElementById(EleDom);
        var sHtml = targetObj.innerHTML;
        var sFailueList = "";

        var response = unescape(rtnVal);
        var tmpArr = response.split(";");

        var type = "TO";
        if (EleDom == "InputCC") {
            type = "CC";
        }

        //구분$OwnerID$DeptID$이름
        for (var i = 0; i < tmpArr.length; i++) {
            var tArr = tmpArr[i].split("$");

            gubun = tArr[0];
            oid = tArr[1];
            did = tArr[2];
            tname = tArr[3];

            if (oid != "") {
                if (!View.ExistDeptUser(gubun + "_" + oid + "_" + type, EleDom)) {
                    if (gubun == "G") {
                        sHtml += " <a href='#' action='del' id='G_" + oid + "_" + type + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
                    }
                    else {
                        sHtml += " <a href='#' action='del' id='M_" + oid + "_" + type + "' nowrap type='" + gubun + "' oid='M_" + oid + "_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
                    }
                }
                else {
                    if (sFailueList.length > 0) sFailueList += "','";
                    sFailueList += tname;
                }
            }
        }

        targetObj.innerHTML = sHtml;

        if (sFailueList.length > 0) {
            alert(R_MsgChangeSave + "\n\n" + R_AleadyRegisted + "\n'" + sFailueList + "'");
        }
        else {
            alert(R_MsgChangeSave);
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 지정되어 있는 수신자, 참조자 정보 설정하기 
***************************************************/
View.SetUserDeptDiv = function (rtnVal, type) {

    var sHtml = "";

    //isGroup + "‡" + userID  + "‡" + deptID + "‡" + name;
    var sList = unescape(rtnVal);
    var tmpArr = sList.split("‡");

    var groupArr = tmpArr[0].split(",");
    var uidArr = tmpArr[1].split(",");
    var didArr = tmpArr[2].split(",");
    var nameArr = tmpArr[3].split(",");

    for (var i = 0; i < uidArr.length; i++) {
        uid = uidArr[i];
        did = didArr[i];
        tname = nameArr[i];
        gubun = groupArr[i];

        if (uid != "") {
            if (gubun == "G") {
                sHtml += " <a href='#' action='del' id='G_" + did + "_" + type + "' nowrap type='" + gubun + "' oid='G_0_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
            }
            else {
                sHtml += " <a href='#' action='del' id='M_" + uid + "_" + type + "' nowrap type='" + gubun + "' oid='M_" + uid + "_" + did + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + tname + "\">" + tname + ",</a>";
            }
        }
    }

    return sHtml;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
View.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }

    return sList;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-28
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
View.ModList = function (target) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                sList += oChildrens[i].innerText;
            }
        }
    }

    return sList;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-28
작성목적 : 값 비교
***************************************************/
View.ModExistDeptUser = function (_name, _nameTotal) {

    if (_nameTotal.indexOf(_name) > -1) return false;

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-28
작성목적 : 변경저장 
***************************************************/
View.IngDetail = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbStartDate", R_InputStartDate, true);
    validator.checkRequired("ctbEndDate", R_InputEndDate, true);
    if (!validator.validate()) return;

    var getStartDate = $("#ctbStartDate").val();
    var getEndDate = $("#ctbEndDate").val();
    var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

    if (dtResponse == false) {
        alert(R_StartDateEndDate);
        return false;
    }

    $("#hdReceiveID").val(View.GetList("InputTO", "oid"));
    $("#hdReferenceID").val(View.GetList("InputCC", "oid"));

    if ($("#hdReceiveID").val() == "") {
        alert(R_InputReceive);
        return;
    }

    var CheckIngDetail = "N";

    if (StartDate != getStartDate) {
        CheckIngDetail = "Y";
        $("#tbStartDate").val("Y");
    }

    if (EndDate != getEndDate) {
        CheckIngDetail = "Y";
        $("#tbEndDate").val("Y");
    }

    // 수신자 비교
    var tmpTo = View.ModList("InputTO").trimEnds(",");
    var ArrTo = tmpTo.split(',');
    var tmpBeforTo = ReceiveName.split('‡')[3].trimEnds(",");
    $("#hdReceiveName").val(tmpTo);

    for (var i = 0; i < ArrTo.length; i++) {
        if (View.ModExistDeptUser(ArrTo[i], tmpBeforTo)) {
            CheckIngDetail = "Y";
            $("#tbTO").val("Y");
        }
    }

    if ($("#tbTO").val() != "Y") {
        var ArrBeforTo = tmpBeforTo.split(',');

        for (var i = 0; i < ArrBeforTo.length; i++) {
            if (View.ModExistDeptUser(ArrBeforTo[i], tmpTo)) {
                CheckIngDetail = "Y";
                $("#tbTO").val("Y");
            }
        }
    }

    // 참조자 비교
    var tmpCC = View.ModList("InputCC").trimEnds(",");
    $("#hdReferenceName").val(tmpCC);

    if (tmpCC != "") {

        if (ReferenceName == "") {
            CheckIngDetail = "Y";
            $("#tbCC").val("Y");
        }
        else {
            var ArrCC = tmpCC.split(',');
            var tmpBeforCC = ReferenceName.split('‡')[3].trimEnds(",");

            for (var i = 0; i < ArrCC.length; i++) {
                if (View.ModExistDeptUser(ArrCC[i], tmpBeforCC)) {
                    CheckIngDetail = "Y";
                    $("#tbCC").val("Y");
                }
            }

            if ($("#tbCC").val() != "Y") {
                var ArrBeforCC = tmpBeforCC.split(',');

                for (var i = 0; i < ArrBeforCC.length; i++) {
                    if (View.ModExistDeptUser(ArrBeforCC[i], tmpCC)) {
                        CheckIngDetail = "Y";
                        $("#tbCC").val("Y");
                    }
                }
            }
        }
    }
    else {
        if (ReferenceName != "") {
            var tmpBeforCC = ReferenceName.split('‡')[3].trimEnds(",");
            CheckIngDetail = "Y";
            $("#tbCC").val("Y");
        }
    }

    if (CheckIngDetail == "Y") {

        //2번 이상 클릭 불가
        BtnCheck = "Y";
        $("#cbtnIngDetail").attr('onclick', '').unbind('click');

        __doPostBack('LBTNProcess', "IngDetailSave");
    }
    else {
        alert(R_NoHistory);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-12
작성목적 : 재검토
***************************************************/
function AddReject() {

    $("#cbtnReject").attr('onclick', '').unbind('click');
    __doPostBack('LBTNProcess', "REJECT");
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 목록가기 
***************************************************/
View.GoList = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());

    if (ImpoUserYN == "Y") {
        location = "/DeskPlusEIP/Task/ImpoUser/List.aspx?p=" + GetEncryptValue;
    }
    else {
        location = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 파일 백업
***************************************************/
View.FileBackUp = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    $("#ifrm")[0].src = "/DeskPlusEIP/Task/Common/CreateZipFile.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 조회 현황
***************************************************/
View.ReadList = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ReadList.aspx?p=" + GetEncryptValue;
    PopupWin(500, 390, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 전달 현황
***************************************************/
View.ForwardList = function () {

    ParamArray = {
        'listID': ListID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Common/ForwardList.aspx?p=" + GetEncryptValue;
    PopupWin(800, 390, url, "", "", true);
}


/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 답변하기
***************************************************/
View.OpenReply = function () {

    var position = $('#btn_Answer_Area').offset();
    $('html, body').animate({ scrollTop: position.top }, 100);

    $("#Answer_Area").show();
    $('#cbtnOpen').hide();
    $('#cbtnOpen_F').hide();
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 완료하기
***************************************************/
View.Complete = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'listID': ListID,
        'groupID': GroupID,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isType': IsType,
        'impoUserYN': ImpoUserYN,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());

    location = "/DeskPlusEIP/Task/Common/AddCompleteN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 전달하기
***************************************************/
View.Forward = function () {

    ParamArray = {
        'gubun': "2",
        'listID': ListID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Common/ForwardUser.aspx?p=" + GetEncryptValue;
    PopupWin(800, 485, url, "-500", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 접수하기
***************************************************/
View.Accept = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'listID': ListID,
        'groupID': GroupID,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isType': IsType,
        'impoUserYN': ImpoUserYN,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());

    location = "AddAcceptN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-11
작성목적 : 재검토요청
***************************************************/
View.Reject = function () {

    ParamArray = {
        'callBackEvent': "AddReject"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Content/Receive/AddReject.aspx?p=" + GetEncryptValue;
    PopupWin(400, 350, url, "-500", "", true);
}

