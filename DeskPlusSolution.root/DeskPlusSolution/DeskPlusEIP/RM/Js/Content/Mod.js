Mod = {}

var ResourceArray = [];
var ResourceUserArray = []; //db 에서 권한 값을 권한배열에 담는다.
var ShareUserArray = []; //공유자

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 초기화
***************************************************/
$(function () {

    $(document).ready(function () {
        //확인(수정)
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Confirm();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //반복설정
        $('#cbtnRepeat').click(function (e) {
            e.preventDefault();
            Startdate = $('#startDate').val();
            Mod.AddRepeat();
        });

        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            Mod.CancelRepeat();
        });

        //자원 추가
        $('#cbtnAddResource').click(function (e) {
            e.preventDefault();
            Mod.AddResource();
        });

        //공유자 추가
        $('#cbtnAddShareUser').click(function (e) {
            e.preventDefault();
            Mod.AddShareUser();
        });

        //파일추가
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            Mod.AddFile();
        });

        //달력
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $(".clsDate").keydown(function (e) { return false; });

        //첨부파일
        Mod.GetFileList();

        //공유자
        Mod.GetShareUserList();

        //추가할 첨부 파일 초기화
        Mod.FileList();

        //예약 자원
        Mod.AddArray(RListID, RTitle);

        // 반복일정일때 endDate를 설정하는게 무의미
        if ($("#txtrepeat").val() != "") {
            $("#endDate").hide();
        }

        xfe.setHtmlValue($("#hdContent").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#hdContent").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 첨부 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetFileList = function () {
    ParamArray = {
        'reservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessFileList");
    var url = "/DeskPlusEIP/RM/Ws/Content/Mod.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 첨부 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTModFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.FileResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
Mod.FileResource = function () {

    $("#j_m_fileName")[0].innerHTML = R_FileName;
    $("#j_m_fileType")[0].innerHTML = R_FileType;
    $("#j_m_fileLength")[0].innerHTML = R_FileLength;
    $("#j_m_filedelete")[0].innerHTML = R_Del;

    if ($("#NoModListData")[0] != undefined) {
        $("#NoModListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 추가할 첨부파일 초기화
**********************************************************************************************/
Mod.FileList = function () {
    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate("");

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 추가할 이미지 다국어 처리
**********************************************************************************************/
Mod.FilSpooleResource = function () {

    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLength")[0].innerHTML = R_FileLength;
    $("#j_filedelete")[0].innerHTML = R_Del;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 실제 파일 삭제 처리 (웹서비스 호출)
**********************************************************************************************/
Mod.DelFile = function (_attachFileID) {
    if (window.confirm(R_DelAttach)) {

        ParamArray = {
            'reservationID': ReservationID,
            'attachFileID': _attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelFile");
        var url = "/DeskPlusEIP/RM/Ws/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 실제 파일 삭제 처리 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessDelFile = function (data, status) {
    if (data.d) {
        Mod.GetFileList();
    }
    else {
        alert(R_ErrorDelFile);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 첨부파일 추가
***************************************************/
Mod.AddFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'RM',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Mod.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
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
        var obj = eval("Mod.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 반복설정
***************************************************/
Mod.AddRepeat = function () {
    var url = "AddRepeat.aspx";

    var getStartDate = $("#startDate").val();

    ParamArray = {
        'startDate': getStartDate
    };

    CallEncrypt(CallGetParam());

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "330")
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 반복설정
***************************************************/
function AddRepeatInfo(_resultInfo) {
    $("#txtrepeat").val(_resultInfo);
    $("#endDate").hide();

    if ($("#hdRepeatChk").val() == "Y") {
        $("#hdRepeatChk").val('N');
    }
}
/*
function AddRepeatInfo(_ddlResult, _resultInfo, _repeatValue) {
$("#txtrepeat").val(_resultInfo);
//$("#tbRepeat").val(_ddlResult);
$("#tbRepeatVal").val(_repeatValue);

if ($("#hdRepeatChk").val() == "Y") {
$("#hdRepeatChk").val('N');
}

$("#endDate").hide();
}
*/
/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 반복 설정 취소
***************************************************/
Mod.CancelRepeat = function () {
    $("#txtrepeat").val('');
    $("#endDate").show();

    $("#hdRepeatType").val('');
    $("#hdSelectType").val('');
    $("#hdIntervalCnt").val('');
    $("#hdOption1").val('');
    $("#hdOption2").val('');
    $("#hdOption3").val('');
    $("#hdOption4").val('');
    $("#hdOption5").val('');
    $("#hdOption6").val('');
    $("#hdOption7").val('');
    $("#hdMaxReptCnt").val('');
    $("#hdEndDate").val('');

    $("#tbRepeat").val('');
    /*
    $("#txtrepeat").val('');
    $("#tbRepeat").val('');
    $("#tbRepeatVal").val('');

    $("#endDate").show();
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 자원추가
***************************************************/
Mod.AddResource = function () {
    if (ResourceArray.length > 0) {
        if (ResourceArray[0].id != 0) {
            alert(R_ResourceOne);
            return;
        }
    }

    var url = "AddResource.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Resource", "1000", "500")
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 자원추가
***************************************************/
function SuccessAddResource(_tmpTitle, _tmpQTy, _tmpListID, _tmpUsableQty, _tmpAuthority) {
    //Mod.DelRow(0, 0);
    RowMax = RowMax + 1;

    $("#phRMList").append(
		"<li id='RMAdd" + RowMax + "'>"
        + "<span>" + _tmpTitle + " (" + _tmpQTy + ")</span>"
        + "<span id='tdUsableQty" + _tmpListID + "' style='display:none'>" + _tmpUsableQty + "</span>"
        + "<span id='tdAuthority" + _tmpListID + "' style='display:none'>" + _tmpAuthority + "</span>"
        + "<span id='tdQty" + _tmpListID + "' style='display:none'>" + _tmpQTy + "</span>"
        + "<a onclick=Mod.DelRow(" + RowMax + ",'" + _tmpListID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
	);

    Mod.AddArray(_tmpListID, _tmpTitle)

}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddArray = function (keyID, keyName) {
    ResourceArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : table 특정 row 삭제하기
***************************************************/
Mod.DelRow = function (index, keyID) {
    var row = $("#RMAdd" + index);

    for (var i = 0; i < ResourceArray.length; i++) {
        if (ResourceArray[i].id == keyID) {
            ResourceArray.splice(i, 1);

            //row.fadeOut("slow", function () {
            row.remove();
            //});

            break;
        }
    }

    /*
    if ((ResourceArray.length == 0) && (keyID != 0)) {
        Mod.ResoEmptyList();
    }
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 자원 없을때 초기화 보여주기 - 사용X
**********************************************************************************************/
Mod.ResoEmptyList = function () {
    $("#contentTable").append(
		"<tr id='RMAdd0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Mod.AddArray(0, 'NO');
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-23
작성목적 : 취소
**************************************************************/
Mod.Cancel = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpdate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun,
        'reservationID': ReservationID,
        'groupGubun': GroupGubun
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-23
작성목적 : 예약 수정
**************************************************************/
Mod.Confirm = function () {
    if (BtnCheck == "Y") {
        alert(R_Processing);
        return;
    }

    //에디터 처리
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

            alert(R_EditorScript);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    //제목 입력 필수
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputSubject, true);
    if (!validator.validate()) return;

    //자원 선택 필수
    //if (ResourceArray[0].id == 0) {
    if (ResourceArray.length == 0) {
        alert(R_SelectResources);
        return;
    }

    //공유자를 선택 하였을경우
    var arrHiddenShareUser = "";
    if (ShareUserArray.length > 0) {
        if (ShareUserArray[0].id != 0) {
            for (var i = 0; i < ShareUserArray.length; i++) {
                arrHiddenShareUser += ShareUserArray[i].id + "|";
            }
        }
    }
    $("#tbHiddenShareUser").val(arrHiddenShareUser);

    // 패턴 선택 필수
    //    if ($(':radio[name="RblPattern"]:checked').length < 1) {
    //        alert(R_SelectGubun);
    //        return;
    //    }

    //날짜 확인
    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();
    var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
    var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

    if ($("#txtrepeat").val() != "") {
        if (getStartDate == "") {
            alert(R_ApplyStartDate);
            return false;
        }
        else {
            if (eTime <= sTime) {
                alert(R_StartDateEndDiffer);
                return;
            }
        }

        if ($("#hdRepeatChk").val() != "") {
            var url = "/DeskPlusEIP/RM/Content/ConfirmRepeat.aspx";
            PopupWin(400, 270, url, "", "", true);
        }
        else {
            Mod.GetRepeat();
        }
    }
    else {
        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return false;
            }

            var dtResponse1 = CompareDT(getStartDate, getEndDate, "equal", PreLangReal);
            if (dtResponse1 == true) {
                if (eTime <= sTime) {
                    alert(R_StartDateEndDiffer);
                    return;
                }
            }

        }

        Mod.UseResource();
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 호출)
************************************************************************/
Mod.UseResource = function () {
    var arrListID = "";
    var arrQty = "";
    var arrUsableQty = "";
    var arrHiddenResource = "";

    for (var i = 0; i < ResourceArray.length; i++) {
        arrListID += ResourceArray[i].id + ",";
        arrQty += $("#tdQty" + ResourceArray[i].id).text() + "$";
        arrUsableQty += $("#tdUsableQty" + ResourceArray[i].id).text() + "$";
        arrHiddenResource += ResourceArray[i].id + "$" + $("#tdQty" + ResourceArray[i].id).text() + "$" + $("#tdAuthority" + ResourceArray[i].id).text() + "|";
    }

    $("#tbHiddenResource").val(arrHiddenResource);

    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();
    var getSTime = $("#ddlstartTime").val();
    var getETime = $("#ddlendTime").val();
    var repeat = $("#tbRepeat").val();

    ParamArray = {
        'listID': arrListID,
        'qty': arrQty,
        'usableQty': arrUsableQty,
        'startDate': getStartDate,
        'endDate': getEndDate,
        'sTime': getSTime,
        'eTime': getETime,
        'repeat': repeat,
        'reservationID': ReservationID,
        'parentReservationID': ParentReservationID,
        'repeatChk': $("#hdRepeatChk").val(),
        'repeatSel': $("#hdRepeatSel").val(),
        'beforeStartDate': BeforeStartDate
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessUseResource");
    var url = "/DeskPlusEIP/RM/Ws/Content/Mod.asmx/UseResource";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-23
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessUseResource = function (data, status) {
    if (data.d) {
        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        alert(R_UsableResource);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-13
작성목적 : 자원 반복 설정
**********************************************************************************************/
function RepeatMod(_arg) {
    $("#hdRepeatSel").val(_arg);

    if (($("#hdRepeatChk").val() == "Y") && ($("#hdRepeatSel").val() == "1")) {
        Mod.UseResource();
    }
    else {
        Mod.GetRepeat();
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-03-14
작성목적 : 반복 일자 가져오기 (웹서비스 호출)
************************************************************************/
Mod.GetRepeat = function () {
    ParamArray = {
        'repeatType': $("#hdRepeatType").val(),
        'selectType': $("#hdSelectType").val(),
        'intervalCnt': $("#hdIntervalCnt").val(),
        'option1': $("#hdOption1").val(),
        'option2': $("#hdOption2").val(),
        'option3': $("#hdOption3").val(),
        'option4': $("#hdOption4").val(),
        'option5': $("#hdOption5").val(),
        'option6': $("#hdOption6").val(),
        'option7': $("#hdOption7").val(),
        'maxReptCnt': $("#hdMaxReptCnt").val(),
        'endDate': $("#hdEndDate").val(),
        'startDate': $("#startDate").val(),
        'repeatChk': $("#hdRepeatChk").val(),
        'repeatSel': $("#hdRepeatSel").val(),
        'groupStartDate': GroupStartDate
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetRepeat");
    var url = "/DeskPlusEIP/RM/Ws/Content/Mod.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-14
작성목적 : 반복 일자 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetRepeat = function (data, status) {
    if (data.d != "") {
        $("#tbRepeat").val(data.d);
        Mod.UseResource();
    }
    else {
        alert(R_NoRepeatDay);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 공유자 리스트 초기화 보여주기 - 사용 안 함
**********************************************************************************************/
Mod.ShareResoList = function () {
    $("#contentTable_Share").append(
		"<tr id='RMShareAdd0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Mod.AddShareUserArray(0, 'NO');
}

/**************************************************
작성자:김광일
작성일:2011-10-26
작성목적 : 공유자 선택
***************************************************/
Mod.AddShareUser = function () {

    var url = "/DeskPlusEIP/RM/Content/AddShareUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "ShareUser", "700", "485")
}

function SuccessAddShareUser(_selShareUser) {
    //Mod.DelShareRow(0, 0);
    $.each(_selShareUser, function (i, v) {
        ShareRowMax = ShareRowMax + 1;

        var vName = "";
        if (v.id.split('$')[0] == "M") {
            vName = v.name.split('/')[2];
        }
        else {
            vName = v.name;
        }

        $("#phShareList").append(
		          "<li id='RMShareAdd" + ShareRowMax + "'>"
			    + "<span>(" + ((v.id.split('$')[0] == "M") ? R_AttendeeType2 : R_AttendeeType1) + ")</span>"
			    + "<span class='mount'>" + vName + "</span>"
                + "<a onclick=Mod.DelShareRow(" + ShareRowMax + ",'" + v.id + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
                + "</li>"
	    );   

        Mod.AddShareUserArray(v.id, v.name);
    });

}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 배열값(공유자) 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddShareUserArray = function (id, name) {
    ShareUserArray.push({
        'id': id,
        'name': name
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : table(공유자) 특정 row 삭제하기
***************************************************/
Mod.DelShareRow = function (index, keyID) {
    var row = $("#RMShareAdd" + index);

    for (var i = 0; i < ShareUserArray.length; i++) {
        if (ShareUserArray[i].id == keyID) {
            ShareUserArray.splice(i, 1);
            row.remove();
            break;
        }
    }

    /*
    if ((ShareUserArray.length == 0) && (keyID != 0)) {
        Mod.ShareResoList();
    }
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-20
작성목적 : 공유자  리스트 호출
**********************************************************************************************/
Mod.GetShareUserList = function () {
    ParamArray = {
        'reservationID': ParentReservationID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessShareUserList");
    var url = "/DeskPlusEIP/RM/Ws/Content/View.asmx/GetShareUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-20
작성목적 : 공유자 리스트 성공시 페이지 표시
**********************************************************************************************/
Mod.SuccessShareUserList = function (data, status) {
    if (data.d.length > 0) {
        for (var i = 0; i < data.d.length; i++) {
            ShareRowMax = ShareRowMax + 1;
            var vID = "";
            if (data.d[i].Kind == "M") {
                vID = data.d[i].Kind + "$" + data.d[i].UserID;
            }
            else {
                vID = data.d[i].Kind + "$" + data.d[i].DeptID
            }

            $("#phShareList").append(
                  "<li id='RMShareAdd" + ShareRowMax + "'>"
                + "<span>(" + ((data.d[i].Kind == "M") ? R_AttendeeType2 : R_AttendeeType1) + ")</span>"
                + "<span class='mount'>" + data.d[i].NameBase + "</span>"
                + "<a onclick=Mod.DelShareRow(" + ShareRowMax + ",'" + vID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
                + "</li>"
                ); 

            Mod.AddShareUserArray(vID, data.d[i].NameBase);
        }
    }

    /*
    else {
        Mod.ShareResoList();
    }
    */
}