/*
 ●●●●●●●●●●
 ● prototype 관련 ●
 ●●●●●●●●●●
*/

var ie5=(document.all && document.getElementById);
var ns6=(!document.all && document.getElementById);

//object.value.trim()
String.prototype.trim = function()
{
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//object.value.repl('aa','bb')
String.prototype.repl = function(aa,bb)
{
  rex = new RegExp(aa,"g");
  return this.replace(rex, bb);
}

function TypeCheck(target, cmt, astr, lmin, lmax)
{
        var i;

        if (target.length < lmin || target.length > lmax) {
                if (lmin == lmax) alert(cmt + '는 ' + lmin + ' 자 이어야 합니다');

                else alert(cmt + '는 ' + lmin + ' ~ ' + lmax + ' 자 이내로 입력하셔야 합니다');

                return false;
        }
		
		

        if (astr.length > 1) {
			
                for (i=0; i<target.length; i++)
                        if(astr.indexOf(target.substring(i,i+1))<0) {
								alert(cmt + '에 허용할 수 없는 문자가 입력되었습니다');
                                return false;
                        }
        }

        return true;
}  


/*
 ●●●●●●●●●●
 ● text 입력 관련 ●
 ●●●●●●●●●●
*/

//함수명	: onlyPhone
//설명		: 전화번호만 입력 가능 (0~9, '-')
//ex		: ONKEYPRESS="onlyPhone();"
function onlyPhone(){
	var keyCode = event.keyCode ? event.keyCode : 
	event.which ? event.which : event.charCode;
	if((keyCode>47 && keyCode<58) || (keyCode==45)) {
		event.returnValue = true;
        } else {
		event.returnValue = false;
        }
}

//함수명	: onlyNum
//설명		: 숫자만 입력 가능 (0~9)
//ex		: ONKEYPRESS="onlyNum();"
function onlyNum(){
	var keyCode = event.keyCode ? event.keyCode : 
	event.which ? event.which : event.charCode;
	if((keyCode>47 && keyCode<58)) {
		event.returnValue = true;
        } else {
		event.returnValue = false;
        }
}

//함수명	: onlyID
//설명		: ID 입력 관련 - 숫자, 영문 대소문자만 입력 가능)
//ex		: ONKEYPRESS="onlyID();"
function onlyID(){
	var keyCode = event.keyCode ? event.keyCode : 
	event.which ? event.which : event.charCode;
	if((keyCode==45) || (keyCode>47 && keyCode<58) || (keyCode>96 && keyCode<123) || (keyCode>64 && keyCode<91)) {
		event.returnValue = true;
	} else {
		event.returnValue = false;
	}
}

//함수명	: onlyID_regist
//설명		: ID 입력 관련 - 숫자, 영문 대소문자만 입력 가능)
//ex		: ONKEYPRESS="onlyID();"
function onlyID_regist(){
	var keyCode = event.keyCode ? event.keyCode : 
	event.which ? event.which : event.charCode;
	if((keyCode>47 && keyCode<58) || (keyCode>96 && keyCode<123) || (keyCode>64 && keyCode<91)) {
		event.returnValue = true;
	} else {
		event.returnValue = false;
	}
}

//함수명	: onlyMoney
//설명		: 숫자, 컴마 입력 가능 + 숫자 자동컴마 찍힘
//ex		: onkup="onlyNum(this);"
/* keycode
	48~58	: 숫자(키보드)'
	96~105	: 숫자(키패드)'
	8		: back space'
	9		: tab'
	46		: del'
	37		: <- (방향키)'
	39		: -> (방향키)'
	33~36	: home,end,pageup,pagedown'
	16~18	: alt,ctrl,shift'
	229		: 마우스 포커스 이동시
	27		: ESC
	13		: 엔터
	113~123 : F2~F12키
	*/
function onlyMoney(obj) {
	var keykey = event.keyCode;
	if((keykey==37) || (keykey==39) || (keykey>=33 && keykey<=36) || (keykey>=16 && keykey<=18) || (keykey==9) || (keykey==229) || (keykey==27) || (keykey==13) || (keykey>=113 && keykey<=123)) {
		//방향키,home,end,pageup,pagedown,tab등 허용
		return false;
	}else if ((keykey>=48 && keykey<=58) || (keykey>=96 && keykey<=105) || (keykey==8) || (keykey==46)) {
		var oldv = "";
		if(oldv == obj.value) return;
		oldv = obj.value;
		obj.value = formatNumber(oldv);
	}else {
//		alert(keykey);
		alert('숫자만 입력 가능합니다.');
		obj.value='';
		return false;
	}
}

//함수명	: autoTab
//설명		: 자동 포커스 이동
//ex		: ONKEYUP="autoTab(this,nextobj,size);"
function autoTab(obj1,obj2,ss){
	if (obj1.value.trim().length>=parseInt(ss,10)) {
		obj2.focus();
	}
}





/*
 ●●●●●●●●●●●
 ● 문자열 변환관련  ●
 ●●●●●●●●●●●
*/

//함수명	: Replace
//설명		: 특정 문자열 변환
//param		: sourceStr(원본문자열), findStr(앚을문자열), repStr(바꿀문자열)
function Replace( sourceStr, findStr, repStr ){
	var source = sourceStr;
	
	if ( source.length > 0 && findStr.length > 0 ) {
		while ( source.indexOf(findStr) >= 0 ) {
			source = source.replace( findStr, repStr );
		}
	}
		
	return source;
}

//함수명	: Left
//설명		: 문자열 왼쪽부터 잘라내기
function Left( sourceStr, charIdx ) {
	if((sourceStr==null) || (sourceStr=="")) return "";
	return sourceStr.substring(0, charIdx);
}

//함수명	: Right
//설명		: 문자열 오른쪽부터 잘라내기
function Right( sourceStr, charIdx ) {	
	if((sourceStr==null) || (sourceStr=="")) return "";
	return sourceStr.substring(sourceStr.length-charIdx, sourceStr.length);
}

//함수명	: formatNumber
//설명		: 자동 콤마 생성 (123456->123,456)
//param		: 바꿀 문자열
function formatNumber(s)
{	
	var str  = s.replace(/\D/g,"");    
    	var len  = str.length;
    	var tmp  = "";
    	var tm2  = "";
    	var i    = 0;  
    	str = str.substring(i,len);
    	len = str.length;
    	if(len < 3) {
    		return str;
    	}
   	else {
    		var sit = len % 3;
    		if (sit > 0) {
        		tmp = tmp + str.substring(0,sit) + ',';
        		len = len - sit;
    		}
    		while (len > 3) {
        		tmp = tmp + str.substring(sit,sit+3) + ',';
        		len = len - 3;
        		sit = sit + 3;
    		}
    		tmp = tmp + str.substring(sit,sit+3) + tm2;
    		str = tmp;
   	 }
    	return str;
}

//함수명	: cutStr
//설명		: 문자열 byte로 잘라내기
//param		: aquery(문자열), cc(바이트수)
function cutStr(aquery,cc)
{
	var tmpStr;
	var temp=0;
	var onechar;
	var tcount;
	tcount = 0;
	tmpStr = new String(aquery);
	l = tmpStr.length;

	for(k=0;k<l;k++)
	{
		onechar = tmpStr.charAt(k);
		
		if(escape(onechar).length > 4) {
			tcount += 2;
		}
		else if(onechar!='\r') {
			tcount++;
		}
		if(tcount>cc) {
			tmpStr = tmpStr.substring(0,k);
			break;
		}
	}
	
	if (tcount>cc) {
		return (tmpStr + ' ..');
	} else {
		return aquery;
	}
}

//함수명	: OpenWinow
//설명		: Window Open
//param		: left, top, width, height, name, url
function OpenWinow(L,T,W,H,nm,url){
	var winOpts="Left="+L+", Top="+T+", width="+W+", height="+H+", scrollbars=no"; 
	windowID = open(url, nm, winOpts);
	windowID.focus();
}

//함수명	: OpenWinowScroll
//설명		: Window Open
//param		: left, top, width, height, name, url
function OpenWinowScroll(L,T,W,H,nm,url){
	var winOpts="Left="+L+", Top="+T+", width="+W+", height="+H+", scrollbars=yes"; 
	windowID = open(url, nm, winOpts);
	windowID.focus();
}

//함수명	: flashobject
//설명		: Flash 외부 js에서 불러오기..
//param		: file,width,height,bgcolor,name
function flashobject(file,width,height,bgcolor,name) {
	document.write ("<OBJECT classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' codebase='http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0' WIDTH='" + width + "' HEIGHT='" + height + "' id='" + name + "'>");
	document.write ("<PARAM NAME='movie' VALUE='" + file + "'>");
	document.write ("<PARAM NAME='Src' VALUE='" + file + "'>");
	document.write ("<PARAM NAME='Quality' VALUE='high'>");
	document.write ("<PARAM NAME='BGColor' VALUE='" + bgcolor + "'>");
	document.write ("<PARAM NAME='WMode' VALUE='Window'>");
	document.write ("<PARAM NAME='Play' VALUE='-1'>");
	document.write ("<PARAM NAME='Loop' VALUE='-1'>");
	document.write ("<PARAM NAME='SAlign' VALUE=''>");
	document.write ("<PARAM NAME='Menu' VALUE='0'>");
	document.write ("<PARAM NAME='Base' VALUE=''>");
	document.write ("<PARAM NAME='AllowScriptAccess' VALUE='always'>");
	document.write ("<PARAM NAME='Scale' VALUE='ShowAll'>");
	document.write ("<PARAM NAME='DeviceFont' VALUE='0'>");
	document.write ("<PARAM NAME='EmbedMovie' VALUE='0'>");
	document.write ("<PARAM NAME='SWRemote' VALUE=''>");
	document.write ("<PARAM NAME='MovieData' VALUE=''>");
	document.write ("<PARAM NAME='SeamlessTabbing' VALUE='1'>");
	document.write ("<EMBED src='" + file + "' quality=high bgcolor='" + bgcolor + "' WIDTH='" + width + "' HEIGHT='" + height + "' NAME='" + name + "' TYPE='application/x-shockwave-flash' PLUGINSPAGE='http://www.macromedia.com/go/getflashplayer'></EMBED>");
	document.write ("</OBJECT>");
}

//함수명	: txtvalue
//설명		: 텍스트 체크후.. value값 공백처리..
//param		: obj,txt,cc
function txtvalue(obj,txt,cc) {
	if (cc=='click') {
		if (obj.value==txt) {
			obj.value='';
		}
	} else {
		if (obj.value=='') {
			obj.value=txt;
		}
	}
}

//함수명	: fn_login
//설명		: 텍스트 체크후.. value값 공백처리..
//param		: 
function fn_login() {
	var f = document.lf;
	if ((f.lf_usercode.value.trim()=='') || (f.lf_usercode.value.trim()=='아이디')) {
		alert('아이디를 입력해주세요.');
		f.lf_usercode.value = '';
		f.lf_usercode.focus();
	} else if ((f.lf_password.value.trim()=='') || (f.lf_password.value.trim()=='*****')) {
		alert('비밀번호를 입력해주세요.');
		f.lf_password.value = '';
		f.lf_password.focus();
	} else {
		f.submit();
	}
}

//함수명	: fn_guest
//설명		: guest계정 로그인 
//param	: 
function fn_guest(){
	
	var f = document.lf;

		f.lf_usercode.value = 'guest';
		f.lf_password.value = 'guest';

		f.submit();

	
}
//함수명	: fn_guest
//설명		: guest계정 로그인 _ 주소록 미리보기
//param	: 
function fn_guest_phonbook(){
	
	var f = document.lf;

		f.lf_usercode.value = 'guest';
		f.lf_password.value = 'guest';
		f.lf_path.value = '/phonebook/main/main.asp';

		f.submit();
	
}




function fn_login_enter(){
	if(window.event.keyCode==13){
		fn_login();
	}
}
function fn_logout(aa){
	location.href='/common/login/logout.asp?suremessenger_login='+aa;
}

function fn_go(url) {
	location.href=url;
}



//●○●○●○●○●○●○●○●○●○●○●○●○●○●○TIP의 샤샤샥 메뉴 시작●○●○●○●○●○
var apl = new Array();
apl[0] = new Array(45,"");
nOpac = 50
nPlus = 4
nMin = 4
speed = 30
timer = null; 
timer2 = null;

function fadeImg(teller) {
	document.all["tip_content"].style.posLeft = window.tiptip.offsetLeft-28;
	document.all["tip_content"].style.posTop = window.tiptip.offsetTop+19;
	if (document.all["tip_content"].style.display=="") {
		apl[teller][1] = "Down";
	} else {
		apl[teller][1] = "Up";
	}
	changes();
}
function fadeImgend(teller) {
	apl[teller][1] = "Down";
	setTimeout("changes()",50);
}
function changes() {
	next_loop = true;
	for (i=0;i<apl.length;i++) {
		obj = link_table;
		opacity = apl[i][0]
		if (apl[i][1] == "Up") {
			opacity += nPlus;
			apl[i][0] = opacity;
			if (apl[i][0] > 100) {
				apl[i][1] = "";
			} else {
                next_loop = false;
                $("#tip_content").css("display", "");				
			}
			nOpac = opacity;
		} else {
			if (apl[i][1] == "Down") {
				opacity -= nMin;
				apl[i][0] = opacity;
				if (apl[i][0] < 0) {
					apl[i][1] = "";					
					$("#tip_content").css("display", "none");
				} else {
					next_loop = false;
				}
				nOpac = opacity;
			}
		}
		if(ie5){ 
			obj.style.filter="alpha(opacity="+opacity+")";
		}
		if(ns6){ 
			obj.style.MozOpacity = opacity + '%';
		}
	}
	if (next_loop == false) {
		timer = setTimeout("changes()",speed);
	} else {
		clearTimeout(timer);
	}
}
//●○●○●○●○●○●○●○●○●○●○●○●○●○●○TIP의 샤샤샥 메뉴 끝●○●○●○●○●○



//함수명	: fn_BookMark()
//설명		: 북마크 설정
//param		: 
function fn_BookMark() {
	var bookmarkname = document.all["bookmark_name"];
	var bm_iframe = bookmark_iframe.document.f_bookmark;
	if (bookmarkname.value.trim()!='') {
		if (confirm('북마크에 추가하시겠습니까?')) {
			bm_iframe.ifrm_url.value = document.location.href;
			bm_iframe.ifrm_bmname.value = bookmarkname.value.trimText();
			if (parent.document.ifrm1 != null) {
				bm_iframe.ifrm1_url.value = parent.document.ifrm1.location.href;
			}
			if (parent.document.ifrm2 != null) {
				bm_iframe.ifrm2_url.value = parent.document.ifrm2.location.href;
            }

            $("#bookmark_input").css("display", "none");

			bm_iframe.action='/common/include/bookmark_save.asp';
			bm_iframe.submit();
		}
	} else {
		alert('북마크 이름을 입력해주세요.');
		bookmarkname.focus();
	}
}

//함수명	: fn_BookMarkView()
//설명		: 북마크 창 보기
//param		: ck (북마크 기본명)
function fn_BookMarkView(ck) {
    if (ck != '') {

        document.f_bookmark.ifrm_bmname_old.value = ck;

	    parent.document.all["bookmark_name"].value = ck;
        parent.document.all["bookmark_input"].style.posLeft = parent.window.bookbook.offsetLeft-64;
        parent.document.all["bookmark_input"].style.posTop = parent.window.bookbook.offsetTop + 25;

		if (parent.document.all["bookmark_input"].style.display == "") {
			parent.document.all["bookmark_input"].style.display = "none";
		} else {
			parent.document.all["bookmark_input"].style.display = "";
			parent.document.all["bookmark_name"].select();
		}
	} else {
		alert('로그인해주세요.');
	}
}


//함수명	: fn_BookMarkClose()
//설명		: 북마크 창 닫기
//param		: 
function fn_BookMarkClose() {
	document.all["bookmark_input"].style.display = "none";
}



//함수명	: fn_bookmark_go()
//설명		: 북마크 바로가기
//param		: aa (idx)
function fn_bookmark_go(aa) {
	var ff = document.f_bookmark;
	ff.ifrm_idx.value = aa.value;
	ff.action = '/common/include/bookmark.asp';
	ff.submit();
}


//공통 로그인 페이지
function suremLogin(){
	top.location.href="/";
}


//클럽보드에 복사하기
function clipclip(str) {
	window.clipboardData.clearData("Text");
	window.clipboardData.setData("Text",str);
	alert('클립보드에 복사되었습니다.')
}

//인터넷익스플로러 북마크 추가
function add_bookmark(aa,bb){
	window.external.AddFavorite(aa,bb)
}

//인터넷익스플로러 시작페이지 등록
function add_startpage(tt,aa) {
	tt.style.behavior='url(#default#homepage)';
	tt.setHomePage(aa);
}

function PleaseLogin(){
	alert("로그인 후 이용해 주세요");
	return;
}


//tr 들어올때 색깔 바꾸기
function inCell(cell, newcolor) {
	if (!cell.contains(event.fromElement)) {
		if ((cell.bgColor=='') || (cell.bgColor=='#ffffff')) {
			cell.bgColor = newcolor;
		}
	}
}

//tr 나갈때 색깔 바꾸기
function outCell(cell, newcolor) {
	if (!cell.contains(event.toElement)) {
		if ((cell.bgColor!='#bbbbbb')) {
			cell.bgColor = newcolor;
		}
	}
}

//사이버머니 가져오기
function load_money() {
	var nowmoney = document.all["span_check_money"];
	
	if (nowmoney != undefined) {
		var XRS;
		var returnval;
		XRS = XRSExecute("/common/include/XRSClass/XRS_money.asp", "money_check", "money");
		returnval = XRS.return_value;
		if ((returnval!='0')&&(returnval!=0)&&(returnval!='')) {
			nowmoney.innerHTML = ': ' + formatNumber(String(returnval)) + '원';
		}
	}
}
setInterval("load_money()",60000);

function testtest() {
	alert('abc');
}



//#####################################
//Main 페이지의 XP SP2 관련 스크립트
//#####################################

	//스크립트로 쿠키 가져오기
	function getCookie(name){

		var nameOfCookie = name + "=";

		var x = 0;

		while ( x <= document.cookie.length ){
			var y = (x+nameOfCookie.length);
			
			if ( document.cookie.substring( x, y ) == nameOfCookie ) {
				if ( (endOfCookie=document.cookie.indexOf( ";", y )) == -1 )
						endOfCookie = document.cookie.length;
				return unescape( document.cookie.substring( y, endOfCookie ) );
			}
			
			x = document.cookie.indexOf( " ", x ) + 1;

			if ( x == 0 )
				break;
		}

		return "";
	}

	 

	//쿠키값 저장하는 함수
	 function setCookie( name, value, expiredays ) {
		  var todayDate = new Date();
		  todayDate.setDate( todayDate.getDate() + expiredays );
		  document.cookie = name + "=" + escape( value ) + "; path=/;expires=" + todayDate.toGMTString() + ";"
	 }


//함수명	: OpenWinow 창을 모니터 가운데로 띄우기.
//설명		: Window Open  창을 모니터 가운데로 띄우기.
//param		: width, height, name, url
function OpenWinowCenter(W,H,nm,url){

	var L =  (window.screen.width  - W) / 2 ; //창이 뜰때 가운데로 뜨기
	var T  =  (window.screen.height - H) / 2 ;

	var winOpts="Left="+L+", Top="+T+", width="+W+", height="+H+", scrollbars=no;, status=no"; 
	windowID = open(url, nm, winOpts);
	windowID.focus();
}

//함수명	: OpenWinow 창을 모니터 왼쪽top 으로 띄우기.
//설명		: Window Open  창을 모니터 왼쪽top 으로 띄우기.
//param		: width, height, name, url
function OpenWinowTop(W,H,nm,url){

	var L =  0;
	var T  =  0;

	var winOpts="Left="+L+", Top="+T+", width="+W+", height="+H+", scrollbars=no;, status=no"; 
	windowID = open(url, nm, winOpts);
	windowID.focus();
}


        function showMenu(name,img)
        {
		var lastLayer = "";
       // 현재 서브 메뉴가 나타나 있다면 보이지 않게 만든다.
       if (document.all[name].style.display != "none") 
           {
           document.all[name].style.display = "none"
		   document.all[img].src = "../../images/common//bullet08.gif";

       }
       // 서브 메뉴가 나타나게 만든다.
       else 
           {
           document.all[name].style.display = ""	
		   document.all[img].src = "../../images/common//bullet07.gif";
           // 이전 서브 메뉴 레이어를 보이지 않게 만든다.
//           if(lastLayer != "" && name != lastLayer) 
//                   {
//               document.all[lastLayer].style.display = "none";
//           }
           // 현재 서브 메뉴 레이어 이름 기억
           lastLayer = name;		   
       }
    }