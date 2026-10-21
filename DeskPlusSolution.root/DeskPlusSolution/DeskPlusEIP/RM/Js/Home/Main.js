Main = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //나의 예약
        $('#mymore').click(function (e) {

            ParamArray = {
                accActive: 1
            };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/RM/Folder/Tree.aspx?p=" + GetEncryptValue;

            parent.frmLeft.location = url;

        });

        //나의 예약 이전목록
        $('#myL').click(function (e) {
            e.preventDefault();
            MainPageNo = parseInt(MainPageNo) - 1;
            Main.GetGoMainList();
        });

        //나의 예약 다음목록
        $('#myR').click(function (e) {
            e.preventDefault();
            MainPageNo = parseInt(MainPageNo) + 1;
            Main.GetGoMainList();
        });

        //부서 예약 이전목록
        $('#deptL').click(function (e) {
            e.preventDefault();
            DeptPageNo = parseInt(DeptPageNo) - 1;
            Main.GetGoDeptList();
        });

        //부서 예약 다음목록
        $('#deptR').click(function (e) {
            e.preventDefault();
            DeptPageNo = parseInt(DeptPageNo) + 1;
            Main.GetGoDeptList();
        });

        //예약 가능 확인
        $('#btnAdd').click(function (e) {
            e.preventDefault();
            Main.UseResource();
        });

        __addEventListener(document, "click", Main.click);

        $('#homedate').html(CalendarDate);

        Main.GetMainList();
        Main.GetDeptList();
        Main.SetTab(ListGubun);

        // 대기자원 카운트 메뉴트리에 보이게 처리
        if (CntYN == "Y") {
            if (RmCnt == 0) {
                if (parent.frmLeft.mCnt) {
                    parent.frmLeft.mCnt.innerText = "";
                    parent.frmLeft.mSubCnt.innerText = "";
                }
            }
            else {
                parent.frmLeft.mCnt.innerText = "(" + RmCnt + ")";
                parent.frmLeft.mSubCnt.innerText = "(" + RmCnt + ")";
            }
        }
    });
});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetMainList = function () {

    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/GetMainList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 5,
        'pageSizePageNo': MainPageNo,
        'listGubun': ListGubun
    };

    var obj = eval("Main.SuccessMainList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessMainList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/RM/Jt/Home/JTMainList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    Main.MainResource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.MainResource = function () {

    $("#r_musable")[0].innerHTML = R_UsableStr;
    $("#r_mresourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#r_mreserveDate")[0].innerHTML = R_ReserveDate;
    $("#r_msubject")[0].innerHTML = R_Subject;
    $("#r_mattach")[0].innerHTML = R_HomeAttach;
    $("#r_mwriteuser")[0].innerHTML = R_Subscriber;

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }

    if ($("#MNoListData")[0] != undefined) {
        $("#MNoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 부서 내역 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetDeptList = function () {

    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/GetDeptList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 5,
        'pageSizePageNo': DeptPageNo
    };

    var obj = eval("Main.SuccessDeptList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 부서 내역 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessDeptList = function (data, status) {

    $('#phDeptList').setTemplateURL('/DeskPlusEIP/RM/Jt/Home/JTDeptList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phDeptList').processTemplate(data.d);

    Main.DeptResource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.DeptResource = function () {

    $("#r_dusable")[0].innerHTML = R_UsableStr;
    $("#r_dresourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#r_dreserveDate")[0].innerHTML = R_ReserveDate;
    $("#r_dsubject")[0].innerHTML = R_Subject;
    $("#r_dattach")[0].innerHTML = R_HomeAttach;
    $("#r_dwriteuser")[0].innerHTML = R_Subscriber;

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }

    if ($("#DNoListData")[0] != undefined) {
        $("#DNoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 자원 보기
**********************************************************************************************/
Main.View = function (_reservationID) {
    ParamArray = {
        'tagGubun': 'main',
        'reservationID': _reservationID,
        'deptPageNo': DeptPageNo,
        'mainPageNo': MainPageNo
    };

    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/RM/Content/View.aspx?p=" + GetEncryptValue;
}

/******************************************
작성자: famo23
작성일자: 2016-03-16
작성목적: 자원 예약
****************************************/
Main.Add = function (_listID) {

    ParamArray = {
        'listID': _listID,
        'tagGubun': 'main'
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/RM/Content/AddN.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 달력 선택
***************************************************/
Main.OnCalendarChange = function (cal) {

    var selectedYear = cal.FormatDate(CalendarSingle.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(CalendarSingle.GetSelectedDate(), "MM");
    var selectedDay = cal.FormatDate(CalendarSingle.GetSelectedDate(), "dd");

    CalendarDate = selectedYear + '-' + selectedMonth + '-' + selectedDay;

    $('#homedate').html(CalendarDate);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 페이지이동 처리
**********************************************************************************************/
Main.GoPageLoad = function () {
    
    ParamArray = {
        'deptPageNo': DeptPageNo,
        'mainPageNo': MainPageNo
    };

    CallEncrypt(CallGetParam());
    location = "Main.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 나의 예약 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoMainList = function () {

    if (MainPageNo < 0) {
        alert(R_FirstList);
        MainPageNo = 0;
        return;
    }
    
    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/GetMainList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 5,
        'pageSizePageNo': MainPageNo,
        'listGubun': ListGubun
    };

    var obj = eval("Main.SuccessGoMainList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 나의 예약 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoMainList = function (data, status) {

    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (MainPageNo > 0) {
            alert(R_LastList);
            MainPageNo = parseInt(MainPageNo) - 1;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 부서 내역 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoDeptList = function () {

    if (DeptPageNo < 0) {
        alert(R_FirstList);
        DeptPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/GetDeptList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 5,
        'pageSizePageNo': DeptPageNo
    };

    var obj = eval("Main.SuccessGoDeptList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-16
작성목적 : 부서 내역 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoDeptList = function (data, status) {

    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (DeptPageNo > 0) {
            alert(R_LastList);
            DeptPageNo = parseInt(DeptPageNo) - 1;
        }
    }
}


/***********************************************************************
작성자   : famo23
작성일자 : 2016-03-18
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 호출)
************************************************************************/
Main.UseResource = function () {

    if ($("#ddlList").val() != null) {
        var arrList = $("#ddlList").val().split("_");

        //시간 확인
        var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
        var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

        if (eTime <= sTime) {
            alert(R_StartDateEndDiffer);
            return;
        }

        ParamArray = {
            'listID': arrList[0],
            'usableQty': arrList[1],
            'startDate': CalendarDate,
            'endDate': CalendarDate,
            'sTime': $("#ddlstartTime").val(),
            'eTime': $("#ddlendTime").val()
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Main.SuccessUseResource");
        var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/UseResource";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_HomeNoReserve);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-18
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessUseResource = function (data, status) {
    if (data.d) {
        if (confirm(R_ConfirmAdd)) {
            var arrList = $("#ddlList").val().split("_");

            ParamArray = {
                'tmpDate': CalendarDate,
                'tmpStartTime': $("#ddlstartTime").val(),
                'tmpEndTime': $("#ddlendTime").val(),
                'listID': arrList[0],
                'tagGubun': 'main'
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusEIP/RM/Content/AddN.aspx?p=" + GetEncryptValue;
        }
    }
    else {
        alert(R_UsableResource);
        return;
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 클릭 이벤트 분기
***************************************************/
Main.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "attach": Main.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 정보가져오기
***************************************************/
Main.GetAttachFileListMenu = function (p_reservationID) {
    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/GetAttachFileListMenu";
    var data = "reservationID=" + p_reservationID;
    var obj = eval("Main.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Main.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Main.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    Main.GetAttachFileListMenu(listID);

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        /*
        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Main.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }
        */

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            oItem = oContextMenuCtrl.addItem(attachFileName, Main.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 열기
***************************************************/
Main.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 전체 다운로드
***************************************************/
Main.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        PopupWinOpen("", "", "openWin", "600", "400")
        document.getElementById("frmTotalDownLoad").submit();
    }
}


/*** TAB start *********************************************************************************/

// TAB
var selList = "";
Main.SetTab = function (_listGubun) {   //alert("SetTab:" + _module);

    if (_listGubun == "") return;
    selList = _listGubun;

    $(".tab_menu li").removeClass("selected");
    $("#sp_" + _listGubun).parent().parent().addClass("selected");    

    //$(".sel_list").css("font-weight", "");
    //$("#sp_" + _listGubun).css("font-weight", "bold");    
}

// TAB - 클릭시
Main.GoTab = function (_listGubun) {

    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/AddRMHomeMenu";

    ParamArray = { 'listGubun': _listGubun };
    var obj = eval("Main.GoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)

}

Main.GoList = function (data, status) {

    if (data.d) {
        MainPageNo = 0;
        Main.GoPageLoad();
    }
}

/*** TAB end *********************************************************************************/