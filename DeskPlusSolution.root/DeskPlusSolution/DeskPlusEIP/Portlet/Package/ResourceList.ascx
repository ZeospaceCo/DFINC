<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceList.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.ResourceList" %>
<script type="text/javascript">
    var resourceList_tabPortletID = "<%=TabPortletID%>";
    var resourceList_portletID = "<%=PortletID%>";
    var resourceList_categoryID = "<%=CategoryID%>";
    var resourceList_dataURL = "<%=DataURL%>";
    var resourceList_dataType = "<%=DataType%>";
    var resourceList_IsAuth = "<%=IsAuth%>";
    var resourceList_IsMini = "<%=IsMinimize %>";

    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    var R_NoResources = "<%=GetResource("NoResources") %>"; 

</script> 
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/ResourceList.js"></script>

<!-- 포틀릿(자원현황-A) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-reservation">  
      
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">
                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
            </div>
        </div>     
        
        <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>

<!-- 포틀릿(자원현황-A) End //-->
