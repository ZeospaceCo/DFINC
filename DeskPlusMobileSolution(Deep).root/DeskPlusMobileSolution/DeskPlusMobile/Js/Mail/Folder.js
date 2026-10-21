Folder = {}

/**************************************************

작성일자 : 2011-07-07
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 카운트 가져오기
    setTimeout("Folder.getCount();", 400);

    $('#rec').click(function (e) {
        Folder.goList('1');          //받은메일함
    });

    $('#me').click(function (e) {
        Folder.goList('0800');       //내게 보낸 메일함(메뉴에서 삭제)
    });

    $('#sen').click(function (e) {
        Folder.goList('3');          //보낸 메일함
    });

    $('#dra').click(function (e) {
        Folder.goList('4');          //임시 보관함
    });

    $('#del').click(function (e) {
        Folder.goListParam('Trash');
    });

    $('#pri').click(function (e) {
        Folder.goListParam('Private');
    });

    $('#wri').click(function (e) {
        Folder.send();
    });

});


/**************************************************

작성일자 : 2011-07-07
작성목적 : 안 읽은 메일 카운트 가져오기 (웹서비스 호출)
***************************************************/
Folder.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetMailTot";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Folder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-11-15
작성목적 : 안 읽은 메일 카운트 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeeded = function (data, status) {
    var tmpCount = data.d.split('^');
    $("#Receive").text("(" + tmpCount[0] + ")");
    $("#DelBox").text("(" + (tmpCount[1] == undefined ? "" : tmpCount[1]) + ")");
    $("#Private").text("(" + (tmpCount[2] == undefined ? "" : tmpCount[2]) + ")");

    //LoadingShow(false);
}

/**************************************************

작성일자 : 2011-07-07
작성목적 : 메일 리스트 보기 페이지 이동
***************************************************/
Folder.goList = function (arg) {

    ParamArray = {
        'Code': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************

작성일자 : 2011-07-19
작성목적 : 개인함, 휴지통 이동
***************************************************/
Folder.goListParam = function (arg) {

    location.href = arg + ".aspx";
}

/**************************************************

작성일자 : 2013-01-14
작성목적 : 메일쓰기
***************************************************/
Folder.send = function () {

    ParamArray = {
        'History': '3'
    };

    CallEncrypt(CallGetParam());

    location.href = "Send.aspx?p=" + GetEncryptValue;

}