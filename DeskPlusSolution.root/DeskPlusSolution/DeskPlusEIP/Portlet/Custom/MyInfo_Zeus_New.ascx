<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInfo_Zeus_New.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Custom.MyInfo_Zeus_New" %>
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

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '</ul>';

        $("#MenuList").append(conMenu);
    });

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Custom/MyInfo_Zeus_New.js"></script>
<!-- 포틀릿(MyInfo-B) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox myinfo" onmouseover="poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" >
    <div class="portlet-header"><li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li></div>
    <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
    <li class="myinfoli">
        <span class="img"><a href="#" onclick="MyInfo_Mod()"><%=PhotoImage %></a></span>
        <span class="job"><label id="lblMyRootDeptInfo" runat="server"></label></span>
        <span class="name"><label id="lblMyDeptInfo" runat="server"></label></span>
        <span class="sns"><asp:Literal id="ltSNS" runat="server"></asp:Literal></span>
    </li>
    <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" height="200px"></div>
        <li class="mytoday" style="display:none"><%=CurrentUserInfo.Name%>님 새로운 알림이 있습니다</li>
            <li class="mynew">
                <ul class="mynewlist">
                    <div id="ptList_<%=TabPortletID%>_<%=PortletID%>" style=""></div>                    
                </ul>
            </li>
            <li class="mynew_zeus" style="height:50px">
                <ul class="mynewlist_zeus">
                     <div id="ptList_Zeus_<%=TabPortletID%>_<%=PortletID%>" style=""></div>                      
                </ul>
            </li>
        <li class="mylog">
        <label class="sub" id="lblAccessDate" runat="server"></label>
        <label class="date" id="lbToday" runat="server" visible="false"></label>
        </li>
    </div>
    </ul>
</div>

<!-- 포틀릿(MyInfo-B) End //-->