var GetPoint = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            GetPoint.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 : 확인 
***************************************************/
GetPoint.Confirm = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Common/GetPoint.asmx/ModPoint";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'type': Type,
        'point': $("#ddlPoint").val()
    };

    var obj = eval("GetPoint.SuccessModPoint");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-21
작성목적 :  수정 후 리스트 재갱신 (웹서비스 값 받기)
***************************************************/
GetPoint.SuccessModPoint = function (data, state) {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    parent.Point.GetPageList();
}

