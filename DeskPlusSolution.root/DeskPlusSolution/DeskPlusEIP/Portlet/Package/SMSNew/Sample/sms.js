var addrCellSize = 20;

/**
 * 입력란에 있는 모든 번호 초기화
 */
function clearReceiveNumber() {
	for ( var i = addrCellSize - 1; i >= 0; i-- ) {
		deleteReceiveNum( i );
	}
}

/**
 * 입력셀 삭제
 */
function deleteReceiveNum( idx ) {
	if ( idx < 0 || idx >= addrCellSize ) {
		alert( 'deleteReceiveNum() invalid index ' + idx );
		return;
	}

	for ( var i = idx; i < addrCellSize; i++ ) {
		var objPhone = document.getElementById( "cell" + i );
		var objName = document.getElementById( "ncell" + i );
		var objPhoneNext = document.getElementById( "cell" + (i + 1) );
		var objNameNext = document.getElementById( "ncell" + (i + 1) );

		if ( objPhoneNext != null ) {
			objPhone.value = objPhoneNext.value;
			setText( objName, objNameNext.firstChild.nodeValue );
			objName.title = objNameNext.firstChild.nodeValue;

			if ( objPhone.value == "" ) {
				setLayerDisplay( "rcpt" + i, false );
			}
		} else {
			objPhone.value = "";
			setText( objName, "" );
			objName.title = "";
			setLayerDisplay( "rcpt" + i, false );
		}
	}

	var newCnt = countRcpt();
	if ( newCnt < addrCellSize ) {
		setLayerDisplay( "rcptDesc", true );
	} else {
		setLayerDisplay( "rcptDesc", false );
	}
}

/**
 * 다음 수신번호 입력창으로
 */
function moveNext( inputObj ) {
	var no = parseInt( inputObj.id.substr( 4 ), 10 );
	var nextNo = no + 1;
	if ( nextNo > addrCellSize - 1 ) {
		nextNo = addrCellSize - 1;
	}

	var nextObj = document.getElementById( "cell" + nextNo );
	if ( nextObj != null ) {
		if ( nextObj.value == "" ) {
			var maxIdx = countRcpt();
			if ( nextNo == maxIdx ) {
				rcptNew();
				document.getElementById( "cell" + nextNo ).focus();
			}
		} else {
			document.getElementById( "cell" + nextNo ).focus();
		}
	}
}

/**
 * 다음 수신번호 입력창으로
 */
function moveNext2( inputObj ) {
	var no = parseInt( inputObj.id.substr( 4 ), 10 );
	var nextNo = no + 1;
	if ( nextNo > addrCellSize - 1 ) {
		nextNo = addrCellSize - 1;
	}

	var nextObj = document.getElementById( "cell" + nextNo );
	if ( nextObj != null ) {
		if ( nextObj.value == "" ) {
			var maxIdx = countRcpt();
			if ( nextNo == maxIdx ) {
				rcptNew();
				document.getElementById( "cell" + nextNo ).focus();
			}
		} else {
			document.getElementById( "cell" + nextNo ).focus();
		}
	}
}

/**
 * 이전 수신번호 입력창으로
 */
function movePrev( inputObj ) {
	var no = parseInt( inputObj.id.substr( 4 ), 10 );
	var nextNo = no - 1;
	if ( nextNo < 0 ) {
		nextNo = 0;
	}

	var prevObj = document.getElementById( "cell" + nextNo );
	if ( prevObj != null ) {
		prevObj.focus();
	}
}

/**
 * 이전 수신번호 입력창으로
 */
function movePrev2( inputObj ) {
	var no = parseInt( inputObj.id.substr( 4 ), 10 );
	var nextNo = no - 1;
	if ( nextNo < 0 ) {
		nextNo = 0;
	}

	var prevObj = document.getElementById( "cell" + nextNo );
	if ( prevObj != null ) {
		prevObj.focus();
	}
}

var blurCheckFlag = true;
/**
 * 방향키 등이 호출될 경우 호출
 */
