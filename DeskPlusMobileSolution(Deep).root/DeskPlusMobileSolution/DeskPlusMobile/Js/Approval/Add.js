Add = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    Add.Init();

    $('#listTitle').click(function (e) {
        e.preventDefault();
        location.href = "WFolder.aspx";
    });

    //임시저장
    $('#btnDraft').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.SaveDraft();
    });

    $('#btnDraftV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.SaveDraft();
    });

    //근거문서
    $('#btnBaseDoc').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.OpenBaseDoc();
    });

    $('#btnBaseDocV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.OpenBaseDoc();
    });

    //첨부파일
    $("#btAddFile").click(function () {
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        $("#upload").click();
    });

    $("#btAddFileV").click(function () {
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        $("#upload").click();
    });

    $('#form1').ajaxForm({
        dataType: 'text',
        beforeSerialize: function () {
            //$('#data').attr('value', '야호');
        },
        beforeSubmit: function () {
            $('#fileList').html('uploading...');
        },

        success: function (data) {
            Add.getSpoolList();
        },
        error: function (data) {
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
        }

    });

    // EDM 설정
    $('#btEDM').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.GetEDM();
    });

    $('#btEDMV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.GetEDM();
    });

    //상신
    $('#btnAdd').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.Submit();
    });

    $('#btnAddV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        Add.Submit();
    });

    //결재자 선택
    $("input:radio[name=rdApprover]").click(function () {
        Add.LoadDisplay(this);
    });

    //검색
    $('#btnSearch').click(function (e) {
        e.preventDefault();
        Add.search();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //검색입력필드 엔터 이벤트 기술
    $("#tbSearch").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            Add.search();
        }
    });

    $("#tbSearch").attr("placeholder", TagDic_INFO3 + "," + R_Dept);

    // 부서정보 변경 시, 변수값 재설정.
    $("#ddlAdditionDept").change(function (e) {
        e.preventDefault();
        Common.ModDeptInfo();
    });

    //결재의견 입력값 체크하기
    Add.MaxLang();

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            //LoadingShow(false);
            location.reload();
        });
    }

});

/**************************************************
작성자   : famo23
작성일자 : 2019-07-16
작성목적 : 글자 제한
***************************************************/
Add.MaxLang = function () {

    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-15
작성목적 : frame 사이즈 조정
**********************************************************************************************/
function autoResize(obj) {
    obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 페이지 로드
**********************************************************************************************/
Add.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {
        NoticeShow("!", R_SignNo);
        return;
    }

    DHeight = $('#dButton').height() + 285;
    FormActionPath = $("#form1").attr('action');

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    var tmpWidth = $('#ifrmForm').contents().width() + 50;
    TmpSize = $(window).width() / tmpWidth;

    if (TmpSize < 1) {

        $("#ifrmForm").css({
            '-ms-zoom': +TmpSize,
            '-moz-transform': 'scale(' + TmpSize + ')',
            '-moz-transform-origin': '0px 0',
            '-o-transform': 'scale(' + TmpSize + ')',
            '-o-transform-origin': '0 0',
            '-webkit-transform': 'scale(' + TmpSize + ')',
            '-webkit-transform-origin': '0 0',
            '-ms-transform': 'scale(' + TmpSize + ')',
            '-ms-transform-origin': '0 0'
        });

        var tmpHeight = $('#ifrmForm').contents().height() * TmpSize + 30;
        $("#contentDoc").css("height", tmpHeight + "px");
    }
    else {
        TmpSize = 1;
    }

    ifrmObj = ifrmForm.document;
    setTimeout("Add.InitForm();", 100);

    // 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    // 자주지정하는 결재자 정보
    Common.GetFavoriteApp();

    // 양식결재라인 정보
    Common.GetFormSeq(FormID);

    //지정된 결재자중 변경된 결재자 리스트
    Add.GetNetModfySeq(FormID);

    //개인결재선
    Add.GetFavSeq();
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-16
작성목적 : 양식초기화
***************************************************/
Add.InitForm = function () {
    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
        if ($("#" + o.id, ifrmObj).attr("isapp")) {
            $("#" + o.id, ifrmObj).removeAttr("isapp")      //승인시, isapp 어트리뷰트값 setting
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.tagName.toUpperCase() == "INPUT") { o.value = ""; o.setAttribute("readonly", "true"); }
        if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }

        Common.ExceptObj(o);   // 특정필드 예외처리 (에디터 ..)
    });

    // 기본문서정보 설정 (양식관련 기본정보 처리 / 문서번호,보존년한,제목)
    Common.SetDefaultInfo();

    // 입력필드 활성화
    var actObj1 = $.grep(arrInfo, function (o, i) {
        return (o.id.indexOf("BODY") == 0);
    });
    var actObj2 = $("#SUBJ,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each($.merge(actObj1, actObj2), function (i, o) {
        $("#" + o.id, ifrmObj).removeAttr("unselectable");
        $("#" + o.id, ifrmObj).removeAttr("readonly");
        $("#" + o.id, ifrmObj).attr("contenteditable", "true");
    });

    // 유동결재 초기화
    Common.SetDefault();

    if ($("#FLOWZD", ifrmObj).length > 0) {
        Common.SetFlowInfo("FLOWZD:1");
    }

    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
    Add.GetNetSeq(FormID);

    //수신자 지정
    if (appSeqDicZQ.size() < 1) {
        Add.GetNetZQSeq(FormID);
    }

    //참조자지정
    if (appSeqDicZC.size() < 1) {
        Add.GetNetZCSeq(FormID);
    }


    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);

    nowEl = null;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-17
