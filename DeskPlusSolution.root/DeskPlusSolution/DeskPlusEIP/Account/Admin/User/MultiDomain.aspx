<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiDomain.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.MultiDomain" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>

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
            <h1><%=GetResource("MultiDomain")%></h1>
            <ul class="btn_right">                
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="60px"/>
                <col width="300px"/>
                <col width="60px"/>
                <col />
            </colgroup>
            <tbody>
                <tr id="baseInput">
                    <th><%=GetResource("Private_AccountInfo_ltMail")%></th> 
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbMailID" Width="100px" InputMode="None"  class="searchtext">
                        </cc3:ClientSmartTextBox>@<asp:DropDownList ID="ddlMailDomain" runat="server" CssClass="searchselect form-control" Width="150px"></asp:DropDownList>
                    </td>
                    <th><%=GetResource("Admin_UserManager_GridHeader7")%></th>
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbMailPwd" Width="120px" InputMode="None"  class="searchtext"></cc3:ClientSmartTextBox>
                    </td>
                    <td><ul class="btn_smallL"><li><cc3:ClientSmartButton ID="cbtnMailCheck" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li></ul>
                    <ul class="btn_smallL"><li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li></ul></td>
                </tr>
            </tbody>
            </table>
            
            <div class="layout_subbox">
                <h2><%=GetResource("MultiDomainState")%></h2>
            </div>                
            <!-- 테이블 시작 //-->
            <div class="layout_scroll"  style="min-height:180px;height:180px;">            
            <div id="placeholder" style="clear: both;"></div>            
            </div>
            <!-- 테이블 끝 //-->   
        </div>
    </div>
    </div>
    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=UserID %>";
        var MailCheck = false;

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        
        var R_InputBaseMail = "<%=GetResource("InputBaseMail") %>";
        var R_InputPassword = "<%=GetResource("InputPassword") %>";
        var R_InputMailPassword = "<%=GetResource("InputMailPassword") %>";
        
        var R_OnlyOneMail = "<%=GetResource("OnlyOneMail") %>";
        var R_RepetitionMail = "<%=GetResource("RepetitionMail") %>";
        var R_CheckMailRepetition = "<%=GetResource("CheckMailRepetition") %>";
        var R_InputPassword = "<%=GetResource("InputPassword") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_MailID = "<%=GetResource("MailID") %>";
        var R_MailDomain = "<%=GetResource("MailDomain") %>";
        var R_BaseMail = "<%=GetResource("BaseMail") %>";
        var R_Admin_UserManager_cbsUserDel = "<%=GetResource("Admin_UserManager_cbsUserDel") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_CheckBaseMail = "<%=GetResource("CheckBaseMail") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/User/MultiDomain.js"></script>
     <script type="text/javascript">
         //JsCall("../../Js/Admin/User/MultiDomain.js");         
    </script>
</body>
</html>
