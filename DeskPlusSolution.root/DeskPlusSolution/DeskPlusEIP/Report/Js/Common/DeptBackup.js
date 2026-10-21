DeptBackup = {}

$(function () {
    $(document).ready(function () {
        //취소
        $('#cbtnClose').click(function (e) { e.preventDefault(); DeptBackup.Close(); });

        DeptBackup.LoadData();
    });
});

/**************************************************
작성자   :
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
DeptBackup.LoadData = function () {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        DeptBackup.GetPageList();
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
DeptBackup.GetPageList = function () {
    var url = "/DeskPlusEIP/Report/Ws/Common/DeptBackup.asmx/GetList";

    ParamArray = { 'p': '' };
    var obj = eval("DeptBackup.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
DeptBackup.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Common/JTDeptBackup.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    DeptBackup.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 :
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
DeptBackup.Resource = function () {
    $("#j_Name")[0].innerHTML = R_DeptName;
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 부서선택
***************************************************/
DeptBackup.View = function (p_deptID, p_deptName) { //alert(p_deptID +":"+p_deptName)
    if (p_deptID == "0" || p_deptName == "") return;

    p_deptName = p_deptID + "|" + p_deptName;

    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(p_deptID), escape(p_deptName));
        }
    }
    DeptBackup.Close();
}

/**************************************************
작성자   :
작성일자 :
작성목적 : [취소]
***************************************************/
DeptBackup.Close = function () {
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 선택한 부서값
***************************************************/
DeptBackup.SelDelDept = function (p_deptID, p_deptName) {
    if (Callback != "") {
        var obj = eval("parent." + Callback);
        if (obj) {
            obj(escape(p_deptID), escape(p_deptName));
        }
    }
    else {
        parent.GetDept(escape(p_deptID), escape(p_deptName));
    }

    DeptBackup.Close();
}