<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Menu" %>
<div class="loginimgoverbox"></div>
<div class="login-logo-area">
  <div class="logo"><a href="../Portal/Portal.aspx"><img src="/DeskPlusMobile/FileData/<%=CompanyID%>/lgn_logo.png" /></a></div>  
</div>
<div>
    <!-- confirm 모달을 쓸 페이지에 추가 start-->
    <section class="mymodal modal-section type-confirm">
        <div class="enroll_box">
            <p class="menu_msg"></p>
        </div>
        <div class="enroll_btn">
            <button class="btnS btnT btn-primary btn_ok"><%=GetResource("ConfirmZR")%></button>
            <button class="btnS btnT modal_close"><%=GetResource("Cancel")%></button>
        </div>
    </section>
    <!-- confirm 모달을 쓸 페이지에 추가 end-->
    <!-- alert 모달을 쓸 페이지에 추가 start-->
    <section class="mymodal modal-section type-alert">
        <div class="enroll_box">
            <p class="menu_msg"></p>
        </div>
        <div class="enroll_btn">
            <button class="btnS btnT btn-primary modal_close"><%=GetResource("ConfirmZR")%></button>
        </div>
    </section>
    <!-- alert 모달을 쓸 페이지에 추가 end-->
</div>
<div class="navi bg-primary">
	<!-- navi -->
    <nav class="navbar navbar-expand-lg navbar-dark ">
		<div class="navbar-menu navbar-collapse mobilenavi">
		   <a class="dropdown-item viewtype " href="#" title=""><i class="fas fa-bars dropbtn fa-arrow-right"></i></a>
           <ul class="dropdown-content" id="mnav" aria-labelledby="mainmenu">
              <li id="maMail" runat="server">
             	  <a class="dropdown-item " href="../Mail/Folder.aspx" title="<%=GetResource("Menu_Mail")%>"><span class="label"><%=GetResource("Menu_Mail")%></span><i class="fas fa-envelope dropbtn "></i><span class="badge badge-light"><%=MailCnt%></span></a>
              </li>
              <li id="maApproval" runat="server">
                <a class="dropdown-item " href="../Approval/Folder.aspx" title="<%=GetResource("Menu_Approval")%>"><span class="label"><%=GetResource("Menu_Approval")%></span><i class="fas fa-file-signature  dropbtn " ></i><span class="badge badge-light"><%=AppCnt%></span></a>
              </li>
              <li id="maNotice" runat="server">
                <a class="dropdown-item" href="../Board/Folder.aspx" title="<%=GetResource("Menu_Board")%>"><span class="label"><%=GetResource("Menu_Board")%></span><i class="fas fa-align-left dropbtn "></i><span class="badge badge-light"><%=BoaCnt%></span></a>
              </li>
              <li id="maPims" runat="server">
                <a class="dropdown-item" href="../Pims/List.aspx"  title="<%=GetResource("Menu_Pims")%>"><span class="label"><%=GetResource("Menu_Pims")%></span><i class="fas fa-calendar dropbtn "></i><span class="badge badge-light"><%=PimCnt%></span></a>
              </li>
              <li id="maAddress" runat="server">
                <a class="dropdown-item" href="../Address/List.aspx" title="<%=GetResource("Menu_Address")%>"><span class="label"><%=GetResource("Menu_Address")%></span><i class="fas fa-address-book dropbtn "></i><span class="badge badge-light"></span></a>
              </li>
              <li id="maWebHard" runat="server">
                <a class="dropdown-item" href="../WebHard/Folder.aspx" title="<%=GetResource("Menu_WebHard")%>"><span class="label"><%=GetResource("Menu_WebHard")%></span><i class="fas fa-folder-open dropbtn "></i><span class="badge badge-light"></span></a>
              </li>
              <li id="maCommunity" runat="server">
                <a class="dropdown-item" href="../Community/Folder.aspx" title="<%=GetResource("Menu_Community")%>"><span class="label"><%=GetResource("Menu_Community")%></span><i class="fas fa-users dropbtn "></i><span class="badge badge-light"><%=ComCnt%></span></a>
              </li>
              <li id="maMyTalk" runat="server">
                <a class="dropdown-item" href="../MyTalk/List.aspx" title="<%=GetResource("Menu_MyTalk")%>"><span class="label"><%=GetResource("Menu_MyTalk")%></span><i class="fas fa-comments dropbtn "></i><span class="badge badge-light"><%=MyTalkCnt%></span></a>
              </li>
              <li id="maTask" runat="server">
                <a class="dropdown-item" href="../Task/Folder.aspx" title="<%=GetResource("Menu_Task")%>"><span class="label"><%=GetResource("Menu_Task")%></span><i class="fas fa-puzzle-piece dropbtn "></i><span class="badge badge-light"><%=TaskCnt%></span></a>
              </li>
              <li id="maRM" runat="server">
                <a class="dropdown-item" href="../RM/Folder.aspx" title="<%=GetResource("Menu_RM")%>"><span class="label"><%=GetResource("Menu_RM")%></span><i class="fas fa-calendar-minus dropbtn "></i><span class="badge badge-light"><%=RMCnt%></span></a>
              </li>
              <li id="maAttend" runat="server">
                <a class="dropdown-item" href="../HR/Attendance.aspx" title="<%=GetResource("Menu_Attendance")%>"><span class="label"><%=GetResource("Menu_Attendance")%></span><i class="fas fa-user dropbtn "></i><span class="badge badge-light"></span></a>
              </li>
              <li id="maNotification" runat="server">
                <a class="dropdown-item" href="../MDM/List.aspx" title="<%=GetResource("Menu_Notification")%>"><span class="label"><%=GetResource("Menu_Notification")%></span><i class="fas fa-bell dropbtn "></i><span class="badge badge-light"><%=NBCnt%></span></a>
              </li>
              <li id="maSet" runat="server">
                <a class="dropdown-item " href="../Config/List.aspx" title="<%=GetResource("Menu_Config")%>"><span class="label"><%=GetResource("Menu_Config")%></span><i class="fas fa-cog dropbtn "></i><span class="badge badge-light"></span></a>
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