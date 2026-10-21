// toggle Layer
function toggleLayer( id ) {

	try {
		var obj = document.getElementById( id );
		return setLayerDisplay( id, (obj.style.display == "none") );
	} catch (e) {
		return true;
	}
}

// show Layer
function setLayerDisplay( id, display ) {
	try {
		document.getElementById( id ).style.display = display ? "block" : "none";
	} catch (e) {
		// do nothing
	}
	return true;
}

function showLayer( id ) {
	return setLayerDisplay( id, true );
}

function hideLayer( id ) {
	return setLayerDisplay( id, false );
}

function removeAllChildren( node ) {
	if ( node == null )
		return;
	while (node.hasChildNodes()) {
		node.removeChild( node.firstChild );
	}
}

function setText( node, text ) {
	if ( node == null )
		return;
	removeAllChildren( node );
	node.appendChild( document.createTextNode( text ) );
}

function addEvent( obj, type, fn ) {
	if ( obj.addEventListener ) {
		obj.addEventListener( type, fn, false );
	} else if ( obj.attachEvent ) {
		// ie 일경우
		obj["e" + type + fn] = fn;
		obj[type + fn] = function() {
			obj["e" + type + fn]( window.event );
		}
		obj.attachEvent( "on" + type, obj[type + fn] );
	}
}

function removeEvent( obj, type, fn ) {
	if ( obj.removeEventListener )
		obj.removeEventListener( type, fn, false );
	else if ( obj.detachEvent ) {
		obj.detachEvent( "on" + type, obj[type + fn] ); // ie 일경우
		obj[type + fn] = null;
		obj["e" + type + fn] = null;
	}
}

/**
 * 좌우 여백 제거
 */
function trim( str ) {
	while (str.charAt( 0 ) == " ") {
		str = str.substring( 1 );
	}

	while (true) {
		var len = str.length;
		if ( len == 0 || str.charAt( len - 1 ) != " " ) {
			break;
		}
		str = str.substring( 0, len - 1 );
	}

	return str;
}

function resizeWindowTo( width, height ) {
	try {
		var topWin = top.window;
		var oBody = parent.document.body;
		topWin.resizeTo( width, height );

		if ( getBrowser() != BROWSER_IE ) {
			topWin.resizeBy( 0, oBody.scrollHeight - topWin.innerHeight );
		} else {
			topWin.resizeBy( oBody.scrollWidth - oBody.clientWitdh, oBody.scrollHeight - oBody.clientHeight );
		}
	} catch (e) {
		// alert('Resize failed');
	}
}

function resizeWindow() {
	resizeWindowTo( 820, 632 );
}

function resizeWindowSKT() {
	resizeWindowTo( 820, 658 );
}

/**
 * @returns: ErrorCode, num1, num2, num3, hasStar, has200 ErrorCode : 0 ok
 */
function parseNumber( str ) {
	var result = new Array( 6 );

	var len = str.length;
	var num = str;
	var num1, num2, num3;
	var errorCode = 0;

	if ( len < 10 || len > 11 ) {
		errorCode = 1;
	} else {
		num1 = num.substring( 0, 3 );
		num2 = num.substring( 3, len - 4 );
		num3 = num.substring( len - 4, len );

		if ( num1 != "010" && num1 != "011" && num1 != "016" && num1 != "017" && num1 != "018" && num1 != "019" ) {
			errorCode = 2;
		} else {
			if ( !isNumeric( num ) )
				errorCode = 3;
		}
	}

	result[0] = errorCode;
	result[1] = num1;
	result[2] = num2;
	result[3] = num3;

	return result;
}

/**
 * 번호를 검사하고 - 을 채운다. <Valid 목록> 3자리 국번: 010-222-2222 4자리 국번: 010-2222-2222
 * 200메시지: 200-010-111-1111 별메시지: 010-222-3333*
 */
