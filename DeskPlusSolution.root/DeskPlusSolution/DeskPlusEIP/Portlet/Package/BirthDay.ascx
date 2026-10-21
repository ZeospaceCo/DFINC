<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BirthDay.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.BirthDay" %>
<script type="text/javascript">
    var birthDay_tabPortletID = "<%=TabPortletID%>";
    var birthDay_portletID = "<%=PortletID%>";
    var birthDay_categoryID = "<%=CategoryID%>";
    var birthDay_dataURL = "<%=DataURL%>";
    var birthDay_dataType = "<%=DataType%>";
    var birthDay_IsAuth = "<%=IsAuth%>";
    var birthDay_ListCount = "<%=ListCount %>";
    var birthDay_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";

    var birthDay_SearchValue = "";
    var birthDay_AnniversaryDate = "";
    var birthDay_Scope = "";

    var R_InputDate = "<%=GetResource("InputDate") %>";
    var R_NoContents = "<%=GetResource("NoContents") %>";
    var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
      
    var CID_BirthDayStartDate = "<%=BirthDayStartDate.ClientID %>"; 
    var CID_BirthDayEndDate = "<%=BirthDayEndDate.ClientID %>"; 

    var CID_BirthDayStartMonth = "<%=ddlBirthDayStartMonth.ClientID %>";
    var CID_BirthDayStartDay = "<%=ddlBirthDayStartDay.ClientID %>";
    var CID_BirthDayEndMonth = "<%=ddlBirthDayEndMonth.ClientID %>";
    var CID_BirthDayEndDay = "<%=ddlBirthDayEndDay.ClientID %>";

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/BirthDay.js"></script>
<!-- 포틀릿(축하합니다) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice"> 

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

    <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
        <li class="poboxlistset" style="vertical-align:text-bottom;">
            <%=GetResource("StartSearch")%> : <asp:DropDownList runat="server" ID="ddlBirthDayStartMonth"></asp:DropDownList> <%=GetResource("Month")%>
            <asp:DropDownList runat="server" ID="ddlBirthDayStartDay"></asp:DropDownList> <%=GetResource("Day")%>
            <span style="display:none;" class="listsetbox">        
            <a style="display:none;" onclick="BirthDay_DateChange('M')"  >◀</a>
            <input type="text" id="BirthDayStartDate" style="width:65px;" runat="server" />-
            <input type="text" id="BirthDayEndDate" style="width:65px;" runat="server" />
            <a style="display:none;" onclick="BirthDay_DateChange('P')" >▶</a>
            </span>
            <input style="display:none;" type="text" size="10" id="searchValue" value="" />
            
        </li>
        <hr />
        <li class="poboxlistset" style="vertical-align:text-bottom;">
            <%=GetResource("EndSearch")%> : <asp:DropDownList runat="server" ID="ddlBirthDayEndMonth"></asp:DropDownList> <%=GetResource("Month")%>
            <asp:DropDownList runat="server" ID="ddlBirthDayEndDay"></asp:DropDownList> <%=GetResource("Day")%>
            <a onclick="BirthDay_Search()"><img class="IconSearch" alt="" /></a>
        </li>
        <hr />
        <div id="phBirthDay" style="clear: both;" style="height:200px;"></div>
    </div>

    <div style="display: none">
    <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
	</div>	

</div>
<!-- 포틀릿(축하합니다) End //-->