작성목적 : (결재망저장) 양식별 저장된 기 결재라인 가져오기 
***************************************************/
Add.GetNetSeq = function (_formid) {
    if (_formid == 0) return;

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetNetLine";
    ParamArray = { 'formID': _formid };
    var obj = eval("Add.SuccessGetNetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

Add.SuccessGetNetSeq = function (data, status) {

    if (data != null) {
        $.each(data.d, function (i, o) {

            // 유동결재망처리
            if (($("#FLOWZD", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZD") == 0)) {
                var tmpZD = data.d[i].ApprovalTag.replace("ZD", "");
                addZDcnt = tmpZD - $("#FLOWZD", ifrmObj).children().length + 1;
                if (addZDcnt > 0) { Common.SetFlowInfo("FLOWZD:" + addZDcnt); }
            }
            if (($("#FLOWZA", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZA") == 0)) {
                var tmpZA = data.d[i].ApprovalTag.replace("ZA", "");
                addZAcnt = tmpZA - $("#FLOWZA", ifrmObj).children().length;
                if (addZAcnt > 0) { Common.SetFlowInfo("FLOWZA:" + addZAcnt); }
            }
            if (($("#FLOWZH", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZH") == 0)) {
                var tmpZH = data.d[i].ApprovalTag.replace("ZH", "");
                addZHcnt = tmpZH - $("#FLOWZH", ifrmObj).children().length;
                if (addZHcnt > 0) { Common.SetFlowInfo("FLOWZH:" + addZHcnt); }
            }

            Common.SetApproval(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].UserName, data.d[i].DeptID, data.d[i].DeptName, data.d[i].TitleID, data.d[i].TitleName, data.d[i].DutyID, data.d[i].DutyName, data.d[i].IsGroup, data.d[i].IsSubTree, "");
        });
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-17
작성목적 : 수신 정보 가져오기
***************************************************/
Add.GetNetZQSeq = function (_formid) {
    if (_formid == 0) return;

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetNetZQZCLine";

    ParamArray = {
        'formID': _formid,
        'appointerKind': "ZQ"
    };

    var obj = eval("Add.SuccessGetNetSeq");

    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-17
작성목적 : 참조 정보 가져오기
***************************************************/
Add.GetNetZCSeq = function (_formid) {
    if (_formid == 0) return;
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetNetZQZCLine";

    ParamArray = {
        'formID': _formid,
        'appointerKind': "ZC"
    };
    var obj = eval("Add.SuccessGetNetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-17
작성목적 : 양식별 저장된 결재라인 중 부서 변경된 결재라인정보 가져오기
***************************************************/
Add.GetNetModfySeq = function (_formid) {
    if (_formid == 0) return;
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetNetModfyLine";
    ParamArray = { 'formID': _formid };
    var obj = eval("Add.SuccessGetNetModfySeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

Add.SuccessGetNetModfySeq = function (data, status) {
    var msgModfyLine = "";
    var tmpVal = "";

    if (data != null) {
        $.each(data.d, function (i, o) {
            tmpVal += "\n - " + data.d[i].DeptName + " " + data.d[i].TitleName + " " + data.d[i].UserName;
        });
    }

    if (tmpVal != "") {
        msgModfyLine = "[" + R_ModfyAppLine + "]" + tmpVal + "\n" + R_Msg_ConfirmAppLine;
        action_popup.alert(msgModfyLine);
        /*
        [결재정보변경]
        - 기술본부 저승이 사원
        - 기술본부 도깨비 회장
        결재라인을 확인하세요.
        */
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-18
작성목적 : 검색
***************************************************/
Add.SetSearch = function () {

    Add.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-18
작성목적 : 검색  (웹서비스 호출)
***************************************************/
Add.search = function () {

    var searchValue = $("#tbSearch").val();

    if (jQuery.trim(searchValue) != "") {

        var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetDeptUser";

        //파라미터값 배열에 담기
        ParamArray = {
            'Sv': searchValue.securityText()
        };

        var obj = eval("Add.succeeded");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {
        $("#lbDeptUser").empty();
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2019-07-18
작성목적 : 검색 (웹서비스 값 받기)
***************************************************/
Add.succeeded = function (data, status) {

    $("#lbDeptUser").empty();

    if (data != null) {
        $.each(data.d, function (i, o) {
            $("#lbDeptUser").append("<span class='CDeptUser' id='" + i + "_DeptUser' style='width:100%;cursor:pointer;' onclick=\"javascript:Add.SetUserInfo('" + i + "','" + data.d[i].NameValue + "')\">" + data.d[i].NameText + "</span><br>");
        });
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-18
작성목적 : 사용자, 부서선택
***************************************************/
Add.SetUserInfo = function (_valID, _val) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    if (_val == "") return;
    if (_val == undefined) return;

    $(".CDeptUser").css("background-color", "");
    $("#" + _valID + "_DeptUser").css("background-color", "#d3d3d3");

    if (nowEl == null) {
        action_popup.alert(R_Msg_ErrSelUnit); return;    // 모든 결재자의 경우 (결재칸선택)
    }

    if (nowEl.id == "ZD0") { return; }

    if (!/^Z{1}[D|H|C|P|Q|A|R]{1}([0-9])?[0-9]*$/.test(nowEl.id)) {
        action_popup.alert(R_Msg_ErrSelUnit); return;    // 모든 결재자의 경우 (결재칸선택)
    }

    if (nowEl.getAttribute("IsAsign")) { action_popup.alert(R_Msg_ErrReCancel); return; }

    var tmpval = _val.split('|');

    if (tmpval[0] == "M") {
        //사용자선택
        Common.SetApproval(nowEl.id, tmpval[1], tmpval[2], tmpval[3], tmpval[4], tmpval[5], tmpval[6], tmpval[7], tmpval[8], "M", "0", "");
    } else {
        //부서선택

        // 참조(ZC)
        if (/Z{1}[C][0-9]?[0-9]?$/.test(nowEl.id)) {
            //하위부서지정 popup
            Common.popUpGroupList(_val); return;
        }
        // 발신(ZD)/협조(ZH)/합의(ZA)/공람(ZR)
        if ((/Z{1}[D|H|A|R][0-9]?[0-9]?[0-9]?$/.test(nowEl.id))) {
            // 결재자
            Common.SetApproval(nowEl.id, tmpval[17], tmpval[18], tmpval[19], tmpval[20], tmpval[21], tmpval[22], tmpval[23], tmpval[24], "G", "0", ""); return;
        }
        // 수신(ZQ/ZP)
        if ((/Z{1}[Q|P][0-9]?[0-9]?[0-9]?$/.test(nowEl.id))) {
            // 수발자
            Common.SetApproval(nowEl.id, tmpval[9], tmpval[10], tmpval[11], tmpval[12], tmpval[13], tmpval[14], tmpval[15], tmpval[16], "G", "0", ""); return;
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 결재자선택 
***************************************************/
Add.LoadDisplay = function (arg) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var tmpID = arg.id;

    if (tmpID == "rdApprover1") {
        jQuery.each($("#divSearch1"), function () { this.style.display = ""; });
        jQuery.each($("#divSearch2"), function () { this.style.display = ""; });
        jQuery.each($("#divFavSeq"), function () { this.style.display = "none"; });

    }
    else {
        jQuery.each($("#divSearch1"), function () { this.style.display = "none"; });
        jQuery.each($("#divSearch2"), function () { this.style.display = "none"; });
        jQuery.each($("#divFavSeq"), function () { this.style.display = ""; });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 개인결재선 
***************************************************/
Add.GetFavSeq = function () {

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetFavSeq";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Add.succeededGetFavSeq");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 개인결재선  (웹서비스 값 받기)
***************************************************/
Add.succeededGetFavSeq = function (data, status) {

    if (data != null) {
        $.each(data.d, function (i, o) {
            $("#lbFavSeq").append("<span class='CFavSeq' id='" + i + "_FavSeq' style='width:100%;cursor:pointer;' onclick=\"javascript:Add.SetFavSeq('" + i + "','" + data.d[i].FavSeqID + "')\" >" + data.d[i].Subject + "</span><br>");
        });
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 개인결재선 선택
***************************************************/
Add.SetFavSeq = function (_valID, _val) {

    cntZD = 0, cntZH = 0, cntZA = 0;
    cntZP = 0, cntZQ = 0, cntZC = 0;

    dicFavZD.clear();
    dicFavZH.clear();
    dicFavZA.clear();
    dicFavZP.clear();
    dicFavZQ.clear();
    dicFavZC.clear();

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    $(".CFavSeq").css("background-color", "");
    $("#" + _valID + "_FavSeq").css("background-color", "#d3d3d3");

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetFlowSeqLine";

    //파라미터값 배열에 담기
    ParamArray = {
        'favSeqID': _val
    };

    var obj = eval("Add.succeededSetFavSeq");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

Add.AddItemObj = function (_tag, _obj) {

    _tag = _tag.toUpperCase();
    switch (_tag) {
        case "ZD":
            dicFavZD.add(cntZD, _obj);
            cntZD++;
            break;
        case "ZH":
            dicFavZH.add(cntZH, _obj);
            cntZH++;
            break;
        case "ZA":
            dicFavZA.add(cntZA, _obj);
            cntZA++;
            break;
        case "ZP":
            dicFavZP.add(cntZP, _obj);
            cntZP++;
            break;
        case "ZQ":
            dicFavZQ.add(cntZQ, _obj);
            cntZQ++;
            break;
        case "ZC":
            dicFavZC.add(cntZC, _obj);
            cntZC++;
            break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 개인결재선  (웹서비스 값 받기)
***************************************************/
Add.succeededSetFavSeq = function (data, status) {

    if (data != null) {

        var rtnval = {};
        $.each(data.d, function (i, o) {
            rtnval = {
                'tag': data.d[i].ApprovalTag
                , 'userID': data.d[i].UserID
                , 'name': data.d[i].NameBase
                , 'deptID': data.d[i].DeptID
                , 'deptName': data.d[i].DeptBase
                , 'titleID': data.d[i].TitleID
                , 'titleName': data.d[i].TitleBase
                , 'dutyID': data.d[i].DutyID
                , 'dutyName': data.d[i].DutyBase
                , 'isDept': data.d[i].IsDept
                , 'isSub': data.d[i].IsSub
            };

            Add.AddItemObj(data.d[i].ApprovalTag, rtnval);
        });
    }

    Add.SetSeqLine();
}

// 결재라인지정
Add.SetSeqLine = function () {

    var items = null, subkey = null, subitem = null;
    if (dicFavZD.size() > 0 || dicFavZH.size() > 0 || dicFavZA.size() > 0) {

        items = formSeqDic.items;
        for (var o in items) {

            /*** 고정결재 ***/

            // 고정결재칸(ZD1,ZD2,ZD3 ...)
            if (o.indexOf("ZD") == 0 && dicFavZD.size() > 0 && o != "ZD0") {
                subkey = cntZD - dicFavZD.size(); subitem = dicFavZD.items[subkey];
                Add.SetObj(o, subitem);
                dicFavZD.remove(subkey);
            }
            // 고정결재칸(ZH1,ZH2,ZH3 ...)
            if (o.indexOf("ZH") == 0 && dicFavZH.size() > 0) {
                subkey = cntZH - dicFavZH.size(); subitem = dicFavZH.items[subkey];
                Add.SetObj(o, subitem);
                dicFavZH.remove(subkey);
            }
            // 고정결재칸(ZA1,ZA2,ZA3 ...)
            if (o.indexOf("ZA") == 0 && dicFavZA.size() > 0) {
                subkey = cntZA - dicFavZA.size(); subitem = dicFavZA.items[subkey];
                Add.SetObj(o, subitem);
                dicFavZA.remove(subkey);
            }

            /*** 유동결재 ***/
            var i = 0, addcnt = 0;

            // 유동결재망(ZD)
            if (o == "FLOWZD" && dicFavZD.size() > 0) {

                addcnt = (dicFavZD.size() - $("#FLOWZD", ifrmObj).children().length) + 1;
                Common.SetFlowInfo("FLOWZD:" + addcnt);

                items = dicFavZD.items;
                i = 1;
                for (var o in items) {
                    Add.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
            // 유동결재망(ZH)
            if (o == "FLOWZH" && dicFavZH.size() > 0) {

                addcnt = dicFavZH.size() - $("#FLOWZH", ifrmObj).children().length;
                Common.SetFlowInfo("FLOWZH:" + addcnt);

                items = dicFavZH.items;
                i = 1;
                for (var o in items) {
                    Add.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
            // 유동결재망(ZA)
            if (o == "FLOWZA" && dicFavZA.size() > 0) {

                addcnt = dicFavZA.size() - $("#FLOWZA", ifrmObj).children().length;
                Common.SetFlowInfo("FLOWZA:" + addcnt);

                items = dicFavZA.items;
                i = 1;
                for (var o in items) {
                    Add.SetObj(items[o].tag + i, items[o]); i++;
                }
            }
        }
    }
    //수신(ZP)
    if (dicFavZP.size() > 0 && formSeqDic.contains("ZP")) {
        items = dicFavZP.items;
        for (var o in items) {
            Add.SetObj(items[o].tag, items[o]);
        }
    }
    //수신(ZQ)
    if (dicFavZQ.size() > 0 && formSeqDic.contains("ZQ")) {
        items = dicFavZQ.items;
        for (var o in items) {
            Add.SetObj(items[o].tag, items[o]);
        }
    }
    //참조(ZC)
    if (dicFavZC.size() > 0 && formSeqDic.contains("ZC")) {
        items = dicFavZC.items;
        for (var o in items) {
            Add.SetObj(items[o].tag, items[o]);
        }
    }
}

// 결재라인지정 (실제, 태그에 결재자 지정)
Add.SetObj = function (_nowEl, _item) {
    //_item.tag
    Common.SetApproval(_nowEl, _item.userID, _item.name, _item.deptID, _item.deptName, _item.titleID, _item.titleName, _item.dutyID, _item.dutyName, _item.isDept, _item.isSub, "");
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-19
작성목적 : 근거문서
***************************************************/
Add.OpenBaseDoc = function () {

    var tmpWidth = $(window).width() - 30;

    if (tmpWidth > 400) {
        tmpWidth = 400;
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'baseDocIDs': BaseDocIDs
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusMobile/Approval/GetBaseDoc.aspx?p=" + GetEncryptValue;
    PopupWin(tmpWidth, "620", url, "5", "50", false);

    $('html, body').scrollTop(0);

}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 근거문서 삭제
***************************************************/
BaseDel = function (p_documentID, p_docNumber) {

    BaseDocIDs = BaseDocIDs.split("|").join("");  // replaceAll
    BaseDocIDs = BaseDocIDs.trimEnds(';');
    var BaseDocIDsStr = BaseDocIDs.split(';');
    var newDocIDs = "";

    // 작제된 문서번호를 제외한 나머지 문서번호 새로 저장
    for (var i = 0; i < BaseDocIDsStr.length; i++) {
        if (BaseDocIDsStr[i] != p_documentID + "^" + p_docNumber) {
            newDocIDs = newDocIDs + '|' + BaseDocIDsStr[i] + ";";
        }
    }

    BaseDocIDs = newDocIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocIDs);

    Add.DelRow(p_documentID);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-23
작성목적 : 근거문서 삭제시 ROW 제거
***************************************************/
Add.DelRow = function (p_documentID) {

    var row = $("#" + p_documentID);
    BaseDocCnt = BaseDocCnt - 1;
    row.remove();

    if (BaseDocCnt == 0) {
        jQuery.each($("#uBase"), function () { this.style.display = "none"; });
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-24
작성목적 : 파일 선택
***************************************************/
function fileChange(obj) {

    NoticeHide();

    var tmpRtn = "Y";

    if (jQuery.trim($('#upload').val()) != "") {
        var file = obj.files;

        if (file) {
            for (var i = 0; i < file.length; i++) {

                if (file[i].size > UpFileSize) {
                    tmpRtn = "N";
                    var tmpMsg = bytesToSize(UpFileSize);
                    //NoticeShow("!", tmpMsg + R_MaxFile);
                    action_popup.alert(tmpMsg + R_MaxFile);
                    return;
                }

                var fileExt = file[i].name.substr(file[i].name.lastIndexOf("."));
                fileExt = fileExt.toLowerCase();

                $.each(BlockExt.split(','), function (idx) {
                    if (fileExt == ("." + BlockExt.split(',')[idx])) {
                        tmpRtn = "N";
                        //NoticeShow("!", BlockExt + R_CheckFileType);
                        action_popup.alert(BlockExt + R_CheckFileType);
                        return;
                    }
                });
            }

            if (tmpRtn == "Y") {
                LoadingShow(true);

                $("#form1").attr("action", "/DeskPlusMobile/Common/Upload/UploadCommon.ashx");
                $("#form1").attr("enctype", "multipart/form-data");
                $("#form1").attr("target", "hid_func");
                $("#form1").submit();
            }
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-24
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Add.getSpoolList = function () {

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Add.succeededSpool");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-24
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Add.succeededSpool = function (data, status) {

    var Cnt = 0;
    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {
            var contentHtml = "<ul class='mli8'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' onclick=\"Add.DelFile('" + data.d[i].FileName + "')\" style='cursor:pointer' />&nbsp;&nbsp;</span>"
             + "<div class='conBox'><span class='sch_user'>" + data.d[i].FileName + "</span></div></li></ul>";

            $("#fileList").append(contentHtml);

        });

        if (data.d.length == 0) {
            jQuery.each($("#uFile"), function () { this.style.display = "none"; });

        }
        else {
            jQuery.each($("#uFile"), function () { this.style.display = ""; });
        }
    }

    $('#upload').val("");
    $('#upload').replaceWith($('#upload').clone(true));

    $('html, body').scrollTop($(document).height());

    $("#form1").attr("action", FormActionPath);
    $("#form1").removeAttr("enctype");
    $("#form1").removeAttr("target");

    LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-24
작성목적 : Spool 폴더 파일삭제
***************************************************/
Add.DelFile = function (fileName) {
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/DelSpoolFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileName': fileName,
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Add.getSpoolList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-25
작성목적 : 임시저장
***************************************************/
Add.SaveDraft = function () {

    if (Add.SetDraftInfo()) {

        var url = "/DeskPlusMobile/Approval/SaveDraft.aspx";
        PopupWin("300", "122", url, "", "", true);
    }
}

Add.SetDraftInfo = function () {

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    // 유동결재망 처리
    $("#tbIsUseFlow").val(Common.GetFlowInfo());

    // 결재순서정렬
    appSeqKey = Common.GetAppSeqQueue(appSeqDic);

    // 결재정보처리 (순서에 맞게)
    $("#htaXmlSeq").val(Common.GetSeqXML(appSeqKey, appSeqDic, appSeqDicZR));   // 발신결재
    $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));                   // 수신결재
    $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));                   // 참조결재

    // 특정태그처리 (for, chrome,ff,safari)
    Common.UpdateDOM();

    // 문서내용(Html)처리
    $("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-29
작성목적 : [문서상신]
***************************************************/
Add.Submit = function () {

    // 이중클릭 확인
    if (ApprovalType != "") { action_popup.alert(R_ProcessingWait); return; }

    //필수입력 - 결재라인
    if (appSeqDic.size() < 2) { action_popup.alert(Msg_SetLine); return; }

    //필수입력 - 제목
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $.trim($("#SUBJ", ifrmObj).val());
        if (subj == "") { action_popup.alert(R_InputSubject); return; }
    }

    //조건별 필수 입력- EDM
    if (IsLinked == "1" && IsLinkedType == "2") {
        var edmVal = $("#tbIsLinkedEtc").val();
        if (edmVal == "") { action_popup.alert(R_Msg_AlertEDMCheck); return; }
    }

    //필수입력 - 보존년한
    var life = "";
    if ($("#LIFE", ifrmObj).length > 0) {
        life = $.trim($("#LIFE", ifrmObj).val());
        if (life == "") { action_popup.alert(R_Msg_ErrLife); return; }
    }
    life = (life == "") ? Tit_Life3 : life;  // 보존년한필드가 없으면 기본값으로 대체
    $("#tbLifeTime").val(life);

    // 에디터 스타일 체크

    // 부서정보 (상신부서)
    var deptVal = $("#ddlAdditionDept").val();     //38|솔루션개발팀|15|과장|41|파트장
    if (deptVal == undefined) {
        $("#tbDeptID").val(DeptID);
        $("#tbDeptName").val(DeptName);
    }
    else {
        $("#tbDeptID").val(deptVal.split("|")[0]);
        $("#tbDeptName").val(deptVal.split("|")[1]);
    }

    // 결재의견 (For.첨언처리)
    var opinion = $("#memo").val();
    if (opinion.length > 200) { action_popup.alert(R_Msg_ErrOpinion); return; }
    $("#memo").val(opinion.replace(/\n/gi, "<br>"));

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    appSeqKey = Common.GetAppSeqQueue(appSeqDic);

    var rtnval = false;
    var url = "/DeskPlusMobile/Approval/ModSequence.aspx";
    PopupWin(300, 350, url, "", "", true);
}

function ConfirmModSequenceFunc(rtnVal) {

    if (rtnVal) {

        // 유동결재망 처리
        $("#tbIsUseFlow").val(Common.GetFlowInfo());
        // 결재순서정렬
        appSeqKey = Common.GetAppSeqQueue(appSeqDic);
        // 결재정보처리 (순서에 맞게)
        $("#htaXmlSeq").val(Common.GetSeqXML(appSeqKey, appSeqDic, appSeqDicZR));   // 발신결재
        $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));                   // 수신결재
        $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));                   // 참조결재

        // 병렬합의 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZA") > 0) {
            action_popup.alert(R_Msg_SeqErrZA); return;
        }
        // 협조 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZH") > 0) {
            action_popup.alert(R_Msg_SeqErrZH); return;
        }

        // EDM처리 & 커스텀
        ApprovalType = "S";
        LoadingShow(true); //처리중

        if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
            ifrmProcessing.onComplete();

        } else {                                                        // 그외의 경우, submit호출
            AddSubmit();
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-29
작성목적 : Submit (연동문서에서 onComplete함수가 있다면, submit을 위해 이 함수를 호출)
***************************************************/
AddSubmit = function () {

    // 제목 처리
    if ($("#SUBJ", ifrmObj).length > 0) {
        $("#tbSubject").val($("#SUBJ", ifrmObj).val());
    }

    // 문서내용(에디터) + 에디터이미지 처리(activeX인경우만, 이미지 업로드)
    Common.SetEditorTags();

    //  빈 결재란 처리
    Common.SetDefaultTags();

    //  인장처리 (+결재정보+서버시간 체크)
    Common.SetStamp("ZD0", SealPath, UserName, DeptName, TitleName, DutyName, "0");

    // 첨언처리
    Common.SetOpinion(UserName, TitleName, DutyName, DeptName, $("#memo").val());

    // Input Tag 처리
    Common.SetReadOnly();

    // 특정태그처리 (for, chrome,ff,safari)
    Common.UpdateDOM();

    var content = RemoveScriptTag($("HTML", ifrmObj).html());

    // 문서내용(Html)처리
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

    __doPostBack('lbtnSubmit', "ADD");
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-01
작성목적 : 문서상신 후, 호출 (서버호출)
***************************************************/
var documentID = 0; var msg = "";
SubmitAfterCall = function (_customPath, _docID, _msg) {

    if (_customPath == "") { SubmitClose(_msg); }

    documentID = _docID;
    msg = _msg;

    // 커스텀연동 호출
    Common.GetLegacy(_customPath);

}
SubmitAfterCallOK = function () {
    SubmitClose(msg);
}
/**************************************************
작성자   : famo23
작성일자 : 2019-08-01
작성목적 : 문서상신 후(성공/실패), 페이지 이동 (서버호출)
***************************************************/
SubmitClose = function (_msg) {
    action_popup.alert(_msg);

    ParamArray = {
        'BPage': "REP"
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-16
작성목적 : 문서관리 
***************************************************/
Add.GetEDM = function () {

    ParamArray = {
        'CompanyID': CompanyID,
        'USERID': LoginID,
        'USERPASSWORD': UPwd,
        'targetURL': '/DeskplusEDM/Folder/Dialog/transEDM/GetFolder.aspx'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEDM/Authentication/LoginOutSide.aspx?p=" + GetEncryptValue;
    PopupWin("300", "365", url, "", "", true);

}

function ConfirmGetEDMFunc(rtnVal) {

    if (rtnVal != "" && rtnVal != undefined) {
        $("#tbIsLinkedEtc").val(rtnVal.split('^')[0]);

        // 양식에 이관 항목 표시 필드가 있다면 채워준다.
        if ($("#spEDMFolderNM", ifrmObj).length > 0) {
            $("#spEDMFolderNM", ifrmObj).text(rtnVal.split('^')[1]);
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 커스텀파일에서 에러발생시
***************************************************/
ResetProcess = function () {
    ApprovalType = "";
    LoadingShow(false);
}
