AllSchedule = {}



/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    $(document).ready(function () {


        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();

                $("#hdShareList").val($("#hdShareTemp").val());
                AllSchedule.LoadData();
            }
        });

        if (SelectDateS.length > 0) {

            StartDate = SelectDateS;
            EndDate = SelectDateE;
        }


        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);



        //조회대상선택
        $('#CbtnTarget').click(function (e) {
            e.preventDefault();
            AllSchedule.CallSearchObject();
            //
        });
        //엑셀로보기
        $('#CbtnExcel').click(function (e) {
            e.preventDefault();
            AllSchedule.ExcelDown();
        });
        //인쇄
        $('#CbtnPrin').click(function (e) {
            e.preventDefault();

        });

        //검색
        $('#btnsearch').click(function (e) {
            e.preventDefault();
            $("#hdShareList").val($("#hdShareTemp").val());
            AllSchedule.LoadData();
        });

        ///나의일정
        $('#CbtnMySchedule').click(function (e) {
            e.preventDefault();
            document.location.href = "ScheduleMain.aspx";
        });


        //공유선택        
        $('#ddlSearchKind').change(function (e) {
            e.preventDefault();
            AllSchedule.SearchComboSelect($('#ddlSearchKind').val());


        });


        //검색데이타

        if (StartDate.length > 0) {

            if (StartDate == "0")
                $("#ddlSearchKind").val(StartDate);
            else if (StartDate == "WEEK")
                $("#ddlSearchKind").val(StartDate);
            else if (StartDate == "MON")
                $("#ddlSearchKind").val(StartDate);
            else {
                $("#ddlSearchKind").val('U');
                $("#startDate").val(StartDate);
                $("#endDate").val(StartDate);

                $("#spanDate").show();

            }

            if (title.length > 0) {
                $("#ddlkey").val('Title');
                $("#ctbSearch").val(title);
            }
            if (name.length > 0) {
                $("#ddlkey").val('Name');
                $("#ctbSearch").val(name);
            }

        }


        AllSchedule.LoadData();


        if (_Searchkey.length > 0) {

            OpenType = _Searchkey;
            $("#hdShareList").val(_SearchValue);
            var arrayid = _SearchValue.split(',');

            var strHtml = "";

            for (var i = 0; i < arrayid.length - 1; i++) {
                var arrayName = arrayid[i].split('$');

                if (arrayName[0].length > 0) {
                    strHtml += arrayName[2] + "&nbsp;<a  onclick='javascript:AllSchedule.ShareUserClick(this)' value='" + arrayid[i] + "' ><font color='red'>삭제</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }

            $("#divShare").html(strHtml);
            AllSchedule.LoadData();
        }

        // 도움말 오픈
        $('#HelpCategory').click(function (e) {
            e.preventDefault();

            var url = "../Help/GetCategoryInfo.aspx";
            PopupWin(300, 350, url, "-320", "0", false);
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


/*********************
작성자   : 김광일
작성일자 :  2011-9-21
작성목적 : 기간선택
*******************************/
AllSchedule.SearchComboSelect = function (arg) {
    if (arg == "U") {
        $("#spanDate").show();
    }
    else {
        $("#spanDate").hide();
    }

    $("#startDate").val("");
    $("#endDate").val("");
}



/*********************
작성자   : 김광일
작성일자 :  2011-9-21
작성목적 : 조회대상선택
*******************************/
AllSchedule.CallSearchObject = function () {

    var url = "/DeskPlusEIP/Pim/Common/MemberShareUser.aspx";

    PopupWin(800, 500, url, "50", "", true);
    //    PopupWin(600, 800, url, "-100", "100", false);
    //    $("#zeoModalContact").draggable({ disabled: false });
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 검색전 세팅
작성목적 : 
**********************************************************************************************/
AllSchedule.LoadData = function () {

    StartDate = '';
    EndDate = '';
    //CurrentPage = '1';

    UserList = $("#hdShareList").val();
    PageSize = $('#selPageDistance').val();
    RecordPerPage = PageSize;



    if ($('#ddlSearchKind').val() == "U") {
        StartDate = $("#startDate").val();
        EndDate = $("#endDate").val();

        if ((StartDate == "") || (EndDate == "")) {
            alert(InputSearchDay);
            return;
        }

        var dtResponse = CompareDT(StartDate, EndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return;
        }
    }
    else {
        StartDate = $('#ddlSearchKind').val();
    }
    title = "";
    name = "";

    if ($('#ddlkey').val() == "Title") {

        title = $('#ctbSearch').val();
    }

    if ($('#ddlkey').val() == "Name") {

        name = $('#ctbSearch').val();
    }


    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {

        AllSchedule.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {

        //AllSchedule.GetPageListCnt(); //검색 총 카운트 가져오기
        AllSchedule.GetPageList(); //검색 리스트 가져오기
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {

        loadBoardSearchData();
    }, 100);
}


/**********************************************************************************************
작성자   : DANIEL  
작성일자 : 2011년 8월 16일
작성목적 : 리스트 가져오기
**********************************************************************************************/
AllSchedule.GetPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/AllSchedule.asmx/GetAllUserScheduleList";


    ParamArray = {
        'UserID': UserID,
        'CompanyID': CompanyID,
        'OpenType': OpenType,
        'UserList': UserList,
        'StartDate': StartDate,
        'EndDate': EndDate,
        'title': title.replace(/&/g, '＆'),
        'name': name
    };


    var obj = eval("AllSchedule.SuccessUserList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}



AllSchedule.SuccessUserList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Schedule/JTAllchedule.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    //alert(data.d);
    AllSchedule.Resource();
}

AllSchedule.Resource = function () {
    $("#r_name")[0].innerHTML = col1;
    $("#r_kind")[0].innerHTML = col2;
    $("#r_date")[0].innerHTML = col3;
    $("#Th1")[0].innerHTML = col4;
    $("#r_title")[0].innerHTML = "" + col5 + "";
    $("#divt1").text(TextEmpty);

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
}



/*********************
작성자:김광일
작성일:2011년 9월 19일
용도: 공유리스트 관리
**********************/
AllSchedule.ShareUserClick = function (obj) {

    var strtemp = $("#hdShareList").val().replace(obj.value + ",", '');

    var arrayid = strtemp.split(',');

    var strHtml = "";

    for (var i = 0; i < arrayid.length - 1; i++) {
        var arrayName = arrayid[i].split('$');

        if (arrayName[0].length > 0) {
            strHtml += arrayName[2] + "&nbsp;<a  onclick='javascript:AllSchedule.ShareUserClick(this)' value='" + arrayid[i] + "' ><font color='red'>삭제</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
    }
    $("#hdShareList").val(strtemp);

    $("#divShare").html(strHtml);

}


//페이지이동
AllSchedule.MovePage = function (arg, kind) {



    var url = "";
    if (kind == 8) {
        url = "/DeskPlusEIP/Pim/Todo/View.aspx?p=";
        ParamArray = {
            'UserList': UserList,
            'StartDate': StartDate,
            'EndDate': EndDate,
            'title': title.replace(/&/g, '＆'),
            'name': name,
            'Page': 'AllSchedule.aspx',
            'TodoID': arg
        };
    }
    else {
        url = "/DeskPlusEIP/Pim/Schedule/View.aspx?p=";
        ParamArray = {
            //'s_Distance': $("#divShare").html(),
            'UserList': UserList,
            'StartDate': StartDate,
            'EndDate': EndDate,
            'title': title.replace(/&/g, '＆'),
            'name': name,
            'Page': 'AllSchedule.aspx',
            'DisplayID': arg
        };
    }
    CallEncrypt(CallGetParam());
    document.location.href = url + GetEncryptValue;
}

/******
작성자:김광일
용도:  조회 유저 삭제
작성일:2011-11-25

********/
AllSchedule.UserBlind = function (uid) {
    //alert("#div" + uid);

    //return;
    //$("#tr" + uid + "1").css("display", "none");
    //$("#tr" + uid + "2").css("display", "none");
    //$("#tr" + uid + "3").css("display", "none");
    //$("#div" + uid ).css("display", "none");

    var tuser = $("#hdShareList").val();

    var aruser = tuser.split(',');
    var tuserlist = '';

    for (i = 0; i < aruser.length; i++) {
        if (aruser[i].length > 0) {
            if (uid != aruser[i]) {
                tuserlist = tuserlist + aruser[i] + ',';
            }
        }
    }

    UserList = tuserlist;
    $("#hdShareList").val(tuserlist);
    AllSchedule.GetPageList();

}





/**********************************************************************************************
작성자   : daniel  
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
AllSchedule.ExcelDown = function () {
    ParamArray = {
        'UserID': UserID,
        'CompanyID': CompanyID,
        'OpenType': OpenType,
        'UserList': UserList,
        'StartDate': StartDate,
        'EndDate': EndDate,
        'title': title.replace(/&/g, '＆'),
        'name': name
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 할일 댓글 리스트 팝업
**********************************************************************************************/
AllSchedule.PopupTodoShort = function (p_toDoID) {
    ParamArray = {
        'toDoID': p_toDoID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Todo/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

    //var url = "/DeskPlusEIP/Pim/Todo/ShortReplyList.aspx?p=" + GetEncryptValue;
    //PopupWin(800, 500, url, "", "", true);
}

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 일정 댓글 리스트 팝업
**********************************************************************************************/
AllSchedule.PopupScheduleShort = function (p_scheduleID, p_displayID) {
    ParamArray = {
        'scheduleID': p_scheduleID,
        'displayID': p_displayID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Schedule/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

    //var url = "/DeskPlusEIP/Pim/Schedule/ShortReplyList.aspx?p=" + GetEncryptValue;
    //PopupWin(800, 500, url, "", "", true);
}

/**********************************************************************************************
작성자   : famo23  
작성일자 : 2013-06-19
작성목적 : 사용자 마이톡
**********************************************************************************************/
AllSchedule.PopupShortReplyList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/SNS/Common/MyTalkGroup.aspx?p=" + GetEncryptValue;
    PopupWin(700, 520, url, "", "", true);
}