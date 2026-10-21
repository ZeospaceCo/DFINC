<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EPMenu.ascx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Menu.EPMenu" %>
<div id="layout_menu">
    <div class="UTIL">
    	<div class="menu_logo"><a href="/DeskPlusEIP/EIP/EPDefault.aspx" target="_top"><img src="<%=BaseImgPath%>/Portal/logo.gif" title="SMART PLUS" alt="SmartPlus Logo" /></a></div>
        <ul class="menu_util">
            <P class="date_title"><asp:Literal id="lbTodayDate" runat="server"></asp:Literal></P>

            <li><a href="http://ep.zeospace.com/UCWare_m_Setup.exe">메신저다운로드</a></li>
            <li><asp:HyperLink ID="SignOutLink" runat="server" Text="로그아웃"></asp:HyperLink></li>
            <li><asp:HyperLink ID="MyInfoLink" runat="server" Text="내정보관리"></asp:HyperLink></li>
            <li id="LiAdminLink" runat="server" visible="false"><asp:HyperLink ID="AdminLink" runat="server" Text="슈퍼관리자"></asp:HyperLink></li>
            <li id="LiPersonalLink" runat="server" visible="false"><asp:HyperLink ID="PersonalLink" runat="server" Text="개인화"></asp:HyperLink></li>
            <li id="LiPersonalOutLink" runat="server" visible="false"><asp:HyperLink ID="PersonalOutLink" runat="server" Text="나의페이지"></asp:HyperLink></li>
        </ul>
    </div><!-- //UTIL --> 
    
    <div class="GNB">
        <ul class="menu_gnb">
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/EIP/EPDefault.aspx', 'EP')">Home</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/Board/Default.aspx', '')">게시판</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/Pim/Default.aspx', '')">일정관리</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/Address/Default.aspx', '')")>주소록</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/Pim/Target/List.aspx', '')")>목표관리</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/WebHard/Default.aspx', '')")>웹하드</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/RM/Default.aspx', '')")>자원관리</a></li>
        <li><a href="javascript:EPMenu.MovePage('/DeskPlusEIP/Opinion/Default.aspx', '')")>설문조사</a></li>
        <li><a href="javascript:EPMenu.MovePage('http://ep.zeospace.com/DeskPlusEDM/Authentication/LoginOutSide.aspx?USERID=test01&USERPASSWORD=tfttest', '')")>문서관리</a></li>
        <asp:DataList ID="dlMenuList" runat="server"  RepeatDirection="Horizontal" EnableViewState="False" CellPadding="0" CellSpacing="0">
            <ItemTemplate>

            </ItemTemplate>
            <SelectedItemTemplate>

            </SelectedItemTemplate>
        </asp:DataList>        
        </ul>
        <ul class="menu_cate" id="menu_cate">
            <li><a href="#">WORK SPACE</a>
                <ul>
                    <li><a href="#">WORK SPACE</a></li>
                    <li><a href="#">CO SPACE</a></li>
                    <li><a href="#">MY SPACE</a></li>
                </ul>
            </li>
        </ul>
    </div><!-- //GNB --> 

    <div class="SUB">
        <ul class="menu_sub">
        <asp:DataList ID="dlSubMenuList" RepeatDirection="Horizontal" EnableViewState="False" CellSpacing="0" CellPadding="0" runat="server">
        <ItemTemplate>
            
        </ItemTemplate>
        <SelectedItemTemplate>
            
        </SelectedItemTemplate>
        </asp:DataList>
        </ul>
    </div><!-- //SUB --> 
</div><!-- //layout_menu --> 
