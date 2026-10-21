EduInfo = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("user");
        });

        //부서정보 이동
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("dept");
        });

        //리스트 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("list");
        });


        //가족정보
        $('#family').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("family");
        });

        //교육정보
        $('#education').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("education");
        });

        //상벌사항
        $('#reward').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("reward");
        });

        //경력사항
        $('#career').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("career");
        });

        //학력사항
        $('#school').click(function (e) {
            e.preventDefault();
            EduInfo.GoList("school");
        });

        //저장
        $('#Save').click(function (e) {
            e.preventDefault();
            EduInfo.Add();
        });

        //순서 변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            EduInfo.MoveUpdown();

        });

        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbStartDate").keydown(function (e) { return false; });
        $("#ctbEndDate").keydown(function (e) { return false; });

        EduInfo.LoadData();

    });

});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동
***************************************************/
EduInfo.GoList = function (gubun) {

    var goList = "";

    switch (gubun) {
        case "user":
            goList = "/DeskPlusEIP/Account/Admin/User/Mod.aspx";
            break;
        case "dept":
            goList = "/DeskPlusEIP/Account/Admin/User/UserDept/List.aspx";
            break;
        case "list":
            goList = "/DeskPlusEIP/Account/Admin/User/" + ListPage;
            break;
        case "family":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/FamilyInfo.aspx";
            break;
        case "education":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/EduInfo.aspx";
            break;
        case "reward":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/RewardInfo.aspx";
            break;
        case "career":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/CareerInfo.aspx";
            break;
        case "school":
            goList = "/DeskPlusEIP/Account/Admin/User/Extend/ScholarInfo.aspx";
            break;
    }

    ParamArray = {
        'userID': UserID,
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

    CallEncrypt(CallGetParam());
    var url = goList + "?p=" + GetEncryptValue;
    location = url;
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 가족관계 리스트 호출
**********************************************************************************************/
EduInfo.LoadData = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/EduInfo.asmx/GetEducation";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    var obj = eval("EduInfo.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 가족관계 리스트 표시
**********************************************************************************************/
EduInfo.SuccessLoadData = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Admin/User/Extend/JTEduInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    EduInfo.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

EduInfo.Resource = function () {
    $("#r_name")[0].innerHTML = R_CourseName;
    $("#r_point")[0].innerHTML = R_Score;
    $("#r_grade")[0].innerHTML = R_Grade;
    $("#r_place")[0].innerHTML = R_Location;
    $("#r_date")[0].innerHTML = R_CompleteCheck;
    $("#r_isfinish")[0].innerHTML = R_FinishDate;

    $(".r_delete").text(R_Admin_DeptManager_btnDel);

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 교육관계 저장
**********************************************************************************************/
EduInfo.Add = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/EduInfo.asmx/AddEducation";

    // 필수입력값 체크
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_WriteEduName, true);
    validator.checkRequired("ctbStartDate", R_SelectEduDate, true);
    validator.checkRequired("ctbEndDate", R_SelectEduDate, true);    

    if (!validator.validate()) return;

    var name = $("#ctbName").val();
    var point = $("#ctbPoint").val();
    var grade = $("#ctbGrade").val();
    var place = $("#ctbPlace").val();
    var startDate = $("#ctbStartDate").val();
    var endDate = $("#ctbEndDate").val();
    var isFinish = $("#ddlIsFinish").val();

    name = name.replace(/&/g, '＆').replace(/"/g, '＂');
    point = point.replace(/&/g, '＆').replace(/"/g, '＂');
    grade = grade.replace(/&/g, '＆').replace(/"/g, '＂');
    place = place.replace(/&/g, '＆').replace(/"/g, '＂');
    isFinish = isFinish.replace(/&/g, '＆').replace(/"/g, '＂');

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'name': name,
        'point': point,
        'grade': grade,
        'place': place,
        'startDate': startDate,
        'endDate': endDate,
        'isFinish': isFinish
    };

    var obj = eval("EduInfo.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
EduInfo.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteSave);
        EduInfo.LoadData();

        $("#ctbName").val("");
        $("#ctbPoint").val("");
        $("#ctbGrade").val("");
        $("#ctbPlace").val("");
        $("#ctbStartDate").val("");
        $("#ctbEndDate").val("");
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
EduInfo.Del = function (companyID, id) {

    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/EduInfo.asmx/DelEdu";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': companyID,
            'id': id
        };

        var obj = eval("EduInfo.SuccessDel");
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
EduInfo.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteDelete);
        EduInfo.LoadData();
    }
}



/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
EduInfo.MoveUpdown = function () {

    ParamArray = {
        'userID': UserID,
        'gubun': 'education'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Admin/User/Extend/MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}