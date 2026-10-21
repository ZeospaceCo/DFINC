var GetMember = {};

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            GetMember.LoadData("");
        }
    }

    $(document).ready(function () {

        RecordPerPage = $("#ddlListCount").val();
        
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
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            GetMember.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            GetMember.GoPageListType();
        });

        GetMember.LoadData("");
    });
});

/**************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 리스트 초기화
***************************************************/
GetMember.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadUserInfoData = function () {
        GetMember.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadUserInfoSearchData = function () {
        GetMember.GetCount(); //검색 총 카운트 가져오기
        GetMember.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                GetMember.LoadData("search");
            }
        }

        RecordPerPage = $("#ddlListCount").val();

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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadUserInfoSearchData();
        }
        else {
            loadUserInfoData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetMember.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Common/GetMember.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("GetMember.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
GetMember.SuccessPageList = function (data, status) {
    $('#phGetMember').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Admin/General/Common/JTGetMember.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phGetMember').processTemplate(data.d);
    

    GetMember.Resource();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 확인
**********************************************************************************************/
GetMember.Confirm = function () {
    var selValue = $('input[name="radioUser"]:checked').val()

    if (selValue != "" && selValue != undefined && selValue != null) {
        parent.Mod.UserSetting(selValue);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_CheckUser);
    }
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
GetMember.GoPageListType = function () {
    RecordPerPage = $('#ddlListCount').val();
    GetMember.Search();
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-30
작성목적 : 검색
***************************************************/
GetMember.Search = function () {
    CurrentPage = 1; //1페이지로 이동처리     
    GetMember.LoadData("search");
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
GetMember.Resource = function () {

    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_title")[0].innerHTML = R_TitleName; 
    $("#r_name")[0].innerHTML = R_UserName;
    $("#r_gradeName")[0].innerHTML = R_MemeberGrade;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    /*
    var tmpOrder = Order.replace(" asc", "").replace(" desc", "");
    var colValue = tmpOrder + " asc";

    if (Order == colValue) {
    $("#a_" + tmpOrder).addClass("asc");
    }
    else {
    $("#a_" + tmpOrder).addClass("desc");
    }

    if ($(".IcionOn") != undefined) { $(".IcionOn").attr('src', ImagePath + '/icon/fav_Icon_on.gif'); }
    if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    */
}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-30 
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetMember.GetCount = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Common/GetMember.asmx/GetListCount";
    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetMember.SuccessCount");

    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-30 
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
GetMember.SuccessCount = function (data, status) {

    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
GetMember.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;