Menu = {}

$(function () {

    $(document).ready(function () {

        // [대결중지]
        $('#btnProxyStop').click(function (e) { e.preventDefault(); Menu.ProxyAction(); });
        // [대결지정]
        $('#btnProxyMgr').click(function (e) { e.preventDefault(); Menu.MoveProxy(); });

        // Today 건수 evt
        $('#cntSM').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('SM'); });      //결재대상문서
        $('#cntCC').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('CC'); });      //참조받은문서
        $('#cntSC').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('SC'); });      //상신한 문서
        $('#cntCP').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('CP'); });      //수신받은 문서       
        $('#PrivateFolder').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('PrivateFolder'); });      //개인별문서함     
        $('#ApprovalFolder').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('ApprovalFolder'); });      //완결결재문서함     
        $('#RejectFolder').click(function (e) { e.preventDefault(); Menu.MoveAppCnt('RejectFolder'); });      //완결결재문서함     
               
        Menu.Init();
    });
});


Menu.Init = function () {

    // 1. 대결자정보
    Menu.GetProxy();

    // 2. 결재건수
    Menu.GetAppCnt();

    // 3. 자주사용하는 양식리스트
    Menu.GetFavForm();
}

/*** 건수정보 start ***********************************************************************************************/
// 건수가져오기
Menu.GetAppCnt = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Menu.asmx/GetTodayInfo";

    ParamArray = {};
    var obj = eval("Menu.SetAppCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Menu.SetAppCnt = function (data, status) {
    if (data.d) {
        $("#cntSM").html("<p>" + HomeCntSM + "</p><span class='badge number badge-light'>" + data.d.CntSM + "</span>"); 	//결재대상문서
        $("#cntCC").html("<p>" + HomeCntCC + "</p><span class='badge number badge-light'>" + data.d.CntCC + "</span>"); 	//참조받은문서
        $("#cntSC").html("<p>" + HomeCntSC + "</p><span class='badge number badge-light'>" + data.d.CntSC + "</span>"); 	//상신한 문서
        $("#cntCP").html("<p>" + HomeCntCP + "</p><span class='badge number badge-light'>" + data.d.CntCP + "</span>"); 	//수신받은 문서

        $("#PrivateFolder").html("<p>" + HomePrivate + "</p>");
        $("#ApprovalFolder").html("<p>" + HomeApproval + "</p>");
        $("#RejectFolder").html("<p>" + HomeReject + "</p>");
    }
}
// 건수링크
Menu.MoveAppCnt = function (_sec) {
    if (_sec == "") return;

    var url = "";
    var statusUrl = (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/DeskPlusEIP/Approval/Status/Default.aspx";
    switch (_sec.toUpperCase()) {
        case "SM":
            url = statusUrl;
            ParamArray = { 'folderStatusID': 'Approve' };
            break;
        case "CC":
            if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1) {
                url = statusUrl;
                ParamArray = { 'folderStatusID': 'CarboncopyReceiveAll' };
            } else {
                url = "/DeskPlusEIP/Approval/Folder/Default.aspx";
                ParamArray = { 'cabinetID': 'Carboncopy' };
            }
            break;
        case "SC":
            url = statusUrl;
            ParamArray = { 'folderStatusID': 'Process' };
            //ParamArray = { 'folderStatusID': 'Complete' };            
            break;
        case "CP":
            url = statusUrl;
            ParamArray = { 'folderStatusID': 'Receive' };
            break;
        case "PRIVATEFOLDER":
            if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1) {
                url = statusUrl;
                ParamArray = { 'folderStatusID': 'PrivateCompleteAll' };
            } else {
                url = "/DeskPlusEIP/Approval/Folder/Default.aspx";
                ParamArray = { 'cabinetID': 'Private' };
            }
            break;
        case "APPROVALFOLDER":
            if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1) {
                url = statusUrl;
                ParamArray = { 'folderStatusID': 'ApproveCompleteAll' };
            } else {
                url = "/DeskPlusEIP/Approval/Folder/Default.aspx";
                ParamArray = { 'cabinetID': 'Approval' };
            }
            break;
        case "REJECTFOLDER":
            if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1) {
                url = statusUrl;
                ParamArray = { 'folderStatusID': 'ApproveRejectAll' };
            } else {
                url = "/DeskPlusEIP/Approval/Folder/Default.aspx";
                ParamArray = { 'cabinetID': 'Reject' };
            }
            break;
        default:
            url = statusUrl;
            ParamArray = { 'folderStatusID': 'Approve' };
            break;
    }

    CallEncrypt(CallGetParam());
    parent.document.location.href = url + "?p=" + GetEncryptValue;
}
/*** 건수정보 end ***********************************************************************************************/

