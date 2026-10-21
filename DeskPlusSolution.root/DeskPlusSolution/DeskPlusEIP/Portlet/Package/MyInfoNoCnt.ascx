<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInfoNoCnt.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyInfoNoCnt" %>
<script type="text/javascript">
    var myInfo_tabPortletID1 = "<%=TabPortletID%>";
    var myInfo_portletID1 = "<%=PortletID%>";
    var myInfo_categoryID1 = "<%=CategoryID%>";
    var myInfo_dataURL1 = "<%=DataURL%>";
    var myInfo_dataType1 = "<%=DataType%>";
    var myInfo_IsAuth1 = "<%=IsAuth%>";
    var myInfo_IsMini1 = "<%=IsMinimize %>";
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
    var ApprovalStatusMenuUrl = "<%=ApprovalStatusMenuUrl%>";

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
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/MyInfoNoCnt.js"></script>
<!-- 포틀릿(MyInfo-B) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    

    <ul class="pobox myinfo" onmouseover="this.className='pobox myinfo poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox myinfo';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'" >

        <div class="portlet-header">
            <!-- 상단 타이틀 //-->
            <li class="title headtitle"><a class="ptReload" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content" style="min-height:85px">
            <!-- 이미지, 사용자정보 //-->
            <li class="myinfoli">
                <span class="img"><a href="#" onclick="MyInfo_Mod()"><%=PhotoImage %></a></span>
                <span class="job"><label id="lblMyRootDeptInfo" runat="server"></label></span>
                <span class="name"><label id="lblMyDeptInfo" runat="server"></label>
                <% if (ddlChangeDept.Items.Count > 2)
                { %>
                <a href="#" id="ChangeDept_<%=TabPortletID%>_<%=PortletID%>">▼</a></span>
                <% } %>
                
                <span class="sns"><asp:Literal id="ltSNS" runat="server"></asp:Literal></span>
            </li>

            <div id="myUserDept_<%=TabPortletID%>_<%=PortletID%>" style="display:none">
                <li class="mylog" style="height:25px">
                    <asp:DropDownList ID="ddlChangeDept" onChange="MyInfo_ChangeUserDept(this)" runat="server" CssClass="searchselect" Width="190px" />
                </li>
            </div>
            <!-- 환경설정 //-->
            <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>"></div>
            <!-- 내용 //-->
            <li class="mytoday" style="display:none"><%=CurrentUserInfo.Name%>님 새로운 알림이 있습니다</li>
            <li class="mynew" style="display:none">
                <ul class="mynewlist">
                    <div id="ptList_<%=TabPortletID%>_<%=PortletID%>" style=""></div>
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