function checkInputChar( event, inputObj ) {
	var keyCode = event.keyCode;

	if ( keyCode == 38 ) { // Up-arrow
		/* 위쪽 수신번호로 */
		movePrev2( inputObj );
		return false;
	} else if ( keyCode == 40 ) { // Down-arrow
		/* 아래쪽 수신번호로 */
		moveNext2( inputObj );
		return false;
	} else if ( keyCode == 13 ) {
		var rslt = checkRcptValue( inputObj );
		if ( rslt == 0 ) {
			moveNext2( inputObj );
		} else {
			blurCheckFlag = false;
			if ( rslt == 1 ) {
				alert( "번호 형식이 잘못되었습니다." );
			} else {
				alert( "수신번호 중복입니다" );
			}
			blurCheckFlag = true;
			inputObj.focus();
		}
		return false;
	} else {
		return true;
	}
}

/**
 * @param inputObj
 * @return 0: OK
 *            1: Invalid phone number
 *            2: Duplicated phone number
 */
function checkRcptValue( inputObj ) {
	if ( inputObj != null ) {
		var str = trim( removeHipen( inputObj.value ) );
		var no = parseInt( inputObj.id.substr( 4 ), 10 );
		var nameObj = document.getElementById( "ncell" + no );

		var recipient = addHipen( str, false );
		if ( recipient == "" ) {
			return 1;
		} else {
			inputObj.value = recipient;
			if ( removeDupRecpient2( false, false ) ) {
				return 2;
			}
		}
	}
	return 0;
}


function countRcpt() {
	var i = 0;
	for ( i = 0; i < addrCellSize; i++ ) {
		var numObj = document.getElementById( "cell" + i );
		if ( numObj.value == "" ) {
			return i;
		}
	}
	return addrCellSize;
}

function rcptNew() {
	var newCnt = countRcpt();
	setLayerDisplay( "rcpt" + newCnt, true );
	setLayerDisplay( "rcptDesc", true );
	document.getElementById( "cell" + newCnt ).focus();
}

function onRcptClick( event ) {
	var src = event.target;
	if ( src == null ) src = event.srcElement;
	if ( src.id == "rcpt_new_name" || src.id == "rcpt_new_number" || src.id == "rcptUl") {
		rcptNew();
	}
}

/**
 * 수신번호 공간에 전화번호를 추가한다.
 */
function add( number, name ) {
	var i;
	if ( name == null ) {
		name = "";
	}

	if ( number == null || number == "" ) {
		return false;
	}

	for ( i = 0; i < addrCellSize; i++ ) {
		var objPhone = document.getElementById( "cell" + i );
		var objName = document.getElementById( "ncell" + i );
		if ( objPhone.value == "" ) {
			/* Empty slot. insert it here. */
			objPhone.value = number;
			setText( objName, name );
			objName.title = name;
			orgNumber = null;
			if ( pack( objPhone ) ) {
				setLayerDisplay( "rcpt" + i, true );
			}
			return true;
		}
	}

	alert( "더이상 수신번호를 추가할 수 없습니다" );
	return false;
}

var orgNumber = null;

/**
 * 입력하려고 포커스를 받았을 경우 호출
 */
function onEdit( event, inputObj ) {
	orgNumber = removeHipen( inputObj.value );
	inputObj.value = orgNumber;
	return false;
}

function onEditBlur( event, inputObj ) {
	if ( blurCheckFlag ) {
		pack( inputObj );
	}
}

function pack( inputObj ) {
	var result = true;
	if ( inputObj != null ) {
		var str = trim( removeHipen( inputObj.value ) );
		var no = parseInt( inputObj.id.substr( 4 ), 10 );
		var nameObj = document.getElementById( "ncell" + no );

		if ( str == "" ) {
			deleteReceiveNum( no );
		} else {
			if ( orgNumber != null && str != orgNumber ) {
				if ( nameObj != null ) {
					setText( nameObj, "" );
					nameObj.title = "";
				}
			}

			var recipient = addHipen( str, true );
			if ( recipient == "" ) {
				deleteReceiveNum( no );
				return false;
			} else {
				// Firefox버그 때문에 땜빵-inputObj.value = recipient;
				document.getElementById( inputObj.id ).value = recipient;
				result = !removeDupRecpient( true );
			}
		}
	} else {
		result = false;
	}

	var newCnt = countRcpt();
	if ( newCnt < addrCellSize ) {
		setLayerDisplay( "rcptDesc", true );
	} else {
		setLayerDisplay( "rcptDesc", false );
	}
	
	return result;
}

/**
 * 중복된 전화번호를 제거한다.
 */
