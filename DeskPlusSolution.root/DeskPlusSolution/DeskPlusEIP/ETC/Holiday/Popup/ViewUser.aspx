<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewUser.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewUser" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="overflow:hidden !Important;">
            <div class="layout_scroll" style="min-height:120px;height:120px;">
            <!-- 테이블 시작 //-->
                
                <table class="table_typeA">
                <colgroup>
                    <col width="32%" />
                    <col width="34%" />
                    <col width="34%" />                                                 
                </colgroup>
                <tr>
                    <th class="center"><%=GetResource("HoliTitle")%></th>
                    <th class="center"><%=GetResource("HoliAll")%></th>
                    <th class="center"><%=GetResource("HoliJan")%></th>
                </tr>
                <tr>
                    <td class="center"><%=GetResource("HoliType1")%></td>
                    <td class="center"><asp:Literal id="ltHoliAll1" runat="server"></asp:Literal></td>                    
                    <td class="center"><asp:Literal id="ltHoliJan1" runat="server"></asp:Literal></td>                    
                </tr>
                </table>
                   
            <!-- 테이블 끝 //-->
            </div>
            </div>
        </div>
    </div>
    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewUser.js");
    </script>
</body>
</html>

