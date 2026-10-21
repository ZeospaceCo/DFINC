<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Config.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
<form id="form1" runat="server">

    <div id="layout_body">
        <div class="layout_full">

            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("SettingEnv")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnModClassInfo" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>  

            <div id="layout_content">
                <div class="row">
                    <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                        <table class="table_typeB">
                        <caption><%=GetResource("Setting2")%></caption>
			            <colgroup>
                            <col width="180px" />
                            <col />
			            </colgroup>
			            <tbody>
                        <tr>
                            <th><%=GetResource("AttachName")%> <%=GetResource("Upload")%><%=GetResource("CheckSize")%></th>
                            <td><cc3:ClientSmartTextBox runat="server" ID="ctbFileLength" Width="200px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>  (KB) (ex : 1 GB => 1048576 KB)</td>
                        </tr>            
                        <tr>
                            <th><%=GetResource("CarbonUserViewOption")%></th>
                            <td>
                                <asp:RadioButtonList ID="rblCarbonUserViewOption" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                                <span class="app_subtxt"> * <%=GetResource("Msg_CarbonUserViewOption")%></span>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("BasicListCnt")%></th>
                            <td>
                                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trreport">
                            <th><%=GetResource("ReportUserCarbonViewOption")%></th>
                            <td>
                                <asp:RadioButtonList ID="rblReportUserCarbonViewOption" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                                <span class="app_subtxt"> * <%=GetResource("Msg_ReportUserCarbonViewOption")%></span>
                            </td>
                        </tr>

                        </tbody>
                        </table>
                        
                    </div><!-- end col content-area-body-left -->
                </div><!-- end row  -->
            </div>
    
        </div>
    </div>
    <div id="layout_info">
        <div class="table_typeBbottom_left"><p></p></div>
    </div> 
    <div style="display:none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
    </div> 

</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Config/List.js?v=1"></script>

</body>
</html>

