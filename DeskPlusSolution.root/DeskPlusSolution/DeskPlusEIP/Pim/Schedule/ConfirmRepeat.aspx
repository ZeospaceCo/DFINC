<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmRepeat.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Content.ConfirmRepeat" %>
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
            <h1><%=GetResource("RepeatMod") %></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"  /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"   /></li>
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
                    <th><input type="radio" id="rbRechk1" name="rbRe" checked="checked"   /><%=GetResource("THISPIM")%></th>
                    <td><%=GetResource("RepeatMent1")%></td>
                </tr>
                <tr>
                    <th><input type="radio" id="rbRechk2" name="rbRe" /><%=GetResource("AfterPim")%></th>
                    <td><%=GetResource("RepeatMent2")%></td>
                </tr>
                <tr>
                    <th><input type="radio" id="rbRechk3" name="rbRe"  /><%=GetResource("AllPim")%></th>
                    <td><%=GetResource("RepeatMent3")%></td>
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
    <script type="text/javascript">
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PIM/Js/Schedule/ConfirmRepeat.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PIM/Js/Schedule/ConfirmRepeat.js");
    </script> 
</body>
</html>
