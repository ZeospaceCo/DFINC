<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInfo.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyInfo" %>
<script type="text/javascript">
    var myInfo_tabPortletID = "<%=TabPortletID%>";
    var myInfo_portletID = "<%=PortletID%>";
    var myInfo_categoryID = "<%=CategoryID%>";
    var myInfo_dataURL = "<%=DataURL%>";
    var myInfo_dataType = "<%=DataType%>";
    var myInfo_IsAuth = "<%=IsAuth%>";
    var myInfo_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";

    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";

    var R_Count = "<%=GetResource("Count") %>";
    var R_NoSettingInfo = "<%=GetResource("NoSettingInfo") %>";
    var BaseImgPathBtn = "<%=BaseImgPath%>/Lang/<%=PreLang%>";

    var R_MyInfo_Title1= "<%=GetResource("MyInfo_Title1") %>";
    var R_MyInfo_Title2= "<%=GetResource("MyInfo_Title2") %>";
    var R_MyInfo_Title3= "<%=GetResource("MyInfo_Title3") %>";
    var R_MyInfo_Title4= "<%=GetResource("MyInfo_Title4") %>";
    var R_MyInfo_Title5= "<%=GetResource("MyInfo_Title5") %>";
    var R_MyInfo_Title6= "<%=GetResource("MyInfo_Title6") %>";
    var R_MyInfo_Title7= "<%=GetResource("MyInfo_Title7") %>";
    var loginID_Enc = "<%= CurrentUserInfo.BirthAddress1 %>";
    var ApprovalStatusMenuUrl = "<%=ApprovalStatusMenuUrl%>";

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/MyInfo.js"></script>
<!-- 포틀릿(MyInfo-B) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-msearch">    

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
                <div class="nav-tabs">                                   
                </div>

                <div class="content-tabs myinfo">
                    <!-- 이미지, 사용자정보 //-->                    
                        <div class="myinfoli">
                            <span class="img"><a href="#" onclick="MyInfo_Mod()"><%=PhotoImage %></a></span>
                            <span class="job"><label id="lblMyRootDeptInfo" runat="server"></label></span>
                            <span class="name"><label id="lblMyDeptInfo" runat="server" class="nameDept"></label></span>
                            <span class="sns"><asp:Literal id="ltSNS" runat="server"></asp:Literal></span>
                        </div>
                        <!-- 내용 //-->
                        <div class="mytoday" style="display:none"><%=CurrentUserInfo.Name%>님 새로운 알림이 있습니다</div>
                        <div class="mynew">
                            <ul class="mynewlist">
                                <div id="ptList_<%=TabPortletID%>_<%=PortletID%>" style=""></div>
                            </ul>
                        </div>
                        <div class="mylog">
                            <label class="sub" id="lblAccessDate" runat="server"></label>
                            <label class="date" id="lbToday" runat="server" visible="false"></label>
                        </div>
                    
                </div>
               </div>
        </div>

        <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>

<!-- 포틀릿(MyInfo-B) End //-->
