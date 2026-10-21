<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewFaxImage.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.ViewFaxImage" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.resize.js"></script>
    <script type="text/javascript">

        var PageCnt = "<%=PageCnt %>";
        var FaxImage = "<%=FaxImage %>";        

        $(function () {

            $(document).ready(function () {

                //인쇄
                $('#cbtnPrint').click(function (e) {
                    e.preventDefault();

                    ParamArray = {
                        'faxImage': FaxImage,
                        'pageCnt': PageCnt
                    };

                    CallEncrypt(CallGetParam());
                    var url = "/DeskPlusEIP/Common/WebFax/ViewFaxPrint.aspx?p=" + GetEncryptValue;

                    location = url;

                });

                //인쇄
                $('#cbtnClose').click(function (e) {
                    e.preventDefault();
                    self.close();
                });



            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1>보기</h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server"/></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div id="toPrint">
                    <div id="scrollDiv">
                        <asp:Literal ID="ltlFaxImage" runat="server"></asp:Literal>                        
                    </div>
                </div>
            </div>
        </div>
    </div>


    </form>
</body>
</html>
