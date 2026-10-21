<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.SendMail" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("SendMail")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <table cellspacing="0" border="0" style="width:100%;"> 
            <tr>
                <td valign="top" width="270px" style="text-align:center;"> 
                    <div class="layout_scroll"  style="width:270px;min-height:420px;height:420px;overflow:hidden"> 
                        <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" />
                    </div>
                </td>
                <td class="center" valign="top" style="width:100px;">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                </td>
                <td valign="top">          
                    <div class="layout_scroll"  style="min-height:420px;height:420px;">          
                        <div class="layout_subbox">
                            <h2><%=GetResource("SendMailTitle")%></h2>
                        </div>
                        <div class="memo"><cc3:ClientSmartTextBox runat="server" ID="ctbSubject" Width="98%"  Required="true" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox></div>
                        <div style="min-height:10px;"></div>                
                        
                        <table class="table_typeA" id="contentTable">
                            <caption><%=GetResource("SpammerList")%></caption>
                            <colgroup>
                                <col />
                                <col width="60px" />
			                </colgroup>
			                <tbody>
				            <tr>
                                <th id="r_name"><%=GetResource("Receiver")%></th>
                                <th id="r_del"><%=GetResource("Del") %></th>
                            </tr>                
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var CategoryID = "<%=CategoryID %>";
        var ListID = "<%=ListID %>";
        var UserList = "";
        var AuthArrayList = "";
        var RowMax = 1;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Name = "<%=GetResource("Name")%>";
        var R_Del = "<%= GetResource("Del")%>";
        var R_Processing = "<%=GetResource("Processing")%>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent")%>";
        var R_SettingAuthority = "<%=GetResource("SelectSubject")%>";
        var R_Add_InputTitle = "<%=GetResource("Add_InputTitle")%>";
        var R_Del = "<%=GetResource("Del")%>";  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/SendMail.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/SendMail.js");
    </script>
</body>
</html>
