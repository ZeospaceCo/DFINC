<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatusInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Common.StatusInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <!-- 테이블 상단환경 -->
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("State")%></h1>
                <ul class="btn_right">
                   <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
            <table cellspacing="0" class="table_typeA">
                <caption></caption>
                <colgroup>
                    <col width="50%" />
                    <col width="50%" />
                </colgroup>
                <tr>
                    <th><%=GetResource("State")%>  </th>
                    <th><%=GetResource("NotiColor")%>  </th>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("StandBy")%></td>
                    <td class="center"><div style="background-color: #75ace9; width: 50px; height: 15px;display:inline-block;" title="#7EB618"></div></td>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("Progress")%></td>
                    <td class="center"><div style="background-color: #8bc63e; width: 50px; height: 15px;display:inline-block;" title="#C48D1C"></div></td>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("Complete")%></td>
                    <td class="center"><div style="background-color: #343a40; width: 50px; height: 15px;display:inline-block;" title="#646464"></div></td>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("Delay")%></td>
                    <td class="center"><div style="background-color: #9e9080; width: 50px; height: 15px;display:inline-block;" title="#900909"></div></td>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("ForNotifications")%></td>
                    <td class="center"><div style="background-color: #328a32; width: 50px; height: 15px;display:inline-block;" title="#258EE9"></div></td>
                </tr>
            </table>
            </div>
        </div>

    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/StatusInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Common/StatusInfo.js");
    </script>
</body>
</html>