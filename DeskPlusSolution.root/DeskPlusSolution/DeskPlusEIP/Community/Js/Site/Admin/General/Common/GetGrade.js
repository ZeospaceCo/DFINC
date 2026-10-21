var GetGrade = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            GetGrade.Confirm();
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
작성일자 : 2012-05-30
작성목적 : 확인 
***************************************************/
GetGrade.Confirm = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Common/GetGrade.asmx/ModGrade";

    var name_base = $("#tbGradeName").val().replace(/,/g, '');
    var name_ko = $("#ctbNameKo").val().replace(/,/g, '');
    var name_en = $("#ctbNameEn").val().replace(/,/g, '');
    var name_ja = $("#ctbNameJa").val().replace(/,/g, '');
    var name_fr = $("#ctbNameFr").val().replace(/,/g, '');
    var name_zhcn = $("#ctbNameZhcn").val().replace(/,/g, '');
    var name_zhtw = $("#ctbNameZhtw").val().replace(/,/g, '');

    /*
    name_base = encodeURIComponent(name_base);
    name_ko = encodeURIComponent(name_ko);
    name_en = encodeURIComponent(name_en);
    name_ja = encodeURIComponent(name_ja);
    name_fr = encodeURIComponent(name_fr);
    name_zhcn = encodeURIComponent(name_zhcn);
    name_zhtw = encodeURIComponent(name_zhtw);
    */


    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'groupID': GroupID,
        'nameBase': name_base,
        'nameKo': name_ko,
        'nameEn': name_en,
        'nameJa': name_ja,
        'nameFr': name_fr,
        'nameZhcn': name_zhcn,
        'nameZhtw': name_zhtw
    };

    var obj = eval("GetGrade.SuccessModGrade");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 :  수정 후 리스트 재갱신 (웹서비스 값 받기)
***************************************************/
GetGrade.SuccessModGrade = function (data, state) {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    parent.Grade.GetPageList();
}

