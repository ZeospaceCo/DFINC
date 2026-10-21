Headers = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //취소
        $('#cbtnApply').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        Headers.GetPageList();
    });
});

/**************************************************
말머리 추가
***************************************************/
Headers.Add = function () {
    var txtValue = $("#Newheader").val();

    var url = "/DeskPlusEIP/Board/Ws/Admin/Category/Headers.asmx/AddHeader";

    ParamArray = {
        'categoryID': CatID,
        'headerID': 0,
        'name': txtValue
    };

    var obj = eval("Headers.Success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
말머리 수정
***************************************************/
Headers.Mod = function (p_txtObj, p_headerID) {
    if (p_txtObj == "" || p_headerID == "") return;

    var txtValue = $("#" + p_txtObj).val();

    var url = "/DeskPlusEIP/Board/Ws/Admin/Category/Headers.asmx/AddHeader";

    ParamArray = {
        'categoryID': CatID,
        'headerID': p_headerID,
        'name': txtValue
    };

    var obj = eval("Headers.Success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 완료
**********************************************************************************************/
Headers.Success = function (data, status) {
    if (data.d == true) {
        alert(R_Processing);
        Headers.GetPageList()
    }
}

/**************************************************
말머리 삭제
***************************************************/
Headers.Del = function (p_txtObj, p_headerID) {
    if (p_headerID == "") return;

    if (window.confirm(R_ApplyDelete)) {
        var url = "/DeskPlusEIP/Board/Ws/Admin/Category/Headers.asmx/DelHeader";

        ParamArray = {
            'categoryID': CatID,
            'headerID': p_headerID
        };

        var obj = eval("Headers.Success");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
Headers.GetPageList = function () {
    var url = "/DeskPlusEIP/Board/Ws/Admin/Category/Headers.asmx/GetHeaderList";

    ParamArray = {
        'categoryID': CatID
    };

    var obj = eval("Headers.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Headers.SuccessPageList = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Admin/Content/JTHeaders.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    $('#loading').html("");

    if ($(".IconAdd") != undefined) { $(".IconAdd").attr('src', ImagePath + '/Lang/ko/Tree_add.gif'); }
    if ($(".IconEdit") != undefined) { $(".IconEdit").attr('src', ImagePath + '/Lang/ko/Tree_modify.gif'); }
    if ($(".IconDel") != undefined) { $(".IconDel").attr('src', ImagePath + '/Lang/ko/Tree_delete.gif'); }
}