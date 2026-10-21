<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSimple.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.AddSimple" %>
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
        <!-- 테이블 상단환경 -->
		
		<table class="layer_popupA_box" border="0"><tr><td>
		<div class="layer_popupA_gap">
        <!-- 테이블 상단환경 끝 -->
        
		<table cellspacing="0" class="layer_popupA"> 
        <tr> 
            <td>
                <h1><div id="modalTitle"><%=GetResource("AddSimpleAddress") %></div></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Height="10" Width="100" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" Height="10" Width="100" />
                </ul>
            </td> 
        </tr>
        <tr> 
          	<td valign="top"> 
            <!-- 테이블 내용시작 -->    
            <table cellspacing="0" border="0" class="table_typeF">
            <tr>
               <th><%=GetResource("FirstName") %></th>
               <td><cc3:ClientSmartTextBox ID="ctbName" runat="server" InputMode="None" Width="130px"></cc3:ClientSmartTextBox></td>
               <th><%=GetResource("E-Mail") %></th>
               <td><cc3:ClientSmartTextBox ID="ctbMail" runat="server" InputMode="None" Width="130px"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
               <th><%=GetResource("CellPhone") %></th>
               <td colspan="3">
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbMobile1" runat="server" MaxLength="3" InputMode="None" Width="50px"></cc3:ClientSmartTextBox>&nbsp;-&nbsp;</li>
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbMobile2" runat="server" MaxLength="4" InputMode="None" Width="50px"></cc3:ClientSmartTextBox>&nbsp;-&nbsp;</li>
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbMobile3" runat="server" MaxLength="4" InputMode="None" Width="50px"></cc3:ClientSmartTextBox></li>
               </td>            
            </tr>
            <tr>
               <th><%=GetResource("CompanyName") %></th>
               <td><cc3:ClientSmartTextBox ID="ctbCompany" runat="server" InputMode="None" Width="130px"></cc3:ClientSmartTextBox></td>
               <th><%=GetResource("DeptName") %></th>
               <td><cc3:ClientSmartTextBox ID="ctbDeptName" runat="server" InputMode="None" Width="130px"></cc3:ClientSmartTextBox></td>
            </tr>    
            <tr>
               <th><%=GetResource("CompanyTel") %></th>
               <td colspan="3">
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbPhone1" runat="server" MaxLength="3" InputMode="None" Width="50px"></cc3:ClientSmartTextBox>&nbsp;-&nbsp;</li>
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbPhone2" runat="server" MaxLength="4" InputMode="None" Width="50px"></cc3:ClientSmartTextBox>&nbsp;-&nbsp;</li>
               <li style="float:left;"><cc3:ClientSmartTextBox ID="ctbPhone3" runat="server" MaxLength="4" InputMode="None" Width="50px"></cc3:ClientSmartTextBox></li>               
               </td>
            </tr>         
		    </table> 
            </td>
        </tr>
        </table>
        <!-- 테이블 하단환경 -->
        </div>
        </td></tr></table>
		
        <!-- 테이블 하단환경 끝 -->  
        <div style="display:none;">
            <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
        </div>  
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID %>";
        var Types = "<%=Types %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";        
        var R_InputFitstName = "<%=GetResource("InputFitstName") %>";
        var R_InputEmail = "<%=GetResource("InputEmail") %>";
        var R_NotEmailFormat = "<%=GetResource("NotEmailFormat") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/AddSimple.js"></script>
</body>
</html>
