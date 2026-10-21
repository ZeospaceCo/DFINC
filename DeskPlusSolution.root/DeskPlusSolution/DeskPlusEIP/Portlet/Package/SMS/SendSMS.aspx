<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSMS.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.SMS.SendSMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
<style type="text/css">
<!--
@import url("Css/style.css");
-->
</style> 
<script language="javascript">

    var CompanyID = "<%=CompanyID %>";
    var UserID = "<%=UserID %>";
    var MessengerSendNumber = "<%=PhoneNumber %>"; //메신저에서 보내준 전화번호
    var g_D = 0;
    var headerfooter_time_year = "<%=Year %>";
    var headerfooter_time_month = "<%=Month %>";
    var headerfooter_time_day = "<%=Day %>";
    var headerfooter_time_hour = "<%=Hour %>";
    var headerfooter_time_minute = "<%=Minite %>";
    var headerfooter_time_second = "<%=Second %>";
    var headerfooter_time_wday = 2;
    /* 요일 (일요일: 0) */	


	var INPUT_PHONEMSG_MSG          = "";
	var IDX_DIV                     = 10;
	var LIMIT_PHONEMSG_MSG          = "허용 길이 이상의 글을 쓰셨습니다.\n80 bytes 이상 전송시 메시지 끝부분이 잘릴수 있습니다.";
	var WRONG_INPUT_PHONENUM_MSG    = "수신번호를 잘못 입력하셨습니다. 확인 후 10~12자리로 다시 입력해 주시기 바랍니다.";
	var NoDeleteNumber_MSG          = "더이상 삭제할 번호가 없습니다.";
	var Use2040_MSG                 = "2040년까지만 지원합니다.";
	var NoInputMonth_MSG            = "입력하신 달은 없습니다.";
	var Sunday_MSG                  = "일";
    var Monday_MSG                  = "월";
    var Tuesday_MSG                 = "화";
    var Wendensday_MSG              = "수";
    var Thursday_MSG                = "목";
    var Friday_MSG                  = "금";
    var Saturday_MSG                = "토";
    var NeedDate_MSG                = "날짜를 입력해주세요.";
    var LeapYear_MSG                = "윤년이 아닙니다. 다시 입력해주세요.";
    var DateLimit_MSG               = "날짜 범위를 벗어났습니다. 다시 입력해주세요.";
    var BeforeDate_MSG              = "기준일을 오늘보다 이전 날짜로 설정하세요.";
    var MoreThanZero_MSG            = "0 보다 큰 수를 입력하세요.";
    var Year_MSG                    = "년";
    var SetNextDay_MSG              = "기준일을 오늘 이후 날짜로 설정하세요.";
    var NextYear100_MSG             = "년도를 100년 이후로 입력해 주세요.";
    var Leap_MSG                    = "윤";
    var GoReplySettingPage          = "회신번호 설정 페이지로 이동하시겠습니까?";

    var NewYear = "신정";
    var NewYearsDay = "설날";
    var SamilIndependence = "삼일절";
    var Buddhism = "석가탄신일";
    var ChildDay = "어린이날";
    var MemorialDay = "현충일";
    var Constituent = "제헌절";
    var KoreaIndependenceDay = "광복절";
    var ThanksgivingDay = "추석";
    var NationalFoundationDay = "개천절";
    var Christmas = "성탄절";
    var FirstMonth15 = "대보름";
    var ArborDay = "식목일";
    var Danofestival = "단오";
    var ArmyDay = "국군의날";
    var KoreanAlphabet = "한글날";
    var Accident625 = "625사변일";
    var ThirdLunarMonth = "삼짇날";
    var WaterDay = "물의날";
    var FoolsDay = "만우절";
    var HandicappedDay = "장애인의날";
    var ScienceDay = "과학의날";
    var LeeSunSinDay = "충무공탄신일";
    var LawDay = "근로자의날/법의날";
    var ParentsDay = "어버이의날";
    var TeacherDay = "스승의날";
    var InventionDay = "발명의날";
    var SeaDay = "바다의날";
    var EnvironmentDay = "환경의날";
    var NippleDay = "제헌절";
    var SevenMonthDay = "칠월칠석";
    var CenterDay = "중앙절";
    var RailDay = "철도의날";
    var FireDay = "소방절";
    var SMS_WriteSMS_MSG_SUCESS = "전송되었습니다.";
    var InputSendMsg = "전송하실 메시지를 입력 해주세요.";
    var InputReceiveNumber = "수신번호를 입력해 주세요.";
    var SelectReservationDate = "예약일자를 선택해 주세요.";
    var SelectReservationTime = "예약시간을 입력해 주세요.";
    var ConfirmReservatonMonth = "예약월을 확인해 주시기 바랍니다. \n 예약시각은 현재이후이어야 합니다.";
    var InputCurrectTime0023 = "시간을 정확하게 입력하세요.(00∼23)";
    var InputCurrectTime0059 = "시간을 정확하게 입력하세요.(00∼59)";
    var ShallWeSendSMS = "전송하시겠습니까?";
    var ConfirmDel = "삭제하시겠습니까?";
    var SelectDeleteSendNumber = "삭제할 전송번호를 선택해주세요";
    var SelectInputSendNumber = "입력할 전송번호를 선택해주세요";
    var SelectDeleteContent = "삭제할 문자를 선택해주세요";
    var SelectInputContent = "입력할 문자를 선택해주세요";
    var SMS_SMSConfig_Success = "처리되었습니다.";
    var ShallWeInputSave = "문장을 저장하시겠습니까?";

