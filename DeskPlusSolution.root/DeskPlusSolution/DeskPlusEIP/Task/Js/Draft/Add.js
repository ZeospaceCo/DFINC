Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //대분류 선택
        $('#ddlItem').change(function () {
            Add.GetItemSubList();
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

        // 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        //요청하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        //임시저장
        $('#cbtnDraft').click(function (e) {
            e.preventDefault();
            Add.SaveDraft();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Add.Mod();
        });        

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();

            if (window.confirm(R_Delete)) {
                //2번 이상 클릭 불가
                $("#cbtnDel").attr('onclick', '').unbind('click');
                __doPostBack('lbtnAdd', "DEL");
            }
        });

        // 전자결재 근거문서 추가
        $('#cbtnAddApp').click(function (e) {
            e.preventDefault();
            Add.AddApp();
        });

        // 업무협업 근거문서 추가
        $('#cbtnAddTask').click(function (e) {
            e.preventDefault();
            Add.AddTask();
        });

        //임시저장불러오기
        $('#cbtnDraftList').click(function (e) {
            e.preventDefault();
            Add.AddDraft();
        });

        // 취소하기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();

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

            if (IsType == "RE") {
                location = "/DeskPlusEIP/Task/Content/Request/List.aspx?p=" + GetEncryptValue;
            }
            else {
                location = "List.aspx?p=" + GetEncryptValue;
            }    

        });

        $("#tbxCompleteRequestDate").datepick($.datepicker.regional[PreLang]);
        $('#tbxCompleteRequestDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
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
                        location.href = "Add.aspx?" + ClientQueryString;
                    }
                });
            } else {
                $('#NonActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "AddN.aspx?" + ClientQueryString;
                    }
                });
            }
            */

        }, 1000);

        __addEventListener(document, "click", Add.click);

        Add.Init();
    });
});

