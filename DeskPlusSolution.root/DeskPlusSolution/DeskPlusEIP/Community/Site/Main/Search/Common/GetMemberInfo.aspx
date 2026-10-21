<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetMemberInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Main.Search.Common.GetMemberInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">        
        <div class="popup_header">
            <h1><%=GetResource("UserInfo")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnDrop" runat="server" Class="btn btn-danger" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:340px;height:340px;">
        	<!-- 테이블 시작 -->
            <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="80px"/>
                    <col/>
                    <col width="110px"/>
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("UserName")%></th>
                        <td>&nbsp;<asp:Literal id="ltName" runat="server"></asp:Literal></td>
                        <td rowspan="5" class="last-child2" style="padding-top:9px;"><asp:image id="imgPhoto" runat="server" width="94px" height="124px" ></asp:image></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Basic")%><%=GetResource("DeptName")%></th>
                        <td>&nbsp;<asp:Literal id="ltDeptName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Both")%><%=GetResource("DeptName")%></th>
                        <td>&nbsp;<asp:Literal id="ltDeptNameEtc" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Mobile")%></th>
                        <td>&nbsp;<asp:Literal id="ltMobile" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("OfficePhone")%></th>
                        <td>&nbsp;<asp:Literal id="ltPhone" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("MAIL")%></th>
                        <td>&nbsp;<asp:Literal id="ltEmail" runat="server"></asp:Literal></td>
                        <td><asp:Literal id="ltSNS" runat="server"></asp:Literal></td>
                    </tr>
                    <tr id="Des1" runat="server">
                        <th class="last-child"><%=GetResource("Introduce")%></th>
                        <td colspan="2">
                        <div class="layout_scroll"  style="min-height:100px;height:100px;">
                        <asp:Literal id="ltMemo" runat="server"></asp:Literal>
                        </div>
                        </td>
                    </tr>
                    <tr id="Des2" runat="server">
                        <th class="last-child"><%=GetResource("Introduce")%></th>
                        <td colspan="2">
                        <cc3:ClientSmartTextBox ID="ctbMemo" TextMode="MultiLine" MaxLength="250" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">            
        var CommunityID = "<%=CommunityID %>";
        var R_InputDes = "<%=GetResource("InputDes") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmDrop = "<%=GetResource("ConfirmDrop") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Main/Search/Common/GetMemberInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Main/Search/Common/GetMemberInfo.js");
    </script>
</body>
</html>
