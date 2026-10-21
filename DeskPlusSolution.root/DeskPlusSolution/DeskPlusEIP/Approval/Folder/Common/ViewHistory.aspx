<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewHistory.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Common.ViewHistory" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>        
</head>
<body scroll="auto">
    <form id="form1" runat="server">
    <div id="layer_popup" class="view">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("ViewHistory")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" /> 
                </ul>
            </div>
            <div id="layout_caption" style="border-bottom: 0px;">
                <span class="app_caption" style="font-weight:bold;">
                    * <%=GetResource("AttachName")%>(<strong><asp:Literal ID="ltlAttachCnt" runat="server"></asp:Literal><%=GetResource("EA")%></strong>) / <%=GetResource("BaseDoc")%>(<strong><asp:Literal ID="ltlBaseDocCnt" runat="server"></asp:Literal><%=GetResource("EA")%></strong>)
                </span>
            </div>
            <div id="layout_content">
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("DocInfo")%></caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("FormName2")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlDocType" runat="server"></asp:Literal>
                            </td>
                            <th>
                                <%=GetResource("ViewAuth")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlOpenAuth" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Reason")%>
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="ltlOpinion" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("AttachName")%>
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="ltlAttach" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("BaseDoc")%>
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="ltlBaseDoc" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="gap">
                </div>
                <!-- 양식로드 //-->
                <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>               
            </div>
        </div>
    </div>
    <div style="display:none">
        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    </div>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var DocID = "<%=DocID %>";
        var HistoryID = "<%=HistoryID %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Common/ViewHistory.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Common/ViewHistory.js");
    </script>

</body>
</html>
