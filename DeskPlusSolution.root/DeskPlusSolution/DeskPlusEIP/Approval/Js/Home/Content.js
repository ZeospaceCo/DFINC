Content = {}

$(function () {

    $(document).ready(function () {
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.       

        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) { e.preventDefault(); Content.SearchDoc(); });
        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Content.SearchDoc(); } });


        // 전자결재검색 evt
        $('#searchDocN').click(function (e) { e.preventDefault(); Content.SearchDocMoveList('N'); }); //다음
        $('#searchDocB').click(function (e) { e.preventDefault(); Content.SearchDocMoveList('B'); }); //이전
        // 첨부파일사용내역 evt
        $('#attachN').click(function (e) { e.preventDefault(); Content.AttachMoveList('N'); }); //다음
        $('#attachB').click(function (e) { e.preventDefault(); Content.AttachMoveList('B'); }); //이전
        // 결재의견 리스트 evt
        $('#replyN').click(function (e) { e.preventDefault(); Content.ReplyMoveList('N'); });   //다음
        $('#replyB').click(function (e) { e.preventDefault(); Content.ReplyMoveList('B'); });   //이전
        // 자주 지정하는 결재자 evt
        $('#favAppN').click(function (e) { e.preventDefault(); Content.FavAppMoveList('N'); }); //다음
        $('#favAppB').click(function (e) { e.preventDefault(); Content.FavAppMoveList('B'); }); //이전
        // 중요문서 evt
        $('#impoN').click(function (e) { e.preventDefault(); Content.ImpoMoveList('N'); });     //다음
        $('#impoB').click(function (e) { e.preventDefault(); Content.ImpoMoveList('B'); });     //이전


        // TAB more evt 
        $("#appmore").click(function (e) {
            parent.frmLeft.Menu.MoveAppCnt("SC");
        });
        $("#compmore").click(function (e) {
            parent.frmLeft.Menu.MoveAppCnt("PRIVATEFOLDER");
        });
        $("#ccmore").click(function (e) {
            parent.frmLeft.Menu.MoveAppCnt("CC");
        });


        //검색어입력
        $("#searchValue").attr("placeholder", R_PlaceholderMenu);

        Content.Init();
    });
});

Content.Init = function () {

    // 1. 최근 첨부파일 사용내역
    Content.GetAttach();

    // 2. 결재의견 리스트
    Content.GetReply();

    // 3. 자주 지정하는 결재자
    Content.GetFavApp();

    // 4. 중요문서
    Content.GetImpo();

    // 5. 업무리스트 (전체,결재대상문서,완료문서,참조문서)
    Content.SetTab(DefaultTab);
}

/*** 검색영역 start ***********************************************************************************************/


