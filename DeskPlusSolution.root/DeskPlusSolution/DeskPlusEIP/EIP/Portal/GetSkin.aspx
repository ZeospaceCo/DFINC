<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSkin.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.GetSkin" %>
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
                <h1><%=GetResource("ConfigTabSkin")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnMail" runat="server" BeginEndTag="li" Visible="false" />
                    <li class="sec clsmail"></li>
                    <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>

            <!-- 스킨구성 //-->
            <div class="popup_layout_style3">
            
            <ul class="colorbox">
                    <li class="colorchip">
                    <span style="background:#3d6efc;"><a href="#" onclick="GetSkin.SetSkin('C1');"></a></span> 
                    <span style="background:#3ec155;"><a href="#" onclick="GetSkin.SetSkin('C2');"></a></span>
                    <span style="background:#722ef5;"><a href="#" onclick="GetSkin.SetSkin('C3');"></a></span>
                    <span style="background:#f62e5c;"><a href="#" onclick="GetSkin.SetSkin('C4');"></a></span>
                    <span style="background:#ff5717;"><a href="#" onclick="GetSkin.SetSkin('C5');"></a></span>
                    <span style="background:#10b587;"><a href="#" onclick="GetSkin.SetSkin('C6');"></a></span>
                    <span style="background:#33b5e5;"><a href="#" onclick="GetSkin.SetSkin('C7');"></a></span>
                    </li>
            	    <li class="colordir"><img src="<%=BaseImgPath%>/Package/colordir.gif" /></li>
                    <li class="colorsample"><img id="ColorSample" src="<%=BaseImgPath%>/Package/colorsample<%=SelSkin%>.bmp" /></li>    
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
        var MailProduct = "<%=MailProduct %>";
        var R_ConfirmApply = "<%=GetResource("ConfirmApply") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/GetSkin.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/GetSkin.js");
    </script>
</body>
</html>
