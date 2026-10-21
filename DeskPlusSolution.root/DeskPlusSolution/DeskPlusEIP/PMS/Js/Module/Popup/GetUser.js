GetUser = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            GetUser.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            GetUser.Cancel();
        });
    });
});

/**********************************************************************************************
[참여자 리스트 선택]
**********************************************************************************************/
function lbxListOnChange() {
    //    var selIndex = $("#lbxList").selectedIndex;

    var selValue = "";
    var selText = "";

    //    if (selIndex != -1) {
    //	    for (i = 0; i < $("#lbxList").size(); i++)
    //        {
    //            if ($("#lbxList").options[i].selected == true)
    //            {
    //                selValue += $("#lbxList").options[i].value + ",";
    //                selText += $("#lbxList").options[i].text + ",";
    //            }
    //        }
    //	}

    $("#lbxList option:selected").each(function () {
        selValue += $(this).val() + ",";
        selText += $(this).text() + ",";
    });

    $("#tbReceiveMan").val(selText);
    $("#hiddenSelectedID").val(selValue);
}

/**********************************************************************************************
[확인]
**********************************************************************************************/
GetUser.Confirm = function () {
    var userName = $("#tbReceiveMan").val();
    var userID = $("#hiddenSelectedID").val();
    var response = userID + "‡" + userName + "‡M";

    parent.SelectedGetUser(escape(response));
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**********************************************************************************************
[닫기]
**********************************************************************************************/
GetUser.Cancel = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}