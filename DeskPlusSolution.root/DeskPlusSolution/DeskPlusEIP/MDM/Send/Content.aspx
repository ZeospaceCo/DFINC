<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Send.Content" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>     
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>       
    <script type="text/javascript" >
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Send/Content.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/Send/Content.js");
    </script>
</head>
<body>
   <form id="form1" runat="server">
     <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Content")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div id="layout_content">
                <div id="TxtContent" runat="server" style="width: 310px; height: 321px; overflow:auto; overflow-x:hidden; word-wrap:break-word;"></div>
            </div>
        </div>
    </div>
  </form>
</body>
</html>
