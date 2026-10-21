AddOpinion = {}

/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        AddOpinion.viewList();
    });

    $('#btAdd').click(function (e) {
        AddOpinion.Add();
    });

    $('#before').click(function (e) {
        AddOpinion.goBefore();
    });

    $('#listTitle').click(function (e) {
        AddOpinion.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    AddOpinion.MaxLang();
});

/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 글자 제한
***************************************************/
AddOpinion.MaxLang = function () {

    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 목록 페이지 이동
***************************************************/
AddOpinion.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'TargetUID': TargetUID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 취소 버튼 클릭시
***************************************************/
AddOpinion.viewList = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'DID': DocumentID,
            'ATag': ApprovalTag,
            'Seq': Sequence,
            'TargetUID': TargetUID,
            'TargetDID': TargetDID
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'DID': DocumentID,
            'Stat': Stat,
            'ATag': ApprovalTag,
            'AccTag': AccTag,
            'Num2': number2,
            'Seq': Sequence,
            'TargetUID': TargetUID,
            'TargetDID': TargetDID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Approval/View.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-13
작성목적 : 의견 추가  
***************************************************/
AddOpinion.Add = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait); return;
    }

    var tmpOpinion = $("#memo").val();

    if (jQuery.trim(tmpOpinion) == "") {
        NoticeShow("!", R_InputOpinion);
        return;
    }

    LoadingShow(true);

    var Gubun = "P";

    if (Stat != "C") {
        var rtnVal = AddOpinion.GetViewing();

        if (rtnVal == "ERROR") {
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
            return;
        }
        else {
            if (rtnVal != "OK") {
                NoticeShow("!", rtnVal + R_ApprovalWait);
                LoadingShow(false);
                return;
            }
        }
    }
    else {
        Gubun = "C";
    }

    BtnClickCheck = "Y";

    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'AccTag': AccTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'GroupID': GroupID,
        'Gubun': Gubun
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetOpinion",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "OK") {

                NoticeShow("*", R_Complete);

                AddOpinion.viewList();

            }
            else {
                NoticeShow("!", R_ProcessingError);
                BtnClickCheck = "";
                return;
            }
        },
        error: function (msg) {
            BtnClickCheck = "";
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
            return;
        }
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2017-01-16
작성목적 : 문서 보고 있는 지 체크
***************************************************/
AddOpinion.GetViewing = function () {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetViewing",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = msg.d;
        },
        error: function (msg) {
            response = "ERROR";
        }
    });

    return response;
}
