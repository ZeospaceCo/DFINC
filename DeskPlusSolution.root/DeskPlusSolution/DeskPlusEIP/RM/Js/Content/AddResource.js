AddResource = {}
AddResource.Control = null;

/*******************************************************
작성자: famo23
작성일: 2012-02-15
작성목적: 초기화
********************************************************/
$(function () {
    $(document).ready(function () {
        // 자원 예약
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();

            AddResource.Confirm();
            //txtRepeat
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        $("#phImage").append("<ul><li><img src='" + ImagePath + "/Icon/ico_noimg2.gif' alt='img' style='height:160px;'/></li></ul>");

        AddResource.Init();
    });
});

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/********************************************************************************
작성자 : famo23
작성일자 : 2012-02-15
작성목적 : 초기화
*********************************************************************************/
AddResource.Init = function () {
    AddResource.Control = tvFolderTree;

    if (AddResource.Control != null) {
        if (AddResource.Control.SelectedNode != undefined) {
            CategoryID = AddResource.Control.SelectedNode.ID;

            AddResource.GetList();
        }
        else {
            AddResource.InitList();
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 가져오기 (초기화)
**********************************************************************************************/
AddResource.InitList = function () {
    $('#phResource').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTAddResources.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phResource').processTemplate('');
    $('#loading').html("");

    AddResource.Resource();
}

/********************************************************************************
작성자 : famo23
작성일자 : 2012-02-15
작성목적 : onNodeSelect 트리 노드 선택
*********************************************************************************/
function onNodeSelect(node) {
    CategoryID = node.ID;
    AddResource.GetList();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
AddResource.GetList = function () {
    var url = "/DeskPlusEIP/RM/Ws/Content/AddResource.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID
    };

    var obj = eval("AddResource.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AddResource.SuccessList = function (data, status) {
    $('#phResource').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTAddResources.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phResource').processTemplate(data.d);
    $('#loading').html("");

    AddResource.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AddResource.Resource = function () {
    $("#j_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#j_managerName")[0].innerHTML = R_ResourcesUser;

    if ($("#NoResourcesListData")[0] != undefined) {
        $("#NoResourcesListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 자원 상세 정보 가져오기 (웹서비스 호출)
**********************************************************************************************/
AddResource.ListInfo = function (_listID) {
    var url = "/DeskPlusEIP/RM/Ws/Content/AddResource.asmx/GetListInfo";

    //파라미터값 배열에 담기
    ParamArray = {
        'listID': _listID
    };

    var obj = eval("AddResource.SuccessListInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

    //이미지 파일
    AddResource.GetFileList(_listID);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-15
작성목적 : 자원 상세 정보 가져오기 (웹서비스 값받기)
**********************************************************************************************/
AddResource.SuccessListInfo = function (data, status) {
    $("#tbListID").val(data.d[0].ListID);
    $("#tdTitle").text(data.d[0].Title);
    $("#tdCode").text(data.d[0].Code);

    if (data.d[0].Authority == "0") {
        $("#tdAuth").text(R_No);
    }
    else {
        $("#tdAuth").text(R_Yes);
    }    
    
    $("#tdUsableQty").text(data.d[0].UsableQty);
    $("#tdPlace").text(data.d[0].Place);
    $("#tdContent").html(data.d[0].Content);
    $("#tbAuthority").val(data.d[0].Authority);

    var tmpQty = data.d[0].UsableQty;

    $('#ddlQty').empty();
    if (tmpQty == 0) {
        $('#ddlQty').append("<option value='0'>0</option>");
        return;
    }

    for (var i = 1; i < tmpQty + 1; i++) {
        $('#ddlQty').append("<option value='" + i + "' >" + i + "</option>");
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-05
작성목적 : 이미지 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
AddResource.GetFileList = function (_listID) {
    ParamArray = {
        'listID': _listID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddResource.SuccessFileList");
    var url = "/DeskPlusEIP/RM/Ws/Content/AddResource.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-05
작성목적 : 이미지 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
AddResource.SuccessFileList = function (data, status) {
    $("#phImage").empty();

    if (data.d.length > 0) {
        if (data.d.length == 1) {
            $("#imgLeft").css("display", "none");
            $("#imgRight").css("display", "none");
            $("#phImage").append("<ul><li><img src='" + data.d[0].FilePath + "' alt='img' style='height:160px;'/></li></ul>");
        }
        else {
            //이미지가 2개 이상일 경우 활성화 함

            $("#imgLeft").css("display", "");
            $("#imgRight").css("display", "");

            var tmpImage = "";
            for (var i = 0; i < data.d.length; i++) {
                tmpImage += "<li><img src='" + data.d[i].FilePath + "' alt='img' style='width:250px;height:160px;'/></li>";
            }

            tmpImage = "<ul>" + tmpImage + "</ul>";

            $("#phImage").append(tmpImage);

            $(".imageSlider .imageList").jCarouselLite({
                btnNext: ".imageSlider .right",
                btnPrev: ".imageSlider .left",
                visible: 1
                //auto: 1200,
                //speed: 1000
            });
        }
    }
    else {
        $("#imgLeft").css("display", "none");
        $("#imgRight").css("display", "none");
        $("#phImage").append("<ul><li><img src='" + ImagePath + "/Icon/ico_noimg2.gif' alt='img' style='height:160px;'/></li></ul>");
    }
}

/*******************************************************************************
작성자: famo23
작성일: 2012-02-15
작성목적: 자원 예약
********************************************************************************/
AddResource.Confirm = function () {
    var tmpListID = $("#tbListID").val();
    if (tmpListID == "") {
        alert(R_SelectResources);
        return;
    }

    var tmpUsableQty = $("#tdUsableQty").text();
    if (tmpUsableQty == "0") {
        alert(R_QtyZero);
        return
    }

    var tmpQTy = $("#ddlQty").val();
    var tmpTitle = $("#tdTitle").text();
    var tmpAuthority = $("#tbAuthority").val();

    var tmpLength = top.opener.ResourceArray.length;
    for (var i = 0; i < tmpLength; i++) {
        if (top.opener.ResourceArray[i].id == tmpListID) {
            alert(R_BeforResource);
            return;
        }
    }

    eval("top.opener.SuccessAddResource")(tmpTitle, tmpQTy, tmpListID, tmpUsableQty, tmpAuthority);
    self.close();
}