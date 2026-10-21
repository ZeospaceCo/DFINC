<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddReject.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Content.Receive.AddReject" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("Reject")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="height:270px">

            <table class="table_typeB">
                <caption><%=GetResource("Opinion")%></caption>
				<colgroup>
					<col width="60px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th><%=GetResource("Opinion")%></th>
						<td><asp:TextBox ID="tbOpinion" runat="server" TextMode="MultiLine" Width="93%" Height="210px" MaxLength="200"></asp:TextBox></td>
					</tr>
				</tbody>
			</table>             
        </div> 
    </div>

</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type="text/javascript">
        var CallBackEvent = "<%=CallBackEvent%>";
        var R_ShallWeReject  = "<%=GetResource("ShallWeReject") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Content/Receive/AddReject.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Content/Receive/AddReject.js");
    </script>
</body>
</html>
