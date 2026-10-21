Auth = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        Auth.GetPageList();
    });
});




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한 등급 리스트 가져오기
**********************************************************************************************/
Auth.GetPageList = function () {

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/Auth.asmx/GetMyUserRoleList";    

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'deptID': DeptID
    };

    var obj = eval("Auth.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 권한 등급 리스트 가져오기
**********************************************************************************************/
Auth.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/admin/User/JTAuth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    Auth.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Auth.Resource = function () {
    $("#r_rolename")[0].innerHTML = R_AUTH;
    $("#r_name")[0].innerHTML = R_NAME;
    $("#r_types")[0].innerHTML = R_GUBUN;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NOCONTENT;
    }
}