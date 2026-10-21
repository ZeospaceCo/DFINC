<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" EnableViewState="false" Inherits="Zeospace.DeskPlus.Account.Web.Common.AccountInfo" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/Common/AccountInfo.js?v=2"></script>
    <script language="javascript">
        var URL_GateUrl = "<%= URL_GateUrl %>";

        var EmployeeNumber = "<%= CurrentUserInfo.EmployeeNumber %>";
        var UserEmpNO = "<%= UserEmpNO %>";

    </script>
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">        
        <div class="popup_header">
            <h1><%=GetResource("DetailInfo")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnDetail" Visible="false" runat="server" /></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:260px;height:260px;">
        	<!-- 테이블 시작 -->
            <table class="table_typeB">
                <caption>
                <%=GetResource("RegistContact")%>
                </caption>
                <colgroup>
                    <col width="80px"/>
                    <col/>
                    <col width="110px"/>
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("Admin_UserList_MSG_HTName")%></th>
                        <td>&nbsp;<asp:Literal id="ltName" runat="server"></asp:Literal>
                            <asp:Literal ID="ltImg" runat="server" Visible="false"></asp:Literal>
                        </td>
                        <td rowspan="5" class="last-child2" style="padding-top:9px;"><asp:image id="imgPhoto" runat="server" width="94px" height="124px" ></asp:image></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Basic")%><%=GetResource("Private_AccountInfo_ltUserDept")%></th>
                        <td>&nbsp;<asp:Literal id="ltDeptName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Both")%><%=GetResource("Private_AccountInfo_ltUserDept")%></th>
                        <td>&nbsp;<asp:Literal id="ltDeptNameEtc" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Private_AccountInfo_ltMobile")%></th>
                        <td>&nbsp;<asp:Literal id="ltMobile" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("OfficePhone")%></th>
                        <td>&nbsp;<asp:Literal id="ltPhone" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Private_AccountInfo_ltMail")%></th>
                        <td>&nbsp;<asp:Literal id="ltEmail" runat="server"></asp:Literal></td>
                        <td><asp:Literal id="ltSNS" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th class="last-child"><%=GetResource("Business")%></th>
                        <td class="last-child" colspan="2">
                        <div class="layout_scroll"  style="min-height:40px;height:40px;">
                        <asp:Literal id="ltMemo" runat="server"></asp:Literal>
                        </div>
                        </td>
                    </tr>
                </tbody>
            </table>  
            <!-- 테이블 끝 -->
        </div>
        </div>
    </div>
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
