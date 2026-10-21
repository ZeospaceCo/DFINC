<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMailDeep.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.SendMailDeep" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
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

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var CategoryID = "<%=categoryID %>";
        var ListID = "<%=listID %>";
        var URL_GateUrl = "<%= URL_GateUrl %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/SendMailDeep.js" ></script>

</body>
</html>
