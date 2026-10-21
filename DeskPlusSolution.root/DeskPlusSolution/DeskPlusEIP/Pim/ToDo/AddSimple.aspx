<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSimple.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.ToDo.AddSimple" %>
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
            <h1><asp:Label ID="lblHearder" runat="server"></asp:Label></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnMod" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <div id="tMe" runat="server">
            <table class="table_typeB">
			<caption>
			    <%=GetResource("AddToDo")%>
			</caption>
			<colgroup>
                <col width="60px" />
                <col />
                <col width="60px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("Title")%></th>
                    <td colspan="3"><input type="text" class="pims_text" runat="server" style="width:95%" id="txtTitle" maxlength="100"/></td>
                </tr>
                <tr>
                    <th><%=GetResource("IMPORTANCE_MEASURE")%></th>
                    <td><asp:DropDownList ID="ddltodo" runat="server" CssClass="searchselect form-control"></asp:DropDownList> </td>
                    <th><%=GetResource("Progress")%></th>
                    <td><asp:DropDownList ID="ddlPriorty" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><%=GetResource("Schedule")%></th>
                    <td><asp:DropDownList id="ddlDate" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    <td colspan="2"><input type="text" id="txtDate" size="15" runat="server"  /> </td>
                </tr>
                <tr>
                    <th><%=GetResource("ViewLimit")%></th>
                    <td colspan="3"><asp:DropDownList id="ddlShare" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
            </tbody>
            </table>
            </div>

            <div id="tShare" runat="server">
            <table class="table_typeB">
			<caption>
			    <%=GetResource("AddToDo")%>
			</caption>
			<colgroup>
                <col width="60px" />
                <col />
                <col width="60px" />
                <col />
			</colgroup>
            <tbody>
                <tr style="height:40px">
                    <th><%=GetResource("Title")%></th>
                    <td colspan="3" style="vertical-align:middle"><asp:Label ID="lblTitle" runat="server"></asp:Label></td>
                </tr>
                <tr style="height:40px">
                    <th><%=GetResource("IMPORTANCE_MEASURE")%></th>
                    <td style="vertical-align:middle"><asp:Label ID="lbltodo" runat="server"></asp:Label></td>
                    <th><%=GetResource("Progress")%></th>
                    <td style="vertical-align:middle"><asp:Label ID="lblPriorty" runat="server"></asp:Label></td>
                </tr>
                <tr style="height:40px">
                    <th><%=GetResource("ViewLimit")%></th>
                    <td style="vertical-align:middle"><asp:Label ID="lblShare" runat="server"></asp:Label></td>
                    <th ><%=GetResource("WRITER")%></th>                      
                    <td style="vertical-align:middle"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                </tr>
            </tbody>
            </table>
            </div>

        </div>
    </div>

    <div style="display:none;">
    <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>        
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';        
        var Add_InputContent = '<%= GetResource("Add_InputContent") %>';
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var UserID = <%=UserID %>;
        var CurrentUserID= <%=CurrentUserID %>;
        var CompnayID = <%=CompanyID %>;
        var TodoID= <%=TodoID%>;
        var Title ="";
        var SCOPE ="0";
        var DateInfo="";
        var Priority="";
        var Status="";
        var t_Opentype="";
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var PreLang = "<%=PreLang%>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/ToDo/AddSimple.js" ></script>  
   <script type="text/javascript">
       //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
       //JsCall("/DeskPlusEIP/Pim/Js/ToDo/AddSimple.js");
    </script>
</body>
</html>
