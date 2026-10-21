List = {}

$(function () {
    $(document).ready(function () {

        // 출처 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        // 출처 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            List.Mod();
        });

        // 출처 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //출처 다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMenuName").val(), "GetMultiName", MultiLang);
        });

        // 출처 순서 변경
        $('#CbtnMove').click(function (e) {
            e.preventDefault();
            List.Updown();
        });

        $('#ctbMenuName').attr("disabled", true);

        $("#ctbParentName").focus();

        List.LoadData();

    })
})


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처추가
**********************************************************************************************/
List.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_SourceName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Task/Ws/Admin/Source/List.asmx/AddSource";

    var active = $("#chkActive").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'active': active
    };

    var obj = eval("List.AddSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처추가 성공시
**********************************************************************************************/
List.AddSuccess = function (data, status) {
    
    if (data.d == true) {
        List.LoadData();
        List.SourceInit();
        alert(R_CompleteConfirm);
    }
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 리스트 초기화
**********************************************************************************************/
List.LoadData = function () {
    // 출처 리스트 가져오기
    List.GetPageList();
    List.Resource();

}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 출처리스트 상세내역 가져오기
**********************************************************************************************/
List.GetSource = function (p_SourceID) {
    SourceID = p_SourceID;

    var url = "/DeskPlusEIP/Task/Ws/Admin/Source/List.asmx/GetSourceID";

    ParamArray = {
        'sourceID': p_SourceID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.Success");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처리스트 선택 시 상단 바인딩 (웹서비스 값 받기)
**********************************************************************************************/
List.Success = function (data, status) {
    $("#ctbParentName").val("");
    $("#ctbMenuName").val("");

    $("#ctbParentName").val(data.d[0].NameBase);

    GetMultiName(data.d[0].NameBase,
                 data.d[0].NameKo,
                 data.d[0].NameEn,
                 data.d[0].NameJa,
                 data.d[0].NameFr,
                 data.d[0].NameZhcn,
                 data.d[0].NameZhtw);

    if (data.d[0].IsActive == "1")
        $("#chkActive").attr("checked", true);
    else
        $("#chkActive").attr("checked", false);
}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-05-06
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMenuName").val(multiNames);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($(".Active_1") != undefined) { $(".Active_1").text(R_Active); }
    if ($(".Active_0") != undefined) { $(".Active_0").text(R_NonActive); }
    if ($(".r_active") != undefined) { $(".r_active").text(R_UsedYN); }
    if ($(".r_group") != undefined) { $(".r_group").text(R_IsSource); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }  
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 출처관리 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Admin/Source/List.asmx/GetPageList";

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-05-04
작성목적 : 출처관리 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#phSourceList').setTemplateURL('/DeskPlusEIP/Task/Jt/Admin/Source/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSourceList').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처수정
**********************************************************************************************/
List.Mod = function () {
    if (SourceID == "") {
        return;
    }
    var url = "/DeskPlusEIP/Task/Ws/Admin/Source/List.asmx/ModSource";

    var active = $("#chkActive").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'active': active,
        'sourceID': SourceID
    };

    $('#ctbMenuName').attr("disabled", true);

    var obj = eval("List.ModSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처 수정 완료
**********************************************************************************************/
List.ModSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageList();
        List.Resource();
        List.SourceInit();
        alert(R_CompleteConfirm);
        
    }
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처삭제
**********************************************************************************************/
List.Del = function () {

    if (SourceID == "") {       
        return;
    }

    if (window.confirm(R_DelGubun)) {
        var url = "/DeskPlusEIP/Task/Ws/Admin/Source/List.asmx/DelSource";

        ParamArray = {
            'sourceID': SourceID
        };

        var obj = eval("List.DelSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }

}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-05-04
작성목적 : 출처 삭제
**********************************************************************************************/
List.DelSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageList();
        List.Resource();
        List.SourceInit();
        alert(R_CompleteConfirm);
    }
}


List.Updown = function () {
    ParamArray = {
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Source/Updown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 출처 상단 초기화
**********************************************************************************************/
List.SourceInit = function () {
    $("#ctbParentName").val("");
    $("#ctbMenuName").val("");
    $("#chkAcitve").attr("checked", true);
}