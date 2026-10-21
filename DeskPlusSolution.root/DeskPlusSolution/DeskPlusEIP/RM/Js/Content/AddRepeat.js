AddRepeat = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-13
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        // 저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddRepeat.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //달력
        $("#txtcalendar").datepick($.datepicker.regional[PreLang]);

        $("#txtcalendar").keydown(function (e) { return false; });
    });
});

/*************************************************************************************
작성자   : famo23
작성일자 : 2012-03-15
작성목적 : 반복설정 저장
*************************************************************************************/
AddRepeat.Confirm = function () {
    var RepeatType = "";
    var SelectType = "";
    var IntervalCnt = "";
    var Option1 = "";
    var Option2 = "";
    var Option3 = "";
    var Option4 = "";
    var Option5 = "";
    var Option6 = "";
    var Option7 = "";
    var MaxReptCnt = "";
    var EndDate = "";

    var ResultInfo = "";

    var validator = new Validator($("#form1"));

    if ($("#rbday").attr("checked")) {
        // 매일 반복 (주기 입력)
        validator.checkRequired("txtday", R_RepeatInput, true);
        validator.checkOnlyNumber("txtday", R_RepeatNumber, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtday").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtday").focus();
            return;
        }

        RepeatType = "D";
        SelectType = "1";
        IntervalCnt = $("#txtday").val();

        ResultInfo = $("#txtday").val() + R_Repeat17;
    }
    else if ($("#rbmail").attr("checked")) {
        //매일반복 (평일)
        RepeatType = "D";
        SelectType = "2";

        ResultInfo = R_Repeat3 + ",";
    }
    else if ($("#rbweek").attr("checked")) {
        //매주반복
        validator.checkRequired("txtweek", R_RepeatInput, true);
        validator.checkOnlyNumber("txtweek", R_RepeatNumber, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtweek").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtweek").focus();
            return;
        }

        ResultInfo = $("#txtweek").val() + R_Repeat18 + " ";

        var chk = "0";

        if ($("#chksun").attr("checked")) {
            Option1 = "1";
            chk = "1";
            ResultInfo += R_Sun + " ";
        }
        if ($("#chkmon").attr("checked")) {
            Option2 = "1";
            chk = "1";
            ResultInfo += R_Mon + " ";
        }
        if ($("#chktue").attr("checked")) {
            Option3 = "1";
            chk = "1";
            ResultInfo += R_Tue + " ";
        }
        if ($("#chkwed").attr("checked")) {
            Option4 = "1";
            chk = "1";
            ResultInfo += R_Wed + " ";
        }
        if ($("#chkthu").attr("checked")) {
            Option5 = "1";
            chk = "1";
            ResultInfo += R_Thu + " ";
        }
        if ($("#chkfri").attr("checked")) {
            Option6 = "1";
            chk = "1";
            ResultInfo += R_Fri + " ";
        }
        if ($("#chksat").attr("checked")) {
            Option7 = "1";
            chk = "1";
            ResultInfo += R_Sat + " ";
        }

        if (chk == "0") {
            alert(R_SelectWeek);
            return;
        }

        RepeatType = "W";
        SelectType = "1";
        IntervalCnt = $("#txtweek").val();

        ResultInfo += R_Repeat19;
    }
    else if ($("#rbmonth").attr("checked")) {
        //매달반복 : 일자선택
        validator.checkRequired("txtmonth", R_RepeatInput, true);
        validator.checkOnlyNumber("txtmonth", R_RepeatNumber, true);
        validator.checkRequired("txtmonthday", R_RepeatInput, true);
        validator.checkOnlyNumber("txtmonthday", R_RepeatNumber, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtmonth").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtmonth").focus();
            return;
        }

        if (jQuery.trim($("#txtmonthday").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtmonthday").focus();
            return;
        }

        if (Number($("#txtmonthday").val()) > 30) {
            alert(R_RepeatNumber);
            $("#txtmonthday").focus();
            return;
        }

        RepeatType = "M";
        SelectType = "1";
        IntervalCnt = $("#txtmonth").val();
        Option1 = $("#txtmonthday").val();

        ResultInfo = $("#txtmonth").val() + R_Repeat5 + " " + $("#txtmonthday").val() + R_Repeat20;
    }
    else if ($("#rbmonth2").attr("checked")) {
        //매달반복 : 첫째.둘째..
        validator.checkRequired("txtmonth2", R_RepeatInput, true);
        validator.checkOnlyNumber("txtmonth2", R_RepeatNumber, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtmonth2").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtmonth2").focus();
            return;
        }

        var tmplorder = $("option:selected", $("#ddlorder")).text();
        var tmpkind = $("option:selected", $("#ddlkind")).text();

        RepeatType = "M";
        IntervalCnt = $("#txtmonth2").val();
        SelectType = "2";
        Option1 = $("#ddlorder").val();
        Option2 = $("#ddlkind").val();

        ResultInfo = $("#txtmonth2").val() + R_Repeat5 + " " + tmplorder + " " + tmpkind + R_Repeat21;
    }

    //반복범위 : 날짜
    if ($("#rbrepeat1").attr("checked")) {
        var tmpCalendar = $("#txtcalendar").val().split('-').join('');

        if (jQuery.trim(tmpCalendar) == "") {
            alert(R_SelectRepeatDay);
            return;
        }

        var tmpStartDate = StartDate.split('-').join('');

        if (tmpStartDate > tmpCalendar) {
            alert(R_RepeatStartDateEndDate);
            return;
        }

        EndDate = $("#txtcalendar").val();

        ResultInfo += $("#txtcalendar").val() + R_Repeat22;
    }

    //반복범위 : 횟수
    if ($("#rbrepeat2").attr("checked")) {
        validator.checkRequired("txtrepeat", R_RepeatCount, true);
        validator.checkOnlyNumber("txtrepeat", R_RepeatNumber, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtrepeat").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtrepeat").focus();
            return;
        }

        MaxReptCnt = $("#txtrepeat").val();

        ResultInfo += $("#txtrepeat").val() + R_Repeat23;
    }

    top.opener.$("#hdRepeatType").val(RepeatType);
    top.opener.$("#hdSelectType").val(SelectType);
    top.opener.$("#hdIntervalCnt").val(IntervalCnt);
    top.opener.$("#hdOption1").val(Option1);
    top.opener.$("#hdOption2").val(Option2);
    top.opener.$("#hdOption3").val(Option3);
    top.opener.$("#hdOption4").val(Option4);
    top.opener.$("#hdOption5").val(Option5);
    top.opener.$("#hdOption6").val(Option6);
    top.opener.$("#hdOption7").val(Option7);
    top.opener.$("#hdMaxReptCnt").val(MaxReptCnt);
    top.opener.$("#hdEndDate").val(EndDate);

    top.opener.AddRepeatInfo(ResultInfo);

    self.close();
}