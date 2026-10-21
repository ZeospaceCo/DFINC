<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GetFolder.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Module.Popup.GetFolder" %>
<%@ Register Src="../Folder.ascx" TagName="FolderTree" TagPrefix="uc1" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />    
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><%=GetResource("Module_Folder_Node1")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:320px;">
                <div class="layout_scroll" style="height:300px;margin:10px 25px 15px 1px;padding:10px;outline:1px solid rgba(0,0,0,.1);" >
                    <uc1:FolderTree ID="FolderTree1" runat="server" UseIcon="true"></uc1:FolderTree>
                </div>
            </div>                
        </div>
    </div>
        	    
    <div style="display: none">
        <input type="hidden" id="hdnFolderID" name="hdnFolderID" />
        <input type="hidden" id="hdnFolderName" name="hdnFolderName" />
    </div>
    
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_Project_MSGFolder = "<%=GetResource("R_Project_MSGFolder") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Module/Popup/GetFolder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Module/Popup/GetFolder.js");
    </script>
</body>
</html>
