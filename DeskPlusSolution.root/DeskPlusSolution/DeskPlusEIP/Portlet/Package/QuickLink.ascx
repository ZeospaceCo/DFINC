<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickLink.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.QuickLink" %>
<script type="text/javascript">
    var QuickLink_tabPortletID = "<%=TabPortletID%>";
    var QuickLink_portletID = "<%=PortletID%>";
    var QuickLink_dataURL = "<%=DataURL%>";
    var QuickLink_dataType = "<%=DataType%>";
    var QuickLink_IsAuth = "<%=IsAuth%>";
    var QuickLink_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";
    var ColumnID = "<%=ColumnID %>";


</script>

<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/QuickLink.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/livicons/raphael.min.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/livicons/livicons-1.4.min.js"></script>

<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">
    
       <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase text-primary"><%=Title%></span>
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
                    <div class="form-inline" style="padding-top:10px;padding-left:5px; justify-content: center;">
                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail" target="_parent">
                        <div class="quickDiv bg-secondary">  
                            <!--span class="material-icons">mail</span-->                        
                            <span class="livicon" data-n="mail" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-1" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Mail")%></div>
                        </div>
                        </a>                        

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=TASK" target="_parent">
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">more</span-->
                            <span class="livicon" data-n="users-add" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-2" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Task")%></div>
                        </div>
                        </a>
                        
                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalHome" target="_parent">  
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">edit</span-->
                            <span class="livicon" data-n="edit" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-3" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Approval")%></div>
                        </div>
                        </a>
             
                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims" target="_parent">
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">calendar</span-->
                            <span class="livicon" data-n="calendar" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-4"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Pims")%></div>
                        </div>
                        </a>

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=edms" target="_parent">  
                        <div class="quickDiv bg-secondary" style="display:none">
                            <!--span class="material-icons">folder-lock</span-->
                            <span class="livicon" data-n="folder-lock" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-5" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Edms")%></div>                        
                        </div>
                        </a>

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=Resource" target="_parent">  
                        <div class="quickDiv bg-secondary">   
                            <!--span class="material-icons">timer</span-->                     
                            <span class="livicon" data-n="timer" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-6" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Reservation")%></div>                        
                        </div>
                        </a>
             
                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=Community" target="_parent"> 
                        <div class="quickDiv bg-secondary">                        
                            <!--span class="material-icons">users</span--> 
                            <span class="livicon" data-n="users" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-7" style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Community")%></div>                        
                        </div>
                        </a>
             
                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=comSNS" target="_parent">                
                        <div class="quickDiv bg-secondary" style="display:none">
                            <!--span class="material-icons">comments</span-->
                            <span class="livicon" data-n="comments" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-8"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_SNS")%></div>
                        </div>
                        </a>

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=Address" target="_parent">                
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">inbox</span-->
                            <span class="livicon" data-n="address-book" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="Span1"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Address")%></div>                        
                        </div>
                        </a>

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=Board" target="_parent">                
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">inbox</span-->
                            <span class="livicon" data-n="pen" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-9"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Board")%></div>                        
                        </div>
                        </a>

                        <a href="/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=report" target="_parent">                
                        <div class="quickDiv bg-secondary">
                            <!--span class="material-icons">inbox</span-->
                            <span class="livicon" data-n="list" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="Span2"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Report")%></div>                        
                        </div>
                        </a>

                        <a href="#">                
                        <div class="quickDiv bg-secondary" style="display:none">
                            <!--span class="material-icons">gears</span-->
                            <span class="livicon" data-n="gears" data-s="30" data-c="#FFFFFF" data-hc="#585c66" id="livicon-10"  style="width: 49px; height: 30px;"></span>
                            <div><%=GetResource("Qlink_Setting")%></div>                        
	                    </div>
                        </a>
                    </div>
                </div>
            </div>
        </div>

         <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>
<!-- 포틀릿(배너) End //-->

