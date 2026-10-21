<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterLangExp.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.RegisterLangExp" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ListLang")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="85px"/>
                <col />
            </colgroup>
            <tbody>
                <tr id="baseInput">
                    <th><%=GetResource("BaseLang")%></th>
                    <td>
                        <input type="text" id="expFirstBase" style="width:30%" />
                        <input type="text" id="expMidBase" style="width:30%" />
                        <input type="text" id="expEndBase" style="width:30%" />
                    </td>
                </tr>
                <tr id="koInput">
                    <th><%=GetResource("Korean")%></th>
                    <td>
                        <input type="text" id="expFirstKo" style="width:30%" />
                        <input type="text" id="expMidKo" style="width:30%" />
                        <input type="text" id="expEndKo" style="width:30%" />
                    </td>
                </tr>
                <tr  id="enInput">
                    <th><%=GetResource("English")%></th>
                    <td>
                        <input type="text" id="expFirstEn" style="width:30%" />
                        <input type="text" id="expMidEn" style="width:30%" />
                        <input type="text" id="expEndEn" style="width:30%" />
                    </td>
                </tr>
                <tr  id="jaInput">
                    <th><%=GetResource("Japanese")%></th>
                    <td>
                        <input type="text" id="expFirstJa" style="width:30%" />
                        <input type="text" id="expMidJa" style="width:30%" />
                        <input type="text" id="expEndJa" style="width:30%" />
                    </td>
                </tr>
                <tr  id="frInput">
                    <th><%=GetResource("French")%></th>
                    <td>
                        <input type="text" id="expFirstFr" style="width:30%" />
                        <input type="text" id="expMidFr" style="width:30%" />
                        <input type="text" id="expEndFr" style="width:30%" />
                    </td>
                </tr>
                <tr  id="zhcnInput">
                    <th><%=GetResource("Chinese")%></th>
                    <td>
                        <input type="text" id="expFirstZhcn" style="width:30%" />
                        <input type="text" id="expMidZhcn" style="width:30%" />
                        <input type="text" id="expEndZhcn" style="width:30%" />
                    </td>
                </tr>
                <tr  id="zhtwInput">
                    <th><%=GetResource("Taiwanese")%></th>
                    <td>
                        <input type="text" id="expFirstZhtw" style="width:30%" />
                        <input type="text" id="expMidZhtw" style="width:30%" />
                        <input type="text" id="expEndZhtw" style="width:30%" />
                    </td>
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

        CallBackPage = "<%=CallBackPage%>";
        UseName = "<%=UseName%>";

        FirstBase = "<%=FirstBase%>";
        FirstKo = "<%=FirstKo%>";
        FirstEn = "<%=FirstEn%>";
        FirstJa = "<%=FirstJa%>";
        FirstFr = "<%=FirstFr%>";
        FirstZhcn = "<%=FirstZhcn%>";
        FirstZhtw = "<%=FirstZhtw%>";

        MidBase = "<%=MidBase%>";
        MidKo = "<%=MidKo%>";
        MidEn = "<%=MidEn%>";
        MidJa = "<%=MidJa%>";
        MidFr = "<%=MidFr%>";
        MidZhcn = "<%=MidZhcn%>";
        MidZhtw = "<%=MidZhtw%>";

        EndBase = "<%=EndBase%>";
        EndKo = "<%=EndKo%>";
        EndEn = "<%=EndEn%>";
        EndJa = "<%=EndJa%>";
        EndFr = "<%=EndFr%>";
        EndZhcn = "<%=EndZhcn%>";
        EndZhtw = "<%=EndZhtw%>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/RegisterLangExp.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/RegisterLangExp.js");
    </script>   
</body>
</html>
