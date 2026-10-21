Add = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        if (ItemID != "0") {
            $('#ddlItem').val(ItemID);
        }

        if (TplItemSubID != "0,0" || ItemID != "0") {
            //$('#ddlItem').val(TplItemSubID.split(',')[0]);
            $("#ddlItemSub option").remove();
            $("#ddlItemSub").append("<option value='0,0'>------</option>");
            Add.GetItemSubList();
            $("#ddlItemSub").val(TplItemSubID);
        }

        //1차 분류 선택
        $('#ddlItem').change(function () {
            $("#ddlItemSub option").remove();
            $("#ddlItemSub").append("<option value='0,0'>------</option>");
            var selVal = $('#ddlItem').val();
            if (selVal != "") {
                Add.GetItemSubList();
            }
        });

        //2차 분류 선택
        $('#ddlItemSub').change(function () {
            var selVal = $("#ddlItemSub").val();

            //지정자 정보 가져오기
            Add.GetFavUser();

            //템플릿 카운트
            if (selVal.split(',')[1] != "0") {
                if (window.confirm(R_MovePage)) {
                    ParamArray = {
                        'itemID': $("#ddlItem").val(),
                        'tplItemSubID': selVal
                    };

                    CallEncrypt(CallGetParam());

                    if ($.browser.msie) {
                        location = "Add.aspx?p=" + GetEncryptValue;
                    } else {
                        location = "AddN.aspx?p=" + GetEncryptValue;
                    }
                }
            }
        });

        //수신자추가
        $('#cbtnReceive').click(function (e) {
            e.preventDefault();

            EleDom = "InputDiv1";

            ParamArray = {
                'gubun': "2"
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Coop/Common/GetUserList.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });

        //참조자추가
        $('#cbtnReference').click(function (e) {
            e.preventDefault();

            EleDom = "InputDiv2";

            ParamArray = {
                'gubun': "2"
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Coop/Common/GetUserList.aspx?p=" + GetEncryptValue;
            PopupWin(800, 485, url, "-500", "", true);
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        // 전자결재 근거문서 추가
        $('#cbtnDocAdd').click(function (e) {
            e.preventDefault();
            Add.DocAdd();
        });

        // 업무협업 근거문서 추가
        $('#cbtnCoopAdd').click(function (e) {
            e.preventDefault();
            Add.CoopAdd();
        });

        //목록가기
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());

            location = "List.aspx?p=" + GetEncryptValue;
        });

        $("#tbxCompleteRequestDate").datepick($.datepicker.regional[PreLang]);
        $('#tbxCompleteRequestDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
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

        setTimeout(function () {
            EleDom = "InputDiv1"; //수신자 화면란
            GetDept(escape(FavorUser)); //자주지정자 설정하기
        }, 200);

        Add.GetSpoolFileList();
        __addEventListener(document, "click", Add.click);
    });
});

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
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

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Add.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "Add.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
Add.DelItem = function (id) {
    $("#" + id).remove();
    /*
    var oEl = document.getElementById(id);
    if (oEl) {
    oEl.removeNode(true);
    }
    */
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 2차 분류 가져오기
***************************************************/
Add.GetItemSubList = function () {
    var url = "/DeskPlusEIP/Coop/Ws/Content/Request/Add.asmx/GetItemSub";

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
작성자   :
작성일자 :
작성목적 : 2차 가져오기, 성공
**********************************************************************************************/
Add.SuccessGetItemSubList = function (data, status) {
    for (var obj in data.d) {
        var nameBase = data.d[obj].NameBase;
        var vals = data.d[obj].ItemSubID + "," + data.d[obj].TempCnt;

        $("#ddlItemSub").append("<option value='" + vals + "'>" + nameBase + "</option>");
    }
}

/**************************************************
작성자   :
작성일자 : 심재운
작성목적 : 사용자 추가값 가져오기
***************************************************/
function GetDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡");

        sHtml = Add.GetDeptUserDiv(response);

        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 선택하기
***************************************************/
Add.GetDeptUserDiv = function (sList) {
    var targetObj = document.getElementById(EleDom);
    var sHtml = targetObj.innerHTML;
    var sFailueList = "";
    var tmpArr = sList.split("‡");
    var idArr = tmpArr[0].split(",");
    var nameArr = tmpArr[1].split(",");
    var catArr = tmpArr[2].split(",");

    for (var i = 0; i < idArr.length; i++) {
        oid = idArr[i];
        name = nameArr[i];
        gubun = catArr[i];

        if (oid != "") {
            if (!Add.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
                if (gubun == "G") {
                    sHtml += " <a href='#' action='del' id=\"G_" + oid + "_" + EleDom + "\" nowrap type='G' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                } else {
                    sHtml += " <a href='#' action='del' id=\"M_" + oid + "_" + EleDom + "\" nowrap type='M' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                }
            }
            else {
                if (sFailueList.length > 0) sFailueList += "','";
                sFailueList += name;
            }
        }
    }
    if (sFailueList.length > 0)
        alert(R_AleadyRegisted + "\n\n'" + sFailueList + "'");

    return sHtml;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
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
작성자   : 이현수
작성일자 : 2012-04-12
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
                //oChildrens[i].type + "|" +
                //sList += oChildrens[i].getAttribute(tag);
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }

    return sList;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'COOP',
        'callBackEvent': "Add.GetSpoolFileList"
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Add.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoContent); }
    if ($(".NoBaseCoopListData") != undefined) { $(".NoBaseCoopListData").text(R_NoContent); }

    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Main/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/icon_search.gif'); }
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
        var obj = eval("Add.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
[저장]
**********************************************************************************************/
Add.Save = function () {
    var item = $("#ddlItem").val();
    var itemsub = $("#ddlItemSub").val();

    if (item == "") {
        alert(R_SelectFirst);
        return;
    }

    if (itemsub == "0,0") {
        alert(R_SelectSecond);
        return;
    }

    //2차 분류 담기
    $("#tbSelItemSubID").val(itemsub);

    var getcompReqDate = $("#tbxCompleteRequestDate").val();

    if (getcompReqDate == "") {
        alert(R_InputCompleteDate);
        return false;
    }

    $("#hdReceiveID").val(Add.GetList("InputDiv1", "oid"));
    $("#hdReceiveName").val(Add.GetList("InputDiv1", "title"));
    $("#hdReferenceID").val(Add.GetList("InputDiv2", "oid"));
    $("#hdReferenceName").val(Add.GetList("InputDiv2", "title"));

    if ($("#hdReceiveID").val() == "") {
        alert(R_InputReceive);
        return;
    }

    //전자결재 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseCoopIDs").val(CoopBaseIDs);

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
    $("#cbtnAdd").attr('onclick', '').unbind('click');

    __doPostBack('lbtnAdd', '');
}

/**********************************************************************************************
전자결재 근거문서 추가
**********************************************************************************************/
Add.DocAdd = function () {
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
Add.CoopAdd = function () {
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

/**************************************************
작성자   : 심재운
작성일자 :
작성목적 : 지정자 정보 가져오기
***************************************************/
Add.GetFavUser = function () {
    var selVal = $("#ddlItemSub").val();

    //2차 분류
    if (selVal.split(',')[0] != "0") {
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/Add.asmx/GetFavUser";

        ParamArray = {
            'itemSubID': selVal.split(',')[0]
        };

        var obj = eval("Add.SuccessGetFavUser");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 :
작성목적 : 지정자 정보 가져오기, 성공
**********************************************************************************************/
Add.SuccessGetFavUser = function (data, status) {
    FavorUser = data.d;
    $("#InputDiv1")[0].innerHTML = ""; //수신자 초기화

    setTimeout(function () {
        EleDom = "InputDiv1"; //수신자 화면란
        GetDept(escape(FavorUser)); //자주지정자 설정하기
    }, 200);
}