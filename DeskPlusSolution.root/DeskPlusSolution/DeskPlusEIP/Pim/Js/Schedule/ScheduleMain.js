ScheduleMain = {}

$(function () {
    $(document).ready(function () {
        ParentCallInterFace();

        //공유대상
        $('#CbtnShareObj').click(function (e) {
            e.preventDefault();
            //ScheduleMain.CallPage("SHAREOBJ");
            ScheduleMain.GetUser();
        });


        //전체공유일정
        $('#CbtnAllShare').click(function (e) {
            e.preventDefault();

            ScheduleMain.CallPage("ALLSAHRE");
        });

        //기념일관리
        $('#CbtnAnniv').click(function (e) {
            e.preventDefault();

            ScheduleMain.CallPage("ANNIVER");

            //ScheduleMain.TabPageMove("week");
        });

        //사용자일정
        $('#CbtnUserSche').click(function (e) {
            e.preventDefault();

            ScheduleMain.CallPage("USER");
        });

        //관리자
        $('#CbtnAdmin').click(function (e) {
            e.preventDefault();
            parent.MovePage('/Deskpluseip/Pim/Admin/Default.aspx');
            //ScheduleMain.CallPage("ADMIN");
        });

        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {

                e.preventDefault();
                ScheduleMain.SearchList();
            }
        });

        //기본보기 설정
        $('#CbtnDefault').click(function (e) {
            e.preventDefault();
            ScheduleMain.CallPage("DEFAULT");
        });

        //인쇄
        $('#CbtnPrint').click(function (e) {
            e.preventDefault();
            var d1 = "";
            var sdate;

            //if ((parseInt(frmContent1.NowDate.getMonth()) + 1) < 10) {
            if ((parseInt(frmContent1.NowMonth) + 1) < 10) {
                //if (parseInt(frmContent1.NowDate.getDate()) < 10)
                if (parseInt(frmContent1.NowDay) < 10)
                //sdate = frmContent1.NowDate.getFullYear() + "-0" + (parseInt(frmContent1.NowDate.getMonth()) + 1) + '-0' + parseInt(frmContent1.NowDate.getDate());
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                //sdate = frmContent1.NowDate.getFullYear() + "-0" + (parseInt(frmContent1.NowDate.getMonth()) + 1) + '-' + parseInt(frmContent1.NowDate.getDate());
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }
            else {
                //if (parseInt(NowDate.getDate()) < 10)
                if (parseInt(frmContent1.NowDay) < 10)
                //sdate = frmContent1.NowDate.getFullYear() + "-" + (parseInt(frmContent1.NowDate.getMonth()) + 1) + '-0' + parseInt(frmContent1.NowDate.getDate());
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                //sdate = frmContent1.NowDate.getFullYear() + "-" + (parseInt(frmContent1.NowDate.getMonth()) + 1) + '-' + parseInt(frmContent1.NowDate.getDate());
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }

            d1 = sdate;

            ParamArray = {
                'Date': d1,
                'PageScope': PageScope,
                'Opentype': ScheduleCheck
            };

            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Pim/Schedule/PrintPage.aspx?p=" + GetEncryptValue;
            var param = '';
            WindowObjectReference = PopupWinOpen(url, param, "print", "700", "600")
        });

        //외부연동
        $("#cbtnLinkPim").click(function (e) {
            e.preventDefault();
            ScheduleMain.LinkPim();
        });

        //공유선택        
        $('#ddlSearchKind').change(function (e) {
            e.preventDefault();
            ScheduleMain.SearchComboSelect($('#ddlSearchKind').val());
        });

        //할일추가
        $('#cbtnToDoAdd').click(function (e) {
            e.preventDefault();

            var d1 = "";
            var sdate;


            if ((parseInt(frmContent1.NowMonth)) < 10) {
                if (parseInt(frmContent1.NowDay) < 10)
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }
            else {
                if (parseInt(frmContent1.NowDay) < 10)
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }


            d1 = sdate;

            ParamArray = {
                'ToDoID': '',
                'Date': d1,
                'PageScope': PageScope,
                'Opentype': ScheduleCheck
            };

            CallEncrypt(CallGetParam());

            location.href = "../ToDo/AddN.aspx?p=" + GetEncryptValue;
        });

        //일정추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();

            var d1 = "";
            var sdate;

            if ((parseInt(frmContent1.NowMonth)) < 10) {
                if (parseInt(frmContent1.NowDay) < 10)
                //sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth) + 1) + '-0' + parseInt(frmContent1.NowDay);
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                    sdate = frmContent1.NowYear + "-0" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }
            else {
                if (parseInt(frmContent1.NowDay) < 10)
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-0' + parseInt(frmContent1.NowDay);
                else
                    sdate = frmContent1.NowYear + "-" + (parseInt(frmContent1.NowMonth)) + '-' + parseInt(frmContent1.NowDay);
            }
            d1 = sdate;


            ParamArray = {
                'ScheduleID': '',
                'Date': d1,
                'PageScope': PageScope,
                'Opentype': ScheduleCheck
            };

            CallEncrypt(CallGetParam());
            location.href = "AddN.aspx?p=" + GetEncryptValue;
        });

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        //ScheduleMain.GetShareList();

        var Contentpage = "ListDay.aspx";

        if (PageScope == "ListDay.aspx") {
            Contentpage = "ListDay.aspx";
            $("#aDay").attr("class", "tab_on");
            $("#aWeek").attr("class", "");
            $("#aMonth").attr("class", "");
            //$("#frmContent1").height(900);

        }
        else if (PageScope == "ListWeek.aspx") {
            Contentpage = "ListWeek.aspx";
            $("#aDay").attr("class", "");
            $("#aWeek").attr("class", "tab_on");
            $("#aMonth").attr("class", "");
            //$("#frmContent1").height(600);

        }
        else if (PageScope == "ListMonth.aspx") {
            Contentpage = "ListMonth.aspx";
            $("#aDay").attr("class", "");
            $("#aWeek").attr("class", "");
            $("#aMonth").attr("class", "tab_on");
            // $("#frmContent1").height(930);

        }
        else if (PageScope == "") {
            Contentpage = "ListDay.aspx";

        }

        //$("#frmContent1").height($(window).height() - 100);

        if (MoveDatePage.length > 0) {
            $("#frmContent1").attr("src", Contentpage + "?p=" + MoveDatePage);
        }
        else {

            $("#frmContent1").attr("src", Contentpage);
        }

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

