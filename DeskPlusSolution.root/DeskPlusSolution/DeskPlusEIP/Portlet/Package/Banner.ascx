<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Banner.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Banner" %>
<script type="text/javascript">
    var Banner_tabPortletID = "<%=TabPortletID%>";
    var Banner_portletID = "<%=PortletID%>";
    var Banner_categoryID = "<%=CategoryID%>";
    var Banner_dataURL = "<%=DataURL%>";
    var Banner_dataType = "<%=DataType%>";
    var Banner_IsAuth = "<%=IsAuth%>";
    var Banner_ListCount = "<%=ListCount %>";
    var Banner_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";



    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        //삭제
        $("#del_<%=TabPortletID%>_<%=PortletID%>").on("click", function (e) {
            e.preventDefault();

            Main.DelTabPortlet("<%=TabPortletID%>_<%=PortletID%>");
            $("#portlet_<%=TabPortletID%>_<%=PortletID%>").fadeOut("slow");
        });

        //아이콘 감추기
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();

    });


    function MOver_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").show();
        //관리자가 아니면 - 관리자 경우 활성화
        if (Banner_IsAuth != "True") {
            $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
        }
    }

    function MOut_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
    }

</script>

<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">

        <div class="portlet-free" style="height:80px;" onMouseOver="MOver_<%=TabPortletID%>_<%=PortletID%>()" onMouseOut="MOut_<%=TabPortletID%>_<%=PortletID%>()">
            <a href="http://www.zeospace.com" target="_blank"><img alt="" title="" src="<%=BaseImgPath%>/Package/banner1.gif" /></a>
            
            <div class="flowdel" id="div_<%=TabPortletID%>_<%=PortletID%>">
                <a id="del_<%=TabPortletID%>_<%=PortletID%>"><img title="" alt=""  src="<%=BaseImgPath%>/Icon/ContextMenu/del.png" /></a>
            </div>
        </div>
    
</div>
<!-- 포틀릿(배너) End //-->