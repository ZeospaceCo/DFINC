<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Del.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.General.Del" %>
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
            <h1><%=GetResource("DelCommunity")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>    
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="120px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("DissolutionReason")%></th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbDelDes" MaxLength="250" runat="server" CssStyle="width:95% !important;" TextMode="MultiLine"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>                
			        </tbody>
		            </table> 
                    <div class="cm_gap10"></div>
                    <span class="cm_subtxt">* <%=GetResource("DisDes1")%></span>
                </div>
            </div>
        </div>        
    </div>
</div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>    
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CommunityID = "<%=CommunityID %>";
        var BtnCheck = "N";
        var R_Processing = "<%=GetResource("Processing") %>";     
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_InputDissolution = "<%=GetResource("InputDissolution") %>";
        var R_DelConfirmCommunity = "<%=GetResource("DelConfirmCommunity") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Admin/General/Del.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/General/Del.js");
    </script>
</body>
</html>
