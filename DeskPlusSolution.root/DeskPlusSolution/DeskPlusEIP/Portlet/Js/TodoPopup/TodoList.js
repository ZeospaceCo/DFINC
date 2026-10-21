TodoList = {}

$(function () {
    $(document).ready(function () {

        //환경설정
        $('#settmenu').click(function (e) {
            e.preventDefault();
            $("#settarea").fadeToggle("slow", "linear");
        });

        //[닫기]
        $('#settclose').click(function (e) { e.preventDefault(); TodoList.SettClose(); });

        //[저장]
        $('#settsave').click(function (e) { e.preventDefault(); TodoList.Save(); });

        // 메뉴설정 초기화
        TodoList.GetMenu(TodoMenu);

        // 데이타 가져오기
        TodoList.GetList(TodoMenu);
    });
});


// 메뉴설정 초기화
TodoList.GetMenu = function (_todomenu) {

    if (_todomenu == "") return;

    // 자원관리자만 보임
    if (IsMgr == "TRUE") {
        $(".mgr").show();
    } else {
        $(".mgr").hide();
    }

    jQuery.each($("input:checked"), function (i, o) {
        $(o).removeAttr("checked");
    });

    var arrobj = TodoMenu.split(",");
    $.each(arrobj, function (i, o) {
        $("#" + o).attr("checked", "checked");
    });
}


// 데이타 가져오기 (건수,리스트)
TodoList.GetList = function (_todomenu) {

    if (_todomenu == "") return;

    $(".pTWa").html("");

    var arrobj = TodoMenu.split(",");
    $.each(arrobj, function (i, o) {

        if (o != "") {
            ParamArray = { 'itemid': "item_" + i, 'menu': o };

            var url = "/DeskPlusEIP/Portlet/Ws/TodoPopup/TodoList.asmx/GetList";
            var obj = eval("TodoList.SuccessList");

            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    });
}
TodoList.SuccessList = function (data, status) {
    if (data != null) {
        if ($("#" + data.d.CLIENTID).length > 0) {
            $("#" + data.d.CLIENTID).setTemplateURL("/DeskPlusEIP/Portlet/Jt/TodoPopup/JTTodoList.htm?seq=" + Guids(), null, { filter_data: false });
            $("#" + data.d.CLIENTID).processTemplate(data.d);
        }
    }
}

//설정-[저장]
TodoList.Save = function () {

    var arr = $("input:checked");

    // 최대항목 체크 (4개까지)
    if (arr.length > 4) { alert(MsgLimitedItem); return; }

    var vals = "";
    jQuery.each(arr, function (i, o) {
        vals += o.id + ",";
    });

    TodoMenu = vals;

    //웹서비스호출
    var url = "/DeskPlusEIP/Portlet/Ws/TodoPopup/TodoList.asmx/SetSetting";
    ParamArray = { 'todomenu': vals };

    var obj = eval("TodoList.SuccessSave");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
TodoList.SuccessSave = function (data, status) {
    if (data.d) {
        TodoList.SettClose();
        TodoList.GetList(TodoMenu);
    }
}



//설정-[닫기]
TodoList.SettClose = function () {
    TodoList.GetMenu(TodoMenu);
    $("#settarea").fadeOut(500);
}

//쿠키저장
TodoList.setCookie = function (name, value, expiredays) {
    var todayDate = new Date();
    var year = todayDate.getFullYear();
    var month = todayDate.getMonth();
    var day = todayDate.getDate() + 1 + expiredays;
    todayDate = new Date(year, month, day);
    document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + ";"
}

// [오늘 하루 띄우지 않음]
TodoList.saveCookie = function() {
    TodoList.setCookie(popid, "yes", 0);
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

//[일주일 동안 띄우지 않음]
TodoList.ViewChecked = function () {
    TodoList.setCookie(popid, "yes", 7);
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

//[닫기]
TodoList.PopupClose = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

//[항목선택]
TodoList.PopupItem = function (_url) {
    if (_url == "") return;
    window.open(_url, _url, 'menubar=no, scrollbars=yes, status=no, toolbar=no, resizable=yes, width=1200, height=800');
}

// 더보기
//$("#ptPreview_" + portletDom).on("click", function (e) {
//    e.preventDefault();

//    parent.frmTop.Menu.GoParam("webpims");
//    document.location.href = "/DeskPlusEIP/PIM/Default.aspx";
//});