</script>
<!--######### 달력만들기 시작 ######### -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>

    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/SendSMS.js");
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/replaynum.js");
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/recentnum.js");
        JsCall("/DeskPlusEIP/Portlet/Js/Package/SMS/openContent.js");
    </script>
</head>
<body onload="AddMessengerCheck()">
<form id="Form1" name="Form1" action>
	<!--######## 전체사이즈 width="177"##########-->
	<div align="center">
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td align="center"><img src="<%=BaseImgPath %>/SMS/guide_top.gif"></td>
			</tr>
			<tr>
				<td align="center">
				<!--메세지입력칸 시작-->
				<table width="177px" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="25"><img src="<%=BaseImgPath %>/SMS/guide_left.gif" /></td>
						<td>
						<!--##############메세지입력창 시작#############-->
						<span id="posLayerSMS" style="position:absolute;">
						</span>
						<table width="127" border="0" cellspacing="0" cellpadding="0">
							<tr>
								<td width="5"><img src="<%=BaseImgPath %>/SMS/M_top_left.gif"></td>
								<td background="<%=BaseImgPath %>/SMS/M_top_center.gif"></td>
								<td width="5"><img src="<%=BaseImgPath %>/SMS/M_top_right.gif"></td>
							</tr>
							<tr>
								<td background="<%=BaseImgPath %>/SMS/M_left.gif" width="5" height="104">
								</td>
								<td valign="top" background="<%=BaseImgPath %>/SMS/M_center.gif">
								<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td colspan="2" height="100%">
										<textarea id="txtMessage" name="txtMessage" cols="6" rows="4" class="phonemsgbox" value="" onclick="SelText()" onkeyup="javascript:checklen(this);" tabindex="11"></textarea></td>
									</tr>
									<tr>
										<td>
										<input type="text" id="msglen" name="msglen" class="phonemsglen" readonly value="0" size="2"><input type="text" id="whole" name="whole" class="phonemsglen" value="/ 80" readonly size="2" style="width:30;"></td>
										<td width="46px">
										<a onclick="Open_Emoticon()">
										<img src="<%=BaseImgPath %>/SMS/sp_mess.gif" border="0"></a></td>
									</tr>
								</table>
								</td>
								<td background="<%=BaseImgPath %>/SMS/M_right.gif" width="5" height="104">
								</td>
							</tr>
							<tr>
								<td width="5">
								<img src="<%=BaseImgPath %>/SMS/M_bottom_left.gif"></td>
								<td background="<%=BaseImgPath %>/SMS/M_bottom_center.gif"></td>
								<td width="5">
								<img src="<%=BaseImgPath %>/SMS/M_bottom_right.gif"></td>
							</tr>
						</table>
						<!--#############메세지입력창 시작###############--></td>
						<td width="25"><img src="<%=BaseImgPath %>/SMS/guide_right.gif" /></td>
					</tr>
				</table>
				<!--메세지입력칸 끝--></td>
			</tr>
			<tr>
				<td style="padding-left:3px;">
				<table border="0" cellspacing="0" cellpadding="0" id="table5">
					<tr>
						<td width="25" height="23"><img src="<%=BaseImgPath %>/SMS/mess_bottom_L.gif" /></td>
						<td width="130" background="<%=BaseImgPath %>/SMS/mess_bottom_C.gif" height="23">
						<table cellspacing="0" cellpadding="0"  border="0" id="table6">
							<tr>								
								<td><a onFocus="blur()" href="javascript:Open_Content()">
								    <img src="<%=BaseImgPath %>/SMS/M_open.gif" border="0"></a>
								</td>								
								<td><a onFocus="blur()" href="javascript:Save_Content()">
								<img src="<%=BaseImgPath %>/SMS/M_save.gif"></a></td>								
							</tr>
						</table>
						</td>
						<td width="25" height="23"><img src="<%=BaseImgPath %>/SMS/mess_bottom_R.gif" /></td>
					</tr>
				</table>
				</td>
			</tr>
			<tr>
				<td style="padding-left:3px;">
				<!--주소록/최근번호 버튼 시작-->
				<table width="177px" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td><img src="<%=BaseImgPath %>/SMS/guide_left_S.gif"></td>
						<td>
						<!--주소록-->
						<a onfocus="this.blur();" href="javascript:OpenAddressBook();">
						<img src="<%=BaseImgPath %>/SMS/add_but.gif" border="0"></a></td>
						<td>
						<!--최근번호-->
						<a onfocus="this.blur();" href="javascript:Open_Recent();">
						<img src="<%=BaseImgPath %>/SMS/number_but.gif" border="0"></a></td>
						<td><img src="<%=BaseImgPath %>/SMS/guide_right_S.gif"></td>
					</tr>
				</table>
				<!--주소록/최근번호 버튼 끝--></td>
			</tr>
			<tr>
				<td style="padding-left:3px;">
				<!--슬라이트 버튼 시작-->
				<table width="177px" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="81"><img src="<%=BaseImgPath %>/SMS/guide_bottom_01_1.gif"></td>
						<td>
						<div id="divFolderOpen" style="display:none;">
							<a onclick="folderContentOpen();" onfocus="blur()">
							<img src="<%=BaseImgPath %>/SMS/guide_bottom_close.gif" border="0"></a></div>
						<div id="divFolderClose">
							<a onclick="folderContentClose();" onfocus="blur()">
							<img src="<%=BaseImgPath %>/SMS/guide_bottom_open.gif" border="0"></a></div>
						</td>
						<td><img src="<%=BaseImgPath %>/SMS/guide_bottom_01_3.gif"></td>
					</tr>
				</table>
				<!--슬라이트 버튼 끝--></td>
			</tr>
			<tr>
				<td align="center"><img src="<%=BaseImgPath %>/SMS/guide_bottom_02.gif" /></td>
			</tr>
			<!-- ########### 폴더 열고 닫기 ########### -->	
			<tr id="folderContent">
				<td align="center">			
					<!--///////////////////////////////////////수신번호입력칸////////////////////////////////-->
					<table width="177px" height="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<!--07.07.11수정부분--><td width="18" background="<%=BaseImgPath %>/SMS/guide_left_text.gif"></td><!--07.07.11수정부분-->
							<td>
							<!--수신번호입력박스 시작-->
							<table width="100%" border="0" cellpadding="0" cellspacing="0" height="100%" bgcolor="ebebec">
								<!--수신번호-->
								<tr>
									<td>수신번호<input id="phone123" name="phone123" type="text" class="input_basic" size="10" maxlength="15" value autocomplete="off"></td>
								</tr>
								<!--수신번호-->
								<!--명/추가/삭제-->
								<tr>
									<td>
									<table width="100%" border="0" cellspacing="0" cellpadding="0" height="100%">
										<tr>
											<td style="font-family:돋움; font-size:13; ">
											<input name="sendlen" type="text" class="input_basic" size="2" readonly value="0"> 
											명</td>
											<td align="right">
											<table border="0" cellspacing="0" cellpadding="0">
												<tr>
													<td>
													<a onfocus="this.blur();" href="javascript:phoneCHK();">
													<img src="<%=BaseImgPath %>/SMS/but_add.gif" border="0"></a></td>
													<td>
													<a onfocus="this.blur();" href="javascript:DelCheck();">
													<img src="<%=BaseImgPath %>/SMS/but_del.gif" border="0"></a></td>
												</tr>
											</table>
											</td>
										</tr>
									</table>
									</td>
								</tr>
								<!--명/추가/삭제-->
								<!--번호리스트-->
								<tr>
									<td>
									<iframe id="tframe" frameborder="0" style="width:135;height:115;position:absolute;visibility:hidden;z-index:2;Filter: Alpha(Opacity=0);">
									</iframe>
									<iframe id="tframe2" frameborder="0" style="width:135;height:115;position:absolute;visibility:hidden;z-index:2;Filter: Alpha(Opacity=0);">
									</iframe>
									<select name="ExList" size="2" style="width:135px;height:80px;border:0px;z-index:1;" ondblclick="DelCheck();">
									</select></td>
								</tr>
								<!--번호리스트-->
								<!--회신번호-->
								<tr>
									<td>
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td>회신번호</td>
											<td>
											<div id="SelectPhoneList" style="position:absolute; width:103px; height:22px; z-index:2; display:none;">
											
												<table width="83" border="0" cellspacing="0" cellpadding="0" bgcolor="#CCCCCC">
													<tr>
														<td bgcolor="#FFFFFF">
														<table width="83" border="0" cellspacing="0" cellpadding="0" align="center">
															<%= ReplyNumberList%>
															<tr>
																<td width="83" align="left" onmouseout="mouseOut(this);" onmouseover="mouseOver(this);" onclick="ClickBoardList('none');">&nbsp;입력하지않음</td>
															</tr>
															<tr>
																<td width="83" align="left" onmouseout="mouseOut(this);" onmouseover="mouseOver(this);" onclick="ClickBoardList('');">&nbsp;직접입력</td>
															</tr>
															<tr>
																<td width="83" align="left" onmouseout="mouseOut(this);" onmouseover="mouseOver(this);" onclick="ClickSetReqNo();">&nbsp;회신번호설정</td>
															</tr>
														</table>
														</td>
													</tr>
												</table>
											</div>
											<span id="posLayerSMS2" style="position:absolute;">
											</span>
											<input id="req123_hidden" name="req123_hidden" type="hidden" value="<%=TopReplyNumber %>">
											<input id="req123" name="req123" type="text" class="input_basic" size="10" maxlength="15" value="<%=TopReplyNumber %>" tabindex="13" onkeypress="onlyPhone();" style="ime-mode:disabled;">
                                            <a href="javascript:SelectReqphone();"><img src="<%=BaseImgPath %>/SMS/but_but.gif" border="0" align="absmiddle" name="Img_SelectReqPhone"></a></td>
										</tr>
									</table>
									</td>
								</tr>
								<!--회신번호-->
								<!--예약전송-->
								<tr>
									<td><input type="checkbox" id="calltype" name="calltype" value="0" onclick="javascript:reserveCHK();" />예약전송</td>
								</tr>
								<!--예약전송-->
								<!--예약전송 설정-->
								<tr>
									<td align="center" style="padding-left:4px;">
									<div id="ReserveBody" style="display:none;">
										<table width="135" border="0" cellspacing="0" cellpadding="0">
											<tr>
												<td class="small_basic">
												<input id="yy" name="yy" type="text" class="input_basic" size="2" readonly tabindex="16" style="text-align:center;">년
												<input id="mm" name="mm" type="text" class="input_basic" size="1" readonly tabindex="17" style="text-align:center;">월
												<input id="dd" name="dd" type="text" class="input_basic" size="1" readonly tabindex="18" style="text-align:center;">일</td>
											</tr>
											<tr>
												<td class="small_basic" style="padding-left:46px;">
												<input id="h" name="h" type="text" class="input_basic" size="1" maxlength="2" tabindex="19" style="text-align:center;">시
												<input id="m" name="m" type="text" class="input_basic" size="1" maxlength="2" tabindex="20" style="text-align:center;">분
												</td>
											</tr>
										</table>
									</div>
									</td>
								</tr>
								<!--예약전송 설정-->
							</table>
							<!--수신번호입력박스 끝--></td>
							<!--07.07.11수정부분--><td width="19" background="<%=BaseImgPath %>/SMS/guide_right_text.gif"></td><!--07.07.11수정부분-->
						</tr>
					</table><!--///////////////////////////////////////수신번호입력칸////////////////////////////////-->
				</td>
			</tr><!-- ########### 폴더 열고 닫기 ########### -->
			<tr>
				<td align="center">
				<table cellspacing="0" cellpadding="0" border="0" id="table17">
					<tr>
						<td>
						<img height="30" src="<%=BaseImgPath %>/SMS/guide_bottom_03_01_open.gif" width="18" /></td>
						<!--width="71"로 수정--><td width="71">
						<a onFocus="blur()" href="javascript:SendSMS()">
						<img src="<%=BaseImgPath %>/SMS/guide_bottom_03_01_send.gif" border="0"></a></td>
						<!--width="69"로 수정--><td width="69">
						<a onFocus="blur()" href="javascript:Formreset()">
						<img src="<%=BaseImgPath %>/SMS/guide_bottom_03_01_re.gif" border="0"></a></td>
						<td>
						<img height="30" src="<%=BaseImgPath %>/SMS/guide_bottom_03_04_open.gif" width="19"></td>
					</tr>
				</table>
				</td>
			</tr>
			<tr>
				<td align="center"><img src="<%=BaseImgPath %>/SMS/guide_close.gif"></td>
			</tr>
		</table>		
		<!--############ 최근번호 시작 ##############-->
		<div id="Recent" style="position:absolute;z-index:4;display:none;">
			
			<table width="165" height="171" border="0" cellspacing="0" cellpadding="0" background="<%=BaseImgPath %>/SMS/vp_bg16.gif">
				<iframe id="ifrmRecentSMSProc" name="ifrmRecentSMSProc" width="0" height="0" border="0" frameborder="0"></iframe>
				<tr>
					<td width="151" height="33" align="right">
					<a href="javascript:Close_Recent();">
					<img src="<%=BaseImgPath %>/SMS/vp_btn17.gif" border="0"></a></td>
					<td width="14"></td>
				</tr>
				<tr>
					<td height="101" valign="top">
					<table width="137" height="100" align="right" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td valign="top">
							<iframe id="ifrm_recent" name="ifrm_recent" src="RecentNumberSMS.aspx" width="137" height="100" frameborder="0" scrolling="NO" leftmargin="0" marginheight="0" marginwidth="0">
							</iframe></td>
						</tr>
					</table>
					</td>
					<td></td>
				</tr>
				<tr>
					<td height="37" align="right" style="padding-bottom:6px;">					
					<a href="javascript:ifr_recent_del();">
					<img src="<%=BaseImgPath %>/SMS/vp_btn18.gif" name="Image29" width="37" height="16" border="0"></a>&nbsp;
					</td>
				</tr>
			</table>
		</div>
		<!--############ 최근번호 끝 ##############-->
		
		<!--############ 문자 열기 시작 ##############-->
		<div id="OpenContent" style="position:absolute;z-index:4;display:none;">		    
			
			<table width="165" height="171" border="0" cellspacing="0" cellpadding="0" background="<%=BaseImgPath %>/SMS/vp_bg17.gif">
				<iframe id="ifrmContentSMSProc" name="ifrmContentSMSProc" width="0" height="0" border="0" frameborder="0"></iframe>
				<tr>
					<td width="151" height="33" align="right">
					<a href="javascript:Close_Content();">
					<img src="<%=BaseImgPath %>/SMS/vp_btn17.gif" border="0"></a></td>
					<td width="14"></td>
				</tr>
				<tr>
					<td height="101" valign="top">
					<table width="137" height="100" align="right" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td valign="top">
							<iframe id="ifrm_openContent" name="ifrm_openContent" src="OpenContentSMS.aspx" width="137" height="100" frameborder="0" scrolling="NO" leftmargin="0" marginheight="0" marginwidth="0">
							</iframe></td>
						</tr>
					</table>
					</td>
					<td></td>
				</tr>
				<tr>
					<td height="37" align="right" style="padding-bottom:6px;">					
					<a href="javascript:ifr_openContent_del();">
					<img src="<%=BaseImgPath %>/SMS/vp_btn18.gif" name="Image29" width="37" height="16" border="0"></a>&nbsp;
					</td>
				</tr>
			</table>
		</div>
		<!--############ 문자 열기 끝 ##############-->
		
		<!--############전체사이즈 width="177"##############-->
		<div id="Emoticon" style="position:absolute;z-index:3;display:none;">
			<table width="177" border="0" cellspacing="0" cellpadding="0">
				<tr>
					<td height="34" background="<%=BaseImgPath %>/SMS/vp_bg17_1.gif">
					<!--- title -------->
					<table width="177" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td align="right">
							<a href="javascript:Close_Emoticon();">
							<img src="<%=BaseImgPath %>/SMS/vp_btn17.gif" border="0"></a></td>
							<td width="14">&nbsp;</td>
						</tr>
					</table>
					<!---------------------></td>
				</tr>
				<tr>
					<td background="<%=BaseImgPath %>/SMS/vp_bg17_2.gif">
					<!----특수문자랑 특수이모티콘----------------->
					<table width="145" align="center" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td>
							<!--특수문자------>
							<table width="145" border="0" bgcolor="F4F4F4" cellspacing="1" cellpadding="0">
								<tr>
									<td class="dotum" bgcolor="#FFFFFF" style="padding-top:5px;padding-left:8px;padding-right:5px;padding-bottom:5px;">
									<a href="javascript:AddChar('☆');">☆</a>
									<a href="javascript:AddChar('○');">○</a>
									<a href="javascript:AddChar('□');">□</a>
									<a href="javascript:AddChar('◎');">◎</a>
									<a href="javascript:AddChar('★');">★</a>
									<a href="javascript:AddChar('●');">●</a>
									<a href="javascript:AddChar('■');">■</a>
									<a href="javascript:AddChar('☏');">☏</a>
									<a href="javascript:AddChar('♪');">♪</a><br>
									<a href="javascript:AddChar('☎');">☎</a>
									<a href="javascript:AddChar('◈');">◈</a>
									<a href="javascript:AddChar('▣');">▣</a>
									<a href="javascript:AddChar('◐');">◐</a>
									<a href="javascript:AddChar('◑');">◑</a>
									<a href="javascript:AddChar('☜');">☜</a>
									<a href="javascript:AddChar('☞');">☞</a>
									<a href="javascript:AddChar('◀');">◀</a><br>
									<a href="javascript:AddChar('▶');">▶</a>
									<a href="javascript:AddChar('▲');">▲</a>
									<a href="javascript:AddChar('▼');">▼</a>
									<a href="javascript:AddChar('♠');">♠</a>
									<a href="javascript:AddChar('♣');">♣</a>
									<a href="javascript:AddChar('♥');">♥</a>
									<a href="javascript:AddChar('◆');">◆</a>
									<a href="javascript:AddChar('◁');">◁</a>
									<a href="javascript:AddChar('▷');">▷</a><br>
									<a href="javascript:AddChar('△');">△</a>
									<a href="javascript:AddChar('▽');">▽</a>
									<a href="javascript:AddChar('♤');">♤</a>
									<a href="javascript:AddChar('♧');">♧</a>
									<a href="javascript:AddChar('♡');">♡</a>
									<a href="javascript:AddChar('◇');">◇</a>
									<a href="javascript:AddChar('※');">※</a>
									<a href="javascript:AddChar('♨');">♨</a><br>
									</td>
								</tr>
							</table>
							<!-------------------></td>
						</tr>
						<tr>
							<td height="5"></td>
						</tr>
						<tr>
							<td height="1" class="td_line3"></td>
						</tr>
						<tr>
							<td height="5"></td>
						</tr>
						<tr>
							<td>
							<!--특수이모티콘------>
							<table width="145" border="0" bgcolor="#F4F4F4" cellspacing="1" cellpadding="0">
								<tr bgcolor="#FFFFFF">
									<td height="22" class="dotum" align="center">
									<a href="javascript:AddChar('(*^_^*)');">(*^_^*)</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('(^▽^)');">(^▽^)</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('S(^.^)b');">S(^.^)b</a></td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" class="dotum" align="center">
									<a href="javascript:AddChar('*^^*');">*^^*</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('^.~');">^.~</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('＠_＠');">＠_＠</a></td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" class="dotum" align="center">
									<a href="javascript:AddChar('★.★');">★.★</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('(V_V)');">(V_V)</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('(*_*)');">(*_*)</a></td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" class="dotum" align="center">
									<a href="javascript:AddChar('⊙.⊙');">⊙.⊙</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('(*^.☜)');">(*^.☜)</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('(=_=;)');">(=_=;)</a></td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" colspan="3" align="center" class="dotum">
									<table width="145" height="22" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td width="69" align="center" class="dotum">
											<a href="javascript:AddChar('(^*^)Kiss');">(^*^)Kiss</a></td>
											<td width="1" bgcolor="F4F4F4"></td>
											<td width="75" align="center" class="dotum">
											<a href="javascript:AddChar('(*^^)(^_^)');">(*^^)(^_^)</a></td>
										</tr>
									</table>
									</td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" colspan="3" align="center" class="dotum">
									<table width="145" height="22" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td width="81" align="center" class="dotum">
											<a href="javascript:AddChar(';p(*^0^)q');">;p(*^0^)q</a></td>
											<td width="1" bgcolor="F4F4F4"></td>
											<td width="63" align="center" class="dotum">
											<a href="javascript:AddChar('(-_-)…');">(-_-)…</a></td>
										</tr>
									</table>
									</td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" class="dotum" align="center">
									<a href="javascript:AddChar('(-.-+)');">(-.-+)</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('0(--)zz');">0(--)zz</a></td>
									<td class="dotum" align="center">
									<a href="javascript:AddChar('♡.♡');">♡.♡</a></td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" colspan="3" align="center" class="dotum">
									<table width="145" height="22" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td width="94" align="center" class="dotum"><a href="javascript:AddChar('/(*‘‘)♡(^^*)');">
											/(*‘‘)♡(^^*)</a></td>
											<td width="1" bgcolor="F4F4F4"></td>
											<td width="50" align="center" class="dotum"><a href="javascript:AddChar('^}{^');">
											^}{^</a></td>
										</tr>
									</table>
									</td>
								</tr>
								<tr bgcolor="#FFFFFF">
									<td height="22" colspan="3" class="dotum">&nbsp;
									<a href="javascript:AddChar('(/^0^)/(*^_^)');">(/^0^)/(*^_^)</a></td>
								</tr>
							</table>

							<!-----------------------></td>
						</tr>
					</table>
					<!---------------------------------------></td>
				</tr>
				<tr>
					<td><img src="<%=BaseImgPath %>/SMS/vp_bg17_3.gif"></td>
				</tr>
			</table>
		</div>
	</div>
</form>
<form name="frmSMSProc" method="post" target="ifrmSMSSend" action="SendSMSProc.aspx">
    <input type="hidden" name="companyID" />
    <input type="hidden" name="userID" />
    <input type="hidden" name="iMsg" />
    <input type="hidden" name="receiveNumber" />
    <input type="hidden" name="replyNumber" />
    <input type="hidden" name="yyyymmdd" />
    <input type="hidden" name="hms" />
    <input type="hidden" name="calltype_val" />
    <input type="hidden" name="deptName" />
</form>
<iframe id="ifrmSMSSend" name="ifrmSMSSend" action width="257" height="0" frameborder="0" scrolling="NO" leftmargin="0" marginheight="0" marginwidth="0"></iframe>


</body>

</html>

