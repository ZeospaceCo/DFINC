<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySpace.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MySpace" %>
<script type="text/javascript">
    var myspace_tabPortletID = "<%=TabPortletID%>";
    var myspace_portletID = "<%=PortletID%>";
    var myspace_categoryID = "<%=CategoryID%>";
    var myspace_dataURL = "<%=DataURL%>";
    var myspace_dataType = "<%=DataType%>";
    var myspace_IsAuth = "<%=IsAuth%>";
    var myspace_IsMini = "<%=IsMinimize %>";

    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    var R_MailTitle = "<%=GetResource("MailTitle") %>";
    var R_WebHardTitle = "<%=GetResource("WebHardTitle") %>";
    var R_BoardTitle = "<%=GetResource("BoardTitle") %>";
    var R_ApprovalTitle = "<%=GetResource("ApprovalTitle") %>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';       
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);
    });
</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/MySpace.js"></script>

<!-- 포틀릿(잔여용량) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <div class="portlet-header">
        <div class="mid">
            <ul>
                <li><img src="<%=BaseImgPath%>/Main/ico_title.gif" alt="" />&nbsp;&nbsp;<%=Title%></li>                
                <li class="menu">                    
                    <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_set.gif" alt="<%=GetResource("Setting")%>" title="<%=GetResource("Setting")%>" /></a>
                    <a class="ptReload" id="ptReload_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_ref.gif" alt="<%=GetResource("Refresh")%>" title="<%=GetResource("Refresh")%>" /></a>
                    <a class="ptPreview" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><img src="<%=BaseImgPath%>/Main/ico_new.gif" alt="<%=GetResource("More")%>" title="<%=GetResource("More")%>" /></a>
                </li>
            </ul>
        </div>
    </div>
    <div class="ptl_laysec"></div>
    <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">        
        <div id="ptList_<%=TabPortletID%>_<%=PortletID%>" style=""></div>        
    </div>
</div>

<!-- 포틀릿(잔여용량) End //-->