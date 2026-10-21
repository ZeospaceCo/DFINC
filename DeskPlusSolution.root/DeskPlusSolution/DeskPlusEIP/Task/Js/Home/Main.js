Main = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) { e.preventDefault(); Main.Search(); });
        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Main.Search(); } });


        //관리자 화면 탭이동
        $('#tabPage').click(function (e) {
            e.preventDefault();
            Main.SaveEnv();
        });

        //최신 업데이트 업무리스트 이전목록
        $('#recentL').click(function (e) {
            e.preventDefault();
            RecentPageNo = parseInt(RecentPageNo) - 1;
            Main.GetGoRecentList();
        });

        //최신 업데이트 업무리스트 다음목록
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

        //참조 받은 업무리스트 이전목록
        $('#ccL').click(function (e) {
            e.preventDefault();
            CCPageNo = parseInt(CCPageNo) - 1;
            Main.GetGoCCList();
        });

        //참조 받은 업무리스트 다음목록
        $('#ccR').click(function (e) {
            e.preventDefault();
            CCPageNo = parseInt(CCPageNo) + 1;
            Main.GetGoCCList();
        });

        //완료예정 및 지연 업무리스트 이전목록
        $('#delayL').click(function (e) {
            e.preventDefault();
            DelayPageNo = parseInt(DelayPageNo) - 1;
            Main.GetGoDelayList();
        });

        //완료예정 및 지연 업무리스트 이전목록
        $('#delayR').click(function (e) {
            e.preventDefault();
            DelayPageNo = parseInt(DelayPageNo) + 1;
            Main.GetGoDelayList();
        });

        //개인 답변 업무리스트 이전목록
        $('#writeCommentL').click(function (e) {
            e.preventDefault();
            WriteCommentPageNo = parseInt(WriteCommentPageNo) - 1;
            Main.GetGoWriteCommentList();
        });

        //개인 답변 업무리스트 다음목록
        $('#writeCommentR').click(function (e) {
            e.preventDefault();
            WriteCommentPageNo = parseInt(WriteCommentPageNo) + 1;
            Main.GetGoWriteCommentList();
        });

        //주요 검토 업무리스트 이전목록
        $('#impoL').click(function (e) {
            e.preventDefault();
            ImpoPageNo = parseInt(ImpoPageNo) - 1;
            Main.GetGoImpoList();
        });

        //주요 검토 업무리스트 다음목록
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
    Main.GetProcessList();
    Main.GetCCList();
    Main.GetDelayList();
    Main.GetRecentList();  
    Main.GetWriteCommentList();
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
작성일자 : 2016-02-11
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetProcessList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetProgressList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ProcessPageNo
    };

    var obj = eval("Main.SuccessProcessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 진행중인 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessProcessList = function (data, status) {

    $('#phProcessList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTProgress.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phProcessList').processTemplate(data.d);
                
    setTimeout("Main.ProcessResource();", 200);    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.ProcessResource = function () {

    if ($("#r_pgubun")[0] == undefined) {
        setTimeout("Main.ProcessResource();", 200);
    }
    else {
        $("#r_pgubun")[0].innerHTML = R_IsGubun;
        $("#r_ptitle")[0].innerHTML = R_Title;
        $("#r_phname")[0].innerHTML = R_HName;

        if ($("#P_NoListData")[0] != undefined) {
            $("#P_NoListData")[0].innerHTML = R_ProgressNoContent;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 참조받은 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetCCList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetCCList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': CCPageNo
    };

    var obj = eval("Main.SuccessCCList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 참조받은 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessCCList = function (data, status) {
    
    $('#phCCList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTCC.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCCList').processTemplate(data.d);
                
    setTimeout("Main.CCResource();", 200);   
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.CCResource = function () {

    if ($("#r_ctitle")[0] == undefined) {
        setTimeout("Main.CCResource();", 200);
    }
    else {
        $("#r_ctitle")[0].innerHTML = R_Title;
        $("#r_crequester")[0].innerHTML = R_Requester;
        $("#r_ctoname")[0].innerHTML = R_Receiver;

        if ($("#C_NoListData")[0] != undefined) {
            $("#C_NoListData")[0].innerHTML = R_CCNoContent;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 완료예정업무 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetDelayList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetDelayList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': DelayPageNo
    };

    var obj = eval("Main.SuccessDelayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
작성목적 : 완료예정업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessDelayList = function (data, status) {
    
    $('#phDelayList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTDelay.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phDelayList').processTemplate(data.d);
                
    setTimeout("Main.DelayResource();", 200);   
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.DelayResource = function () {
    if ($("#r_dgubun")[0] == undefined) {
        setTimeout("Main.DelayResource();", 200);
    }
    else {
        $("#r_dgubun")[0].innerHTML = R_IsGubun;
        $("#r_dtitle")[0].innerHTML = R_Title;
        $("#r_dstatus")[0].innerHTML = R_State;
        $("#r_drequestdept")[0].innerHTML = R_RequestDept;
        $("#r_drequester")[0].innerHTML = R_Requester;
        $("#r_drequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_denddate")[0].innerHTML = R_EndDate;
        $("#r_dtodept")[0].innerHTML = R_ToDeptName;
        $("#r_dtoname")[0].innerHTML = R_Receiver;

        if ($("#D_NoListData")[0] != undefined) {
            $("#D_NoListData")[0].innerHTML = R_HDelayNoContent;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetRecentList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetRecentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': RecentPageNo
    };

    var obj = eval("Main.SuccessRecentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessRecentList = function (data, status) {

    $('#phRecentList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTRecent.htm?seq=' + Guids(), null, { filter_data: false });
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
작성일자 : 2015-12-24
작성목적 : 개인 답변 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetWriteCommentList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetWriteCommentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': WriteCommentPageNo
    };

    var obj = eval("Main.SuccessWriteCommentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 개인 답변 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessWriteCommentList = function (data, status) {

    $('#phWritecommentList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTWriteComment.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phWritecommentList').processTemplate(data.d);
                
    setTimeout("Main.WriteCommentResource();", 200);    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Main.WriteCommentResource = function () {

    if ($("#r_wtitle")[0] == undefined) {
        setTimeout("Main.WriteCommentResource();", 200);
    }
    else {
        $("#r_wtitle")[0].innerHTML = R_Title;
        $("#r_wstatus")[0].innerHTML = R_State;
        $("#r_wrequestdept")[0].innerHTML = R_RequestDept;
        $("#r_wrequester")[0].innerHTML = R_Requester;
        $("#r_wrequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_wenddate")[0].innerHTML = R_EndDate;
        $("#r_wtodept")[0].innerHTML = R_ToDeptName;
        $("#r_wtoname")[0].innerHTML = R_Receiver;

        if ($("#W_NoListData")[0] != undefined) {
            $("#W_NoListData")[0].innerHTML = R_UserNoContent;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-12
작성목적 : 주요 검토 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetImpoList = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetImpoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ImpoPageNo
    };

    var obj = eval("Main.SuccessImpoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-12
작성목적 : 주요 검토 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessImpoList = function (data, status) {
    
    $('#phImpoList').setTemplateURL('/DeskPlusEIP/Task/Jt/Home/JTImpo.htm?seq=' + Guids(), null, { filter_data: false });
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
        $("#r_istatus")[0].innerHTML = R_State;
        $("#r_igubun")[0].innerHTML = R_HGubun;
        $("#r_irequestdept")[0].innerHTML = R_RequestDept;
        $("#r_irequester")[0].innerHTML = R_Requester;
        $("#r_irequsetdate")[0].innerHTML = R_RequestDate;
        $("#r_ienddate")[0].innerHTML = R_EndDate;
        $("#r_itodept")[0].innerHTML = R_ToDeptName;
        $("#r_itoname")[0].innerHTML = R_Receiver;

        if ($("#I_NoListData")[0] != undefined) {
            $("#I_NoListData")[0].innerHTML = R_PointNoContent;
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
        'isPage': "Main",        
        'processPageNo': ProcessPageNo,
        'ccPageNo': CCPageNo,
        'delayPageNo': DelayPageNo,
        'recentPageNo': RecentPageNo,
        'writeCommentPageNo':WriteCommentPageNo,
        'impoPageNo': ImpoPageNo
    };

    CallEncrypt(CallGetParam());

    location = "ViewN.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-13
작성목적 : 탭환경저장 (웹서비스 호출)
**********************************************************************************************/
Main.SaveEnv = function () {

    var url = "/DeskPlusEIP/Task/Ws/Home/AdminMain.asmx/ModEnv";

    //파라미터값 배열에 담기
    ParamArray = {
        'isPage': "AdminMain"
    };

    var obj = eval("Main.SuccessSaveEnv");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-13
작성목적 : 탭환경저장 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessSaveEnv = function (data, status) {

    if (data.d) {
        parent.frmLeft.DefaultTab = "AdminMain";
        location.href = "/DeskPlusEIP/Task/Home/AdminMain.aspx";
    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 페이지이동 처리
**********************************************************************************************/
Main.GoPageLoad = function () {

    ParamArray = {
        'delayPageNo': DelayPageNo,
        'processPageNo': ProcessPageNo,
        'ccPageNo': CCPageNo,
        'recentPageNo':RecentPageNo,
        'writeCommentPageNo': WriteCommentPageNo,
        'impoPageNo': ImpoPageNo
    };

    CallEncrypt(CallGetParam());
    location = "Main.aspx?p=" + GetEncryptValue;
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

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetProgressList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ProcessPageNo
    };

    var obj = eval("Main.SuccessGoProcessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-11
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
작성목적 : 참조받은 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoCCList = function () {

    if (CCPageNo < 0) {
        alert(R_FirstList);
        CCPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetCCList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': CCPageNo
    };

    var obj = eval("Main.SuccessGoCCList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 참조받은 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoCCList = function (data, status) {
    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (CCPageNo > 0) {
            alert(R_LastList);
            CCPageNo = parseInt(CCPageNo) - 1;
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 완료예정업무 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoDelayList = function () {

    if (DelayPageNo < 0) {
        alert(R_FirstList);
        DelayPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetDelayList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': DelayPageNo
    };

    var obj = eval("Main.SuccessGoDelayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 완료예정업무 리스트 가져오기 (웹서비스 값 받기)
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
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoRecentList = function () {

    if (RecentPageNo < 0) {
        alert(R_FirstList);
        RecentPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetRecentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': RecentPageNo
    };

    var obj = eval("Main.SuccessGoRecentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-23
작성목적 : 최신 업데이트 리스트 가져오기 (웹서비스 값 받기)
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
작성목적 : 개인 답변 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoWriteCommentList = function () {

    if (WriteCommentPageNo < 0) {
        alert(R_FirstList);
        WriteCommentPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetWriteCommentList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': WriteCommentPageNo
    };

    var obj = eval("Main.SuccessGoWriteCommentList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 개인 답변 업무 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Main.SuccessGoWriteCommentList = function (data, status) {

    if (data.d.length > 0) {
        Main.GoPageLoad();
    }
    else {
        if (WriteCommentPageNo > 0) {
            alert(R_LastList);
            WriteCommentPageNo = parseInt(WriteCommentPageNo) - 1;
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 주요 검토 업무 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Main.GetGoImpoList = function () {

    if (ImpoPageNo < 0) {
        alert(R_FirstList);
        ImpoPageNo = 0;
        return;
    }

    var url = "/DeskPlusEIP/Task/Ws/Home/Main.asmx/GetImpoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': 7,
        'pageSizePageNo': ImpoPageNo
    };

    var obj = eval("Main.SuccessGoImpoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-23
작성목적 : 주요 검토 리스트 가져오기 (웹서비스 값 받기)
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