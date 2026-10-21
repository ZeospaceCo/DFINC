List = {}

$(function () {
    $(document).ready(function () {
        //관리자
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            List.Admin();
        });

        //목표등록
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //ddlSearchYear
        //년도선택        
        $('#ddlSearchYear').change(function (e) {
            e.preventDefault();
            List.GetList();
        });


        List.GetList();
        __addEventListener(document, "click", List.click);
    });
});

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 목표 추가
***************************************************/
List.Add = function (arg) {
    //var encParam = AjaxGetEncrypt(encodeURIComponent("folderID=" + FolderID + "&classID=" + ClassID + "&sortKey=" + SortKey + "&sortOrder=" + SortOrder));
    //document.location.href = "Add.aspx?p=" + encodeURIComponent(encParam);
    
    //목표등록 버튼을 눌렀을때와 메뉴에서 추가 버튼을 눌렀을때 Add 화면에서 상위 목표명/기간을 visible 처리함
    document.location.href = "Add.aspx";
}


List.SubAdd = function (arg) {
    //var encParam = AjaxGetEncrypt(encodeURIComponent("folderID=" + FolderID + "&classID=" + ClassID + "&sortKey=" + SortKey + "&sortOrder=" + SortOrder));
    //document.location.href = "Add.aspx?p=" + encodeURIComponent(encParam);

    //목표등록 버튼을 눌렀을때와 메뉴에서 추가 버튼을 눌렀을때 Add 화면에서 상위 목표명/기간을 visible 처리함
    
    ParamArray = {
        'MBOID': arg
    };
    CallEncrypt(CallGetParam());

    document.location.href = "SubAdd.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 관리자 부서 목표 추가
***************************************************/
List.Admin = function (arg) {
    //var encParam = AjaxGetEncrypt(encodeURIComponent("folderID=" + FolderID + "&classID=" + ClassID + "&sortKey=" + SortKey + "&sortOrder=" + SortOrder));
    //document.location.href = "Add.aspx?p=" + encodeURIComponent(encParam);

    //목표등록 버튼을 눌렀을때와 메뉴에서 추가 버튼을 눌렀을때 Add 화면에서 상위 목표명/기간을 visible 처리함
    document.location.href = "../Admin/Target/List.aspx";
}


List.GetList = function () {
    
    var url = "/DeskPlusEIP/Pim/Ws/Target/List.asmx/GetTargetList";
    var sYear = $("#ddlSearchYear").val();
    sYear = sYear + "-06-01";
    

    ParamArray = {
        'UserID': UserId,
        'startDate': sYear,
        'DeptID': DeptID
    };


    var obj = eval("List.SuccessTarget");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}



List.SuccessTarget = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Target/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    
    $('#placeholder').processTemplate(data.d);
    
    $('#loading').html("");
    List.Resource();
    //alert(data.d);
}

List.Resource = function () {

    $("#r_name")[0].innerHTML = col1; //MarkName
    $("#r_kind")[0].innerHTML = col2;
    $("#r_date")[0].innerHTML = col3;
    $("#r_menu")[0].innerHTML = col4;    

    $(".spAdd").text(add1);
    $(".spDel").text(Del1);

    if ($(".NoListData") != undefined) { $(".NoListData").text(TextEmpty); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }

}



List.Del = function (arg) {
    if (!confirm(ApplyDelete)) {
        return;
    }
 
    var url = "/DeskPlusEIP/Pim/Ws/Target/List.asmx/DelTarget";
    ParamArray = {
        'MBOID': arg,
        'CompanyID': CompanyID
    };


    var obj = eval("List.SuccessDelTarget");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}



List.SuccessDelTarget = function (data, status) {

    
    List.GetList();
    //ListTotal.Resource();
    //alert(data.d);
}


List.FileList = function (arg) {
    
    var url = "/DeskPlusEIP/Pim/Ws/Target/List.asmx/GetTargetFileList";
    ParamArray = {
        'MBOID': arg,
        'CompanyID': CompanyID
    };


    var obj = eval("List.SuccessFileList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
List.SuccessFileList = function (data, status) {
    
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}
var oContextMenuCtrl = null;


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
List.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    //var categoryID = el.getAttribute("CategoryID");
    var MBOID = el.getAttribute("MBOID");
    
    List.FileList(MBOID);
    

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        if (attachArr.length > 1) {
            oItem = oContextMenuCtrl.addItem(R_AllDownLoad, List.TotalDownLoad, ImagePath + "/Icon/folders.gif");
            oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
            oContextMenuCtrl.addBlank();
        }

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            CallEncrypt("filePath=" + attachFilePath + "&fileName=" + attachFileName);
            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;

            oItem = oContextMenuCtrl.addItem(attachFileName, List.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}




/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
List.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;     
        window.open(arg);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 전체 다운로드
***************************************************/
List.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        //window.open("", "openWin", "width=506px,height=388px,scrollbars=no,status=no,toolbar=no,resizable=0,location=no,menu=no");
        //$("#frmTotalDownLoad").submit();
        document.getElementById("frmTotalDownLoad").submit();

    }
}