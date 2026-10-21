<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInfo_Zeus.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Custom.MyInfo_Zeus" %>
<script type="text/javascript">
    var myInfo_tabPortletID = "<%=TabPortletID%>";
    var myInfo_portletID = "<%=PortletID%>";
    var myInfo_categoryID = "<%=CategoryID%>";
    var myInfo_dataURL = "<%=DataURL%>";
    var myInfo_dataType = "<%=DataType%>";
    var myInfo_IsAuth = "<%=IsAuth%>";
    var myInfo_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";

    var R_Count = "<%=GetResource("Count") %>";
    var BaseImgPathBtn = "<%=BaseImgPath%>/Lang/<%=PreLang%>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';

        $("#MenuList").append(conMenu);
    });

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Custom/MyInfo_Zeus.js"></script>
<!-- 최 상단 작업표시줄 메뉴바-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    
    <div class="portlet-header">
        <div class="mid">
            <ul>
                <li><img src="<%=BaseImgPath%>/Main/ico_title.gif" alt="" />&nbsp;&nbsp;<%=Title%></li>
                <li class="menu">
                    <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_set.gif" alt="<%=GetResource("Setting")%>" title="<%=GetResource("Setting")%>" /></a>
                    <a class="ptReload" id="ptReload_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_ref.gif" alt="<%=GetResource("Refresh")%>" title="<%=GetResource("Refresh")%>" /></a>
                    <!--a class="ptPreview" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_new.gif" alt="더보기" title="더보기" /></a-->
                </li>
            </ul>
        </div>
    </div>
 <!-- 최 상단 작업표시줄 메뉴바 종료 --> 

 <!-- 하단부분 시작 -->
    <div class="ptl_laysec"></div>
    <!-- 나의 정보 시작 (모듈 count제외) -->
    <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>"></div>       
        <div class="ptl_type4">
            <ul class="type4Top">
                <li class="tit">
                    <ul>
                        <li class="row"><a href="#" onclick="MyInfo_Mod()"><%=PhotoImage %></a></li>
                        <li>
                            <label class="date" id="lbToday" runat="server"></label>
                            <!--a href="jascript:alert('달력띄우기')" target="_blank"><img src="<%=BaseImgPath%>/Icon/ico_calendar-blue.gif" alt="달력" title="달력" /></a-->
                            <label class="info" id="lblMyDeptInfo" runat="server"></label>
                        </li>
                        <li>
                            <label class="sub" id="lblAccessDate" runat="server"></label></li>
                    </ul>
                </li>
            </ul>
    <!-- 나의정보 종료 -->
            
    <!-- 각 모듈 카운트 시작 -->
            <ul class="type4Cont">
                <li class="libg">
                    <div id="ptList_<%=TabPortletID%>_<%=PortletID%>" style="clear: both;"></div>                    
                </li>
            </ul>
    <!-- 각 모듈 카운트 종료 -->
   
        </div>

    </div>

</div>

<!-- 하단 최종 종료 //-->