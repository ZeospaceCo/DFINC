<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Package.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PackageMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Package.css" />
    <!--[if IE 7]><link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PackageMenu7.css" media="all" /><![endif]-->
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />    
</head>
<body>
<form id="form1" runat="server">
<div id="container"> 
    <ul id="ContextSubMenuUserDept" class="contextMenu" style="width:auto;height:60px;overflow:auto;scrollbar-face-color:#d8d8d8;scrollbar-shadow-color:#a5a5a5;scrollbar-highlight-color:#a5a5a5;scrollbar-3dlight-color:#d8d8d8;scrollbar-darkshadow-color:#d8d8d8;scrollbar-track-color:#d8d8d8;scrollbar-arrow-color:#a5a5a5;">
        <%=UserDeptName %>
    </ul> 
        <div id="toparea">
        
        	<div class="util">

            <a id="HomeReload"><img src="<%=MenuImage%>" height="28px" alt="Menu Image" style=" padding-top:5px;" /></a>
            	<ul class="search" style="display:none">
                <li class="searchtextbox"><input type="text" id="dummy" style="display: none;" /><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="ptl_srhtext" text="" placeholder="제목, 첨부파일명" ></cc3:ClientSmartTextBox></li>
                <li class="gosearch bg"><a id="btnSearch" href="#" title="<%=GetResource("Search")%>"><img src="<%=BaseImgPath%>/Package/gosearch.png" /></a></li>
                <li class="mosearch bg"><a id="btnSearchDetail" href="#" title="<%=GetResource("DeepSearch")%>"><img src="<%=BaseImgPath%>/Package/mosearch.png" /></a></li>
	            </ul>
               
                           
             <ul class="shtmn" style="margin: 0px;" id="shtmn">
               <li><a href="#"><span class="pro" id="pro"><span>Profile</span></span></a></li>
               <!-- li><a href="#"><span class="mes"><span>Messages</span></span></a></li -->
               <li><a href="#"><span class="sett" id="sett"><span>Settings</span></span></a></li>
               <li><a href="#"><span class="search-glass" id="search-glass"><span>Search</span></span></a></li>
               <li><a href="#"><span class="lgot" id="lgot"><span>Logout</span></span></a></li>
               <li><span class="sadmin" id="sadmin" runat="server" visible="false"><span><asp:HyperLink ID="lnkAdmin" Target="_parent" runat="server"></asp:HyperLink></span></span></li>
               </ul>

            </div><!--div://util-->


			<div class="gnb">
            	<div class="gleft bg">
                    <ul class="menu">
                    <li class="id">
                    <em><%= CurrentUserInfo.DeptName %> <%= CurrentUserInfo.Name %> (<strong><%= CurrentUserInfo.TitleName %></strong>)</em>
                    </li>
                    <!--li class="idmore"><a href="#"><img src="<%=BaseImgPath%>/Package/idmore.gif" /></a></li-->
                    <li id="clock" class="idtime headtitle"><asp:Literal id="lbTodayDate" runat="server"></asp:Literal></li>
                    </ul>
				</div>
            	<div class="gright headtitle">
                    <ul class="mmenu">
                    <%=TopMenu %>  
                    </ul>
                    <ul id="subMenu" class="smenu topborder" style="position:absolute; left:250px; top:38px;"> <!-- 서브메뉴마다 위치 정해주어야 합니다. -->
                    <div id="plSubMenu"></div>
                    </ul>
				</div>
                
                <div class="gnbshadow"> </div>
            </div><!--div://gnb-->
            
        </div><!--div://toparea-->   
        
        <div id="dropmy">
                <div id="effect">
                	<div id="dropmenu">
                     <ul class="mymenu headtitle">
	                     <li><a href="#"><strong><%= CurrentUserInfo.DeptName %> <%= CurrentUserInfo.Name %> <%= CurrentUserInfo.TitleName %></strong></a> 
                            <% if (BaseDeptCnt > 1) { %>
                            &nbsp;<a id="ContextMenuUserDept" class="callContextMenu" href="#"> ▼</a>
                            <% } %>
                         </li><!-- 다중 직책일경우-->
                         <li><a href="#" id="MyInfo"><%=GetResource("AdminMyInfo")%></a></li>
                         <li><a href="#" id="SetSkin"><%=GetResource("ConfigTabSkin")%></a></li>
                         <li><a href="/DeskPlusEIP/Data/Cab/UCWare_m_Setup.exe"><%=GetResource("DownloadMsg")%></a></li>
                         <li><asp:HyperLink ID="SignOutLink" runat="server"></asp:HyperLink></li>
                         <li id="LiAdminLink" runat="server" visible="false"><asp:HyperLink ID="AdminLink" Target="_parent" runat="server"></asp:HyperLink></li>
                     </ul>
                     <ul class="mynew headtitle">
                        <div id="plTodayInfo"></div>
                     </ul>
                     
                    </div>
                </div>
                
                <div id="button">
                <p style=" background:red;"></p>
                </div>
        </div>            
	<!--span style="clear:both;border:0px; position:absolute; top:89px; left:20px; z-index:99;font-size:12px;">
	<a href="/DeskPlusEIP/Site/Auth/LogOut.aspx" alt=""><u style="color:white">[<%=GetResource("LogOut")%>]</u></span-->    
</div>

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
        var ImagePath = "<%=BaseImgPath%>";
        var R_InputSearchValue = "<%=GetResource("InputSearchValue") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Package/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Package/Menu.js");

        /**************************************************
        작성자   : freeman
        작성일자 : 2012-05-10
        작성목적 : DeskPlusApp 사용하는곳에서 임시 사용 DPApp 사용하지 않으면 삭제 필요 (33버전)
        ***************************************************/
        $(function () {
            $(document).ready(function () {

                //Home CI Reload
                $("#HomeReload").click(function (e) {
                    e.preventDefault();
                    parent.frmMain.location.href = "../Package/Main.aspx";
                    Menu.GoParam("home");
                });

                //                ParamArray = {
                //                    'cID': CompanyID,
                //                    'uID': LoginID,
                //                    'uPW': Password
                //                };

                //                CallEncrypt(CallGetParam());

                //                var url = "/DeskPlusApp/Site/Auth/LoginOutSide.aspx?p=" + GetEncryptValue;

                //                $("#ifrmSLO").attr("src", url);
            });
        });
    </script>
</body>
</html>
