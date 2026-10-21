<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PerformMon.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.STA.PerformMon" %>
<script type="text/javascript">
    var performMon_tabPortletID = "<%=TabPortletID%>";
    var performMon_portletID = "<%=PortletID%>";
    var performMon_dataURL = "<%=DataURL%>";
    var performMon_dataType = "<%=DataType%>";
    var performMon_IsAuth = "<%=IsAuth%>";
    var performMon_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";
    var machinName = "<%=MachinName%>";

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/STA/PerformMon.js"></script>
<!-- 서버 상태 Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">

    
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><%=Title%></span>
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
                <div class="nav-tabs">                                   
                </div>
                  
                <div id="phperformMon" style="min-height:150px; text-align:center;">
                <canvas id="lineChart" style="display: inline-block;max-width: 600px; max-height: 400px;min-height:150px;"></canvas>
                </div>
            </div>
       </div>

       <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
       </div>

</div>
<!-- 서버 상태 End //-->