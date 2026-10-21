SubmitAdd = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    SubmitAdd.Init();

    $('#listTitle').click(function (e) {
        e.preventDefault();
        SubmitAdd.goBefore();
    });

    //근거문서
    $('#btnBaseDoc').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        SubmitAdd.OpenBaseDoc();
    });

    $('#btnBaseDocV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        SubmitAdd.OpenBaseDoc();
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
            SubmitAdd.getSpoolList();
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
        SubmitAdd.GetEDM();
    });

    $('#btEDMV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        SubmitAdd.GetEDM();
    });

    //상신
    $('#btnAdd').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        SubmitAdd.Submit();
    });

    $('#btnAddV').click(function (e) {
        e.preventDefault();
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        SubmitAdd.Submit();
    });

    //검색
    $('#btnSearch').click(function (e) {
        e.preventDefault();
        SubmitAdd.search();
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
            SubmitAdd.search();
        }
    });

    $("#tbSearch").attr("placeholder", TagDic_INFO3 + "," + R_Dept);

    // 부서정보 변경 시, 변수값 재설정.
    $("#ddlAdditionDept").change(function (e) {
        e.preventDefault();
        Common.ModDeptInfo();
    });

    //결재의견 입력값 체크하기
    SubmitAdd.MaxLang();

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
작성일자 : 2019-08-14
작성목적 : 글자 제한
***************************************************/
SubmitAdd.MaxLang = function () {

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
작성일자 : 2019-08-14
작성목적 : frame 사이즈 조정
**********************************************************************************************/
function autoResize(obj) {
    obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 페이지 로드
**********************************************************************************************/
SubmitAdd.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {
        NoticeShow("!", R_SignNo);
        return;
    }

    DHeight = $('#dButton').height() + 285;
    FormActionPath = $("#form1").attr('action');

    if ($("#htaFormHTML").val() == "") {
        SubmitAdd.goError();
        return;
    }

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
    setTimeout("SubmitAdd.CheckOld();", 100);     //수신부서 반려체크 후, 문서초기화

    // 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    // 자주지정하는 결재자 정보
    Common.GetFavoriteApp();

    // 양식결재라인 정보
    Common.GetFormSeq(FormID);

    // 이전 변경된 결재라인정보 (상신자 정보도, 해당 내용에 포함됨)
    SubmitAdd.GetModfySeq(DocumentID);

    // 근거문서
    SubmitAdd.getBase();

    // 첨부파일
    SubmitAdd.getAttach();

}

/**************************************************
작성목적 : 수신부서 반려체크
***************************************************/
// 변경대상 초기화 (span & div & input) : 결재태그
SubmitAdd.UpdIds = function (o, _oldtag) {

    var tmptag = "";
    var oldTag = _oldtag;
    $.each(o, function (i, o) {

        //var aaa = o.id;
        if ($("#" + o.id, ifrmObj).attr("btag")) {

            tmptag = $("#" + o.id, ifrmObj).attr("btag");       //ZQ01
            o.id = (o.id.indexOf("ZD0") > -1) ? o.id.replace(oldTag + "ZD0", tmptag) : o.id.replace(oldTag, tmptag + "_");
            //o.className = o.className.replace(oldTag, "");
            o.className = tmptag.substr(0, 2);

        } else {
            o.id = o.id.replace(oldTag, "");
            o.className = o.className.replace(oldTag, "");
        }
        //alert(aaa + "   :   " + o.id)

        nowEl = o;
        Common.DelSeq();
    });
}

SubmitAdd.CheckOld = function () {

    // 수신
    if ($("#PARENTDOCUMENT_ZQ01", ifrmObj).length > 0) {
        ifrmObj.body.innerHTML = $("#PARENTDOCUMENT_ZQ01", ifrmObj).html();

        setTimeout(function () {
            var strOld = "";
            var zTags = $("span", ifrmObj);
            $.each(zTags, function (i, o) {
                if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                    strOld += o.id.split("_")[0] + ",";
                }
            });

            // 고정수신 (최근발신태그 선처리)
            var objs = $(".ZD,.formfieldinput", ifrmObj);
            SubmitAdd.UpdIds(objs, "");

            // 고정수신 (이전수발신태그 처리 - 승인)
            var oldTag = "", ids = "";
            $.each(strOld.trimEnds(',').split(','), function (i, o) {

                oldTag = o + "_";
                ids = "." + oldTag + "ZD," + "." + oldTag + "ZP," + "." + oldTag + "ZQ," + "." + oldTag + "ZH," + "." + oldTag + "ZC," + "." + oldTag + "ZR," + "." + oldTag + "ZA," + "." + oldTag + "formfieldinput";
                objs = $(ids, ifrmObj);

                SubmitAdd.UpdIds(objs, oldTag);
            });

            // 고정수신 (이전수발신태그 강제초기화 처리)
            $.each($(".ZQ01,.ZQ02,.ZQ03,.ZQ04,.ZQ05", ifrmObj), function (i, o) {
                $(o).attr("class", o.id.substr(0, 2));
            });


            var actObj = $("input:checkbox,select", ifrmObj);
            $.each(actObj, function (i, o) {
                if ($(o, ifrmObj).attr("disabled")) {
                    $(o, ifrmObj).removeAttr("disabled");
                }
                if ($(o, ifrmObj).attr("readonly")) {
                    $(o, ifrmObj).removeAttr("readonly");
                }
            });
        }, 100);
    }

    setTimeout("SubmitAdd.InitForm();", 100);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 양식초기화
***************************************************/
SubmitAdd.InitForm = function () {

    // 에디터 처리
    if ($("#EDITOR", ifrmObj).length > 0) {
        $("#htaEditorHTML").val($("#EDITOR", ifrmObj).html());
    }

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        if (o.id.indexOf("FLOW") < 0) {

            if ($("#" + o.id, ifrmObj).attr("idx")) {
                $("#" + o.id, ifrmObj).remove();
            }
            else {
                o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
                if ($("#" + o.id, ifrmObj).attr("isapp")) {
                    $("#" + o.id, ifrmObj).removeAttr("isapp")
                }
                if ($("#" + o.id, ifrmObj).attr("IsAsign")) {
                    $("#" + o.id, ifrmObj).removeAttr("IsAsign");   //20130807 부서변경된 경우, 예외처리
                }
            }
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.id == "OPINION") { o.innerHTML = ""; }    // 첨언필드 처리 (이전 완료문서에 있는 첨언내용삭제)
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
        $(o).removeAttr("unselectable");
        $(o).removeAttr("readonly");
        $(o).attr("contenteditable", "true");
    });
    var actObj = $("input:checkbox,input:radio,select,textarea", ifrmObj);
    $.each(actObj, function (i, o) {
        if ($(o).attr("unselectable")) { $(o).removeAttr("unselectable"); }
        if ($(o).attr("readonly")) { $(o).removeAttr("readonly"); }
        if ($(o).attr("disabled")) { $(o).removeAttr("disabled"); }
        if ($(o).attr("contenteditable")) { $(o).attr("contenteditable", "true"); }
    });

    // 유동결재 초기화
    Common.SetDefault();
    Common.SetFlowInfoModCotent($("#tbIsUseFlow").val());

    SubmitAdd.GetSeq(GroupID);

    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);

    nowEl = null;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 검색
