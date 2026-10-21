<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkURL.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.LinkURL" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("URLAddBtn")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server"  class="btn btn-secondary" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

        <table class="table_typeB" border="0" style="width:98%; padding:5px !Important; word-spacing:3px; line-height:26px;" > 
		<caption><%=GetResource("URLAddBtn")%></caption>
		<colgroup>
            <col width="80px" />
            <col />
		</colgroup>
		<tbody>
        <tr>
            <th width="90"><%=GetResource("FileName")%> : </th>
            <td>
               <input type="text" id="txtLinkURLName" runat="server" size="55" />
             </td>
        </tr>
        <tr> 
            <th ><%=GetResource("URLName")%> : </th>
            <td align="left">                         
                <textarea name="textarea" id="txtLinkURL"  class="textarea" style="width:97%;" maxlength="200"></textarea>
            </td>  
        </tr>      
        </table> 
        </div>
    </div>

</div>

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_URLNameMsg = "<%= GetResource("URLNameMsg") %>";     
        var R_URLMsg = "<%= GetResource("URLMsg") %>";     
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/LinkURL.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/LinkURL.js");
    </script>
</body>
</html>