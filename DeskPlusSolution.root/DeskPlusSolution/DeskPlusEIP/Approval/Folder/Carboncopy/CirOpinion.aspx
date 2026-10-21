<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CirOpinion.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.CirOpinion" %>

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
                <h1><%=GetResource("ViewCirOpinion")%></h1>   
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="layout_scroll" style="min-height:120px;height:120px;">
                        <textarea name="textarea" id="textarea"  class="textarea" style="width:97%;" maxlength="200" runat="server"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    
    <script type="text/javascript">
        $(function () {

            $(document).ready(function () {
                //닫기
                $('#CbtnClose').click(function (e) {
                    e.preventDefault();
                    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
                });
            });
        });
    </script>

</body>
</html>