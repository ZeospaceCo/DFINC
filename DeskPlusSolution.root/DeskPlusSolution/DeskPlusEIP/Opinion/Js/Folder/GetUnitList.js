GetUnitList = {}

$(function () {

    $(document).ready(function () {

        //[확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); GetUnitList.Confirm(); });

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        GetUnitList.LoadData();     //페이지가져오기
    });
});


/***리스트 처리 및 페이징 시작***/

//리스트내용가져오기
GetUnitList.GetPageList = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Folder/GetUnitList.asmx/GetUnitListByQuesID";

    //파라미터값 배열에 담기
    ParamArray = {
        'totCnt': TotCnt
        , 'quesID' : QuesID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': ''
        , 'sVal': ''
        , 'order': Order
    };

    var obj = eval("GetUnitList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
GetUnitList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Folder/JtUnitList.htm?seq=' + Guids(), null, { filter_data: false });

    $('#placeholder').processTemplate(data.d);
    //$('#loading').html("");

    GetUnitList.Resource();    //다국어처리
}

//리스트총개수가져오기
GetUnitList.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Folder/GetUnitList.asmx/GetUnitListByQuesIDCnt";
    ParamArray = { 'quesID': QuesID, 'sKey': '', 'sVal': '' };

    var obj = eval("GetUnitList.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
GetUnitList.SuccessPageCnt = function (data, status) {
    TotCnt = data.d;
    var pageCount = $("#pager").pager.refresh(TotCnt, PageSize);
    //$("#pageInfo").text(PageNo + " / " + pageCount);
}

//리스트가져오기
GetUnitList.LoadData = function () {
    //var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    //$('#loading').html(loadingPath);

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (sPage) {
            PageNo = sPage;
            GetUnitList.LoadData();
        }
    }

    // 페이저 초기화 (실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.)
    $("#pager").pager({
        pager: "#pager",        //pageID
        pagerMode: "text",      //text or image
        totalItemCount: TotCnt, //총카운트
        recordPerPage: PageSize, //페이지 보여질 개수
        currentPage: PageNo,    //페이지번호(1부터시작)
        pageIndexChanged: Pager.pageIndexChanged,   //event
        prev10Text: "Before", //이전
        next10Text: "After",     //다음
        prev10ImgSrc: "images/Prev10.gif",
        next10ImgSrc: "images/next10.gif",  //"<img src='" + ImagePath + "/Icon/loading.gif'/>";
        selectedPageFontColor: "orange"
    });

    setTimeout(function () { GetUnitList.GetPageList(); }, 100);
}
/***리스트 처리 및 페이징 끝***/


//정렬
GetUnitList.Sort = function (col) {
    var colValue = col + " asc";
    if (Order == colValue) { colValue = col + " desc"; }    //메뉴를 동일한 필드에서 두번이상 선택했을 경우

    Order = colValue;
    PageNo = 1;
    GetUnitList.LoadData();
}

//전체 체크박스
GetUnitList.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

//체크박스 선택된 값 가져오기
GetUnitList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

//[확인]
GetUnitList.Confirm = function () {
    var selVals = GetUnitList.CheckedValue();
    if (selVals == "") { alert(R_Msg_Unit); return; }

    if (window.confirm(R_Msg_Add)) {
        if (opener.UnitAddWs) {
            opener.UnitAddWs(selVals);
            setTimeout(function () { GetUnitList.GetPageCnt(); }, 100);     //총카운트
            setTimeout(function () { GetUnitList.LoadData(); }, 100);       //리스트
            //self.close();
        } else {
            alert(R_Msg_Admin);
        }
    }
}

//다국어처리
GetUnitList.Resource = function () {
    $("#jtUL_F1").text(R_JTF1);
    $("#jtUL_F2").text(R_JTF2);
    $("#jtUL_F3").text(R_JTF3);
    $(".jtNoData").text(R_JTNoData);
}