PopDepartMent = {}



/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            PopDepartMent.LoadData();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        //        $("#pager").pager({
        //            pager: "#pager",
        //            pagerMode: "text", //text, image 중 선택
        //            totalItemCount: TotalItemCount,
        //            recordPerPage: RecordPerPage,
        //            currentPage: CurrentPage,
        //            pageIndexChanged: Pager.pageIndexChanged,
        //            prev10Text: R_Previous,
        //            next10Text: R_Next,
        //            prev10ImgSrc: "images/Prev10.gif",
        //            next10ImgSrc: "images/next10.gif",
        //            selectedPageFontColor: "orange"
        //        });

        //        //페이지 카운트
        //        $('#ddlListCount').change(function () {
        //            PopDepartMent.PageSize();
        //        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //        //입력값 체크하기
        //        if (navigator.appName == 'Microsoft Internet Explorer') {
        //            $('.charsRemaining').text($('#textarea').attr('maxlength'));
        //            $('#textarea').bind('keyup', function () {
        //                $(this).css('border', '1px solid #d2d2d2');
        //                var $this = $(this);
        //                var len = $this.val().length;
        //                var maxlength = $this.attr('maxlength')

        //                $('.charsRemaining').text(maxlength - len);
        //                if (maxlength && len > maxlength) {
        //                    $(this).css('border', '1px solid #f00');
        //                    alert(maxlength + " limited");
        //                    $this.val($this.val().slice(0, maxlength));
        //                    $('.charsRemaining').text("0");
        //                    return;
        //                }
        //            });
        //        }
        //        else {
        //            PopDepartMent.MaxLang();
        //        }

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                PopDepartMent.GetPageList();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            PopDepartMent.GetPageList();
        });


        PopDepartMent.LoadData();
        //        $("#textarea").select(); 검색초기 UI 포커스

        $("#ctbSearch").select(); //검색초기 UI 포커스

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

///**************************************************
//작성자   : seo
//작성일자 : 2011-04-11
//작성목적 : 글자 제한
//***************************************************/
//PopDepartMent.MaxLang = function () {

//    var onEditCallback = function (remaining) {
//        //$(this).siblings('.charsRemaining').text("Characters remaining: " + remaining);
//        $('.charsRemaining').text(remaining);

//        if (remaining > 0) {
//            $(this).css('border', '1px solid #d2d2d2');
//        }
//    }

//    var onLimitCallback = function () {
//        $(this).css('border', '1px solid #f00');
//    }

//    $('textarea[maxlength]').limitMaxlength({
//        onEdit: onEditCallback,
//        onLimit: onLimitCallback
//    });
//}

/**************************************************
작성자   : seo
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
PopDepartMent.LoadData = function () {

    //PopDepartMent.GetCount(); //검색 총 카운트 가져오기    


    //    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    //    $("#pageInfo").text(CurrentPage + " / " + pageCount);


    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {

        PopDepartMent.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        //        var Pager = {
        //            pageIndexChanged: function (selectedPage) {
        //                CurrentPage = selectedPage;
        //                PopDepartMent.LoadData();
        //            }
        //        }

        //        // 페이저 초기화
        //        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        //        $("#pager").pager({
        //            pager: "#pager",
        //            pagerMode: "text", //text, image 중 선택
        //            totalItemCount: TotalItemCount,
        //            recordPerPage: RecordPerPage,
        //            currentPage: CurrentPage,
        //            pageIndexChanged: Pager.pageIndexChanged,
        //            prev10Text: R_Previous,
        //            next10Text: R_Next,
        //            prev10ImgSrc: "images/Prev10.gif",
        //            next10ImgSrc: "images/next10.gif",
        //            selectedPageFontColor: "orange"
        //        });
    }

    setTimeout(function () {
        loadBoardSearchData();
    }, 100);
}




/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
PopDepartMent.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDepartMent.asmx/GetPageList";

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'vendorCD': VendorCD
    };

    var obj = eval("PopDepartMent.SuccessPageList");

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
PopDepartMent.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Customize/User/Popup/JTPopDepartMent.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    PopDepartMent.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '375px', 'height': '375px' });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
//PopDepartMent.PageSize = function () {
//    var iPageUrl = $.url(PageUrl);
//    var iPath = iPageUrl.attr('path');

//    ParamArray = {
//        'categoryID': CategoryID,
//        'listID': ListID,
//        'recordPerPage': $('#ddlListCount').val()
//    };

//    CallEncrypt(CallGetParam());

//    //이동하기    
//    location = iPath + "?p=" + GetEncryptValue;
//}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
PopDepartMent.Resource = function () {
    //    $("#j_Name")[0].innerHTML = R_Name;
    //    $("#j_Content")[0].innerHTML = R_Contents;
    //    $("#j_RegDate")[0].innerHTML = R_RegDate;

    //    if ($(".r_mod") != undefined) {
    //        $(".r_mod").text(R_Modify);
    //    }

    //    if ($(".r_del") != undefined) {
    //        $(".r_del").text(R_Del);
    //    }

    //    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    //    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
PopDepartMent.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDepartMent.asmx/GetCount";

    ParamArray = {
        'searchField': '',
        'searchValue': ''
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PopDepartMent.SuccessCount");
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
PopDepartMent.SuccessCount = function (data, status) {

    TotalItemCount = data.d;

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-18
**********************************************************************************************/
PopDepartMent.Binding = function (_code, _name) {

    if (IsAdmin != "") {
        parent.ListAdmin.DepartMentBind(_code, _name);
    }
    else {
        if (MasterDetailGubun == "M") {
            parent.AddMaster.DepartMentBind(_code, _name);
        }
        else {
            parent.AddMaster.DepartMentBindEx(_code, _name);
        }
    }

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}


