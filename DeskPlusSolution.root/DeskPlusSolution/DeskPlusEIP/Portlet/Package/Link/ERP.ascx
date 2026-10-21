<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ERP.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Link.ERP" %>
<script type="text/javascript">


    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();

            ParamArray = {
            };

            CallEncrypt(CallGetParam());

            location = "/DeskPlusEIP/ETC/Customize/Default.aspx?p=" + GetEncryptValue;
        });

    });
</script>
<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" style="padding-bottom:0px"> 
        <div class="portlet-header">
            <li style="text-align: center" class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset">
                <!--<a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a>-->
            </li>
        </div>
    </ul>
</div>
<!-- 포틀릿(배너) End //-->