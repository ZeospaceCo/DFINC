var Tree = {};

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        var h = $(".menusub:eq(" + AccordionActive + ")").height();
        $(".menusub:eq(" + AccordionActive + ")").attr("style", "display:;height:" + h + "px;");

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //폴더 (추가)
        $('#addFolder').click(function (e) {
            e.preventDefault();
            Tree.AddFolder("Add");
        });

        //폴더 (수정)
        $('#modFolder').click(function (e) {
            e.preventDefault();
            Tree.AddFolder("Mod");
        });

        //폴더 (삭제)
        $('#delFolder').click(function (e) {
            e.preventDefault();
            Tree.DelFolder("Del");
        });

        //수발자 여부
        Tree.GetAccountUser();

        //시행문관리자여부
        if (IsEnforceAdmin == "1") {
            $("#divEnforceAdmin").attr("style", "display:");
        } else {
            $("#divEnforceAdmin").attr("style", "display:none;");
        }

        Tree.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Tree.Init = function () {

    $("#addFolder").hide();
    $("#modFolder").hide();
    $("#delFolder").hide();


    Tree.ContentFrame = parent.frmContent;
    Tree.SelectedNodeID = CabinetID;
}


/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
var tnode = null;
function onNodeSelect(node) {

    $("#addFolder").hide();
    $("#modFolder").hide();
    $("#delFolder").hide();

    var id = node.ID;
    var nodeName = node.Text;
    var targetPage = node.GetProperty("TargetPage");

    FormID = node.GetProperty("FormID");    
    YYYY = node.GetProperty("YYYY");
    YYYYMM = node.GetProperty("YYYYMM");
    Gubun = node.GetProperty("Gubun");
    Types = node.GetProperty("Types"); //S:기본폴더; U : 수발자 관리자 추가 폴더
    GroupID = node.GetProperty("GroupID");

    //부서별 공용폴더
    if (DeptAdmin == true) {
        if (Types == "S" && Gubun == "Dept_Common" && GroupID == "Dept") {
            $("#addFolder").show();
        } else if (Gubun == "Dept_Common" && GroupID == "Public") {
            $("#addFolder").show();
            $("#modFolder").show();
            $("#delFolder").show();
        }
    }    
    
    //폴더 추가 가능 폴더
    if (Gubun == "Private_Folder" || Gubun == "Approval_Folder" || Gubun == "Arbitrary_Folder" || Gubun == "Reject_Folder") {
        if (Types == "S") {
            $("#addFolder").show();
        } else {
            $("#addFolder").show();
            $("#modFolder").show();
            $("#delFolder").show();
        }
    }
    
    if (YYYY == undefined) YYYY = "";
    if (YYYYMM == undefined) YYYYMM = "";

    CabinetID = id;

    //alert("Gubun:" + Gubun + ", CabinetID:" + CabinetID)
    ParamArray = {
        "formID": FormID,
        "yyyy": YYYY,
        "yyyymm": YYYYMM,
        "cabinetID": id,
        "types": Types,
        "headerName": nodeName,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());

    if (targetPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = targetPage + "?p=" + GetEncryptValue;
    }

    tnode = node;
    setTimeout("tnode.Expand()", 300); //node.Expand();  //하위노드 확장
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}


