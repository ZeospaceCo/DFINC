<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRepeat.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.AddRepeat" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RepeatSetting")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
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
            <th width="90"><%=GetResource("RepeatDay")%></th>
            <td>
               <asp:RadioButton ID="rbday" runat="server"  GroupName="rbkind" Checked="true" BorderWidth="0" />&nbsp;<%=GetResource("Repeat1")%>
               <input type="text" id="txtday" size="5" runat="server" /><%=GetResource("Repeat2")%>&nbsp;&nbsp;
               <asp:RadioButton ID="rbmail" runat="server"  GroupName="rbkind" /><%=GetResource("Repeat3")%>
             </td>
        </tr>
        <tr> 
            <th ><%=GetResource("RepeatWeek")%></th>
            <td align="left">                         
            <asp:RadioButton ID="rbweek" runat="server"  GroupName="rbkind" />&nbsp;<%=GetResource("Repeat1")%> 
            <input type="text" id="txtweek" size="5" runat="server"  /><%=GetResource("Repeat4")%><br />
            <asp:CheckBox ID="chksun" runat="server" /><%=GetResource("Sun")%> &nbsp;
            <asp:CheckBox ID="chkmon" runat="server" /><%=GetResource("Mon")%> &nbsp;
            <asp:CheckBox ID="chktue" runat="server" /><%=GetResource("Tue")%> &nbsp;
            <asp:CheckBox ID="chkwed" runat="server" /><%=GetResource("Wed")%> &nbsp;
            <asp:CheckBox ID="chkthu" runat="server" /><%=GetResource("Thu")%> &nbsp;
            <asp:CheckBox ID="chkfri" runat="server" /><%=GetResource("Fri")%> &nbsp;
            <asp:CheckBox ID="chksat" runat="server" /><%=GetResource("Sat")%>                      
            </td>  
        </tr> 
        <tr>
            <th ><%=GetResource("RepeatMonth")%></th>
            <td>
            <asp:RadioButton ID="rbmonth" GroupName="rbkind" runat="server" />&nbsp;<%=GetResource("Repeat1")%>
            <input type="text" id="txtmonth" size="5" runat="server"  /><%=GetResource("Repeat5")%>
            <input type="text" id="txtmonthday" size="5" runat="server"  /><%=GetResource("Repeat6")%><br />
            <asp:RadioButton ID="rbmonth2" GroupName="rbkind"  runat="server" />&nbsp;<%=GetResource("Repeat1")%> 
            <input type="text" id="txtmonth2" size="5" runat="server"  /><%=GetResource("Repeat5")%> 
            <asp:DropDownList ID="ddlorder" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>  
            <asp:DropDownList ID="ddlkind" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>  
        </td>
        </tr>                 
        <tr>
            <th><%=GetResource("RepeatRange")%></th>
            <td>
            <asp:RadioButton ID="rbrepeat1" GroupName="rbrepeat" runat="server" Checked="true" />&nbsp;
            <input type="text" id="txtcalendar" size="15" runat="server" readonly="true"  /><%=GetResource("Repeat7")%><br />
            <asp:RadioButton ID="rbrepeat2" GroupName="rbrepeat" runat="server" />&nbsp;
            <input type="text" id="txtrepeat" size="5" runat="server"  /><%=GetResource("Repeat8")%>
            </td>
        </tr>       
        </table> 

        </div>
    </div>    
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        var StartDate = "<%=StartDate %>";      
        var R_SelectRepeatDay = "<%=GetResource("SelectRepeatDay") %>";
        var R_RepeatStartDateEndDate = "<%=GetResource("RepeatStartDateEndDate") %>";
        var R_RepeatCount = "<%=GetResource("RepeatCount") %>";
        var R_RepeatInput = "<%=GetResource("RepeatInput") %>";
        var R_RepeatNumber = "<%=GetResource("RepeatNumber") %>";
        var R_SelectWeek = "<%=GetResource("SelectWeek") %>";
        var R_Repeat5 = "<%=GetResource("Repeat5") %>";
        var R_Repeat3 = "<%=GetResource("Repeat3") %>";
        var R_Repeat17 = "<%=GetResource("Repeat17") %>";
        var R_Repeat18 = "<%=GetResource("Repeat18") %>";
        var R_Repeat19 = "<%=GetResource("Repeat19") %>";
        var R_Repeat20 = "<%=GetResource("Repeat20") %>";
        var R_Repeat21 = "<%=GetResource("Repeat21") %>";
        var R_Repeat22 = "<%=GetResource("Repeat22") %>";
        var R_Repeat23 = "<%=GetResource("Repeat23") %>";
        var R_Sun = "<%=GetResource("Sun") %>";
        var R_Mon = "<%=GetResource("Mon") %>";
        var R_Tue = "<%=GetResource("Tue") %>";
        var R_Wed = "<%=GetResource("Wed") %>";
        var R_Thu = "<%=GetResource("Thu") %>";
        var R_Fri = "<%=GetResource("Fri") %>";
        var R_Sat = "<%=GetResource("Sat") %>";
        var R_RepeatZero = "<%=GetResource("RepeatZero") %>";
        var R_InputNumber = "<%=GetResource("InputNumber") %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/AddRepeat.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
//        JsCall("/DeskPlusEIP/RM/Js/Content/AddRepeat.js");
    </script>
</body>
</html>
