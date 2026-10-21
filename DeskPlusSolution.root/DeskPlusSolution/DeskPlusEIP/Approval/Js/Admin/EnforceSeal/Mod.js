Mod = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //직인추가
        $('#stampAdd').click(function (e) { e.preventDefault(); Mod.StampAdd(); });
        //직인삭제
        $('#stampDel').click(function (e) { e.preventDefault(); Mod.StampDel(); });

        //표현명 다국어
        $('#exp').click(function (e) { e.preventDefault(); Mod.SetMultiExp(); });

        //주소 다국어
        $('#address').click(function (e) { e.preventDefault(); Mod.SetMultiAddress(); });

        //전화번호 다국어
        $('#phone').click(function (e) { e.preventDefault(); CallMultiName($("#ctbPhoneMulti").val(), "GetMultiPhone", ""); });

        //팩스번호 다국어
        $('#fax').click(function (e) { e.preventDefault(); CallMultiName($("#ctbFaxMulti").val(), "GetMultiFax", ""); });

        //우편번호
        $('#zipcode').click(function (e) { e.preventDefault(); Mod.SetZipcode(); });

        //확인
        $('#CbtnConfirm').click(function (e) { e.preventDefault(); Mod.Confirm(); });

        //삭제
        $('#CbtnDel').click(function (e) { e.preventDefault(); Mod.Del(); });

        //목록
        $('#CbtnCancel').click(function (e) { e.preventDefault(); Mod.Cancel(); });

        //담당자추가
        $('#cbtnAdminAdd').click(function (e) { e.preventDefault(); Mod.AdminAdd(); });

        $('.searchtext').attr("disabled", true);
        $('#ddlDept').attr("disabled", true);       //계열사정보

        Mod.GetPageList();      //담당자리스트
    });

});