Content.SearchDoc = function () {
    var validator = new Validator($("#form1"));
    //validator.checkRequired("searchValue", "검색어를 입력해 주세요", true);
    validator.checkRequired("searchValue", R_InputSearch, true);
    if (!validator.validate()) return;

    var searchVal = $("#searchValue").val();

    ParamArray = {
        'searchValue': searchVal
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    location.href = "/DeskPlusEIP/Approval/Home/Search.aspx?" + param;
}

/*** 검색영역 end ***********************************************************************************************/





/*** 최근 첨부파일 사용내역 start *********************************************************************************/
// 최근 첨부파일 사용내역
var AttachNo = 1, AttachPageSize = 5;
Content.GetAttach = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/GetAttach";

    ParamArray = {
        'totCnt': AttachCnt
        , 'pageSize': AttachPageSize
        , 'pageNo': AttachNo
    };
    var obj = eval("Content.SetAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Content.SetAttach = function (data, status) {

    $('#phAttach').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTFile.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phAttach').processTemplate(data.d);

    // 리소스작업
    if ($("#spAttachTit") != undefined) { $("#spAttachTit").text(R_HomeAttachTit); }
    if ($("#spAttachTitSub") != undefined) { $("#spAttachTitSub").text(R_HomeAttachTitSub); }
    if ($("#spAttachNoData") != undefined) { $("#spAttachNoData").text(R_HomeNoData); }

    if ($("#spFileTh01") != undefined) { $("#spFileTh01").text(R_HomeFileTh01); }
    if ($("#spFileTh02") != undefined) { $("#spFileTh02").text(R_HomeFileTh02); }
    if ($("#spFileTh03") != undefined) { $("#spFileTh03").text(R_HomeFileTh03); }
    if ($("#spFileTh04") != undefined) { $("#spFileTh04").text(R_HomeFileTh04); }

    if ($(".fBtnNew") != undefined) { $(".fBtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

// 이전/이후
Content.AttachMoveList = function (_sec) {    //alert(_sec + "," + AttachNo)
    if (_sec == "") return;
    if (_sec == "B" && AttachNo == "1") return;

    AttachNo = (_sec == "N") ? AttachNo + 1 : AttachNo - 1;
    Content.GetAttach();
}

// 첨부파일다운로드
Content.FileDownload = function (_filePath, _fileName, _fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + _filePath;
    window.open(attachFilePath);
}

/*** 최근 첨부파일 사용내역 end *********************************************************************************/


/*** 결재의견 리스트 start *********************************************************************************/
// 결재의견 리스트
var ReplyNo = 1, ReplyPageSize = 3;
Content.GetReply = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/GetShortReply";

    ParamArray = {
        'totCnt': ReplyCnt
        , 'pageSize': ReplyPageSize
        , 'pageNo': ReplyNo
    };
    var obj = eval("Content.SetReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Content.SetReply = function (data, status) {

    $('#phReply').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTReply.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phReply').processTemplate(data.d);

    // 리소스작업
    if ($("#spReplyTit") != undefined) { $("#spReplyTit").text(R_HomeReplyTit); }
    if ($("#spReplyNoData") != undefined) { $("#spReplyNoData").text(R_HomeNoData); }

    if ($(".spsubject") != undefined) { $(".spsubject").text(R_HomeImpoTh01); }
    if ($(".rBtnNew") != undefined) { $(".rBtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
}

// 이전/이후
Content.ReplyMoveList = function (_sec) {    //alert(_sec + "," + AttachNo)
    if (_sec == "") return;
    if (_sec == "B" && ReplyNo == "1") return;

    ReplyNo = (_sec == "N") ? ReplyNo + 1 : ReplyNo - 1;
    Content.GetReply();
}


/*** 결재의견 리스트 end *********************************************************************************/



/*** 자주 지정하는 결재자 start *********************************************************************************/

// 자주 지정하는 결재자
var FavAppNo = 1, FavAppPageSize = 5;
Content.GetFavApp = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/GetFavApp";

    ParamArray = {
        'totCnt': FavAppCnt
        , 'pageSize': FavAppPageSize
        , 'pageNo': FavAppNo
    };
    var obj = eval("Content.SetFavApp");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Content.SetFavApp = function (data, status) {
    $('#phFavApp').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTFavApp.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phFavApp').processTemplate(data.d);

    // 리소스작업
    if ($("#spFavAppTit") != undefined) { $("#spFavAppTit").text(R_HomeFavAppTit); }
    if ($("#spFavAppNoData") != undefined) { $("#spFavAppNoData").text(R_HomeFavLink); }
    if ($(".spdel").length > 0) {
        $(".spdel").text(R_Btn_Del);
    }
}

// 이전/이후
Content.FavAppMoveList = function (_sec) {    //alert(_sec + "," + AttachNo)
    if (_sec == "") return;
    if (_sec == "B" && FavAppNo == "1") return;

    FavAppNo = (_sec == "N") ? FavAppNo + 1 : FavAppNo - 1;
    Content.GetFavApp();
}
// 삭제
Content.DelFavApp = function (_favAppID) {

    if (!confirm(R_HomeDelMsg)) return;

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/DelFavApp";

    FavAppNo = 1;
    ParamArray = { 'appFavID': _favAppID };
    var obj = eval("Content.GetFavApp");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
// 자주사용하는 결재자지정 페이지이동
Content.MoveFavApp = function () {
    var url = "/DeskPlusEIP/Approval/Preference/Default.aspx";
    ParamArray = { 'menuID': 'F' };
    CallEncrypt(CallGetParam());
    parent.document.location.href = url + "?p=" + GetEncryptValue;
}


/*** 자주 지정하는 결재자 end *********************************************************************************/


/*** 중요문서 start *********************************************************************************/
// 중요문서
var ImpoNo = 1, ImpoPageSize = 5;
Content.GetImpo = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/GetImpo";

    ParamArray = {
        'totCnt': ImpoCnt
        , 'pageSize': ImpoPageSize
        , 'pageNo': ImpoNo
    };
    var obj = eval("Content.SetImpo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Content.SetImpo = function (data, status) {
    $('#phImpo').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTImpo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phImpo').processTemplate(data.d);

    // 리소스작업
    if ($("#spImpoTit") != undefined) { $("#spImpoTit").text(R_HomeImpoTit); }
    if ($("#spImpoNoData") != undefined) { $("#spImpoNoData").text(R_HomeImpoLink); }
    if ($("#spImpoTh01") != undefined) { $("#spImpoTh01").text(R_HomeImpoTh01); }
    if ($("#spImpoTh02") != undefined) { $("#spImpoTh02").text(R_HomeImpoTh02); }
    if ($(".iBtnNew") != undefined) { $(".iBtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

// 이전/이후
Content.ImpoMoveList = function (_sec) {    //alert(_sec + "," + AttachNo)
    if (_sec == "") return;
    if (_sec == "B" && ImpoNo == "1") return;

    ImpoNo = (_sec == "N") ? ImpoNo + 1 : ImpoNo - 1;
    Content.GetImpo();
}

/*** 중요문서 end *********************************************************************************/


/*** TAB start *********************************************************************************/

// TAB
var selmodule = "";
Content.SetTab = function (_module) {   //alert("SetTab:" + _module);

    if (_module == "") return;
    selmodule = _module;

    $("#ulTabMenu li").removeClass("selected");

    switch (_module.toUpperCase()) {
        case "LISTTOTAL":           // 전체
            $("#ulTabMenu li").eq(0).addClass("selected");
            break;
        case "LISTAPPROVE":         // 결재대상문서
            $("#ulTabMenu li").eq(1).addClass("selected");
            break;
        case "LISTCOMPLETE":        // 완료문서
            $("#ulTabMenu li").eq(2).addClass("selected");
            break;
        case "LISTCARBON":          // 참조문서
            $("#ulTabMenu li").eq(3).addClass("selected");
            break;
        default:                    // 전체문서
            $("#ulTabMenu li").eq(0).addClass("selected");
            break;
    }
    Content.GetList(_module);
}

// TAB - 리스트
Content.GetList = function (_module) {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Content.asmx/";
    switch (_module.toUpperCase()) {
        case "LISTTOTAL":
            url += "GetListTotal";      // 전체
            break;
        case "LISTAPPROVE":
            url += "GetListApprove";    // 결재대상문서
            break;
        case "LISTCOMPLETE":
            url += "GetListComplete";   // 완료문서
            break;
        case "LISTCARBON":
            url += "GetListCarbon";     // 참조문서
            break;
        default:
            url += "GetListTotal";      // 전체문서
            break;
    }

    ParamArray = { 'd': 'd' };
    var obj = eval("Content.SetList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj)
}
Content.SetList = function (data, status) {

    var url = "/DeskPlusEIP/Approval/Jt/Home/JTListTab.htm?seq=" + Guids();

    $('#phlist').setTemplateURL(url, null, { filter_data: false });
    $('#phlist').processTemplate(data.d);

    // 리소스작업
    if ($(".spListNoData") != undefined) { $(".spListNoData").text(R_HomeNoData); }
    if ($(".spListTh01") != undefined) { $(".spListTh01").text(R_HomeTabTH01); }
    if ($(".spListTh02") != undefined) { $(".spListTh02").text(R_HomeTabTH02); }
    if ($(".spListTh03") != undefined) { $(".spListTh03").text(R_HomeTabTH03); }
    if ($(".spListTh04") != undefined) { $(".spListTh04").text(R_HomeTabTH04); }
    if ($(".spListTh05") != undefined) { $(".spListTh05").text(R_HomeTabTH05); }
    if ($(".spListTh06") != undefined) { $(".spListTh06").text(R_HomeTabTH06); }

    if ($(".tBtnNew") != undefined) { $(".tBtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
}

/*** TAB end *********************************************************************************/


/*** 공통 start *********************************************************************************/

// 문서보기
Content.View = function (_companyID, _documentID, groupID) {

    if (_companyID == "" || _documentID == "" || _companyID == "0" || _documentID == "0") return;

    ParamArray = { 'companyID': _companyID, 'documentID': _documentID, 'srh': '1' };
    CallEncrypt(CallGetParam());

    //var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
    //window.open(url, "View" + _documentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    if (groupID < 800000) {
        url = "/DeskPlusEIP/Approval/Common/Popup33/ViewTot.aspx?p=" + GetEncryptValue;

        if ($.browser.msie) {
            PopupWinOpen(url, "", "View" + _documentID, "1000", "745", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
        else {
            PopupWinOpen(url, "", "View" + _documentID, "1000", "700", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
    }
    else {
        var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
        window.open(url, "View" + _documentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }
}

// 결재상태보기
Content.State = function (_isComplete, _documentID, _accountTag) {

    ParamArray = {
        'isComplete': _isComplete
        , 'documentID': _documentID
        , 'accountTag': _accountTag
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Status/Common/GetDocFlow.aspx?p=" + GetEncryptValue;
    PopupWin(850, 400, url, "-500", "", true);
}

// 결재하기
Content.Approve = function (_cID, _dID, _seq, _apptag, _tuserid, _tdeptid) {

    if (_cID == "" || _cID == "0" || _dID == "" || _dID == "0" || _seq == "" || _seq == "0" || _apptag == "" || _tuserid == "" || _tuserid == "0" || _tdeptid == "" || _tdeptid == "0") return;

    ParamArray = {
        'companyID': _cID
        , 'documentID': _dID
        , 'sequence': _seq
        , 'approvalTag': _apptag
        , 'targetUserID': _tuserid
        , 'targetDeptID': _tdeptid
        , 'func': 'Content.ApproveComplete()'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Process/Basic/ApprovePopup.aspx?p=" + GetEncryptValue;
    window.open(url, "Approve" + _dID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}
Content.ApproveComplete = function () {
    Content.SetTab(selmodule);
}
/*** 공통 start *********************************************************************************/