$(window).scroll(function () {
    PageScrollTop = $(document).scrollTop();
});


ScheduleMain.SearchComboSelect = function (arg) {
    if (arg == "U") {
        $("#spanDate").show();
    }
    else {
        $("#spanDate").hide();
    }

    $("#startDate").val("");
    $("#endDate").val("");
}


ScheduleMain.SearchList = function () {

    var Distance = "";  //일정 할일
    var inquryPerson = "";  //조회대상
    var DatePeriod = ""; //기간
    var SearchKind = ""; //검색 종류
    var SerachValue = "";  //검색값
    var _StartDate = "";  //시작일
    var _EndDate = "";  //종료일


    Distance = $("#ddlDistance").val();
    inquryPerson = $("#ddlPerson").val();
    DatePeriod = $("#ddlSearchKind").val();
    SearchKind = $("#ddlValue").val();
    SerachValue = $("#ctbSearch").val();
    _StartDate = $("#startDate").val();
    _EndDate = $("#endDate").val();

    if (DatePeriod == "U") {

        if ((_StartDate == "") || (_EndDate == "")) {
            alert(InputSearchDay);
            return;
        }

        var dtResponse = CompareDT(_StartDate, _EndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return;
        }
    }

    //alert(DatePeriod);
    //0,1,2,U
    ParamArray = {
        'Distance': Distance,
        'inquryPerson': inquryPerson,
        'DatePeriod': DatePeriod,
        'SearchKind': SearchKind,
        'SerachValue': SerachValue,
        'StartDate': _StartDate,
        'EndDate': _EndDate
    };
    CallEncrypt(CallGetParam());

    var movepage = "";
    movepage = "ListSearch.aspx?p=" + GetEncryptValue;
    document.location.href = movepage;

}



/**************************************************
작성자:김광일
작성일:2011-10-26
작성목적 : 일정 공개대상
***************************************************/
ScheduleMain.GetUser = function () {


    var url = "/DeskPlusEIP/Pim/Common/GetUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "GoogleCalen", "800", "500")
}



