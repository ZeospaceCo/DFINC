Main = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) { e.preventDefault(); Main.Search(); });
        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Main.Search(); } });


        //사용자 화면 탭이동
        $('#tabPage').click(function (e) {
            e.preventDefault();
            Main.SaveEnv();
        });

        //최신업데이트 업무 이전목록
        $('#recentL').click(function (e) {
            e.preventDefault();
            RecentPageNo = parseInt(RecentPageNo) - 1;
            Main.GetGoRecentList();
        });

        //최신업데이트 업무 다음목록
        $('#recentR').click(function (e) {
            e.preventDefault();
            RecentPageNo = parseInt(RecentPageNo) + 1;
            Main.GetGoRecentList();
        });

        //진행중인 업무리스트 이전목록
        $('#processL').click(function (e) {
            e.preventDefault();
            ProcessPageNo = parseInt(ProcessPageNo) - 1;
            Main.GetGoProcessList();
        });

        //진행중인 업무리스트 다음목록
        $('#processR').click(function (e) {
            e.preventDefault();
            ProcessPageNo = parseInt(ProcessPageNo) + 1;
            Main.GetGoProcessList();
        });

        //지연된 업무리스트 이전목록
        $('#delayL').click(function (e) {
            e.preventDefault();
            DelayPageNo = parseInt(DelayPageNo) - 1;
            Main.GetGoDelayList();
        });

        //지연된 업무리스트 다음목록
        $('#delayR').click(function (e) {
            e.preventDefault();
            DelayPageNo = parseInt(DelayPageNo) + 1;
            Main.GetGoDelayList();
        });

        //완료된 업무리스트 이전목록
        $('#comL').click(function (e) {
            e.preventDefault();
            ComPageNo = parseInt(ComPageNo) - 1;
            Main.GetGoComList();
        });

        //완료된 업무리스트 다음목록
        $('#comR').click(function (e) {
            e.preventDefault();
            ComPageNo = parseInt(ComPageNo) + 1;
            Main.GetGoComList();
        });

        //중요표시 업무리스트 이전목록
        $('#impoL').click(function (e) {
            e.preventDefault();
            ImpoPageNo = parseInt(ImpoPageNo) - 1;
            Main.GetGoImpoList();
        });

        //중요표시 업무리스트 다음목록
        $('#impoR').click(function (e) {
            e.preventDefault();
            ImpoPageNo = parseInt(ImpoPageNo) + 1;
            Main.GetGoImpoList();
        });

        // 기간
        $("#ddlYear").change(function (e) {
            e.preventDefault();
            Main.ChangeYear();
        });

        $("#spTot").html(CntTot);
        $("#spI").html(CntI);
        $("#spD").html(CntD);
        $("#spC").html(CntC);

        //검색어입력
        $("#searchValue").attr("placeholder", R_PlaceholderMenu);

        Main.Init();

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-04
작성목적 : 초기화 하기
**********************************************************************************************/
Main.Init = function () {
    Main.GetRecentList();
    Main.GetProcessList();
    Main.GetDelayList();
    Main.GetComList();    
    //Main.GetImpoList();
    Main.ReLoadTree();
}