/**********************************************************************************************
초기화
**********************************************************************************************/
Add.Init = function () {

    if (ReceiveName != "") {
        sHtml = Add.SetUserDeptDiv(ReceiveName, "TO");
        var targetObj = document.getElementById("InputTO");
        targetObj.innerHTML = sHtml;
    }

    if (ReferenceName != "") {
        sHtml = Add.SetUserDeptDiv(ReferenceName, "CC");
        var targetObj = document.getElementById("InputCC");
        targetObj.innerHTML = sHtml;
    }

    Add.SetSpoolFile();
    Add.GetAttachList();
    Add.GetBaseAppList();
    //Add.GetBaseTaskList();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 다국어 처리
**********************************************************************************************/
Add.SetSpoolFile = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 지정되어 있는 수신자, 참조자 정보 설정하기 
***************************************************/
Add.SetUserDeptDiv = function (rtnVal, type) {

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
작성일자 : 2015-04-24
작성목적 : 소분류 가져오기 (웹서비스 호출)
***************************************************/
Add.GetItemSubList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/GetItemSub";

    var selVal = $('#ddlItem').val();

    ParamArray = {
        'itemID': selVal
    };

    var obj = eval("Add.SuccessGetItemSubList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 소분류 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessGetItemSubList = function (data, status) {

    $("#ddlItemSub option").remove();
    $("#ddlItemSub").append("<option value='0'>" + R_SubCategory + "</option>");

    for (var obj in data.d) {
        var nameBase = data.d[obj].NameBase;
        var vals = data.d[obj].ItemSubID;

        $("#ddlItemSub").append("<option value='" + vals + "'>" + nameBase + "</option>");
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'TASK',
        'callBackEvent': "Add.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Add.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        Add.Resource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 다국어 처리
**********************************************************************************************/
Add.Resource = function () {

    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_filedelete")[0].innerHTML = R_FileDel;

    $("#j_basetitle")[0].innerHTML = R_DocNumber;
    $("#j_basedel")[0].innerHTML = R_Del;

    //$("#j_baseTasktitle")[0].innerHTML = R_DocNumber;
    //$("#j_baseTaskdel")[0].innerHTML = R_Del;   

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseAppListData") != undefined) { $(".NoBaseAppListData").text(R_NoContent); }
    if ($(".NoBaseTaskListData") != undefined) { $(".NoBaseTaskListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    */

    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }

    if ($(".fa-times-circle").length > 0) {
        $(".fa-times-circle").attr("title", R_FileDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Add.GetAttachList = function () {

    ParamArray = {
        'draftID': DraftID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetAttachList");
    var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/GetAttachList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessGetAttachList = function (data, status) {

    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Task/Jt/Draft/JTFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'draftID': DraftID,
            'attachID': p_attachID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessDelFile");
        var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/DelAttach";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 실제 파일 삭제 처리
**********************************************************************************************/
Add.SuccessDelFile = function (data, status) {
    if (data.d == true) {
        Add.GetAttachList();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 전자결재 근거문서  리스트 호출
**********************************************************************************************/
Add.GetBaseAppList = function () {
    ParamArray = {
        'draftID': DraftID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetBaseAppList");
    var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/GetBaseAppList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 전자결재 근거문서 리스트 보여주기
**********************************************************************************************/
Add.SuccessGetBaseAppList = function (data, status) {
    $('#placeholder_baseAppList').setTemplateURL('/DeskPlusEIP/Task/Jt/Draft/JTBaseApp.htm?seq=' + Guids());
    $('#placeholder_baseAppList').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 전자결재 근거문서 삭제 처리
**********************************************************************************************/
function DelBaseApp(p_draftID, p_documentID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'draftID': p_draftID,
            'documentID': p_documentID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessDelBaseApp");
        var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/DelBaseApp";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 전자결재 근거문서 삭제 처리
**********************************************************************************************/
Add.SuccessDelBaseApp = function (data, status) {
    if (data.d == true) {
        Add.GetBaseAppList();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 협업 근거문서  리스트 호출
**********************************************************************************************/
Add.GetBaseTaskList = function () {
    ParamArray = {
        'draftID': DraftID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetBaseTaskList");
    var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/GetBaseTaskList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 협업 근거문서 리스트 보여주기
**********************************************************************************************/
Add.SuccessGetBaseTaskList = function (data, status) {
    $('#placeholder_baseTaskList').setTemplateURL('/DeskPlusEIP/Task/Jt/Draft/JTBaseTask.htm?seq=' + Guids());
    $('#placeholder_baseTaskList').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 협업 근거문서 삭제 처리
**********************************************************************************************/
function DelBaseTask(p_draftID, p_baseTaskID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'draftID': p_draftID,
            'baseTaskID': p_baseTaskID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessDelBaseTask");
        var url = "/DeskPlusEIP/Task/Ws/Draft/Add.asmx/DelBaseTask";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 협업 근거문서 리스트 보여주기
**********************************************************************************************/
Add.SuccessDelBaseTask = function (data, status) {
    if (data.d == true) {
        Add.GetBaseTaskList();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 근거문서 추가
**********************************************************************************************/
Add.AddApp = function () {
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
작성일자 : 2015-04-24
작성목적 : 업무협업 근거문서 추가
**********************************************************************************************/
Add.AddTask = function () {
    ParamArray = {
        'baseTasks': BaseTasks
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
작성일자 : 2015-04-24
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
        var obj = eval("Add.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);

    //var pos = document.body.scrollHeight;
    //$("html, body").animate({ scrollTop: pos }, 100);
}


var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Add.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Add.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Add.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "Add.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 부서/사용자 삭제
***************************************************/
Add.DelItem = function (id) {
    $("#" + id).remove();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
Add.ExistDeptUser = function (oid, target) {
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
작성일자 : 2015-04-24
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
                if (!Add.ExistDeptUser(gubun + "_" + oid + "_" + type, EleDom)) {
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

        if (sFailueList.length > 0)
            alert(R_AleadyRegisted + "\n\n'" + sFailueList + "'");
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
Add.GetList = function (target, tag) {
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


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 요청하기
**********************************************************************************************/
Add.Save = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    // 대분류, 소분류
    var item = $("#ddlItem").val();
    var itemsub = $("#ddlItemSub").val();

    if (item == "0") {
        alert(R_SelectFirst);
        return;
    }

    if (itemsub == "0") {
        alert(R_SelectSecond);
        return;
    }

    $("#tbSelItemSubID").val(itemsub);

    // 완료요청일
    var getcompReqDate = $("#tbxCompleteRequestDate").val();

    if (getcompReqDate == "") {
        alert(R_InputCompleteDate);
        return false;
    }

    // 수신자, 참조자
    $("#hdReceiveID").val(Add.GetList("InputTO", "oid"));
    $("#hdReferenceID").val(Add.GetList("InputCC", "oid"));

    if ($("#hdReceiveID").val() == "") {
        alert(R_InputReceive);
        return;
    }

    //전자결재 근거문서
    $("#tbBaseApps").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseTasks").val(BaseTasks);

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", R_InputTitle, true);
    if (!validator.validate()) return;

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

    __doPostBack('lbtnAdd', "ADD");
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 임시저장
***************************************************/
Add.SaveDraft = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    // 소분류
    var itemsub = $("#ddlItemSub").val();
    $("#tbSelItemSubID").val(itemsub);

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", R_InputTitle, true);
    if (!validator.validate()) return;

    // 수신자, 참조자
    $("#hdReceiveID").val(Add.GetList("InputTO", "oid"));
    $("#hdReferenceID").val(Add.GetList("InputCC", "oid"));

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

    //2번 이상 클릭 불가
    BtnCheck = "Y";

    __doPostBack('lbtnAdd', 'Draft');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-24
작성목적 : 수정하기
**********************************************************************************************/
Add.Mod = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    // 소분류
    var itemsub = $("#ddlItemSub").val();
    $("#tbSelItemSubID").val(itemsub);

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", R_InputTitle, true);
    if (!validator.validate()) return;

    // 수신자, 참조자
    $("#hdReceiveID").val(Add.GetList("InputTO", "oid"));
    $("#hdReferenceID").val(Add.GetList("InputCC", "oid"));

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

    //2번 이상 클릭 불가
    BtnCheck = "Y";

    __doPostBack('lbtnAdd', "MOD");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 임시저장 불러오기
**********************************************************************************************/
Add.AddDraft = function () {

    ParamArray = {
        'callBackEvent': "GetDraft"
    };

    var popWidth = 800; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Task/Draft/AddList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AddDraft", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-27
작성목적 : 임시저장 불러오기
**********************************************************************************************/
Add.GetDraft = function (p_draftID) {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,        
        'draftID': p_draftID,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isType': "RE",
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());

    location = "/DeskPlusEIP/Task/Draft/AddN.aspx?p=" + GetEncryptValue;
}