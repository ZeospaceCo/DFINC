TopMenu = {}



/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-09
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        TopMenu.GetMenu();

    });
});


/**************************************************
작성자   : freeman
작성일자 : 2011-11-09
작성목적 : 관리자 메인 Menu 가져오기
***************************************************/
TopMenu.GetMenu = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/TopMenu.asmx/GetAdminMenu";
    var obj = eval("TopMenu.SuccessAdminMenu");
    AjaxService(url, "", obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-09
작성목적 : 성공시 메인 메뉴 표시
**********************************************************************************************/
TopMenu.SuccessAdminMenu = function (data, status) {
    $('#phMenu').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/Menu/JTMenu.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMenu').processTemplate(data.d);

    $(".menu_gnb a:first").click();
}


/**************************************************
작성자   : 이현수
작성일자 : 2011-05-06
작성목적 : 관리자 페이지 이동
***************************************************/
TopMenu.MovePage = function (_id, _target, _url) {
    //alert(_id + " : " + _target + " : " + _url);
    var mnuID = "#" + _id;

    //Sub 메뉴인지 확인 
    if (_target == "S") {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/TopMenu.asmx/GetAdminSubMenu";
        var data = "ID=" + _id;
        var obj = eval("TopMenu.SuccessSubMenu");
        CallEncrypt(data);
        data = "{p:'" + GetEncryptValue + "'}";

        //부모메뉴 CSS 처리        
        $(".menu_gnb a").removeAttr("class");      //Class 삭제
        $(mnuID).attr("class", "menu_gnb_sel");  //부모 Class

        AjaxService(url, data, obj);
    } else {

        if (_url != "") { parent.frmContent.location.href = _url; }

        //선택한 메뉴에 대한 Class 처리
        $(".menu_sub a").removeAttr("class");      //서브 Class 삭제
        $(mnuID).attr("class", "menu_sub_sel");  //서브 Class
    }
}


/**********************************************************************************************
작성자   : 이현수
작성일자 : 2011-05-06
작성목적 : 관리자 서브 페이지 생성
**********************************************************************************************/
TopMenu.SuccessSubMenu = function (data, status) {
    $('#phSubMenu').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/Menu/JTMenu.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSubMenu').processTemplate(data.d);

    $(".menu_sub a:first").click();
}
