<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IFrame.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.IFrame" %>
<script type="text/javascript">
    var iframe_tabPortletID = "<%=TabPortletID%>";
    var iframe_portletID = "<%=PortletID%>";
    var iframe_categoryID = "<%=CategoryID%>";    
    var iframe_dataURL = "<%=DataURL%>";
    var iframe_dataType = "<%=DataType%>";
    var iframe_IsAuth = "<%=IsAuth%>";
    var iframe_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';        
        conMenu += '</ul>';

        $("#MenuList").append(conMenu);
    });

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/IFrame.js"></script>

<!-- 포틀릿(축하합니다) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 
        <div class="portlet-header"><li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li></div>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
            <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>
        </li>
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <div class="ptl_memo">
            <iframe id="ifrm" name="ifrm" class="pane ui-layout-center" width="100%" height="120px"  frameborder="0" scrolling="auto" src="http://www.naver.com" >
            </iframe>     
            </div>

        </div>
    </ul>
</div>
