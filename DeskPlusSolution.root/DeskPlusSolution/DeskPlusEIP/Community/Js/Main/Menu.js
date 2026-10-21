var Menu = {};


/**************************************************
작성자 : famo23
작성일자 : 2012-05-08
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive,
            overTop: 200
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        Menu.GetMyList();
        Menu.GetApplyList();

    });
});


/**************************************************
작성자 : famo23
작성일자 : 2012-05-08
작성목적 : 내 커뮤니티 목록 (웹서비스 호출)
***************************************************/
Menu.GetMyList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Main/Menu.asmx/GetMyList";
    var obj = eval("Menu.SuccessMyCom");  
    AjaxService(url, "", obj);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 내 커뮤니티 목록 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessMyCom = function (data, status) {
    var tmpCount = data.d.length;

    for (var i = 0; i < tmpCount; i++) {
        $("#phMyList").append("<li><h2><a onclick=Menu.goUrl('" + data.d[i].CommunityID + "','" + data.d[i].Grade + "')>" + data.d[i].Name + "</a></h2></li>");
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-05-08
작성목적 : 개설 신청중인 목록 (웹서비스 호출)
***************************************************/
Menu.GetApplyList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Main/Menu.asmx/GetApplyList";
    var obj = eval("Menu.SuccessApplyCom");
    AjaxService(url, "", obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 개설 신청중인 목록 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessApplyCom = function (data, status) {
    $("#phApplyList").empty();

    var tmpCount = data.d.length;

    for (var i = 0; i < tmpCount; i++) {
        $("#phApplyList").append("<li><h2>" + data.d[i].Name + "<a onclick=Menu.DelComApply('" + data.d[i].ApplyID + "') style='display:inline-block;' title='" + R_Del + "'><i class='fas fa-times-circle text-danger'></i></a></h2></li>");        
    } 
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 개설 신청중인 목록 저장하기 (웹서비스 호출)
**********************************************************************************************/
Menu.DelComApply = function (applyID) {

    if (window.confirm(R_DelAdminCommunity)) {

        var url = "/DeskPlusEIP/Community/Ws/Main/Menu.asmx/DelComApply";
        ParamArray = {
            'applyID': applyID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Menu.SuccessDelComApply");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-08
작성목적 : 개설 신청중인 목록 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessDelComApply = function (data, status) {
    if (data.d) {
        Menu.GetApplyList();
        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-09
작성목적 : 개설 신청
**********************************************************************************************/
Menu.Add = function () {
    parent.frmContent.location.href = "Add.aspx";
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 커뮤니티 화면가기 - 방문 카운트 넣기 , 포인트 넣기 (웹서비스 호출)
**********************************************************************************************/
Menu.goUrl = function (_communityID, _grade) {
    CommunityID = _communityID;
    Grade = _grade;

    var url = "/DeskPlusEIP/Community/Ws/Main/Menu.asmx/AddVisitPoint";
    ParamArray = {
        'communityID': _communityID,
        'type': '6',
        'menuID': '0'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessAddVisitPoint");

    AjaxService(url, data, obj);
}
    
/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 커뮤니티 화면가기 - 방문 카운트 넣기 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessAddVisitPoint = function (data, status) {

    if (data.d) {

        var AdminCheck = "N";
        if ($("#cbShowImage").is(":checked")) {
            Grade = "2";
            AdminCheck = "Y";
        }

        ParamArray = {
            'communityID': CommunityID,
            'grade': Grade,
            'adminCheck': AdminCheck
        };

        CallEncrypt(CallGetParam());

        parent.location.href = "../Site/Default.aspx?p=" + GetEncryptValue;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}