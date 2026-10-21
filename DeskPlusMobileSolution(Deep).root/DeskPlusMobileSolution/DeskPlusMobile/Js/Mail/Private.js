Private = {}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 개인함 리스트 가져오기
    setTimeout("Private.getList();", 400);

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });
});


/**************************************************

작성일자 : 2011-07-08
작성목적 : 개인함 리스트 가져오기 (웹서비스 호출)
***************************************************/
Private.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetMailBoxList";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Private.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 개인함 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Private.succeeded = function (data, status) {

    $('#phPrivate').setTemplateURL('/DeskPlusMobile/Mail/Jt/JTPrivate.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phPrivate').processTemplate(data.d);

    setTimeout(function () {
        Private.Resource();
    }, 100);
}

/**********************************************************************************************

작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Private.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoPrivateMail;
    }
}

/**************************************************

작성일자 : 2011-07-08
작성목적 : 메일 리스트 보기 페이지 이동
***************************************************/
Private.goList = function (arg) {

    ParamArray = {
        'Code': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