/**************************************************
작성자   : famo23
작성일자 : 2016-01-04
작성목적 : 좌측 메뉴 reload
***************************************************/
Main.ReLoadTree = function () {

    if (ReLoad == "1") {
        ParamArray = {
            'cabinetID': "root",
            'reLoad': ReLoad
        };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
        parent.frmLeft.location = url;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 리스트 이동
**********************************************************************************************/
Main.ChangeYear = function () {

    var year = $("#ddlYear").val();

    ParamArray = {
        'year': year
    };
    CallEncrypt(CallGetParam());

    location = "AdminMain.aspx?p=" + GetEncryptValue;

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetRecentList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetRecentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': RecentPageNo,
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessRecentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트  업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessRecentList = function (data, status) {

    $('#phRecentList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTAdminRecent.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phRecentList').processTemplate(data.d);

    setTimeout("Main.RecentResource();", 200);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.RecentResource = function () {

    if ($("#r_rtitle")[0] == undefined) {
        setTimeout("Main.RecentResource();", 200);
    }
    else {
        $("#r_rtitle")[0].innerHTML = R_Title;
        $("#r_rstatus")[0].innerHTML = R_State;
        $("#r_rrequestdept")[0].innerHTML = R_RequestDept;
        $("#r_rrequester")[0].innerHTML = R_Requester;
        $("#r_rrequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_renddate")[0].innerHTML = R_EndDate;
        $("#r_rtodept")[0].innerHTML = R_ToDeptName;
        $("#r_rtoname")[0].innerHTML = R_Receiver;

        if ($("#R_NoListData")[0] != undefined) {
            $("#R_NoListData")[0].innerHTML = R_ProgressNoContent;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetProcessList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ProcessPageNo,
        'status': '2,3',
        'order': 'List.ListID desc',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessProcessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessProcessList = function (data, status) {

    $('#phProcessList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTAdminProgress.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phProcessList').processTemplate(data.d);

    setTimeout("Main.ProcessResource();", 200);   
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.ProcessResource = function () {

    if ($("#r_ptitle")[0] == undefined) {
        setTimeout("Main.ProcessResource();", 200);
    }
    else {
        $("#r_ptitle")[0].innerHTML = R_Title;
        $("#r_prequestdept")[0].innerHTML = R_RequestDept;
        $("#r_prequester")[0].innerHTML = R_Requester;
        $("#r_prequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_penddate")[0].innerHTML = R_EndDate;
        $("#r_ptodept")[0].innerHTML = R_ToDeptName;
        $("#r_ptoname")[0].innerHTML = R_Receiver;

        if ($("#P_NoListData")[0] != undefined) {
            $("#P_NoListData")[0].innerHTML = R_ProgressNoContent;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 지연된 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetDelayList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': DelayPageNo,
        'status': '6',
        'order': 'ExcessDay , List.ListID desc ',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessDelayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 지연된 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessDelayList = function (data, status) {

    $('#phDelayList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTAdminDelay.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phDelayList').processTemplate(data.d);

    setTimeout("Main.DelayResource();", 200);   
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-23
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.DelayResource = function () {

    if ($("#r_dtitle")[0] == undefined) {
        setTimeout("Main.DelayResource();", 200);
    }
    else {
        $("#r_dtitle")[0].innerHTML = R_Title;
        $("#r_drequestdept")[0].innerHTML = R_RequestDept;
        $("#r_drequester")[0].innerHTML = R_Requester;
        $("#r_drequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_denddate")[0].innerHTML = R_EndDate;
        $("#r_dtodept")[0].innerHTML = R_ToDeptName;
        $("#r_dtoname")[0].innerHTML = R_Receiver;

        if ($("#D_NoListData")[0] != undefined) {
            $("#D_NoListData")[0].innerHTML = R_DelayNoContent;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-07-05
작성목적 : 완료된 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetComList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ComPageNo,
        'status': '4',
        'order': 'CompleteDate desc, EndDate desc, List.ListID desc ',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessComList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-07-05
작성목적 : 완료된 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessComList = function (data, status) {

    $('#phComList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTAdminCom.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phComList').processTemplate(data.d);

    setTimeout("Main.ComResource();", 200);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-07-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.ComResource = function () {

    if ($("#r_ctitle")[0] == undefined) {
        setTimeout("Main.ComResource();", 200);
    }
    else {
        $("#r_ctitle")[0].innerHTML = R_Title;
        $("#r_crequestdept")[0].innerHTML = R_RequestDept;
        $("#r_crequester")[0].innerHTML = R_Requester;
        $("#r_crequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_cenddate")[0].innerHTML = R_End;
        $("#r_ctodept")[0].innerHTML = R_ToDeptName;
        $("#r_ctoname")[0].innerHTML = R_Receiver;

        if ($("#C_NoListData")[0] != undefined) {
            $("#C_NoListData")[0].innerHTML = R_ComNoContent;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 중요표시 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetImpoList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetImpoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ImpoPageNo,
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessImpoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 중요표시 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessImpoList = function (data, status) {

    $('#phImpoList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTAdminImpo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phImpoList').processTemplate(data.d);

    setTimeout("Main.ImpoResource();", 200);    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.ImpoResource = function () {

    if ($("#r_ititle")[0] == undefined) {
        setTimeout("Main.ImpoResource();", 200);
    }
    else {
        $("#r_ititle")[0].innerHTML = R_Title;
        $("#r_irequestdept")[0].innerHTML = R_RequestDept;
        $("#r_irequester")[0].innerHTML = R_Requester;
        $("#r_irequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_ienddate")[0].innerHTML = R_EndDate;
        $("#r_itodept")[0].innerHTML = R_ToDeptName;
        $("#r_itoname")[0].innerHTML = R_Receiver;

        if ($("#I_NoListData")[0] != undefined) {
            $("#I_NoListData")[0].innerHTML = R_ImpoNoContent;
        }
    }    
}


/******************************************
작성자: freeman
작성일자: 2015-12-30
작성목적: 검색
****************************************/
Main.Search = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("searchValue", R_InputSearch, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {
        'searchValue': searchVal
    };
    CallEncrypt(CallGetParam());

    location = "Search.aspx?p=" + GetEncryptValue;

    /*
    ParamArray = {
        'moduleName': 'TASK',
        'searchValue': searchVal
    };
    CallEncrypt(CallGetParam());

    //DB 검색
    //var url = "/DeskPlusEIP/EIP/Search/Default.aspx";
    //검색엔진
    var url = "/DeskPlusEIP/Search/Default.aspx";

    parent.parent.frmMain.location.href = url + "?p=" + GetEncryptValue;
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-31
작성목적 : 보기 이동
**********************************************************************************************/
Main.View = function (p_listID, p_groupID, p_viewcnt) {

    if (p_viewcnt == 0) {
        ReLoad = "1";
    }
    else {
        ReLoad = "";
    }

    ParamArray = {
        'listID': p_listID,
        'groupID': p_groupID,
        'reLoad': ReLoad,
        'isPage': "AdminMain",
        'recentPageNo': RecentPageNo,
        'processPageNo': ProcessPageNo,
        'delayPageNo': DelayPageNo,
        'impoPageNo': ImpoPageNo,
        'comPageNo': ComPageNo,
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());

    location = "ViewN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-12
작성목적 : 탭환경저장 (웹서비스 호출)
**********************************************************************************************/
Main.SaveEnv = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/ModEnv";

    //파라미터값 배열에 담기
    ParamArray = {
        'isPage': "Main"
    };

    var obj = eval("Main.SuccessSaveEnv");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-12
작성목적 : 탭환경저장 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessSaveEnv = function (data, status) {

    if (data.d) {
        parent.frmLeft.DefaultTab = "Main";
        location.href = "/DeskPlusEIP/Task/Home/Main.aspx";
    }
    else {
        alert(R_ErrorProcessing);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-12
작성목적 : 리스트 이동
**********************************************************************************************/
Main.GoList = function (p_itemID) {

    var tmpDate = $("#ddlYear").val() + "-01-01," + $("#ddlYear").val() + "-12-31";
    var tmpField = "RequestDate";

    if ($("#ddlYear").val() == "") {
        tmpDate = "";
        tmpField = "";
    }

    ParamArray = {
        'cabinetID': "Total",
        'itemID': p_itemID,
        'searchField': tmpField,
        'searchDateValue': tmpDate
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
    parent.frmLeft.location = url;

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-12
작성목적 : 리스트 이동
**********************************************************************************************/
Main.GoCntList = function (p_itemID, p_cnt, p_status) {

    if (p_cnt == "0") return;

    var tmpDate = $("#ddlYear").val() + "-01-01," + $("#ddlYear").val() + "-12-31";
    var tmpField = "RequestDate";

    if ($("#ddlYear").val() == "") {
        tmpDate = "";
        tmpField = "";
    }

    ParamArray = {
        'cabinetID': "Total",
        'itemID': p_itemID,
        'hStatus': p_status,
        'searchField': tmpField,
        'searchDateValue': tmpDate
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
    parent.frmLeft.location = url;

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-12
작성목적 : 리스트 이동
**********************************************************************************************/
Main.GoTotalList = function (p_status) {

    if ((p_status == "2,3,4,5,6") && (CntTot == "0")) return;
    if ((p_status == "2,3") && (CntI == "0")) return;
    if ((p_status == "6") && (CntD == "0")) return;
    if ((p_status == "4,5") && (CntC == "0")) return;

    var tmpDate = $("#ddlYear").val() + "-01-01," + $("#ddlYear").val() + "-12-31";
    var tmpField = "RequestDate";

    if ($("#ddlYear").val() == "") {
        tmpDate = "";
        tmpField = "";
    }

    ParamArray = {
        'cabinetID': "Total",
        'hStatus': p_status,
        'searchField': tmpField,
        'searchDateValue': tmpDate
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;
    parent.frmLeft.location = url;

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 페이지이동 처리
**********************************************************************************************/
Main.GoPageLoad = function () {

    ParamArray = {
        'recentPageNo':RecentPageNo,
        'processPageNo': ProcessPageNo,
        'delayPageNo': DelayPageNo,
        'impoPageNo': ImpoPageNo,
        'comPageNo': ComPageNo,
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());
    location = "AdminMain.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoRecentList = function () {

    if (RecentPageNo < 0) {
        alert(R_FirstList);
        RecentPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetRecentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': RecentPageNo,
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessGoRecentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트  업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoRecentList = function (data, status) {
    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (RecentPageNo > 0) {
            alert(R_LastList);
            RecentPageNo = parseInt(RecentPageNo) - 1;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoProcessList = function () {

    if (ProcessPageNo < 0) {
        alert(R_FirstList);
        ProcessPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ProcessPageNo,
        'status': '2,3',
        'order': 'List.ListID desc',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessGoProcessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoProcessList = function (data, status) {
    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (ProcessPageNo > 0) {
            alert(R_LastList);
            ProcessPageNo = parseInt(ProcessPageNo) - 1;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 지연된 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoDelayList = function () {

    if (DelayPageNo < 0) {
        alert(R_FirstList);
        DelayPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': DelayPageNo,
        'status': '6',
        'order': 'ExcessDay , List.ListID desc ',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessGoDelayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 지연된 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoDelayList = function (data, status) {

    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (DelayPageNo > 0) {
            alert(R_LastList);
            DelayPageNo = parseInt(DelayPageNo) - 1;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-07-05
작성목적 : 완료된 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoComList = function () {

    if (ComPageNo < 0) {
        alert(R_FirstList);
        ComPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetStatusList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ComPageNo,
        'status': '4',
        'order': 'CompleteDate desc, EndDate desc, List.ListID desc ',
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessGoComList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-07-05
작성목적 : 완료된 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoComList = function (data, status) {
    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (ComPageNo > 0) {
            alert(R_LastList);
            ComPageNo = parseInt(ComPageNo) - 1;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 중요표시 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoImpoList = function () {

    if (ImpoPageNo < 0) {
        alert(R_FirstList);
        ImpoPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/GetImpoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ImpoPageNo,
        'year': $("#ddlYear").val()
    };

    var obj = eval("Main.SuccessGoImpoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 중요표시 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoImpoList = function (data, status) {

    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (ImpoPageNo > 0) {
            alert(R_LastList);
            ImpoPageNo = parseInt(ImpoPageNo) - 1;
        }
    }
}