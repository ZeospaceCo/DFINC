<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Opinion.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Opinion" %>
<script type="text/javascript">
    var opinion_tabPortletID = "<%=TabPortletID%>";
    var opinion_portletID = "<%=PortletID%>";
    var opinion_categoryID = "<%=CategoryID%>";
    var opinion_dataURL = "<%=DataURL%>";
    var opinion_dataType = "<%=DataType%>";
    var opinion_IsAuth = "<%=IsAuth%>";
    var opinion_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";

    var R_JTOpinionNoData = "<%=GetResource("JTOpinionNoData") %>";
    var R_JTOpinionItemNoData = "<%=GetResource("JTOpinionItemNoData") %>";
    var R_JTOpinionR1 = "<%=GetResource("JTOpinionR1") %>";
    var R_JTOpinionR2 = "<%=GetResource("JTOpinionR2") %>";
    var R_MenuF1= "<%=GetResource("MenuF1") %>";

    var portletDomID = opinion_tabPortletID + "_" + opinion_portletID;

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/Opinion.js"></script>

<!-- 포틀릿(설문조사) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">    

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
                <!-- 탭 //-->
                <div class="nav-tabs">                      
                </div>
                <!-- 내용 //-->
                <div class="content-tabs" id="ptList_<%=TabPortletID%>_<%=PortletID%>">
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
<!-- 포틀릿(설문조사) End //-->