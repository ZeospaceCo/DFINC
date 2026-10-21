Alarm = {}

$(function () {
    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
        });

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            Alarm.Confirm();
        });

        //닫기
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            self.close();
        });

        $("select").change(function (e) {
            e.preventDefault();
            Alarm.ChkItem($("#" + this.id.split("ddl").join("")), true);
        });

        Alarm.Init();
    });
});

Alarm.Init = function () {

    if (IsAdmin == "1") {
        $(".popup_layout_style3").attr("style", "min-height:300px;");       //팝업처리 시,높이값 수정
    }

    var o = $("select");
    $.each(o, function (i, o) {
        Alarm.ChkItem($("#" + o.id.split("ddl").join("")), false);
    });
}

// 사용여부에 따른 옵션처리
Alarm.ChkItem = function (o, ischk) {

    if (o == null) return;

    if ($("select", o).val() == "1") {
        //사용
        if (ischk) $(":checkbox", o).prop("checked", true);
        $(":checkbox", o).prop("disabled", false);
    } else {
        //미사용
        $(":checkbox", o).prop("checked", false);
        $(":checkbox", o).prop("disabled", true);
    }
}

// 저장
Alarm.Confirm = function () {

    //설정값 확인 : 사용유무-옵션1,옵션2

    var app = "", boa = "", pims = "", task = "", work = "", comm = "", rm = "", sns = "";

    //전자결재
    if ($("#ddlAlarmApproval").val() == "1") {
        var cbapp = $("input:checked", "#cbAlarmApproval");
        if (cbapp.length == 0) { alert(MSG_SELITEM); return; }

        app = "1-";
        $.each(cbapp, function (i, o) {
            app += o.value + ",";
        });

    } else {
        app = "0-";
    }

    //게시판
    if ($("#ddlAlarmBoard").val() == "1") {
        var cbboa = $("input:checked", "#cbAlarmBoard");
        if (cbboa.length == 0) { alert(MSG_SELITEM); return; }

        boa = "1-";
        $.each(cbboa, function (i, o) {
            boa += o.value + ",";
        });

    } else {
        boa = "0-";
    }

    //일정관리
    if ($("#ddlAlarmPims").val() == "1") {
        var cbpims = $("input:checked", "#cbAlarmPims");
        if (cbpims.length == 0) { alert(MSG_SELITEM); return; }

        pims = "1-";
        $.each(cbpims, function (i, o) {
            pims += o.value + ",";
        });

    } else {
        pims = "0-";
    }

    //업무협업
    if ($("#ddlAlarmTask").val() == "1") {
        var cbtask = $("input:checked", "#cbAlarmTask");
        if (cbtask.length == 0) { alert(MSG_SELITEM); return; }

        task = "1-";
        $.each(cbtask, function (i, o) {
            task += o.value + ",";
        });

    } else {
        task = "0-";
    }

    //업무일지
    if ($("#ddlAlarmWork").val() == "1") {
        var cbwork = $("input:checked", "#cbAlarmWork");
        if (cbwork.length == 0) { alert(MSG_SELITEM); return; }

        work = "1-";
        $.each(cbwork, function (i, o) {
            work += o.value + ",";
        });

    } else {
        work = "0-";
    }

    //커뮤니티
    if ($("#ddlAlarmCommunity").val() == "1") {
        var cbcomm = $("input:checked", "#cbAlarmCommunity");
        if (cbcomm.length == 0) { alert(MSG_SELITEM); return; }

        comm = "1-";
        $.each(cbcomm, function (i, o) {
            comm += o.value + ",";
        });

    } else {
        comm = "0-";
    }

    //자원관리
    if ($("#ddlAlarmResource").val() == "1") {
        var cbrm = $("input:checked", "#cbAlarmResource");
        if (cbrm.length == 0) { alert(MSG_SELITEM); return; }

        rm = "1-";
        $.each(cbrm, function (i, o) {
            rm += o.value + ",";
        });

    } else {
        rm = "0-";
    }

    //사내SNS
    if ($("#ddlAlarmSNS").val() == "1") {
        var cbsns = $("input:checked", "#cbAlarmSNS");
        if (cbsns.length == 0) { alert(MSG_SELITEM); return; }

        sns = "1-";
        $.each(cbsns, function (i, o) {
            sns += o.value + ",";
        });

    } else {
        sns = "0-";
    }

    //alert(app + "\n" + boa + "\n" + pims + "\n" + task + "\n" + work + "\n" + comm + "\n" + rm + "\n" + sns);
    ParamArray = {
        'cid': CompanyID
        , 'uid': UserID
        , 'app': app
        , 'boa': boa
        , 'pims': pims
        , 'task': task
        , 'work': work
        , 'comm': comm
        , 'rm': rm
        , 'sns': sns
    };
    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Alarm.SuccessConfirm");
    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Alarm.asmx/SetAlarm";
    AjaxService(url, data, obj);
}
Alarm.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(MSG_SUCCESS);
        if (IsAdmin == "1") { self.close(); }
        return;
    }
}