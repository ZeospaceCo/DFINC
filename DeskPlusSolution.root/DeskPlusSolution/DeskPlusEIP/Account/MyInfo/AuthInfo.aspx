<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.AuthInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/AuthInfo.js"></script>
</head>

<body>
    <form id="form1" runat="server">
    <div id="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Auth")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3"  style="height: 320px;min-height: 320px;">
 
                    <!-- 권한설정 start-->   
                     
                        <table class="table_typeB" id="contentTable">
                        <caption></caption>
                        <colgroup>                        
                            <col width="70%"/>
                            <col width="30%"/>
			            </colgroup>
			            <tbody>
				        <tr>                        
                            <th class="center"><%=GetResource("Auth")%></th>                        
                        </tr>
                        <%=AuthTable%>                    
                        </tbody>
                        </table>
           
                    <!-- 권한설정 end-->   
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
