PopDraft = {}



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
            PopDraft.LoadData();
        }
    }

    $(document).ready(function () {

        //저장혹은 Update
        $('#CbtnSave').click(function (e) {
            e.preventDefault();
            PopDraft.Save();
        });

        //불러오기
        $('#CbtnLoad').click(function (e) {
            e.preventDefault();
            PopDraft.Load();
        });

        //삭제
        $('#CbtnDelete').click(function (e) {
            e.preventDefault();
            PopDraft.Delete();
        });  
                              

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });



        PopDraft.LoadData();
        //        $("#textarea").select(); 검색초기 UI 포커스
    });
});


PopDraft.Delete = function () {
    if (SelDraftID == "") {
        alert("삭제할 데이터를 선택 해주세요.");
        return;
    }


    ParamArray = {
        'DraftID': SelDraftID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/Del";

    var obj = eval("PopDraft.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);

    SelDraftID = "";
    $("#tbDraftName").val("");

    PopDraft.LoadData();

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}



PopDraft.Load = function () {
    if (SelDraftID == "") {
        alert("불러올 데이터를 선택 해주세요.");
        return;
    }


    ParamArray = {
        'DraftID': SelDraftID
        , 'UserID': UserID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/Load";

    var obj = eval("PopDraft.SuccessConfirm");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);

    parent.AddMaster.ExReload();

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }


}



/**************************************************
작성자   : seo
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
PopDraft.LoadData = function () {

    //PopDraft.GetCount(); //검색 총 카운트 가져오기    


    //    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    //    $("#pageInfo").text(CurrentPage + " / " + pageCount);


    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {

        PopDraft.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        //        var Pager = {
        //            pageIndexChanged: function (selectedPage) {
        //                CurrentPage = selectedPage;
        //                PopDraft.LoadData();
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



PopDraft.Save = function () {

    if (CheckVal == "") {
        alert("비용 정산창에서 임시저장 할 항목을 선택 해주세요.");
        return false;
    }

    // 공통적으로 선택 된 데이터가 카드데이터이면 안됨
    if (IsCard != "") {
        alert("선택 된 데이터에 카드항목이 포함되어 있습니다.\n벤더항목만이 임시저장 가능 합니다.");
        return false;
    }

    if ($("#tbDraftName").val() == "") {
        alert("임시저장명을 입력해 주세요.");
        return false;
    }

    var DraftName = $("#tbDraftName").val();

    // 기존것에 대한 수정 저장 

    if (SelDraftID != "") {
        // 수정 저장인 경우 기존게 날라감으로 다시 한번 물어봄
        if (confirm("기존 임시저장 데이터를 현재 선택 된 데이터로 수정 저장 하시겠습니까?")) {
            ParamArray = {
             'CheckVal': CheckVal
            , 'DraftID': SelDraftID
            , 'DraftName': DraftName
            };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/AddMod";

            var obj = eval("PopDraft.SuccessConfirm");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);

            SelDraftID = "";
            $("#tbDraftName").val("");
        }
    }
    else { // 그냥 저장
        ParamArray = {
            'UserID': UserID
        , 'CheckVal': CheckVal
         , 'DraftName': DraftName
        };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/Add";

        var obj = eval("PopDraft.SuccessConfirm");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }

    PopDraft.LoadData();


}



PopDraft.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        $("#cbtnCardNM").val("");
        $("#cbtnCardNumber").val("");
        $("#cbtnCardOwner").val("");
        CardInfo.GetList();
    }
}



/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
PopDraft.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/GetPageList";

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'UserID': UserID
    };

    var obj = eval("PopDraft.SuccessPageList");

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
PopDraft.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Customize/User/Popup/JTPopDraft.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    PopDraft.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '280px', 'height': '280px' });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
//PopDraft.PageSize = function () {
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
PopDraft.Resource = function () {
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
PopDraft.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Customize/User/Popup/PopDraft.asmx/GetCount";

    ParamArray = {
        'searchField': '',
        'searchValue': ''
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PopDraft.SuccessCount");
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
PopDraft.SuccessCount = function (data, status) {

    TotalItemCount = data.d;

}


/**********************************************************************************************
작성자   : seo
작성일자 : 2011-04-18
**********************************************************************************************/
PopDraft.Binding = function (_code, _name) {

    $("#tbDraftName").val(_name);

    SelDraftID = _code;

}


