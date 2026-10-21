WorkList = {}


$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            WorkList.LoadData("");
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });


        //확인
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();
            WorkList.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            WorkList.Cancel();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            WorkList.Search();
        });

        WorkList.LoadData("");

    });
});



/**************************************************
작성자   : shpark1325
작성일자 : 
작성목적 : 리스트 초기화
***************************************************/
WorkList.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        WorkList.GetPageList();
    }

    setTimeout(function () {
        loadData();
    }, 100);
}

/**********************************************************************************************
작성자 : shpark1325 
작성일자 : 
작성목적 : 문서번호 리스트 가져오기
**********************************************************************************************/
WorkList.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/ETCLegacy/WorkList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("WorkList.ListSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자 : shpark1325
작성일자 : 
작성목적 : 리스트 가져오기 성공시, 완료
**********************************************************************************************/
WorkList.ListSuccess = function (data, status) {

    $('#phWorkList').setTemplateURL('/DeskPlusEIP/ETC/Jt/ETCLegacy/JTWorkList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phWorkList').processTemplate(data.d);

    setTimeout(function () {
        WorkList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자 : shpark1325 
작성일자 : 
작성목적 : 선택한 특근내역 가져오기
**********************************************************************************************/
WorkList.GetPageWorkList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/ETCLegacy/WorkList.asmx/GetPageWorkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'WorkID': WorkID
    };

    var obj = eval("WorkList.WorkListSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자 : shpark1325
작성일자 : 
작성목적 : 특근내역 가져오기 성공시, 완료
**********************************************************************************************/
WorkList.WorkListSuccess = function (data, status) {

    //alert(data.d[0].DocumentID + "," + data.d[0].DocNumber + "," + data.d[0].CustomerName + "," + data.d[0].StartDate + "," + data.d[0].EndDate + "," + data.d[0].StartHour + "," + data.d[0].EndHour + "," + data.d[0].Duration + "," + data.d[0].Hour + "," + data.d[0].Reason + "," + data.d[0].ResonDetails);

    $('#BODY_etc_customer', opener.parent.ifrmForm.document).val(data.d[0].CustomerName);
    $('#Reason', opener.parent.ifrmForm.document).val(1);
    $('#BODY_etc_ReasonDetail', opener.parent.ifrmForm.document).text(data.d[0].ResonDetails);
    $('#BODY_etc_duration', opener.parent.ifrmForm.document).val(data.d[0].Duration);
    $('#time1', opener.parent.ifrmForm.document).val(data.d[0].StartHour);
    $('#time2', opener.parent.ifrmForm.document).val(data.d[0].EndHour);
    $('#BODY_etc_hour', opener.parent.ifrmForm.document).text(data.d[0].Hour);

    var arrStartDate = data.d[0].StartDate.split("-");
    var arrEndDate = data.d[0].EndDate.split("-");

    var formatStart = arrStartDate[0] + "년 " + arrStartDate[1] + "월 " + arrStartDate[2] + "일";
    var formatEnd = arrEndDate[0] + "년 " + arrEndDate[1] + "월 " + arrEndDate[2] + "일";

    $('#SPAN_STARTDATE', opener.parent.ifrmForm.document).text(formatStart);
    $('#SPAN_ENDDATE', opener.parent.ifrmForm.document).text("~ " + formatEnd);
    
    $('#companyid', opener.parent.ifrmForm.document).text(opener.parent.CompanyID);
    $('#deptid', opener.parent.ifrmForm.document).text(opener.parent.DeptID);
    $('#pathid', opener.parent.ifrmForm.document).text(opener.PathID);
    $('#userid', opener.parent.ifrmForm.document).text(opener.parent.UserID);
    $('#username', opener.parent.ifrmForm.document).text(opener.parent.UserName);
    $('#title', opener.parent.ifrmForm.document).text(opener.parent.TitleName);
    $('#deptcodeid', opener.parent.ifrmForm.document).text(data.d[0].DeptCodeID);
    $('#startdate', opener.parent.ifrmForm.document).text(data.d[0].StartDate);
    $('#enddate', opener.parent.ifrmForm.document).text(data.d[0].EndDate);
    $('#starthour', opener.parent.ifrmForm.document).text(data.d[0].StartHour);
    $('#endhour', opener.parent.ifrmForm.document).text(data.d[0].EndHour);
    $('#hour', opener.parent.ifrmForm.document).text(data.d[0].Hour);
    $('#duration', opener.parent.ifrmForm.document).text(data.d[0].Duration);

    //부모창 변수에 선택된 팝업창 변수값 저장
    $(opener.parent.document).find("#contentTable > li").remove();

    var BaseDocPopUpIDs = "|" + data.d[0].DocumentID + "^" + data.d[0].DocNumber + ";";
    opener.parent.BaseDocIDs = BaseDocPopUpIDs;
    $('#tbBaseDocIDs', opener.parent.document).val(BaseDocPopUpIDs);

    //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
    opener.parent.BaseDocCnt = 1;

    //부모창 히든박스에 문서번호 넣기 - 개발용
    $(opener.parent.document).find('#tbAddDocID').val(BaseDocPopUpIDs);

    // ROW 그리기
    /*
    var contentHtml = "<tr id='" + data.d[0].DocumentID + "' class='rows' name='" + data.d[0].DocNumber + "'>"
            + "<td class='center'>&nbsp;</td>"
            + "<td>" + data.d[0].DocNumber + "</td>"
            + "<td class='center'>특근신청서</td>"
            + "<td><a onclick=\"Common.View('" + data.d[0].DocumentID + "')\" style='cursor:pointer' >" + TextReverseToXss("특근신청서") + "</a></td>"
            + "<td class='center'>" + opener.parent.UserName + "</td>"
            + "<td class='center'><a onclick=\"Common.View('" + data.d[0].DocumentID + "')\">&nbsp;&nbsp;<img class='BtnView' alt=''></a></td>"
            + "<td class='center'><a onclick=\"Common.Del('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\">&nbsp;&nbsp;<img class='BtnDel' alt=''></a></td>"
            + "<td class='center'><a onclick=\"Common.Up('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\"><img class='BtnUp' alt=''></a>&nbsp;&nbsp;&nbsp;"
            + "<a onclick=\"Common.Down('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\"><img class='BtnDown' alt=''></a></td>"
        + "</tr>";
    */

    var contentHtml = "<li id='" + data.d[0].DocumentID + "' class='rows2' name='" + data.d[0].DocNumber + "'>"
            + "<a title=" + R_Subject + " onclick=\"Common.View('" + data.d[0].DocumentID + "')\">" + TextReverseToXss(data.d[0].Subject) + "</a>&nbsp;<a onclick=\"Common.Del('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\"><i class='fas fa-times-circle' title=" + R_FileDel + "></i></a>&nbsp;"
            + "<a onclick=\"Common.Up('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\"><i class='fas fa-caret-up' title='up'></i></a><a onclick=\"Common.Down('" + data.d[0].DocumentID + "', '" + data.d[0].DocNumber + "')\"><i class='fas fa-caret-down' title='down'></i></a>"
            + "<p>"
            + "<span title=" + R_Sort + ">특근신청서내역</span><span title=" + R_DocNumber + ">" + data.d[0].DocNumber + "</span><span title=" + R_FormName + ">" + data.d[0].FormBase + "</span><span title=" + R_ReportUser + ">" + data.d[0].TitleBase + " " + data.d[0].NameBase + "</span>"
            + "</p>"
        + "</li>";

    $(opener.parent.document).find("#noContent").remove();
    $(opener.parent.document).find("#contentTable").append(contentHtml);

    //문서보기, 삭제, 순서변경 아이콘 처리
    if ($(".BtnView", opener.parent.document) != undefined) { $(".BtnView", opener.parent.document).attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnDel", opener.parent.document) != undefined) { $(".BtnDel", opener.parent.document).attr('src', ImagePath + '/Icon/ico_tree_listDel.gif'); }
    if ($(".BtnUp", opener.parent.document) != undefined) { $(".BtnUp", opener.parent.document).attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown", opener.parent.document) != undefined) { $(".BtnDown", opener.parent.document).attr('src', ImagePath + '/Btn/sort_on.gif'); }


    setTimeout(function () {
        WorkList.Resource();
    }, 100);
}

function TextReverseToXss(arg) {

    arg = arg.replace(/&/gi, "&amp;");
    arg = arg.replace(/"/gi, "&quot;");
    arg = arg.replace(/#/gi, "&#35;");
    arg = arg.replace(/'/gi, "&#39;");
    arg = arg.replace("(", "&#40;");
    arg = arg.replace(")", "&#41;");
    arg = arg.replace(/</gi, "&lt;");
    arg = arg.replace(/>/gi, "&gt;");

    return arg;
}

/**********************************************************************************************
작성자 : shpark1325 
작성일자 : 
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
WorkList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : shpark1325
작성일자 : 
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
WorkList.GetCount = function () {

    var url = "/DeskPlusEIP/ETC/Ws/ETCLegacy/WorkList.asmx/GetListCount";

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WorkList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shpark1325
작성일자 : 
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
WorkList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shpark1325
작성일자 : 
작성목적 : 확인
***************************************************/
WorkList.Confirm = function () {

    if (document.getElementById("tbWL").value != "") {

        WorkID = document.getElementById("hdWorkID").value;

        WorkList.GetPageWorkList();
        
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        
    }
}

/**************************************************
작성자   : shpark1325
작성일자 : 
작성목적 : 취소
***************************************************/
WorkList.Cancel = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자 : shpark1325 
작성일자 : 
작성목적 : 선택한 값 
***************************************************/
WorkList.Choice = function (_docnumber, _workid, _subject, _namebase, _writetime) {

    $("#tbWL").val(_docnumber);
    $("#hdWorkID").val(_workid);
    $("#hdSubject").val(_subject);
    $("#hdNameBase").val(_namebase);
    $("#hdWriteTime").val(_writetime);
}

/**************************************************
작성자 : shpark1325 
작성일자 : 
작성목적 : 더블 클릭
***************************************************/
WorkList.ChoiceClose = function (_docnumber, _workid, _subject, _namebase, _writetime) {

    $("#tbWL").val(_docnumber);
    $("#hdWorkID").val(_workid);
    $("#hdSubject").val(_subject);
    $("#hdNameBase").val(_namebase);
    $("#hdWriteTime").val(_writetime);

    WorkList.Confirm();
}
