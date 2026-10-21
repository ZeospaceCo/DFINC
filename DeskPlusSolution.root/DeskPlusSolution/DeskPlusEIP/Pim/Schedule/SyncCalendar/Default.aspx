<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.SyncCalendar.Default" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("GoogleCalendarSync") %></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"  /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:190px;">

                <table class="table_typeB">
			    <caption></caption>
			    <colgroup>
                    <col width="85px"/>
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("SyncAccount") %></th>
                        <td>
                            <span class="aLeft">
                            <cc3:ClientSmartTextBox ID="ctbGoogle" runat="server" InputMode="None" Width="200px"></cc3:ClientSmartTextBox>&nbsp;
                            </span>
                            <ul class="btn_smallL">
                            <li><cc3:ClientSmartButton ID="cbtnSignIn" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li>
                            <li><cc3:ClientSmartButton ID="cbtnSignOut" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li>
                            </ul>
                        </td>
                    </tr>
                   <tr runat="server" id="trSelCal">
                        <th><%=GetResource("GoogleGetCalendar")%></th>
                        <td>
                            <span class="aLeft">
                            <asp:DropDownList runat="server" ID="ddlCalendarsList" AutoPostBack="false" CssClass="searchselect form-control"></asp:DropDownList>
                            </span>
                        </td>
                    </tr>
                   <tr runat="server" id="trSelFilter">
                        <th><%=GetResource("NowSync")%></th>
                        <td>
                            <span class="aLeft">
                            <asp:DropDownList runat="server" ID="ddlSearchFilter" AutoPostBack="false" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;
                            <span id="spanDate" style="display:none;"><cc3:ClientSmartTextBox ID="ctbDate" runat="server" InputMode="None" Width="80px"></cc3:ClientSmartTextBox> <%=GetResource("Since")%></span>
                            </span>

                            <ul>
                            <li><cc3:ClientSmartButton ID="cbtnGetCalendar" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li>
                            </ul>
                        </td>
                    </tr>
                    <tr>
                        <th>최근 동기화</th>
                        <td>
                        <div runat="server" id="dvLatestSyncTime" />
                        </td>
                    </tr>
                </tbody>
                </table>
              
            </div>
             <div style="padding-top:10px; padding-left:10px">            
               <%=GetResource("GoogleAccount1")%> <a href="#" id="PopupLink"><span style="font-weight:bold;color:navy;"><%=GetResource("GoogleAccount2")%></span></a><%=GetResource("GoogleAccount3")%>
            </div>  
        </div>
    </div>    

    <div style="display:none;">
        <asp:LinkButton ID="lbtnSignIn" runat="server" OnClick="lbtnSignIn_Click"></asp:LinkButton>    
        <asp:LinkButton ID="lbtnSignOut" runat="server" OnClick="lbtnSignOut_Click"></asp:LinkButton>    
        <asp:LinkButton ID="lbtnGetCalendar" runat="server" OnClick="lbtnGetCalendar_Click"></asp:LinkButton>    
        <asp:TextBox ID="tbGoogle" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbGooglePwd" runat="server" Width="0"></asp:TextBox>    
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>          
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script> 
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var GoogleInfo_Input = '<%=GetResource("GoogleInfo_Input") %>';
        var Inquery_Input = '<%=GetResource("Inquery_Input") %>';
        var PreLang = "<%=PreLang%>";
        var UserID = "<%=CurrentUserInfo.UserID%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/SyncCalendar/Default.js" ></script> 
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
         //JsCall("/DeskPlusEIP/Pim/Js/Schedule/SyncCalendar/Default.js");             
    </script>
</body>
</html>
