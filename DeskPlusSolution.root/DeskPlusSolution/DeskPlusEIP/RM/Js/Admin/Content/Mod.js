Mod = {}

var BeforeArray = []; //db 에서 권한 값을 권한배열에 담는다.

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
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

        //첨부파일 추가
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            Mod.AddFile();
        });

        //자원 담당자 추가
        $('#cbtnAddUser').click(function (e) {
            e.preventDefault();
            Mod.AddUser();
        });

        //이미지 파일
        Mod.GetFileList();

        //추가할 이미지 파일 초기화
        Mod.FileList();

        //자원담당자
        Mod.GetUserList();
    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 이미지 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessFileList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/Mod.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 이미지 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTModFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);    

    /*
    setTimeout(function () {
        Mod.FileResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 리스트 메뉴 다국어 처리
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

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-22
작성목적 : 취소
**************************************************************/
Mod.Cancel = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 추가할 이미지 초기화
**********************************************************************************************/
Mod.FileList = function () {
    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate("");

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
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
작성일자 : 2012-02-22
작성목적 : 실제 파일 삭제 처리 (웹서비스 호출)
**********************************************************************************************/
Mod.DelFile = function (_attachFileID) {
    if (window.confirm(R_DelAttach)) {

        ParamArray = {
            'categoryID': TmpCategoryID,
            'listID': ListID,
            'attachFileID': _attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDelFile");
        var url = "/DeskPlusEIP/RM/Ws/Admin/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
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
작성일자 : 2012-02-22
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
작성일자 : 2012-02-22
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
작성일자 : 2012-02-22
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    $('#phSpoolFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#phSpoolFile').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.FilSpooleResource();
    }, 100);
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 담당자 추가
***************************************************/
Mod.AddUser = function () {
    ParamArray = {
        'callBackEvent': "Mod.SuccessUserList"
    };

    CallEncrypt(CallGetParam());
    var url = "AddUser.aspx?p=" + GetEncryptValue;
    PopupWin(700, 485, url, "", "", true);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 자원담당자 리스트 보여주기
**********************************************************************************************/
Mod.SuccessUserList = function (data) {
    //Mod.DelRow(0, 0);

    var arrData = data.split(';');
    for (i = 0; i < arrData.length; i++) {
        var arrID = arrData[i].split('|');

        Mod.CompareArrayVal(arrID[0], arrID[1]);
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-22
작성목적 : 자원 수정
**************************************************************/
Mod.Confirm = function () {
    if (BtnCheck == "Y") {
        alert(R_Processing);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputTitle, true);
    if (!validator.validate()) return;

    validator.checkRequired("ctbCount", R_InputCount, true);
    if (!validator.validate()) return;

    validator.checkOnlyNumber("ctbCount", R_InputNumber, true);
    if (!validator.validate()) return;

    validator.checkOnlyNumber("ctbReCount", R_InputNumber, true);
    if (!validator.validate()) return;

    if ($("#ctbCount").val() < $("#ctbReCount").val()) {
        alert(R_ResourceCount);
        return;
    }

    //자원 담당자 확인
    var UserVal = $(".radioUser:checked").val();

    if (UserVal != "" && UserVal != undefined) {
        var tmpManager = "";
        for (var i = 0; i < BeforeArray.length; i++) {
            tmpManager += BeforeArray[i].id + "|" + BeforeArray[i].name + ";";
        }

        tmpManager = tmpManager.trimEnds(';');

        $("#tbManager").val(tmpManager);
        $("#tbManagerOne").val(UserVal);

        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        alert(R_SelectUser);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
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

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 자원 담당자 리스트 (웹서비스 호출)
**********************************************************************************************/
Mod.GetUserList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetUserList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/Mod.asmx/GetUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 자원 담당자 리스트 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetUserList = function (data) {
    for (var i = 0; i < data.d.length; i++) {
        Mod.AddAuthRow(data.d[i].ManagerUserID, data.d[i].ManagerName, data.d[i].Gubun);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 추가 table row 생성하고 권한값 BeforeArray[] 배열에 할당하기
***************************************************/
Mod.AddAuthRow = function (keyID, name, gubun) {

    RowMax = RowMax + 1;

    if (gubun == "Y") {
        gubun = "checked";
    }

    $("#phUserList").append(
          "<li id='RMUser" + RowMax + "'>"
        + "<span>" + R_ManagerGubun1 + "</span>"
        + "<span class='mount' style='position:relative; display:inline-block;width:20px; padding-left:4'>"
        + "<input type='radio' class='radioUser' name='radioUser' " + gubun + " value='" + keyID + "' style='position:absolute; top:-11px;' />"
        + "</span> / "
        + "<a onclick=\"AccountInfo('" + keyID + "');return false;\">" + name + "</a>"
        + "<a onclick=Mod.DelRow(" + RowMax + ",'" + keyID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle'></i></a>"
        + "</li>"
    );

    /*    
    if (CheckYN == "N") {
        gubun = "checked";
        CheckYN = "Y";
    }

    $("#contentTable").append(
        "<tr id='RMUser" + RowMax + "'>"
        + "<td class='center'>"
        + "<a href=\"#\" onclick=\"AccountInfo('" + keyID + "');return false;\">" + name + "</a>"
        + "</td>"
        + "<td class='center'>"
        + "<input type='radio' class='radioUser' name='radioUser' " + gubun + " value='" + keyID + "' />"
        + "</td>"
        + "<td class='center'><a onclick=Mod.DelRow(" + RowMax + ",'" + keyID + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
        + "</tr>"
    );
    */

    Mod.AddBeforeArray(keyID, name)

}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 권한 배열값 저장소; 비교 대상자 배열값
***************************************************/
Mod.AddBeforeArray = function (keyID, keyName) {
    BeforeArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : table 특정 row 삭제하기
***************************************************/
Mod.DelRow = function (index, keyID) {
    var row = $("#RMUser" + index);

    for (var i = 0; i < BeforeArray.length; i++) {
        if (BeforeArray[i].id == keyID) {
            BeforeArray.splice(i, 1);
            row.remove();
            break;
        }
    }


    /*
    if ((BeforeArray.length == 0) && (keyID != 0)) {
        Mod.ResoEmptyList();
    }
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 자원 없을때 초기화 보여주기 - 사용X
**********************************************************************************************/
Mod.ResoEmptyList = function () {
    CheckYN = "N";

    $("#contentTable").append(
		"<tr id='RMUser0'>"
			+ "<td colspan='3' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>"
	);

    Mod.AddBeforeArray(0, 'NO');
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
Mod.CompareArrayVal = function (compID, compName) {
    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    if (BeforeArray.length > 0) {
        for (var i = 0; i < BeforeArray.length; i++) {
            if (BeforeArray[i].id == compID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
            else {
                response = 0; //다른 값일 경우
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            Mod.AddAuthRow(compID, compName, '');
        }
    }
    else {
        response = 0; //다른 값일 경우
        Mod.AddAuthRow(compID, compName, '');
    }

    return response;
}
