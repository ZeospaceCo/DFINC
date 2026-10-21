Mod = {}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        $("#cbxAllDay").click(function () {
            if (this.checked) {
                var getStartDate = $("#startDate").val();
                var getEndDate = $("#endDate").val();

                if (getStartDate == "") {
                    $("#cbxAllDay").attr("checked", false);
                    alert(R_ApplyStartDate); //"시작일자를 입력해 주세요"
                    return;
                }
                else {
                    // 종일 체크한 경우, 종료일자를 입력전이거나 종료일자가 시작일자보다 같거나 작을때 시작일자에서 +1일 처리
                    if (getEndDate == "") {
                        var endDate = ConvertDT(getStartDate, PreLangReal);
                        endDate.setDate(endDate.getDate() + 1);

                        endDate = GetMultiDate(endDate.getFullYear(), (endDate.getMonth() + 1), endDate.getDate(), PreLangReal);
                        $("#endDate").val(endDate);
                    }
                    else {
                        var dtResponse = CompareDT(getStartDate, getEndDate, "greaterequal", PreLangReal);
                        if (dtResponse == true) {
                            var endDate = ConvertDT(getStartDate, PreLangReal);
                            endDate.setDate(endDate.getDate() + 1);

                            endDate = GetMultiDate(endDate.getFullYear(), (endDate.getMonth() + 1), endDate.getDate(), PreLangReal);
                            $("#endDate").val(endDate);
                        }
                    }
                }

                $("#ddlstartTime option:eq(0)").attr("selected", "selected");
                $("#ddlendTime option:eq(0)").attr("selected", "selected");

                $("#ddlstartTime").attr("disabled", "disabled");
                $("#ddlendTime").attr("disabled", "disabled");
            }
            else {
                $("#ddlstartTime").removeAttr("disabled");
                $("#ddlendTime").removeAttr("disabled");
            }
        });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); Mod.List(); });
        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); Mod.Confirm(); });

        Mod.Init();

    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 초기화 하기