function removeDupRecpient( warn ) {
	removeDupRecpient2( true, warn );
}

function removeDupRecpient2( delDuplicated, warn ) {
	var i, j, num, numj, duplicated;
	duplicated = false;
	for ( i = 1; i < addrCellSize; i++ ) {
		num = document.getElementById( "cell" + i ).value;
		if ( num == "" )
			continue;

		for ( j = 0; j < i; j++ ) {
			numj = document.getElementById( "cell" + j ).value;
			if ( num == numj ) {
				if ( delDuplicated ) {
					deleteReceiveNum( i );
					i--;
				}
				duplicated = true;
				break;
			}
		}
	}

	if ( duplicated && warn ) {
		alert( "수신번호 중복입니다" );
	}
	return duplicated;
}

function onSendNumFocus( event, obj ) {
	obj.value = removeHipen( obj.value );
}

function onSendNumBlur( event, obj ) {
	var num = obj.value;
	var newNum = "";
	var i = 0;
	for ( i = 0; i < num.length; i++ ) {
		var ch = num.charAt( i );
		if ( ch >= '0' && ch <= '9' ) {
			newNum = newNum + ch;
		}
	}
	var hipened = addHipen( newNum, false );
	if ( hipened == "" ) {
		obj.value = newNum;
	} else {
		obj.value = hipened;
	}
}

function sendNumKey( event, obj ) {
	if ( event.keyCode == 13 ) {
		obj.blur();
		return false;
	}
	return true;
}

function quickSearch() {
	var query = trim( document.getElementById( "quickSearch" ).value );
	if ( query == "빠른검색 (이름 또는 번호)" || query == "" ) {
		alert( "검색어를 입력해 주세요." );
		return false;
	}

	removeAllChildren( document.getElementById( "quick_search_result" ) );
	setLayerDisplay( "search-list", true );
	setLayerDisplay( "search-list-none", false );

	var nextUrl = "/web2009/html/quickSearch.jsp";
	var sParam = "&txtSearchTp=4" + "&txtSearch_Value=" + query;

	sendRequest( rspQuickSearch, sParam, 'GET', nextUrl, true, true );
}

function rspQuickSearch( oj ) {
	if ( oj.readyState != 4 || oj.status != 200 ) {
		setText( document.getElementById( "search-list-none-msg" ), "주소록 조회 오류" );
		setLayerDisplay( "search-list", false );
		setLayerDisplay( "search-list-none", true );
		return;
	}

	var res = decodeURIComponent( oj.responseText );
	var rows = res.split( "\n" );
	var cols = '';
	var i = 0; // while loop index

	var isExistAddr = false; // 조회한 주소록이 있는가?
	var tempRoot = document.getElementById( "quick_search_result" );

	for ( i = 0; i < rows.length; i++ ) {
		if ( rows[i] != "\r" ) {
			cols = rows[i].split( "," );

			if ( cols[0] != "" ) {
				isExistAddr = true;

				var newName = document.createElement( "span" );
				newName.appendChild( document.createTextNode( cols[0] ) );
				newName.setAttribute( "title", cols[0] );
				
				var newNum = document.createElement( "input" );
				newNum.value = cols[1];
				newNum.className = "text";

				var tempElement = document.createElement( "li" );
				tempElement.appendChild( newName );
				tempElement.appendChild( newNum );

				addEvent( tempElement, "click", selectQuickSearchRecord );

				tempRoot.appendChild( tempElement );
			}
		}
	}

	if ( !isExistAddr ) {
		var msgObj = document.getElementById( "search-list-none-msg" );
		removeAllChildren( msgObj );
		msgObj.appendChild( document.createTextNode( "입력 정보와 일치하는" ) );
		msgObj.appendChild( document.createElement( "br" ) );
		msgObj.appendChild( document.createTextNode( "정보가 없습니다." ) );
		setLayerDisplay( "search-list", false );
		setLayerDisplay( "search-list-none", true );
	}
}

/**
 * 해당 항목을 더블 클릭한 경우, 정해진 곳에 주소록 필드에 해당 내용을 보여준 후, 레이어를 토글시킨다.
 */
