<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSimple.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.AddSimple" %>
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

<div class="layer_popup" style="background:white">

    <div class="inside">
        <div class="popup_header">
            <h1><%= GetResource("AddSchedule")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            	<li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <table class="table_typeB">
			<caption>
			    <%= GetResource("AddSchedule")%>
			</caption>
			<colgroup>
                <col width="60px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%= GetResource("Title")%></th>
                    <td><input type="text" class="pims_text" runat="server" id="tbTitle" style="width:300px" /></td>
                </tr>
                <tr>
                    <th><%= GetResource("Schedule")%></th>
                    <td>
                        <div id="startEndDate">
                          <input type="text" id="tbstartDate" size="12"  runat="server" class="clsDate searchtext form-control"  /> 
                          <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                           ~
                           <input type="text" id="tbendDate" size="12"  runat="server" class="clsDate searchtext form-control"  /> 
                           <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><%= GetResource("ShareSchedule")%></th>
                    <td><asp:DropDownList id="ddlShare" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
              
            </tbody>
            </table>

        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script> 
     <script type="text/javascript">
         var TimeOver = '<%= GetResource("TimeOver") %>';
         var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
         var Type = "";
         var Title = "";
         var StartTime = "";
         var EndTime = "";
         var OpenType = "";
         var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
         var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
         //목록버튼
         var PageScope = '<%=PageScope%>';
         var paraDate = '<%=Date%>';
         var paraOpentype = '<%=Opentype%>';
         var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js");
    </script>   
</body>
</html>

