<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SNS.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.SNS" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("LinkOutsideAccount")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>

        <div class="popup_layout_style3" style="min-height:160px;">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="150px"/>
                <col />
            </colgroup>
            <tbody>
                <tr  id="google" style="display:">
                    <th><%=GetResource("Google")%></th>
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbGoogle" width="250px" InputMode="None"></cc3:ClientSmartTextBox></td>
                    <!--th style="display:none;"><%=GetResource("Admin_UserManager_ltPassword")%></th>
                    <td style="display:none;"><cc3:ClientSmartTextBox runat="server" ID="ctbGooglePwd" TextMode="Password" style="width:1px" InputMode="None"></cc3:ClientSmartTextBox></td-->
                </tr>    
                <tr id="facebook" style="display:;">
                    <th><%=GetResource("FaceBook")%></th>                    
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbFacebook" width="250px" InputMode="None"></cc3:ClientSmartTextBox></td>
                    <!--th style="display:none;"><%=GetResource("Admin_UserManager_ltPassword")%></th>
                    <td style="display:none;"><cc3:ClientSmartTextBox runat="server" ID="ctbFacebookPwd" TextMode="Password" Visible="false" style="width:1px" InputMode="None"></cc3:ClientSmartTextBox></td-->
                </tr>
                <tr id="twitter" style="display:;">
                    <th><%=GetResource("Twitter")%></th>                    
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbTwitter" width="250px" InputMode="None"></cc3:ClientSmartTextBox></td>
                    <!--th style="display:none;"><%=GetResource("Admin_UserManager_ltPassword")%></th>
                    <td style="display:none;"><cc3:ClientSmartTextBox runat="server" ID="ctbTwitterPwd" TextMode="Password" Visible="false" style="width:1px" InputMode="None"></cc3:ClientSmartTextBox></td-->                    
                </tr>
                <tr  id="naverBlog" style="display:;">
                    <th><%=GetResource("NaverBlog")%></th>
                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbNaverBlog" width="250px" InputMode="None"></cc3:ClientSmartTextBox></td>
                    <!--th style="display:none;"><%=GetResource("Admin_UserManager_ltPassword")%></th>
                    <td style="display:none;"><cc3:ClientSmartTextBox runat="server" ID="ctbNaverBlogPwd" TextMode="Password" Visible="false" style="width:1px" InputMode="None"></cc3:ClientSmartTextBox></td-->
                </tr>                
            </tbody>
            </table>
         <!--div id="modPwdComment" style="margin-top:10px;display:none;">            
            <%=GetResource("InputChangedPassword")%>
        </div-->            
        </div>
    </div>
    </div>

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=UserID %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/SNS.js"></script>        
    <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Account/Js/MyInfo/SNS.js");
    </script>
</body>
</html>