<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetFolder.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.GetFolder" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        FolderID = "<%=FolderID%>";
        FolderName = "";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

         var R_Msg_SelFolder = "<%=GetResource("MSG_SelFolder") %>"; //"폴더를 선택하세요";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/FormGeneration/GetFolder.js"></script>     
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div id="popup_header">
            <h1><%=GetResource("Btn_FolderSel")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layer_scroll_box" style="height:270px">

                <zeoAppFormFolderTree:UCAppFormFolderTreeScript ID="UCAppFormFolderTreeScript1" 
                    SetCallbackComplete="onCallbackComplete" 
                    SetNodeSelect="FolderTree_onNodeSelect" 
                    UseCheckBox="false" runat="server" />

            </div>
        </div>                
    </div>
</div><!-- //layout_info --> 


    </form>
</body>
</html>
