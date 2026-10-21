Add = {}

$(function () {
    $(document).ready(function () {
        $('input').placeholder(); //watertextbox

        //확인
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            Add.GoList();
        });

        //집주소 우편번호
        $('#cbtnHomePost').click(function (e) {
            e.preventDefault();
            Add.CallHomeZipCode();
        });

        //본적 우편번호
        $('#cbtnBirthPost').click(function (e) {
            e.preventDefault();
            Add.CallHometownZipCode();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });

        //사진 첨부파일 추가
        $('#photoAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile("photo");
        });

        //사진 첨부파일 추가
        $('#sealAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile("seal");
        });

        //도장 첨부파일 삭제
        $('#sealDel').click(function (e) {
            e.preventDefault();
            Add.DelPhotoSeal();
        });

        //로긴중복체크
        $('#cbtnLoginCheck').click(function (e) {
            e.preventDefault();
            Add.LoginCheck();
        });

        //메일중복체크
        $('#cbtnMailCheck').click(function (e) {
            e.preventDefault();
            Add.MailCheck();
        });

        //SNS 등록
        $('#cbtnSNS').click(function (e) {
            e.preventDefault();
            Add.CallSNS();
        });

        $("#ctbBirthDay").datepick($.datepicker.regional[PreLang]);
        $("#ctbJoinDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbRetireDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbWedAnn").datepick($.datepicker.regional[PreLang]);

        $('#ctbMuliLang').attr("disabled", true);
        $('#ctbHomeZipCode').attr("disabled", true);
        $('#ctbHomeAddress1').attr("disabled", true);
        $('#ctbBirthZipCode').attr("disabled", true);
        $('#ctbBirthAddress1').attr("disabled", true);

        $("#ctbBirthDay").keydown(function (e) { return false; });
        $("#ctbJoinDate").keydown(function (e) { return false; });
        $("#ctbRetireDate").keydown(function (e) { return false; });
        $("#ctbRetireDate").keydown(function (e) { return false; });
        $("#ctbWedAnn").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 로그인 체크
