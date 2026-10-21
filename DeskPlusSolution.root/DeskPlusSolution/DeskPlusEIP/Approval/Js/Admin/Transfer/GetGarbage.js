GetGarbage = {}


$(function () {


    $(document).ready(function () {

        //취소
        $('#cbtnClose').click(function (e) { e.preventDefault(); GetGarbage.Close(); });

        GetGarbage.LoadData();

    });
});


/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
GetGarbage.LoadData = function () {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        GetGarbage.GetPageList();
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardData();
    }, 100);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 호출
**********************************************************************************************/
GetGarbage.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/GetListDeptGarbage";

    ParamArray = { 'p' : 'p' };
    var obj = eval("GetGarbage.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
GetGarbage.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTGetGarbage.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    GetGarbage.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
GetGarbage.Resource = function () {

    $("#r_deptID")[0].innerHTML = R_DeptID;
    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_deptCnt")[0].innerHTML = R_DeptCnt;

    if ($(".cntUnit") != undefined) { $(".cntUnit").text(R_EA); } 
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 부서선택
***************************************************/
GetGarbage.View = function (p_deptID, p_deptName) { //alert(p_deptID +":"+p_deptName)
    if (p_deptID == "0" || p_deptName == "") return;

    p_deptName = p_deptID + "|" + p_deptName;

    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(p_deptID), escape(p_deptName));
        }
    }
    GetGarbage.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
***************************************************/
GetGarbage.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

