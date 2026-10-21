<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateThum.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Category.CreateThum" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ScreenShot")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>                        
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:50px; vertical-align:middle;">
            <span style="float:left;"><%=GetResource("CreateScreenShot")%></span><div id="loading" style="float:left;"></div> 
            </div> 
        </div>

    </div>
     <div style="display:none;">
         <asp:LinkButton ID="lbtnRegister" OnClick="lbtnRegister_Click" runat="server"></asp:LinkButton>
     </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript">
        /**************************************************
        초기화
        ***************************************************/
        var ImagePath = "<%=BaseImgPath%>";

        $(function () {
            $(document).ready(function () {
                //취소
                $('#cbtnClose').click(function (e) {
                    e.preventDefault();
                    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
                });

                //확인
                $('#cbtnConfirm').click(function (e) {
                    e.preventDefault();
                    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
                    $('#loading').html(loadingPath);
                    __doPostBack('lbtnRegister', '');
                });
            });
        });
     </script>
</body>
</html>
