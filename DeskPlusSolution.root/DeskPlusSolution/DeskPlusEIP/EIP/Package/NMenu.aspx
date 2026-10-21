<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NMenu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Package.NMenu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PackageMenu.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PackageMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenuRight.js"></script>     
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/bootstrap.min.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/jquery.cookie.js"></script>

    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var LoginID = "<%=CurrentUserInfo.LogInID %>";
        var Password = "<%=CurrentUserInfo.Password %>";
        var MenuID = "0";
        var DirectCallMenuID = "<%=DirectCallMenuID%>";
        var DirectCall = "<%=DirectCall%>";
        var TargetParam = "<%=TargetParam%>";
        var ApprovalStatusNewMenuDirectCall = <%=ApprovalStatusNewMenuDirectCall%>;
        var R_Complete = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var R_InputSearchValue = "<%=GetResource("InputSearchValue") %>";
        var DropDept = "N";
        var DropSetting = "N";
        var TmpMenu = "";
        var HomeID = "<%=HomeID%>";
        var loginID_Enc = "<%= CurrentUserInfo.BirthAddress1 %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
        var R_ConfirmDeleteMenu =  "<%=GetResource("ConfirmDeleteMenu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Package/NMenu.js?v=3"></script>
</head>
<body>
<form id="form1" runat="server">
<div class="navi">

    <ul id="ContextSubMenuUserDept" class="contextMenu" style="width:auto;height:60px;overflow:auto;scrollbar-face-color:#d8d8d8;scrollbar-shadow-color:#a5a5a5;scrollbar-highlight-color:#a5a5a5;scrollbar-3dlight-color:#d8d8d8;scrollbar-darkshadow-color:#d8d8d8;scrollbar-track-color:#d8d8d8;scrollbar-arrow-color:#a5a5a5;">
        <%=UserDeptName %>
    </ul> 

    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
    	<!-- image type -->
		<div class="navbar-logo"><a class="navbar-brand" href="/" id="HomeReload"><img src="<%=MenuImage%>" /></a></div>
        
		<div class="navbar-menu  navbar-collapse">
           <ul class="dropdown-content" id="mnav" aria-labelledby="mainmenu">
              <%=TopMenu %>  
            </ul>
        </div>


 		<div class="navbar-alert">
            <div class="form-inline my-2 my-lg-0" >
                <ul class="nav">
                  <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarDropdownMenuLink" >
                        <div class="myimg">
                        <%=PhotoImage %>
                        <i class="fas fa-cog"></i>
                        </div>
                    </a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                        <div>                       
                            <asp:Literal id="lbTodayDate" runat="server"></asp:Literal>
                            <a class="dropdown-item" href="/DeskPlusEIP/Data/Cab/UCWare_m_Setup.exe"><%=GetResource("DownloadMsg")%></a>
                            <a class="dropdown-item" href="#"><span id="pro">Profile</span></a>
                            <a class="dropdown-item" href="#"><span id="sett">Settings</span></a>
                            <a class="dropdown-item" href="#"><span id="search-glass">Search</span></a>                                             
                            <span id="sadmin" runat="server" visible="false"><asp:HyperLink ID="lnkAdmin" Target="_parent" runat="server" class="dropdown-item">Admin</asp:HyperLink></span>
                        </div>
                    </div>
                  </li>
                  <li class="nav-item namecard">
                  		<div class="navDept"> 
                            <% if (BaseDeptCnt > 1) { %>
                               <a id="ContextMenuUserDept" class="callContextMenu" href="#">
                               <%= CurrentUserInfo.DeptName %> <%= CurrentUserInfo.Name %> (<%= CurrentUserInfo.TitleName %>) ▼</a>
                            <% } %> 
                            <% else { %>
                                <%= CurrentUserInfo.DeptName %> <%= CurrentUserInfo.Name %> (<%= CurrentUserInfo.TitleName %>)
                            <% } %> 
                        </div>
                        <div><a href="#" class="logout"><span id="lgot">Logout</span></a></div>    
                  </li>
                  </ul>
                  <a onclick="javascript:MyInfo_ShortCut('myInfo_NewMail')" title="<%=GetResource("SearchFieldMail")%>"><button type="button" class="btn btn-primary"><i class="fas fa-envelope"></i><span class="badge number badge-light" id="MCnt1"></span></button></a>
                  <a onclick="javascript:MyInfo_ShortCut('myInfo_TodayS')" title="<%=GetResource("TodaySchedule")%>"><button type="button" class="btn btn-primary"><i class="fas fa-calendar-minus"></i><span class="badge number badge-light" id="MCnt2"></span></button></a>
                  <a onclick="javascript:MyInfo_ShortCut('myInfo_Task')" title="<%=GetResource("Task")%>"><button type="button" class="btn btn-primary"><i class="fas fa-user-plus dropbtn"></i><span class="badge number badge-light" id="MCnt3"></span></button></a>
                  <a onclick="javascript:MyInfo_ShortCut('myInfo_AppApp')" title="<%=GetResource("CompleteDoc")%>"><button type="button" class="btn btn-primary"><i class="fas fa-file-signature dropbtn"></i><span class="badge number badge-light" id="MCnt4"></span></button></a>
            </div>
        </div>
    </nav>
</div>

<iframe src="about:blank" id="ifrmSLO" name="ifrmSLO" width="0" height="0"></iframe>
</form>    
</body>
</html>
