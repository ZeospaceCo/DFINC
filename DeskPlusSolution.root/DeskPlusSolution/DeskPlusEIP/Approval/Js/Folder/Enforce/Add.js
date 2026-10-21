Add = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //우편번호
        $('#zipcode').click(function (e) { e.preventDefault(); Add.SetZipcode(); });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Submit();
        });

        //목록
        $('#CbtnCancel').click(function (e) { e.preventDefault(); Add.Cancel(); });

        //계열사정보
        $('#ddlDept').change(function () {
            Add.GetDeptInfo();
        });
        //발송형태
        $('#ddlSendType').change(function () {
            Add.SetSendType();
        });

        Add.Init();

    });
});


Add.Init = function () {

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();
    ifrmObj = ifrmForm.document;

    setTimeout((function () {
        if ($("#CONTENT", ifrmObj).length > 0) {
            $("#htaEditorHTML").val($("#CONTENT", ifrmObj).html());
        }
        if ($("#EDITOR", ifrmObj).length > 0) {
            $("#htaEditorHTML").val($("#EDITOR", ifrmObj).html());
        }
    }), 100);

    Add.GetDeptInfo();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 우편번호
***************************************************/
Add.SetZipcode = function () {

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
작성목적 : 계열사(직인)정보 가져오기
***************************************************/
Add.GetDeptInfo = function () {

    var selval = $('#ddlDept').val();
    if (selval != "") {

        var stampID = $(".stampID_" + selval).attr("stampID");
        var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/Add.asmx/GetEnforceInfo";
        ParamArray = { 'stampID': stampID };

        var obj = eval("Add.GetDeptInfoSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    } else {
        //우편번호,주소
        $("#ctbZipCode").val(""); $("#ctbAddressFirstBase").val(""); $("#ctbAddressEndBase").val("");
        //전화번호,팩스번호,홈페이지
        $("#ctbPhoneBase").val(""); $("#ctbFaxBase").val(""); $("#ctbHomepage").val("");
        //표현명
        $("#tbExpFirst").val(""); $("#tbExpMid").val(""); $("#tbExpEnd").val("");
        $("#tbEnforceStampID").val("0"); $("#tbEnforceDeptID").val("0");
    }
}
Add.GetDeptInfoSuccess = function (data, status) {

    if (data.d[0] != null) {

        //우편번호,주소
        $("#ctbZipCode").val(data.d[0].ZipCode);
        $("#ctbAddressFirstBase").val(data.d[0].AddressFirstBase);
        $("#ctbAddressEndBase").val(data.d[0].AddressEndBase);

        //전화번호
        $("#ctbPhoneBase").val(data.d[0].PhoneBase);
        //팩스번호
        $("#ctbFaxBase").val(data.d[0].FaxBase);
        //홈페이지
        $("#ctbHomepage").val(data.d[0].Homepage);

        //표현명
        $("#tbExpFirst").val(data.d[0].ExpFirstBase);
        $("#tbExpMid").val(data.d[0].ExpMidBase);
        $("#tbExpEnd").val(data.d[0].ExpEndBase);

        $("#tbEnforceStampID").val(data.d[0].EnforceStampID);
        $("#tbEnforceDeptID").val(data.d[0].DeptID);
    }
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 발송형태에 따른 메일정보 display체크
***************************************************/
Add.SetSendType = function () {
    var ddlSendType = $("#ddlSendType").val();
    if (ddlSendType == "0") {
        $(".clsMail").attr("style", "display:");
    } else {
        $(".clsMail").attr("style", "display:none");
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [문서상신]
***************************************************/
Add.Submit = function () {

    //필수입력
    var validator = new Validator($("#form1"));
    if ($("#ddlDept").val() == "") {
        validator.checkRequired("ddlDept", R_MSG_EnforceMsg00, true);
    }
    validator.checkRequired("ctbSubject", R_MSG_EnforceSubject, true);
    validator.checkRequired("ctbReceive", R_MSG_EnforceReceive, true);
    validator.checkRequired("ctbReference", R_MSG_EnforceReference, true);

    if ($("#ddlSendType").val() == "0") {
        validator.checkRequired("ctbTargetMail", R_MSG_EnforceAddress, true);
        validator.checkRequired("ctbTargetMailSubject", R_MSG_EnforceTargetMail, true);
    }
    validator.checkRequired("ctbPurpose", R_MSG_EnforceTargetMailSubject, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnSubmit', "ADD");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 취소
***************************************************/
Add.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'targetPage': TargetPage
        , 'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}
