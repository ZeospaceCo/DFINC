<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Search.Help" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Search.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="srh_layout_left">
    
        <div class="srh_blue_t"></div>
        <div class="srh_blue_m">
            <ul class="srh_inbox">
                <li class="srh_darkgray_int"></li>
                <li class="srh_darkgray_inm">
                    <img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/srh_bar.gif" alt="" />
                </li>
                <li class="srh_darkgray_inb"></li>
            </ul>

            <ul class="srh_inbox">
                <li class="srh_gray_int"></li>
                <li class="srh_gray_inm">
                    <ul class="srh_ul">
                        <li class="bar"><img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/srh_barQna1.gif" alt="" /></li>
                        <li><%=GetResource("SearchWord1")%> <span class="srh_emp">&</span> <%=GetResource("SearchWord2")%></li>
                        <li><%=GetResource("SearchWord1n2")%></li>
                    </ul>
                </li>
                <li class="srh_gray_inb"></li>
            </ul>

            <ul class="srh_inbox">
                <li class="srh_gray_int"></li>
                <li class="srh_gray_inm">
                    <ul class="srh_ul">
                        <li class="bar"><img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/srh_barQna2.gif" alt="" /></li>
                        <li><%=GetResource("SearchWord1")%> <span class="srh_emp">|</span> <%=GetResource("SearchWord2")%></li>
                        <li><%=GetResource("SearchWord1or2")%></li>
                    </ul>
                </li>
                <li class="srh_gray_inb"></li>
            </ul>
            <!--
            <ul class="srh_inbox">
                <li class="srh_gray_int"></li>
                <li class="srh_gray_inm">
                    <ul class="srh_ul">
                        <li class="bar"><img src="<%=BaseImgPath%>/Search/srh_barQna3.gif" alt="" /></li>
                        <li>검색어1 <span class="srh_emp">!</span> 검색어2</li>
                        <li>검색어1은 포함, 2는 제외</li>
                    </ul>
                </li>
                <li class="srh_gray_inb"></li>
            </ul>

            <ul class="srh_inbox">
                <li class="srh_gray_int"></li>
                <li class="srh_gray_inm">
                    <ul class="srh_ul">
                        <li class="bar"><img src="<%=BaseImgPath%>/Search/srh_barQna4.gif" alt="" /></li>
                        <li>검색어1 <span class="srh_emp">(</span>검색조건2<span class="srh_emp">)</span></li>
                        <li>검색조건2 를 먼저 수행</li>
                    </ul>
                </li>
                <li class="srh_gray_inb"></li>
            </ul>
            -->
        </div>
        <div class="srh_blue_b">&nbsp;</div>

    </div>
    </form>
</body>
</html>
