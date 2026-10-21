ConfirmOpinion = {}

$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); self.close(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ConfirmOpinion.Confirm(); });

        //입력값 체크하기
        if ($.browser.msie) {
            $('.charsRemaining').text($('#tbOpinion').attr('maxlength'));
            $('#tbOpinion').bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('.charsRemaining').text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ConfirmOpinion.MaxLang();
        }

        targetObj = (window.dialogArguments) ? window.dialogArguments : parent;
        $("#tbOpinion").val("");
        $("#tbOpinion").focus();
    });
});
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 글자 제한
***************************************************/
ConfirmOpinion.MaxLang = function () {

    var onEditCallback = function (remaining) {
        $('.charsRemaining').text(remaining);
        if (remaining > 0) { $(this).css('border', '1px solid #d2d2d2'); }
    }
    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }
    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
**********************************************************************************************/
ConfirmOpinion.Confirm = function () {

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("tbOpinion", R_Msg_ErrOpinionNon, true); //validator.checkMaxLength("tbOpinion", 200, R_Msg_ErrOpinion, true);
    if (!validator.validate()) return;

    var ChildDocID = targetObj.ChildDocID;

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ConfirmOpinion.SuccessOpinionAdd");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetDocCont";

    AjaxService(url, data, obj);

    //ConfirmOpinion.CallFunc();
}

ConfirmOpinion.SuccessOpinionAdd = function (data, status) { //alert(data.d);
    if (data.d != "") {
        $("#htaFormHTML", targetObj.document).val(data.d);
        targetObj.View.GetContentHtmlList();
    }
    setTimeout("ConfirmOpinion.CallFunc();", 600);
}

ConfirmOpinion.CallFunc = function () {

    ConfirmOpinion.GetServerTime(); //서버시간정보 가져오기.

    ConfirmOpinion.SetOpinion(UserName, TitleName, DutyName, DeptName, $("#tbOpinion").val().replace(/\n/gi, "<br>"));  // 결재의견 추가.

    targetObj.AddOpinion();         //결재의견 처리함수 호출.

    if (window.dialogArguments) {
        window.returnValue = true; self.close();
    } else {
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 첨언처리(결재의견)
***************************************************/
ConfirmOpinion.SetOpinion = function (_userName, _titleName, _dutyName, _deptName, _txt) { //alert(_userName + "-" + _titleName + "-" + _dutyName + "-" + _deptName + "-" + _txt)

    if (_txt == "" || timeObj == "") return;

    var objTit = _userName + "/" + _titleName + "/" + _deptName;

    var objHtml = "<div class='app_opi'>";
    objHtml += "<span class='tit'>[" + objTit + "]</span>";
    objHtml += "<span class='date'>- " + timeObj + "</span>";
    objHtml += "<span class='opt'> <b>(*)</b></span>";
    objHtml += "<div class='cont'>" + _txt + "</div>";
    objHtml += "</div>";

    if ($("#OPINION", targetObj.ifrmForm.document).length > 0) {                        //alert($("#OPINION", ifrmObj).length)
        $("#OPINION", targetObj.ifrmForm.document)[0].insertAdjacentHTML("beforeEnd", objHtml);
    }

    if ($("#OPINIONVIEW", targetObj.ifrmForm.document).length > 0) {
        $("#OPINIONVIEW", targetObj.ifrmForm.document).html(TagDic_OPINIONVIEW);        //결재의견붙임 태그처리.
    }

    if ($("#tbOpinion", targetObj.document).length > 0) {        //결재의견(For.업무톡)
        $("#tbOpinion", targetObj.document).val(_txt);
    }
}
/**************************************************
서버시간 가져오기
***************************************************/
var todayObj = "", timeObj = "", MonthDayObj = "", timeExObj = "";
ConfirmOpinion.GetServerTime = function () {
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/Common.asmx/GetServerTime";
    ParamArray = { 'p': 'p' };
    var obj = eval("ConfirmOpinion.SuccessServerTime");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
ConfirmOpinion.SuccessServerTime = function (data, status) {
    if (data != null) {
        $.each(data.d, function (i, o) {
            switch (i) {
                case 0: todayObj = o; break;      //getToDayByServer
                case 1: timeObj = o; break;       //getTimeByServer
                case 2: MonthDayObj = o; break;   //getMonthDayByServer
                case 3: timeExObj = o; break;     //getTimeExByServer
            }
        });
    }
}