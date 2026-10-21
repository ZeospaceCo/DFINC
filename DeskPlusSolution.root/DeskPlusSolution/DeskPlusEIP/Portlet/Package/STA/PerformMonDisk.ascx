<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PerformMonDisk.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.STA.PerformMonDisk" %>
<script type="text/javascript">
    var performMonDisk_tabPortletID = "<%=TabPortletID%>";
    var performMonDisk_portletID = "<%=PortletID%>";
    var performMonDisk_dataURL = "<%=DataURL%>";
    var performMonDisk_dataType = "<%=DataType%>";
    var performMonDisk_IsAuth = "<%=IsAuth%>";
    var performMonDisk_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";
    var machinName = "<%=MachinName%>";


</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/STA/PerformMonDisk.js"></script>
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
                  
                <div id="phperformMonDisk" style="min-height:150px; text-align:center;"><canvas id="barChart" style="display: inline-block;max-width: 600px; max-height: 400px;min-height:150px;"></canvas>
                [<span id="diskMbytes" style="width:200px; font-weight:bold;"></span>]
                </div>

            </div>
       </div>

       <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
       </div>
</div>
<!-- 서버 상태 End //-->