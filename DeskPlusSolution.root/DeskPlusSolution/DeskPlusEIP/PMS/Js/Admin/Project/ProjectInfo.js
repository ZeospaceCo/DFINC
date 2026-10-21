var ProjectInfo = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ProjectInfo.cbtnOk_OnClick();
        });

        //책임자명 검색
        $('#cbtnDeptUserTree').click(function (e) {
            e.preventDefault();
            EleDom = "InputDiv1"
            ProjectInfo.PopupUser('2');
        });

        //폴더관리
        $('#cbtnFolder').click(function (e) {
            e.preventDefault();
            ProjectInfo.GetFolder();
        });

        //계획시작일
        $('#cbxPlanStartDate').click(function () {
            if ($('#cbxPlanStartDate').is(":checked")) {
                $("#tbxStartDate").val($('#tbxPlanStartDate').val());
            } else {
                $("#tbxStartDate").val("");
            }
        });

        //계획완료일
        $('#cbxPlanEndDate').click(function () {
            if ($('#cbxPlanEndDate').is(":checked")) {
                $("#tbxEndDate").val($('#tbxPlanEndDate').val());
            } else {
                $("#tbxEndDate").val("");
            }
        });

        $("#tbxPlanStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxPlanEndDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxStartDate").datepick($.datepicker.regional[PreLang]);
        $("#tbxEndDate").datepick($.datepicker.regional[PreLang]);

        ProjectInfo.Init();

        __addEventListener(document, "click", ProjectInfo.click);
    });
});

/**********************************************************************************************
[책임자 할당]
**********************************************************************************************/
ProjectInfo.Init = function () {
    if (chargerName != "") {
        EleDom = "InputDiv1";
        sHtml = ProjectInfo.GetDeptUserDiv(chargerName);
        var targetObj = document.getElementById("InputDiv1");
        targetObj.innerHTML = sHtml;
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
ProjectInfo.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": ProjectInfo.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
ProjectInfo.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addItem(R_Del, "ProjectInfo.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
ProjectInfo.DelItem = function (id) {
    $("#" + id).remove();
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 선택하기
***************************************************/
ProjectInfo.GetDeptUserDiv = function (sList) {
    var targetObj = document.getElementById(EleDom);
    var sHtml = targetObj.innerHTML;
    var sFailueList = "";
    var tmpArr = sList.split("‡");
    var idArr = tmpArr[0].split(",");
    var nameArr = tmpArr[1].split(",");
    var catArr = tmpArr[2].split(",");

    for (var i = 0; i < idArr.length; i++) {
        oid = idArr[i];
        name = nameArr[i];
        gubun = catArr[i];

        if (oid != "") {
            if (!ProjectInfo.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
                if (gubun == "G") {
                    sHtml += "<a href='#' action='del' id=\"G_" + oid + "_" + EleDom + "\" nowrap type='G' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                } else {
                    sHtml += "<a href='#' action='del' id=\"M_" + oid + "_" + EleDom + "\" nowrap type='M' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                }
            }
            else {
                if (sFailueList.length > 0) sFailueList += "','";
                sFailueList += name;
            }
        }
    }
    if (sFailueList.length > 0)
        alert(MSG_DUF_USERDEPT + "\n\n'" + sFailueList + "'");

    return sHtml;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
ProjectInfo.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 선택된 부서/사용자 정보 가져오기
***************************************************/
ProjectInfo.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                //oChildrens[i].type + "|" +
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }
    return sList;
}

/**********************************************************************************************
[폴더 새창]
**********************************************************************************************/
ProjectInfo.GetFolder = function () {
    var url = "../../Module/Popup/GetFolder.aspx";
    var param = "";

    PopupWin(400, 400, url, "-500", "", true);
}

ProjectInfo.cbtnOk_OnClick = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("tbxTitle", R_Project_MSGProjectName, true);
    if (!validator.validate()) return;

    var getStartDate = $("#tbxPlanStartDate").val();
    var getEndDate = $("#tbxPlanEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputDate);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_CheckDate);
            return false;
        }
    }

    getStartDate = $("#tbxStartDate").val();
    getEndDate = $("#tbxEndDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_InputStartDate);
        return false;
    }
    else {
        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_CheckEndDate);
            return false;
        }
    }

    document.getElementById("hdChargerID").value = ProjectInfo.GetList("InputDiv1", "oid");
    __doPostBack('lbtnOk', '');
}

/**********************************************************************************************
[책임자명 새창]
**********************************************************************************************/
ProjectInfo.PopupUser = function (gubun, ctl) {
    ParamArray = {
        'projectID': projectID
    };

    CallEncrypt(CallGetParam());
    var url = "../../Module/Popup/GetUser.aspx?p=" + GetEncryptValue;
    PopupWin(300, 470, url, "-500", "", true);

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡");

        sHtml = ProjectInfo.GetDeptUserDiv(response, ctl);

        var targetObj = document.all(ctl);
        targetObj.innerHTML = sHtml;
        return;
    }
}

/**********************************************************************************************
[책임자명 새창] 가져오기
**********************************************************************************************/
function SelectedGetUser(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);
        var responseStr = response.split("‡");
        sHtml = ProjectInfo.GetDeptUserDiv(response);
        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
        return;
    }
}

/**********************************************************************************************
폴더 선택하기
**********************************************************************************************/
function SelectedFolder(folderID, folderName) {
    document.getElementById("tbxFolderID").value = folderID;
    document.getElementById("tbxFolder").value = unescape(folderName);
}