<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Footer" %>
    <div data-role="footer">
    <div>&nbsp;<br />&nbsp;<br />&nbsp;</div>
    <div class="nbf" id="divfooter" runat="server">
    <font class="nbflo"><a onclick="forcelogout();" href="../Portal/LogOut.aspx"><%=GetResource("LogOut")%></a><a href="<%=httpType %><%=fULLDOMAIN %>" id="pcUrl" runat="server">&nbsp;&nbsp;|&nbsp;&nbsp;<%=GetResource("PC")%></a></font>
    </div>
    </div>