/**************************************************
작성목적 : 폴더 추가 및 수정 페이지 이동
***************************************************/
Tree.AddFolder = function (p_CRUD) {

    var goList = "";

    ParamArray = {
        "cabinetID": CabinetID,
        "gubun": p_CRUD
    };

    if (Gubun == "Dept_Common") {
        goList = "/DeskPlusEIP/Approval/Folder/Dept/Add.aspx";
    }
    else if (Gubun == "Private_Folder") {
        goList = "/DeskPlusEIP/Approval/Folder/Private/Add.aspx";
    }
    else if (Gubun == "Approval_Folder") {
        goList = "/DeskPlusEIP/Approval/Folder/Approve/Add.aspx";
    }
    else if (Gubun == "Arbitrary_Folder") {
        goList = "/DeskPlusEIP/Approval/Folder/Arbitrary/Add.aspx";
    }
    else if (Gubun == "Reject_Folder") {
        goList = "/DeskPlusEIP/Approval/Folder/Approve/AddR.aspx";
    }

    if (goList != "") {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = goList + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성목적 : 폴더 삭제 처리
***************************************************/
Tree.DelFolder = function () {

    if (CabinetID != "" && Gubun != "") {

        if (window.confirm(R_MSG_Del5)) {
            ParamArray = {
                "folderID": CabinetID,
                "gubun": Gubun
            };

            var url = "/DeskPlusEIP/Approval/Ws/Folder/Menu/Tree.asmx/Del";
            var obj = eval("Tree.SuccessDelFolder");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 폴더 삭제 처리
**********************************************************************************************/
Tree.SuccessDelFolder = function (data, status) {
    if (data.d > 0) {
        alert(R_MSG_SUCCESS + " " + R_MSG_Refresh);
        var goCabinetID = "";

        if (Gubun == "Dept_Common") {
            AccordionActive = 0;
            goCabinetID = "Dept";
        } else if (Gubun == "Private_Folder") {
            AccordionActive = 1;
            goCabinetID = "Private";
        }
        else if (Gubun == "Approval_Folder") {
            AccordionActive = 1;
            goCabinetID = "Private";
        }
        else if (Gubun == "Arbitrary_Folder") {
            AccordionActive = 1;
            goCabinetID = "Private";
        }
        else if (Gubun == "Reject_Folder") {
            AccordionActive = 1;
            goCabinetID = "Reject";
        }

        ParamArray = {
            'cabinetID' : goCabinetID,
            'accActive': AccordionActive
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        parent.frmLeft.location = "/DeskplusEIP/Approval/Folder/Menu/Tree.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 수발자 여부
**********************************************************************************************/
Tree.GetAccountUser = function () {    

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Menu/Tree.asmx/GetAccountUser";
    
    ParamArray = {
    };

    var obj = eval("Tree.SuccessGetAccountUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 수발자 여부
**********************************************************************************************/
Tree.SuccessGetAccountUser = function (data, status) {
    DeptAdmin = data.d; //부서공용자 여부
}

/**************************************************************
리스트 페이지 호출하기 (초기화)
**************************************************************/
Tree.GoListPage = function (_menuID) {      // alert(_menuID);

    var listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";

    switch (_menuID) {
        case "Dept":
            listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";
            break;
        case "Dept_Common":
            listPage = "/DeskplusEIP/Approval/Folder/Dept/CommonList.aspx";
            break;
        case "Dept_Doc":
            listPage = "/DeskplusEIP/Approval/Folder/Dept/List.aspx";
            break;
        case "Dept_Receipt":
            listPage = "/DeskplusEIP/Approval/Folder/Dept/ReceiptList.aspx";
            break;
        case "Dept_Send":
            listPage = "/DeskplusEIP/Approval/Folder/Dept/SendList.aspx";
            break;
        case "Private":
            listPage = "/DeskplusEIP/Approval/Folder/Private/List.aspx";
            break;
        case "Private_Doc":
            listPage = "/DeskplusEIP/Approval/Folder/Private/List.aspx";
            break;
        case "Private_Folder":
            listPage = "/DeskplusEIP/Approval/Folder/Private/ListFolder.aspx";
            break;
        case "Approval":
            listPage = "/DeskPlusEIP/Approval/Folder/Approve/List.aspx";
            break;
        case "Approval_Doc":
            listPage = "/DeskPlusEIP/Approval/Folder/Approve/List.aspx";
            break;
        case "Approval_Folder":
            listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListFolder.aspx";
            break;
        case "Reject":
            listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListReject.aspx";
            break;
        case "Reject_Doc":
            listPage = "/DeskplusEIP/Approval/Folder/Approve/ListReject.aspx";
            break;
        case "Reject_Folder":
            listPage = "/DeskplusEIP/Approval/Folder/Approve/ListRejectFolder.aspx";
            break;
        case "Arbitrary":
            listPage = "/DeskPlusEIP/Approval/Folder/Arbitrary/List.aspx";
            break;
        case "Arbitrary_Doc":
            listPage = "/DeskplusEIP/Approval/Folder/Arbitrary/List.aspx";
            break;
        case "Arbitrary_Folder":
            listPage = "/DeskplusEIP/Approval/Folder/Arbitrary/ListFolder.aspx";
            break;
        case "Carboncopy":
            listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/List.aspx";
            break;
        case "Carboncopy_Doc":
            listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/List.aspx";
            break;
        case "Carboncopy_Forward":
            listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardList.aspx";
            break;
        case "Carboncopy_CirReceipt":
            listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirReceiptList.aspx";
            break;
        case "Carboncopy_CirSend":
            listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirSendList.aspx";
            break;
        case "Enforce":
            listPage = "/DeskPlusEIP/Approval/Folder/Enforce/List.aspx";
            break;
        case "EnforceSend":
            listPage = "/DeskPlusEIP/Approval/Folder/Enforce/ListSend.aspx";
            break;
        case "EnforceSendAdmin":
            listPage = "/DeskPlusEIP/Approval/Folder/EnforceAdmin/ListSend.aspx";
            break;
        case "EnforceSubmit":
            listPage = "/DeskPlusEIP/Approval/Folder/Enforce/ListSubmit.aspx";
            break;
        case "EnforceSubmitAdmin":
            listPage = "/DeskPlusEIP/Approval/Folder/EnforceAdmin/ListSubmit.aspx";
            break;
        case "Impo":
            listPage = "/DeskPlusEIP/Approval/Folder/Impo/List.aspx";
            break;
    }
    ParamArray = {
        "formID": "0",
        "yyyy": "",
        "yyyymm": "",
        "cabinetID": _menuID,
        "types": "S",
        "headerName": "",
        'gubun': _menuID
    };
    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}