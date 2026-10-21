Add = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        $("#ctbEstablishDate").datepick();

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

        //회사 목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            Add.List();
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

        //로그인 이미지 추가
        $('#loginAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("login");
        });

        //로그인 이미지 삭제
        $('#loginDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("login");
        });

        //메뉴 로고 추가
        $('#menuAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("menu");
        });

        //메뉴 로고 삭제
        $('#menuDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("menu");
        });

        //세로 로그인 이미지 추가
        $('#VloginAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("Vlogin");
        });

        //세로로그인 이미지 삭제
        $('#VloginDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("Vlogin");
        });

        //메뉴 로고 추가
        $('#AmenuAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("Amenu");
        });

        //메뉴 로고 삭제
        $('#AmenuDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("Amenu");
        });


        //로그인 배경이미지 추가
        $('#BGloginAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("BGlogin");
        });

        //로그인 배경이미지 삭제
        $('#BGloginDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("BGlogin");
        });


        //포탈 배경이미지 추가
        $('#BGPotalAdd').click(function (e) {
            e.preventDefault();
            Add.AttachFile("BGPotal");
        });

        //포탈 배경이미지 삭제
        $('#BGPotalDel').click(function (e) {
            e.preventDefault();
            Add.DelImage("BGPotal");
        });


        $('#ctbMuliLang').attr("disabled", true);
        $('#ctbInstitutorEngName').attr("disabled", true);

        $("#ctbEstablishDate").keydown(function (e) { return false; });

        Add.LoadData("");
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 회사담당자 정보 리스트
***************************************************/
Add.LoadData = function (gubun) {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Subsidiary/Add.asmx/GetSubsideUserList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID
    };

    var obj = eval("Add.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 회사담당자 리스트 표시
**********************************************************************************************/
Add.SuccessLoadData = function (data, status) {
    $('#placeholderUserList').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Subsidiary/JTUserList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderUserList').processTemplate(data.d);
    //$('#loading').html("");

    Add.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Add.Resource = function () {
    $("#r_companyName")[0].innerHTML = R_CompanyName;
    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_title")[0].innerHTML = R_TitleName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_login")[0].innerHTML = R_Login;
    $("#r_officePhone")[0].innerHTML = R_CompanyPhone;
    $("#r_mobilePhone")[0].innerHTML = R_Handphone;
    $("#r_mail")[0].innerHTML = R_Email;
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사정보 수정
***************************************************/
Add.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbCompanyName", R_InputCompanyName, true);
    if (!validator.validate()) return;

    $('#ctbMuliLang').attr("disabled", false);
    $('#ctbInstitutorEngName').attr("disabled", false);

    Add.CheckLicense();
    if (UseLicense == true) {
        __doPostBack('lbtnRegister', '');
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
    PopupWin(700, 435, url, "-100", "", true);
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
    $("#ctbAddress2").val("");
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
        'callBackPage': "GetSelectUser"
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
function GetSelectUser(selUserList) {

    selUserList = decodeURIComponent(selUserList);

    ParamArray = {
        'companyID': CompanyID,
        'userID': selUserList,
        'roleCode': 'admin'
    };

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Subsidiary/Add.asmx/AddSubsideUser";
    var obj = eval("Add.SuccessSubsideUser");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
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
        }

        Add.LoadData("");
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
        Add.LoadData("");
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2013-03-15
작성목적 : 첨부파일 추가
***************************************************/
Add.AttachFile = function (gubun) {

    AttachGubun = gubun;

    if (gubun == "Amenu") {
        FileType = MenuFileType;
    }
    else if (gubun == "BGlogin") {
        FileType = BGFileType;
    }
    else if (gubun == "BGPotal") {
        FileType = BGFileType;
    }
    else {
        FileType = LoginFileType;
    }

    ParamArray = {
        'swfUploadGUID': UploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc + "( " + FileType + " )",
        'fileCount': '1',
        'moduleName': 'PORTAL',
        'callBackEvent': "Add.GetAttachFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 사진 이미지 넣기
**********************************************************************************************/
Add.GetAttachFileList = function () {
    ParamArray = {
        'UploadGUID': UploadGUID,
        'AttachGubun': AttachGubun,
        'CompanyID': CompanyID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessAttachFile");
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Subsidiary/Add.asmx/MoveImage";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : Freeman
작성일자 : 2013-03-15
작성목적 : 이미지 표시
**********************************************************************************************/
Add.SuccessAttachFile = function (data, status) {

    if (data.d != "") {  //이미지 이동 성공

        location.reload();

        /*
        if (AttachGubun == "login") {
        $("#loginImg")[0].innerHTML = "<img src='" + data.d + "' height='35px' alt='Login Image' />";
        }
        else if (AttachGubun == "Vlogin") {
        $("#VloginImg")[0].innerHTML = "<img src='" + data.d + "' height='140px' alt='Login Image' />";
        }
        else if (AttachGubun == "menu") {
        $("#menuImg")[0].innerHTML = "<img src='" + data.d + "' height='25px' alt='Menu Image' />";
        }
        else if (AttachGubun == "BGlogin") {
        $("#BGloginImg")[0].innerHTML = "<img src='" + data.d + "' height='140px' alt='Login Image' />";
        }
        else if (AttachGubun == "BGPotal") {
        $("#BGPortalImg")[0].innerHTML = "<img src='" + data.d + "' height='140px' alt='Potal Image' />";
        }
        else {
        $("#AmenuImg")[0].innerHTML = "<img src='" + data.d + "' height='28px' alt='Menu Image' />";
        }
        */
    }
}


/**************************************************
작성자   : Freeman
작성일자 : 2013-03-15
작성목적 : 사진, 도장 삭제
***************************************************/
Add.DelImage = function (gubun) {
    if (window.confirm(R_ConfirmDelete)) {
        ParamArray = {
            'CompanyID': CompanyID,
            'AttachGubun': gubun
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessDelImage");
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Subsidiary/Add.asmx/DelImage";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 성공시, 사진, 도장 삭제
***************************************************/
Add.SuccessDelImage = function (data, status) {
    if (data.d == true) {
        location = location.href;
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