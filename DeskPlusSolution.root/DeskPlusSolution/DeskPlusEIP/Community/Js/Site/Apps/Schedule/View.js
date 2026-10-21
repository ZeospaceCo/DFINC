View = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.GoList();
        });
        
        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
            return;
        });

    });
});

/**************************************************************
작성자 : famo23
작성일자 : 2012-06-01
작성목적 : 스케줄 삭제
**************************************************************/
View.Del = function () {
    if (confirm(R_DelSchedule)) {

        if (ScheduleID != ParentScheduleID) {
            var url = "/DeskPlusEIP/Community/Site/Apps/Schedule/DelRepeat.aspx";
            PopupWin(400, 225, url, "50", "", true);
        }
        else {
            View.GetRepeat();
        }
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-06-01
작성목적 : 자원 수정
**************************************************************/
View.Mod = function () {

    ParamArray = {
        'scheduleID': ScheduleID,
        'communityID': CommunityID,
        'menuID': MenuID,
        'tmpDate': TmpDate,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;

}

/**************************************************************
작성자 : famo23
작성일자 : 2012-06-01
작성목적 : 목록
**************************************************************/
View.GoList = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'tmpDate': TmpDate,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복 일정 삭제 
**********************************************************************************************/
function RepeatDel(_arg) {
    $("#hdRepeatSel").val(_arg);

    __doPostBack('lbtnConfirm', '');
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복일정인지 체크 (웹서비스 호출)
**********************************************************************************************/
View.GetRepeat = function () {

    ParamArray = {
         'communityID': CommunityID,
         'scheduleID': ScheduleID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessGetRepeat");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Schedule/View.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복일정인지 체크 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessGetRepeat = function (data, status) {

    if (data.d) {
        var url = "/DeskPlusEIP/Community/Site/Apps/Schedule/DelRepeat.aspx";
        PopupWin(400, 225, url, "50", "", true);
    }
    else {
        __doPostBack('lbtnConfirm', '');
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}