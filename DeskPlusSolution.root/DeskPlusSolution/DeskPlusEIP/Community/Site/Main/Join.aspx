<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Join.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Main.Join" %>
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
<div id="layout_body">
<div class="layout_full">
    <div id="layout_header">
        <h1><%=GetResource("JoinApply")%></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
        </ul>
    </div>

    <div id="layout_content">
         <div class="row">
			<div class="col-12 content-area-body"><!-- width 100%(col-12) -->  
                <!-- 개설신청 시작 //-->
                <table class="table_typeB">
		        <caption><%=GetResource("ComInfo")%></caption>
		        <colgroup>
                    <col width="120px" />
                    <col />
                    <col width="120px" />
                    <col />
		        </colgroup>
		        <tbody>
                    <tr>
                        <th><%=GetResource("ComHost")%></th>
                        <td><asp:Label ID="lbOpenName" runat="server"></asp:Label></td>
				        <th><%=GetResource("Membership")%></th>
				        <td><asp:Label ID="lbMembership" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("JoinType")%></th>
                        <td><asp:Label ID="lbType" runat="server"></asp:Label></td>
                        <th><%=GetResource("ComApplyDate")%></th>
                        <td><asp:Label ID="lbOpenDate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("ComName")%></th>
                        <td colspan="3"><asp:Label ID="lbCmnName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("CommunityDes")%></th>
                        <td colspan="3" style="height:120px;">
                            <asp:Label title="" ID="lbDescription" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th class="last-child"><%=GetResource("Introduce")%></th>
                        <td colspan="3">
                        <ul>
                            <li><cc3:ClientSmartTextBox ID="ctbMyDescription" InputMode="None" runat="server" MaxLength="500" CssStyle="width:98% !important;" ></cc3:ClientSmartTextBox></li>      
                        </ul>
                        </td>
                    </tr>
		        </tbody>
		        </table> 
             </div>
           </div>
    </div>
</div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        var CommunityID = "<%=CommunityID %>";
        var Type = "<%=Type %>";
        var JoinType = "<%=JoinType %>";
        var Grade = "<%=Grade %>";
        var JoinCheck = "<%=JoinCheck %>";
        var StatusCheck = "<%=StatusCheck %>";
        var R_CompleteApply = "<%=GetResource("CompleteApply") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_InputDes = "<%=GetResource("InputDes") %>";
        var R_ConfirmWaiting = "<%=GetResource("ConfirmWaiting") %>";
        var R_RejectJoin = "<%=GetResource("RejectJoin") %>";
        var R_ConfirmJoin = "<%=GetResource("ConfirmJoin") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Main/Join.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Main/Join.js");
    </script>
</body>
</html>
