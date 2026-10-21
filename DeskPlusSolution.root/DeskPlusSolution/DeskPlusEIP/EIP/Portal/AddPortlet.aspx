<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPortlet.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.AddPortlet" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = 0;
        var CatID = "0";
        var Gubun = "";
        var IsAdmin = "<%=IsAdmin %>";
        var TitleName = "<%=TitleName %>";
        var Modules = "<%=Modules %>";
        var DataUrl = "<%=DataUrl %>";
        var Template = "<%=Template %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Complete = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";        
        var R_CheckPortletGroup = "<%=GetResource("CheckGroup") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/AddPortlet.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/AddPortlet.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
      <ul class="left-menu-container left-menu-2">
      <li>

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("CreatePortlet")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"/></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:370px">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="100px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("PortletGroupName")%></th>
                    <td><span id="portletName" style="width:95%"></span></td>
                </tr>
            </tbody>
            </table>
            <div class="layout_scroll" >
                <div id="layout_treeboxAcc"  style="border-bottom:1px solid #aeaeae;">
                    <div class="accordion" >
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("PrivatePortlet")%></a></div>
                        <div class="menusub">
                            <zeoPortletTree:UCPortletTreeScript ID="PrivatePortletTree" runat="server" UseIcon="true" UseActive="true"
                                SetCallbackComplete="onCallbackComplete" SetNodeSelect="PrivatePortletTree_onNodeSelect" />
                        </div>
                        <div id="commonMenu" class="menu"><a href="#" onclick="" ><%=GetResource("CommonPortlet")%></a></div>
                        <div id="commonMenuSub" class="menusub">
                            <zeoPortletTree:UCPortletTreeScript ID="CommonPortletTree" runat="server" UseIcon="true" UseActive="true"
                                SetCallbackComplete="onCallbackComplete" SetNodeSelect="CommonPortletTree_onNodeSelect" />
                            
                        </div>
                    </div>    
                </div>
            </div>

        </div>
    </div>
    </li>
    </ul>
</div>
    </form>
</body>
</html>
