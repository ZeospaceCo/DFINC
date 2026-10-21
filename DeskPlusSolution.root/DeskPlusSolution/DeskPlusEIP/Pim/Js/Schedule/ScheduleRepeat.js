ScheduleRepeat = {}

$(function () {
    $(document).ready(function () {

        StartDate = parent.Startdate;

        // 저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            ScheduleRepeat.Preview();
            
            
            //txtRepeat
        });
        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            //if (opener.disablePopup) { opener.disablePopup(); } else { self.close(); }
            self.close();
        });
        // 미리보기
        $('#cbtnPreview').click(function (e) {
            e.preventDefault();
            ScheduleRepeat.Preview();
        });

        $('#txtcalendar').datepick($.datepicker.regional[PreLang]);

    });
});

/*************************
작성자: 김광일
작성일 2011-9-11
용도: 반복설정 저장
**************************/
ScheduleRepeat.Confirm = function () {

//    if ($("#ddlresult").val() == null) {
//        alert('미리보기 버튼을 클릭하세요');
//        return;
//    }
    ParamArray = {
        'CompanyID': CompanyID,
        'RepeatType': RepeatType,
        'SelectType': SelectType,
        'IntervalCnt': IntervalCnt,
        'Option1': Option1,
        'Option2': Option2,
        'Option3': Option3,
        'Option4': Option4,
        'Option5': Option5,
        'Option6': Option6,
        'Option7': Option7,
        'MaxReptCnt': MaxReptCnt,
        'EndDate': EndDate,
        'StartDate': StartDate
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    opener.RepeatValue = GetEncryptValue;

    opener.AddMent($("#hdMent").val());

    top.opener.RepeatHide();

    self.close();

}



/*************************
 작성자: 김광일
 작성일 2011-9-11
 용도: 반복설정 미리보기
**************************/
ScheduleRepeat.Preview = function () {
    RepeatType = "";
    SelectType = "";
    IntervalCnt = "";
    Option1 = "";
    Option2 = "";
    Option3 = "";
    Option4 = "";
    Option5 = "";
    Option6 = "";
    Option7 = "";
    MaxReptCnt = "";
    EndDate = "";

    var validator = new Validator($("#form1"));

    //반복범위 유효성
    if ($("#rbrepeat1").attr("checked")) { //날짜  

        if (jQuery.trim($("#txtcalendar").val()).length == 0) {
            alert(RepeatAlert1);
            return;
        }
    }
    if ($("#rbrepeat2").attr("checked")) { //횟수

        /*
        if (jQuery.trim($("#txtrepeat").val()).length == 0) {
            alert(RepeatAlert2);
            return;
        }
        */

        validator.checkRequired("txtrepeat", RepeatAlert3, true);
        validator.checkOnlyNumber("txtrepeat", RepeatAlert3, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtrepeat").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtrepeat").focus();
            return;
        }
    }


    if ($("#rbday").attr("checked")) { //일반복
        /*
        if (jQuery.trim($("#txtday").val()).length == 0) {
            alert(RepeatAlert3);
            return;
        }
        */

        // 매일 반복 (주기 입력)        
        validator.checkRequired("txtday", RepeatAlert3, true);
        validator.checkOnlyNumber("txtday", RepeatAlert3, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtday").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtday").focus();
            return;
        }

        RepeatType = "D";
        SelectType = "1";
        IntervalCnt = $("#txtday").val();
        if ($("#rbrepeat1").attr("checked")) { //날짜 리미트
            EndDate = $("#txtcalendar").val();
        }
        if ($("#rbrepeat2").attr("checked")) { //날짜 리미트
            MaxReptCnt = $("#txtrepeat").val();
        }
    }
    else if ($("#rbmail").attr("checked")) { //매일반복

        RepeatType = "D";
        SelectType = "2";
        IntervalCnt = "1";

        if ($("#rbrepeat1").attr("checked")) { //날짜 리미트
            EndDate = $("#txtcalendar").val();
        }
        if ($("#rbrepeat2").attr("checked")) { //횟수 리미트
            MaxReptCnt = $("#txtrepeat").val();
        }

        

    }
    else if ($("#rbweek").attr("checked")) { //주반복
        /*
        if (jQuery.trim($("#txtweek").val()).length == 0) {
            alert(RepeatAlert3);
            return;
        }
        */

        //매주반복
        validator.checkRequired("txtweek", RepeatAlert3, true);
        validator.checkOnlyNumber("txtweek", RepeatAlert3, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtweek").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtweek").focus();
            return;
        }


        RepeatType = "W";
        SelectType = "2";
        IntervalCnt = $("#txtweek").val();

        if ($("#rbrepeat1").attr("checked")) { //날짜 리미트
            EndDate = $("#txtcalendar").val();
        }
        if ($("#rbrepeat2").attr("checked")) { //날짜 리미트
            MaxReptCnt = $("#txtrepeat").val();
        }

        var chk = "0";

        if ($("#chksun").attr("checked")) {
            Option1 = "1";
            chk = "1";
        }
        if ($("#chkmon").attr("checked")) {
            Option2 = "1";
            chk = "1";
        }
        if ($("#chktue").attr("checked")) {
            Option3 = "1";
            chk = "1";
        }
        if ($("#chkwed").attr("checked")) {
            Option4 = "1";
            chk = "1";
        }
        if ($("#chkthu").attr("checked")) {
            Option5 = "1";
            chk = "1";
        }
        if ($("#chkfri").attr("checked")) {
            Option6 = "1";
            chk = "1";
        }
        if ($("#chksat").attr("checked")) {
            Option7 = "1";
            chk = "1";
        }


        if (chk == "0") {
            alert(RepeatAlert5);
            return;
        }

    }
    else if ($("#rbmonth").attr("checked")) { //달반복

        //매달반복 : 일자선택
        validator.checkRequired("txtmonth", RepeatAlert3, true);
        validator.checkOnlyNumber("txtmonth", RepeatAlert3, true);
        validator.checkRequired("txtmonthday", RepeatAlert3, true);
        validator.checkOnlyNumber("txtmonthday", RepeatAlert3, true);
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
            alert(RepeatAlert3);
            $("#txtmonthday").focus();
            return;
        }

        RepeatType = "M";
        IntervalCnt = $("#txtmonth").val();

        SelectType = "1";

        if ($("#rbrepeat1").attr("checked")) { //날짜 리미트
            EndDate = $("#txtcalendar").val();
        }
        if ($("#rbrepeat2").attr("checked")) { //날짜 리미트
            MaxReptCnt = $("#txtrepeat").val();
        }
        IntervalCnt = $("#txtmonth").val();
        Option1 = $("#txtmonthday").val();

    }
    else if ($("#rbmonth2").attr("checked")) { //달반복

        //매달반복 : 첫째.둘째..
        validator.checkRequired("txtmonth2", RepeatAlert3, true);
        validator.checkOnlyNumber("txtmonth2", RepeatAlert3, true);
        if (!validator.validate()) return;

        if (jQuery.trim($("#txtmonth2").val()) == "0") {
            alert(R_RepeatZero);
            $("#txtmonth2").focus();
            return;
        }

        RepeatType = "M";
        IntervalCnt = $("#txtmonth2").val();
        SelectType = "2";

        if ($("#rbrepeat1").attr("checked")) { //날짜 리미트
            EndDate = $("#txtcalendar").val();
        }
        if ($("#rbrepeat2").attr("checked")) { //날짜 리미트
            MaxReptCnt = $("#txtrepeat").val();
        }


        Option1 = $("#ddlorder").val();
        Option2 = $("#ddlkind").val();

    }

    ScheduleRepeat.Calcu();
    //return;
    ScheduleRepeat.CalcuList();

    //alert("");
    ScheduleRepeat.Confirm();
}

