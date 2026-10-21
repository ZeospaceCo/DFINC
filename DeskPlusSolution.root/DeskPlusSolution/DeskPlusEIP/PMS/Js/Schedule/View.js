View = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();

            View.Mod();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();

            if (confirm(Private_BoardView_IsDelMSG)) {
                if ($("#hdRepeatChk").val() == "") {
                    __doPostBack('cbtnDelete', '');
                }
                else {
                    View.RepeatInfoMod();
                }
            }
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });
    });
});

/**********************************************************************************************
[편집 페이지 이동]
**********************************************************************************************/
View.Mod = function () {
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'rqProjectID': rqProjectID,
        'scheduleID': scheduleID,
        'DisplayID': DisplayID
    };

    CallEncrypt(CallGetParam());
    var url = "ModN.aspx?p=" + GetEncryptValue;

    self.location.href = url;
}

/**********************************************************************************************
[취소]
**********************************************************************************************/
View.Cancel = function () {
    //history.back(-1);
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'rqProjectID': rqProjectID,
        'scheduleID': scheduleID,
        'status': status
    };

    CallEncrypt(CallGetParam());
    var url = "List.aspx?p=" + GetEncryptValue;
    self.location.href = url;
}

View.RepeatInfoMod = function () {
    var url = "/DeskPlusEIP/Pim/Schedule/ConfirmRepeat.aspx";
    PopupWin(500, 220, url, "-500", "-10", false);
}