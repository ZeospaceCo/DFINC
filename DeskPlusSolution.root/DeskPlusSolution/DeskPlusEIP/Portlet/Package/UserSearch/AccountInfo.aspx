<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.UserSearch.AccountInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/UserSearch/AccountInfo.js"></script>
    <script type="text/javascript" >
        var DeptID  = "<%=DeptID %>"; 
        var R_InputName = "<%=GetResource("InputName") %>"; 
        var URL_GateUrl = "<%= URL_GateUrl %>";
    </script>    
</head>
<body>
<form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">        
        <div class="popup_header">
            <h1><%=GetResource("DetailInfo") %></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbName" Width="80px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>&nbsp;</li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton></li>      
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
            </ul>
            <span style="float:right;color:Orange"><%=SelectDeptName %>&nbsp;</span>           
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:260px;height:355px;">
        	<!-- 테이블 시작 -->                      
             <%=DeptUserInfo %>
            <!-- 테이블 끝 -->
        </div>
        </div>
       
    </div>
</div>
<div style="display:none">
    <asp:LinkButton ID="lbtnSearch" runat="server" OnClick="lbtnSearch_Click"></asp:LinkButton>
</div>
<!-- //layout_info -->
</form>
 <!-- ############# 메일발송 ############## -->
<div style="display: none">   
    <form id="frmMail" name="frmMail" target="WriteMail" method="post">
        <input id="act" name="act" type="text" style="display: none" runat="server" />
        <input id="mode" name="mode" type="text" style="display: none" runat="server" />
        <input id="domain" name="domain" type="text" style="display: none" runat="server" />
        <input id="id" name="id" type="text" style="display: none" runat="server" />
        <input id="pw" name="pw" type="text" style="display: none" runat="server" />
        <input id="language" name="language" type="text" style="display: none" runat="server" />
        <input id="lp" name="lp" type="text" style="display: none" runat="server" />
        <input id="mbox" name="mbox" type="text" style="display: none" runat="server" />
        <input id="msgkey" name="msgkey" type="text" style="display: none" runat="server" />
        <input id="to" name="to" type="text" style="display: none" runat="server" />
        <input id="content" name="content" style="display: none" runat="server" />
        <input id="hideTopMenu" name="hideTopMenu" type="text" style="display: none" runat="server" /> 
        <input id="linkPage" name="linkPage" type="text" style="display: none" runat="server" />
        <input id="subject" name="subject" type="text" style="display: none" runat="server" />
    </form>
</div> 
<!-- ############# 메일발송 ############## -->

</body>
</html>