/*************************
작성자: 김광일
작성일 2011-9-11
용도: 반복설정 계산
**************************/
ScheduleRepeat.Calcu = function () {
    
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ScheduleRepeat.asmx/RepeatCalcu";

    ParamArray = {
        'CompanyID': CompanyID,
        'RepeatType': RepeatType,
        'SelectType': SelectType,
        'IntervalCnt': IntervalCnt,
        'Option1': Option1,
        'Option2': Option2,
        'Option3': Option3,
        'Option4': Option4,
        'Option5': Option5,
        'Option6': Option6,
        'Option7': Option7,
        'MaxReptCnt': MaxReptCnt,
        'EndDate': EndDate
        //'StartDate': StartDate
    };

    var obj = eval("ScheduleRepeat.SuccessCalcu");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
/*************************
작성자: 김광일
작성일 2011-9-11
용도: 반복설정 계산성공
**************************/
ScheduleRepeat.SuccessCalcu = function (data, status) {
    
    //alert(data.d);
    $("#hdMent").val(data.d);
}


/*************************
작성자: 김광일
작성일 2011-9-11
용도: 반복설정 리스트
**************************/
ScheduleRepeat.CalcuList = function () {
     
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ScheduleRepeat.asmx/RepeatCalcuList";

    ParamArray = {
        'CompanyID': CompanyID,
        'RepeatType': RepeatType,
        'SelectType': SelectType,
        'IntervalCnt': IntervalCnt,
        'Option1': Option1,
        'Option2': Option2,
        'Option3': Option3,
        'Option4': Option4,
        'Option5': Option5,
        'Option6': Option6,
        'Option7': Option7,
        'MaxReptCnt': MaxReptCnt,
        'EndDate': EndDate
        //'StartDate': StartDate
    };

    var obj = eval("ScheduleRepeat.SuccessCalcuList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
/*************************
작성자: 김광일
작성일 2011-9-11
용도: 반복설정 리스트성공
**************************/
ScheduleRepeat.SuccessCalcuList = function (data, status) {
    
    $('option', "#ddlresult").remove();
    //$('#ddlresult').attr('value', "aaaaa").text("fff").appendTo('#colors');
    //$('#ddlresult').append("<option value='foo' >Foo</option>");
    //return;
    index1 = 0;
//    $.each(data.d, function (index, list) {
//        index1 = index1 + 1;
//        $('#ddlresult').append("<option value='" + list.strStartDate + "' >" + index1+". " + list.strStartDate + "</option>");

//    });
}