function selectQuickSearchRecord( ev ) {
	var selected = null;

	if ( window.event ) {
		selected = event.srcElement;
	} else {
		selected = ev.target;
	}

	if ( selected.nodeName != "LI" ) {
		selected = selected.parentNode;
	}

	var name = selected.childNodes[0].childNodes[0].nodeValue;
	var num = selected.childNodes[1].value;

	add( removeHipen( num ), name );
	setLayerDisplay( "search-list", false );
}

function onQuickSearchKey( event ) {
	if ( event.keyCode == 13 ) {
		quickSearch();
	}
}

function clearMessage() {
	if ( confirm( "현재 작성중인 메시지 정보를\n모두 초기화 하시겠습니까?\n(1) 입력 메시지 삭제\n(2) 보내는 사람 번호 초기화\n(3) 받는 사람 정보 삭제\n(4) 예약 설정 초기화" ) ) {
		document.getElementById( 'vc_message' ).value = "";
		clearReceiveNumber();
		if ( isReserved() ) {
			toggleReserve();
		}
		document.getElementById( "vc_send_num" ).value = sendNum;
		updateTextLen();
	}
}

/**
 * 주어진 시각이 예약 가능한 시각인지 확인
 * 
 * @param rsvDate
 *          yyyyMMddHHmmss
 * @return 0:예약 가능, 1:10분이내(예약불가), 2:1년 이후(예약불가)
 */
function checkRsvTime( rsv_date ) {
	var rsvDate = new Date( rsv_date.substring( 0, 4 ), rsv_date.substring( 4, 6 ) - 1, rsv_date.substring( 6, 8 ), rsv_date.substring( 8, 10 ), rsv_date.substring( 10, 12 ) );
	var mills = rsvDate.getTime() - 540 * 1000 * 60 - rsvDate.getTimezoneOffset() * 1000 * 60;
	rsvDate = new Date( mills );  // 한국시간을 고려해서 Timezone 을 계산
	
	return checkRsvTime2( rsvDate );
}

function checkRsvTime2( rsvDate ) {
	var limitTime = new Date();
	var MaxLimitTime = new Date( limitTime.getTime() + 60000 * 60 * 24 * 366 );
	limitTime = new Date( limitTime.getTime() + 600000 );

	if ( rsvDate < limitTime ) {
		return 1;
	} else if ( rsvDate > MaxLimitTime ) {
		return 2;
	}

	return 0;
}

var isSending = false;

/**
 * 전송 완료 후 이 함수를 부른다.
 * 
 * @param isOK
 *          true, if sent successfully.
 */
function sendingFinished( isOK ) {
	isSending = false;
	if ( isOK ) {
		// TODO: 전송 완료 후 창 지워줄까?

		// to reload recent-list
		isRecentLoaded = false;
	}
}

function sendMessage() {
	if ( isSending ) {
		alert( "이전에 전송한 메시지가 처리 중입니다." );
		return false;
	}

	isSending = true;	
	var msg = document.getElementById( "vc_message" ).value;

	if ( msg == "" ) {
		alert( "전송하실 메시지를 입력해주세요.(한글기준 40글자)" );
		document.getElementById( "vc_message" ).focus();
		isSending = false;
		return false;
	}

	var notEucKR = getNotEucKR( msg );
	if ( notEucKR != null ) {
		alert( "문자전송시 사용불가능한 문자가\n포함되어 있습니다: " + notEucKR );
		document.getElementById( "vc_message" ).focus();
		isSending = false;
		return false;
	}

	if ( countBytes( msg ) > 80 ) {
		alert( "메시지는 한글기준 40글자를 초과할 수 없습니다." );
		document.getElementById( "vc_message" ).focus();
		isSending = false;
		return false;
	}

	var sendNum = document.getElementById( "vc_send_num" ).value;
	if ( sendNum == "" ) {
		alert( "보내는 사람 번호를 입력해 주세요" );
		document.getElementById( "vc_send_num" ).focus();
		isSending = false;
		return false;
	}

	var recipients = "";
	var rcptNames = "";
	for ( var i = 0; i < addrCellSize; i++ ) {
		var tmp = document.getElementById( "cell" + i ).value;
		var tmpN = document.getElementById( "ncell" + i ).firstChild.nodeValue;
		if ( tmp != null && tmp != "" ) {
			if ( recipients != "" ) {
				recipients = recipients + ",";
				rcptNames = rcptNames + ",";
			}
			recipients = recipients + removeHipen( tmp );
			if ( tmpN == "" )
				tmpN = " ";
			rcptNames = rcptNames + tmpN;
		} else {
			break;
		}
	}

	if ( recipients == "" ) {
		alert( "메시지를 수신할 사람의\n휴대폰 번호를 입력해주세요.\n(최대 20명)" );
		isSending = false;
		return false;
	}

	if ( isReserved() ) {
		var date = getReserveOnceDate();
		var str = "" + getYearFromDate( date ) + makeFixLenDigit( date.getMonth() + 1, 2 ) + makeFixLenDigit( date.getDate(), 2 );
		str = str + makeFixLenDigit( document.getElementById( "once_hour" ).value, 2 );
		str = str + makeFixLenDigit( document.getElementById( "once_minute" ).value, 2 );

		var rslt = checkRsvTime( str );
		if ( rslt == 1 ) {
			window.alert( "현재 시간으로부터 10분 이후의 시간부터 예약 설정이 가능합니다." );
			isSending = false;
			return false;
		} else if ( rslt == 2 ) {
			window.alert( "오늘 날짜로부터 1년 이내의 기간까지만 예약 가능합니다." );
			isSending = false;
			return false;
		}

		document.sendForm.rsvDate.value = str;
	} else {
		document.sendForm.rsvDate.value = "";
	}

	// TODO: Check balance

	document.sendForm.msg.value = msg;
	document.sendForm.sendNum.value = removeHipen( sendNum );
	document.sendForm.recipients.value = recipients;
	document.sendForm.rcptNames.value = rcptNames;

	document.sendForm.submit();
	return true;
}

