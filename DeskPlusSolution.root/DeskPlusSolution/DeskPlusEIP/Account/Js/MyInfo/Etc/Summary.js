Summary = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) { e.preventDefault(); Summary.GoList("user"); });

        //닫기
        $('#cbtnList').click(function (e) { e.preventDefault(); self.close(); });

        //권한정보
        $('#aut').click(function (e) { e.preventDefault(); Summary.GoList("aut"); });

        //요약정보
        $('#sum').click(function (e) { e.preventDefault(); Summary.GoList("sum"); });

        //로그인정보
        $('#log').click(function (e) { e.preventDefault(); Summary.GoList("log"); });


        //조회 - 전자결재
        $('#btnapp').click(function (e) { e.preventDefault(); Summary.GetApp(); });

        //조회 - 게시판
        $('#btnboa').click(function (e) { e.preventDefault(); Summary.GetBoa(); });

        //조회 - 커뮤니티
        $('#btncomm').click(function (e) { e.preventDefault(); Summary.GetComm(); });

        //조회 - 권한
        $('#btnauth').click(function (e) { e.preventDefault(); Summary.GetAuth(); });

        //조회 - 업무협업
        $('#btntask').click(function (e) { e.preventDefault(); Summary.GetTask(); });
    });
});

/**************************************************
이동
***************************************************/
Summary.GoList = function (gubun) {

    var goList = "";
    switch (gubun) {
        case "user":
            goList = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";            //기본정보
            break;
        case "aut":
            goList = "/DeskPlusEIP/Account/MyInfo/Etc/AuthInfo.aspx";   //권한정보
            break;
        case "sum":
            goList = "/DeskPlusEIP/Account/MyInfo/Etc/Summary.aspx";    //요약정보
            break;
        case "log":
            goList = "/DeskPlusEIP/Account/MyInfo/Etc/LoginInfo.aspx";  //로그인접속정보
            break;
    }

    ParamArray = {
        'companyID': CompanyID
        , 'deptID': DeptID
        , 'userID': UserID
        , 'isadmin': IsAdmin
    };
    CallEncrypt(CallGetParam());

    location = goList + "?p=" + GetEncryptValue;
}

/**************************************************
전자결재-조회
***************************************************/
Summary.GetApp = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Etc/Summary.asmx/GetApp";
    ParamArray = {
        'companyID': CompanyID
        , 'deptID': DeptID
        , 'userID': UserID
    };
    var obj = eval("Summary.SetApp");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Summary.SetApp = function (data, status) {
    $('#apping').html(data.d[0]);
    $('#appcom').html(data.d[1]);
}

/**************************************************
게시판-조회
***************************************************/
Summary.GetBoa = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Etc/Summary.asmx/GetBoa";
    ParamArray = {
        'companyID': CompanyID
        , 'deptID': DeptID
        , 'userID': UserID        
    };
    var obj = eval("Summary.SetBoa");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Summary.SetBoa = function (data, status) {
    $('#boacnt').html(data.d[0]);
    $('#boanm').html(data.d[1]);
}

/**************************************************
커뮤니티-조회
***************************************************/
Summary.GetComm = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Etc/Summary.asmx/GetComm";
    ParamArray = {
        'companyID': CompanyID
        , 'deptID': DeptID
        , 'userID': UserID        
    };
    var obj = eval("Summary.SetComm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Summary.SetComm = function (data, status) {
    $('#cmmcnt').html(data.d[0]);
    $('#cmmmgr').html(data.d[1]);
}

/**************************************************
업무협업-조회
***************************************************/
Summary.GetTask = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Etc/Summary.asmx/GetTask";
    ParamArray = {
        'companyID': CompanyID
        , 'deptID': DeptID
        , 'userID': UserID
    };
    var obj = eval("Summary.SetTask");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Summary.SetTask = function (data, status) {
    $('#taskcnt').html(data.d[0]);
}

