<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Menu.Menu" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = <%=AccActive %>;
        var TmpDeptID = 0;
        var MenuID = "";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/Menu/Menu.js");
    </script>
</head>
<body>
   <form id="form1" runat="server">
   <div id="layout_left" class="left-menu admin"> 
      <ul class="left-menu-container">
        <li>
            <div id="layout_treetop">
                <span class="treetop_title"><%=GetResource("MDM")%></span>
            </div>

            <div id="layout_treeboxAcc" style="height:90%;">
                <div class="accordion">
                    <div class="menu" id="setting" ><a href="#" onclick="" ><%=GetResource("Auth")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a id="listAdd"><%=GetResource("ReqApprove")%></a></h2></li>
                            <li><h2><a id="listReject"><%=GetResource("ReqReject")%></a></h2></li>
                            <li><h2><a id="listUse"><%=GetResource("ReqUse")%></a></h2></li>
                            <li><h2><a id="authGroup"><%=GetResource("AuthGroup")%></a></h2></li>
                            <li><h2><a id="config"><%=GetResource("EnvSetting")%></a></h2></li>
                            <li><h2><a id="change"><%=GetResource("ListSetting")%></a></h2></li>
                        </ul>
                    </div>
                    <div class="menu" id="auth"><a href="#" onclick="" ><%=GetResource("AuthAccessControl")%></a></div>             
                    <div class="menusub" style="display:none">
                        <zeoMDMTree:UCMDMTreeScript ID="UCMDMTreeScript1" runat="server" UseIcon="true" SetCallbackComplete="onCallbackComplete" SetNodeSelect="MenuNodeSelect_onNodeSelect" />
                    </div>
                    <div class="menu" id="portal"><a href="#" onclick="" ><%=GetResource("Portal")%></a></div>             
                    <div class="menusub" style="display:none">
                         <ul>
                            <li><h2><a id="portalS"><%=GetResource("Portal")%></a></h2></li>
                        </ul>
                    </div>
                    <div class="menu" id="accountDept"><a href="#" onclick="" ><%=GetResource("AccountDept")%></a></div>
                    <div class="menusub" style="display:none">
                        <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveDept="false" />
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
