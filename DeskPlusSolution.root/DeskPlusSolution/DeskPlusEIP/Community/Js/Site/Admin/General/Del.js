var Del = {};


/**************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //해체
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Del.Confirm();
        });       

    });
});


/**************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 커뮤니티 해체
***************************************************/
Del.Confirm = function () {

    if (BtnCheck == "Y") return;
    //alert(R_Processing);
    //return;
    
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbDelDes", R_InputDissolution, true);
    if (!validator.validate()) return;

    if (window.confirm(R_DelConfirmCommunity)) {

        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');

    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-05-11
작성목적 : 개설 신청후 트리 추가
***************************************************/
Del.GoMain = function () {

    alert(R_CompleteConfirm);
    var goUrl = "/DeskPlusEIP/Community/Default.aspx";
    parent.location.href = goUrl;
}
