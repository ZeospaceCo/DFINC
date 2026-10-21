List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {

        //이전달
        $('#pvMonth').click(function (e) {
            e.preventDefault();
            List.PvMonth();
        });

        //이후달
        $('#nxMonth').click(function (e) {
            e.preventDefault();
            List.NxMonth();
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add('0');
        });

        // 년 변경
        $("#ddlYear").change(function (e) {
            e.preventDefault();
            List.ChangeYear();
        });

        // 월 변경
        $("#ddlMonth").change(function (e) {
            e.preventDefault();
            List.ChangeMonth();
        });

        var tmpTime = GetTime($("#timeList").text());
        $("#timeList").html(tmpTime);

        //데이터 로딩
        List.GetList(TmpDate);

    });
});


/**************************************************
작성자: famo23
작성일자 : 2012-05-31
작성목적 : 날짜 바인딩
***************************************************/
function GetTime(_date) {
    var arrDate = _date.split('-');
    var rtnVal = "";

    switch (PreLang) {
        case "ko":
            //"yy-mm";
            rtnVal = arrDate[0] + "-" + arrDate[1];
            break;
        case "en":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm";
            rtnVal = arrDate[0] + "/" + arrDate[1];
            break;
        case "fr":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm";
            rtnVal = arrDate[0] + "/" + arrDate[1];
            break;
        case "zh-tw":            
            //"yy-mm";
            rtnVal = arrDate[0] + "-" + arrDate[1];
            break;
    }
    return rtnVal;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetList = function (_date) {
    var arrDate = _date.split('-');

    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Schedule/List.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'year': arrDate[0],
        'month': arrDate[1]
    };

    var obj = eval("List.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
List.SuccessList = function (data, status) {

    $('#phListMonth').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Schedule/JTListMonth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListMonth').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#j_sun")[0].innerHTML = R_Sunday;
    $("#j_mon")[0].innerHTML = R_Monday;
    $("#j_tue")[0].innerHTML = R_Tuesday;
    $("#j_wed")[0].innerHTML = R_Wednesday;
    $("#j_thu")[0].innerHTML = R_Thursday;
    $("#j_fri")[0].innerHTML = R_Friday;
    $("#j_sat")[0].innerHTML = R_Saturday;
}

function addMonth(tmpDate, v_month) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = 1;
    var tmpMM = "";
    var addDate = new Date(yyyy, eval(mm + '+' + v_month), dd);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    tmpMM = (addDate.getMonth() + 1);

    $("#timeList").empty();    
    var tmpTime = GetTime(yyyy + "-" + mm);
    $("#timeList").html(tmpTime);

    $("#ddlYear").val(yyyy);
    $("#ddlMonth").val(tmpMM);

    var tmpYear = $("#ddlYear").val();

    if (tmpYear > yyyy) {
        TargetYear = Number(TargetYear) - 10;
        $('#ddlYear').empty();

        $('#ddlYear').append("<option value='0000'>▲</option>");

        for (var i = TargetYear; i < TargetYear + 10; i++) {
            $('#ddlYear').append("<option value='" + i + "' >" + i + "</option>");
        }

        $('#ddlYear').append("<option value='9999'>▼</option>");

        $("#ddlYear").val(yyyy);
    }

    if (tmpYear < yyyy) {
        TargetYear = Number(TargetYear) + 10;
        $('#ddlYear').empty();

        $('#ddlYear').append("<option value='0000'>▲</option>");

        for (var i = TargetYear; i < TargetYear + 10; i++) {
            $('#ddlYear').append("<option value='" + i + "' >" + i + "</option>");
        }

        $('#ddlYear').append("<option value='9999'>▼</option>");

        $("#ddlYear").val(yyyy);
    }

    $("#tbYear").val(yyyy);

    return yyyy + "-" + mm;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 이전 날짜 가기
**********************************************************************************************/
List.PvMonth = function () {
    TmpDate = addMonth(TmpDate, -1);
    //List.GetList(TmpDate);    
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 이후 날짜 가기
**********************************************************************************************/
List.NxMonth = function () {

    TmpDate = addMonth(TmpDate, 1);
    //List.GetList(TmpDate);
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-31
작성목적 : 일정 보기
**********************************************************************************************/
List.View = function (_scheduleID) {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'scheduleID': _scheduleID,
        'tmpDate': TmpDate,
        'grade': Grade                
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}


/******************************************
작성자: famo23
작성일자: 2012-05-31
작성목적: 일정 추가
****************************************/
List.Add = function (_tmpDate) {

    if(CheckAuth == "")
    {
        ParamArray = {
            'communityID': CommunityID,
            'menuID': MenuID,
            'tmpDate': _tmpDate,
            'grade': Grade
        };

        CallEncrypt(CallGetParam());

        location.href = "AddN.aspx?p=" + GetEncryptValue;

    }
    else{
        alert(R_NoWriteAuth);
        return;
    }
}

/******************************************
작성자: famo23
작성일자: 2012-06-05
작성목적: 년 변경
****************************************/
List.ChangeYear = function () {

    if ($('#ddlYear').val() == "0000") {
        TargetYear = Number(TargetYear) - 10;
        var tmpYear1 = "";
        $('#ddlYear').empty();

        $('#ddlYear').append("<option value='0000'>▲</option>");

        for (var i = TargetYear; i < TargetYear + 10; i++) {
            $('#ddlYear').append("<option value='" + i + "' >" + i + "</option>");
            tmpYear1 = i;
        }

        $('#ddlYear').append("<option value='9999'>▼</option>");

        $("#ddlYear").val(tmpYear1);
    }

    if ($('#ddlYear').val() == "9999") {
        TargetYear = Number(TargetYear) + 10;
        $('#ddlYear').empty();

        $('#ddlYear').append("<option value='0000'>▲</option>");

        for (var i = TargetYear; i < TargetYear + 10; i++) {
            $('#ddlYear').append("<option value='" + i + "' >" + i + "</option>");
        }

        $('#ddlYear').append("<option value='9999'>▼</option>");

        $("#ddlYear").val(TargetYear);
    }

    var tmpYear = $('#ddlYear').val();
    var tmpMonth = $('#ddlMonth').val();

    tmpMonth = tmpMonth < 10 ? "0" + tmpMonth : tmpMonth;

    TmpDate = tmpYear + "-" + tmpMonth;

    $("#timeList").empty();
    var tmpTime = GetTime(tmpYear + "-" + tmpMonth);
    $("#timeList").html(tmpTime);

    //List.GetList(TmpDate);
    List.GoPageLoad();
}

/******************************************
작성자: famo23
작성일자: 2012-06-05
작성목적: 월 변경 
****************************************/
List.ChangeMonth = function () {

    var tmpYear = $('#ddlYear').val();
    var tmpMonth = $('#ddlMonth').val();

    tmpMonth = tmpMonth < 10 ? "0" + tmpMonth : tmpMonth;

    TmpDate = tmpYear + "-" + tmpMonth;

    $("#timeList").empty();
    var tmpTime = GetTime(tmpYear + "-" + tmpMonth);
    $("#timeList").html(tmpTime);

    //List.GetList(TmpDate);
    List.GoPageLoad();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'tmpDate': TmpDate,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


