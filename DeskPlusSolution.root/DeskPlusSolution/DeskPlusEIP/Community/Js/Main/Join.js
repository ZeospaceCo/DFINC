var Join = {};


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-08
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        // 가입신청함, 관리자의 승인 기다리는 중
        if (JoinCheck == 1) {
            $("#cbtnConfirm").hide();
            $("#lblMsg").text(R_ConfirmWaiting);
        }

        // 관리자가 강제탈퇴 시킴(가입불가능)
        if (StatusCheck == 1) {
            $("#cbtnConfirm").hide();
            $("#lblMsg").text(R_RejectJoin);
        }

        //가입
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Join.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            //if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            location.href = "Content.aspx";
        });

        Join.Init();
    });
});

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 가입 유형에 따른 가입신청 세팅
**********************************************************************************************/
Join.Init = function () {

    $("#type2").hide();
    $("#type3").hide();

    // 비공개
    if (Type == "2") {
        $("#type2").show();
    }
    else if (Type == "3") { // 퀴즈
        $("#type3").show();
    }

    __addEventListener(document, "click", Join.click);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-10
작성목적 : 회원가입
**********************************************************************************************/
Join.Confirm = function () {
    var myDescription = $("#ctbMyDescription").val();   // 자기소개
    var communityName = $("#lbCmnName").text();         // 커뮤니티 이름
    MyAnswer = $("#ctbAnswer").val();                   // 답변

    if (Type != "3") {
        // 자기소개 입력 체크
        if (myDescription.replace(/^\s\s*/, "").replace(/\s\s*$/, "") == "") {
            alert(R_InputDes);
            $("#ctbMyDescription").focus();
            return;
        }
    }

    // 퀴즈형 일 경우 답변 체크
    if (Type == "3") {
        if (Join.Quize() == "") return;
    }

    var url = "/DeskPlusEIP/Community/Ws/Main/Join.asmx/AddUserApply";
    ParamArray = {
        'communityID': CommunityID,
        'description': myDescription,
        'communityName': communityName,
        'type': Type,
        'joinType': JoinType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Join.SuccessAddUserApply");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-10
작성목적 : 회원가입 신청 완료
**********************************************************************************************/
Join.SuccessAddUserApply = function (data, status) {
    if (data.d) {
        if (Type == 3) {
            alert(R_ConfirmJoin);
        }
        else {
            alert(R_CompleteApply);
        }

        location.href = "Content.aspx";
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 퀴즈형 회원신청 답변 체크
**********************************************************************************************/
Join.Quize = function () {
    var rtn = "TRUE";

    if (Check == "0") {
        Join.GetAnswer();
        Check = "1";
    }

    // 질문 답변 체크
    if (Answer != MyAnswer) {
        alert(R_WrongAnswer);
        $("#ctbAnswer").focus();
        rtn = "";
    }
    return rtn;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 질문에 대한 답변 가져오기
**********************************************************************************************/
Join.GetAnswer = function () {

    var url = "/DeskPlusEIP/Community/Ws/Main/Join.asmx/GetAnswer";
    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Join.SuccessAnswer");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 질문에 대한 답변 가져오기 성공
**********************************************************************************************/
Join.SuccessAnswer = function (data, status) {
    Answer = data.d[0].Answer;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Join.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": ProjectInfo.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;