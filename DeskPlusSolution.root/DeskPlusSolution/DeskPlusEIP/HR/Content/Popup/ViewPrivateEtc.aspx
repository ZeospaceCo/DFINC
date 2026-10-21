<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPrivateEtc.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.ViewPrivateEtc" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css" /> 
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">
        
        <div class="popup_header">
            <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
            <ul class="btn_right">
                <li><div id="loading"></div></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:220px;height:220px;">
        	<!-- 테이블 시작 -->
            <table class="table_typeC">
                <colgroup>
                    <col width="70px"/>
                    <col/>
                    <col width="70px"/>
                </colgroup>
                <tbody>
                    <tr>
                        <th class="center"><b><%=GetResource("Gubun")%></b></th>
                        <th class="center"><b><%=GetResource("Reason2")%></b></th>
                        <th class="center"><b><%=GetResource("DocumentID")%></b></th>
                    </tr>
                    <tr id="trHoli" style="display: none">
                        <td class="center">    
                            <asp:Literal id="ltHoliGubun" runat="server"></asp:Literal>
                        </td>
                        <td>     
                            <asp:Literal id="ltHoliGubun2" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal id="ltHUDocumentID" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trOver" style="display: none">
                        <td class="center">    
                            <asp:Literal id="ltbOverGubun" runat="server"></asp:Literal>
                        </td>
                        <td>     
                            <asp:Literal id="ltOverRemark" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal id="ltPRDocumentID" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trLate" style="display: none">
                        <td class="center">     
                            <asp:Literal id="ltbLateGubun" runat="server"></asp:Literal>
                        </td>
                        <td>        
                            <asp:Literal id="ltLateRemark" runat="server"></asp:Literal>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr id="trEarly" style="display: none">
                        <td class="center">    
                            <asp:Literal id="ltbEarlyGubun" runat="server"></asp:Literal>
                        </td>
                        <td>      
                            <asp:Literal id="ltEarlyRemark" runat="server"></asp:Literal>
                        </td>
                        <td>&nbsp;</td>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script language="javascript">
        var bHoliAll = "<%= bHoliAll %>";
        var bLate = "<%= bLate %>";
        var bEarly = "<%= bEarly %>";
        var bOver = "<%= bOver %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/ViewPrivateEtc.js"></script>
</body>
</html>