<%@ Control Language="C#" AutoEventWireup="true" Codebehind="NoticeBoard.ascx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Module.NoticeBoard" %>
<%@ Register TagPrefix="cc2" Namespace="Zeospace.DeskPlus.Component.Grids" Assembly="Zeospace.DeskPlus.Component.Grids" %>
<script type="text/javascript">
    
</script>
<h2><%=MenuName %></h2>  

<cc2:MultiSelectGrid ID="MsgList" runat="server" PageSize="50" DataKeyField="BoardID" AutoGenerateColumns="False" Width="100%" ImgPath="<%#ImgPath%>" AllowPaging="false">
    <Columns>
        <asp:BoundColumn DataField="BoardID" visible="False" />
        <asp:TemplateColumn visible="False">
            <itemstyle horizontalalign="Center" width="45px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# RecNo(DataBinder.Eval(Container.DataItem, "BoardID").ToString())%></itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="150px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# RecWriteUserName(GetLangName(
                        Eval("NameBase").ToString(),
                        Eval("NameKo").ToString(),
                        Eval("NameEn").ToString(),
                        Eval("NameJa").ToString(),
                        Eval("NameFr").ToString(),
                        Eval("NameZhcn").ToString(),
                        Eval("NameZhtw").ToString(),
                        PreLang))%></itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Left" width="" verticalalign="Middle"></itemstyle>
            <itemtemplate>	
                    <%# RecDepth(DataBinder.Eval(Container.DataItem,"Depth").ToString()) %>					
					<%# RecUrl(
                        DataBinder.Eval(Container.DataItem, "BoardID").ToString(),
                        DataBinder.Eval(Container.DataItem, "MenuID").ToString(),
                        DataBinder.Eval(Container.DataItem, "Title").ToString(),                                                           
                        DataBinder.Eval(Container.DataItem, "GroupID").ToString(), 
                        DataBinder.Eval(Container.DataItem, "ListOrder").ToString(),
                        DataBinder.Eval(Container.DataItem, "Depth").ToString())%>
		            <%# RecNewImage(DataBinder.Eval(Container.DataItem,"WriteDate").ToString()) %>						
				</itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
					<%# RecWriteDate(DataBinder.Eval(Container.DataItem,"WriteDate").ToString()) %>
				</itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="30px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
					<%# RecFilecount(DataBinder.Eval(Container.DataItem,"Filecount").ToString()) %>
				</itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="30px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
					<%# RecReadCount(DataBinder.Eval(Container.DataItem,"ReadCount").ToString()) %>
				</itemtemplate>
        </asp:TemplateColumn>
    </Columns>
</cc2:MultiSelectGrid>

