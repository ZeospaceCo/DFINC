FamilyInfo = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {



        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("user");
        });

        //부서정보 이동
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("dept");
        });

        //리스트 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("list");
        });


        //가족정보
        $('#family').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("family");
        });

        //교육정보
        $('#education').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("education");
        });

        //상벌사항
        $('#reward').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("reward");
        });

        //경력사항
        $('#career').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("career");
        });

        //학력사항
        $('#school').click(function (e) {
            e.preventDefault();
            FamilyInfo.GoList("school");
        });

        //저장
        $('#Save').click(function (e) {
            e.preventDefault();
            FamilyInfo.Add();
        });


        //순서 변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            FamilyInfo.MoveUpdown();
        });

        $("#ctbBirth").datepick($.datepicker.regional[PreLang]);

        $("#ctbBirth").keydown(function (e) { return false; });

        FamilyInfo.LoadData();

    });

});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동
***************************************************/
FamilyInfo.GoList = function (gubun) {

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
FamilyInfo.LoadData = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/FamilyInfo.asmx/GetFamily";
    
    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    var obj = eval("FamilyInfo.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 가족관계 리스트 표시
**********************************************************************************************/
FamilyInfo.SuccessLoadData = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Admin/User/Extend/JTFamilyInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    FamilyInfo.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

FamilyInfo.Resource = function () {
    $("#r_relation")[0].innerHTML = R_Relationship;                     //"관계";
    $("#r_name")[0].innerHTML = R_Private_AccountInfo_ltUserName        //"이름";
    $("#r_birth")[0].innerHTML = R_Admin_UserManager_ltBirthDay         //"생년월일";
    $("#r_job")[0].innerHTML = R_Job                                    //"직업";
    $("#r_scholar")[0].innerHTML = R_Academic                           //"학력";
    $("#r_withlive")[0].innerHTML = R_Cohabitation                      //"동거";

    $(".r_delete").text(R_Admin_DeptManager_btnDel);

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 가족관계 리스트 호출
**********************************************************************************************/
FamilyInfo.Add = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/FamilyInfo.asmx/AddFamily";

    var validator = new Validator($("#form1"));
    validator.checkRequired("ddlRelationship", R_SelectRelation, true);
    validator.checkRequired("ctbName", R_InputName, true);
    if (!validator.validate()) return;

    var relation = $("#ddlRelationship").val();
    var scholar = $("#ddlScholarship").val();
    var withLive = $("#ddlIsWithLive").val();
    var name = $("#ctbName").val();
    var birth = $("#ctbBirth").val();
    var job = $("#ctbJob").val();

    relation = relation.replace(/&/g, '＆').replace(/"/g, '＂');
    scholar = scholar.replace(/&/g, '＆').replace(/"/g, '＂');
    withLive = withLive.replace(/&/g, '＆').replace(/"/g, '＂');
    name = name.replace(/&/g, '＆').replace(/"/g, '＂');
    birth = birth.replace(/&/g, '＆').replace(/"/g, '＂');
    job = job.replace(/&/g, '＆').replace(/"/g, '＂');

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'relation': relation,
        'scholar': scholar,
        'withLive': withLive,
        'name': name,
        'birth': birth,
        'job': job
    };

    var obj = eval("FamilyInfo.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
FamilyInfo.SuccessAdd = function (data, status) {
    
    if (data.d == true) {
        alert(R_CompleteSave);
        FamilyInfo.LoadData();

        $("#ctbName").val("");
        $("#ctbBirth").val("");
        $("#ctbJob").val("");
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
FamilyInfo.Del = function (companyID, id) {

    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/Admin/User/Extend/FamilyInfo.asmx/DelFamily";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': companyID,
            'id': id
        };

        var obj = eval("FamilyInfo.SuccessDel");
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
FamilyInfo.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteDelete);
        FamilyInfo.LoadData();
    }
}


/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
FamilyInfo.MoveUpdown = function () {

    ParamArray = {
        'userID': UserID,
        'gubun': 'family'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Admin/User/Extend/MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}