function addHipen( str, warn ) {
	var parsed = parseNumber( str );
	if ( parsed[0] != 0 ) {
		/*
		 * if(warn) window.alert( "번호 형식이 잘못되었습니다 - code:" + parsed[0] ); return "";
		 */
		if ( warn )
			window.alert( "번호 형식이 잘못되었습니다." );
		return "";
	}

	var num1 = parsed[1];
	var num2 = parsed[2];
	var num3 = parsed[3];
	var num = num1 + num2 + num3;
	var len = num.length;

	return num1 + "-" + num2 + "-" + num3;
}

function removeHipen( val ) {
	var newVal = "";
	for ( rhI = 0; rhI < val.length; rhI++ ) {
		if ( !(val.charAt( rhI ) == "-") )
			newVal = newVal + val.charAt( rhI );
	}
	return newVal;
}

/**
 * 숫자 3자리마다 , 붙인다
 */
function formatNumber( num ) {
	var src = String( num );
	var tmp = src.replace( /(\d)(\d{3}$)/g, "$1,$2" )
	var result = tmp.replace( /(\d)(\d{3},)/g, "$1,$2" );
	while (result != tmp) {
		tmp = result;
		result = tmp.replace( /(\d)(\d{3},)/g, "$1,$2" );
	}

	return result;
}

/**
 * 도토리 잔액표시, 100으로 나눈다.
 */
function formatAmount( num ) {
	var num2 = num / 100;
	var src = String( num2 );
	var tmp = src.replace( /(\d)(\d{3}$)/g, "$1,$2")
	var result = tmp.replace( /(\d)(\d{3},)/g, "$1,$2" );
	while ( result != tmp ) {
		tmp = result;
		result = tmp.replace( /(\d)(\d{3},)/g, "$1,$2" );
	}

	return result;
}

function isNumeric( num ) {
	for ( var i = 3; i < num.length; i++ ) {
		var ch = num.charAt( i );
		if ( ch < '0' || ch > '9' ) {
			return false;
		}
	}
	return true;
}

var BROWSER_NOT_SPECIFIED = 0x00;
var BROWSER_IE = 0x10; // Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1;
												// Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;
												// SV1) ; Embedded Web Browser from: http://bsalsa.com/;
												// .NET CLR 2.0.50727)
var BROWSER_FIREFOX = 0x20; // Mozilla/5.0 (Macintosh; U; PPC Mac OS X Mach-O;
														// ko; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6
var BROWSER_SAFARI = 0x30; // Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ko-kr)
														// AppleWebKit/412 (KHTML, like Gecko) Safari/412
var BROWSER_KONQUEROR = 0x40; // Mozilla/5.0 (compatible; Konqueror/3.5; Linux)
															// KHTML/3.5.7 (like Gecko) (Kubuntu)
var BROWSER_UNKNOWN = 0x99;

var currentBrowserType = BROWSER_NOT_SPECIFIED;
function getBrowser() {
	if ( currentBrowserType == BROWSER_NOT_SPECIFIED ) {
		var agentName = navigator.userAgent;

		if ( agentName.indexOf( "MSIE" ) > -1 && agentName.indexOf( "Windows" ) > -1 ) {
			currentBrowserType = BROWSER_IE;
		} else if ( agentName.indexOf( "Firefox" ) > -1 ) {
			currentBrowserType = BROWSER_FIREFOX;
		} else if ( agentName.indexOf( "Safari" ) > -1 || agentName.indexOf( "Mac" ) > -1 ) {
			currentBrowserType = BROWSER_SAFARI;
		} else if ( agentName.indexOf( "Konqueror" ) > -1 ) {
			currentBrowserType = BROWSER_KONQUEROR;
		} else {
			currentBrowserType = BROWSER_IE; // BROWSER_UNKNOWN;
		}
	}
	return currentBrowserType;
}

/**
 * Gets window version.
 * 
 * @returns NT 5.1 means Windows XP. NT 6.0 means Windows Vista.
 */
