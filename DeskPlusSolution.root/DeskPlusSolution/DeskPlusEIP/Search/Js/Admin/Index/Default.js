Default = {}

var timerClose = "N";
var subintervalID = 0;
var checkedVal = "";
/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        // 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Default.Del();
        });

        // 최적화
        $('#cbtnOptimize').click(function (e) {
            e.preventDefault();
            Default.Optimize();
        });

        //검색입력필드 엔터 이벤트 기술
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Default.Add();
        });

        Default.LoadData();
        //Default.LoadDataDetail();
    });
});

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 리스트 초기화
***************************************************/
Default.LoadData = function () {
    //var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    //$('.loading').html(loadingPath);
    checkedVal = Default.CheckedValue(); //선택된 분류 저장

    //Index Log
    setTimeout(function () {
        var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/GetCategory"

        //파라미터값 배열에 담기
        ParamArray = {
        };

        var obj = eval("Default.SuccessCategory");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }, 100);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessCategory = function (data, status) {
    $('#phCategory').setTemplateURL('/DeskPlusEIP/Search/Jt/Admin/Index/JTCategory.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCategory').processTemplate(data.d);
    //$('.loading').html("");

    //신규 추가시 상세 로그값을 가져오기위함
    if (checkedVal != "") {
        for (i = 0; i < checkedVal.length; i++) {
            $('input:checkbox[value=' + checkedVal[i] + ']').attr("checked", true);
        }
    }

    checkedVal = "";
    Default.Resource();
    Default.LoadDataDetail();
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 리스트 초기화
***************************************************/
Default.LoadDataDetail = function (id) {
    var selID;
    if (id != "") {
        selID = id;
    } else {
        selID = Default.CheckedID();
    }

    //Index Detail Log
    setTimeout(function () {
        var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/GetLogDetail"

        //파라미터값 배열에 담기
        ParamArray = {
            'logID': selID
        };

        var obj = eval("Default.SuccessLogDetail");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }, 100);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessLogDetail = function (data, status) {
    $('#phLogDetail').setTemplateURL('/DeskPlusEIP/Search/Jt/Admin/Index/JTLogDetail.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phLogDetail').processTemplate(data.d);
    //$('.loading').html("");

    $("#r_message")[0].innerHTML = R_Message;
    $("#r_writedate")[0].innerHTML = R_ExecTime;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Default.Resource = function () {
    $("#r_category")[0].innerHTML = R_Category;
    $("#r_status")[0].innerHTML = R_Status;
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_exectime")[0].innerHTML = R_ExecTime;
    $("#r_ftrtextcount")[0].innerHTML = R_FTRTextCount;
    $("#r_indexcount")[0].innerHTML = R_IndexCount;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    $(".r_p").text(R_P);
    $(".r_c").text(R_C);
    $(".r_e").text(R_E);
    $(".r_d").text(R_D);
    $(".r_n").text(R_N);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 전체 체크박스
**********************************************************************************************/
Default.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 선택된 값 가져오기
***************************************************/
Default.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 선택된 ID 가져오기
***************************************************/
Default.CheckedID = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).attr("id");
    }).get();
    return allVals;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 인덱스 생성  (웹서비스 호출)
**********************************************************************************************/
Default.Add = function () {
    var selValue = Default.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmExec)) {
            Default.SetLoading(true);

            //            timerClose = "N"
            //            startProgress();

            //파라미터값 배열에 담기
            ParamArray = {
                'f': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/Add";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Default.SuccessAdd");

            AjaxAsyncService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/*
function startProgress() {
if (timerClose == "N") {
Default.LoadDataDetail();
subintervalID = setTimeout("startProgress()", 5000);
}
else {
clearTimeout(subintervalID);
Default.SetLoading(false);
}
}*/

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 인덱스 추가  (웹서비스 값 받기)
***************************************************/
Default.SuccessAdd = function (data, status) {
    timerClose = "Y";
    if (data.d) {
        Default.LoadData();

        //Default.LoadDataDetail();
    }
    else {
        Default.LoadData();
    }

    Default.SetLoading(false);
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 색인 결과  (웹서비스 값 받기)
***************************************************/
Default.AllRecord = function () {
    var selValue = Default.CheckedValue();

    if (selValue != "") {
        Default.SetLoading(true);

        //            timerClose = "N"
        //            startProgress();

        //파라미터값 배열에 담기
        ParamArray = {
            'f': selValue
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/GetAllRecord";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Default.SuccessAllRecord");

        AjaxAsyncService(url, data, obj);
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 인덱스 추가  (웹서비스 값 받기)
***************************************************/
Default.SuccessAllRecord = function (data, status) {
    if (data.d) {
        $('#phLogDetail').setTemplateURL('/DeskPlusEIP/Search/Jt/Admin/Index/JTAllRecord.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phLogDetail').processTemplate(data.d);

        if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    }
    else {
    }

    Default.SetLoading(false);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 최적화  (웹서비스 호출)
**********************************************************************************************/
Default.Optimize = function () {
    var selValue = Default.CheckedValue();
    var selID = Default.CheckedID();

    if (selValue != "") {
        if (selID != "") {
            for (i = 0; i < selID.length; i++) {
                if ($("#Status_" + selID).val() != "C") {
                    alert(R_ComfirmComplete);
                    return;
                }
            }
        }

        if (window.confirm(R_ConfirmOpt)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'f': selValue,
                'i': selID
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/Optimize";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Default.SuccessOptimize");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 최적화 처리  (웹서비스 값 받기)
***************************************************/
Default.SuccessOptimize = function (data, status) {
    if (data.d == true) {
        Default.LoadData();
        //Default.LoadDataDetail();
    }
    else {
        Default.LoadData();
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 인덱스 삭제  (웹서비스 호출)
**********************************************************************************************/
Default.Del = function () {
    var selValue = Default.CheckedValue();
    var selID = Default.CheckedID();

    if (selValue != "") {
        if (selID != "") {
            for (i = 0; i < selID.length; i++) {
                if ($("#Status_" + selID).val() != "C") {
                    alert(R_ComfirmComplete);
                    return;
                }
            }
        }

        if (window.confirm(R_ConfirmDel)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'f': selValue,
                'i': selID
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Search/Ws/Admin/Index/Default.asmx/Del";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Default.SuccessDel");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-22
작성목적 : 삭제 불가 처리  (웹서비스 값 받기)
***************************************************/
Default.SuccessDel = function (data, status) {
    if (data.d == true) {
        Default.LoadData();
        //Default.LoadDataDetail();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : [로딩페이지]
***************************************************/
Default.SetLoading = function (_isComplete) {
    var arr = $("#divLoading");
    if (arr == null) return;

    $.each(arr, function () {
        if (_isComplete) {
            arr.fadeIn(500); //350
        } else {
            arr.fadeOut(1000);
        }
    });
}