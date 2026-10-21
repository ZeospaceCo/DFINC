View = {}



$(function () {
    $(document).ready(function () {
        //수정
        $('#cbtnUpd').click(function (e) {
            e.preventDefault();
            ParamArray = {
                'PageScope': PageScope,
                'Date': paraDate,
                'Opentype': paraOpentype,

                's_Distance': s_Distance,
                's_inquryPerson': s_inquryPerson,
                's_DatePeriod': s_DatePeriod,
                's_SearchKind': s_SearchKind,
                's_SerachValue': s_SerachValue,
                's_StartDate': s_StartDate,
                's_EndDate': s_EndDate,
                's_PageSize': s_PageSize,
                's_CurrentPage': s_CurrentPage,
                'TodoID': TodoID
            };
            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusEIP/Pim/ToDO/AddN.aspx?p=" + GetEncryptValue;

        });

        //취소
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            if (PageScope == "ListDay.aspx" || PageScope == "ListMonth.aspx" || PageScope == "ListWeek.aspx") {
                ParamArray = {
                    'PageScope': PageScope,
                    'Date': paraDate,
                    'Opentype': paraOpentype
                }
                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx?p=" + GetEncryptValue;
                return;
            }

            if (PageUrl == "") {
                PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
            }
            else if (PageUrl == "ListSearch.aspx") {
                PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
                ParamArray = {
                    'Distance': s_Distance,
                    'inquryPerson': s_inquryPerson,
                    'DatePeriod': s_DatePeriod,
                    'SearchKind': s_SearchKind,
                    'SerachValue': s_SerachValue,
                    'StartDate': s_StartDate,
                    'EndDate': s_EndDate,
                    'PageSize': s_PageSize,
                    'CurrentPage': s_CurrentPage
                };
            }
            else if (PageUrl == "ListTotal.aspx") {
                PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
                ParamArray = {
                    'Distance': s_Distance,
                    'inquryPerson': s_inquryPerson,
                    'DatePeriod': s_DatePeriod,
                    'SearchKind': s_SearchKind,
                    'SerachValue': s_SerachValue,
                    'StartDate': s_StartDate,
                    'EndDate': s_EndDate,
                    'PageSize': s_PageSize,
                    'CurrentPage': s_CurrentPage
                };
            }
            else if (PageUrl == "AllSchedule.aspx") {

                PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
                ParamArray = {
                    'UserList': s_UserId,
                    'StartDate': s_StartDate,
                    'EndDate': s_EndDate,
                    'title': s_SearchKind,
                    'name': s_SerachValue
                };
            }
            else if (PageUrl == "ListDay.aspx") {

                PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
                ParamArray = {
                    'Page': '',
                    'Date': ''
                    , 'PageScope': 'ListDay'
                };

            }
            else if (PageUrl == "ListMonth.aspx") {

                PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
                ParamArray = {
                    'Page': '',
                    'Date': '',
                    'PageScope': 'ListMonth'
                };


            }
            else if (PageUrl == "ListWeek.aspx") {

                PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
                ParamArray = {
                    'Page': '',
                    'Date': '',
                    'PageScope': 'ListWeek'
                };

            }
            else {
                ParamArray = {
                    'ScheduleID': '',
                    'Page': '',
                    'Date': ''
                };
            }
            CallEncrypt(CallGetParam());

            location.href = PageUrl + "?p=" + GetEncryptValue;
            //List.Add();
        });

        if (parent.FrameHeightControl != null) {
            var yHeight = 900;
            parent.FrameHeightControl(yHeight);
        }

        //인쇄
        $('#CbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            if (confirm(R_ApplyDelete))
                __doPostBack('lbtnDel', "");
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            View.Close();
        });
        

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        // 댓글리스트
        View.GetShortReplyList();
        View.Resource();
    });
});

View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {
    
    ParamArray = {
        'toDoID': TodoID
    };

    CallEncrypt(CallGetParam())

    //var url = "ShortReplyList.aspx?p=" + GetEncryptValue;
    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    /*
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_Content")[0].innerHTML = R_Contents;
    $("#j_RegDate")[0].innerHTML = R_RegDate;

    
    if ($(".r_mod") != undefined) {
        $(".r_mod").text(R_Modify);
    }

    if ($(".r_del") != undefined) {
        $(".r_del").text(R_Del);
    }

    if ($(".NoListData") != undefined) {
        $(".NoListData").text(R_TextEmpty);
    }
    */


    if ($(".NoAuthListData") != undefined) { $(".NoAuthListData").text(R_SecurityMsg); }

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (toDoID, replyID) {

    ParamArray = {
        'toDoID': toDoID,
        'replyID': replyID
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
View.DelReple = function (replyID) {

    if (confirm(R_ReallyApplyDelete)) {

        ParamArray = {
            'toDoID': TodoID,
            'replyID': replyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Pim/Ws/ToDo/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
    else
        alert(R_MsgError);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'todoID': TodoID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Pim/Ws/ToDo/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder_reply').setTemplateURL('/DeskPlusEIP/Pim/Jt/Todo/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_reply').processTemplate(data.d);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-12-14
작성목적 : 목록으로 가기 
***************************************************/
View.Close = function () {

    self.close();
    return;
}