<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Config.Setting" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>  
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("ListSetting")%></h1>           
            <ul><li><span class="mdm_subtxt"><%=GetResource("Desc2")%></span></li></ul>
        </div>  

        <div id="layout_content">
            <div class="row">
			  <div class="col-12 content-area-body"><!-- width 100%(col-12) --> 
                    <div class="layout_subbox">
                        <h2><%=GetResource("AlarmSetting")%></h2>
                    </div>
                    <table class="table_typeB">
                    <caption><%=GetResource("AlarmSetting")%></caption>
			        <colgroup>
                        <col width="120px" />
                        <col />
                        <col width="120px" />
			        </colgroup>
			        <tbody>
                    <tr>
                        <th><%=GetResource("AlarmSetting")%></th>
                        <td>
                            <span><%=GetResource("Mail")%> : </span>
                            <asp:DropDownList ID="ddlMailAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Approval")%> : </span>
                            <asp:DropDownList ID="ddlApprovalAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Board")%> : </span>
                            <asp:DropDownList ID="ddlBoardAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Pims")%> : </span>
                            <asp:DropDownList ID="ddlPimsAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Task")%> : </span>
                            <asp:DropDownList ID="ddlTaskAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <br />
                            <span><%=GetResource("Work")%> : </span>
                            <asp:DropDownList ID="ddlWorkAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Community")%> : </span>
                            <asp:DropDownList ID="ddlCommunityAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("ResourceManager")%> : </span>
                            <asp:DropDownList ID="ddlResourceAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("SNS")%> : </span>
                            <asp:DropDownList ID="ddlSNSAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            <span><%=GetResource("Notice")%> : </span>
                            <asp:DropDownList ID="ddlNoticeAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>  
                        </td>
                        <td>
                            <ul class="btn_right">
                                <li><cc3:ClientSmartButton ID="cbtnAlarm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </td>
                    </tr>            
                    </tbody>
                    </table>

                    <div class="layout_subbox">
                        <h2><%=GetResource("MobileAuth")%></h2>
                    </div>
                    <table cellspacing="0" class="table_typeB">
                    <caption><%=GetResource("MobileAuth")%></caption>
			        <colgroup>
                        <col width="120px" />
                        <col />
                        <col width="80px" />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("LoginType")%></th>
                            <td><asp:DropDownList ID="ddlLoginType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                            <td>
                                <ul class="btn_right">
                                    <li><cc3:ClientSmartButton ID="cbtnLogin" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("AccessRight")%></th>
                            <td><asp:RadioButtonList ID="rblAuth" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
                            <td>
                                <ul class="btn_right">
                                    <li><cc3:ClientSmartButton ID="cbtnAuth" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
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
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script> 
<script type="text/javascript">      
    var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>"; 
    var R_ProcessingError = "<%=GetResource("ProcessingError") %>";       
</script>       
<script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Config/Setting.js" ></script> 
<script type="text/javascript">
    //JsCall("/DeskPlusEIP/MDM/Js/Config/Setting.js");
</script>
</body>
</html>
