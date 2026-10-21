Folder = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#public').click(function (e) {
        Folder.goList('-6');
    });

    $('#private').click(function (e) {
        Folder.goList('0');
    });

    $('#send').click(function (e) {
        Folder.goList('-3');
    });

    $('#share').click(function (e) {
        Folder.goListParam();
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2012-04-13
작성목적 : 파일 리스트
***************************************************/
Folder.goList = function (arg) {

    ParamArray = {
        'FolderID': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-04-13
작성목적 : 대결문서함
***************************************************/
Folder.goListParam = function () {
    location.href = "Share.aspx";
}

