
	//"최근전송번호" 열기
	function Open_Recent(){

		var ifr = document.all["tframe"];
		var RecentForm = document.all["Recent"];

		if (RecentForm.style.display == "")
		{
			Close_Recent();
		}		
		else {

			ifr.style.posLeft = window.posLayerSMS.offsetLeft-20;
			ifr.style.posTop = window.posLayerSMS.offsetTop+0;
			IDX_DIV++;
			ifr.style.zIndex = IDX_DIV;
			ifr.style.visibility = "visible";

			RecentForm.style.posLeft = window.posLayerSMS.offsetLeft-20;
			RecentForm.style.posTop = window.posLayerSMS.offsetTop+0;
			IDX_DIV++;
			RecentForm.style.zIndex = IDX_DIV;
			RecentForm.style.display = "";
		}
	}

	//"최근전송번호" 열기
	function Open_Recent_Div(){

		var ifr = document.all["tframe_Div"];
		var RecentForm = document.all["Recent"];

		if (RecentForm.style.display == "")
		{
			Close_Recent();
		}		
		else {

			ifr.style.posLeft = window.posLayerSMS_Div.offsetLeft+215;
			ifr.style.posTop = window.posLayerSMS_Div.offsetTop-25;
			ifr.style.visibility = "visible";

			RecentForm.style.posLeft = window.posLayerSMS_Div.offsetLeft+215;
			RecentForm.style.posTop = window.posLayerSMS_Div.offsetTop-25;
			RecentForm.style.display = "";
		}
	}

	//"최근전송번호" 닫기
	function Close_Recent_Div(){

		var ifr = document.all["tframe_Div"];
		var RecentForm = document.all["Recent"];

		RecentForm.style.display = "none";
		ifr.style.visibility = "hidden";
	}

	//"최근전송번호" 닫기
	function Close_Recent(){

		var ifr = document.all["tframe"];
		var RecentForm = document.all["Recent"];

		RecentForm.style.display = "none";
		ifr.style.visibility = "hidden";
	}


	//최근전송번호 선택
	function inputCell(cell, newcolor, idx, tr, phone) {
		var f = document.rf;
		temp_idx = '#' + idx;
		temp_tr = '#' + tr;
		temp_phone = '#' + phone;
		if ((cell.bgColor!='#bbbbbb')) {
			f.recentidx.value = f.recentidx.value + temp_idx;
			f.tr_idx.value = f.tr_idx.value + temp_tr;
			f.callphone.value = f.callphone.value + temp_phone;
			cell.bgColor = newcolor;
		} else {
			f.recentidx.value = f.recentidx.value.repl(temp_idx,'');
			f.tr_idx.value = f.tr_idx.value.repl(temp_tr,'');
			f.callphone.value = f.callphone.value.repl(temp_phone,'');
			cell.bgColor = '#ffffff';
		}
//		alert(temp_phone);
//		alert(f.callphone.value);
	}


	//최근전송번호 삭제
	function recent_del() {
		var f = document.rf;
		if (f.recentidx.value=='') {
			alert(SelectDeleteSendNumber);
		} else {
			var temp_tr = document.rf.tr_idx.value.substr(1,document.rf.tr_idx.value.length);
			var temp_tr_arr = temp_tr.split('#');

			if (confirm(ConfirmDel)) {
				f.action='/common/visualphone/VisualPhone_Recent_del.asp';
				f.target='rf_iframe';
				f.method='post';
				f.submit();
				for (ri=0; ri<temp_tr_arr.length; ri++) {
					var recent_tr_i = "recent_tr_" + temp_tr_arr[ri];
					var recent_tr = document.all[recent_tr_i];
					recent_tr.style.display = "none";
				}
			}
		}
	}

	//최근전송번호 선택 - 입력
	function recent_sel() {
		var f = document.rf;
		var myselobj = document.VisualPhone;

		if (f.recentidx.value=='') {
			alert(SelectInputSendNumber);
		} else {
			var temp = document.rf.callphone.value.substr(1,document.rf.callphone.value.length);
			var temp_arr = temp.split('#');

			for (ri=0; ri<temp_arr.length; ri++) {
				var temp_data = temp_arr[ri].split('@');
				if (temp_data[0]=='') {
					temp_name = temp_data[1];
				} else {
					temp_name = temp_data[0];
				}
				myselobj.ExList.options[myselobj.ExList.options.length] = new Option(temp_name,temp_data[1]);
				myselobj.sendlen.value = parseInt(myselobj.sendlen.value) + 1;
			}
			Close_Recent();
		}
	}

	//최근전송번호 선택 (더블클릭) - 입력
	function recent_sel_dblclk(phone,name) {
		var f = document.rf;
		var myselobj = document.VisualPhone;

		if (name=='') {
			temp_name = phone;
		} else {
			temp_name = name;
		}
		myselobj.ExList.options[myselobj.ExList.options.length] = new Option(temp_name,phone);
		myselobj.sendlen.value = parseInt(myselobj.sendlen.value) + 1;
	}




