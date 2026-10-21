<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterLangAddress.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.RegisterLangAddress" %>

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
                        <input type="text" id="addressFirstBase" style="width:40%" />
                        <input type="text" id="addressEndBase" style="width:40%" />
                    </td>
                </tr>
                <tr id="koInput">
                    <th><%=GetResource("Korean")%></th>
                    <td>
                        <input type="text" id="addressFirstKo" style="width:40%" />
                        <input type="text" id="addressEndKo" style="width:40%" />
                    </td>
                </tr>
                <tr  id="enInput">
                    <th><%=GetResource("English")%></th>
                    <td>
                        <input type="text" id="addressFirstEn" style="width:40%" />
                        <input type="text" id="addressEndEn" style="width:40%" />
                    </td>
                </tr>
                <tr  id="jaInput">
                    <th><%=GetResource("Japanese")%></th>
                    <td>
                        <input type="text" id="addressFirstJa" style="width:40%" />
                        <input type="text" id="addressEndJa" style="width:40%" />
                    </td>
                </tr>
                <tr  id="frInput">
                    <th><%=GetResource("French")%></th>
                    <td>
                        <input type="text" id="addressFirstFr" style="width:40%" />
                        <input type="text" id="addressEndFr" style="width:40%" />
                    </td>
                </tr>
                <tr  id="zhcnInput">
                    <th><%=GetResource("Chinese")%></th>
                    <td>
                        <input type="text" id="addressFirstZhcn" style="width:40%" />
                        <input type="text" id="addressEndZhcn" style="width:40%" />
                    </td>
                </tr>
                <tr  id="zhtwInput">
                    <th><%=GetResource("Taiwanese")%></th>
                    <td>
                        <input type="text" id="addressFirstZhtw" style="width:40%" />
                        <input type="text" id="addressEndZhtw" style="width:40%" />
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

        EndBase = "<%=EndBase%>";
        EndKo = "<%=EndKo%>";
        EndEn = "<%=EndEn%>";
        EndJa = "<%=EndJa%>";
        EndFr = "<%=EndFr%>";
        EndZhcn = "<%=EndZhcn%>";
        EndZhtw = "<%=EndZhtw%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/RegisterLangAddress.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/EnforceSeal/RegisterLangAddress.js");
    </script> 
</body>
</html>
