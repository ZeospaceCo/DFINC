Mod = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        // [수정]
        $('#cbtnMod').click(function (e) { e.preventDefault(); Mod.Confirm(); });

        // [목록]
        $('#cbtnClose').click(function (e) { e.preventDefault(); Mod.Close(); });

        // [추가] 
        $('#cbtnAdd').click(function (e) { e.preventDefault(); Mod.Add(); });

        // [삭제] 
        $('#cbtnDel').click(function (e) { e.preventDefault(); Mod.Del(); });

        // 다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiLang').attr("disabled", true);

        Mod.LoadData();
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-12
작성목적 : 직급 정보 리스트
***************************************************/
Mod.LoadData = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/Mod.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'accessID': AccessID
    };

    var obj = eval("Mod.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-12
작성목적 : 성공시 직급 리스트 표시
**********************************************************************************************/
Mod.SuccessPageList = function (data, status) {
    $('#placeholderTitleList').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/AccessGrade/JtTitleList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderTitleList').processTemplate(data.d);

    Mod.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-12
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {

    $("#r_titleName2")[0].innerHTML = TitleName2;
    $("#r_isActiveTit")[0].innerHTML = IsActiveTit;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인
***************************************************/
Mod.Confirm = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbAccessGradeName", R_Msg_ErrAccessGradeName, true);
    if (!validator.validate()) return;

    $('#ctbMultiLang').attr("disabled", false);

    __doPostBack('lbtnSubmit', "MOD");
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
Mod.Close = function () {
    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 전체 체크박스
***************************************************/
Mod.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
Mod.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 수발자,결재자 리스트 사용자 삭제
***************************************************/
Mod.Add = function () {

    // 열람권한 
    var codeID = $("#ddlAccessTitle").val();

    if (codeID == "") { alert(R_MSG_SelItem); return; }

    $("#ddlAccessTitleIs > option[value=" + codeID + "]").attr("selected", "true");

    var selectAccessID = $("#ddlAccessTitleIs option:selected").text();

    if (selectAccessID == "0" || selectAccessID == "") {
        var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/Mod.asmx/Add";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'accessID': AccessID,
            'codeID': codeID
        };

        var obj = eval("Mod.SuccessAdd");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
    else {
        if (confirm(R_Msg_ErrAccessGradeModify)) {
            var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/Mod.asmx/Add";

            //파라미터값 배열에 담기
            ParamArray = {
                'companyID': CompanyID,
                'accessID': AccessID,
                'codeID': codeID
            };

            var obj = eval("Mod.SuccessAdd");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 :  사용자 삭제 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        Mod.List();
    }
    else {
        alert(R_MSG_FAIL);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 : 수발자,결재자 리스트 사용자 삭제
***************************************************/
Mod.Del = function () {
    var codeIDList = Mod.CheckedValue();
    if (codeIDList == "") { alert(R_MSG_SelItem); return; }

    if (confirm(R_Msg_Del)) {
        var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/Mod.asmx/Del";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'codeIDList': codeIDList
        };

        var obj = eval("Mod.SuccessDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-13
작성목적 :  사용자 삭제 후 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        Mod.List();
    }
    else {
        alert(R_MSG_FAIL);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
Mod.List = function () {

    ParamArray = {
        'accessID': AccessID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-08
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMultiLang").val(multiNames);
}