***************************************************/
SubmitAdd.SetSearch = function () {

    SubmitAdd.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 검색  (웹서비스 호출)
***************************************************/
SubmitAdd.search = function () {

    var searchValue = $("#tbSearch").val();

    if (jQuery.trim(searchValue) != "") {

        var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetDeptUser";

        //파라미터값 배열에 담기
        ParamArray = {
            'Sv': searchValue.securityText()
        };

        var obj = eval("SubmitAdd.succeeded");
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
작성일자 : 2019-08-14
작성목적 : 검색 (웹서비스 값 받기)
***************************************************/
SubmitAdd.succeeded = function (data, status) {

    $("#lbDeptUser").empty();

    if (data != null) {
        $.each(data.d, function (i, o) {
            $("#lbDeptUser").append("<span class='CDeptUser' id='" + i + "_DeptUser' style='width:100%;cursor:pointer;' onclick=\"javascript:SubmitAdd.SetUserInfo('" + i + "','" + data.d[i].NameValue + "')\">" + data.d[i].NameText + "</span><br>");
        });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 사용자, 부서선택
***************************************************/
SubmitAdd.SetUserInfo = function (_valID, _val) {

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
작성일자 : 2019-08-14
작성목적 : 근거문서
***************************************************/
SubmitAdd.OpenBaseDoc = function () {

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
작성일자 : 2019-08-14
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

    SubmitAdd.DelRow(p_documentID);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 근거문서 삭제시 ROW 제거
***************************************************/
SubmitAdd.DelRow = function (p_documentID) {

    var row = $("#" + p_documentID);
    BaseDocCnt = BaseDocCnt - 1;
    row.remove();

    if (BaseDocCnt == 0) {
        jQuery.each($("#uBase"), function () { this.style.display = "none"; });
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
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
작성일자 : 2019-08-14
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
SubmitAdd.getSpoolList = function () {

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("SubmitAdd.succeededSpool");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
SubmitAdd.succeededSpool = function (data, status) {

    var Cnt = 0;
    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {
            var contentHtml = "<ul class='mli8'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' onclick=\"SubmitAdd.DelFile('" + data.d[i].FileName + "')\" style='cursor:pointer' />&nbsp;&nbsp;</span>"
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
작성일자 : 2019-08-14
작성목적 : Spool 폴더 파일삭제
***************************************************/
SubmitAdd.DelFile = function (fileName) {
    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/DelSpoolFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileName': fileName,
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("SubmitAdd.getSpoolList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 수정후 재상신
***************************************************/
SubmitAdd.Submit = function () {

    // 이중클릭 확인
    if (ApprovalType != "") { action_popup.alert(R_ProcessingWait); return; }

    //필수입력 - 결재라인
    if (appSeqDic.size() < 2) { action_popup.alert(Msg_SetLine); return; }

    //필수입력 - 제목
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $.trim($("#SUBJ", ifrmObj).val());
        if (subj == "") { action_popup.alert(R_InputSubject); return; }
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
작성일자 : 2019-08-14
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
작성일자 : 2019-08-14
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
작성일자 : 2019-08-14
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
작성목적 : 이전결재라인 가져오기
***************************************************/
SubmitAdd.GetSeq = function (_docID) {
    if (_docID == 0) return;

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetSeqBackup";
    ParamArray = { 'docID': _docID };
    var obj = eval("SubmitAdd.SuccessGetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
SubmitAdd.SuccessGetSeq = function (data, status) {
    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");

    if (data != null) {
        $.each(data.d, function (i, o) {
            if (data.d[i].ApprovalTag != "ZD0") {
                Common.SetApproval(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].UserName, data.d[i].DeptID, data.d[i].DeptName, data.d[i].TitleID, data.d[i].TitleName, data.d[i].DutyID, data.d[i].DutyName, data.d[i].IsGroup, data.d[i].IsSubTree, "");
            }
        });
    }
}

/**************************************************
작성목적 : 부서정보가 변경된 이전결재라인 가져오기
***************************************************/
SubmitAdd.GetModfySeq = function (_docID) {
    if (_docID == 0) return;

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetModfySeqBackup";
    ParamArray = { 'docID': _docID };
    var obj = eval("SubmitAdd.SuccessGetModfySeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
SubmitAdd.SuccessGetModfySeq = function (data, status) {
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
작성일자 : 2019-08-14
작성목적 : 문서 근거문서 가져오기 (웹서비스 호출)
***************************************************/
SubmitAdd.getBase = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBaseDoc";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID
    };

    var obj = eval("SubmitAdd.succeededBase");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 문서 근거문서 가져오기 (웹서비스 값 받기)
***************************************************/
SubmitAdd.succeededBase = function (data, status) {
    var baseObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpBaseTag = "";
    var beforeIDs = "";
    var Cnt = 0;
    for (i = 0; i < baseObj.dsBase.length; i++) {
        beforeIDs += "|" + baseObj.dsBase[i].ParentDocumentID + "^" + baseObj.dsBase[i].Name + ";";
        Cnt++;

        tmpBaseTag += "<ul class='mli7' id='" + baseObj.dsBase[i].ParentDocumentID + "'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif'onclick=\"BaseDel('" + baseObj.dsBase[i].ParentDocumentID + "','" + baseObj.dsBase[i].Name + "')\" style='cursor:pointer' />&nbsp;&nbsp;</span>"
             + "<div class='conBox'><span class='LNoready'>" + baseObj.dsBase[i].Subject + "</span><br />"
             + "<span class='sch_user'>" + baseObj.dsBase[i].Name + "</span></div></li></ul>";
    }

    $("#baseList").append(tmpBaseTag);

    BaseDocIDs = beforeIDs;
    BaseDocCnt = Cnt;
    $('#tbAddDocID').val(BaseDocIDs);

    //근거문서 타이틀
    if ($("#baseList").text() != "") {
        jQuery.each($("#uBase"), function () { this.style.display = ""; });
    }
    else {
        jQuery.each($("#uBase"), function () { this.style.display = "none"; });
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 리스트 페이지 이동
***************************************************/
SubmitAdd.goBefore = function () {

    ParamArray = {
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "SubmitList.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : 에러 발생시에 처리
***************************************************/
SubmitAdd.goError = function () {
    action_popup.alert(R_ReLoadDoc);
    location.href = "WFolder.aspx";
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
SubmitAdd.getAttach = function () {

    var url = "/DeskPlusMobile/Approval/Ws/Common.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("SubmitAdd.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-14
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
SubmitAdd.succeededAttach = function (data, status) {

    var Cnt = 0;
    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {
            var contentHtml = "<ul class='mli8'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' onclick=\"SubmitAdd.DelFile('" + data.d[i].FileName + "')\" style='cursor:pointer' />&nbsp;&nbsp;</span>"
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

    $("#form1").attr("action", FormActionPath);
    $("#form1").removeAttr("enctype");
    $("#form1").removeAttr("target");

}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-16
작성목적 : 문서관리 
***************************************************/
SubmitAdd.GetEDM = function () {

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