***************************************************/
Mod.Init = function () {

    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    if ($("#cbxAllDay").is(":checked")) {
        $("#ddlstartTime").attr("disabled", "disabled");
        $("#ddlendTime").attr("disabled", "disabled");
    }

    Mod.LoadData("");
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 초기화
***************************************************/
Mod.LoadData = function (gubun) {


}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [확인]
***************************************************/
Mod.Confirm = function () {
    //필수입력
    var validator = new Validator($("#form1"));

    validator.checkRequired("tbxProxyUserName", R_SelectProxyPerson, true); ///"대결자를 선택해 주세요."
    if (!validator.validate()) return;

    //동일인은 지정 불가
    if ($("#tbxUserID").val() == $("#tbxProxyUserID").val()) {
        alert(R_SelectProxyDiffer); //"동일인은 지정 불가합니다."
        return;
    }

    //날짜 확인
    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();
    var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
    var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

    if (getStartDate == "" || getEndDate == "") {
        alert(R_ApplyAllRegDate); //"시작일자, 종료일자 둘다 입력해 주시기 바랍니다"
        return;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);
        if (dtResponse == false) {
            alert(R_StartDateEndDate); //"시작일자가 종료일자보다 작아야 합니다.";
            return;
        }

        var dtResponse1 = CompareDT(getStartDate, getEndDate, "equal", PreLangReal);
        if (dtResponse1 == true) {
            if (eTime <= sTime) {
                alert(R_StartDateEndDiffer); //"종료시간은 시작시간과 같거나 작을 수 없습니다"
                return;
            }
        }
    }

    if ($("#ddlAbsenceType").val() == "") {
        alert(R_SelectTypeAbsence); //"부재유형을 선택하세요."
        return;
    }

    //지정한 대결자 등록 여부 및 기간 중복 조회
    //Mod.GetProxyByUserID();
    Mod.GetProxySelByDate();
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-17
작성목적 : 지정한 대결자 등록되어있는지 조회
**********************************************************************************************/
Mod.GetProxyByUserID = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/Mod.asmx/GetProxyByUserID";

    ParamArray = {
        'proxyUserID': $("#tbxProxyUserID").val(),
        'proxyDeptID': $("#tbxProxyDeptID").val(),
        'targetProxyID': TargetProxyID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetProxyByUserID");
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 지정한 대결자 등록되어있는지 조회
**********************************************************************************************/
Mod.SuccessGetProxyByUserID = function (data, status) {
    if (data.d) {
        Mod.GetProxySelByDate();
    }
    else {
        alert(R_SelectProxyOverlap); //"선택한 사용자는 이미 대결자로 등록되어 있습니다."
        return;
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-17
작성목적 : 선택한 기간에 대결자 지정을 할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Mod.GetProxySelByDate = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/Mod.asmx/GetProxySelByDate";

    ParamArray = {
        'startDate': $("#startDate").val(),
        'endDate': $("#endDate").val(),
        'sTime': $("#ddlstartTime").val(),
        'eTime': $("#ddlendTime").val(),
        'targetProxyID': TargetProxyID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetProxySelByDate");
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 선택한 기간에 대결자 지정을 할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetProxySelByDate = function (data, status) {
    if (data.d) {
        if (confirm(R_ProxyConfirm1 + "\r" + R_ProxyConfirm2)) { //등록된 대결자는 부재기간에 따라 대결진행, 대기, 종료됩니다. 등록하시겠습니까?
            __doPostBack('lbtnSubmit', '');
        }
    }
    else {
        alert(R_ApplyDateOverlap); //"선택한 날짜에 이미 대결자가 지정되어 있습니다."
        return;
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
Mod.List = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 대결자 선택
**********************************************************************************************/
Mod.GetUser = function () {

    var callback = "GetProxyUserPath";

    ParamArray = {
        'gubun': "1",
        'isCheckBox': "0",
        'callback': callback
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUserSelect.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}

function GetProxyUserPath(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {

        rtnVal = unescape(rtnVal);

        var responseStr = rtnVal.split("");
        var userID = responseStr[0].split(",")[0];
        var userName = responseStr[1].split(",")[0];
        var deptID = responseStr[2].split(",")[0];

        var items = responseStr[4].split("|");
        var titleID = items[4];
        var titleName = items[5];
        var deptName = items[11];


        $("#tbxProxyDeptName").val(deptName);
        $("#tbxProxyUserName").val(userName + "(" + titleName + ")");
        $("#tbxProxyDeptID").val(deptID);
        $("#tbxProxyUserID").val(userID);
    }
}

/**************************************************
작성목적 : 날짜 변환
* 작성자 : shimpark
***************************************************/
ConvertDT = function (p_Date, p_preLang) {
    var dateformat = "yy-mm-dd";

    var setDate = new Date();

    switch (p_preLang) {
        case "ko":
            dateformat = "yy-mm-dd";
            p_Date = p_Date.replaceText("-", "");
            setDate = new Date(p_Date.substring(0, 4), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(6, 8));

            break;
        case "en":
            dateformat = "mm/dd/yy";
            p_sDate = p_sDate.replaceText("/", "");
            setDate = new Date(p_Date.substring(6, 8), parseInt(p_Date.substring(0, 4)) - 1, p_Date.substring(4, 6));
            break;
        case "en-gb":
            dateformat = "dd/mm/yy";
            p_sDate = p_sDate.replaceText("/", "");
            setDate = new Date(p_Date.substring(6, 8), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(0, 4));
            break;
        case "ja":
            dateformat = "yy/mm/dd";
            p_sDate = p_sDate.replaceText("/", "");
            setDate = new Date(p_Date.substring(0, 4), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(6, 8));
            break;
        case "fr":
            dateformat = "dd/mm/yy";
            p_sDate = p_sDate.replaceText("/", "");
            setDate = new Date(p_Date.substring(6, 8), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(0, 4));
            break;
        case "zh-cn":
            dateformat = "yy/mm/dd";
            p_sDate = p_sDate.replaceText("/", "");
            setDate = new Date(p_Date.substring(0, 4), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(6, 8));
            break;
        case "zh-tw":
            dateformat = "yy-mm-dd";
            p_sDate = p_sDate.replaceText("-", "");
            setDate = new Date(p_Date.substring(0, 4), parseInt(p_Date.substring(4, 6)) - 1, p_Date.substring(6, 8));
            break;
    }

    return setDate;
}