function isReserved() {
	return (document.getElementById( "rsvChk" ).className.indexOf( "off" ) < 0);
}

var rsvOnceDate = null;

function getReserveOnceDate() {
	return rsvOnceDate;
}

function setReserveOnceDate( date ) {
	rsvOnceDate = date;
	var str = getYearFromDate( date ) + "-" + makeFixLenDigit( date.getMonth() + 1, 2 ) + "-" + makeFixLenDigit( date.getDate(), 2 ) + " (" + dayOfWeek[date.getDay()] + ")";
	document.getElementById( "rsv_date_disp" ).value = str;
}

function toggleReserve() {
	var el = document.getElementById( "rsvChk" );
	if ( isReserved() ) {
		if ( confirm( "예약 전송 설정을 해제하시겠습니까?" ) ) {
			setLayerDisplay( "info", false );
			el.className = el.className.replace( "on", "off" );
			if ( isCalenderOpend ) {
				cancelCalendar();
			}
		}
	} else {
		var now = new Date();
		var nowMills = now.getTime();
		nowMills = nowMills - ( nowMills % 600000 ) + 1200000;		//10분 후
		now = new Date( nowMills );
		setReserveOnceDate( now );
		document.getElementById( "once_hour" ).value = now.getHours();
		document.getElementById( "once_minute" ).value = now.getMinutes();
		el.className = el.className.replace( "off", "on" );
		setLayerDisplay( "info", true );
	}
	return false;
}

function setReserveDate( date, isOK ) {
	if ( isOK ) {
		if ( checkRsvTime3( date ) != 0 ) {
			alert( "오늘 이전 또는 1년 이후 날짜를 선택할 수 없습니다." );
			return false;			
		}
		setReserveOnceDate( date );
	}
	return true;
}

function checkRsvTime3( rsvDate ) {
	var limitTime = new Date( new Date().getTime() - 60000 * 60 * 24 );
	var MaxLimitTime = new Date( limitTime.getTime() + 60000 * 60 * 24 * 366 );

	if ( rsvDate < limitTime ) {
		return 1;
	} else if ( rsvDate > MaxLimitTime ) {
		return 2;
	}

	return 0;
}


function closeReserveLayer() {
	if ( isCalenderOpend ) {
		cancelCalendar();
	}
	hideLayer( "info" );
}

function inputMsg( event ) {
	var count = updateTextLen();
	if ( count > 80 ) {
		var ta = document.getElementById( "vc_message" );
		var cut = cutByLen( ta.value, 80 );
		ta.value = cut;
		alert( "메시지의 길이는 한글 기준 40글자를 초과할 수 없습니다." );
		ta.value = cut;	//IE에서 현재입력키가 한글인 경우, 기존 메시지가 전부 지워지는 경우가 있다.
		clearCurrentPos();
		updateTextLen();
		return false;
	}

	return true;
}

