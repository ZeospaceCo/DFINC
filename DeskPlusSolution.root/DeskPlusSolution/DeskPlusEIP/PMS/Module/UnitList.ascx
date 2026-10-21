<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UnitList.ascx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Module.UnitList" %>
<%@ Register TagPrefix="cc2" Namespace="Zeospace.DeskPlus.Component.Grids" Assembly="Zeospace.DeskPlus.Component.Grids" %>

<script type="text/javascript">
    function UnitRead(p_UnitID)
    {
        var  projectID      = "<%=ProjectID %>";
        var rqProjectID = "<%=RqProjectID %>";      
        

        ParamArray = {
            'projectID': projectID,
            'menuType': "2",
            'rqProjectID': rqProjectID,
            'pageListUrl': "List.aspx",
            'unitID': p_UnitID
        };

        CallEncrypt(CallGetParam());

        self.location.href = "../Unit/View.aspx?p=" + GetEncryptValue;
    }
</script>
<!-- 단위 업무 -->

<h2><%=MenuName %></h2>  

<cc2:MultiSelectGrid ID="MsgList" runat="server" PageSize="50" DataKeyField="UnitID" AutoGenerateColumns="False" Width="100%" BaseImgPath="<%#BaseImgPath%>" AllowPaging="false">
    <Columns>
        <asp:BoundColumn DataField="UnitID" visible="False" />
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="45px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# DataBinder.Eval(Container.DataItem, "Code").ToString() %></itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="left" width="" verticalalign="Middle"></itemstyle>
            <itemtemplate>
            <a onclick="UnitRead('<%# DataBinder.Eval(Container.DataItem, "UnitID").ToString() %>')">
                <%# DataBinder.Eval(Container.DataItem, "Subjects").ToString()%>
            </a>
            </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="50px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# RecStatus(DataBinder.Eval(Container.DataItem,"Status").ToString()) %></itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
                <a onclick="AccountInfo('<%# Eval("WriteUserID").ToString() %>');return false;" style="cursor:pointer">
                <%# RecName(GetLangName(
                        Eval("NameBase").ToString(),
                        Eval("NameKo").ToString(),
                        Eval("NameEn").ToString(),
                        Eval("NameJa").ToString(),
                        Eval("NameFr").ToString(),
                        Eval("NameZhcn").ToString(),
                        Eval("NameZhtw").ToString(),
                        PreLang))%></a>
            </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <headerstyle horizontalalign="Center" height="45px" verticalalign="Middle"></headerstyle>
            <itemstyle horizontalalign="Center" width="80px" height="40px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
	        <%# RecDate(DataBinder.Eval(Container.DataItem, "PlanStartDate").ToString())%><br />
	        <%# RecDate(DataBinder.Eval(Container.DataItem, "StartDate").ToString())%>
            </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <headerstyle horizontalalign="Center" height="45px" verticalalign="Middle"></headerstyle>
            <itemstyle horizontalalign="Center" width="80px" verticalalign="Middle"></itemstyle>
            <itemtemplate>
	        <%# RecDate(DataBinder.Eval(Container.DataItem, "PlanEndDate").ToString())%><br />
	        <%# RecDate(DataBinder.Eval(Container.DataItem, "EndDate").ToString())%>
            </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="70px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# DataBinder.Eval(Container.DataItem,"PlanDay").ToString() %></itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="50px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# DataBinder.Eval(Container.DataItem,"WeightPercent").ToString() %>%</itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="50px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# DataBinder.Eval(Container.DataItem,"Process").ToString() %>%</itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <itemstyle horizontalalign="Center" width="70px" verticalalign="Middle"></itemstyle>
            <itemtemplate><%# RecProcessPercent(DataBinder.Eval(Container.DataItem,"WeightPercent").ToString(), DataBinder.Eval(Container.DataItem,"Process").ToString()) %>%</itemtemplate>
        </asp:TemplateColumn>
    </Columns>
</cc2:MultiSelectGrid>
