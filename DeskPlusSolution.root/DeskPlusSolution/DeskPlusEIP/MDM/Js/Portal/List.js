List = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            List.Mod();
        })

        List.GetList();

    })
})

/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 리스트 가져오기
**********************************************************************************************/
List.GetList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Portal/List.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'gubun': '0'
    };

    var obj = eval("List.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/MDM/Jt/Portal/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    List.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 리소스
**********************************************************************************************/
List.Resource = function () {

    $("#r_name")[0].innerHTML = R_Portlet;
    $("#r_order")[0].innerHTML = R_Orders;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 설정 저장
**********************************************************************************************/
List.Mod = function () {

    var selID = List.CheckedID();
    var selValue = "";

    for (var i = 0; i < selID.length; i++) {

        if ($('#cPortalID_' + selID[i]).val() == "1") {
            if (selValue.indexOf("1") > -1) {
                alert(R_TopSet);
                return "";
            }
        }

        if ($('#cPortalID_' + selID[i]).val() == "2") {
            if (selValue.indexOf("2") > -1) {
                alert(R_BottomSet);
                return "";
            }
        }

        selValue += $('#cPortalID_' + selID[i]).val() + ",";
    }

    if (selValue != "") {

        //파라미터값 배열에 담기
        ParamArray = {
            'portalID': selID,
            'orderValue': selValue
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/MDM/Ws/Portal/List.asmx/Mod";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessMod");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 설정 저장 완료
**********************************************************************************************/
List.SuccessMod = function (data, status) {

    if (data.d == true) {
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-17
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedID = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


