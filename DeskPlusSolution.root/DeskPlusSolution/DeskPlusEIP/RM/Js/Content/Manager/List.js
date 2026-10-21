List = {}

$(function () {

    $(document).ready(function () {
        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        List.GetPList();

    });
});

/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 리스트 가져오기
***************************************************/
List.GetPList = function () {
    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/List.asmx/GetList";

    var obj = eval("List.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
};


/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 리스트 가져오기 성공
***************************************************/
List.SuccessList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/Manager/JTList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);

    List.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $("#j_FolderName")[0].innerHTML = R_FolderName;
    $("#j_Name")[0].innerHTML = R_ResourcesTitle;
    $("#j_Qry")[0].innerHTML = R_Qty;
    $("#j_UsableQty")[0].innerHTML = R_UsableQty;
    $("#j_Authority")[0].innerHTML = R_Authority;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

