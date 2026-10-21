    function onlyPhone()
	{
		var keyCode = event.keyCode ? event.keyCode : 
		event.which ? event.which : event.charCode;
		if((keyCode>47 && keyCode<58) || (keyCode==45)) {
			event.returnValue = true;
			} else {
			event.returnValue = false;
			}
	}

    //비주얼폰 글 쓸때 초기화
	function SelText()
	{
		if (document.Form1.txtMessage.value == INPUT_PHONEMSG_MSG)
		{
		    document.Form1.txtMessage.value = "";
		}
		document.Form1.txtMessage.msglen = 0;
	}

	//비주얼 폰 글사이즈 구하기
	function checklen()
	{
		var msgtext, msglen;
		
		msgtext = document.Form1.txtMessage.value;
		msglen = document.Form1.msglen.value;
		
		var i=0,l=0;
		var temp,lastl;
		
		//길이를 구한다.
		while(i < msgtext.length)
		{
			temp = msgtext.charAt(i);
			
			if (escape(temp).length > 4)
				l+=2;
			else if (temp!='\r')
				l++;

			if (temp=='\r' && l>90) //			if (temp=='\r' && l>400)
			{
				document.Form1.txtMessage.value = document.Form1.txtMessage.value.substr(0,i);
			}

			// OverFlow
			if(l>80)
			{
				alert(LIMIT_PHONEMSG_MSG);
				temp = document.Form1.txtMessage.value.substr(0,i);
				document.Form1.txtMessage.value = temp;
				l = lastl;
				i = msgtext.length;
				Form1.msglen.value=l;
				document.Form1.phone123.focus();
				return;
				break;
			}
			lastl = l;
			i++;
		}
		Form1.msglen.value=l;
	}

	//"특수문자" 열기
	function Open_Emoticon(){

		//var ifr = document.all["tframe"];
		var Emoticon = document.all["Emoticon"];

		if (Emoticon.style.display == "")
		{
			Close_Emoticon();
		}		
		else {

			//ifr.style.posLeft = window.posLayerSMS.offsetLeft;
			//ifr.style.posTop = window.posLayerSMS.offsetTop+130;
			IDX_DIV++;
			//ifr.style.zIndex = IDX_DIV;
			//ifr.style.visibility = "visible";
			//document.Form1.ExList.style.visibility			= "hidden"; 
			Emoticon.style.posLeft = window.posLayerSMS.offsetLeft-20;
			Emoticon.style.posTop = window.posLayerSMS.offsetTop+100;
			IDX_DIV++;
			Emoticon.style.zIndex = IDX_DIV;
			Emoticon.style.display = "";
		}
	}

	//"특수문자" 닫기
	function Close_Emoticon(){

		//var ifr = document.all["tframe"];
		var Emoticon = document.all["Emoticon"];
		//document.Form1.ExList.style.visibility			= "visible"; 
		Emoticon.style.display = "none";
		//ifr.style.visibility = "hidden";
	}


	//이모티콘,특수문자에서 비주얼폰으로 옮기기
	function AddChar (ch)
	{
		var t;
		var msglen;
		var tempstr;
		msglen = 0;

//		if (document.Form1.txtMessage.value == INPUT_PHONEMSG_MSG) //메시지는 여기에 입력해주세요..초기세팅메시지인경우
//		{
//				document.Form1.txtMessage.value = "";
//				document.Form1.txtMessage.msglen = 0;
//        }

		document.Form1.txtMessage.value = document.Form1.txtMessage.value + ch;
		tempstr = document.Form1.txtMessage.value;
		l = document.Form1.txtMessage.value.length;
		for(k=0;k<l;k++){
		t = document.Form1.txtMessage.value.charAt(k);
		if (escape(t).length > 4)
			msglen += 2;
		else
			msglen++;
		}
		
		document.Form1.msglen.value = msglen;
	}


	//비주얼폰에서 예약전송 시간 입력폼 노출여부 조절
	function reserveCHK()
	{	    	
		if(document.Form1.calltype.checked)
		{
		    document.all["ReserveBody"].style.display = "";
			//url = "../../Pages/Common/Calendar/CalendarDialog.aspx?func=GetSMSDate&startDate=";
		    //window.open(url,"SMSCalendar","toolbar=no,width=330,height=190,directories=no,status=no,scrollbars=no,resize=no,menubar=no");
		    
		    var url = "/DeskPlusEIP/Portlet/Package/SMS/PopCalendar.aspx";		    
		    var WindowObjectReference = PopupWinOpen(url, "", "reserveCHK", 300, 220);
		}
		
		
		/*
		if(document.Form1.calltype.checked){

			Open_reserveCHK();

		}
		else{
			document.all["calcal"].style.display = "none";	
			if(document.all["ReserveBody"] != undefined){
				document.all["ReserveBody"].style.display = "none";
			}

			document.Form1.yy.value = "";
			document.Form1.mm.value = "";
			document.Form1.dd.value = "";
			document.Form1.h.value = "";
			document.Form1.m.value = "";

			document.Form1.h.readOnly = true;
			document.Form1.m.readOnly = true;
		}
		*/
    }

    function SMSSelCalendar(arg) {
        
        var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/SendSMS.asmx/GetCulDate";
                
        ParamArray = {
            'dateVal': arg
        };

        var obj = eval("SuccessSMSSelCalendar");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);

    }

    //==================================================================    
    // 선택한 날짜 , 년월일 input 에 넣어주기
    //==================================================================
    function SuccessSMSSelCalendar(data, status) {
        var res = data.d;
        if (res != "") {
            var resStr = res.split(",");

            document.Form1.yy.value = resStr[0];
            document.Form1.mm.value = resStr[1];
            document.Form1.dd.value = resStr[2];
            document.Form1.h.value = headerfooter_time_hour;
            document.Form1.m.value = eval(headerfooter_time_minute);
        }
    }	


	//무조건 예약전송 폼 열기
	function Open_reserveCHK(){
		if(document.all["calcal"].style.display != ""){

			if(!document.Form1.calltype.checked){
				document.Form1.calltype.checked = true;
			}

			document.all["calcal"].style.posLeft = window.posLayerSMS.offsetLeft-170;
			document.all["calcal"].style.posTop = window.posLayerSMS.offsetTop+360;

			if(document.all["ReserveBody"] != undefined){
				document.all["ReserveBody"].style.display = "";
			}
				
			IDX_DIV++;
			document.all["calcal"].style.zIndex = IDX_DIV;
			document.all["calcal"].style.display = "";	
				
			document.Form1.yy.value = headerfooter_time_year;
			if (headerfooter_time_month >=10){
				document.Form1.mm.value = headerfooter_time_month; }
			else{
				document.Form1.mm.value = "0"+headerfooter_time_month; }

			if (headerfooter_time_day >=10){
				document.Form1.dd.value = headerfooter_time_day; }
			else{
				document.Form1.dd.value = "0"+headerfooter_time_day; }

			if (headerfooter_time_hour >=10)
				document.Form1.h.value = headerfooter_time_hour;
			else
				document.Form1.h.value = "0"+headerfooter_time_hour;

			if (headerfooter_time_minute >=10)
				document.Form1.m.value = headerfooter_time_minute;
			else
				document.Form1.m.value = "0"+headerfooter_time_minute;

			document.Form1.h.readOnly = false;
			document.Form1.m.readOnly = false;
		}

	}


	//무조건 예약전송 폼 닫기
	function Close_reserveCHK(){
	
		if(document.all["calcal"].style.display == ""){

			document.Form1.calltype.checked = false;
			
			document.all["calcal"].style.display = "none";	
			if(document.all["ReserveBody"] != undefined){
				document.all["ReserveBody"].style.display = "none";
			}

			document.Form1.yy.value = "";
			document.Form1.mm.value = "";
			document.Form1.dd.value = "";
			document.Form1.h.value = "";
			document.Form1.m.value = "";

			document.Form1.h.readOnly = true;
			document.Form1.m.readOnly = true;
		}
	}

	//수신번호 체크
	//비주얼폰에서 그룹전송select박스가 있는경우
	//비주얼폰내 박스 또는 그룹전송 폼 선택하기
	//-비주얼폰폼: isEnterVi()-> phoneCHK()-> AddCheck()..del_hipen()
	//-그룹폼: isEnterVi()-> phoneCHK()-> checksum()
	function phoneCHK(){

		var f = document.Form1;
		var Num = '0123456789';
		
		var temp;
		var str;
		var str2;
			
		str = f.phone123.value;

		if(!str){			
			return;
		}
		
	   for(i=0;i<f.phone123.value.length;i++){
		   temp = f.phone123.value.substring(i,i+1);

		   if(Num.indexOf(temp)<0) str = str.replace(temp,"");
		   
	   }

		if (str.length<10)
		{
			alert(WRONG_INPUT_PHONENUM_MSG);  //수신번호잘못입력
			f.phone123.value=str;
			f.phone123.focus();
			return;
		}

		if(str.length==12 && str.substring(0,4)=="0130"){
			f.phone123.value =str.substring(0,12);						
		}
		else if(str.length>12){ //-- 길이가 13자리 이상인 경우
			f.phone123.value =str.substring(0,11);
		}
		else{
			f.phone123.value =str;
		}							

		if( f.ExList.options.length > 1000 ) {
			if(confirm(LIMIT_PHONENUM_MSG)){ //한번에 전송할 수 있는 수신번호의 수: 1000개
				window.open('/phonebook/pop/main.asp','phonebookpop','width=670,height=550,top=0,left=200');
			}
			return;
		}
		else{
			AddCheck();
			f.phone123.focus();
			return;				
		}
	}


	//비주얼폰 내의 수신번호 들어가는 select 박스로 옮기기
	function AddCheck(){

		var selobj = document.Form1.ExList;//listbox
		var cnt = document.Form1.sendlen.value;//사람수
	
		if (document.Form1.phone123.value)
		{
		    //수신번호 입력값
			var chk_phone123 = del_hipen(document.Form1.phone123.value);
			for(var i=0;i<selobj.options.length;i++)
			{			    
				if(selobj.options[i].value == chk_phone123){
					chk_phone123 = "";
					i = selobj.options.length;
				}
			}
			
			if(chk_phone123 != ""){
				var selvalue = del_hipen(document.Form1.phone123.value);
				var selview = document.Form1.phone123.value; //수신번호 입력값				

				selobj.options[selobj.options.length] = new Option(selview,selvalue);	

				cnt = parseInt(cnt)+1;
			}
		}

		document.Form1.sendlen.value = parseInt(cnt);
		document.Form1.phone123.value='';	
	}


	// 동보전송시 select box에 전화번호 삭제
	//-비주얼폰폼삭제: DelCheck2()-> arrangeList()
	function DelCheck(){
		var val;
		var cnt =1;

		if(document.Form1.ExList.options.length==0) {
			alert(NoDeleteNumber_MSG);
			return;
		}

		if(document.Form1.ExList.selectedIndex==-1) {
			if(document.Form1.ExList.options[document.Form1.ExList.options.length-1].value.substring(0,1) == "/"){
				val = document.Form1.ExList.options[document.Form1.ExList.options.length-1].value;
				cnt = val.split(":(:")[1].replace(":):","");
			}
			//첫번째부터 삭제한다.
			document.Form1.ExList.options[document.Form1.ExList.options.length-1] = null;

		} else {
			Selindex= document.Form1.ExList.selectedIndex;

			if(document.Form1.ExList.options[Selindex].value.substring(0,1) == "/"){
				val = document.Form1.ExList.options[Selindex].value;
				cnt = val.split(":(:")[1].replace(":):","");
			}

			//선택번호를 리스트에서 삭제한다.
			document.Form1.ExList.options[Selindex] = null;
		}
			
			document.Form1.sendlen.value = parseInt(document.Form1.sendlen.value)+(parseInt(cnt)*-1);
		//	arrangeList();	
	}


	// 하이픈 삭제
	function del_hipen(str) {
		var len = str.length;
		var val;
		val = str.replace("-", "");
		val = str.replace(" ", "");
		for(var i=0 ; i < len ; i++) {
			val = val.replace("-", "");
			val = val.replace(" ", "");
		}
		return val;
	}

	//주소록 새창 띄우기
	function OpenPhoneBook(){
		window.open('sms_open.htm','phonebookpop','width=670,height=550,top=0,left=200');
	}
	

	//회신 주소록 새창 띄우기
	function OpenReplyPhoneBook(){
		window.open('../SMS/ReplyNumberSMS.aspx','sms_replynumber','width=500,height=400,top=0,left=200');
	}

	//폴더 열기
	function folderContentOpen() {
	    $("#ifrmSMSSend", parent.document).css("height", 530);
	    $("#divFolderOpen").css("display", "none");
	    $("#divFolderClose").css("display", "");
	    $("#folderContent").css("display", "");
	}


	//폴더 닫기
	function folderContentClose() {
	    $("#ifrmSMSSend", parent.document).css("height", 350);
	    $("#divFolderOpen").css("display", "");
	    $("#divFolderClose").css("display", "none");
	    $("#folderContent").css("display", "none");
		
        //document.all["divFolderOpen"].style.display = "";
		//document.all["divFolderClose"].style.display = "none";
		//document.all["folderContent"].style.display = "none";
	}
	
	
	//재입력
	function Formreset(){

		var f = document.Form1;
		f.txtMessage.value = "";
		f.msglen.value = "0";
	    f.txtMessage.focus();
	}
	
	
	/**********************************************************************************************
		[주소록 추가]
	**********************************************************************************************/
	function OpenAddressBook()
	{
		ranNum = Math.random();
		ranNum = ranNum.toString();
		ranNum = ranNum.substring(2,ranNum.length);

		window.open("AddressBook.aspx?ran=" + ranNum, "OpenAddressBook", "toolbar=no,width=800,height=580,directories=no,status=no,scrollbars=no,resize=no,menubar=no");

		
    }

    //==================================================================    
    // GetSMSUser : SMS 전송할 사용자 정보 가져오기
    //==================================================================
    function GetSMSUser(p_user) {        

        var rtnVal = p_user
        var selobj = document.Form1.ExList; //listbox
        var cnt = document.Form1.sendlen.value; //사람수

        if (rtnVal) {
            if (rtnVal.indexOf("$") > -1) {
                var wholeSplit = rtnVal.split("$");
                for (var i = 0; i < wholeSplit.length; i++) {
                    //M|23|이름|핸펀번호
                    var valueSplit = wholeSplit[i].split("|");
                    var phoneNumber = valueSplit[1];
                    var phoneName = valueSplit[0];                    

                    if (phoneNumber != "" && phoneNumber != undefined) {
                        //수신번호 입력값
                        var chk_phone123 = del_hipen(phoneNumber);
                        for (var j = 0; j < selobj.options.length; j++) {
                            if (selobj.options[j].value == chk_phone123) {
                                chk_phone123 = "";
                                j = selobj.options.length;
                            }
                        }

                        if (chk_phone123 != "") {
                            var selvalue = chk_phone123;
                            var selview = phoneName + ":" + chk_phone123; //수신번호 입력값				

                            selobj.options[selobj.options.length] = new Option(selview, selvalue);

                            cnt = parseInt(cnt) + 1;
                        }
                    }
                }
            }

            document.Form1.sendlen.value = parseInt(cnt);
        }
    }		
			
		
	//==================================================================
    //						
    // SendSMS() : 전송
    //==================================================================

	function SendSMS(){
		var i,j;				

		var calltype_val = 0;
		var str;	
		var temp;
		var Num = '0123456789';

		if(navigator.appVersion.indexOf("MSIE 5") != -1 || navigator.appVersion.indexOf("MSIE 6") != -1){
			
			if(document.all("txtMessage").value.length == 0 || document.all("txtMessage").value == INPUT_PHONEMSG_MSG)
			{
				alert(InputSendMsg);
				SelText();
				document.all("txtMessage").focus();
				return ;
			}
		}
		
		if(document.all("msglen").value == "0")
		{
		    alert(InputSendMsg);
		    document.all("txtMessage").focus();		    
		    return;
		}

		//수신번호 체크
		if(document.all("sendlen").value<1){
			alert(InputReceiveNumber);
			document.all("phone123").focus();
			return;
		}


		//회신번호 정리
		var req123 = document.all("req123").value;
		if(req123){			
			for(i=0;i<req123.length;i++){
			   temp = req123.substring(i,i+1);
			   if(Num.indexOf(temp)<0) req123 = req123.replace(temp,"");
			}

			document.all("req123").value=req123;
	    }

		//예약시간 처리
		if(document.all("calltype").checked==true){

			calltype_val = "1"; //예약용

			if (!document.all("yy").value || !document.all("mm").value || !document.all("dd").value)
			{
				alert(SelectReservationDate);
				return;
			}

			if (!document.all("h").value)
			{
				alert(SelectReservationTime);
				document.all("h").focus();
				return;
			}

			if (!document.all("m").value)
			{
				alert(SelectReservationTime);
				document.all("m").focus();
				return;
			}

			//예약시간체크- 현재이후이어야
			if(document.all("yy").value == headerfooter_time_year)
			{
				if( Number(document.all("mm").value) < headerfooter_time_month ){

					alert(ConfirmReservatonMonth);

					document.all("mm").value = headerfooter_time_month;
					return;
				}
				else if( document.all("mm").value == headerfooter_time_month && ( Number(document.all("dd").value) < headerfooter_time_day )){
					alert(ConfirmReservatonDay);

					document.all("dd").value = headerfooter_time_day;
					return;	
				}
			}

			//시간체크
			str = document.all("h").value;
			for(i=0;i<document.all("h").value.length;i++){
			   if(Num.indexOf(document.all("h").value.substring(i,i+1))<0) str = str.replace(document.all("h").value.substring(i,i+1),"");
			}
			if (str.length==1) str = "0"+str;
			document.all("h").value = str;
			if(document.all("h").value < "00" || document.all("h").value > "23") {
				alert(InputCurrectTime0023);
				document.all("h").value = "00";
				document.all("h").focus();
				return ;
			}

			//분체크
			str = document.all("m").value;
			for(i=0;i<document.all("m").value.length;i++){
			   if(Num.indexOf(document.all("m").value.substring(i,i+1))<0) str = str.replace(document.all("m").value.substring(i,i+1),"");
			}
			if (str.length==1) str = "0"+str;
			document.all("m").value = str;
			if(document.all("m").value < "00" || document.all("m").value > "59") {
				alert(InputCurrectTime0059);
				document.all("m").value = "00";
				document.all("m").focus();
				return ;
			}
		}//예약전송 체크 완료
		else{
			calltype_val = "0";
		}

        $("#calltype").val(calltype_val)

		if(confirm(ShallWeSendSMS))
		{
		    document.frmSMSProc.iMsg.value = escape($("#txtMessage").val()); //메세지
		    var selobj = document.Form1.ExList;//listbox
		    var receiveNumber = ""; //
		    for(var j=0;j<selobj.options.length;j++)
            {
                if(selobj.options[j].value != "")
                {
                    receiveNumber += del_hipen(selobj.options[j].value) + ",";
                }
            }           
            
            document.frmSMSProc.receiveNumber.value = receiveNumber;
            document.frmSMSProc.replyNumber.value = $("#req123").val(); //회신번호
            //calltype_val ; 0:즉시, 1:예약
            
            //document.all("dept123").value; //회사선택 이름(SMS 요금땜시)
            //회사하나로 계정을 가질 경우 빈값 처리!!
            document.frmSMSProc.deptName.value = "";            

            document.frmSMSProc.yyyymmdd.value = $("#yy").val() + "-" + $("#mm").val() + "-" + $("#dd").val(); //년월일
            document.frmSMSProc.hms.value = $("#h").val() + $("#m").val() + "10";
            document.frmSMSProc.calltype_val.value = calltype_val;
            document.frmSMSProc.companyID.value = CompanyID;
            document.frmSMSProc.userID.value = UserID;
            
            document.frmSMSProc.submit();
            
            //회사아이디$사용자아이디$메세지$수신번호$회신번호$예약일자$예약시각$예약여부            
            
            //var rtn = AjaxSendSMS(CompanyID,UserID,iMsg,receiveNumber,replyNumber,yyyymmdd,hms,calltype_val);
            
            alert(SMS_WriteSMS_MSG_SUCESS);           

		}else{
			return;
		}	
	}
	
	
	//==================================================================    
    // Save_Content() : 문자저장
    //==================================================================
	function Save_Content()
	{
	    var content = $("#txtMessage").val();	    
	    var confirmAlert = "[" + content + "] " + ShallWeInputSave;
	    
	    if (confirm(confirmAlert)) {

	        var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/SendSMS.asmx/SaveContent";

	        //파라미터값 배열에 담기
	        //auth 관리자페이지 : 1; 일반인페이지 : 0
	        ParamArray = {
	            'id': 0,
	            'gubun': 'insert',
	            'content': escape(content)
	        };

	        var obj = eval("Save_Content_Success");
	        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
	        data = "{p:'" + GetEncryptValue + "'}";
	        AjaxService(url, data, obj);
	    }
	}

	//==================================================================    
	// Save_Content() : 문자저장 완료
	//==================================================================
	function Save_Content_Success(data, status) {
	    if (data.d == true) {
	        alert(SMS_SMSConfig_Success);
	        document.getElementById("ifrm_openContent").src = "OpenContentSMS.aspx";
	    }
	}
	
	
	//==================================================================
    //						
    // 메신저에서 넘기는 전화번호
    //==================================================================
	function AddMessengerCheck(){

		var selobj = document.Form1.ExList;//listbox
		var cnt = document.Form1.sendlen.value;//사람수
		
	
		if (MessengerSendNumber != "")
		{	
			var messengerSendNumberStr = MessengerSendNumber.split(";");
			
			for(var k=0;k<messengerSendNumberStr.length;k++)
			{
			    var chk_phone123 = del_hipen(messengerSendNumberStr[k]);
			    
			    for(var i=0;i<selobj.options.length;i++)
			    {			    
				    if(selobj.options[i].value == chk_phone123){
					    chk_phone123 = "";
					    i = selobj.options.length;
				    }
			    }
    			
			    if(chk_phone123 != ""){
				    var selvalue = del_hipen(chk_phone123);
				    var selview = chk_phone123; //수신번호 입력값				

				    selobj.options[selobj.options.length] = new Option(selview,selvalue);	

				    cnt = parseInt(cnt)+1;
			    }
			}
		}

		document.Form1.sendlen.value = parseInt(cnt);
	}