function getWindowsVer() {
	var version = navigator.appVersion;
	var idxToken = version.indexOf( "Windows" ) + 8;
	if ( idxToken > -1 ) {
		var idxSemi = version.indexOf( ";", idxToken );
		if ( idxSemi > -1 ) {
			var str = version.substring( idxToken, idxSemi );
			return str;
		}
	}
	return null;
}

function showUnsupportedBrowser() {
	alert( "서비스 준비 중인 기능입니다 " );
}

/**
 * 주어진 date 에서 연도를 반환한다. 2007, 2006 같은 값 반환. Firefox에서 getYear()가 107 을 반환하기
 * 때문에, 이 함 수 사용.
 */
function getYearFromDate( date ) {
	var year = date.getYear();
	if ( year < 1900 )
		year = year + 1900;
	return year;
}

function inputTextNumeric( e ) {
	var code = e.keyCode;
	if ( !(code >= 0x30 && code <= 0x39) && !(code >= 96 && code <= 105) ) {
		if ( code != 8 && code != 39 && code != 37 && code != 38 && code != 40 && code != 13 && code != 46 && code != 36 && code != 35 ) {
			e.returnValue = false;
			return false;
		}
	}
	return true;
}

function returnFalse() {
	return false;
}

function getInnerText( obj ) {
	if ( getBrowser() == BROWSER_FIREFOX ) {
		return obj.textContent;
	} else {
		return obj.innerText;
	}
}

function setInnerText( obj, str ) {
	if ( getBrowser() == BROWSER_FIREFOX ) {
		obj.textContent = str;
	} else {
		obj.innerText = str;
	}
}

function numericInput( event, inputObj ) {
	if ( (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 13 && event.keyCode != 9 ) {
		event.returnValue = false;
		return false;
	}
	event.returnValue = true;
	return true;
}

function makeFixLenDigit( num, digitCnt ) {
	var str = "" + num;
	var len = str.length;
	for ( var i = len; i < digitCnt; i++ ) {
		str = "0" + str;
	}
	return str;
}

function searchChg( obj, name ) {
	var searchObj = obj.parentNode;
	if ( name == "select" ) {
		searchObj.className = "bgWrapper_select";
	} else {
		searchObj.className = "bgWrapper";
	}
}

function countBytes( str ) {
	var count = 0;
	for ( var i = 0; i < str.length; i++ ) {
		var onechar = str.charAt( i );
		if ( str.charCodeAt( i ) >= 128 ) {
			count += 2;
		} else if ( onechar != '\r' ) {
			count++;
		}
	}
	return count;
}

function cutByLen( str, byteLen ) {
	var tmpStr = str;
	while (countBytes( tmpStr ) > byteLen) {
		var len = tmpStr.length;
		tmpStr = tmpStr.substr( 0, len - 1 );
	}
	return tmpStr;
}

function toggleCheck( el ) {
	if ( el.className.indexOf( 'off' ) < 0 ) {
		el.className = el.className.replace( ' on', ' off' );
		return false;
	} else {
		el.className = el.className.replace( ' off', ' on' );
		return true;
	}
}

function _NateSearch()	 {
	if ( document._frmNateSearch.q.value == "통합검색" ) document._frmNateSearch.q.value = "";
	document._frmNateSearch.action = "http://search.nate.com/search/all.html"; 
	document._frmNateSearch.target="_new";
	document._frmNateSearch.submit();	
}

function _clearSearchText(oObject)	{
	oObject.value="";
}

function _setSearchText(oObject)	{
	if(oObject.value == '')
		oObject.value="통합검색";
}


function _checkEnter(oInput, e)	{
	if(e.keyCode == 13)	{
		return true;
	}
	else
		return false;
}

function getEventSrc( event ) {
	var src = event.target;
	if ( src == null ) {
		src = event.srcElement;
	}
	return src;
}
