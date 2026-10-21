<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pie.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Chart.Pie" %>
<script type="text/javascript">
    var chartPie_tabPortletID = "<%=TabPortletID%>";
    var chartPie_portletID = "<%=PortletID%>";
    var chartPie_dataURL = "<%=DataURL%>";
    var chartPie_dataType = "<%=DataType%>";
    var chartPie_IsAuth = "<%=IsAuth%>";
    var chartPie_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";
    var piePCount = "<%=PointCount%>";

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
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/Chart/Pie.js"></script>
<!-- 서버 상태 Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 
        <div class="portlet-header">
            <li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <div id="phchartPie" style="min-height:150px; text-align:center;"><canvas id="chartPie_<%=TabPortletID%>_<%=PortletID%>" style="display: inline-block;max-width: 300px; max-height: 300px;min-height:150px;"></canvas></div>
        </div>
    </ul>
</div>
<!-- 서버 상태 End //-->