Mod = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //공동제안자
        $('#cbtnCowork').click(function (e) {
            e.preventDefault();

            EleDom = "InputDiv1";

            ParamArray = {

            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Proposal/Common/GetUserList.aspx?p=" + GetEncryptValue;
            PopupWin(800, 500, url, "-500", "", true);

        });

        //1차 심사자
        $('#cbtnJudge').click(function (e) {
            e.preventDefault();

            EleDom = "InputDiv2";

            ParamArray = {
                'judgeUserList': JudgeUserList
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Proposal/Common/GetJudgeUserList.aspx?p=" + GetEncryptValue;
            PopupWin(600, 485, url, "-500", "", true);

        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Mod.Save();
        });

        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            location = "MyList.aspx";

            /*
            ParamArray = {
            'listCount': ListCount,
            'currentPage': CurrentPage,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'order': Order,
            'proposalNo': ProposalNo,
            'menuID': MenuID
            };

            CallEncrypt(CallGetParam());
            location = "MyList.aspx?p=" + GetEncryptValue;
            */

        });

        Mod.GetSpoolFileList();

        __addEventListener(document, "click", Mod.click);

        Mod.Init();
    });

});

/**********************************************************************************************
초기화
**********************************************************************************************/
Mod.Init = function () {
    //수정모드일경우

    if (ProposalNo != "0") {
        if (CoWorkList != "") {
            EleDom = "InputDiv1";
            sHtml = Mod.GetDeptUserDiv(CoWorkList);
            var targetObj = document.getElementById("InputDiv1");
            targetObj.innerHTML = sHtml;
        }

        if (JudgeList != "") {
            EleDom = "InputDiv2";
            sHtml = Mod.GetDeptUserDiv(JudgeList);
            var targetObj = document.getElementById("InputDiv2");
            targetObj.innerHTML = sHtml;
        }

        Mod.GetAttachFileList();
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Mod.GetAttachFileList = function () {

    ParamArray = {
        'proposalNo': ProposalNo
    };

    CallEncrypt(CallGetParam())
    
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Proposal/Ws/Content/Mod.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTDelFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.Resource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 게시판 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachID) {

    if (window.confirm("삭제하시겠습니까?")) {
        ParamArray = {
            'proposalNo': ProposalNo,
            'attachID': p_attachID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessFileList");
        var url = "/DeskPlusEIP/Proposal/Ws/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {
    if (data.d == true) {
        Mod.GetAttachFileList();
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Mod.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'PROPOSAL',
        'callBackEvent': "Mod.GetSpoolFileList"
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
Mod.GetSpoolFileList = function () {
    
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        Mod.Resource();
    }, 100);
    */
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = "파일명";
    $("#j_spool_fileType")[0].innerHTML = "파일타입";
    $("#j_spool_fileLen")[0].innerHTML = "파일크기";
    $("#j_spool_filedelete")[0].innerHTML = "삭제";

    $("#j_fileName")[0].innerHTML = "파일명";
    $("#j_fileType")[0].innerHTML = "파일타입";
    $("#j_fileLen")[0].innerHTML = "파일크기";
    $("#j_filedelete")[0].innerHTML = "삭제";

    if ($(".NoListData") != undefined) { $(".NoListData").text("첨부파일이 존재하지 않습니다."); }
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
        var obj = eval("Mod.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}


var oContextMenuCtrl = null;

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Mod.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Mod.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}




/**************************************************
작성자   : shine
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Mod.ShowDelContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값


    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem("삭제", "Mod.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
Mod.DelItem = function (id) {
    $("#" + id).remove();
    /*
    var oEl = document.getElementById(id);
    if (oEl) {
    oEl.removeNode(true);
    }
    */
}

/**************************************************
작성자   : 
작성일자 : shine
작성목적 : 새창에서 호출한 사용자 가져오기
***************************************************/
function GetDept(rtnVal) {

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var response = unescape(rtnVal);

        var responseStr = response.split("‡");

        sHtml = Mod.GetDeptUserDiv(response);

        var targetObj = document.getElementById(EleDom);
        targetObj.innerHTML = sHtml;
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 추가한 사용자 html  그리기
***************************************************/
Mod.GetDeptUserDiv = function (sList) {
    //debugger;
    var targetObj = document.getElementById(EleDom);
    var sHtml = targetObj.innerHTML;
    var sFailueList = "";
    var tmpArr = sList.split("‡");
    var idArr = tmpArr[0].split(",");
    var nameArr = tmpArr[1].split(",");
    var catArr = "M";

    // 1차 심사자 UserID
    if (EleDom == "InputDiv2")
        JudgeUserList = tmpArr[0];

    for (var i = 0; i < idArr.length; i++) {
        oid = idArr[i];
        name = nameArr[i];
        gubun = catArr[i];

        if (oid != "") {
            if (!Mod.ExistDeptUser(gubun + "_" + oid + "_" + EleDom, EleDom)) {
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
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 중복된 사용자 체크 여부
***************************************************/
Mod.ExistDeptUser = function (oid, target) {
    var targetObj = document.getElementById(target);
    oChildrens = targetObj.childNodes;
    for (var i = 0; i < oChildrens.length; i++) {
        if (oChildrens[i].id == oid)
            return true;
    }
    return false;
}

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 최종 할당된 공동제안자 및 심사자 정보 가져오기
***************************************************/
Mod.GetList = function (target, tag) {
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
[저장]
**********************************************************************************************/
Mod.Save = function (p_gubun) {

    // 공동제안자
    $("#hdCoWorkID").val(Mod.GetList("InputDiv1", "oid"));
    $("#hdCoWorkName").val(Mod.GetList("InputDiv1", "title"));
    // 1차 심사자
    $("#hdJudgeID").val(Mod.GetList("InputDiv2", "oid"));
    $("#hdJudgeName").val(Mod.GetList("InputDiv2", "title"));


    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbTitle", "제목을 입력해 주세요.", true);
    if (!validator.validate()) return;

    if ($("#hdJudgeID").val() == "") {
        alert("1차 심사자를 등록하세요.");
        return;
    }

    //집계코드 값 체크
    if (Mod.GroupCodeCheck() == "")
        return;

    //2번 이상 클릭 불가
    $("#cbtnMod").attr('onclick', '').unbind('click');

    __doPostBack('lbtnAdd', "");
}

/**************************************************
작성자   : shine
작성일자 : 2012-04-12
작성목적 : 집계코드 값 체크
***************************************************/
Mod.GroupCodeCheck = function () {

    // 집계코드 값 체크하기
    GroupCode = $(":input:radio[name=RblCode]:checked").val();

    if (GroupCode == undefined || GroupCode == "") {
        alert("집계코드를 선택하세요.");
        $("#ctbTitle").blur();
        GroupCode = "";
    }
    return GroupCode;
}