function checkAndSaveCurPos( event ) {
	var result = inputMsg( event );
	saveCurrentPos();
	return result;
}

function updateTextLen() {
	var count = countBytes( document.getElementById( "vc_message" ).value );
	setText( document.getElementById( "txtLen" ), count );
	return count;
}

function saveAlbum() {
	var msg = document.getElementById( "vc_message" ).value;
	var count = countBytes( msg );
	if ( count == 0 ) {
		alert( "메시지 내용을 입력해주세요" );
		return false;
	}

	if ( count > 80 ) {
		alert( "메시지의 길이는 한글 기준 40글자를 초과할 수 없습니다." );
		return false;
	}

	saveAlbumForce( msg, false );
}

function saveAlbumForce( msg, delOld ) {
	var nextUrl = "sms_save_album.jsp";
	msg = encodeURIComponent( msg );
	var sParam = "&msg=" + msg;
	if ( delOld == true ) {
		sParam = sParam + "&del_old=Y";
	}

	sendRequest( rspSaveAlbum, sParam, 'GET', nextUrl, true, true );
}

function rspSaveAlbum( oj ) {
	if ( oj.readyState != 4 || oj.status != 200 ) {
		alert( "앨범저장오류: " + oj.readyState + "/" + oj.status );
		return;
	}

	var res = trim( decodeURIComponent( oj.responseText ) );
	if ( res == "OK" ) {
		alert( "메시지 저장을 완료하였습니다.\n\n'메시지함>문자앨범' 메뉴 또는 우측의 [문자앨범] 탭에서\n현재 저장하신 메시지를 확인하실 수 있습니다." );
	} else if ( res == "KO:OVERFLOW" ) {
		if ( confirm( "메시지 저장 공간이 부족합니다.\n총 100개의 메시지를 저장하실 수 있습니다.\n가장 오래된 메시지부터 삭제한 후\n현재 메시지를 저장하시겠습니까?" ) ) {
			saveAlbumForce( document.getElementById( "vc_message" ).value, true );
		}
	} else {
		alert( "메시지 저장실패:" + res );
	}
}

/**
 * 우측 탭 클릭.
 * 
 * @param id
 *          emoticon, challenge, album
 * @return
 */
function openRightTab( id ) {
	if ( id == "emoticon" ) {
		document.getElementById( "right_emoticon" ).className = "emoticon selected";
		document.getElementById( "right_challenge" ).className = "friend";
		document.getElementById( "right_album" ).className = "ablum";
		ifrm_righttab.location.href = "sms_emoticon.jsp";
	} else if ( id == "challenge" ) {
		document.getElementById( "right_emoticon" ).className = "emoticon";
		document.getElementById( "right_challenge" ).className = "friend selected";
		document.getElementById( "right_album" ).className = "ablum";
		ifrm_righttab.location.href = "sms_challenge.jsp";
	} else if ( id == "album" ) {
		document.getElementById( "right_emoticon" ).className = "emoticon";
		document.getElementById( "right_challenge" ).className = "friend";
		document.getElementById( "right_album" ).className = "ablum selected";
		ifrm_righttab.location.href = "sms_album.jsp";
	} else {
		alert( "openRightTab() : Invalid id" );
	}
}

function setTextContent( text ) {
	document.getElementById( "vc_message" ).value = text;
	updateTextLen();
}

function onClickChar( ch ) {
	insertStr( ch );
	hideLayer( 'layer_sign' );
}

function insertStr( text ) {
	var objTextArea = document.getElementById( "vc_message" );
	if ( objTextArea.createTextRange && objTextArea.currentPos && objTextArea.currentPos != null ) {
		var currentPos = objTextArea.currentPos;
		currentPos.text = currentPos.text.charAt( currentPos.text.length - 1 ) == ' ' ? text + ' ' : text;
	} else {
		objTextArea.value = objTextArea.value + text;
	}
	inputMsg( null );
}

function clearCurrentPos() {
	var objTextArea = document.getElementById( "vc_message" );
	if ( objTextArea.createTextRange ) {
		objTextArea.currentPos = null;
	}
}

function saveCurrentPos() {
	var objTextArea = document.getElementById( "vc_message" );
	if ( objTextArea.createTextRange ) {
		objTextArea.currentPos = document.selection.createRange().duplicate();
	}
}

