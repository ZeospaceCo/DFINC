<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Admin.Config.Default" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%=GetAdminMenuByID("Setting", "Text")%></h1>
            <ul class="btn_right">        
                <cc3:ClientSmartButton ID="CbtnModClassInfo" BeginEndTag="li" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>        
            </ul>
        </div>

        <div id="layout_content">
            <div class="layout_subbox">
                <h2><%=GetResource("SettingEnvironment")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SettingEnvironment")%></caption>
			<colgroup>
                <col width="120px" />
                <col />
			</colgroup>
            <tbody>
            <tr>
                <th class="last-child"><%=GetResource("AttachFile")%><br /><%=GetResource("UpLoadLimitedSize")%></th>
                <td class="last-child">
                    <cc3:ClientSmartTextBox runat="server" ID="ctbFileLength" Width="200px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>    (KB) (ex : 1 GB => 1048576 KB)
                </td>
            </tr>
            </tbody>          
            </table>

            <div class="layout_subbox">
                <h2><%=GetResource("Design")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SettingEnvironment")%></caption>
			<colgroup>
                <col width="120px" />
                <col />
			</colgroup>
			<tbody>
                <tr id="Tr1" runat="server">
                    <th><%=GetResource("ListCount")%></th>
                    <td><asp:DropDownList ID="ddlListCount" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("UseActiveX")%></th>
                    <td class="last-child">
                        <asp:DropDownList ID="ddlUseActiveX" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </td>
                </tr>
                </tbody>
            </table>

        </div>
    </div>
</div>

    <div style="display:none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
    </div>
    
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Admin/Config/Default.js"></script>
</body>
</html>

