<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComplete.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.AddComplete" EnableViewStateMac="true" EnableEventValidation="true" ValidateRequest="true" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>수료증 등록 및 완료</title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
    <style type="text/css">
        #placeholder_file .certificate-file-size { display:inline-block; margin-left:8px; }
        #placeholder_file .certificate-file-delete { display:inline-block; margin-left:10px; color:#dc3545; text-decoration:none; }
        #placeholder_file .certificate-file-delete i { color:inherit; }
        #placeholder_file .certificate-file-delete:hover,
        #placeholder_file .certificate-file-delete:focus { color:#b02a37; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layer_popup">
            <div class="inside">
                <div class="popup_header">
                    <h1>수료증 등록 및 완료</h1>
                    <ul class="btn_right">
                        <li><asp:Button ID="btnComplete" runat="server" Text="교육완료" CssClass="btn btn-secondary" OnClick="btnComplete_Click" OnClientClick="return AddComplete.Validate();" /></li>
                        <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" Text="닫기" /></li>
                    </ul>
                </div>
                <div class="dotLine"></div>
                <div class="popup_layout_style3" style="padding-top:16px;">
                    <table class="table_typeB">
                        <caption>수료증 등록</caption>
                        <colgroup><col width="85px" /><col /></colgroup>
                    </table>
                    <asp:Panel ID="pnlRegistration">
                        <table class="table_typeB">
                            <caption>수료증 파일</caption>
                            <colgroup><col width="85px" /><col /></colgroup>
                            <tbody>
                                <tr>
                                    <th>수료증</th>
                                    <td>
                                        <cc3:ClientSmartButton ID="cbtnSwfFileAdd" runat="server" Text="파일추가" />
                                        <div id="placeholder_file" style="clear:both;margin-top:8px;"></div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <p style="margin-top:16px;">수료증을 등록하면 본인의 교육이 완료 처리됩니다.</p>
                    </asp:Panel>
                    <p style="margin-top:12px;" role="status"><asp:Literal ID="ltlNotice" runat="server" Mode="Encode"></asp:Literal></p>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" role="alert"></asp:Label>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SwfUploadGUID = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SwfUploadGUID) %>";
    </script>
    <!--script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/AddComplete.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Content/AddComplete.js");
    </script>
</body>
</html>
