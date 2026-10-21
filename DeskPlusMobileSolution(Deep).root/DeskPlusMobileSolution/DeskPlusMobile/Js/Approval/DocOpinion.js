DocOpinion = {}

/**************************************************
작성자   : famo23
작성일자 : 2017-01-17
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        DocOpinion.viewList();
    });

    $('#btAdd').click(function (e) {
        DocOpinion.Add();
    });

    $('#before').click(function (e) {
        DocOpinion.goBefore();
    });

    $('#listTitle').click(function (e) {
        DocOpinion.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    DocOpinion.MaxLang();
});

/**************************************************
작성자   : famo23
작성일자 : 2017-01-17
작성목적 : 글자 제한
***************************************************/
DocOpinion.MaxLang = function () {

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
작성일자 : 2017-01-17
작성목적 : 목록 페이지 이동
***************************************************/
DocOpinion.goBefore = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText(),
        'TargetDeptID': TargetDeptID,
        'TargetTitleID': TargetTitleID
    };

    CallEncrypt(CallGetParam());

    if (BPage == "Public") {
        location.href = "DeptList.aspx?p=" + GetEncryptValue;
    }
    else if (BPage == "Carboncopy") {
        location.href = "CarboncopyList.aspx?p=" + GetEncryptValue;
    }
    else {
        location.href = "DocList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-17
작성목적 : 취소 버튼 클릭시
***************************************************/
DocOpinion.viewList = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': DocumentID,
        'Sv': SearchValue.securityText(),
        'TargetDeptID': TargetDeptID,
        'TargetTitleID': TargetTitleID,
        'KeyID': KeyID,
        'key2ID': Key2ID
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/DocView.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2017-01-17
작성목적 : 의견 추가  
***************************************************/
DocOpinion.Add = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait); return;
    }

    var tmpOpinion = $("#memo").val();

    if (jQuery.trim(tmpOpinion) == "") {
        NoticeShow("!", R_InputOpinion);
        return;
    }

    LoadingShow(true);

    BtnClickCheck = "Y";

    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'AccTag': AccTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'GroupID': GroupID,
        'Gubun': 'C'
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

                DocOpinion.viewList();

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
