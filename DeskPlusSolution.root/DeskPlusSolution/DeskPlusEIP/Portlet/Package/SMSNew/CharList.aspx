<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CharList.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMSNew.CharList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />        

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>

    <script type="text/javascript">
        var R_Title = "<%=GetResource("Title") %>";
        var R_NoContent = "<%=GetResource("NoContents") %>";
    </script>

    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMSNew/CharList.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="top-left">
        </div>
        <div class="top-right">
        </div>
        <div class="inside">
            <div class="popup_header">
                <h5>문자리스트</h5>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnDel" runat="server" /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <div id="scrollDiv">
                    <div class="table_typeLtop">
                        <p>
                        </p>
                    </div>
                    <div id="placeholder" style="clear: both;">
                    </div>
                    <div class="table_typeLbottom">
                        <p>
                        </p>
                    </div>
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3>
                            <span id="pageInfo"></span>
                        </h3>
                        <span id="pager"></span>
                    </div>
                </div>                
            </div>
        </div>
        <div class="bottom-left">
        </div>
        <div class="bottom-right">
        </div>
    </div>
    </form>
</body>
</html>
