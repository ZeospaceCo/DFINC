AccountInfo = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            parent.disablePopup();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbName").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AccountInfo.UserSearch();
            }
        });

        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AccountInfo.UserSearch();
        });

    });
});


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 검색
***************************************************/
AccountInfo.UserSearch = function () {

    //부서가 없을 시에는 검색어 입력 필수
    if (DeptID == "0") {

        var validator = new Validator($("#form1"));
        validator.checkRequired("ctbName", R_InputName, true);
        if (!validator.validate()) return;
    }

    __doPostBack('lbtnSearch', '');
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 편지쓰기
***************************************************/
AccountInfo.SendMail = function (_mail) {

    // 딥메일 일때만 적용
    if (URL_GateUrl.indexOf("zeospace.ds") > 0) {
        
        var objPopupMail = window.open("", "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        document.getElementById("frmMail").to.value = _mail;
        document.getElementById("frmMail").action = URL_GateUrl;
        document.getElementById("frmMail").submit();
    }   
}