/*** 대결자 관련 start ***********************************************************************************************/
var proxyID = 0, proxyUserID = 0, proxyDeptID = 0;

// 대결자정보 가져오기
Menu.GetProxy = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Menu.asmx/GetProxy";

    ParamArray = {};
    var obj = eval("Menu.SetProxy");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Menu.SetProxy = function (data, status) {

    proxyID = 0, proxyUserID = 0, proxyDeptID = 0;  //값 초기화

    if (data.d) {

        var vals = '<div class="ProxyUser">' + R_HomeProxy + " : ";
        if (data.d.IsProxy == "Y") {

            vals += data.d.ProxyUserName + ' ' + data.d.ProxyTitleName + '</div>';
            proxyID = data.d.ProxyID;
            proxyUserID = data.d.ProxyUserID;
            proxyDeptID = data.d.ProxyDeptID;

            /*
            $("#divProxyAction").show();        //대결중지
            $("#divProxyMenu").hide();          //대결지정
            */

            vals += data.d.ProxyDateStr;

            $("#divProxyAction").hide();        //대결중지
            $("#divProxyMenu").hide();          //대결지정

        } else {
            vals = R_HomeProxyNo;               //대결자가 없습니다.

            $("#divProxyAction").hide();        //대결중지
            $("#divProxyMenu").show();          //대결지정
        }
        $("#spProxyUserInfo").html(vals);
    }
}

// [대결중지]
Menu.ProxyAction = function () {

    if (proxyID == 0 || proxyUserID == 0 || proxyDeptID == 0) { return; }

    if (confirm(R_HomeProxyAlert)) {    //대결중지하시겠습니까?
        var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/List.asmx/ProxyAction";

        ParamArray = {
            'proxyID': proxyID,
            'ProxyUserID': proxyUserID,
            'ProxyDeptID': proxyDeptID,
            'isProxy': 'N'
        };

        var obj = eval("Menu.SuccessProxyAction");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
Menu.SuccessProxyAction = function (data, status) {
    if (data.d == true) {
        Menu.GetProxy();
    }
}
// [대결지정]
Menu.MoveProxy = function () {
    var url = "/DeskPlusEIP/Approval/Preference/Default.aspx";
    ParamArray = { 'menuID': 'P' };
    CallEncrypt(CallGetParam());
    parent.document.location.href = url + "?p=" + GetEncryptValue;
}
/*** 대결자 관련 end ***********************************************************************************************/


/*** 양식리스트 start ***********************************************************************************************/
// 양식리스트가져오기
Menu.GetFavForm = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Menu.asmx/GetFavForm";

    ParamArray = {};
    var obj = eval("Menu.SetFavForm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Menu.SetFavForm = function (data, status) {

    $('#divFavList').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTFavForm.htm?seq=' + Guids(), null, { filter_data: false });
    $('#divFavList').processTemplate(data.d);

    // 리소스작업
    if ($(".clsFavTit") != undefined) { $(".clsFavTit").text(R_HomeFavTit); }
    if ($(".clsFavTitMgr") != undefined) { $(".clsFavTitMgr").text(R_HomeFavTitMgr); }
}
// 자주 사용하는 양식 선택 (문서작성 이동)
Menu.MoveFavForm = function (_formID) {

    if (_formID == "" || _formID == "0") return;

    var url = "/DeskPlusEIP/Approval/FormFlow/Default.aspx";
    ParamArray = { 'formID': _formID };

    CallEncrypt(CallGetParam());
    parent.document.location.href = url + "?p=" + GetEncryptValue;
}
// 자주 사용하는 양식관리 설정페이지 이동.
Menu.MoveFavFormMgr = function () {

    var url = "/DeskPlusEIP/Approval/Preference/Default.aspx";
    ParamArray = { 'menuID': 'FF' };
    CallEncrypt(CallGetParam());
    parent.document.location.href = url + "?p=" + GetEncryptValue;
}

/*** 양식리스트 end ***********************************************************************************************/
