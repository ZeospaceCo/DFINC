Add = {}

$(function () {
    $(document).ready(function () {
        $('input').placeholder(); //watertextbox

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

        $('#HomeZip').click(function (e) {
            e.preventDefault();
            Add.CallHomeZipCode();
        });

        $('#CompanyZip').click(function (e) {
            e.preventDefault();
            Add.CallCompanyZipCode();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbEnglishName").val(), "GetMultiName", MultiLang);
        });

        // 다국회사이름
        $('#MultiCompany').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbCompanyMulti").val(), "GetMultiCompany", MultiLang);
        });

        // 다국부서이름
        $('#MultiDept').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbDepartmentMulti").val(), "GetMultiDept", MultiLang);
        });

        // 다국직급이름
        $('#MultiTitle').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbTitleMulti").val(), "GetMultiTitle", MultiLang);
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        if (PhotoName != "") {
            $("#photoImg")[0].innerHTML = "<img src='" + PhotoName + "' width='52' height='52' alt='' title='' />";
        }

        $('#ctbEnglishName').attr("disabled", true);
        $('#ctbCompanyMulti').attr("disabled", true);
        $('#ctbDepartmentMulti').attr("disabled", true);
        $('#ctbTitleMulti').attr("disabled", true);

        $("#ctbBirthday").datepick($.datepicker.regional[PreLang]);

        $("#ctbBirthday").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 주소 추가 취소
***************************************************/
Add.Cancel = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'types': Types,
        'rTypes': RTypes,
        'currentPage': CurrentPage,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    var goUrl = "ListBlog.aspx";

    CallEncrypt(CallGetParam());
    location.href = goUrl + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 주소 추가
***************************************************/
Add.Confirm = function () {
    $('#ctbEnglishName').attr("disabled", false);
    $('#ctbCompanyMulti').attr("disabled", false);
    $('#ctbDepartmentMulti').attr("disabled", false);
    $('#ctbTitleMulti').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbLastName", R_InputFitstName, true);

    //validator.checkRequired("ctbFamilyName", R_InputFamilyName, true);
    //validator.checkRequired("ctbMail", R_InputEmail, true);
    //validator.checkEmail("ctbMail", R_NotEmailFormat, true);

    if (!validator.validate()) return;

    __doPostBack('lbtnRegister', 'Register');
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
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

    $("#ctbEnglishName").val(multiNames);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 다국회사값 가져와서 설정하기
***************************************************/
function GetMultiCompany(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbCompanyMulti").val(multiNames);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 다국부서값 가져와서 설정하기
***************************************************/
function GetMultiDept(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbDepartmentMulti").val(multiNames);
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 다국직급값 가져와서 설정하기
***************************************************/
function GetMultiTitle(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbTitleMulti").val(multiNames);
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
    PopupWin(700, 435, url, "-100", "", true);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 회사 우편번호 페이지 호출
***************************************************/
Add.CallCompanyZipCode = function () {
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

            $("#ctbCompanyZipCode").val(zipcode);
            $("#ctbCompanyAddress").val(fullRoadAddr);
            $("#ctbCompanyAddress").focus();
        }
    }).open();

    return;

    //파라미터값 배열에 담기
    ParamArray = {
        'callBackPage': "GetCompanyZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;
    PopupWin(700, 435, url, "-100", "", true);
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
    $("#ctbHomeCountry").val(ido);
    $("#ctbHomeCity").val(si);
    $("#ctbHomeGu").val(dong);
    $("#ctbHomeAddress").focus();
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

    $("#ctbCompanyZipCode").val(zipcode);
    $("#ctbCompanyCountry").val(ido);
    $("#ctbCompanyCity").val(si);
    $("#ctbCompanyGu").val(dong);
    $("#ctbCompanyAddress").focus();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'ADDRESS',
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
        $("#photoImg")[0].innerHTML = "<img src='" + photoPath + "' width='52' height='52' />";
        $("#tbPhotoNEW").val(data.d[count].FileName);
    }
}