


/******************************************************************

테스트용으로 3.3 소스 가져다 놓음 개발시 4.0 형식으로 수정 필요

******************************************************************/





/*--------------------------------------------------------------------------------------------------
// [달력버튼]
--------------------------------------------------------------------------------------------------*/
var objDate;
function SelectDateCode(arg) {
    objDate = arg;
    var startDate = $(objDate).value;

    url = "../../Pages/Common/Calendar/CalendarDialog.aspx?func=GetDate&startDate=" + startDate;
    window.open(url, "Calendar", "toolbar=no,width=780,height=380,directories=no,status=no,scrollbars=no,resize=no,menubar=no");
}

function GetDate(hdYear, hdMonth, hdDay) {
    $(objDate).value = hdYear + "-" + hdMonth + "-" + hdDay;
}


/*--------------------------------------------------------------------------------------------------
//     Description   : 문서 검색/첨부파일 검색 텝이 누를 때 마다 변수 설정하고 검색 호출
-----------------------------------------------------------------------------*/
function searchSubTab(num, w) {

    sendData.w.value = w; //검색분야
    SubmitProcess(num, w, "");
}



/*---------------------------------------------------------------------------
Description   : 날짜의 선택에서 기간을 선택하면
날짜의 직접입력 텍스트 박스에 날짜를 넣어준다.
-----------------------------------------------------------------------------*/
function date_select(type) {
    var today = new Date();
    var today_year = today.getYear();
    var today_month = today.getMonth() + 1;
    var today_date = today.getDate();



    var from;
    var to;

    if (type == 1) {//1주일
        var year = today_year;
        var month = today_month;
        var date = today_date - 7;

        var month_len = [31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        month_len[1] = (((today_year % 100 != 0) && (today_year % 4 == 0)) || (today_year % 400 == 0)) ? 29 : 28;

        if (date <= 0) {
            if (month == 1) {
                date = month_len[11] + date;
                month = 12;
                year = year - 1;
            }
            else {
                date = month_len[month - 2] + date;
                month = month - 1;
            }
        }

        from = year + "-" + padZero(month, 2) + "-" + padZero(date, 2);
        to = today_year + "-" + padZero(today_month, 2) + "-" + padZero(today_date, 2);
    }
    else if (type == 2) {//1개월
        var year = today_year;
        var month = today_month - 1;
        if (month == 0) {
            month = 12;
            year = year - 1;
        }

        from = year + "-" + padZero(month, 2) + "-" + padZero(today_date, 2);
        to = today_year + "-" + padZero(today_month, 2) + "-" + padZero(today_date, 2);
    }
    else if (type == 3) {//6개월
        var year = today_year;
        var month = today_month - 6;
        if (month == 0) {
            month = 12;
            year = year - 1;
        }
        else if (month < 0) {
            month = month + 12;
            year = year - 1;
        }

        from = year + "-" + padZero(month, 2) + "-" + padZero(today_date, 2);
        to = today_year + "-" + padZero(today_month, 2) + "-" + padZero(today_date, 2);
    }
    else if (type == 4) {//1년
        var year = today_year - 1;

        from = year + "-" + padZero(today_month, 2) + "-" + padZero(today_date, 2);
        to = today_year + "-" + padZero(today_month, 2) + "-" + padZero(today_date, 2);
    }

    document.queryForm.startDate.value = from;
    document.queryForm.endDate.value = to;
}

/*---------------------------------------------------------------------------
Description   : 주어진 leng 길이만큼 num의 앞에 0을 채워준다.
-----------------------------------------------------------------------------*/
function padZero(num, leng) {
    var zero = leng - ("" + num).length;
    if (typeof (num) == "number" && zero > 0) {
        var tmp = "";
        for (var i = 0; i < zero; i++) tmp += "0";
        return tmp + num;
    } else return num;
}


/*---------------------------------------------------------------------------
Description   : 날짜의 선택에서 '전체' 라디오버튼 클릭하면
날짜의 직접입력 텍스트 박스의 내용을 비운다.
-----------------------------------------------------------------------------*/
function date_select_all() {
    document.queryForm.startDate.value = "";
    document.queryForm.endDate.value = "";
}


/*---------------------------------------------------------------------------
Description   : 검색 옵션 체크
-----------------------------------------------------------------------------*/


/*---------------------------------------------------------------------------
Description   : 처리하기
-----------------------------------------------------------------------------*/
function SendPage(num) {
    SubmitProcess(1, "", "");
}


function SubmitProcess(num, w, q) {
    var searchInResult = $("cbxSearchInResult").checked;
    sendData.bqValue.value = SearchValue();

    sendData.bq.value = "0";
    if (searchInResult) {
        sendData.bq.value = "1";
    }

    if (q == "") {
        sendData.q.value = queryForm.tbQuery.value; //검색어
    }
    else {
        sendData.bqValue.value = q;
        sendData.q.value = q; //검색어
    }

    //alert(sendData.bqValue.value);

    sendData.date.value = SelectCheck(queryForm.date);

    if (sendData.q.value == "") {
        alert(R_InputSearchEngine);
        return;
    }

    if (w == "") {
        sendData.w.value = SelectCheck(queryForm.rblSearchTarget); //검색분야
    }
    else {
        sendData.w.value = w; //검색분야
    }

    if (sendData.w.value == "") {
        alert(R_CheckSearchSection);
        return false;
    }

    sendData.outmax.value = SelectCheck(queryForm.count); //결과수	
    var sortValue = DropCheck(); //정렬방법



    sendData.sort.value = "";
    sendData.fsort.value = "";
    sendData.dsort.value = "";

    if (sortValue == "") {
        alert(R_InputOrderWay);
        return false;
    }
    else if (sortValue == "ACCURACY") {
        sendData.sort.value = "11";
    }
    else if (sortValue == "DATE") {
        sendData.dsort.value = "11";
    }
    else if (sortValue == "TITLE") {
        sendData.fsort.value = "11";
    }

    var startDate = queryForm.startDate.value;
    var endDate = queryForm.endDate.value;
    startDate = startDate.replace("-", "");
    startDate = startDate.replace("-", "");
    endDate = endDate.replace("-", "");
    endDate = endDate.replace("-", "");

    //전체일경우
    if (startDate != "" && endDate != "") {
        sendData.d.value = startDate + "~" + endDate;
    }

    sendData.pn.value = num; //페이징 번호

    sendData.action = "SearchContent.aspx";
    sendData.submit();

}


//검색 설정 내 세션이름 가져오기
function SelectCheck(obj) {
    var rtnValue = "";
    for (i = 0; i <= obj.length - 1; i++) {
        if (obj[i].checked == true) {
            rtnValue = obj[i].value;
        }
    }

    return rtnValue;
}

//검색 설정 내 세션이름 가져오기
function DropCheck() {
    var e = queryForm.sortField;
    var rtnValue = e.options[e.selectedIndex].value;
    return rtnValue;
}

function PageLink(num) {
    SubmitProcess(num, "", "");
}

function checkAdd(evt, theForm) {
    if (evt.keyCode == 13)
        return false;
}


function GoPage(num, w) {
    sendData.w.value = w; //검색분야
    SubmitProcess(num, w, "");
}


function init() {
    for (var i = 0; i < 5; i++) {
        if (i == DateSelect) {
            queryForm.date[i].checked = true;
        }
    }

    if (QueryW != "") {
        SubmitProcess('1', 'total', QueryW);
    }


}

function SearchValue() {
    var searchInResult = $("cbxSearchInResult").checked;
    var q = queryForm.tbQuery.value;

    if (searchInResult) {
        var bqValue = queryForm.tbSearchValue.value;
        return bqValue + " & " + q;
    }
    else {
        return q;
    }
}

function ClickSearchInResult() {
    var searchInResult = $("cbxSearchInResult").checked;
    if (searchInResult) {
        queryForm.tbQuery.value = "";
        queryForm.tbQuery.focus();
    }

}


/*---------------------------------------------------------------------------
Description   : 그룹웨어, 메일 페이지 호출
-----------------------------------------------------------------------------*/
function open_document(docid, gubun, catid, groupID, listOrder, depth) {
    var random = Math.round(Math.random() * 1000) + 10;
    var random1 = Math.round(Math.random() * 1000) + 100;

    var width = "1024";
    var height = "750";
    var left = Math.ceil((window.screen.availWidth - width) / 2);
    var top = Math.ceil((window.screen.availHeight - height) / 2);

    //전자결재				
    if (gubun == "1") {
        //        window.open("http://hdems.hyundam.com/DeskPlusEIP/Site/SiteBase/Pages/Approval/View/ViewDocumentByMail.aspx?" + docid,
        //							    random,
        //							    "width=800,height=600,top=0,left=0,scrollbars=1");

        window.open("http://hdems.hyundam.com/DeskPlusEIP/Site/SiteBase/Pages/Approval/View/ViewDocumentByMail.aspx?uID=" + random + "&baseDoc=Y&cID=1&&oID=" + random1 + "&dID=" + docid,
							    random,
							    "toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes, width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);

    }
    else if (gubun == "2") //메일
    {

        //http://210.102.151.43:4040/zmail/login.jsp?cmd=readmail_popup&mode=1100&message_sn=-28&id=deskplus@mail2.shingu.ac.kr
        //dev-group.shingu.ac.kr

        window.open("http://hdeps.hyundam.com/zmail/login.jsp?cmd=readmail_popup&mode=1100&message_sn=" + docid + "&id=" + catid,
							    "메일",
							    "width=800,height=600,top=0,left=0,scrollbars=1");

    }
    else if (gubun == "3") //게시판
    {

        window.open("http://hdems.hyundam.com/DeskPlusEIP/Board/PopUp/DefaultNotice.aspx?catID=" + catid + "&listID=" + docid + "&categoryID=" + catid + "&groupID=" + groupID + "&listOrder=" + listOrder + "&depth=" + depth + "&name=", "게시판", "width=800,height=600,top=0,left=0,scrollbars=1");
        //window.open("http://hdems.hyundam.com/DeskPlusEIP/Board/PopUp/DefaultNotice.aspx?listID="+docid+"&categoryID=" + catid,"게시판","width=800,height=600,top=0,left=0,scrollbars=1");

    }
}


//메일 오픈
function client() {
    window.open('', 'client', 'history=no,,status=no,scrollbars=yes,menuebar=no, top=50, left=50, width=780, height=580');
}