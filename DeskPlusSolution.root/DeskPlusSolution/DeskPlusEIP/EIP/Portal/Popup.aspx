<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.Popup" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Portal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Portal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div style="padding-top:15px;">
                <div class="portlet-header" style="cursor:default;">
                    <div class="left"></div>
                    <div class="mid">
                        <ul>
                            <li><img src="<%=BaseImgPath%>/Main/ico_title.gif" alt="" />&nbsp;&nbsp;미리보기</li>
                            <li class="menu">
                                <a class="ptReload" id="ptReload"><img src="<%=BaseImgPath%>/Main/ico_ref.gif" alt="새로고침" title="새로고침" /></a>
                                <a class="ptClose" id="ptClose"><img src="<%=BaseImgPath%>/Main/ico_newClose.gif" alt="닫기" title="닫기" /></a>
                            </li>
                        </ul>
                    </div>
                    <div class="right"></div>
                </div>
                <div class="ptl_laysec"></div>
                <div class="portlet-content" style="min-height:520px;">




                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/Popup.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/Popup.js");
    </script>
</body>
</html>
