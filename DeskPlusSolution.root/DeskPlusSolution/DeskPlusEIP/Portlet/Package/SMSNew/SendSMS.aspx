<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSMS.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMSNew.SendSMS" %>

<!DOCTYPE html>

<html>
<head>

    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>    
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link type="text/css" rel="stylesheet" href="sample/simple.css" />

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    

    <script type="text/javascript">

        var ReplyNumberList = "<%=ReplyNumberList %>";
        var ParamPhoneNumber = "<%=ParamPhoneNumber %>";
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var UserID = "<%=CurrentUserInfo.UserID  %>";
        var PreLang = "<%=PreLang %>";

        var INPUT_PHONEMSG_MSG = "";

        var R_LIMIT_PHONEMSG_MSG = "허용 길이 이상의 글을 쓰셨습니다.\n80 bytes 이상 전송시 메시지 끝부분이 잘릴수 있습니다.";
        var R_ShallWeInputSave = "문장을 저장하시겠습니까?";
        var R_SMS_SMSConfig_Success = "처리되었습니다.";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        window.onload = function () {            
            window.resizeTo(450, 600);
        }


    </script>

    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMSNew/SendSMS.js");
    </script>

</head>
<body>
    <form id="form1" runat="server">
<table border="0" cellpadding="0">
<tr>
	<td width="80"></td>

	<td>

     <!-- 폰 영역(기본형) -->
		<div class="col-phone">
			<h3>메시지 입력창</h3>
			<!-- 입력창 -->
			<div class="message"><!-- onkeydown="return checkAndSaveCurPos( event );" -->
				<textarea cols="16" rows="5" id="txtMessage" name="txtMessage" onselect="SendSMS.Reset()" onclick="SendSMS.Reset()" onkeyup="SendSMS.CheckLen()"></textarea>
			</div>
			<!-- //입력창 -->

			<!-- 추가기능 -->
			<div class="add-function">
				<span class="byte" id="txtLen"> 0/80</span>                
				<button type="button" id="SpecialChar" class="sign" title="특수기호">특수기호</button>                
				<button type="button" id="SaveChar" class="save" title="앨범저장">문자저장</button>                
				<button type="button" id="CallSaveChar" class="open" title="문자열기">열기</button>

			</div>
			<!-- 보내는 사람 -->
			<div class="sender f-clear">
				<strong>보내는사람</strong>
				<input type="text" class="text" name="SendNumber" id="SendNumber" value="<%=CurrentUserInfo.MobilePhone %>" readonly />
			</div>
			<!-- //보내는 사람 -->

			<!-- 받는사람 -->
			<div class="reciver">
				<strong>받는사람</strong>
				<p class="btn">
					<button type="button" class="address" id="CallAddress" title="주소록">주소록</button>
					<button type="button" class="recent" id="RecentNumber" title="최근번호">최근번호</button>
				</p>			

				<!-- 받는 사람 목록 -->
				<div class="wrapper">
					<!-- 검색 -->
					<fieldset>
						<legend>전화번호 추가</legend>
						<p>
							<input id="quickSearch" type="text" class="text" title="전화번호를 입력하세요" value="전화번호를 입력하세요" onfocus="this.value='';" />
							<button type="button" id="AddPhone" title="추가">추가</button>
						</p>
					</fieldset>
					<!-- //검색 -->

					<ul id="rcptUl" class="list scroll" style="cursor:normal;">
						<li id="rcptDesc" class="desc">
							<span class="name" id="rcpt_new_name">이름</span><span class="number" id="rcpt_new_number">수신번호</span>
						</li>
					</ul>					
				</div>
				<!-- //받는 사람 목록 -->
			</div>
			<!-- //받는사람 -->

			<!-- 예약전송 -->
			<div class="booking">
				<span class="label off" id="ChkReservSend"><input type="checkbox" class="check"/><em>예약전송</em></span>
				<div id="ChkReservSendInfo" class="info">				

					<!-- 한번 -->
					<div id="once" class="once" style="display:block;">
						<p class="f-clear" style="text-align:left">
							<input type="text" id="ReservYYYYMMDD" size="10" class="clsDate" />
						</p>
						<p class="f-clear">
							<select id="ReservHour">
								<option value="0" >오전 12시</option>								
								<option value="1" >오전 1시</option>								
								<option value="2" >오전 2시</option>								
								<option value="3" >오전 3시</option>								
								<option value="4" >오전 4시</option>								
								<option value="5" >오전 5시</option>								
								<option value="6" >오전 6시</option>								
								<option value="7" >오전 7시</option>								
								<option value="8" >오전 8시</option>								
								<option value="9" >오전 9시</option>								
								<option value="10" selected="selected">오전 10시</option>								
								<option value="11" >오전 11시</option>								
								<option value="12" >오후 12시</option>								
								<option value="13" >오후 1시</option>								
								<option value="14" >오후 2시</option>								
								<option value="15" >오후 3시</option>								
								<option value="16" >오후 4시</option>								
								<option value="17" >오후 5시</option>								
								<option value="18" >오후 6시</option>								
								<option value="19" >오후 7시</option>								
								<option value="20" >오후 8시</option>								
								<option value="21" >오후 9시</option>								
								<option value="22" >오후 10시</option								
								<option value="23" >오후 11시</option>								
							</select>
							<select id="ReservMinute">							
								<option value="0" >0분</option>								
								<option value="5" >5분</option>								
								<option value="10" >10분</option>								
								<option value="15" >15분</option>								
								<option value="20" >20분</option>								
								<option value="25" >25분</option>								
								<option value="30" >30분</option>								
								<option value="35" >35분</option>								
								<option value="40" >40분</option>								
								<option value="45" selected="selected">45분</option>								
								<option value="50" >50분</option>								
								<option value="55" >55분</option>								
							</select>
						</p>
					</div>
					<!-- //한번 -->

					<button type="button" class="close" title="닫기" id="ReservClose">닫기</button>
				</div>				
			</div>
			<!-- //예약전송 -->

			<!-- 버튼 -->
			<div class="button">
				<button type="button" id="SendMessage" class="b_send" title="전송">전송</button>
				<button type="button" id="ClearMessage" class="b_reset" title="재입력">재입력</button>
			</div>
			<!-- //버튼 -->
		</div>
		<!-- //폰 영역(기본형) -->

	</td>
	</tr>
</table>


    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->    
</body>
</html>