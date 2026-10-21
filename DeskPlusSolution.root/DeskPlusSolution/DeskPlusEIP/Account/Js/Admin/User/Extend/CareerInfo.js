CareerInfo = {}



/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("user");
        });

        //부서정보 이동
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("dept");
        });

        //리스트 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("list");
        });


        //가족정보
        $('#family').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("family");
        });

        //교육정보
        $('#education').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("education");
        });

        //상벌사항
        $('#reward').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("reward");
        });

        //경력사항
        $('#career').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("career");
        });

        //학력사항
        $('#school').click(function (e) {
            e.preventDefault();
            CareerInfo.GoList("school");
        });

        //저장
        $('#Save').click(function (e) {
            e.preventDefault();
            CareerInfo.Add();
        });


        //순서 변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            CareerInfo.MoveUpdown();

        });

        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbStartDate").keydown(function (e) { return false; });
        $("#ctbEndDate").keydown(function (e) { return false; }); 

        CareerInfo.LoadData();

    });

});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동
***************************************************/
CareerInfo.GoList = function (gubun) {

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
CareerInfo.LoadData = function () {    

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/CareerInfo.asmx/GetCareer";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    var obj = eval("CareerInfo.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 가족관계 리스트 표시
**********************************************************************************************/
CareerInfo.SuccessLoadData = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Admin/User/Extend/JTCareerInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    CareerInfo.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

CareerInfo.Resource = function () {
    $("#r_date")[0].innerHTML = R_WorkPeriod;
    $("#r_companyname")[0].innerHTML = R_CompanyName;
    $("#r_careerperiod")[0].innerHTML = R_CareerPeriod;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_jobtype")[0].innerHTML = R_JobClass;
    $("#r_etc")[0].innerHTML = R_Admin_UserManager_tabs3;

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
CareerInfo.Add = function () {

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/CareerInfo.asmx/AddCareer";

    // 필수입력값 체크
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbStartDate", R_SelectCareerDate, true);
    validator.checkRequired("ctbEndDate", R_SelectCareerDate, true);
    validator.checkRequired("ctbCompanyName", R_WriteCompanyName, true);

    if (!validator.validate()) return;

    var startDate = $("#ctbStartDate").val();
    var endDate = $("#ctbEndDate").val();
    var companyName = $("#ctbCompanyName").val();
    var careerPeriod = $("#ctbCareerPeriod").val();
    var titleName = $("#ctbTitleName").val();
    var jobType = $("#ctbJobType").val();
    var etc = $("#ctbEtc").val();

    companyName = companyName.replace(/&/g, '＆').replace(/"/g, '＂');
    careerPeriod = careerPeriod.replace(/&/g, '＆').replace(/"/g, '＂');
    titleName = titleName.replace(/&/g, '＆').replace(/"/g, '＂');
    jobType = jobType.replace(/&/g, '＆').replace(/"/g, '＂');
    etc = etc.replace(/&/g, '＆').replace(/"/g, '＂'); 

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'startDate': startDate,
        'endDate': endDate,
        'companyName': companyName,
        'careerPeriod': careerPeriod,
        'titleName': titleName,
        'jobType': jobType,
        'etc': etc
    };

    var obj = eval("CareerInfo.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
CareerInfo.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteSave);
        CareerInfo.LoadData();

        $("#ctbStartDate").val("");
        $("#ctbEndDate").val("");
        $("#ctbCompanyName").val("");
        $("#ctbCareerPeriod").val("");
        $("#ctbTitleName").val("");
        $("#ctbJobType").val("");
        $("#ctbEtc").val("");
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
CareerInfo.Del = function (companyID, id) {


    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/CareerInfo.asmx/DelCareer";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': companyID,
            'id': id
        };

        var obj = eval("CareerInfo.SuccessDel");
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
CareerInfo.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteDelete);
        CareerInfo.LoadData();
    }
}


/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
CareerInfo.MoveUpdown = function () {

    ParamArray = {
        'userID': UserID,
        'gubun': 'career' 
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Admin/User/Extend/MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}