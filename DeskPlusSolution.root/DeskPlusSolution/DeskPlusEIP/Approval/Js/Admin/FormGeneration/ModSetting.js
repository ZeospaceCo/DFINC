ModSetting = {}

var ZQZCArray = [];
var ZQZCRow = "";
var ZQZCRowMax = 0;

$(function () {
    $(document).ready(function () {

        //event
        $("#ddlType").change(function (e) { e.preventDefault(); ModSetting.SelFolderType('2'); });
        $("#ctbFormName").keyup(function (e) { ModSetting.SetMultiLang(); });

        // [결재라인순서]
        $('#cbtnSeqOrder').click(function (e) { e.preventDefault(); ModSetting.SeqOrder(); });
        // [결재라인지정]
        $('#cbtnSeqLine').click(function (e) { e.preventDefault(); ModSetting.SeqLine(); });
        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); ModSetting.Cancel(); });
        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); ModSetting.Confirm(); });

        // [폴더선택]
        $('#btnFolder').click(function (e) { e.preventDefault(); ModSetting.Folder(); });
        // [중복체크]
        $('#btnOverlap').click(function (e) { e.preventDefault(); ModSetting.Overlap(); });

        //수신추가
        $('#cbtnAddReceive').click(function (e) {
            e.preventDefault();
            ModSetting.AddRow('ZQ', 'Yes');
        });

        //참조추가
        $('#cbtnAddCarboncopy').click(function (e) {
            e.preventDefault();
            ModSetting.AddRow('ZC', 'Yes');
        });

        //수신참조정렬순서
        $('#ModSeq').click(function (e) {
            e.preventDefault();
            ModSetting.ModSeq();
        });

        //수신체크
        if (ZQCount < 1) {
            $("#cbtnAddReceive").css("display", "none");
        }

        //참조체크
        if (ZCCount < 1) {
            $("#cbtnAddCarboncopy").css("display", "none");
        }

        //수신참조설정 순서
        if (ZCCount < 1 && ZQCount < 1) {
            $("#ModSeq").css("display", "none");
        }

        ModSetting.ListRow();

        ModSetting.SelFolderType('1');
    });
});
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 양식명입력 시, 다국어처리
***************************************************/
ModSetting.SetMultiLang = function () {
    var objid = "ctbFormName" + PreLang.replace("-", "").toUpperCase();
    $("#" + objid).val($("#ctbFormName").val());
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 폴더구분 선택
***************************************************/
ModSetting.SelFolderType = function (_s) {
    if (parseInt($("#ddlType").val()) > 2) {        // 1.공통 / 2.자사 / 3.연동 / 4.결재망
        $(".trFolderInfo").hide();
        $("#tbFolderID").val($("#ddlType").val());
        $("#ctbFolderName").val("");
    } else {
        $(".trFolderInfo").show();
        if (_s == "2") {
            $("#tbFolderID").val("");
            $("#ctbFolderName").val("");
        }
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [결재라인순서]
***************************************************/
ModSetting.SeqOrder = function () {
    ParamArray = { 'formID': FormID };
    CallEncrypt(CallGetParam());
    var url = "ModSequence.aspx?p=" + GetEncryptValue;
    PopupWin("400", "400", url, "", "", true);
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [결재라인지정]
***************************************************/
ModSetting.SeqLine = function () {
    alert("결재망관리는 우선 추후에 하자"); return;
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [취소]
***************************************************/
ModSetting.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'formID': FormID
    };
    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [폴더선택]
***************************************************/
ModSetting.Folder = function () {
    //if (FolderID == 0) { alert(R_Msg_ErrSelFolder); return; }
    var folderID = $("#tbFolderID").val();
    var typeInfo = $("#ddlType").val();

    ParamArray = { 'folderID': folderID, 'typeInfo': typeInfo };
    CallEncrypt(CallGetParam());
    var url = "GetFolder.aspx?p=" + GetEncryptValue;
    PopupWin("300", "340", url, "", "", true);
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [중복체크]
***************************************************/
ModSetting.Overlap = function () {

    var nm = $("#ctbNickName").val();
    if (nm == "") { alert(R_Msg_InputNickName); $("#ctbNickName").focus(); return; }

    ParamArray = { 'nickName': nm };
    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModSetting.OverlapScs");
    var url = "/DeskPlusEIP/Approval/Ws/Admin/FormGeneration/ModSetting.asmx/GetNickName";
    AjaxService(url, data, obj);
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 
***************************************************/
ModSetting.OverlapScs = function (data, status) {
    if (data.d > 0) {
        alert(R_Msg_ErrorOverlap);
        $("#ctbNickName").val(); $("#ctbNickName").focus();
        return;
    }
    else {
        alert(R_Msg_SuccessOverlap);
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [확인]
***************************************************/
ModSetting.Confirm = function () {
    //필수입력

    var validator = new Validator($("#form1"));

    validator.checkRequired("ctbFormName", R_Msg_ErrInputName, true);

    //특수문자(^) 체크
    if (!ModSetting.CheckChar()) { alert(R_MSG_SPECIALCHAR); return; }

    if ($("#ddlType").val() == "1" || $("#ddlType").val() == "2") {
        validator.checkRequired("ctbFolderName", R_NeedFolderInfo2, true);
    }
    if (!validator.validate()) return;

    //수신참조체크
    var AppointerKind = "";
    var DeptID = "";
    var UserID = "";
    var IsDept = "";
    var IsActive = "";

    for (var i = 0; i < ZQZCArray.length; i++) {

        var chkCount = 0;

        for (var j = 0; j < ZQZCArray.length; j++) {
            if ($("#txtDeptID" + ZQZCArray[i].id).val() == $("#txtDeptID" + ZQZCArray[j].id).val() && $("#txtUserID" + ZQZCArray[i].id).val() == $("#txtUserID" + ZQZCArray[j].id).val() && $("#txtAppointerKind" + ZQZCArray[i].id).val() == $("#txtAppointerKind" + ZQZCArray[j].id).val() && i != j) {
                chkCount = chkCount + 1;
            }
        }

        if ($("#txtAppointerKind" + ZQZCArray[i].id).val() != "ZC") {
            for (var k = 0; k < ZQZCArray.length; k++) {
                if ($("#txtAppointerKind" + ZQZCArray[k].id).val() != "ZC") {
                    if ($("#txtDeptID" + ZQZCArray[i].id).val() == $("#txtDeptID" + ZQZCArray[k].id).val() && $("#txtAppointerKind" + ZQZCArray[i].id).val() == $("#txtAppointerKind" + ZQZCArray[k].id).val()) {
                        chkCount = chkCount + 1;
                    }
                }
            }
        }

        if (chkCount > 1) {
            alert(R_Msg_ErrorUserBefore);
            return;
        }

        //수신참조 사용자ID
        if ($("#txtUserID" + ZQZCArray[i].id).val() == "") {
            alert(R_MSG_ReceiveCarboncopy);
            return;
        }

        if (i == 0) {
            AppointerKind += $("#txtAppointerKind" + ZQZCArray[i].id).val();
            DeptID += $("#txtDeptID" + ZQZCArray[i].id).val();
            UserID += $("#txtUserID" + ZQZCArray[i].id).val();
            IsDept += $("#txtIsDept" + ZQZCArray[i].id).val();
            //IsActive += $("#IsActive" + ZQZCArray[i].id).val();
            IsActive = $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
        }
        else {
            AppointerKind += "," + $("#txtAppointerKind" + ZQZCArray[i].id).val();
            DeptID += "," + $("#txtDeptID" + ZQZCArray[i].id).val();
            UserID += "," + $("#txtUserID" + ZQZCArray[i].id).val();
            IsDept += "," + $("#txtIsDept" + ZQZCArray[i].id).val();
            //IsActive += "," + $("#IsActive" + ZQZCArray[i].id).val();
            IsActive += "," + $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
        }
    }

    if (DeptID != "undefined") {
        $("#tbAppointerKind").val(AppointerKind);
        $("#tbDeptID").val(DeptID);
        $("#tbUserID").val(UserID);
        $("#tbIsDept").val(IsDept);
        $("#tbIsActive").val(IsActive);
    }
    else {
        $("#tbAppointerKind").val("");
        $("#tbDeptID").val("");
        $("#tbUserID").val("");
        $("#tbIsDept").val("");
        $("#tbIsActive").val("");
    }

    __doPostBack('lbtnSubmit', "MOD");
}

// 특수문자 체크
ModSetting.CheckChar = function () {
    var rtnval = true;
    var chkObj = $("#ctbFormName, #ctbNameCode, #ctbFormNameKO, #ctbFormNameEN, #ctbFormNameJA, #ctbFormNameFR, #ctbFormNameZHCN, #ctbFormNameZHTW");
    $.each(chkObj, function (i, o) {
        if ($("#" + o.id).val().indexOf("^") > -1) {
            $("#" + o.id).focus(); rtnval = false;
        }
    });
    return rtnval;
}



/**************************************************
작성자   : seo
작성일자 : 
작성목적 : EDM 권한 폴더를 띄운다
***************************************************/
ModSetting.PopEDMAuthFolder = function (_encParams) {
    var targetURL = "/DeskPlusEDM/Authentication/LoginOutSide.aspx?p=" + _encParams;

    if ($.browser.msie) {  //크롬에서 window.showModalDialog 지원하지 않는 문제 
        var rtnVal = window.showModalDialog(targetURL, "", "dialogWidth:300px; dialogHeight:366px; center: Yes; help: No; resizable: No; status: No; scroll:No");

        ConfirmGetEDMFunc(rtnVal);
    } else {
        PopupWin(305, 370, url, "", "", true);
    }
}

function ConfirmGetEDMFunc(rtnVal) {
    //PopupWin(300, 366, url, "", "", true);
    //캐비넷값이 변경되었다면!!
    if (rtnVal != "" && rtnVal != undefined) {
        $("#ctbIsLinkedEtc").val(rtnVal.split('^')[0]);
    }
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 초기화
**********************************************************************************************/
ModSetting.ListRow = function () {

    var AppointerKind = $("#tbAppointerKind").val();
    var DeptID = $("#tbDeptID").val();
    var UserID = $("#tbUserID").val();
    var DeptName = $("#tbDeptName").val();
    var UserName = $("#tbUserName").val();
    var IsDept = $("#tbIsDept").val();
    var IsActive = $("#tbIsActive").val();

    var arrAppointerKind = AppointerKind.split(",");
    var arrDeptID = DeptID.split(",");
    var arrUserID = UserID.split(",");
    var arrDeptName = DeptName.split(",");
    var arrUserName = UserName.split(",");
    var arrIsDept = IsDept.split(",");
    var arrIsActive = IsActive.split(",");

    if (DeptID != "") {
        if (arrDeptID.length > 0) {
            var j = 1;
            for (var i = 0; i < arrDeptID.length; i++) {

                ModSetting.AddRow(arrAppointerKind[i], 'No');

                $("#txtAppointerKind" + j).val(arrAppointerKind[i]);
                $("#txtDeptID" + j).val(arrDeptID[i]);
                $("#txtUserID" + j).val(arrUserID[i]);
                $("#txtDeptName" + j).val(arrDeptName[i]);
                $("#txtUserName" + j).val(arrUserName[i]);
                $("#txtIsDept" + j).val(arrIsDept[i]);
                $("input:radio[name=IsActive" + ZQZCArray[i].id + "][value='" + arrIsActive[i] + "']").attr("checked", true);

                j = j + 1;
            }
        }
    }
    else {
        $("#dataTable").append(
		"<tr id='Add0'>"
			+ "<td colspan='4' class='center'>"
			+ R_NoContent
			+ "</td>"
		+ "</tr>")

        ModSetting.AddArray(0);
    };
}

/**************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 추가하기
***************************************************/
ModSetting.AddRow = function (_appointerKind, _GetUser) {

    var appointerKind = "";
    var fontcolror = "<font>";

    ModSetting.DelRow(0);

    ZQZCRowMax = ZQZCRowMax + 1;

    if (_appointerKind == "ZQ") {
        appointerKind = R_Receive;

        if (ZPCount > 0) {
            fontcolror = "<font color ='blue'>";
        }
        else {
            fontcolror = "<font color ='red'>";
        }
    }
    else if (_appointerKind == "ZP") {
        appointerKind = R_Receive;
        fontcolror = "<font color ='blue'>";
    }
    else {
        appointerKind = R_Carboncopy;
    }

    $("#dataTable").append(
		"<tr id='Add" + ZQZCRowMax + "'>"
            + "<td class='center'><b>" + fontcolror + appointerKind + "</font></b></td>"
            + "<td class='center'><input type='radio' name='IsActive" + ZQZCRowMax + "' id='IsActiveY" + ZQZCRowMax + "' checked value='1'/>" + R_IsActiveTitY + "&nbsp;&nbsp;&nbsp;<input type='radio' name='IsActive" + ZQZCRowMax + "' id='IsActiveN" + ZQZCRowMax + "' value='0'/>" + R_IsActiveTitN + "</td>"
            + "<td class='center'><input type='text' id='txtDeptName" + ZQZCRowMax + "' style='width: 100px !important;' disabled=true/> <input type='text' id='txtUserName" + ZQZCRowMax + "' style='width: 100px !important;' disabled=true/>"
            + "<input type='text' id='txtAppointerKind" + ZQZCRowMax + "' style='display: none' /> <input type='text' id='txtDeptID" + ZQZCRowMax + "' style='display: none' /> <input type='text' id='txtUserID" + ZQZCRowMax + "' style='display: none' /> <input type='text' id='txtIsDept" + ZQZCRowMax + "' style='display: none' />"
            + " <a href=\"javascript:ModSetting.GetUser('" + ZQZCRowMax + "','" + _appointerKind + "')\" id='cbtn" + ZQZCRowMax + "'><img src='" + ImagePath + "/Icon/icon_search.gif' /></a>"
            + "</td>"
			+ "<td class='center'><a onclick=ModSetting.DelRow(" + ZQZCRowMax + ") style='display:inline-block;' class='btn'>" + R_Btn_Del + "</a></td>"
		+ "</tr>"
	);

    ModSetting.AddArray(ZQZCRowMax)

    if (_GetUser != "No") {
        ModSetting.GetUser(ZQZCRowMax, _appointerKind)
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 삭제하기
***************************************************/
ModSetting.DelRow = function (keyID) {

    var row = $("#Add" + keyID);

    for (var i = 0; i < ZQZCArray.length; i++) {

        if (ZQZCArray[i].id == keyID) {
            ZQZCArray.splice(i, 1);
            row.remove();
            break;
        }
    }

    if ((ZQZCArray.length == 0) && (keyID != 0)) {
        ModSetting.Update();
        ModSetting.ListRow();
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 배열값 저장
***************************************************/
ModSetting.AddArray = function (keyID) {
    ZQZCArray.push({
        'id': keyID
    });
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-18
작성목적 : 수신자 선택
**********************************************************************************************/
ModSetting.GetUser = function (p_rowID, p_appointerKind) {

    ZQZCRow = p_rowID;

    var gubun = "1";
    var callback = "GetUserReceivePath";

    if (p_appointerKind == "ZQ") {
        gubun = "1";
        callback = "GetUserReceivePath";
    }
    else {
        gubun = "3";
        callback = "GetUserCarboncopyPath";
    }

    ParamArray = {
        'gubun': gubun,
        'isCheckBox': "0",
        'callback': callback
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUserSelect.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 수신자 추가
**********************************************************************************************/
function GetUserReceivePath(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        rtnVal = unescape(rtnVal);

        var responseStr = rtnVal.split("");
        var items = responseStr[4].split("|");
        var itemscom = items[12].split("$");
        var itemsName = items[2].split("/");

        $("#txtDeptID" + ZQZCRow).val(itemscom[0]);
        $("#txtUserID" + ZQZCRow).val(items[1]);
        $("#txtDeptName" + ZQZCRow).val(items[11]);
        $("#txtIsDept" + ZQZCRow).val(items[0]);
        $("#txtAppointerKind" + ZQZCRow).val("ZQ");

        if (items[0] != "G") {
            if (itemsName.length > 1) {
                $("#txtUserName" + ZQZCRow).val(itemsName[0]);
            }
            else {
                $("#txtUserName" + ZQZCRow).val(items[2]);
            }
        }
        else {
            $("#txtUserID" + ZQZCRow).val("0");
        }
    }

    //중복체크
    var chkCount = 0;

    for (var j = 0; j < ZQZCArray.length; j++) {
        if ($("#txtAppointerKind" + ZQZCArray[j].id).val() != "ZC")
        {        
            if ($("#txtDeptID" + ZQZCRow).val() == $("#txtDeptID" + ZQZCArray[j].id).val()) {
                chkCount = chkCount + 1;
            }
        }
    }

    if (chkCount > 1) {
        $("#txtDeptID" + ZQZCRow).val("");
        $("#txtUserID" + ZQZCRow).val("");
        $("#txtDeptName" + ZQZCRow).val("");
        $("#txtUserName" + ZQZCRow).val("");
        $("#txtIsDept" + ZQZCRow).val("");
        $("#txtAppointerKind" + ZQZCRow).val("ZQ");
        alert(R_Msg_ErrorUserBefore);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 참조자 추가
**********************************************************************************************/
function GetUserCarboncopyPath(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        rtnVal = unescape(rtnVal);
        
        var responseStr = rtnVal.split("");
        var items = responseStr[4].split("|");
        var itemscom = items[12].split("$");
        var itemsName = items[2].split("/");

        $("#txtDeptID" + ZQZCRow).val(itemscom[0]);
        $("#txtUserID" + ZQZCRow).val(items[1]);
        $("#txtDeptName" + ZQZCRow).val(items[11]);
        $("#txtIsDept" + ZQZCRow).val(items[0]);
        $("#txtAppointerKind" + ZQZCRow).val("ZC");

        if (items[0] != "G") {
            if (itemsName.length > 1) {
                $("#txtUserName" + ZQZCRow).val(itemsName[0]);
            }
            else {
                $("#txtUserName" + ZQZCRow).val(items[2]);
            }
        }
        else {
            $("#txtUserID" + ZQZCRow).val("0");
        }
    }

    //중복체크
    var chkCount = 0;

    for (var j = 0; j < ZQZCArray.length; j++) {
        if ($("#txtDeptID" + ZQZCRow).val() == $("#txtDeptID" + ZQZCArray[j].id).val() && $("#txtUserID" + ZQZCRow).val() == $("#txtUserID" + ZQZCArray[j].id).val() && $("#txtAppointerKind" + ZQZCRow).val() == $("#txtAppointerKind" + ZQZCArray[j].id).val()) {
            chkCount = chkCount + 1;
        }
    }

    if (chkCount > 1) {
        $("#txtDeptID" + ZQZCRow).val("");
        $("#txtUserID" + ZQZCRow).val("");
        $("#txtDeptName" + ZQZCRow).val("");
        $("#txtUserName" + ZQZCRow).val("");
        $("#txtIsDept" + ZQZCRow).val("");
        $("#txtAppointerKind" + ZQZCRow).val("ZQ");
        alert(R_Msg_ErrorUserBefore);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 수신결재순서변경
**********************************************************************************************/
ModSetting.ModSeq = function () {

    ModSetting.Update();

    var AppointerKind = "";
    var DeptID = "";
    var DeptName = "";
    var UserID = "";
    var UserName = "";
    var IsDept = "";
    var IsActive = "";

    for (var i = 0; i < ZQZCArray.length; i++) {
        //수신참조 사용자ID
        if (ZQZCArray[i].id != "0") {
            if ($("#txtUserID" + ZQZCArray[i].id).val() == "") {
                alert(R_MSG_ReceiveCarboncopy);
                return;
            }

            if (i == 0) {
                AppointerKind += $("#txtAppointerKind" + ZQZCArray[i].id).val();
                DeptID += $("#txtDeptID" + ZQZCArray[i].id).val();
                DeptName += $("#txtDeptName" + ZQZCArray[i].id).val();
                UserID += $("#txtUserID" + ZQZCArray[i].id).val();
                UserName += $("#txtUserName" + ZQZCArray[i].id).val();
                IsDept += $("#txtIsDept" + ZQZCArray[i].id).val();
                IsActive = $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
            }
            else {
                AppointerKind += "," + $("#txtAppointerKind" + ZQZCArray[i].id).val();
                DeptID += "," + $("#txtDeptID" + ZQZCArray[i].id).val();
                DeptName += "," + $("#txtDeptName" + ZQZCArray[i].id).val();
                UserID += "," + $("#txtUserID" + ZQZCArray[i].id).val();
                UserName += "," + $("#txtUserName" + ZQZCArray[i].id).val();
                IsDept += "," + $("#txtIsDept" + ZQZCArray[i].id).val();
                IsActive += "," + $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
            }
        }
    }

    ParamArray = {
        'companyID': CompanyID,
        'formID': FormID,
        'appointerKind': AppointerKind,
        'deptID': DeptID,
        'deptName': DeptName,
        'userID': UserID,
        'userName': UserName,
        'isDept': IsDept,
        'isActive': IsActive
    };
    CallEncrypt(CallGetParam());

    var url = "ModUserSequence.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 결재자 업데이트
**********************************************************************************************/
ModSetting.Update = function () {
    //체크    
    var AppointerKind = "";
    var DeptID = "";
    var UserID = "";
    var IsDept = "";
    var IsActive = "";

    for (var i = 0; i < ZQZCArray.length; i++) {

        if (i == 0) {
            AppointerKind += $("#txtAppointerKind" + ZQZCArray[i].id).val();
            DeptID += $("#txtDeptID" + ZQZCArray[i].id).val();
            UserID += $("#txtUserID" + ZQZCArray[i].id).val();
            IsDept += $("#txtIsDept" + ZQZCArray[i].id).val();
            IsActive = $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
        }
        else {
            AppointerKind += "," + $("#txtAppointerKind" + ZQZCArray[i].id).val();
            DeptID += "," + $("#txtDeptID" + ZQZCArray[i].id).val();
            UserID += "," + $("#txtUserID" + ZQZCArray[i].id).val();
            IsDept += "," + $("#txtIsDept" + ZQZCArray[i].id).val();
            IsActive += "," + $("input:radio[name=IsActive" + ZQZCArray[i].id + "]:checked").val();
        }
    }

    if (DeptID != "undefined") {
        $("#tbAppointerKind").val(AppointerKind);
        $("#tbDeptID").val(DeptID);
        $("#tbUserID").val(UserID);
        $("#tbIsDept").val(IsDept);
        $("#tbIsActive").val(IsActive);
    }
    else {
        $("#tbAppointerKind").val("");
        $("#tbDeptID").val("");
        $("#tbUserID").val("");
        $("#tbIsDept").val("");
        $("#tbIsActive").val("");
    }
}

ModSetting.LoadData = function (_appointerKind, _deptID, _deptName, _userID, _userName, _isDept, _isActive) {

    $("#tbAppointerKind").val(_appointerKind);
    $("#tbDeptID").val(_deptID);
    $("#tbDeptName").val(_deptName);
    $("#tbUserID").val(_userID);
    $("#tbUserName").val(_userName);
    $("#tbIsDept").val(_isDept);
    $("#tbIsActive").val(_isActive);

    for (var i = ZQZCArray.length; i > 0; i--) {
        ZQZCArray.splice(i, 1);
        var row = $("#Add" + i);
        row.remove();        
    }

    ZQZCArray.splice(0, 1);
    ZQZCRowMax = 0;

    ModSetting.ListRow();
}