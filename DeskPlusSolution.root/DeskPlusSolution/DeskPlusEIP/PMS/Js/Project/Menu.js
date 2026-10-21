Menu = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide'
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        // 홈이동
        $('#HomeGo').click(function (e) {
            e.preventDefault();
            parent.location.href = "/DeskPlusEIP/PMS/Default.aspx";
        });

    });
});

/**********************************************************************************************
페이지 이동
**********************************************************************************************/
function goUrl(s) {
    parent.contentFrm.location.href = s;
}

/**********************************************************************************************
[폐쇄신청]
**********************************************************************************************/
Menu.RequestClosed = function () {
    var response = AjaxIsAuth(projectID);

    if (response == "1") {
        if (window.confirm(closedAlarm)) {
            var response = AjaxRequestClosed(projectID);

            if (response == "1") {
                alert(closedOk);
            }
            else {
                alert(errorAlarm);
            }
        }
    }
    else {
        alert(R_OnlyAdminClose);
    }
}

/**********************************************************************************************
[총평]
**********************************************************************************************/
Menu.Opinion = function () {
    var response = AjaxIsAuth(projectID);
    if (response == "1") {
        //window.open(url, "Opinion", "toolbar=no,width=400,height=200,directories=no,status=no,scrollbars=no,resize=no,menubar=no");

        var url = "../Admin/Opinion/Add.aspx";
        var param = projParam;
        WindowObjectReference = PopupWinOpen(url, param, "Opinion", "500", "175")
    }
    else {
        alert(Project_Menu_Opinion);
    }
}
