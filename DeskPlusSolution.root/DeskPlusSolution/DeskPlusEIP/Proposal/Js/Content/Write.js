Write = {}

/**********************************************************************************************
작성자   : 
작성일자 :
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        // 공동 제안자  추가
        $('#cbtnCowork').click(function (e) {

            e.preventDefault();

            EleDom = "InputDiv1";

            ParamArray = {

            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Proposal/Common/GetUserList.aspx?p=" + GetEncryptValue;
            PopupWin(800, 500, url, "-500", "", true);

        });

        // 1차 심사자
        $('#cbtnJudge').click(function (e) {
            e.preventDefault();

            EleDom = "InputDiv2";

            ParamArray = {
                'judgeUserList': JudgeUserList
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Proposal/Common/GetJudgeUserList.aspx?p=" + GetEncryptValue;
            PopupWin(600, 480, url, "-500", "", true);

        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Write.AttachSwfFile();
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Write.Save();
        });

        //목록가기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            location = "List.aspx";
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            Write.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        Write.GetSpoolFileList();
        __addEventListener(document, "click", Write.click);
        
    });
});

/**************************************************
작성자   : 인쇄
작성일자 : shine
작성목적 : 사용자 추가값 가져오기
***************************************************/
Write.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**************************************************
작성자   : 
작성일자 : shine
작성목적 : 사용자 추가값 가져오기
***************************************************/
function GetDept(rtnVal) {

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);

        var responseStr = response.split("‡");

        sHtml = Write.GetDeptUserDiv(response);

        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 선택하기
***************************************************/
Write.GetDeptUserDiv = function (sList) {

    var targetObj = document.getElementById(EleDom);
    var sHtml = targetObj.innerHTML;
    var sFailueList = "";
    var tmpArr = sList.split("‡");
    var idArr = tmpArr[0].split(",");
    var nameArr = tmpArr[1].split(",");
    var catArr = tmpArr[2].split(",");
    
    // 1차 심사자 UserID
    if (EleDom == "InputDiv2")
        JudgeUserList = tmpArr[0];

    for (var i = 0; i < idArr.length; i++) {
        oid = idArr[i];
        name = nameArr[i];
        gubun = catArr[i];

        if (oid != "") {
            if (!Write.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
                if (gubun == "G") {
                    sHtml += " <a href='#' action='del' id=\"G_" + oid + "_" + EleDom + "\" nowrap type='G' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                } else {
                    sHtml += " <a href='#' action='del' id=\"M_" + oid + "_" + EleDom + "\" nowrap type='M' oid='" + oid + "'  objID='" + EleDom + "' style=\"cursor:hand;white-space:; line-height:14px;margin-right:5px;\" title=\"" + name + "\">" + name + "</a>";
                }
            }
            else {
                if (sFailueList.length > 0) sFailueList += "','";
                sFailueList += name;
            }
        }
    }
    if (sFailueList.length > 0)
        alert("이미 등록된 사용자 입니다." + "\n\n'" + sFailueList + "'");

    return sHtml;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
Write.ExistDeptUser = function (oid, target) {
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
Write.GetList = function (target, tag) {
    var sList = "";
    var targetObj = document.getElementById(target);
    if (targetObj) {
        var oChildrens = targetObj.childNodes;

        var sList = "";
        for (var i = 0; i < oChildrens.length; i++) {
            if (oChildrens[i].tagName == "A") {
                if (sList.length > 0) sList += ",";
                //oChildrens[i].type + "|" + 
                //sList += oChildrens[i].getAttribute(tag);
                sList += oChildrens[i].getAttribute(tag);
            }
        }
    }
    
    return sList;
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Write.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Write.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Write.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'PROPOSAL',
        'callBackEvent': "Write.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-20
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Write.GetSpoolFileList = function () {
    
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Write.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Write.SuccessSpoolFileList = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        Write.Resource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Write.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = "파일명";
    $("#j_spool_fileType")[0].innerHTML = "파일타입";
    $("#j_spool_fileLen")[0].innerHTML = "파일크기";
    $("#j_spool_filedelete")[0].innerHTML = "삭제";

    if ($(".NoListData") != undefined) { $(".NoListData").text("첨부파일이 존재하지 않습니다."); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Main/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/icon_search.gif'); }

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 삭제
**********************************************************************************************/
Write.DelFile = function (p_id, p_fileName) {
    
    ParamArray = {
        'fileName': unescape(p_fileName),
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Write.SuccessSpoolDelFileList");
    var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";
    // 첨부파일명 삭제(화면)
    $("#" + p_id).remove();

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 임시파일 삭제 처리
**********************************************************************************************/
function DelSpoolPath(fileName) {

    if (window.confirm("삭제하시겠습니까?")) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Write.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자가 선택이 되었는지
***************************************************/
Write.ExistFileName = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Write.ShowDelContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값


    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();
    
    oContextMenuCtrl.addNameItem("삭제", "Write.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
Write.DelItem = function (id) {

    $("#" + id).remove();

}

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 저장
***************************************************/
Write.Save = function () {

    var judgeUser = Write.GetList("InputDiv2", "oid");
    var title = $("#ctbTitle").val();

    // 1차 심사자 체크
    if (judgeUser == "") {
        alert("1차 심사자를 선택하세요.");
        return;
    }

    //집계코드 값 체크
    if (Write.GroupCodeCheck() == "")
        return;

    // 제목 체크
    if (title == "") {
        alert("제안제목을 입력하세요.");
        $("#ctbTitle").focus();
        return;
    }

    // 공동 제안자 UserID
    $("#hdCoWorkID").val(Write.GetList("InputDiv1", "oid"));
    // 1차 심사자 UserID
    $("#hdJudgeID").val(judgeUser);
    //  첨부파일명
    //$("#hdAttachFileName").val(Write.GetList("attachDiv1", "oid"));

    //2번 이상 클릭 불가
    $("#cbtnAdd").attr('onclick', '').unbind('click');
    
    __doPostBack('lbtnAdd', '');
}


/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 집계코드 값 체크
***************************************************/
Write.GroupCodeCheck = function () {

    // 집계코드 값 체크하기
    GroupCode = $(":input:radio[name=RblCode]:checked").val();

    if (GroupCode == undefined || GroupCode == "") {
        alert("집계코드를 선택하세요.");
        $("#ctbTitle").blur();
        GroupCode = "";
    }
    return GroupCode;
}