Add = {}
var ResourceArray = []; //예약자원
var ResourceUserArray = []; //db 에서 권한 값을 권한배열에 담는다.
var ShareUserArray = []; //공유자

/**************************************************
작성자   : famo23
작성일자 : 2012-02-10
작성목적 : 초기화
***************************************************/
$(function () {

    if (CategoryID == 21 || CategoryID == 22 || CategoryID == 23) {
        var agent = navigator.userAgent.toLowerCase();
        if ((navigator.appName == 'Netscape' && agent.indexOf('trident') != -1) || (agent.indexOf("msie") != -1)) {
            setTimeout(function () {
                form1.twe.BodyValue = "<span style=\"color: rgb(152, 152, 153);\">- 사용목적 :<br><br>- 사용인원 : (ex : 통합기술팀 OO명 + 연구소 OO명 + 외부인원 OO명)<br><br>- 요청사항 : (ex : 빔프로젝터, 노트북, HDMI 케이블 외)<br><br>- 특이사항 기재 :</span>";
            }, 2000);
        }
        else {
            setTimeout(function () {
                xfe.setHtmlValue("<span style=\"color: rgb(152, 152, 153);\">- 사용목적 :<br><br>- 사용인원 : (ex : 통합기술팀 OO명 + 연구소 OO명 + 외부인원 OO명)<br><br>- 요청사항 : (ex : 빔프로젝터, 노트북, HDMI 케이블 외)<br><br>- 특이사항 기재 :</span>");
            }, 2000);
        }
    }
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //반복설정
        $('#cbtnRepeat').click(function (e) {
            e.preventDefault();
            Startdate = $('#startDate').val();
            Add.AddRepeat();
        });

        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            Add.CancelRepeat();
        });

        //자원 추가
        $('#cbtnAddResource').click(function (e) {
            e.preventDefault();
            Add.AddResource();
        });

        //공유자 추가
        $('#cbtnAddShareUser').click(function (e) {
            e.preventDefault();
            Add.AddShareUser();
        });

        //파일추가
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            Add.AddFile();
        });

        //첨부파일 초기화
        Add.FileList();

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

        //예약자원
        if (ListID > 0) {
            SuccessAddResource(ListTitle, ListQty, ListID, ListUsableQty, ListAuthority);
        }

        /*
        else {
            Add.ResoList();
        }

        Add.ShareResoList();
        */

        if ($("#tbHiddenMime_contents")) {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        }
        else {
            xfe.setHtmlValue(form1.tbHiddenMime_contents.value);
        }
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            if ($("#tbHiddenMime_contents")) {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            }
            else {
                xfe.setHtmlValue(form1.tbHiddenMime_contents.value);
            }
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "Add.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "AddN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 공유자 리스트 초기화 보여주기 - 사용X
**********************************************************************************************/
Add.ShareResoList = function () {
    $("#contentTable_Share").append(
		"<tr id='RMShareAdd0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Add.AddShareUserArray(0, 'NO');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 첨부파일 리스트 초기화 보여주기
**********************************************************************************************/
Add.FileList = function () {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate("");

    /*
    setTimeout(function () {
        Add.FileResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 리스트 메뉴 다국어 처리 
**********************************************************************************************/
Add.FileResource = function () {

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
작성일자 : 2012-02-15
작성목적 : 자원담당자 리스트 초기화 보여주기 -사용X
**********************************************************************************************/
Add.ResoList = function () {
    $("#contentTable").append(
		"<tr id='RMAdd0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Add.AddArray(0, 'NO');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Add.ResoResource = function () {
    $("#j_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#j_qty")[0].innerHTML = R_Qty;
    $("#j_managerName")[0].innerHTML = R_ResourcesUser;
    $("#j_delete")[0].innerHTML = R_Del;

    if ($("#NoResourcesListData")[0] != undefined) {
        $("#NoResourcesListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 첨부파일 추가
***************************************************/
Add.AddFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'RM',
        'callBackEvent': "Add.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-13
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
작성일자 : 2012-02-13
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Add.FileResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-13
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

/**************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 반복설정
***************************************************/
Add.AddRepeat = function () {
    var url = "AddRepeat.aspx";

    var getStartDate = $("#startDate").val();

    if (getStartDate == "") {
        alert(R_ApplyStartDate);
        return;
    }

    ParamArray = {
        'startDate': getStartDate
    };

    CallEncrypt(CallGetParam());

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "330")
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 반복설정
***************************************************/
function AddRepeatInfo(_resultInfo) {
    $("#txtrepeat").val(_resultInfo);
    $("#endDate").hide();

    /*
    $("#txtrepeat").val(_resultInfo);
    $("#tbRepeat").val(_ddlResult);
    $("#tbRepeatVal").val(_repeatValue);

    $("#endDate").hide();
    */
    //$("#endDate").val('');
    //$("#endDate").attr("disabled", "disabled");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 반복 설정 취소
***************************************************/
Add.CancelRepeat = function () {
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
작성일자 : 2012-02-15
작성목적 : 자원추가
***************************************************/
Add.AddResource = function () {
    var url = "AddResource.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Resource", "1000", "500")
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 자원추가
***************************************************/
function SuccessAddResource(_tmpTitle, _tmpQTy, _tmpListID, _tmpUsableQty, _tmpAuthority) {
    //Add.DelRow(0, 0);
    RowMax = RowMax + 1;


    $("#phRMList").append(
		"<li id='RMAdd" + RowMax + "'>"
        + "<span>" + _tmpTitle + " (" + _tmpQTy + ")</span>"
        + "<span id='tdUsableQty" + _tmpListID + "' style='display:none'>" + _tmpUsableQty + "</span>"
        + "<span id='tdAuthority" + _tmpListID + "' style='display:none'>" + _tmpAuthority + "</span>"
        + "<span id='tdQty" + _tmpListID + "' style='display:none'>" + _tmpQTy + "</span>"
        + "<a onclick=Add.DelRow(" + RowMax + ",'" + _tmpListID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
	);


    Add.AddArray(_tmpListID, _tmpTitle);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 배열값 저장소; 비교 대상자 배열값
***************************************************/
Add.AddArray = function (keyID, keyName) {
    ResourceArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : table 특정 row 삭제하기
***************************************************/
Add.DelRow = function (index, keyID) {

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
        Add.ResoList();
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-16
작성목적 : 저장
***************************************************/
Add.Confirm = function () {
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

    //날짜 확인
    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();
    var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
    var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

    if ($("#txtrepeat").val() != "") {
        if (getStartDate == "") {
            alert(R_ApplyStartDate);
            return;
        }
        else {
            if (eTime <= sTime) {
                alert(R_StartDateEndDiffer);
                return;
            }
        }

        Add.GetRepeat();
    }
    else {
        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return;
            }

            var dtResponse1 = CompareDT(getStartDate, getEndDate, "equal", PreLangReal);
            if (dtResponse1 == true) {
                if (eTime <= sTime) {
                    alert(R_StartDateEndDiffer);
                    return;
                }
            }
        }
        Add.UseResource();
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-02-16
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 호출)
************************************************************************/
Add.UseResource = function () {
    var arrListID = "";
    var arrQty = "";
    var arrUsableQty = "";
    var arrHiddenResource = "";

    for (var i = 0; i < ResourceArray.length; i++) {
        arrListID += ResourceArray[i].id + ",";
        arrQty += $("#tdQty" + ResourceArray[i].id).text() + "$";
        arrUsableQty += $("#tdUsableQty" + ResourceArray[i].id).text() + "$";
        arrHiddenResource += ResourceArray[i].id + "$" + $("#tdQty" + ResourceArray[i].id).text() + "$" + $("#tdAuthority" + ResourceArray[i].id).text() + "|";

        if ($("#tdQty" + ResourceArray[i].id).text() == "0") {
            alert(R_QtyZero);
            return;
        }
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
        'repeat': repeat
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessUseResource");
    var url = "/DeskPlusEIP/RM/Ws/Content/Add.asmx/UseResource";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-16
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessUseResource = function (data, status) {
    if (data.d) {
        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        alert(R_UsableResource);
        return;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 취소
***************************************************/
Add.Cancel = function () {
    if (TagGubun == "my") {
        ParamArray = {
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());

        location.href = "MyList.aspx?p=" + GetEncryptValue;
    }
    else if (TagGubun == "main") {

        location.href = "/DeskPlusEIP/RM/Home/Main.aspx";
    }
    else {
        ParamArray = {
            'categoryID': CategoryID,
            'tmpDate': TmpDate,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'tagGubun': TagGubun,
            'groupGubun': GroupGubun
        };

        CallEncrypt(CallGetParam());

        location.href = "ListMain.aspx?p=" + GetEncryptValue;
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-03-14
작성목적 : 반복 일자 가져오기 (웹서비스 호출)
************************************************************************/
Add.GetRepeat = function () {
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
        'startDate': $("#startDate").val()
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetRepeat");
    var url = "/DeskPlusEIP/RM/Ws/Content/Add.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-14
작성목적 : 반복 일자 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessGetRepeat = function (data, status) {
    if (data.d != "") {
        $("#tbRepeat").val(data.d);

        Add.UseResource();
    }
    else {
        alert(R_NoRepeatDay);
        return;
    }
}

/**************************************************
작성자:김광일
작성일:2011-10-26
작성목적 : 공유자 선택
***************************************************/
Add.AddShareUser = function () {

    var url = "/DeskPlusEIP/RM/Content/AddShareUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "ShareUser", "700", "485")
}

function SuccessAddShareUser(_selShareUser) {
    //Add.DelShareRow(0, 0);
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
                + "<a onclick=Add.DelShareRow(" + ShareRowMax + ",'" + v.id + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
                + "</li>"
	    );   


        Add.AddShareUserArray(v.id, v.name);
    });

}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 배열값(공유자) 저장소; 비교 대상자 배열값
***************************************************/
Add.AddShareUserArray = function (id, name) {
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
Add.DelShareRow = function (index, keyID) {
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
        Add.ShareResoList();
    }
    */
}