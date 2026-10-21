<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSkinMail.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.GetSkinMail" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Package.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("TitSkinMail")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnGW" BeginEndTag="li" runat="server" />
                    <cc3:ClientSmartButton ID="CbtnConfirm" BeginEndTag="li" class="btn btn-secondary" runat="server" />
                    <cc3:ClientSmartButton ID="CbtnClose" BeginEndTag="li" runat="server" />
                </ul>
            </div>
            <div class="dotLine"></div>

            <!-- 스킨구성 //-->
            <div class="popup_layout_style3">
            
            <ul class="colorbox">
                    <ul class="colorbox">
                    <li class="colorchip">
                    <span style="background:#81c0ff;"><a href="#" onclick="GetSkinMail.SetSkin('blue');"></a></span> 
                    <span style="background:#a0abbf;"><a href="#" onclick="GetSkinMail.SetSkin('bluesilver');"></a></span>
                    <span style="background:#7b99b5;"><a href="#" onclick="GetSkinMail.SetSkin('bluegray');"></a></span>
                    <span style="background:#84c970;"><a href="#" onclick="GetSkinMail.SetSkin('green');"></a></span>
                    <span style="background:#ff884d;"><a href="#" onclick="GetSkinMail.SetSkin('orange');"></a></span>
                    <span style="background:#a0a0e4;"><a href="#" onclick="GetSkinMail.SetSkin('purple');"></a></span>
                    <span style="background:#5e5e5e;"><a href="#" onclick="GetSkinMail.SetSkin('black');"></a></span>
                    </li>
            	    <li class="colordir"><img src="<%=BaseImgPath%>/Package/colordir.gif" /></li>
                    <li class="colorsample"><img id="ColorSample" src="<%=BaseImgPath%>/Package/mailskin_<%=SelSkin%>.gif" /></li>    
            </ul>  
            </ul>
                
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
        var TabID = "<%= TabID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var SkinColor = "<%=SelSkin%>";
        var R_ConfirmApply = "<%=GetResource("ConfirmApply") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/GetSkinMail.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/GetSkinMail.js");
    </script>
</body>
</html>
