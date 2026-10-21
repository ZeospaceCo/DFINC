	var mymsgNumber = "01333621912" //"01333665600"	//문자사서함
	function Alert_MyMsg(){

			//문자사서함 경고
			if(document.all("mymsg").checked){
				if(!confirm("회신번호를 변경하면 문자사서함기능은 이용하실 수 없습니다.\n\n계속하시겠습니까?")){
					document.all("mymsg").focus();
					return false;				
				}
				document.all("mymsg").checked = false;
				document.all("req123").value = document.all("req123").value.replace(mymsgNumber,"");
			}	

			return true;
	}

	function SelectReqphone(){
		if(document.all["SelectPhoneList"].style.display=="none"){
			document.all["SelectPhoneList"].style.posLeft = window.posLayerSMS2.offsetLeft-18;
			document.all["SelectPhoneList"].style.posTop = window.posLayerSMS2.offsetTop+18;
			document.all["SelectPhoneList"].style.display = "";
		}
		else document.all["SelectPhoneList"].style.display="none";
	}

	function mouseOver(a)
	{
		a.style.background="#E6EEFA";
		a.style.color="#000000";
		a.style.cursor="hand";
	}

	function mouseOut(b)
	{
		b.style.background="#FFFFFF";
		b.style.color="#66666C";
	}

	function ClickBoardList(val)
	{
		var temp;
		temp = val.replace('none','');

		document.all("req123").value = temp;
		if(val=='') document.Form1.req123.focus();
		document.all["SelectPhoneList"].style.display = "none";
	}

	function ClickSetReqNo(){
		if(confirm(GoReplySettingPage)){
			 OpenReplyPhoneBook();
		}else{
			return;
		}
	}