/******************************************
작성자:김광일
작성일:2011-10-26
용도:페이지 호출
****************************************/
ScheduleMain.CallPage = function (arg) {

    var movepage = "";
    switch (arg) {
        case "SHAREOBJ": //공유대상
            movepage = "";
            break;
        case "ALLSAHRE": //전체공유일정  popup
            movepage = "ListTotal.aspx";
            break;
        case "ANNIVER": //기념일관리 
            movepage = "../Anniversary/List.aspx";
            break;
        case "USER": //사용자일정
            if (MoveDatePage.length > 0) {
                movepage = "AllSchedule.aspx?P=" + MoveDatePage;
            }
            else {
                movepage = "AllSchedule.aspx";
            }
            break;
        case "ADMIN": //관리자
            movepage = "../Admin/Default.aspx";
            break;
        case "DEFAULT": //기본보기 설정 미개발
            movepage = "";
            alert("개발중");
            break;

    }

    if (movepage.length > 0)
        document.location.href = movepage;

    //$("#frmContent1").attr("src", movepage);
}


/******************************************
작성자:김광일
작성일:2011-10-26
용도: 탭 페이지 호출
****************************************/
ScheduleMain.TabPageMove = function (arg) {

    parent.frmLeft.ChildCommunication(arg);
    //PageScope = arg;
    switch (arg) {
        case "day":
            PageScope = "ListDay.aspx";
            $("#aWeek").attr("class", "");
            $("#aMonth").attr("class", "");
            $("#aDay").attr("class", "tab_on");

            if (MoveDatePage.length == 0) {
                $("#frmContent1").attr("src", "Listday.aspx");
            }
            else {
                $("#frmContent1").attr("src", "Listday.aspx?p=" + MoveDatePage);
            }
            ScheduleMain.ModTabMenu("day");
            break;
        case "week":
            PageScope = "ListWeek.aspx";
            $("#aWeek").attr("class", "tab_on");
            $("#aDay").attr("class", "");
            $("#aMonth").attr("class", "");

            if (MoveDatePage.length == 0) {
                $("#frmContent1").attr("src", "ListWeek.aspx");
            }
            else {
                $("#frmContent1").attr("src", "ListWeek.aspx?p=" + MoveDatePage);
            }
            ScheduleMain.ModTabMenu("week");
            break;
        case "month":
            PageScope = "ListMonth.aspx";
            $("#aMonth").attr("class", "tab_on");
            $("#aDay").attr("class", "");
            $("#aWeek").attr("class", "");
            if (MoveDatePage.length == 0) {
                $("#frmContent1").attr("src", "ListMonth.aspx");
            }
            else {
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + MoveDatePage);
            }
            ScheduleMain.ModTabMenu("month");
            break;
    }
}


/******************************************
작성자:maniaysw
작성일:2017-01-20
용도: 마지막으로 클릭한 탭을 DB에 저장
****************************************/
ScheduleMain.ModTabMenu = function (_type) {
  
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ScheduleMain.asmx/ModTabMenu";
    ParamArray = {
        'companyID': CompanyID,
        'userId': UserID,
        'tabType': _type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ScheduleMain.SuccessTabMenu");

    AjaxService(url, data, obj);
}

ScheduleMain.SuccessTabMenu = function (data, status) {
    // alert(data.d); 성공시 true
}




/******************************************
작성자:김광일
작성일:2011-10-26
용도: 나에게 일정을 공유해준 유저 콤보박스 세팅
****************************************/
ScheduleMain.GetShareList = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ScheduleMain.asmx/GetOpenUserComboList";
    ParamArray = {
        'companyID': CompanyID,
        'UserId': UserID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ScheduleMain.SuccessShareList");

    AjaxService(url, data, obj);
}

ScheduleMain.SuccessShareList = function (data, status) {


    $("select[id='ddlPerson']").find('option').each(function () {
        $(this).remove();

    });

    $("#ddlPerson").get(0).options[0] = new Option(myperson, "P");
    //$("#ddlPerson").get(0).options[1] = new Option(AllSchedule, "M");

    $.each(data.d, function (index, list) {

        $("#ddlPerson").get(0).options[index + 1] = new Option(list.Name, list.UserID);
    });
}



/******************************************
작성자:
작성일:2012-02-09
용도: 나에게 일정을 공유해준 유저 콤보박스 세팅
****************************************/
ScheduleMain.LinkPim = function () {
    //var url = "/DeskPlusEIP/Pim/Schedule/LinkPim.aspx";
    var url = "/DeskPlusEIP/Pim/Schedule/SyncCalendar/Default.aspx";
    var param = "";
    //WindowObjectReference = PopupWinOpen(url, param, "GoogleCalen", "430", "205")
    WindowObjectReference = PopupWinOpen(url, param, "GoogleCalen", "500", "300")
}
 