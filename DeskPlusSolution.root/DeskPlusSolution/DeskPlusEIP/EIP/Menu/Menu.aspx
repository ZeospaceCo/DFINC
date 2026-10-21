<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Menu.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Portal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Portal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
</head>
<body>
<form id="form1" runat="server">

<div id="layout_menu">
    <!-- ############# start of contextMenu ############## -->
    <ul id="ContextSubMenuUserDept" class="contextMenu" style="width:220px;height:60px;overflow-y:auto !important;">
        <%=UserDeptName %>
    </ul>
    <!-- ############# end of contextMenu ############## -->
    <div class="UTIL">
    	<div class="menu_logo"><a href="/DeskPlusEIP/" target="_parent"><img src="<%=BaseImgPath%>/Portal/logo.gif" title="" alt="" /></a></div>
        <div class="menu_srh">
            <ul class="btn_searchboxL">
                <li>
                <input type="text" id="dummy" style="display: none;" />
                <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="ptl_srhtext" text=""></cc3:ClientSmartTextBox>
                </li>
                <li><a id="btnSearch" href="#" class="ptl_srhbtn" title="<%=GetResource("Search")%>"><%=GetResource("Search")%></a></li>
                <li><a id="btnSearchDetail" href="#" title="<%=GetResource("DeepSearch")%>"><img src="<%=BaseImgPath%>/Btn/srh_detail.gif" alt="<%=GetResource("DeepSearch")%>"/></a></li>
            </ul>
        </div>
        <div class="aRight">
        <ul class="menu_util">
            <li><!-- 겸직 정보 -->
                <ul class="ptl_btn_info">
                    <li class="left"></li>
                    <li><%= CurrentUserInfo.DeptName %> <%= CurrentUserInfo.Name %>  <%= CurrentUserInfo.TitleName %></li>
                    <% if (BaseDeptCnt > 1)
                       { %>
                    <li><a id="ContextMenuUserDept" class="callContextMenu" href="#">▼</a></li>
                    <% } %>
                    <li class="right"></li>
                </ul>
                <!-- 겸직 정보 -->
            </li>
            <li><P class="date_title"><asp:Literal id="lbTodayDate" runat="server"></asp:Literal></P></li>
            <li><a id="MyInfo"><%=GetResource("AdminMyInfo")%></a></li>
            <li>|</li>
            <li><a href="/DeskPlusEIP/Data/Cab/UCWare_m_Setup.exe"><%=GetResource("DownloadMsg")%></a></li>
            <li>|</li>
            <li><asp:HyperLink ID="SignOutLink" runat="server"></asp:HyperLink></li>
            <li>|</li>
            <li id="LiAdminLink" runat="server" visible="false"><asp:HyperLink ID="AdminLink" Target="_parent" runat="server"></asp:HyperLink></li>
        </ul>
        <!--ul class="menu_util">
            <li><a href="#">즐겨찾기▼</a></li>
        </ul-->
        </div>
    </div><!-- //UTIL --> 
    
    <div class="GNB">
        <ul class="menu_gnb">
        <%=TopMenu %>              
        </ul>
    </div><!-- //GNB --> 

    <div class="SUB">
        <ul class="menu_sub">
            <div id="plSubMenu" style="clear: both;"></div>
        </ul>        
    </div><!-- //SUB -->     
</div><!-- //layout_menu --> 
<iframe src="about:blank" id="ifrmSLO" name="ifrmSLO" width="0" height="0"></iframe>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>     
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var LoginID = "<%=CurrentUserInfo.LogInID %>";
        var Password = "<%=CurrentUserInfo.Password %>";
        var MenuID = "0";
        var DirectCallMenuID = "<%=DirectCallMenuID%>";
        var TargetParam = "<%=TargetParam%>";
        var R_Complete = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_InputSearchValue = "<%=GetResource("InputSearchValue") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Menu/Menu.js");

        /**************************************************
        작성자   : freeman
        작성일자 : 2012-05-10
        작성목적 : DeskPlusApp 사용하는곳에서 임시 사용 DPApp 사용하지 않으면 삭제 필요 (33버전)
        ***************************************************/
        $(function () {
            $(document).ready(function () {
                //                ParamArray = {
                //                    'cID' : CompanyID,
                //                    'uID' : LoginID,
                //                    'uPW' : Password
                //                };

                //                CallEncrypt(CallGetParam());

                //                var url = "/DeskPlusApp/Site/Auth/LoginOutSide.aspx?p=" + GetEncryptValue;

                //                $("#ifrmSLO").attr("src", url);
            });
        });
    </script>
</body>
</html>