***************************************************/
Add.LoginCheck = function () {
    LoginCheck = false;
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Add.asmx/GetLoginCheck";
    var loginID = $("#ctbLoginID").val();

    if (loginID == "") {
        alert(R_InputLoginID);          //"로그인 아이디를 입력해 주세요"
        $("#ctbLoginID").focus();
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'loginID': loginID,
        'gubun': 'add'
    };

    var obj = eval("Add.SuccessLoginCheck");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  로그인 체크
**********************************************************************************************/
Add.SuccessLoginCheck = function (data, status) {
    if (data.d == false) {
        LoginCheck = true;
        alert(R_OnlyOneLoginID);
    }
    else {
        alert(R_RepetitionLoginID);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 메일 중복 체크
***************************************************/
Add.MailCheck = function () {
    MailCheck = false;
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Add.asmx/GetMailCheck";
    var mailID = $("#ctbMailID").val();
    var mailDomain = $("#ddlMailDomain").val();

    if (mailID == "") {
        alert(R_InputBaseMail);
        $("#ctbMailID").focus();
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'mailID': mailID,
        'mailDomain': mailDomain,
        'gubun': 'add'
    };

    var obj = eval("Add.SuccessMailCheck");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 메일 중복 체크
**********************************************************************************************/
Add.SuccessMailCheck = function (data, status) {
    if (data.d == false) {
        MailCheck = true;
        alert(R_OnlyOneMail);
    }
    else {
        alert(R_RepetitionMail);
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 리스트 이동
***************************************************/
Add.GoList = function () {
    ParamArray = {
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGubun': SearchGubun,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listPage': ListPage
    };

    //var referurl = document.referrer;
    var goUrl = "/DeskPlusEIP/Account/Admin/User/" + ListPage;

    CallEncrypt(CallGetParam());
    location.href = goUrl + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 인사 추가
***************************************************/
Add.Confirm = function () {

    var validator = new Validator($("#form1"));

    validator.checkRequired("ctbName", R_InputName, true);                      //"이름을 입력해 주세요"
    validator.checkRequired("ctbLoginID", R_InputLoginID, true);                //"로그인 아이디를 입력해 주세요"
    validator.checkRequired("ctbPassword", R_InputPassword, true);              //"패스워드를 입력해 주세요"

    //메일 제품 없을 경우
    if (MailProduct != "") {
        validator.checkRequired("ctbMailID", R_InputBaseMail, true);                //"기본메일을 입력해 주세요"
        validator.checkRequired("ctbMailPassword", R_InputMailPassword, true);      //"매일패스워드를 입력해 주세요"

        if (MailCheck == false) {
            alert(R_CheckMailRepetition);
            return;
        }
    }

    if (!validator.validate()) return;

    if (LoginCheck == false) {
        alert(R_CheckLoginRepetition);
        return;
    }

    if (!ExistSpecialChar($("#ctbName").val())) {
        alert(R_SpecialChar);
        $("#ctbName").focus();
        return;
    }

    if (WorkYN == "Y") { //근태 메뉴를 사용할때
        if (jQuery.trim($("#ctbJoinDate").val()) == "") {
            alert(R_Admin_User_Input_JoinDate);
            return;
        }

        if ($('#cbxActiveUser').is(":checked") == false) {
            if (jQuery.trim($("#ctbRetireDate").val()) == "") {
                alert(R_Admin_User_Input_RetireDate);
                return;
            }
        }
    }

    if (AddCheck == true) {
        alert("처리중입니다.");
        return;
    }

    AddCheck = true;

    $('#ctbMuliLang').attr("disabled", false);
    $('#ctbHomeZipCode').attr("disabled", false);
    $('#ctbHomeAddress1').attr("disabled", false);
    $('#ctbBirthZipCode').attr("disabled", false);
    $('#ctbBirthAddress1').attr("disabled", false);

    __doPostBack('lbtnRegister', '');
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
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

    $("#ctbMuliLang").val(multiNames);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 페이지 호출
***************************************************/
Add.CallHomeZipCode = function () {
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

            $("#ctbHomeZipCode").val(zipcode);
            $("#ctbHomeAddress").val(fullRoadAddr);
            $("#ctbHomeAddress").focus();
        }
    }).open();

    return;

    //파라미터값 배열에 담기
    ParamArray = {
        'callBackPage': "GetHomeZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;

    PopupWin(600, 435, url, "-300", "", true);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 본적 우편번호 페이지 호출
***************************************************/
Add.CallHometownZipCode = function () {
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

            $("#ctbBirthZipCode").val(zipcode);
            $("#ctbBirthAddress1").val(fullRoadAddr);
            $("#ctbBirthAddress2").val("");
            $("#ctbBirthAddress2").focus();
        }
    }).open();

    return;

    //파라미터값 배열에 담기
    ParamArray = {
        'callBackPage': "GetCompanyZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;

    PopupWin(600, 435, url, "-300", "", true);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 자택 우편번호 설정하기
***************************************************/
function GetHomeZipCode(zipcode, ido, si, dong) {
    ido = decodeURIComponent(ido);
    si = decodeURIComponent(si);
    dong = decodeURIComponent(dong);

    $("#ctbHomeZipCode").val(zipcode);
    $("#ctbHomeAddress1").val(ido + " " + si + " " + dong);
    $("#ctbHomeAddress2").val();
    $("#ctbHomeAddress2").focus();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 자택 우편번호 설정하기
***************************************************/
function GetCompanyZipCode(zipcode, ido, si, dong) {
    ido = decodeURIComponent(ido);
    si = decodeURIComponent(si);
    dong = decodeURIComponent(dong);

    $("#ctbBirthZipCode").val(zipcode);
    $("#ctbBirthAddress1").val(ido + " " + si + " " + dong);
    $("#ctbBirthAddress2").val();
    $("#ctbBirthAddress2").focus();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function (gubun) {
    AttachGubun = gubun;

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'ACCOUNT',
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
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessAttachFile = function (data, status) {
    for (var count in data.d) {
        var photoPath = data.d[count].FilePath;

        if (AttachGubun == "photo") {
            $("#photoImg")[0].innerHTML = "<img src='" + photoPath + "' width='94' height='124' />";
            $("#tbPhotoNEW").val(data.d[count].FileName);
        }
        else if (AttachGubun == "seal") {
            $("#sealImg")[0].innerHTML = "<img src='" + photoPath + "' width='94' height='124' />";
            $("#tbSealNEW").val(data.d[count].FileName);
        }
    }
}

/**************************************************
작성자   :
작성일자 : 2011-04-11
작성목적 : SNS 호출
***************************************************/
Add.CallSNS = function () {
    ParamArray = {
        'facebook': $("#tbFacebook").val(),
        'facebookPwd': $("#tbFacebookPwd").val(),
        'twitter': $("#tbTwitter").val(),
        'twitterPwd': $("#tbTwitterPwd").val(),
        'me2day': $("#tbMe2day").val(),
        'me2dayPwd': $("#tbMe2dayPwd").val(),
        'me2day': $("#tbMe2day").val(),
        'me2dayPwd': $("#tbMe2dayPwd").val(),
        'google': $("#tbGoogle").val(),
        'googlePwd': $("#tbGooglePwd").val(),
        'NaverBlog': $("#tbNaverBlog").val(),
        'NaverBlogPwd': $("#tbNaverBlogPwd").val()
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Admin/User/SNS.aspx?p=" + GetEncryptValue;
    PopupWin(550, 240, url, "-300", "", false);
}

/**************************************************
작성자   :
작성일자 : 2011-04-11
작성목적 : SNS 설정하기
***************************************************/
function SetSNS(p_snsArray) {
    $.each(p_snsArray, function (key, value) {
        switch (key) {
            case "facebook":
                $("#tbFacebook").val(value);
                break;
            case "facebookPwd":
                $("#tbFacebookPwd").val(value);
                break;
            case "twitter":
                $("#tbTwitter").val(value);
                break;
            case "twitterPwd":
                $("#tbTwitterPwd").val(value);
                break;
            case "me2day":
                $("#tbMe2day").val(value);
                break;
            case "me2dayPwd":
                $("#tbMe2dayPwd").val(value);
                break;
            case "google":
                $("#tbGoogle").val(value);
                break;
            case "googlePwd":
                $("#tbGooglePwd").val(value);
                break;
            case "NaverBlog":
                $("#tbNaverBlog").val(value);
                break;
            case "NaverBlogPwd":
                $("#tbNaverBlogPwd").val(value);
                break;
        }
    });

    $("#snsValue")[0].innerHTML = R_Confirm;
}

/**************************************************
작성목적 : 도장 삭제
***************************************************/
Add.DelPhotoSeal = function () {

    $("#sealImg")[0].innerHTML = "<img src='" + ImagePath + "/Icon/ico_noimg2.gif' alt='Seal' />";
    $("#tbSealNEW").val("");
}

