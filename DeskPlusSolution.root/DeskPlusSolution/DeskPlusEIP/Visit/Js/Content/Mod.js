Mod = {}

var ResourceArray = [];
var ShareArray = []; 

/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
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

        //파일추가
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            Mod.AddFile();
        });

        //공유 대상 추가
        $('#cbtnAddShare').click(function (e) {
            e.preventDefault();
            Mod.AddShare();
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

        //기본공유자 
        Mod.GetShareList();

        //공유설정
        Mod.GetOpenList();

        //예약 자원 
        Mod.GetRmList();

        //첨부파일 
        Mod.GetFileList();

        //추가할 첨부 파일 초기화
        Mod.FileList();

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
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetShareList = function () {

    ParamArray = {
        'reserveID': 0
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetShareList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/GetShareList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetShareList = function (data, status) {

    $('#phShare').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTShare.htm?seq=' + Guids());
    $('#phShare').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.ShareResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 기본공유자 리스트 메뉴 다국어 처리 - 사용X
**********************************************************************************************/
Mod.ShareResource = function () {

    $("#r_sTitle")[0].innerHTML = R_Title;
    $("#r_sType")[0].innerHTML = R_Gubun;

    if ($("#ShareNoListData")[0] != undefined) {
        $("#ShareNoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 공유 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetOpenList = function () {

    ParamArray = {
        'reserveGroupID': ReserveGroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetOpenList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/GetOpenList";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 공유 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetOpenList = function (data) {

    if (data.d.length > 0) {
        for (var i = 0; i < data.d.length; i++) {
            Mod.AddAuthRow(data.d[i].Type + "$" + data.d[i].OwnerID, data.d[i].Title);
        }
    }

    /*
    else {
        Mod.OpenList();
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
Mod.AddAuthRow = function (keyID, name) {

    RowShareMax = RowShareMax + 1;

    $("#phShareList").append(
		      "<li id='ShareUser" + RowShareMax + "'>"
			+ "<span>" + name + "</span>"
            + "<a onclick=Mod.DelShareRow(" + RowShareMax + ",'" + keyID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
            + "</li>"
	);   


    Mod.AddShareArray(keyID, name)
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddShareArray = function (keyID, keyName) {
    ShareArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : table 특정 row 삭제하기
***************************************************/
Mod.DelShareRow = function (index, keyID) {
    var row = $("#ShareUser" + index);

    for (var i = 0; i < ShareArray.length; i++) {

        if (ShareArray[i].id == keyID) {
            ShareArray.splice(i, 1);

            row.remove();

            break;
        }
    }

    /*
    if ((ShareArray.length == 0) && (keyID != 0)) {
        Mod.OpenList();
    }
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-07
작성목적 : 공유자 리스트 보여주기 - 사용X
**********************************************************************************************/
Mod.OpenList = function () {

    $("#contentShare").append(
		"<tr id='ShareUser0'>"
			+ "<td colspan='2' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Mod.AddShareArray(0, 'NO');
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 자원 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetRmList = function () {

    ParamArray = {
        'reserveID': ReserveID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetRmList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/GetRmList";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 자원 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetRmList = function (data) {

    if (data.d.length > 0) {
        for (var i = 0; i < data.d.length; i++) {
            Mod.AddRwRow(data.d[i].Title, data.d[i].Qty, data.d[i].ListID, data.d[i].UsableQty, data.d[i].Authority);
        }
    }

    /*
    else {
        Mod.ResoEmptyList();
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
Mod.AddRwRow = function (_tmpTitle, _tmpQTy, _tmpListID, _tmpUsableQty, _tmpAuthority) {

    RowMax = RowMax + 1;

    $("#phRMList").append(
		"<li id='VisitAdd" + RowMax + "'>"
        + "<span>" + _tmpTitle + " (" + _tmpQTy + ")</span>"
        + "<span id='tdUsableQty" + _tmpListID + "' style='display:none'>" + _tmpUsableQty + "</span>"
        + "<span id='tdAuthority" + _tmpListID + "' style='display:none'>" + _tmpAuthority + "</span>"
        + "<span id='tdQty" + _tmpListID + "' style='display:none'>" + _tmpQTy + "</span>"
        + "<a onclick=Mod.DelRow(" + RowMax + ",'" + _tmpListID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
	);


    Mod.AddArray(_tmpListID, _tmpTitle);
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddShareArray = function (keyID, keyName) {
    ShareArray.push({
        'id': keyID,
        'name': keyName
    });
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 첨부 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetFileList = function () {

    ParamArray = {
        'reserveGroupID': ReserveGroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessFileList");
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 첨부 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTModFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.FileResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : 추가할 첨부파일 초기화
**********************************************************************************************/
Mod.FileList = function () {

    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate("");

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 추가할 이미지 다국어 처리 - 사용X
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
작성일자 : 2014-02-18
작성목적 : 실제 파일 삭제 처리 (웹서비스 호출)
**********************************************************************************************/
Mod.DelFile = function (_attachFileID) {

    if (window.confirm(R_DelAttach)) {
        ParamArray = {            
            'reserveGroupID': ReserveGroupID,
            'attachFileID': _attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelFile");
        var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : 첨부파일 추가
***************************************************/
Mod.AddFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'Visit',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {

    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : 반복설정 
***************************************************/
function AddRepeatInfo(_resultInfo) {
    $("#txtrepeat").val(_resultInfo);
    $("#endDate").hide();

    if ($("#hdRepeatChk").val() == "Y") {
        $("#hdRepeatChk").val('N');
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
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

}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 자원추가
***************************************************/
Mod.AddResource = function () {

    var url = "/DeskPlusEIP/RM/Content/AddResource.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Resource", "1000", "500")
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-12
작성목적 : 자원추가
***************************************************/
function SuccessAddResource(_tmpTitle, _tmpQTy, _tmpListID, _tmpUsableQty, _tmpAuthority) {

    //Mod.DelRow(0, 0);
    RowMax = RowMax + 1;

    $("#phRMList").append(
		"<li id='VisitAdd" + RowMax + "'>"
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
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
작성목적 : table 특정 row 삭제하기
***************************************************/
Mod.DelRow = function (index, keyID) {
    var row = $("#VisitAdd" + index);

    for (var i = 0; i < ResourceArray.length; i++) {

        if (ResourceArray[i].id == keyID) {
            ResourceArray.splice(i, 1);

            row.remove();

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
작성일자 : 2014-02-12
작성목적 : 자원 없을때 초기화 보여주기 - 사용X
**********************************************************************************************/
Mod.ResoEmptyList = function () {

    $("#contentTable").append(
		"<tr id='VisitAdd0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Mod.AddArray(0, 'NO');
}


/**************************************************************
작성자 : famo23
작성일자 : 2014-02-12
작성목적 : 취소
**************************************************************/
Mod.Cancel = function () {

    ParamArray = {
        'tmpdate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun,
        'reserveID': ReserveID
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;

}

/**************************************************************
작성자 : famo23
작성일자 : 2014-02-12
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


    //공유자 설정
    var arrHiddenShare = "";

    for (var i = 0; i < ShareArray.length; i++) {
        if (ShareArray[i].id != "0") {
            arrHiddenShare += ShareArray[i].id + "|";
        }
    }

    $("#tbHiddenShare").val(arrHiddenShare);

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
            var url = "/DeskPlusEIP/Visit/Content/ConfirmRepeat.aspx";
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
작성일자 : 2014-02-12
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 호출)
************************************************************************/
Mod.UseResource = function () {

    var arrListID = "";
    var arrQty = "";
    var arrUsableQty = "";
    var arrHiddenResource = "";

    for (var i = 0; i < ResourceArray.length; i++) {
        if (ResourceArray[i].id != "0") {
            arrListID += ResourceArray[i].id + ",";
            arrQty += $("#tdQty" + ResourceArray[i].id).text() + "$";
            arrUsableQty += $("#tdUsableQty" + ResourceArray[i].id).text() + "$";
            arrHiddenResource += ResourceArray[i].id + "$" + $("#tdQty" + ResourceArray[i].id).text() + "$" + $("#tdAuthority" + ResourceArray[i].id).text() + "|";
        }
    }

    if (arrHiddenResource != "") {

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
            'reserveID': ReserveID,
            'reserveGroupID': ReserveGroupID,
            'repeatChk': $("#hdRepeatChk").val(),
            'repeatSel': $("#hdRepeatSel").val(),
            'beforeStartDate': BeforeStartDate
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessUseResource");
        var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/UseResource";

        AjaxService(url, data, obj);
    }
    else {
        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
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
작성일자 : 2014-02-12
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
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-12
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


/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 공유자 추가 
***************************************************/
Mod.AddShare = function () {

    ParamArray = {
        'callBackEvent': "Mod.SuccessShareList"
    };

    CallEncrypt(CallGetParam());
    var url = "AddShare.aspx?p=" + GetEncryptValue;
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Resource", "700", "485")

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 추가된 공유자 리스트 보여주기
**********************************************************************************************/
Mod.SuccessShareList = function (data) {

    //Mod.DelShareRow(0, 0);

    var arrData = data.split(';');
    for (i = 0; i < arrData.length; i++) {
        var arrID = arrData[i].split('|');

        Mod.CompareArrayVal(arrID[0], arrID[1]);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-13
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Mod.CompareArrayVal = function (compID, compName) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (ShareArray.length > 0) {
        for (var i = 0; i < ShareArray.length; i++) {

            if (ShareArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우                
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            Mod.AddAuthRow(compID, compName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        Mod.AddAuthRow(compID, compName);
    }

    return response;
}