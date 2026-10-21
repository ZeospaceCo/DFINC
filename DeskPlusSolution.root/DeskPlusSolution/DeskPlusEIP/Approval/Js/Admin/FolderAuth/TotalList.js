TotalList = {}

$(function () {


    $(document).ready(function () {


        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        TotalList.LoadData();

    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
TotalList.LoadData = function () {


    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        TotalList.GetPageList();
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardData();
    }, 100);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리스트 호출
**********************************************************************************************/
TotalList.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/FolderAuth/List.asmx/GetTotalList";

    ParamArray = { 'type': 'M' };

    var obj = eval("TotalList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
TotalList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/FolderAuth/JTTotalList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    TotalList.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

TotalList.Resource = function () {

    $("#j_targetdept")[0].innerHTML = R_TargetDept;
    $("#j_basename")[0].innerHTML = R_BaseName;
    $("#j_writedate")[0].innerHTML = R_WriteDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}
