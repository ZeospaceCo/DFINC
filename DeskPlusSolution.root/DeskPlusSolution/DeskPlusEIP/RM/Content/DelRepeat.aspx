<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DelRepeat.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.DelRepeat" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"> 
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RepeatDel")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"/></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="120px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><input type="radio" id="rbRechk1" name="rbRe" checked="checked" /><%=GetResource("RepeatGubun1")%></th>
                    <td><%=GetResource("RepeatEx1")%></td>
                </tr>
                <tr>
                    <th><input type="radio" id="rbRechk2" name="rbRe" /><%=GetResource("RepeatGubun2")%></th>
                    <td><%=GetResource("RepeatEx4")%></td>
                </tr>
                <tr>
                    <th><input type="radio"" id="rbRechk3" name="rbRe" /><%=GetResource("RepeatGubun3")%></th>
                    <td><%=GetResource("RepeatEx5")%></td>
                </tr>
            </tbody>
            </table>
            
        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/DelRepeat.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Content/DelRepeat.js");
    </script>    
</body>
</html>
