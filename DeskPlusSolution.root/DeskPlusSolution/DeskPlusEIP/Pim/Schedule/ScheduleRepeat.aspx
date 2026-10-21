<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleRepeat.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.ScheduleRepeat" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("PimRepeatSet") %></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

        <table class="table_typeB" border="0" style="width:98%; padding:5px !Important; word-spacing:3px; line-height:26px;" > 
		<caption><%=GetResource("RepeatSetting")%></caption>
		<colgroup>
            <col width="80px" />
            <col />
		</colgroup>
		<tbody>
        <tr>
            <th width="90"><%=GetResource("EveryDayRepeat")%></th>
            <td>
               <asp:RadioButton ID="rbday" runat="server"  GroupName="rbkind" Checked="true" BorderWidth="0" />&nbsp;<%=GetResource("Every")%> 
               <input type="text" id="txtday" size="5" runat="server"  /><%=GetResource("DayToo")%>&nbsp;&nbsp;               
               <asp:RadioButton ID="rbmail" runat="server"  GroupName="rbkind" />
               <%=GetResource("normalday")%> 
             </td>
        </tr>
        <tr> 
            <th ><%=GetResource("EveryWeekRepeat")%></th>
            <td align="left">                         
            <asp:RadioButton ID="rbweek" runat="server"  GroupName="rbkind" />&nbsp;<%=GetResource("Every")%> 
            <input type="text" id="txtweek" size="5" runat="server"  /><%=GetResource("WeekNextDay")%><br />
            <asp:CheckBox ID="chksun" runat="server" /><%=GetResource("kind4")%> &nbsp;
            <asp:CheckBox ID="chkmon" runat="server" /><%=GetResource("kind5")%> &nbsp;
            <asp:CheckBox ID="chktue" runat="server" /><%=GetResource("kind6")%> &nbsp;
            <asp:CheckBox ID="chkwed" runat="server" /><%=GetResource("kind7")%> &nbsp;
            <asp:CheckBox ID="chkthu" runat="server" /><%=GetResource("kind8")%> &nbsp;
            <asp:CheckBox ID="chkfri" runat="server" /><%=GetResource("kind9")%> &nbsp;
            <asp:CheckBox ID="chksat" runat="server" /><%=GetResource("kind10")%>                      
            </td>  
        </tr> 
        <tr>
            <th ><%=GetResource("EveryMonRepeat")%></th>
            <td>
            <asp:RadioButton ID="rbmonth" GroupName="rbkind" runat="server" />&nbsp;<%=GetResource("Every")%> 
            <input type="text" id="txtmonth" size="5" runat="server"  /><%=GetResource("MonthToo")%> 
            <input type="text" id="txtmonthday" size="5" runat="server"  /><%=GetResource("TheDay")%><br />
            <asp:RadioButton ID="rbmonth2" GroupName="rbkind"  runat="server" />&nbsp;<%=GetResource("Every")%> 
            <input type="text" id="txtmonth2" size="5" runat="server"  /><%=GetResource("MonthToo")%> 
            <asp:DropDownList ID="ddlorder" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>  
            <asp:DropDownList ID="ddlkind" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>  
        </td>
        </tr>                 
        <tr>
            <th><%=GetResource("RepeatDatePeriod")%></th>
            <td>
            <asp:RadioButton ID="rbrepeat1" GroupName="rbrepeat" runat="server" Checked="true" />&nbsp;
            <input type="text" id="txtcalendar" size="15" runat="server" readonly="readonly"  /><%=GetResource("DayPeriod")%><br />
            <asp:RadioButton ID="rbrepeat2" GroupName="rbrepeat" runat="server" />&nbsp;
            <input type="text" id="txtrepeat" size="5" runat="server"  /><%=GetResource("RepeatSettingMent1")%>
            </td>
        </tr>
         
        </table> 

        </div>
    </div>
</div>

		<asp:TextBox ID="hdMent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <!-- 테이블 하단환경 끝 -->    
    </form>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        var PreLang = "<%=PreLang%>";
        var CompanyID = "<%=CompanyID%>";
        var RepeatType = ''
        var SelectType = ''
        var IntervalCnt = ''
        var Option1 = ''
        var Option2 = ''
        var Option3 = ''
        var Option4 = ''
        var Option5 = ''
        var Option6 = ''
        var Option7 = ''
        var MaxReptCnt = ''
        var EndDate = ''
        var StartDate = ''
        var RepeatAlert1 = '<%=GetResource("RepeatAlert1") %>';
        var RepeatAlert2 = '<%=GetResource("RepeatAlert2") %>';
        var RepeatAlert3 = '<%=GetResource("RepeatAlert3") %>';
        var RepeatAlert5 = '<%=GetResource("RepeatAlert5") %>';
        var R_RepeatZero = '<%=GetResource("RepeatZero") %>';
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/ScheduleRepeat.js" ></script>         
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
         //JsCall("/DeskPlusEIP/Pim/Js/Schedule/ScheduleRepeat.js");             
    </script>
</body>
</html>
