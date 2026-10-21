AuthInfo = {}

$(function () {

    $(document).ready(function () {

        //기본정보 이동
        $('#cbtnUser').click(function (e) { e.preventDefault(); AuthInfo.GoList("user"); });

        //닫기
        $('#cbtnList').click(function (e) { e.preventDefault(); self.close(); });

        //권한정보
        $('#aut').click(function (e) { e.preventDefault(); AuthInfo.GoList("aut"); });

        //요약정보
        $('#sum').click(function (e) { e.preventDefault(); AuthInfo.GoList("sum"); });

        //로그인정보
        $('#log').click(function (e) { e.preventDefault(); AuthInfo.GoList("log"); });
    });
});



/**************************************************
이동
***************************************************/
AuthInfo.GoList = function (gubun) {

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