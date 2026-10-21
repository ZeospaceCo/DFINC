List = {}

$(document).ready(function () {


    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $(this).html(loadingPath);

    setTimeout(function () {

        List.GetPageList();

    }, 1);
    

});



/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 페이지 가져오기
***************************************************/
List.GetPageList = function () {    

    //게시판 리스트
    var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/List.asmx/GetBoardList";

    ParamArray = {
        'pageSize': RecordPerPage,     // 리스트 행 개수
        'pageSizeNo': CurrentPage,      // 페이지Nuumber
        'totalCount': TotalItemCount, // 전체 페이지 카운트
        'categoryID': CatID  // 카테고리 아이디
    };

    CallEncrypt(CallGetParam());
    var paramData = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: urlPage,
        async: false,
        data: paramData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#plList').setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTListIframe.htm?seq=" + Guids(), null, { filter_data: false });
            $('#plList').processTemplate(data.d);

            if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
            if ($(".TitleIcon") != undefined) { $(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif'); }

        }
        ,
        error: function (data, status, err) {
            alert(R_ErrorConnection + data);
        }
    });
}

/**************************************************
작성자: shimpark
작성일:2012-02-07
작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
***************************************************/
List.PageChange = function (arg) {
        
    // MAX페이지 가져오기
    var boardList_MaxCount = 1;
    if (TotalItemCount > 0) {

        // 몫
        var mok = parseInt((TotalItemCount / RecordPerPage));
        // 나머지
        var balance = TotalItemCount % RecordPerPage;

        if (balance != 0) {
            boardList_MaxCount = mok + 1;
        }
        else {
            boardList_MaxCount = mok;
        }
    }

    if (arg == "next") {
        CurrentPage = parseInt(CurrentPage) + 1;
    }
    else if (arg == "before") {
        CurrentPage = parseInt(CurrentPage) - 1;
    }

    // 1페이지에서 이전을 누를 때
    if (CurrentPage < 1) {
        CurrentPage = 1;

        alert(R_FirstPage);
        return;
    }

    // 최종페이지 에서 이후버튼 누를 때 
    if (CurrentPage > boardList_MaxCount) {
        CurrentPage = boardList_MaxCount;

        alert(R_LastPage);
        return;
    }

    List.GetPageList();
}