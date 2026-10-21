<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SMS.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMS.SMS" %>
<script type="text/javascript">
    

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);
        });

    });
</script>
<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" > 
        <div class="portlet-header"><li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li></div>
        <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
            <a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>
        </li>
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <iframe name="ifrmSMSSend" id="ifrmSMSSend" src="/DeskPlusEIP/Portlet/Package/SMS/SendSMS.aspx" frameborder="0" scrolling="NO" leftmargin="0" marginheight="0" marginwidth="0" style="width:200px;height:530px;"></iframe>
        </div>
    </ul>
    
</div>
<!-- 포틀릿(배너) End //-->