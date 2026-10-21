WorkList = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        WorkList.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        WorkList.Close();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        WorkList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        WorkList.nxNum();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    // 문서 리스트 가져오기
    WorkList.getCount();

});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : [닫기]
**********************************************************************************************/
WorkList.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 문서 리스트 갯수 가져오기 (웹서비스 호출)
***************************************************/
WorkList.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/ETC/Ws/GetETC.asmx/GetWorkCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo
    };

    var obj = eval("WorkList.succeededCount");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 문서 리스트 갯수 가져오기 (웹서비스 값 받기)
***************************************************/
WorkList.succeededCount = function (data, status) {

    var tmpVal = data.d;
    var arrVal = tmpVal.split("^");

    PageNo = arrVal[1];

    $('#TotalNum').text(arrVal[0]);
    $('#PageNum').text(arrVal[1]);
    TotalItemCount = arrVal[2];

    WorkList.getList();
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
WorkList.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/ETC/Ws/GetETC.asmx/GetWorkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'TotalCount': TotalItemCount,
        'PageNo': PageNo,
        'PageSize': PageSize
    };

    var obj = eval("WorkList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
WorkList.succeeded = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusMobile/ETC/Jt/JTWorkList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);

    setTimeout(function () {
        WorkList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 다국어 처리
**********************************************************************************************/
WorkList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 이전 페이지 이동
***************************************************/
WorkList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        $('#PageNum').text(PageNo);
        $('#phList').empty();

        WorkList.getList();
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 다음 페이지 이동
***************************************************/
WorkList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        $('#PageNum').text(PageNo);
        $('#phList').empty();

        WorkList.getList();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 선택한 값 
***************************************************/
WorkList.Choice = function (_docnumber, _workid) {

    $("#TargetDocNumber").text(_docnumber);
    $("#hdWorkID").val(_workid);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 선택한 특근내역 가져오기
**********************************************************************************************/
WorkList.GetWorkID = function () {
    var url = "/DeskPlusMobile/ETC/Ws/GetETC.asmx/GetWorkID";

    //파라미터값 배열에 담기
    ParamArray = {
        'WorkID': $("#hdWorkID").val()
    };

    var obj = eval("WorkList.WorkIDSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 특근내역 가져오기 성공시, 완료
**********************************************************************************************/
WorkList.WorkIDSuccess = function (data, status) {

    $('#BODY_etc_customer', parent.ifrmForm.document).val(data.d[0].CustomerName);
    $('#Reason', parent.ifrmForm.document).val(1);
    $('#BODY_etc_ReasonDetail', parent.ifrmForm.document).text(data.d[0].ResonDetails);
    $('#BODY_etc_duration', parent.ifrmForm.document).val(data.d[0].Duration);
    $('#time1', parent.ifrmForm.document).val(data.d[0].StartHour);
    $('#time2', parent.ifrmForm.document).val(data.d[0].EndHour);
    $('#BODY_etc_hour', parent.ifrmForm.document).text(data.d[0].Hour);

    var arrStartDate = data.d[0].StartDate.split("-");
    var arrEndDate = data.d[0].EndDate.split("-");

    var formatStart = arrStartDate[0] + "년 " + arrStartDate[1] + "월 " + arrStartDate[2] + "일";
    var formatEnd = arrEndDate[0] + "년 " + arrEndDate[1] + "월 " + arrEndDate[2] + "일";

    $('#SPAN_STARTDATE', parent.ifrmForm.document).text(formatStart);
    $('#SPAN_ENDDATE', parent.ifrmForm.document).text("~ " + formatEnd);

    $('#companyid', parent.ifrmForm.document).text(parent.ifrmProcessing.CompanyID);
    $('#deptid', parent.ifrmForm.document).text(parent.ifrmProcessing.DeptID);
    $('#pathid', parent.ifrmForm.document).text(parent.ifrmProcessing.PathID);
    $('#userid', parent.ifrmForm.document).text(parent.ifrmProcessing.UserID);
    $('#username', parent.ifrmForm.document).text(parent.ifrmProcessing.UserName);
    $('#title', parent.ifrmForm.document).text(parent.ifrmProcessing.Title);
    $('#deptcodeid', parent.ifrmForm.document).text(data.d[0].DeptCodeID);
    $('#startdate', parent.ifrmForm.document).text(data.d[0].StartDate);
    $('#enddate', parent.ifrmForm.document).text(data.d[0].EndDate);
    $('#starthour', parent.ifrmForm.document).text(data.d[0].StartHour);
    $('#endhour', parent.ifrmForm.document).text(data.d[0].EndHour);
    $('#hour', parent.ifrmForm.document).text(data.d[0].Hour);
    $('#duration', parent.ifrmForm.document).text(data.d[0].Duration);

    var baseDocHTML = "<ul class='mli7' id='" + data.d[0].DocumentID + "'><li class='contentblock'><span class='base'>"
             + "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif'onclick=\"BaseDel('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\" style='cursor:pointer' /></span>"
             + "<div class='conBox'><span class='LNoready'>특근신청서</span><br />"
             + "<span class='sch_user'>" + data.d[0].DocNumber + "</span></div></li></ul>";

    var BaseDocPopUpIDs = "|" + data.d[0].DocumentID + "^" + data.d[0].DocNumber + ";";
    parent.BaseDocIDs = BaseDocPopUpIDs;
    $('#tbBaseDocIDs', parent.document).val(BaseDocPopUpIDs);

    //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
    parent.BaseDocCnt = 1;

    //부모창 히든박스에 문서번호 넣기 - 개발용
    $(parent.document).find('#tbAddDocID').val(BaseDocPopUpIDs);

    //근거문서 타이틀
    $(parent.document).find("#uBase").css('display', '');

    // ROW 그리기
    $(parent.document).find("#baseList").html(baseDocHTML);

    WorkList.Close();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-22
작성목적 : 저장
***************************************************/
WorkList.Confirm = function () {

    if ($("#TargetDocNumber").text() != "") {

        WorkList.GetWorkID();
    }
    else {
        parent.action_popup.alert("근거문서를 선택해주세요");
        return;
    }
}