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
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Join.Cancel();
        });
    });
});

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-10
작성목적 : 회원가입
**********************************************************************************************/
Join.Confirm = function () {
    var myDescription = $("#ctbMyDescription").val();   // 자기소개
    var communityName = $("#lbCmnName").text();         // 커뮤니티 이름
    
    // 자기소개 입력 체크
    if (myDescription.replace(/^\s\s*/, "").replace(/\s\s*$/, "") == "") {
        alert(R_InputDes);
        $("#ctbMyDescription").focus();
        return;
    }

    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Join.asmx/AddUserApply";
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
        if (JoinType == "1") {
            alert(R_ConfirmJoin);

            ParamArray = {
                'communityID': CommunityID,
                'grade': '9',
                'adminCheck': 'N'
            };

            CallEncrypt(CallGetParam());

            parent.frmLeft.location.href = "Menu.aspx?p=" + GetEncryptValue;
            location.href = "Content.aspx?p=" + GetEncryptValue;
        }
        else {
            alert(R_CompleteApply);
            Join.Cancel();
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-12
작성목적 : 취소
**********************************************************************************************/
Join.Cancel = function () {

    ParamArray = {
        'communityID': CommunityID,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());
    location.href = "Content.aspx?p=" + GetEncryptValue;
}
