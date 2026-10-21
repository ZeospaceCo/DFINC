var Tree = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        Tree.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Tree.Init = function () {

    Tree.GetDeptList();

    var targetPage = "/DeskplusEIP/Approval/Folder/OtherDept/List.aspx";

    ParamArray = {
        "formID": "0",
        "yyyy": "",
        "yyyymm": "",
        "cabinetID": "Dept",
        "types": "S",
        "headerName": "",
        "targetDeptID": TargetDeptID
    };

    if (targetPage != "" && parent.frmContent) {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = targetPage + "?p=" + GetEncryptValue;
    }

    if ($.browser.msie) {
        $("#divTree").height(270);
    }
    else {
        $("#divTree").height(300);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 부서 리스트
**********************************************************************************************/
Tree.GetDeptList = function () {

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    $('#loading').html(loadingPath);

    var url = "/DeskPlusEIP/Approval/Ws/Folder/OtherDept/Tree.asmx/GetDeptList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
    };

    var obj = eval("Tree.SuccessGetDeptList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 부서 리스트
**********************************************************************************************/
Tree.SuccessGetDeptList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/OtherDept/JTDeptList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Tree.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Tree.Resource = function () {

    $("#r_deptname")[0].innerHTML = "부서명";

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 부서 선택
**********************************************************************************************/
Tree.SetDept = function (p_deptID) {

    TargetDeptID = p_deptID;

    ParamArray = {
        'targetDeptID': TargetDeptID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    location = "Tree.aspx?p=" + GetEncryptValue;
}





/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    
    if (TargetDeptID == "0") {
        alert("타부서명을 선택해 주시기 바랍니다.");
        return false;
    }

    var targetPage = node.GetProperty("TargetPage");
    targetPage = targetPage.replace("/Dept/", "/OtherDept/");
    CabinetID = node.ID;    

    FormID = node.GetProperty("FormID");
    YYYY = node.GetProperty("YYYY");
    YYYYMM = node.GetProperty("YYYYMM");
    Gubun = node.GetProperty("Gubun");
    Types = node.GetProperty("Types"); //S:기본폴더; U : 수발자 관리자 추가 폴더
    GroupID = node.GetProperty("GroupID");
    var nodeName = node.Text;

    if (YYYY == undefined) YYYY = "";
    if (YYYYMM == undefined) YYYYMM = "";

    ParamArray = {
        "formID": FormID,
        "yyyy": YYYY,
        "yyyymm": YYYYMM,
        "cabinetID": CabinetID,
        "types": Types,
        "headerName": nodeName,
        "targetDeptID": TargetDeptID
    };

    CallEncrypt(CallGetParam());

    if (targetPage != "" && parent.frmContent) {
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = targetPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

