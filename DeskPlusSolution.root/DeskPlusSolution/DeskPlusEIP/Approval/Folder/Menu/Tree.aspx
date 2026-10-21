<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Menu.Tree" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
        var CabinetID = "<%=CabinetID %>";
        var IsEnforceAdmin = "<%=IsEnforceAdmin %>";

        var DeptAdmin = false;

        var FormID = "";        
        var YYYY = "";
        var YYYYMM = "";
        var Gubun = "";
        var Types = ""; //S:기본폴더; U : 수발자 관리자 추가 폴더
        var GroupID = "";

        var R_MSG_Del5 = "<%=GetResource("MSG_Del5")%>"; 		//"정말로 삭제하시겠습니까?"
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>"; 		//처리되었습니다.
        var R_MSG_Refresh = "<%=GetResource("MSG_Refresh")%>"; 		//문서함을 갱신합니다.
         
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <div id="layout_treeboxAcc" style="height: 90%;">
                <div class="accordion">
                    <div class="menu left-main-menu"><a href="#" onclick=""><%=GetResource("DeptFolder")%></a></div>
                    <div class="menusub" style="display:none;">
                        <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript1" SetCallbackComplete="onCallbackComplete"
                            SetNodeSelect="onNodeSelect" SetCabinetList="Dept,Dept_Common" UseCheckBox="false"
                            UsePubFolder="true" runat="server" />
                    </div>
                    <div class="menu left-main-menu"><a href="#" onclick=""><%=GetResource("ApproveFolder")%></a></div>
                    <div class="menusub" style="display:none;">
                        <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript2" SetCallbackComplete="onCallbackComplete"
                            SetNodeSelect="onNodeSelect" SetCabinetList="Private,Approval,Reject,Arbitrary" UseCheckBox="false"
                            UsePubFolder="false" runat="server" />
                    </div>
                    <div class="menu left-main-menu"><a href="#" onclick=""><%=GetResource("CarbonFolder")%></a></div>
                    <div class="menusub" style="display:none;">
                        <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript3" SetCallbackComplete="onCallbackComplete"
                            SetNodeSelect="onNodeSelect" SetCabinetList="Carboncopy" UsePubFolder="false"
                            UseCheckBox="false" runat="server" />
                    </div>
                    <div class="menu left-main-menu"><a href="#" onclick=""><%=GetResource("EnforceFolder")%></a></div>
                    <div class="menusub" style="display:none;">
                        <div>
                        <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript4" SetCallbackComplete="onCallbackComplete"
                            SetNodeSelect="onNodeSelect" SetCabinetList="Enforce,EnforceSubmit,EnforceSend" UsePubFolder="false"
                            UseCheckBox="false" runat="server" />
                        </div>
                        <div style="padding:5px 0px 0px 0px;"></div>
                        <div style="display:none;" id="divEnforceAdmin">
                        <zeoAppFolderTree:UCAppFolderTreeScript ID="UCAppFolderTreeScript5" SetCallbackComplete="onCallbackComplete"
                            SetNodeSelect="onNodeSelect" SetCabinetList="EnforceSubmitAdmin,EnforceSendAdmin" UsePubFolder="false"
                            UseCheckBox="false" runat="server" />
                        </div>
                    </div>
                    <div class="menu"><a onclick="Tree.GoListPage('Impo')"><%=GetResource("ImpoMark")%></a></div>
                </div>
            </div>
            <div id="layout_treebuttom">
            </div>
            <!-- 버튼//-->
            <div id="layout_treebutton">
                <a id="delFolder" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
                <a id="modFolder" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Btn_Edit")%></a>
                <a id="addFolder" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>
            </div>
        </ul>
    </div>
    </form>
</body>
</html>
