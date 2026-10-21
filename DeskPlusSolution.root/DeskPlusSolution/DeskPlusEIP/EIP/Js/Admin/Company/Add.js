Add = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //회사명 다국어
        $('#MultiNameCompany').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });

        //대표자 다국어
        $('#MultiNameInst').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbInstitutorEngName").val(), "GetMultiNameInst", MultiLang);
        });

        //우편번호 검색
        $('#cbtnPost').click(function (e) {
            e.preventDefault();
            Add.CallZipCode();
        });

        //회사 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //회사 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.Del();
        });

        //회사 목록 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            Add.GoList();
        });

        //회사담당자 추가
        $('#cbtnComUserAdd').click(function (e) {
            e.preventDefault();
            Add.AddSubsideUser();
        });

        //회사담당자 삭제
        $('#cbtnComUserDel').click(function (e) {
            e.preventDefault();
            Add.DelSubsideUser();
        });


        $('#ctbMuliLang').attr("disabled", true);
        $('#ctbInstitutorEngName').attr("disabled", true);
        $("#ctbEstablishDate").datepick($.datepicker.regional[PreLang]);

        //추가 시, 
        if (CompanyID == "0") {
            $('#companyUser_Info').hide(); //관리자 추가 비활성화
            $('#cbtnDel').hide(); //삭제 비활성화

            $('#trDefaultValue').show(); //초기 회사 값 가져오기 보이기
        }
        else {
            Add.LoadData();
            $('#trDefaultValue').hide(); //초기 회사 값 가져오기 숨기기
        }

        $("#ctbEstablishDate").keydown(function (e) { return false; });
    });
});



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 회사담당자 정보 리스트
***************************************************/
Add.LoadData = function () {

    Add.PageListSubsideUser();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Add.Resource = function () {

    if (CompanyID != "0") {
        $("#r_companyName")[0].innerHTML = R_CompanyName;
        $("#r_dept")[0].innerHTML = R_DeptName;
        $("#r_title")[0].innerHTML = R_TitleName;
        $("#r_username")[0].innerHTML = R_UserName;
        $("#r_login")[0].innerHTML = R_Login;
        $("#r_officePhone")[0].innerHTML = R_CompanyPhone;
        $("#r_mobilePhone")[0].innerHTML = R_Handphone;
        $("#r_mail")[0].innerHTML = R_Email;

        $(".r_Login").text(R_Login);
        if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사정보 리스트 이동하기
***************************************************/
Add.GoList = function () {
    ParamArray = {
        'companyID': '0',
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사정보 추가, 수정
***************************************************/
Add.Confirm = function () {
    Add.LoadingBox();
    $('#cbtnMod').hide();

    var validator = new Validator($("#form1"));

    validator.checkRequired("ctbCompanyName", R_InputCompanyName, true);
    if (!validator.validate()) return;

    $('#ctbMuliLang').attr("disabled", false);
    $('#ctbInstitutorEngName').attr("disabled", false);

    if (CompanyID == "0") {
        __doPostBack('lbtnRegister', 'Add'); //추가
    }
    else {

        Add.CheckLicense();
        if (UseLicense == true) {
            __doPostBack('lbtnRegister', 'Mod'); //수정
        }
    }
}



/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 회사별 라이센스 카운트 저장
***************************************************/
Add.CheckLicense = function () {

    var licenseCnt = $("#ctbLicenseCnt").val();
    if (licenseCnt == "") licenseCnt = "0";
    var alList = CompanyID + "_" + licenseCnt;
    if (alList != "") {

        ParamArray = {
            'licenseList': alList
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/List.asmx/ModLicense";
        var obj = eval("Add.SuccessCheckLicense");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SearchRequestSelect);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공 시, 회사별 라이센스 카운트 저장
**********************************************************************************************/
Add.SuccessCheckLicense = function (data, status) {

    if (data.d >= 0) {
        UseLicense = true;
    }
    else if (data.d == -1) {
        alert(R_ExcessLicenseCnt);
        UseLicense = false;
        $('#loading').html("");
        $('#cbtnMod').show();
    }
    else {
        alert(R_ProcessingError);
        UseLicense = false;
        $('#loading').html("");
        $('#cbtnMod').show();
    }
}




/**************************************************
작성자   : freeman
작성일자 : 2013-03-13
작성목적 : 로딩 표시
***************************************************/
Add.LoadingBox = function() {
    var loadingPath = "<div id='splashScreen'  style='position:absolute;z-index:5;top:30%;left:35%;background-color:#F5F5F5;border:1px solid #E9E9E9;'>"
        + "<table cellpadding='0' cellspacing='0' style='width:400px; height:200px;'>"
        + "<tr><td style='width:100%; height:100%;font-family:Tahoma;' align='center' valign='middle'>"
        + "<IMG src='/DeskPlusEIP/Design/C/Img/Icon/loading.gif'/>"
        + "   Please wait..."
        + "</td><td></td></tr>"
        + "</table>"
        + "</div>";

    $('#loading').html(loadingPath);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사정보 삭제
***************************************************/
Add.Del = function () {

    if (CompanyID != "0") {
        if (window.confirm(R_ConfirmDelete)) {
            __doPostBack('lbtnRegister', 'Del'); //추가            
        }
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사명 다국어
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

    $("#ctbMuliLang").val(multiNames);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 대표자 다국어
***************************************************/
function GetMultiNameInst(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbInstitutorEngName").val(multiNames);
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 페이지 호출
***************************************************/
Add.CallZipCode = function () {

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
            $("#ctbAddress1").val(fullRoadAddr);
            $("#ctbAddress2").val("");
            $("#ctbAddress2").focus();
        }
    }).open();

    return;

    //파라미터값 배열에 담기    
    ParamArray = {
        'callBackPage': "GetZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;
    PopupWin(600, 435, url, "-100", "", true);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 설정하기
***************************************************/
function GetZipCode(zipcode, ido, si, dong) {
    ido = decodeURIComponent(ido);
    si = decodeURIComponent(si);
    dong = decodeURIComponent(dong);

    $("#ctbZipCode").val(zipcode);
    $("#ctbAddress1").val(ido + " " + si + " " + dong);
    $("#ctbAddress2").val();
    $("#ctbAddress2").focus();
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
Add.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
Add.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사 담당자 추가
***************************************************/
Add.AddSubsideUser = function () {
    ParamArray = {
        'companyID': CompanyID,
        'callBackPage': "GetSelectSubsideUser"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Common/User/GetSelectedUser.aspx?p=" + GetEncryptValue;
    PopupWin(700, 500, url, "-750", "", true);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 선택된 회사 담당자 리스트 가져오기 (AddSubsideUser 과 연관)
***************************************************/
function GetSelectSubsideUser(selUserList) {

    selUserList = decodeURIComponent(selUserList);

    ParamArray = {
        'companyID': CompanyID,
        'userID': selUserList,
        'roleCode': 'admin'
    };
    
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/Add.asmx/AddSubsideUser";
    var obj = eval("Add.SuccessAddSubsideUser");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 관리자 추가 성공시 페이지 표시
**********************************************************************************************/
Add.SuccessAddSubsideUser = function (data, status) {
    if (data.d == true) {
        Add.LoadData();
    }
}


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 회사 담당자 정보 삭제하기 
***************************************************/
Add.DelSubsideUser = function () {

    var selUserList = Add.CheckedValue();

    if (selUserList != "") {

        if (window.confirm(R_ConfirmDeleteCompanyPersonInfo)) {

            $.each(selUserList, function (number) {

                //파라미터값 배열에 담기
                ParamArray = {
                    'companyID': CompanyID,
                    'roleID': RoleAdmin,
                    'userID': selUserList[number],
                    'types': 'GM'
                };

                CallEncrypt(CallGetParam());

                var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/List.asmx/DelRoleUser";
                var obj = eval("Add.SuccessSubsideUser");
                CallEncrypt(CallGetParam());
                data = "{p:'" + GetEncryptValue + "'}";

                AjaxService(url, data, obj);
            });

            Add.LoadData();
        }
    }
    else {
        alert(R_CheckDeleteCompanyPersonInfo);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 관리자 추가 성공시 페이지 표시
**********************************************************************************************/
Add.SuccessSubsideUser = function (data, status) {
    if (data.d == true) {
        Add.SuccessAddSubsideUser();
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 회사담당자 정보 리스트
***************************************************/
Add.PageListSubsideUser = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Company/Add.asmx/GetSubsideUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID
    };

    var obj = eval("Add.SuccessPageListSubsideUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 회사담당자 리스트 표시
**********************************************************************************************/
Add.SuccessPageListSubsideUser = function (data, status) {
    $('#placeholderUserList').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Company/JTUserList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderUserList').processTemplate(data.d);
    //$('#loading').html("");

    Add.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 회사담당자 리스트 표시
**********************************************************************************************/
Add.SLO = function (p_comID, p_userID, p_loginID) {    
    $("#ifrmSLO").attr("src", "slo.aspx?companyID=" + p_comID + "&userID=" + p_userID + "&loginID=" + p_loginID);
}