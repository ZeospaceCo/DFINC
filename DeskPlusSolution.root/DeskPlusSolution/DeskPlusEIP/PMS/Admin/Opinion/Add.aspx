<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Opinion.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">

        <div class="inside">
            <div id="popup_header">
                <h1><%=GetResource("Menu_admin4") %></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div> 
                    <asp:TextBox ID="tbxContent" runat="server" TextMode="MultiLine" Width="97%" Height="107"></asp:TextBox>
                </div>
            </div>                
        </div>
    </div>

    <div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var tbxContent = "<%=tbxContent.ClientID %>";
        var Private_BoardAdd_DPEditorTEXT = "<%=GetResource("Private_BoardAdd_DPEditorTEXT") %>";
        var Project_RegisterOpinion_Save  = "<%=GetResource("Project_RegisterOpinion_Save") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Opinion/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Opinion/Add.js");
    </script>
</body>
</html>
