List = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.cbsWrite_OnClick();
        });

        //검색
        $('#cbtnFindit').click(function (e) {
            e.preventDefault();
            List.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#tbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색어입력
        $("#tbSearch").attr("placeholder", R_PlaceholderText);

    });
});

/**********************************************************************************************
[검색]
**********************************************************************************************/
List.Search = function () {
    var searchValue = $("#tbSearch").val().securityText();

    $("#tbSearch").val(searchValue);
    __doPostBack('lbtnSearch', '');
}

/**********************************************************************************************
[엔터]
**********************************************************************************************/
List.checkMods = function (evt, theForm) {
    if (evt.keyCode == 13)
        return false;
}

List.cbsWrite_OnClick = function () {
    //DeskPlus.Common.WindowPopup(700, 400, "ProjectList.aspx?" + ProjParam);

    var url = "ProjectList.aspx?" + ProjParam;

    PopupWin(700, 400, url, "", "", true);
}

List.ViewLinkProject = function (linkProjectID) {
    //DeskPlus.Common.WindowPopupMax("viewLinkProj", 900, 600, url);

    //    var targetURL = "../Project/Default.aspx?"
    //	targetURL += "companyID=" + CompanyID;
    //	targetURL += "&projectID=" + linkProjectID;
    //	targetURL += "&rqProjectID=" + ProjectID;
    //
    //	W = isValidMaxW(900);
    //	H = isValidMaxH(600);

    //	window.open(targetURL, "ViewLinkProject", "toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=" + W + ",height=" + H + ", top=" + getPostionCenterH(H) + ",left=" + getPostionCenterW(W));

    ParamArray = {
        'companyID': CompanyID,
        'projectID': linkProjectID,
        'rqProjectID': ProjectID
    };

    CallEncrypt(CallGetParam());

    var url = "../Project/Default.aspx";
    var param = "p=" + GetEncryptValue;

    var param = "companyID=" + CompanyID + "&projectID=" + linkProjectID + "&rqProjectID=" + ProjectID;
    WindowObjectReference = PopupWinOpen(url, param, "ViewLinkProject", "900", "600")
}