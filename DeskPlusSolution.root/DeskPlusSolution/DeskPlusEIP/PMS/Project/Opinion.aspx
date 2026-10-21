<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Opinion.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.Opinion" %>
<!DOCTYPE html>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
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
                <h1><%=GetResource("Menu_admin8")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" ></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div class="layout_scroll" style="min-height:120px;height:120px;">
                    <asp:Label ID="lblContent" runat="server" style="width:97%;"></asp:Label> 
                </div>
            </div>                
        </div>
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/Opinion.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Project/Opinion.js");
    </script>
</body>
</html>