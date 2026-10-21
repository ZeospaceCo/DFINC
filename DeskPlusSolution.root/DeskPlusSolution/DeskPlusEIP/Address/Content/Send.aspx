<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.Send" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup"> 
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("SendAddress") %></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnSubmit" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <table cellspacing="0" border="0" style="width:100%;"> 
            <tr> 
                <td valign="top" width="270px" style="text-align:center;"> 
                <div class="layout_scroll"  style="width:270px; min-height:417px; height:417px; overflow:hidden">
                    <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
                </div>
                </td>
                <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                </td>
                <td valign="top">
                    <div class="layout_scroll"  style="min-height:417px;height:417px;">                       
                    <table cellspacing="0" id="contentTable" class="table_typeA" style="width:100% !important;">
                        <caption><%=GetResource("SentList")%>
                        </caption>
                        <colgroup>
                            <col />
                            <col width="60px" />
                        </colgroup>
                        <thead>
                        <tr>
                            <th ><%=GetResource("SelectedUser") %></th>
                            <th><%=GetResource("Delete") %></th>
                        </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        </table>
                    
                    </div>
                </td>            
            </tr> 
            </table>    
		        
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var CategoryID = "";
        var Types = "";
        var AuthArrayList = "";
        var ListID = "<%=ListID%>";
        var RowMax = 0;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_CompleteAddressSend = "<%=GetResource("CompleteAddressSend")%>";
        var R_SelectSendAddressUser = "<%=GetResource("SelectSendAddressUser") %>";
        var R_Del = "<%=GetResource("Delete") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/Send.js"></script>
</body>
</html>
