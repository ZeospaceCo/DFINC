<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Portal.Add" %>
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
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MainPageManage")%></h1>
            <!-- 버튼 //-->
            <span class="cm_subtxt">* <%=GetResource("CMNScript3")%></span>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table style="width:100%">
                    <colgroup>
                    <col width="45%" />
                    <col width="10%" />
                    <col width="45%" />
                    </colgroup>
                    <tr>
                        <td>
                            <h2><%=GetResource("UsingMenu")%></h2>
                            <div style="clear:both;"></div>
                            <div class="mid">
                                <ul class="cm_menu_item">
                                    <asp:ListBox ID="lbxParentCategory" runat="server" Width="100%" Rows="26" style="height:350px;"></asp:ListBox>
                                </ul>
                            </div>
                        </td>
                        <td align="center" style="vertical-align: middle">
                            <ul>
                                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                                <li>&nbsp;</li>
                                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </td>
                        <td>
                            <h2><%=GetResource("DisplayMenu")%></h2>
                            <div style="clear:both;"></div>
                            <div class="mid">
                            <ul class="cm_menu_item">
                                    <asp:ListBox ID="lbxSubCategory" runat="server" Width="100%" Rows="26" style="height:350px;"></asp:ListBox>
                            </ul>
                            </div>
                        </td>
                    </tr>
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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CommunityID = "<%=CommunityID %>";
        var Order = "<%=Order %>";
        var R_CheckAddMenu =  "<%=GetResource("CheckAddMenu") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_CheckDelMenu = "<%=GetResource("CheckDelMenu") %>";
    </script>
    <script type='text/javascript' src='/DeskPlusEIP/Community/Js/Site/Admin/Function/Portal/Add.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/Function/Portal/Add.js");
    </script>
</body>
</html>
