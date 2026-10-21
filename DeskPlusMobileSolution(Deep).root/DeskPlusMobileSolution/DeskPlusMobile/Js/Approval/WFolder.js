WFolder = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 자주 사용하는 양식 가져오기
    setTimeout("WFolder.getList();", 400);

    $('#listTitle').click(function (e) {
        e.preventDefault();
        location.href = "Folder.aspx";
    });

    //결재양식함
    $('#aForm').click(function (e) {
        e.preventDefault();
        location.href = "Form.aspx";
    });

    //모바일상신문서함
    $('#aSubmit').click(function (e) {
        e.preventDefault();
        location.href = "SubmitList.aspx";
    });

    //모바일임시보관함
    $('#aDraft').click(function (e) {
        e.preventDefault();
        location.href = "DraftList.aspx";
    });

});




/**************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 양식 리스트 가져오기 (웹서비스 호출)
***************************************************/
WFolder.getList = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetFavFormList";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("WFolder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 양식 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
WFolder.succeeded = function (data, status) {

    $('#phFavoriteForm').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTFavForm.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phFavoriteForm').processTemplate(data.d);

}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-10
작성목적 : 상신 화면으로 가기 
***************************************************/
WFolder.Add = function (formID) {

    ParamArray = {
        'FolderID': '0',
        'FormID': formID
    };

    CallEncrypt(CallGetParam());
    document.location.href = "Add.aspx?p=" + GetEncryptValue;
}