/**************************************************
작성목적 : 다국어 파라미터 초기설정
***************************************************/
Mod.MultiParam = function (p_multiLang, p_callBackPage, p_useName) {

    var nameKo = "", nameEn = "", nameJa = "", nameFr = "", nameZhcn = "", nameZhtw = "";
    var multiName = p_multiLang;

    if (multiName != "" && multiName.indexOf(",") != -1) {
        nameKo = (multiName.split(",")[0] != undefined) ? multiName.split(",")[0] : "";
        nameEn = (multiName.split(",")[1] != undefined) ? multiName.split(",")[1] : "";
        nameJa = (multiName.split(",")[2] != undefined) ? multiName.split(",")[2] : "";
        nameFr = (multiName.split(",")[3] != undefined) ? multiName.split(",")[3] : "";
        nameZhcn = (multiName.split(",")[4] != undefined) ? multiName.split(",")[4] : "";
        nameZhtw = (multiName.split(",")[5] != undefined) ? multiName.split(",")[5] : "";
    }

    if (p_callBackPage == "") p_callBackPage = "GetMultiName";
    if (p_useName == "") p_useName = "ko,en,ja,fr,zhcn,zhtw";

    //파라미터값 배열에 담기
    //ko,en,ja,fr,zhcn,zhtw
    ParamArray = {
        'callBackPage': p_callBackPage,
        'useName': p_useName,
        'nameKo': nameKo,
        'nameEn': nameEn,
        'nameJa': nameJa,
        'nameFr': nameFr,
        'nameZhcn': nameZhcn,
        'nameZhtw': nameZhtw
    };

    CallEncrypt(CallGetParam());
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 표현명 다국어
***************************************************/
Mod.SetMultiExp = function () {

    var firstBase = "", firstKo = "", firstEn = "", firstJa = "", firstFr = "", firstZhcn = "", firstZhtw = "";
    var midBase = "", midKo = "", midEn = "", midJa = "", midFr = "", midZhcn = "", midZhtw = "";
    var endBase = "", endKo = "", endEn = "", endJa = "", endFr = "", endZhcn = "", endZhtw = "";

    var multiFirst = $("#tbExpFirstMulti").val();
    var multiMid = $("#tbExpMidMulti").val();
    var multiEnd = $("#tbExpEndMulti").val();
    multiFirst = multiFirst.replace(/&/g, '＆').replace(/"/g, '＂');
    multiMid = multiMid.replace(/&/g, '＆').replace(/"/g, '＂');
    multiEnd = multiEnd.replace(/&/g, '＆').replace(/"/g, '＂');

    if (multiFirst != "" && multiFirst.indexOf(",") != -1) {
        firstBase = (multiFirst.split(",")[0] != undefined) ? multiFirst.split(",")[0] : "";
        firstEn = (multiFirst.split(",")[1] != undefined) ? multiFirst.split(",")[1] : "";
        firstJa = (multiFirst.split(",")[2] != undefined) ? multiFirst.split(",")[2] : "";
        firstFr = (multiFirst.split(",")[3] != undefined) ? multiFirst.split(",")[3] : "";
        firstZhcn = (multiFirst.split(",")[4] != undefined) ? multiFirst.split(",")[4] : "";
        firstZhtw = (multiFirst.split(",")[5] != undefined) ? multiFirst.split(",")[5] : "";
    }
    if (multiMid != "" && multiMid.indexOf(",") != -1) {
        midBase = (multiMid.split(",")[0] != undefined) ? multiMid.split(",")[0] : "";
        midEn = (multiMid.split(",")[1] != undefined) ? multiMid.split(",")[1] : "";
        midJa = (multiMid.split(",")[2] != undefined) ? multiMid.split(",")[2] : "";
        midFr = (multiMid.split(",")[3] != undefined) ? multiMid.split(",")[3] : "";
        midZhcn = (multiMid.split(",")[4] != undefined) ? multiMid.split(",")[4] : "";
        midZhtw = (multiMid.split(",")[5] != undefined) ? multiMid.split(",")[5] : "";
    }
    if (multiEnd != "" && multiEnd.indexOf(",") != -1) {
        endBase = (multiEnd.split(",")[0] != undefined) ? multiEnd.split(",")[0] : "";
        endEn = (multiEnd.split(",")[1] != undefined) ? multiEnd.split(",")[1] : "";
        endJa = (multiEnd.split(",")[2] != undefined) ? multiEnd.split(",")[2] : "";
        endFr = (multiEnd.split(",")[3] != undefined) ? multiEnd.split(",")[3] : "";
        endZhcn = (multiEnd.split(",")[4] != undefined) ? multiEnd.split(",")[4] : "";
        endZhtw = (multiEnd.split(",")[5] != undefined) ? multiEnd.split(",")[5] : "";
    }

    ParamArray = {
        'callBackPage': 'SetMultiExpSuccess'
        , 'useName': 'base,ko,en,ja,fr,zhcn,zhtw'
        , 'firstBase': firstBase
        , 'firstKo': firstKo
        , 'firstEn': firstEn
        , 'firstJa': firstJa
        , 'firstFr': firstFr
        , 'firstZhcn': firstZhcn
        , 'firstZhtw': firstZhtw
        , 'midBase': midBase
        , 'midKo': midKo
        , 'midEn': midEn
        , 'midJa': midJa
        , 'midFr': midFr
        , 'midZhcn': midZhcn
        , 'midZhtw': midZhtw
        , 'endBase': endBase
        , 'endKo': endKo
        , 'endEn': endEn
        , 'endJa': endJa
        , 'endFr': endFr
        , 'endZhcn': endZhcn
        , 'endZhtw': endZhtw
    };

    CallEncrypt(CallGetParam());
    var url = "RegisterLangExp.aspx?p=" + GetEncryptValue;
    PopupWin(300, 325, url, "", "", false);
}
SetMultiExpSuccess = function (firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, midBase, midKo, midEn, midJa, midFr, midZhcn, midZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw) {

    firstBase = decodeURIComponent(firstBase);
    firstKo = decodeURIComponent(firstKo);
    firstEn = decodeURIComponent(firstEn);
    firstJa = decodeURIComponent(firstJa);
    firstFr = decodeURIComponent(firstFr);
    firstZhcn = decodeURIComponent(firstZhcn);
    firstZhtw = decodeURIComponent(firstZhtw);
    midBase = decodeURIComponent(midBase);
    midKo = decodeURIComponent(midKo);
    midEn = decodeURIComponent(midEn);
    midJa = decodeURIComponent(midJa);
    midFr = decodeURIComponent(midFr);
    midZhcn = decodeURIComponent(midZhcn);
    midZhtw = decodeURIComponent(midZhtw);
    endBase = decodeURIComponent(endBase);
    endKo = decodeURIComponent(endKo);
    endEn = decodeURIComponent(endEn);
    endJa = decodeURIComponent(endJa);
    endFr = decodeURIComponent(endFr);
    endZhcn = decodeURIComponent(endZhcn);
    endZhtw = decodeURIComponent(endZhtw);

    var multiFirst = firstBase + "," + firstKo + "," + firstEn + "," + firstJa + "," + firstFr + "," + firstZhcn + "," + firstZhtw;
    var multiMid = midBase + "," + midKo + "," + midEn + "," + midJa + "," + midFr + "," + midZhcn + "," + midZhtw;
    var multiEnd = endBase + "," + endKo + "," + endEn + "," + endJa + "," + endFr + "," + endZhcn + "," + endZhtw;

    $("#tbExpFirstMulti").val(multiFirst);
    $("#tbExpMidMulti").val(multiMid);
    $("#tbExpEndMulti").val(multiEnd);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 주소 다국어
***************************************************/
Mod.SetMultiAddress = function () {

    var firstBase = "", firstKo = "", firstEn = "", firstJa = "", firstFr = "", firstZhcn = "", firstZhtw = "";
    var endBase = "", endKo = "", endEn = "", endJa = "", endFr = "", endZhcn = "", endZhtw = "";

    var multiFirst = $("#tbAddressFirstMulti").val();
    var multiEnd = $("#tbAddressEndMulti").val();
    multiFirst = multiFirst.replace(/&/g, '＆').replace(/"/g, '＂');
    multiEnd = multiEnd.replace(/&/g, '＆').replace(/"/g, '＂');

    if (multiFirst != "" && multiFirst.indexOf("|") != -1) {
        firstBase = (multiFirst.split("|")[0] != undefined) ? multiFirst.split("|")[0] : "";
        firstEn = (multiFirst.split("|")[1] != undefined) ? multiFirst.split("|")[1] : "";
        firstJa = (multiFirst.split("|")[2] != undefined) ? multiFirst.split("|")[2] : "";
        firstFr = (multiFirst.split("|")[3] != undefined) ? multiFirst.split("|")[3] : "";
        firstZhcn = (multiFirst.split("|")[4] != undefined) ? multiFirst.split("|")[4] : "";
        firstZhtw = (multiFirst.split("|")[5] != undefined) ? multiFirst.split("|")[5] : "";
    }
    if (multiEnd != "" && multiEnd.indexOf("|") != -1) {
        endBase = (multiEnd.split("|")[0] != undefined) ? multiEnd.split("|")[0] : "";
        endEn = (multiEnd.split("|")[1] != undefined) ? multiEnd.split("|")[1] : "";
        endJa = (multiEnd.split("|")[2] != undefined) ? multiEnd.split("|")[2] : "";
        endFr = (multiEnd.split("|")[3] != undefined) ? multiEnd.split("|")[3] : "";
        endZhcn = (multiEnd.split("|")[4] != undefined) ? multiEnd.split("|")[4] : "";
        endZhtw = (multiEnd.split("|")[5] != undefined) ? multiEnd.split("|")[5] : "";
    }

    ParamArray = {
        'callBackPage': 'SetMultiAddressSuccess'
        , 'useName': 'ko,en,ja,fr,zhcn,zhtw'
        , 'firstBase': firstBase
        , 'firstKo': firstKo
        , 'firstEn': firstEn
        , 'firstJa': firstJa
        , 'firstFr': firstFr
        , 'firstZhcn': firstZhcn
        , 'firstZhtw': firstZhtw
        , 'endBase': endBase
        , 'endKo': endKo
        , 'endEn': endEn
        , 'endJa': endJa
        , 'endFr': endFr
        , 'endZhcn': endZhcn
        , 'endZhtw': endZhtw
    };

    var url = "RegisterLangAddress.aspx?p=" + GetEncryptValue;
    PopupWin(400, 325, url, "", "", false);
}

SetMultiAddressSuccess = function (firstBase, firstKo, firstEn, firstJa, firstFr, firstZhcn, firstZhtw, endBase, endKo, endEn, endJa, endFr, endZhcn, endZhtw) {

    firstBase = decodeURIComponent(firstBase);
    firstKo = decodeURIComponent(firstKo);
    firstEn = decodeURIComponent(firstEn);
    firstJa = decodeURIComponent(firstJa);
    firstFr = decodeURIComponent(firstFr);
    firstZhcn = decodeURIComponent(firstZhcn);
    firstZhtw = decodeURIComponent(firstZhtw);
    endBase = decodeURIComponent(endBase);
    endKo = decodeURIComponent(endKo);
    endEn = decodeURIComponent(endEn);
    endJa = decodeURIComponent(endJa);
    endFr = decodeURIComponent(endFr);
    endZhcn = decodeURIComponent(endZhcn);
    endZhtw = decodeURIComponent(endZhtw);

    var multiFirst = firstBase + "|" + firstKo + "|" + firstEn + "|" + firstJa + "|" + firstFr + "|" + firstZhcn + "|" + firstZhtw;
    var multiEnd = endBase + "|" + endKo + "|" + endEn + "|" + endJa + "|" + endFr + "|" + endZhcn + "|" + endZhtw;

    $("#tbAddressFirstMulti").val(multiFirst);
    $("#tbAddressEndMulti").val(multiEnd);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 전화번호 다국어
***************************************************/
function GetMultiPhone(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;
    $("#ctbPhoneMulti").val(multiNames);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 팩스번호 다국어
***************************************************/
function GetMultiFax(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;
    $("#ctbFaxMulti").val(multiNames);
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 우편번호
***************************************************/
Mod.SetZipcode = function () {

    new daum.Postcode({
        oncomplete: function (data) {
            // 팝업에서 검색결과 항목을 클릭했을때 실행할 코드를 작성하는 부분.

            // 도로명 주소의 노출 규칙에 따라 주소를 조합한다.
            // 내려오는 변수가 값이 없는 경우엔 공백('')값을 가지므로, 이를 참고하여 분기 한다.
            var fullRoadAddr = data.roadAddress; // 도로명 주소 변수
            var extraRoadAddr = ''; // 도로명 조합형 주소 변수

            // 법정동명이 있을 경우 추가한다.
            if (data.bname !== '') {
                extraRoadAddr += data.bname;
            }
            // 건물명이 있을 경우 추가한다.
            if (data.buildingName !== '') {
                extraRoadAddr += (extraRoadAddr !== '' ? ', ' + data.buildingName : data.buildingName);
            }
            // 도로명, 지번 조합형 주소가 있을 경우, 괄호까지 추가한 최종 문자열을 만든다.
            if (extraRoadAddr !== '') {
                extraRoadAddr = ' (' + extraRoadAddr + ')';
            }
            // 도로명, 지번 주소의 유무에 따라 해당 조합형 주소를 추가한다.
            if (fullRoadAddr !== '') {
                fullRoadAddr += extraRoadAddr;
            }

            //var zipcode = data.postcode1 + "" + data.postcode2;
            var zipcode = data.zonecode;

            $("#ctbZipCode").val(zipcode);
            $("#ctbAddressFirstBase").val(fullRoadAddr);
            $("#ctbAddressEndBase").val("");
            $("#ctbAddressEndBase").focus();
        }
    }).open();

    return;

    ParamArray = {
        'callBackPage': "GetZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;
    PopupWin(700, 435, url, "", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 우편번호 설정하기
***************************************************/
function GetZipCode(zipcode, ido, si, dong) {
    ido = decodeURIComponent(ido);
    si = decodeURIComponent(si);
    dong = decodeURIComponent(dong);

    $("#ctbZipCode").val(zipcode);
    $("#ctbAddressFirstBase").val(ido + " " + si + " " + dong);
    $("#ctbAddressEndBase").val("");
    $("#ctbAddressEndBase").focus();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인
***************************************************/
Mod.Confirm = function () {

    //필수입력
    var validator = new Validator($("#form1"));
    if ($("#ddlDept").val() == "") {
        validator.checkRequired("ddlDept", R_MSG_EnforceMsg04, true);
    }
    validator.checkRequired("ctbExpMidBase", R_MSG_EnforceMsg05, true);
    validator.checkRequired("ctbExpEndBase", R_MSG_EnforceMsg05, true);
    validator.checkRequired("ctbAddressFirstBase", R_MSG_EnforceMsg06, true);
    validator.checkRequired("ctbAddressEndBase", R_MSG_EnforceMsg06, true);
    validator.checkRequired("ctbPhoneBase", R_MSG_EnforceMsg07, true);
    validator.checkRequired("ctbFaxBase", R_MSG_EnforceMsg08, true);

    //특수문자(,) 체크
    if (!Mod.CheckChar()) { alert(R_MSG_SPECIALCHAR); return; }
    if (!validator.validate()) return;

    //disble값 처리
    $("#tbExpFirstMultiHidden").val($("#tbExpFirstMulti").val());
    $("#tbExpMidMultiHidden").val($("#tbExpMidMulti").val());
    $("#tbExpEndMultiHidden").val($("#tbExpEndMulti").val());
    $("#tbAddressFirstMultiHidden").val($("#tbAddressFirstMulti").val());
    $("#tbAddressEndMultiHidden").val($("#tbAddressEndMulti").val());
    $("#tbPhoneMultiHidden").val($("#ctbPhoneMulti").val());
    $("#tbFaxMultiHidden").val($("#ctbFaxMulti").val());

    __doPostBack('lbtnSubmit', "MOD");
}
// 특수문자 체크
Mod.CheckChar = function () {
    var rtnval = true;
    var chkObj = $("#ctbExpFirstBase, #ctbExpMidBase, #ctbExpEndBase, #ctbPhoneBase, #ctbFaxBase");
    $.each(chkObj, function (i, o) {
        if ($("#" + o.id).val().indexOf(",") > -1) {
            $("#" + o.id).focus(); rtnval = false;
        }
    });
    return rtnval;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 삭제
***************************************************/
Mod.Del = function () {
    if (confirm(R_Msg_Del)) {
        __doPostBack('lbtnSubmit', "DEL");
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 취소
***************************************************/
Mod.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
    };
    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : [직인추가]
***************************************************/
Mod.StampAdd = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'APPROVAL',
        'callBackEvent': "Mod.GetStampFile "
    };
    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}
Mod.GetStampFile = function () {

    ParamArray = { 'swfUploadGUID': SwfUploadGUID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.GetStampFileSuccess");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}
Mod.GetStampFileSuccess = function (data, status) {
    $("#tbFileName").val("");
    for (var count in data.d) {         //alert(data.d[0].FilePath + "\n" + data.d[0].FileName)
        var path = data.d[0].FilePath;
        $("#stampImg")[0].innerHTML = "<img src='" + path + "' width='94' height='124' alt='' title='' />";
        $("#tbFileName").val(data.d[0].FileName);
    }
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : [직인삭제]
***************************************************/
Mod.StampDel = function () {

    if ($("#tbFileName").val() != "" || FilePath != "") {
        if (confirm(R_DelAlert + R_Msg_Del)) {

            ParamArray = {
                'swfUploadGUID': SwfUploadGUID
            , 'filename': $("#tbFileName").val()
            , 'beforefile': FilePath + FileName
               , 'enforceStampID': EnforceStampID
            };
            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Mod.StampDelSuccess");
            var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/Mod.asmx/DelStamp";
            AjaxService(url, data, obj);
        }
    }
}
Mod.StampDelSuccess = function (data, status) { //alert(data.d)
    if (data.d[0] != "") {
        $("#tbFileName").val("");
        $("#stampImg")[0].innerHTML = data.d;
        FilePath = "";
        FileName = "";
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 담당자 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Mod.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/Mod.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = { 'enforceStampID': EnforceStampID };

    var obj = eval("Mod.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Mod.SuccessPageList = function (data, status) {
    $('#placeholder_admin').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/EnforceSeal/JTListUser.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_admin').processTemplate(data.d);

    Mod.Resource();
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {

    $("#r_photo")[0].innerHTML = R_Photo;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_delete")[0].innerHTML = R_Del;    

    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 :  담당자 추가
**********************************************************************************************/
Mod.AdminAdd = function () {

    ParamArray = { 'deptID': '0', 'callback': "GetUserByDept" };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Account/Common/UserListByDept.aspx?p=" + GetEncryptValue;
    PopupWin(800, 600, url, "-500", "", true);
}
function GetUserByDept(response) {
    var rtnval = response;
    if (rtnval != "undefined" && unescape(rtnval) != "" && rtnval != null) {
        rtnval = unescape(rtnval);
        Mod.UserAdd(rtnval);
    }
}
/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 담당자 저장하기(웹서비스 호출)
**********************************************************************************************/
Mod.UserAdd = function (userList) {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/Mod.asmx/AddUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'userList': userList,
        'enforceStampID': EnforceStampID
    };

    var obj = eval("Mod.UserAddSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Mod.UserAddSuccess = function (data, status) {
    if (data.d == true) {
        Mod.GetPageList();
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 :  담당자 추가
**********************************************************************************************/
Mod.AdminDel = function (_id) {

    if (confirm(R_Msg_Del)) {
        var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/Mod.asmx/DelUser";

        ParamArray = { 'enforceAdminID': _id };
        var obj = eval("Mod.AdminDelSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
Mod.AdminDelSuccess = function (data, status) {
    if (data.d == true) {
        Mod.GetPageList();

    }
}