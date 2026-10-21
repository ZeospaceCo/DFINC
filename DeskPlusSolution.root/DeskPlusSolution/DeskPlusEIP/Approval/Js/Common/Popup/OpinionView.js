OpinionView = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#Close').click(function (e) {
            if (window.dialogArguments) {
                window.returnValue = false; self.close();
            } else {
                if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            }
        });

        OpinionView.LoadData();
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 리스트 초기화
***************************************************/
OpinionView.LoadData = function () {
    OpinionView.GetOpinionInfo();
    OpinionView.Resource();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2020-09-04
작성목적 : 다국어
***************************************************/
OpinionView.Resource = function () {

}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 리스트 호출
**********************************************************************************************/
OpinionView.GetOpinionInfo = function () {
    ParamArray = {
        'companyID': CompanyID,
        'upPathID': UpPathID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("OpinionView.SuccessOpinionInfo");
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/OpinionView.asmx/GetOpinionList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 리스트 보여주기
**********************************************************************************************/
OpinionView.SuccessOpinionInfo = function (data, status) {
    if (data.d != null) {
        $('#placeholder_app').setTemplateURL('/DeskPlusEIP/Approval/Jt/Common/Popup/JTOpinionView.htm?seq=' + Guids(), null, { filter_data: false });
        $('#placeholder_app').processTemplate(data.d);
    }
}
