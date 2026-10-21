Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        //폴더관리
        $('#CbtnFolder').click(function (e) {
            e.preventDefault();
            Add.GetFolder();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBMultiName').attr("disabled", true);
        $('#tbxFolder').attr("disabled", true);

        $("#tbxPlanStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxPlanEndDate").datepick($.datepicker.regional[PreLang]);
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2011-10-18
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiName").val(multiNames);
}

/**********************************************************************************************
[달력선택]
**********************************************************************************************/
Add.SelectDateCode = function () {
    //	url = "/DeskPlusEIP/Common/Calendar/CalendarDualDialog.aspx?func=GetDate";
    //	window.open(url, "BoardCalendarDuals", "toolbar=no,width=400,height=200,directories=no,status=no,scrollbars=no,resize=no,menubar=no");

    var url = "/DeskPlusEIP/Common/Calendar/CalendarDualDialog.aspx";
    var param = "func=List.GetDate";
    WindowObjectReference = PopupWinOpen(url, param, "BoardCalendarDuals", "780", "380")
}

/**********************************************************************************************
[달력값 설정]
**********************************************************************************************/
function GetDate(hdYear1, hdMonth1, hdDay1, hdYear2, hdMonth2, hdDay2) {
    var startDate = hdYear1 + "-" + hdMonth1 + "-" + hdDay1;
    var endDate = hdYear2 + "-" + hdMonth2 + "-" + hdDay2;

    $("#tbxPlanStartDate").val(startDate);
    $("#tbxPlanEndDate").val(endDate);
}

/**********************************************************************************************
[폴더 새창]
**********************************************************************************************/
Add.GetFolder = function () {

    ParamArray = {
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/PMS/Module/Popup/GetFolder.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "", "", true);
}

function SelectedFolder(folderID, folderName) {
    $("#tbxFolderID").val(folderID);
    $("#tbxFolder").val(unescape(folderName));
}

/**********************************************************************************************
[저장하기]
**********************************************************************************************/
Add.Save = function () {
    $('#tbxFolder').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxFolder", R_Project_MSGFolder, true);

    if (!validator.validate()) {
        $('#tbxFolder').attr("disabled", true);
        return;
    }

    validator.checkRequired("tbxPlanStartDate", R_Project_MSGPlanDate, true);
    if (!validator.validate()) {
        $("#tbxPlanStartDate").select();
        return;
    }

    validator.checkRequired("tbxPlanEndDate", R_Project_MSGPlanDate, true);
    if (!validator.validate()) {
        $("#tbxPlanEndDate").select();
        return;
    }

    validator.checkRequired("tbxTitle", R_Project_MSGProjectName, true);
    if (!validator.validate()) {
        $("#tbxTitle").select();
        return;
    }

    if (!validator.validate()) return;

    var getStartDate = $("#tbxPlanStartDate").val();
    var getEndDate = $("#tbxPlanEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputStartDays);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return false;
        }
    }

    $('#tbxFolder').attr("disabled", false);
    __doPostBack('lbtnWrite', '');
}