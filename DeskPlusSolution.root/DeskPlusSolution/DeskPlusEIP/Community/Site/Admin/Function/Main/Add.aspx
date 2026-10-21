<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Main.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body" class="write">

    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MainDisplay")%></h1>
            <!-- 버튼 //-->
            <span class="cm_subtxt">* <%=GetResource("MainDisplayDes")%></span>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab">
                            <li><a id="ActiveX"  class="tab_on"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divEditor">
                            <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div style="display:none;">
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>                 
        </div>

    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript">
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var R_ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Admin/Function/Main/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/Function/Main/Add.js");
    </script>
</body>
</html>