var isRecentLoaded = false;
/**
 * 최근번호 리스트 오픈한다.
 * 
 * @return
 */
function toggleRecent() {
	var obj = document.getElementById( "layer_recent_number" );
	if ( obj.style.display == "none" ) {
		obj.style.display = "block";

		if ( !isRecentLoaded ) {
			var nextUrl = "/web2009/html/recent_recipient.jsp";
			var sParam = "&cmd=list";
			sendRequest( rspLoadRecent, sParam, "GET", nextUrl, true, true );
		}
	} else {
		obj.style.display = "none";
	}
}

function rspLoadRecent( oj ) {
	if ( oj.readyState != 4 || oj.status != 200 ) {
		alert( "최근 보낸 번호 조회 오류" );
		return;
	}
	parseRecentList( decodeURIComponent( oj.responseText ) );
}

function parseRecentList( res ) {
	var rows = res.split( "\n" );
	var cols = '';
	var i = 0; // while loop index

	isRecentLoaded = true;
	var tempRoot = document.getElementById( "ul_recent" );
	removeAllChildren( tempRoot );

	for ( i = 0; i < rows.length; i++ ) {
		if ( rows[i] != "\r" ) {
			cols = rows[i].split( "\t" );

			if ( cols[0] != "" ) {
				var newName = document.createElement( "span" );
				newName.appendChild( document.createTextNode( cols[1] ) );

				var newNum = document.createElement( "input" );
				newNum.value = addHipen( cols[0] );
				newNum.className = "text";

				var btn = document.createElement( "button" );
				btn.setAttribute( "type", "button" );
				btn.setAttribute( "title", "삭제" );
				btn.className = "delete";
				setText( btn, "삭제" );

				var tempElement = document.createElement( "li" );
				tempElement.id = "recent_li_" + cols[0];
				tempElement.appendChild( newName );
				tempElement.appendChild( newNum );
				tempElement.appendChild( btn );

				addEvent( btn, "click", delRecent );
				addEvent( newNum, "click", selectRecentRecord );
				addEvent( newName, "click", selectRecentRecord );

				tempRoot.appendChild( tempElement );
			}
		}
	}
}

function selectRecentRecord( event ) {
	var selected = null;

	if ( event.target == null ) {
		selected = event.srcElement;
	} else {
		selected = event.target;
	}

	if ( selected.nodeName != "LI" ) {
		selected = selected.parentNode;
	}

	var name = selected.childNodes[0].childNodes[0].nodeValue;
	var num = selected.childNodes[1].value;

	add( removeHipen( num ), name );
}

var deletedRecentLi = null;
function delRecent( event ) {
	var selected = null;

	if ( event.target == null ) {
		selected = event.srcElement;
	} else {
		selected = event.target;
	}

	deletedRecentLi = selected.parentNode;
	var num = deletedRecentLi.id.substring( 10 );

	var nextUrl = "/web2009/html/recent_recipient.jsp";
	var sParam = "&cmd=del&num=" + num;
	sendRequest( rspDelRecent, sParam, "GET", nextUrl, true, true );
}

function rspDelRecent( oj ) {
	if ( oj.readyState != 4 || oj.status != 200 ) {
		alert( "삭제 오류" );
		return;
	}
	deletedRecentLi.parentNode.removeChild( deletedRecentLi );
}

function clearRecent() {
	var nextUrl = "/web2009/html/recent_recipient.jsp";
	var sParam = "&cmd=clear";
	sendRequest( rspClearRecent, sParam, "GET", nextUrl, true, true );
}

function rspClearRecent( oj ) {
	if ( oj.readyState != 4 || oj.status != 200 ) {
		alert( "전체삭제 오류" );
		return;
	}
	removeAllChildren( document.getElementById( "ul_recent" ) );
}

function openAddressBook() {
	var win = window.open( "addressbook.jsp", "address", "width=625,height=475,scrollbar=no,resize=yes" );
	win.focus();
}

function debugLog( str ) {
	document.getElementById( "vc_message" ).value = document.getElementById( "vc_message" ).value + str;
}

function focusRcpt( i ) {
	try {
		document.getElementById( 'cell' + i ).focus();
	} catch( ex ) {
		//do nothing
	}
}