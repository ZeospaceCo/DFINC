ScholarInfo = {}



/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("user");
        });

        //부서정보 이동
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("dept");
        });

        //리스트 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("list");
        });


        //가족정보
        $('#family').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("family");
        });

        //교육정보
        $('#education').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("education");
        });

        //상벌사항
        $('#reward').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("reward");
        });

        //경력사항
        $('#career').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("career");
        });

        //학력사항
        $('#school').click(function (e) {
            e.preventDefault();
            ScholarInfo.GoList("school");
        });

        //저장
        $('#Save').click(function (e) {
            e.preventDefault();
            ScholarInfo.Add();
        });

        //순서 변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            ScholarInfo.MoveUpdown();
        });

        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbStartDate").keydown(function (e) { return false; });
        $("#ctbEndDate").keydown(function (e) { return false; });

        ScholarInfo.LoadData();

    });

});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동
***************************************************/
ScholarInfo.GoList = function (gubun) {

    var goList = "";

    switch (gubun) {
        case "user":
            goList = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
            break;
        case "family":
            goList = "/DeskPlusEIP/Account/MyInfo/Extend/FamilyInfo.aspx";
            break;
        case "education":
            goList = "/DeskPlusEIP/Account/MyInfo/Extend/EduInfo.aspx";
            break;
        case "reward":
            goList = "/DeskPlusEIP/Account/MyInfo/Extend/RewardInfo.aspx";
            break;
        case "career":
            goList = "/DeskPlusEIP/Account/MyInfo/Extend/CareerInfo.aspx";
            break;
        case "school":
            goList = "/DeskPlusEIP/Account/MyInfo/Extend/ScholarInfo.aspx";
            break;
    }
    
    location = goList;
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 가족관계 리스트 호출
**********************************************************************************************/
ScholarInfo.LoadData = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/ScholarInfo.asmx/GetScholar";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    var obj = eval("ScholarInfo.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 가족관계 리스트 표시
**********************************************************************************************/
ScholarInfo.SuccessLoadData = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Admin/User/Extend/JTScholarInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    ScholarInfo.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ScholarInfo.Resource = function () {
    $("#r_date")[0].innerHTML = R_Period;                   //"기간"
    $("#r_schoolname")[0].innerHTML = R_SchoolName;         //"학교명"
    $("#r_degree")[0].innerHTML = R_Degree;                 //"학위";
    $("#r_major")[0].innerHTML = R_Major;                   //"전공"
    $("#r_graduatetype")[0].innerHTML = R_Graduation        //"졸업여부";
    $("#r_etc")[0].innerHTML = R_Seat                       //"소재지";

    $(".r_delete").text(R_Admin_CompanyManager_btnDel);

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 교육관계 저장
**********************************************************************************************/
ScholarInfo.Add = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/ScholarInfo.asmx/AddScholar";

    // 필수입력값 체크
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbStartDate", R_SelectPeriod, true);
    validator.checkRequired("ctbEndDate", R_SelectPeriod, true);
    validator.checkRequired("ctbSchoolName", R_WriteSchoolName, true);

    if (!validator.validate()) return;

    var startDate = $("#ctbStartDate").val();
    var endDate = $("#ctbEndDate").val();
    var schoolName = $("#ctbSchoolName").val();
    var degree = $("#ddlDegree").val();
    var major = $("#ctbMajor").val();
    var graduationType = $("#ddlGraduateType").val();
    var etc = $("#ctbEtc").val();

    schoolName = schoolName.replace(/&/g, '＆').replace(/"/g, '＂');
    degree = degree.replace(/&/g, '＆').replace(/"/g, '＂');
    major = major.replace(/&/g, '＆').replace(/"/g, '＂');
    graduationType = graduationType.replace(/&/g, '＆').replace(/"/g, '＂');
    etc = etc.replace(/&/g, '＆').replace(/"/g, '＂');

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'startDate': startDate,
        'endDate': endDate,
        'schoolName': schoolName,
        'degree': degree,
        'major': major,
        'graduationType': graduationType,
        'etc': etc
    };

    var obj = eval("ScholarInfo.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
ScholarInfo.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteSave);
        ScholarInfo.LoadData();

        $("#ctbStartDate").val("");
        $("#ctbEndDate").val("");
        $("#ctbSchoolName").val("");        
        $("#ctbMajor").val("");        
        $("#ctbEtc").val("");
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
ScholarInfo.Del = function (companyID, id) {

    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/ScholarInfo.asmx/DelScholar";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': companyID,
            'id': id
        };

        var obj = eval("ScholarInfo.SuccessDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }

}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 삭제 성공시 리스트 표시
**********************************************************************************************/
ScholarInfo.SuccessDel = function (data, status) {
    if (data.d == true) {
        ScholarInfo.LoadData();
    }
}



/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
ScholarInfo.MoveUpdown = function () {

    ParamArray = {
        'userID': UserID,
        'gubun': 'scholar'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/MyInfo/Extend/MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}
