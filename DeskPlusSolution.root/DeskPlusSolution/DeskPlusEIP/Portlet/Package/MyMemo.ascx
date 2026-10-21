<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyMemo.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyMemo" %>
<script type="text/javascript">
    var mymemo_tabPortletID = "<%=TabPortletID%>";
    var mymemo_portletID = "<%=PortletID%>";
    var mymemo_portletName = "<%=Title%>";
    var mymemo_categoryID = "<%=CategoryID%>";
    var mymemo_dataURL = "<%=DataURL%>";
    var mymemo_dataType = "<%=DataType%>";
    var mymemo_IsAuth = "<%=IsAuth%>";
    var mymemo_IsMini = "<%=IsMinimize %>";
    var mymemo_MyMemo = "";
    var mymemo_OldMyMemo = "";
    var mymemo_BackColor = "";
    var mymemo_FontColor = "";
    var mymemo_MyMemoTitle = "";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    var R_PortletInfoSave = "<%=GetResource("PortletInfoSave") %>";
    var R_ProcessingError = "<%=GetResource("ProcessingError") %>";

</script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.autogrow-textarea.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/MyMemo.js"></script>
<!-- 포틀릿(메모지) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-memo">  

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></span>
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
                 <div class="content-tabs">
                    <!-- 내용 //-->
                    <textarea id="MyMemotxtInput" cols="30" rows="10"></textarea>
                 </div>
                 <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content">
                    <li class="page-item disabled" style="cursor:pointer"><a href="#" id="cbtnMemoSave" class="page-link"><%=GetResource("Save")%></a></li>
                    </ul>
                </nav>
            </div>
        </div>

        <div style="display: none">
                <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>

<!-- 포틀릿(메모지) End //-->