<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyAppCount.ascx.cs"
    Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyAppCount" %>
<script type="text/javascript">
    var ApprovalStatusMenuUrl = "<%=ApprovalStatusMenuUrl%>";

    $(document).ready(function () {
        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var myAppCount_tabPortletID = "<%=TabPortletID%>";
        var myAppCount_portletID = "<%=PortletID%>";
        var myAppCount_categoryID = "<%=CategoryID%>";
        var myAppCount_dataURL = "<%=DataURL%>";
        var myAppCount_dataType = "<%=DataType%>";
        var myAppCount_IsAuth = "<%=IsAuth%>";
        var myAppCount_IsMini = "<%=IsMinimize %>";

        var portletDom = myAppCount_tabPortletID + "_" + myAppCount_portletID;

        if (myAppCount_IsAuth != "True") {
            $("#basicMenu_" + portletDom + " .del").remove();
        }

        //최소화 여부
        if (myAppCount_IsMini == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: "basicMenu_" + portletDom, leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);
        });

        // 더보기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();
        });
    });

    /**********************************************************************************************
    작성자   : shimpark
    작성일자 : 2015-04-18
    작성목적 : 이동하기
    **********************************************************************************************/
    function MyAppCountGoPage(p_gubun, p_param) {

        if (p_gubun == "approvalStatus" || p_gubun == "approvalComplete") {

            parent.frames["frmTop"].Menu.GoParam(p_gubun);

            if (p_gubun == "approvalStatus") {

                ParamArray = {
                    'folderStatusID': p_param,
                    'dcparm': p_gubun,
                    'cabinetID': ''
                };

                CallEncrypt(CallGetParam());

                var statusUrl = (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/DeskplusEIP/Approval/Status/Default.aspx";
                location = statusUrl + "?p=" + GetEncryptValue;
            }
            else if (p_gubun == "approvalComplete") {                          
                var statusUrl = (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/DeskplusEIP/Approval/Status/Default.aspx";
                var folderStatusID = MyAppCountGetApprovalCompleteFolderStatusID(p_param);

                if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1 && folderStatusID != "") {
                    ParamArray = { 'folderStatusID': folderStatusID };
                } else {
                    ParamArray = {
                        'accActive': "",
                        'dcparm': p_gubun,
                        'cabinetID': p_param
                    };
                }

                CallEncrypt(CallGetParam());

                location = (folderStatusID != "" && statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1)
                    ? statusUrl + "?p=" + GetEncryptValue
                    : "/DeskplusEIP/Approval/Folder/Default.aspx?p=" + GetEncryptValue;
            }
        }
        else {
            parent.location = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=" + p_param;
        }
    }

    function MyAppCountGetApprovalCompleteFolderStatusID(p_cabinetID) {
        switch ((p_cabinetID || "").toUpperCase()) {
            case "CARBONCOPY":
            case "CARBONCOPY_DOC":
                return "CarboncopyAll";
            case "CARBONCOPY_CIRRECEIPT":
                return "CarboncopyCirReceiptAll";
            default:
                return "";
        }
    }
</script>

<!-- 포틀릿(결재 카운트) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'"
        onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'">
        <div class="portlet-header">
            <li class="title headtitle"><%=Title%></li>
        </div>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
		<!--<a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>//-->		
	</li>
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <div id="plList_<%=TabPortletID%>_<%=PortletID%>" style="min-height: 260px;">
                <table width="98%" border="0" style="border-color:Blue" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('mail', 'webmail');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/001.png" width="25" height="25" alt="" title="" /><br />신규메일<br /><span style="color:blue">(<%=WebMailCnt%>)</span></a>
                        </td>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('mywork', 'webpims');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/007.png" width="25" height="25" alt="" title="" /><br />오늘일정<br /><span style="color:blue">(<%=PimCnt%>)</span></a>
                        </td>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('task', 'TASK');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/002.png" width="25" height="25" alt="" title="" /><br />업무협업<br /><span style="color:blue">(<%=TASKCnt%>)</span></a>
                        </td>
                    </tr>
                    <tr><td colspan="3">-------------------------------------</td></tr>
                    <tr>                        
                        <td align="center">
                            <a onclick="MyAppCountGoPage('approvalStatus', 'Target');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/043.png" width="25" height="25" alt="" title="" /><br />결재요청<br /><span style="color:blue">(<%=AppReqCnt%>)</span></a>
                        </td>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('approvalStatus', 'Charge');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/034.png" width="25" height="25" alt="" title="" /><br />수신요청<br /><span style="color:blue">(<%=AppRecvReqCnt%>)</span></a>
                        </td>                        
                        <td align="center">
                            <a onclick="MyAppCountGoPage('approvalStatus', 'Proxy');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/009.png" width="25" height="25" alt="" title="" /><br />결재대결<br /><span style="color:blue">(<%=AppProxyCnt%>)</span></a>
                        </td>
                    </tr>
                    <tr><td colspan="3">-------------------------------------</td></tr>
                    <tr>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('approvalStatus', 'Process');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/010.png" width="25" height="25" alt="" title="" /><br />결재진행<br /><span style="color:blue">(<%=AppnoSCCnt%>)</span></a>
                        </td>
                        <td align="center">                        
                            <a title="결재 완결(3일간)" onclick="MyAppCountGoPage('approvalStatus', 'Complete');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/067.png" width="25" height="25" alt="" title="" /><br />결재완결<br /><span style="color:blue">(<%=AppCompCnt%>)</span></a>
                        </td>
                        <td align="center">
                            <img src="/DeskPlusEIP/Portlet/Package/img/054.png" width="25" height="25" alt="" title="" /><br />결재반송<br /><span style="color:blue">(<a title="상신자 반송" onclick="MyAppCountGoPage('approvalStatus', 'Reject');return false;"><%=AppRejectCnt%></a>/<a title="결재자 반송" onclick="MyAppCountGoPage('approvalStatus', 'Target');return false;"><%=AppRejectOtherCnt%></a>)</span>
                        </td>
                    </tr>
                    <tr><td colspan="3">-------------------------------------</td></tr>
                    <tr>
                        <td align="center">                        
                            <a onclick="MyAppCountGoPage('approvalComplete', 'Carboncopy_Doc');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/032.png" width="25" height="25" alt="" title="" /><br />결재참조<br /><span style="color:blue">(<%=AppSCUCnt%>)</span></a>
                        </td>
                        <td align="center">                        
                            <a title="결재 받은회람"  onclick="MyAppCountGoPage('approvalComplete', 'Carboncopy_CirReceipt');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/033.png" width="25" height="25" alt="" title="" /><br />결재회람<br /><span style="color:blue">(<%=AppCRCnt%>)</span></a>
                        </td>
                        <td align="center">
                            <a onclick="MyAppCountGoPage('approvalComplete', 'Enforce');return false;"><img src="/DeskPlusEIP/Portlet/Package/img/025.png" width="25" height="25" alt="" title="" /><br />시행직인<br /><span style="color:blue">(<%=AppEnforceSealCnt%>)</span></a>
                        </td>                        
                    </tr>
                </table>		        
            </div>

        </div>
    </ul>
</div>
