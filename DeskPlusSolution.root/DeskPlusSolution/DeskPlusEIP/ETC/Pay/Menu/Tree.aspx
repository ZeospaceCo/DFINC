<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Pay.Menu.Tree" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/            
        var FormID = "<%=FormID%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Pay/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Pay/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_left" class="left-menu"> 
        <ul class="left-menu-container">
        	<li>
                <div id="layout_treeboxAcc">
                    <div class="accordion">
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("PayTitle")%></a></div>
                        <div class="menusub">                    
                            <zeoEtcPayFolderStatusTree:UCEtcPayFolderStatusTreeScript ID="UCFolderStatusTreeScript1" SetCallbackComplete="onCallbackComplete" SetNodeSelect="onNodeSelect" 
                            SetFolderStatusList = "" UseCheckBox="false" runat="server" />
                        </div>
                    </div>    
                </div>
                <div id="layout_treebuttom"></div>
             </li>
         </ul>
    </div>
    </form>
</body>
</html>
