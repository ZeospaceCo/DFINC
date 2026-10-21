<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.AccountInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />   
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" /> 
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">
        
        <div class="popup_header">
            <h1><%=GetResource("DetailInfo")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:320px;height:320px;">
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
                        <th><%=GetResource("Admin_UserManager_ltJoinDate")%></th>
                        <td colspan="2"><cc3:ClientSmartTextBox runat="server" ID="ctbJoinDate" Width="100px" InputMode="None" MaxLength="10" class="searchtext" ReadOnly></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserRegisterExtend_RetireDate")%></th>
                        <td colspan="2"><cc3:ClientSmartTextBox runat="server" ID="ctbRetireDate" Width="100px" InputMode="None" MaxLength="10" class="searchtext" ReadOnly></cc3:ClientSmartTextBox></td>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>   
    <script language="javascript">
        var CompanyID = "<%= CompanyID %>";
        var UserID = "<%= UserID %>";
        var URL_GateUrl = "<%= URL_GateUrl %>";
        var ActiveUser = "<%= ActiveUser %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";

        var R_Admin_User_Input_JoinDate = "<%=GetResource("Admin_User_Input_JoinDate") %>";
        var R_Admin_User_Input_RetireDate = "<%=GetResource("Admin_User_Input_RetireDate") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/AccountInfo.js"></script>
</body>
</html>