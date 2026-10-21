<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Admin.Content.GetUserDept" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div id="popup_header" style="min-width: 400px;">
            <h1><%=GetResource("TitGetU")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <!-- 유연근무시간 설정 시작 //-->
            <div class="layout_subbox" style="padding-top: 5px">
                <h2><%=GetResource("WorkTimeManage")%></h2>
            </div>
            <table class="table_typeB" style="margin-bottom: 1px">
            <caption><%=GetResource("WorkTimeManage")%></caption>
            <colgroup>
                <col width="60px" />
                <col />
                <col width="60px" />
                <col />
		    </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("WeekDay")%></th>
                    <td colspan="3">
                        <asp:CheckBoxList ID="cbxWeek" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Text="월" Value="2" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="화" Value="3" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="수" Value="4" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="목" Value="5" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="금" Value="6" Selected="True"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("WSTime")%></th>
                    <td><asp:DropDownList ID="ddlEtcWSTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    <th><%=GetResource("WCTime")%></th>
                    <td><asp:DropDownList ID="ddlEtcWCTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>            
            </tbody>
            </table>

            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseIcon="true" UseActiveDept="true" UseCheckBoxMember="true" UseCheckBoxGroup="true" />

        </div>                
    </div>
</div>


<div style="display: none">    
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />    
</div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var Func = "<%=Func%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>";
        var R_BaseWorkTimeError = "<%=GetResource("BaseWorkTimeError") %>";

        var R_Msg_Sel = "<%=GetResource("Msg_Sel")%>";
        var R_Msg_Add = "<%=GetResource("Msg_Add")%>";
        var R_Msg_Admin = "<%=GetResource("Msg_Admin")%>";
        var R_Msg_WeekDay = "<%=GetResource("Msg_WeekDay")%>"; // 요일을 선택하세요.
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Admin/Content/GetUserDept.js"></script>     
</body>
</html>