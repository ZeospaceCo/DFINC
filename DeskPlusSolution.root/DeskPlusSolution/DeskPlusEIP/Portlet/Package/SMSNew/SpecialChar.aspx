<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialChar.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMSNew.SpecialChar" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        

    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMSNew/SpecialChar.js");
    </script>

</head>
<body>
    <form id="form1" runat="server">


    <div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>

        <div class="inside">
            <div class="popup_header">
                <h5>특수문자</h5>                
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div class="layout_scroll" style="min-height:120px;height:120px;">
                <!-- 특수기호 레이어 -->
			        <div id="layer_sign" class="layer_sign">
				        <dl>					        
					        <dd class="scroll">
					
							        <a onclick="SpecialChar.AddChar('☆');">☆</a>
									<a onclick="SpecialChar.AddChar('○');">○</a>
									<a onclick="SpecialChar.AddChar('□');">□</a>
									<a onclick="SpecialChar.AddChar('◎');">◎</a>
									<a onclick="SpecialChar.AddChar('★');">★</a>
									<a onclick="SpecialChar.AddChar('●');">●</a>
									<a onclick="SpecialChar.AddChar('■');">■</a>
									<a onclick="SpecialChar.AddChar('☏');">☏</a>
									<a onclick="SpecialChar.AddChar('♪');">♪</a>
									<a onclick="SpecialChar.AddChar('☎');">☎</a>
									<a onclick="SpecialChar.AddChar('◈');">◈</a>
									<a onclick="SpecialChar.AddChar('▣');">▣</a>
									<a onclick="SpecialChar.AddChar('◐');">◐</a>
									<a onclick="SpecialChar.AddChar('◑');">◑</a>
									<a onclick="SpecialChar.AddChar('☜');">☜</a>
									<a onclick="SpecialChar.AddChar('☞');">☞</a>
									<a onclick="SpecialChar.AddChar('◀');">◀</a>
									<a onclick="SpecialChar.AddChar('▶');">▶</a>
									<a onclick="SpecialChar.AddChar('▲');">▲</a>
									<a onclick="SpecialChar.AddChar('▼');">▼</a>
									<a onclick="SpecialChar.AddChar('♠');">♠</a>
									<a onclick="SpecialChar.AddChar('♣');">♣</a>
									<a onclick="SpecialChar.AddChar('♥');">♥</a>
									<a onclick="SpecialChar.AddChar('◆');">◆</a>
									<a onclick="SpecialChar.AddChar('◁');">◁</a>
									<a onclick="SpecialChar.AddChar('▷');">▷</a>
									<a onclick="SpecialChar.AddChar('△');">△</a>
									<a onclick="SpecialChar.AddChar('▽');">▽</a>
									<a onclick="SpecialChar.AddChar('♤');">♤</a>
									<a onclick="SpecialChar.AddChar('♧');">♧</a>
									<a onclick="SpecialChar.AddChar('♡');">♡</a>
									<a onclick="SpecialChar.AddChar('◇');">◇</a>
									<a onclick="SpecialChar.AddChar('※');">※</a>
									<a onclick="SpecialChar.AddChar('♨');">♨</a>
							
					        </dd>
				        </dl>
			        </div>
			        <!-- //특수기호 레이어 -->
                </div>
            </div>
        </div>

        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>

            
    </form>
</body>
</html>
