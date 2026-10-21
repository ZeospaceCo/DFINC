<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Company.AddUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1>사용자정보</h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <table class="table_typeB">
                    <caption>사용자정보</caption>
                    <colgroup>
                        <col width="70px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                이름
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbName" Required="true" Width="95%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                로그인
                            </th>
                            <td>
                                <span class="aLeft">
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbLoginID" Width="98%"  Required="true" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                                </span>
                                <ul class="btn_smallL" style="padding-left:10px;">
                                    <li><cc3:ClientSmartButton ID="cbtnLoginCheck" runat="server" class="btn btn-sm" /></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                패스워드
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbPassword" Width="98%"  Required="true" TextMode="Password" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CompanyID %>";
        var DeptID = "<%= DeptID %>";
        var LoginCheck = false;
        var R_OnlyOneLoginID = "<%=GetResource("OnlyOneLoginID") %>";
        var R_RepetitionLoginID = "<%=GetResource("RepetitionLoginID") %>";
        var R_CheckLoginRepetition = "<%=GetResource("CheckLoginRepetition") %>";
        var R_InputName = "<%=GetResource("InputName") %>";  
        var R_InputLoginID = "<%=GetResource("InputLoginID") %>";
        var R_InputPassword = "<%=GetResource("InputPassword") %>";
        var R_SUCCESS = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Company/AddUser.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Company/AddUser.js");
    </script>
</body>
</html>
