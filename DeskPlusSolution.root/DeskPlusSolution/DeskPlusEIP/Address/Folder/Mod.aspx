<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Folder.Mod" %>
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
		<table class="layer_popupA_box"><tr><td>
		<div class="layer_popupA_gap">
        <!-- 테이블 상단환경 끝 -->
		<table cellspacing="0" class="layer_popupA"> 
        <tr> 
            <td>
                <h1><%=GetResource("ChangeFolder")%></h1>  
                <ul class="btn_right">
                   <cc3:ClientSmartButton ID="CbtnSubmit" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                   <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
                <h6></h6>
            </td> 
        </tr>
        <tr> 
          	<td valign="top">         
                
		        <table cellspacing="0" class="layer_popupA" border="0"> 
                <tr> 
          	        <td valign="top" width="50%" >
                        <div class="layer_scroll_box_200">
                        <table cellspacing="0" class="table_typeB" style="width:100% !important;">  
                            <thead>
                                <tr>
                                    <th class="table_title"><%=GetResource("SelectedUser") %></th>
                                    <th class="table_lock"><%=GetResource("Delete") %></th>
                                </tr>
                            </thead>
                            <tbody id="dataContainer">
                        <tr id="rowdata" oid="<%# Eval("OwnerID") %>" otype="<%# Eval("Type") %>">
                            <td style="width:170px; overflow:hidden;">
                                <asp:Label ID="LblName" runat="server" Text="<%# Eval("Name") %>"></asp:Label>
                            </td>
                            <td style="width:50px" class="center">
                                <a href="javascript:Folder.Del('<%# Eval("OwnerID") %>','<%# Eval("Type") %>')"><img src="<%=BaseImgPath%>/Btn/folderset_del.gif" /></a>
                            </td>
                        </tr>
                        </tbody></table>
                        </div>
          	        </td>           
          	        <td class="center" valign="top" style="padding-top:60px;width:80px;">
                            <cc3:ClientSmartButton ID="CbtnAdd" runat="server"></cc3:ClientSmartButton>
                    </td>
           	        <td valign="top"  width="40%" style="text-align:left;"> 
                        <ul>
                        <li><cc3:ClientSmartTextBox ID="CTBName" InputMode="None" MaxLength="250" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></li>
                        </ul> 
          	            <div class="layer_scroll_box">
                            <%=GetResource("AddressTree") %>
                         </div>
                         <li><%=GetResource("SelectedAddressAddMove") %></li>
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
    </form>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var MultiLang = "<%=MultiLang %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Folder/Mod.js"></script>    
</body>
</html>
