RewardInfo = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        
        

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("user");
        });

        //부서정보 이동
        $('#cbtnDept').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("dept");
        });

        //리스트 이동
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("list");
        });


        //가족정보
        $('#family').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("family");
        });

        //교육정보
        $('#education').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("education");
        });

        //상벌사항
        $('#reward').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("reward");
        });

        //경력사항
        $('#career').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("career");
        });

        //학력사항
        $('#school').click(function (e) {
            e.preventDefault();
            RewardInfo.GoList("school");
        });

        //저장
        $('#Save').click(function (e) {
            e.preventDefault();
            RewardInfo.Add();
        });

        //순서 변경
        $('#cbtnOrder').click(function (e) {
            e.preventDefault();
            RewardInfo.MoveUpdown();
        });

        $("#ctbRewardDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbRewardDate").keydown(function (e) { return false; });

        RewardInfo.LoadData();

    });

});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동
***************************************************/
RewardInfo.GoList = function (gubun) {

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
RewardInfo.LoadData = function () {


    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/RewardInfo.asmx/GetReward";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID
    };

    var obj = eval("RewardInfo.SuccessLoadData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 가족관계 리스트 표시
**********************************************************************************************/
RewardInfo.SuccessLoadData = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Admin/User/Extend/JTRewardInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    RewardInfo.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

RewardInfo.Resource = function () {
    $("#r_rewardDate")[0].innerHTML = R_RewardDate;
    $("#r_gubun")[0].innerHTML = R_Reward;
    $("#r_rewardType")[0].innerHTML = R_RewardType;
    $("#r_contents")[0].innerHTML = R_Summary;
    $("#r_rewarder")[0].innerHTML = R_RewardPerson;

    $(".r_delete").text(R_Admin_CompanyManager_btnDel);

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 상.벌 저장
**********************************************************************************************/
RewardInfo.Add = function () {


    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/RewardInfo.asmx/AddReward";

    // 필수입력값 체크
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbRewardDate", R_SelectRewardDate, true);
    validator.checkRequired("ctbContents", R_WriteSummary, true);
    validator.checkRequired("ctbRewarder", R_WriteRewarder, true);

    if (!validator.validate()) return;

    var rewardDate = $("#ctbRewardDate").val();
    var gubun = $("#ddlGubun").val();
    var rewardType = $("#ctbRewardType").val();
    var contents = $("#ctbContents").val();
    var rewarder = $("#ctbRewarder").val();

    gubun = gubun.replace(/&/g, '＆').replace(/"/g, '＂');
    rewardType = rewardType.replace(/&/g, '＆').replace(/"/g, '＂');
    contents = contents.replace(/&/g, '＆').replace(/"/g, '＂');
    rewarder = rewarder.replace(/&/g, '＆').replace(/"/g, '＂');

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'rewardDate': rewardDate,
        'gubun': gubun,
        'rewardType': rewardType,
        'contents': contents,
        'rewarder': rewarder
    };

    var obj = eval("RewardInfo.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
RewardInfo.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteSave);
        RewardInfo.LoadData();

        $("#ctbRewardDate").val("");        
        $("#ctbRewardType").val("");
        $("#ctbContents").val("");
        $("#ctbRewarder").val("");
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
RewardInfo.SuccessAdd = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteSave);
        RewardInfo.LoadData();

        $("#ctbRewardDate").val("");
        $("#ctbRewardType").val("");
        $("#ctbContents").val("");
        $("#ctbRewarder").val("");
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
RewardInfo.Del = function (companyID, id) {

    if (window.confirm(R_DeleteInfo)) {
        var url = "/DeskPlusEIP/Account/Ws/MyInfo/Extend/RewardInfo.asmx/DelReward";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': companyID,
            'id': id
        };

        var obj = eval("RewardInfo.SuccessDel");
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
RewardInfo.SuccessDel = function (data, status) {
    if (data.d == true) {        
        RewardInfo.LoadData();
    }
}



/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
RewardInfo.MoveUpdown = function () {

    ParamArray = {
        'userID': UserID,
        'gubun': 'reward'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/MyInfo/Extend/MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}
