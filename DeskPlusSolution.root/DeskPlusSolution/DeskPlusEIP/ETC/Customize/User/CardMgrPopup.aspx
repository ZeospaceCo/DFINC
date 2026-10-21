<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardMgrPopup.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.CardMgrPopup" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>

        <div class="inside">
            <div class="popup_header">
                <h5>법인카드 관리</h5>&nbsp;&nbsp;(개인법인카드는 한개만 등록가능합니다.)
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>

            <table class="table_typeB">
            <colgroup>
                <col width="130px" />
                <col />
                <col width="200px" />
                <col width="60px" />
            </colgroup>
            <tbody>
            <tr>
                <th>카드사 <cc3:ClientSmartTextBox runat="server" ID="cbtnCardNM" TextMode="SingleLine" InputMode="Default" CssStyle="width:60px;"></cc3:ClientSmartTextBox></th>
                <th>카드번호 <cc3:ClientSmartTextBox runat="server" ID="cbtnCardNumber" TextMode="SingleLine" InputMode="Default" CssStyle="width:140px;"></cc3:ClientSmartTextBox></th>
                <th>소유자 <cc3:ClientSmartTextBox runat="server" ID="cbtnCardOwner" TextMode="SingleLine" InputMode="Default" CssStyle="width:60px;"></cc3:ClientSmartTextBox>
                    개인카드 <input type="checkbox" id="chkIsPrivate" title="개인카드" />
                </th>
                <td>
                    <ul class="btn_right">
                        <li class="btn_blue"><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Text="확인"/></li>
                    </ul>
                </td>
            </tr>
            </tbody>
            </table>
            <div class="popup_layout_style3" >
                <div class="layout_scroll" style="min-height:170px;padding-top:5px;height:170px;">
                        
                        <div id="placeholder" style="clear: both;"></div>
                                   
                </div>
            </div>
        </div>

        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>


    </form>
</body>
</html>
