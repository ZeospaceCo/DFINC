<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterLanguage.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.RegisterLanguage" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Common.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Common.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var UseName = "<%=UseName %>";
        var NameBase = "<%=NameBase %>";
        var NameKo = "<%=NameKo %>";
        var NameEn = "<%=NameEn %>";
        var NameJa = "<%=NameJa %>";
        var NameFr = "<%=NameFr %>";
        var NameZhcn = "<%=NameZhcn %>";
        var NameZhtw = "<%=NameZhtw %>";
        var CallBackPage = "<%=CallBackPage %>";
        
    </script>
    <script type="text/javascript">        
        JsCall("/DeskPlusEIP/Common/Js/RegisterLanguage.js");
    </script>    
</head>
<body>

<form id="form1" runat="server">


<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("List")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:220px;">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="60px"/>
                <col />
            </colgroup>
            <tbody>
                <tr id="baseInput">
                    <th><%=GetResource("BaseLang")%></th>
                    <td><input type="text" id="nameBase" style="width:90%" /></td>
                </tr>
                <tr id="koInput">
                    <th><%=GetResource("Korean")%></th>
                    <td><input type="text" id="nameKo" style="width:90%" /></td>
                </tr>
                <tr  id="enInput">
                    <th><%=GetResource("English")%></th>
                    <td><input type="text" id="nameEn" style="width:90%" /></td>
                </tr>
                <tr  id="jaInput">
                    <th><%=GetResource("Japanese")%></th>
                    <td><input type="text" id="nameJa" style="width:90%" /></td>
                </tr>
                <tr  id="frInput">
                    <th><%=GetResource("French")%></th>
                    <td><input type="text" id="nameFr" style="width:90%" /></td>
                </tr>
                <tr  id="zhcnInput">
                    <th><%=GetResource("Chinese")%></th>
                    <td><input type="text" id="nameZhcn" style="width:90%" /></td>
                </tr>
                <tr  id="zhtwInput">
                    <th><%=GetResource("Taiwanese")%></th>
                    <td><input type="text" id="nameZhtw" style="width:90%" /></td>
                </tr>
            
            </tbody>
            </table>
            
        </div>
    </div>
</div>

</form>
</body>
</html>