//*****************************************************************************************************************************************
//IFRAME
//*****************************************************************************************************************************************


	
	//최근전송번호 선택
	function ifr_inputCell(cell, newcolor, idx, tr, phone) {

		temp_idx	= '#' + idx;
		temp_tr		= '#' + tr;
		temp_phone = '#' + phone;

		var recentidx = $("#recentidx").val();
		var tr_idx = $("#tr_idx").val();
		var callphone = $("#callphone").val();


		if ((cell.bgColor != '#bbbbbb')) {
		    $("#recentidx").val(recentidx + temp_idx);
		    $("#tr_idx").val(tr_idx + temp_tr);
		    $("#callphone").val(callphone + temp_phone);
		    cell.bgColor = newcolor;
		} else {

		    $("#recentidx").val(recentidx.replace(temp_idx, ''));
		    $("#tr_idx").val(tr_idx.replace(temp_tr, ''));
		    $("#callphone").val(callphone.replace(callphone, ''));

		    cell.bgColor = '#ffffff';
		}
	}


	function ifr_recent_del() {
		var f = ifrm_recent.document.rf;
		
		if (f.recentidx.value=='') {
			alert(SelectDeleteSendNumber);
		} else {
			//var temp_tr = f.tr_idx.value.substr(1,f.tr_idx.value.length);
			var temp_tr = f.recentidx.value.substr(1,f.recentidx.value.length);			
			var temp_tr_arr = temp_tr.split('#');
			
			if (confirm(ConfirmDel)) {
				document.all("ifrmRecentSMSProc").src = "../SMS/RecentSMSProc.aspx?id=" + escape(temp_tr);				
				for (ri=0; ri<temp_tr_arr.length; ri++) {
					
					var recent_tr_i = "recent_tr_" + temp_tr_arr[ri];									
					var recent_tr =ifrm_recent.document.all[recent_tr_i];				
					recent_tr.style.display = "none";
				}
			}
		}
	}


	//최근전송번호 선택 - 입력
	function ifr_recent_sel() {
	
		var f = ifrm_recent.document.rf;
		//var myselobj = document.VisualPhone;

		if (f.recentidx.value=='') {
			alert(SelectInputSendNumber);
		} else {
			var temp = f.callphone.value.substr(1,f.callphone.value.length);
			var temp_arr = temp.split('#');

			for (ri=0; ri<temp_arr.length; ri++) {
				var temp_data = temp_arr[ri].split('@');
				if (temp_data[0]=='') {
					temp_name = temp_data[1];
				} else {
					temp_name = temp_data[0];
				}
				document.all("ExList").options[document.all("ExList").options.length] = new Option(temp_name,temp_data[1]);
				document.all("sendlen").value = parseInt(document.all("sendlen").value) + 1;
			}
			Close_Recent();
		}
	}

	//최근전송번호 선택 (더블클릭) - 입력
	function ifr_recent_sel_dblclk(phone,name) {
		var f = document.rf;
		//var myselobj = parent.document.Form1;

		if (name=='') {
			temp_name = phone;
		} else {
			temp_name = name;
		}
		parent.document.all("ExList").options[parent.document.all("ExList").options.length] = new Option(temp_name,phone);
		parent.document.all("sendlen").value = parseInt(parent.document.all("sendlen").value) + 1;
	}

	
//*****************************************************************************************************************************************
//TEST
//*****************************************************************************************************************************************


	
	//최근전송번호 선택
	function test_inputCell(cell, newcolor, idx, tr, phone) {

		var f = document.rf;
	
		temp_idx	= '#' + idx;
		temp_tr		= '#' + tr;
		temp_phone	= '#' + phone;

		if ((cell.bgColor!='#bbbbbb')) {
			f.recentidx.value	= f.recentidx.value + temp_idx;
			f.tr_idx.value		= f.tr_idx.value + temp_tr;
			f.callphone.value	= f.callphone.value + temp_phone;
			cell.bgColor		= newcolor;
		} else {
			f.recentidx.value	= f.recentidx.value.repl(temp_idx,'');
			f.tr_idx.value		= f.tr_idx.value.repl(temp_tr,'');
			f.callphone.value	= f.callphone.value.repl(temp_phone,'');
			cell.bgColor		= '#ffffff';
		}
		//alert(temp_phone);
		//alert(f.callphone.value);
	}


	function test_recent_del() {
		var f = ifrm_recent.document.rf;
		
		if (f.recentidx.value=='') {
			alert(SelectDeleteSendNumber);
		} else {
			var temp_tr = f.tr_idx.value.substr(1,f.tr_idx.value.length);
			var temp_tr_arr = temp_tr.split('#');			

			if (confirm(ConfirmDel)) {
				f.action='/common/visualphone/VisualPhone_Recent_del.asp';
				f.target='rf_iframe';
				f.method='post';			
				f.submit();	
				
				for (ri=0; ri<temp_tr_arr.length; ri++) {
					
					var recent_tr_i = "recent_tr_" + temp_tr_arr[ri];				
					var recent_tr =ifrm_recent.document.all[recent_tr_i];				
					recent_tr.style.display = "none";
				}
			}
		}
	}


	//최근전송번호 선택 - 입력
	function test_recent_sel() {
	
		var f = ifrm_recent.document.rf;
		var myselobj = document.VisualPhone;

		if (f.recentidx.value=='') {
			alert(SelectInputSendNumber);
		} else {
			var temp = f.callphone.value.substr(1,f.callphone.value.length);
			var temp_arr = temp.split('#');

			for (ri=0; ri<temp_arr.length; ri++) {
				var temp_data = temp_arr[ri].split('@');

				if (temp_data[0]=='') {
					temp_name = temp_data[1];
				} else {
					temp_name = temp_data[0];
				}
				
				myselobj.ExList.options[myselobj.ExList.options.length] = new Option(temp_name,temp_data[1]);
				myselobj.sendlen.value = parseInt(myselobj.sendlen.value) + 1;
			}
			Close_Recent();
		}
	}

	//최근전송번호 선택 (더블클릭) - 입력
	function test_recent_sel_dblclk(phone,name) {
		var f = document.rf;
		var myselobj = parent.document.VisualPhone;

		if (name=='') {
			temp_name = phone;
		} else {
			temp_name = name;
		}
		myselobj.ExList.options[myselobj.ExList.options.length] = new Option(temp_name,phone);
		myselobj.sendlen.value = parseInt(myselobj.sendlen.value) + 1;
	}
