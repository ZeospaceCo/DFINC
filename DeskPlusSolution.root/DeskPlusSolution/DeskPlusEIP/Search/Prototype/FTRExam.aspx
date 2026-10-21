<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FTRExam.aspx.cs" Inherits="Zeospace.DeskPlus.Search.Web.Prototype.FTRExam" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Search.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
</head>
<body>
    <form id="form1" runat="server">
  <div class="wrapper">
				
<fieldset style="width: 100%;float:left;">
	<legend>테스트 DB 데이타 (<asp:LinkButton ID="lbtnAddIndex" runat="server" 
            onclick="lbtnAddIndex_Click">인덱스생성</asp:LinkButton>
        ) (<asp:LinkButton ID="lbtnToText" runat="server" 
            onclick="lbtnToText_Click">파일변환</asp:LinkButton>
        )</legend> 

        <div style="float:left;padding-right:10px;">
			<label for="tbfolder">폴더경로</label>:
            <asp:TextBox ID="tbFolder" runat="server" Text="c:\Imsi"></asp:TextBox>
		</div>

        <div style="float:left;padding-right:10px;">
			<label for="tbfolder">ToTEXT 대상 폴더</label>:
            <asp:TextBox ID="tbToText" runat="server" Text="/DeskPlusEIP/FileData/"></asp:TextBox>
		</div>
</fieldset>
		
<fieldset style="width: 100%;float:left;">
	<legend>인덱스 데이타
		(<asp:LinkButton ID="lbtnOptimize" runat="server" onclick="lbtnOptimize_Click">옵티마이져</asp:LinkButton>
        )
		(<asp:LinkButton ID="lbtnDelIndex" runat="server" onclick="lbtnDelIndex_Click">인덱스삭제</asp:LinkButton>
        )
	</legend> 
    <asp:GridView ID="gvIndex" runat="server" BackColor="White" 
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" Height="50px" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
</fieldset>
		
<div style="clear:both;"></div>
<fieldset>
	<legend>Messages</legend>
	<span style="color: green;">
        <asp:Label ID="lbOKMsg" runat="server" Text=""></asp:Label></span>
	<span style="color: red;"><asp:Label ID="lbFailMsg" runat="server" Text=""></asp:Label></span>
</fieldset>

<fieldset >
	<legend>인덱스 추가/수정 테스트</legend>
        <div style="float:left;padding-right:10px;">
			<label for="tbID">Id</label>:
            <asp:TextBox ID="tbID" runat="server"></asp:TextBox>
		</div>
		<div style="float:left;padding-right:10px;">
			<label for="tbName">FileName</label>:
            <asp:FileUpload ID="fup" runat="server" style="width:300px;" />
		</div>
        
    <asp:Button ID="btnAddIndex" runat="server" Text="추가/수정" 
        onclick="btnAddIndex_Click" />
</fieldset>
      
<fieldset  style="width: 45%;float:left;">
	<legend>사용자 정의 검색</legend>
	<p><em>"1 3 5", "City", "Russia India", "bel mos ind"</em></p>
    <asp:TextBox ID="tbCustomSearch" runat="server"></asp:TextBox>
    <asp:DropDownList ID="ddlCustomSearchField" runat="server">
    <asp:ListItem Value="" Text="(전체)"></asp:ListItem>
    <asp:ListItem Value="ID" Text="ID"></asp:ListItem>
    <asp:ListItem Value="FileName" Text="FileName"></asp:ListItem>
    <asp:ListItem Value="Contents" Text="Contents"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnCustomSearch" runat="server" Text="검색" 
        onclick="btnCustomSearch_Click" />
</fieldset>

<fieldset  style="width: 45%;float:right;">
	<legend>기본검색</legend>
	<p>검색 방법 <em>"bel* mos* ind*"</em></p>

    <asp:TextBox ID="tbSearch" runat="server"></asp:TextBox>
    <asp:DropDownList ID="ddlSearchField" runat="server">
    <asp:ListItem Value="" Text="(전체)"></asp:ListItem>
    <asp:ListItem Value="ID" Text="ID"></asp:ListItem>
    <asp:ListItem Value="FileName" Text="FileName"></asp:ListItem>
    <asp:ListItem Value="Contents" Text="Contents"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="btnSearch" runat="server" Text="검색" onclick="btnSearch_Click" />
</fieldset>
<div style="clear:both;"></div>
	
<fieldset>
	<legend>검색결과</legend>
    <asp:GridView ID="gvResult" runat="server" BackColor="White" 
        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="White" />
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
</fieldset>	

<script type="text/javascript">
    $(document).ready(function () {
        $('#SearchTerm').focus();
        $('.grid tr:even').css("background", "silver");

    });
</script>
	</div>
    </form>
</body>
</html>
