CharList = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //삭제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            CharList.Del();
        });

        CharList.GetPageList();

    });

});

/**********************************************************************************************
작성자   : shimpark
작성목적 : 이모티콘,특수문자에서 비주얼폰으로 옮기기
**********************************************************************************************/
CharList.AddChar = function (ch) {
    if (parent.SendSMS) {
        parent.SendSMS.AddChar(ch);
    }
    else {
        opener.SendSMS.AddChar(ch);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리스트 호출
**********************************************************************************************/
CharList.GetPageList = function () {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/SMSNew/CharList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
    };

    var obj = eval("CharList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
CharList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Portlet/Jt/Package/SMSNew/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    CharList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '230px', 'height': '230px' });
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리소스
**********************************************************************************************/
CharList.Resource = function () {

    $("#r_content")[0].innerHTML = R_Title;
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 선택한 내용 가져오기
**********************************************************************************************/
CharList.GetContent = function (p_content) {
    CharList.AddChar(p_content);
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
CharList.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
CharList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
CharList.Del = function () {

    //키|구분자,키|구분자
    var selValue = CharList.CheckedValue(); //1|GM

    if (selValue != "") {

        if (window.confirm("삭제하시겠습니까?")) {

            ParamArray = {
                'id': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Portlet/Ws/Package/SMSNew/CharList.asmx/Del";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("CharList.SuccessDel");

            AjaxService(url, data, obj);
        }
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
CharList.SuccessDel = function (data, status) {
    if (data.d == true) {
        CharList.GetPageList();
    }
}