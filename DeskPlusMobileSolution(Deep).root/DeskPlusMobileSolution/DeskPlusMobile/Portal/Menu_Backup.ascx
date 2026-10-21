<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu_Backup.ascx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Menu_Backup" %>
<div class="loginimgoverbox"></div>
<div class="login-logo-area">
  <div class="logo"><a href="../Portal/Portal.aspx"><img src="/DeskPlusMobile/FileData/<%=CompanyID%>/lgn_logo.png" /></a></div>  
</div>
<div class="navi bg-primary">
	<!-- navi -->
    <nav class="navbar navbar-expand-lg navbar-dark ">
		<div class="navbar-menu navbar-collapse mobilenavi">
		   <a class="dropdown-item viewtype " href="#" title=""><i class="fas fa-bars dropbtn fa-arrow-right"></i></a>
           <ul class="dropdown-content" id="mnav" aria-labelledby="mainmenu">
              <li id="maMail" runat="server">
             	  <a class="dropdown-itemL" href="../Mail/Folder.aspx" title="<%=GetResource("Menu_Mail")%>" >
                    <i class="fas fa-envelope dropbtn"></i><br>
                    <span class="label"><%=GetResource("Menu_Mail")%></span>
                    <span class="badgeL badge-light"><%=MailCnt%></span>
                 </a>
              </li>
              <li id="maApproval" runat="server">
                <a class="dropdown-itemR " href="../Approval/Folder.aspx" title="<%=GetResource("Menu_Approval")%>">
                <i class="fas fa-file-signature  dropbtn" ></i><br>
                <span class="label"><%=GetResource("Menu_Approval")%></span>
                <span class="badge badge-light"><%=AppCnt%></span></a>
              </li>
              
              <li id="maNotice" runat="server">
                <a class="dropdown-itemL" href="../Board/Folder.aspx" title="<%=GetResource("Menu_Board")%>">
                <i class="fas fa-align-left dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Board")%></span>
                <span class="badgeL badge-light"><%=BoaCnt%></span></a>
              </li>
              <li id="maPims" runat="server">
                <a class="dropdown-itemR" href="../Pims/List.aspx"  title="<%=GetResource("Menu_Pims")%>">
                <i class="fas fa-calendar dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Pims")%></span>
                <span class="badge badge-light"><%=PimCnt%></span></a>
              </li>

              <li id="maAddress" runat="server">
                <a class="dropdown-itemL" href="../Address/List.aspx" title="<%=GetResource("Menu_Address")%>">
                <i class="fas fa-address-book dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Address")%></span>
                <span class="badgeL badge-light"></span></a>
              </li>
              <li id="maWebHard" runat="server">
                <a class="dropdown-itemR" href="../WebHard/Folder.aspx" title="<%=GetResource("Menu_WebHard")%>">
                <i class="fas fa-folder-open dropbtn "></i><br>                
                <span class="label"><%=GetResource("Menu_WebHard")%></span>
                <span class="badge badge-light"></span></a>
              </li>


              <li id="maCommunity" runat="server">
                <a class="dropdown-itemL" href="../Community/Folder.aspx" title="<%=GetResource("Menu_Community")%>">
                <i class="fas fa-users dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Community")%></span>
                <span class="badgeL badge-light"><%=ComCnt%></span></a>
              </li>
              <li id="maMyTalk" runat="server">
                <a class="dropdown-itemR" href="../MyTalk/List.aspx" title="<%=GetResource("Menu_MyTalk")%>">
                <i class="fas fa-comments dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_MyTalk")%></span>
                <span class="badge badge-light"><%=MyTalkCnt%></span></a>
              </li>

              <li id="maTask" runat="server">
                <a class="dropdown-itemL" href="../Task/Folder.aspx" title="<%=GetResource("Menu_Task")%>">
                <i class="fas fa-puzzle-piece dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Task")%></span>
                <span class="badgeL badge-light"><%=TaskCnt%></span></a>
              </li>
              <li id="maRM" runat="server">
                <a class="dropdown-itemR" href="../RM/Folder.aspx" title="<%=GetResource("Menu_RM")%>">
                <i class="fas fa-calendar-minus dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_RM")%></span>
                <span class="badge badge-light"><%=RMCnt%></span></a>
              </li>

             <li id="maAttend" runat="server">
                <a class="dropdown-itemL" href="../HR/Attendance.aspx" title="<%=GetResource("Menu_Attendance")%>">
                <i class="fas fa-user dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Attendance")%></span>
                <span class="badgeL badge-light"></span></a>
              </li>
              <li id="maNotification" runat="server">
                <a class="dropdown-itemR " href="../MDM/List.aspx" title="<%=GetResource("Menu_Notification")%>">
                <i class="fas fa-bell dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Notification")%></span>
                <span class="badge badge-light"><%=NBCnt%></span></a>
              </li>

              <li>
              </li>
             <li id="maSet" runat="server">
                <a class="dropdown-itemR " href="../Config/List.aspx" title="<%=GetResource("Menu_Config")%>">
                <i class="fas fa-cog dropbtn "></i><br>
                <span class="label"><%=GetResource("Menu_Config")%></span>
                <span class="badgeR badge-light"></span></a>
              </li>
              
            </ul>
        </div>
    </nav>
</div>

<script language="javascript">

    //모바일 앱에서 wake event 일경우
    function portalReload() {
        alert("최신 정보를 가져옵니다");
        document.location.reload();
    }

    //모바일 앱에서 push 알림이 왔을때 포탈 화면으로
    function portalLoad() {
        document.location.href = "/DeskPlusMobile/Portal/Portal.aspx";
    }
</script>