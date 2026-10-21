
	//템플릿 글 열기
	function Open_Content(){
		
		var ifr = document.getElementById("tframe2");
		var OpenContent = document.getElementById("OpenContent");

		if (OpenContent.style.display == "")
		{
			Close_Content();
		}		
		else {

			ifr.style.posLeft = window.posLayerSMS.offsetLeft-20;
			ifr.style.posTop = window.posLayerSMS.offsetTop+55;
			IDX_DIV++;
			ifr.style.zIndex = IDX_DIV;
			ifr.style.visibility = "visible";

			OpenContent.style.posLeft = window.posLayerSMS.offsetLeft-20;
			OpenContent.style.posTop = window.posLayerSMS.offsetTop+55;
			IDX_DIV++;
			OpenContent.style.zIndex = IDX_DIV;
			OpenContent.style.display = "";
		}
	}

	//템플릿 글 닫기
	function Close_Content(){

		var ifr = document.getElementById("tframe2");
		var OpenContent = document.getElementById("OpenContent");

		OpenContent.style.display = "none";
		ifr.style.visibility = "hidden";
	}



//*****************************************************************************************************************************************
//IFRAME
//*****************************************************************************************************************************************



    //템플릿 글 의 삭제
	function ifr_openContent_del() {
	    var f = ifrm_openContent.document.rf;

	    var openContentidx = $('#ifrm_openContent').contents().find('#openContentidx').val();	    
		
		if (openContentidx=='') {
			alert(SelectDeleteContent);
		} else {
			//var temp_tr = f.tr_idx.value.substr(1,f.tr_idx.value.length);
			var temp_tr = openContentidx.substr(1,openContentidx.length);			
			var temp_tr_arr = temp_tr.split('#');

			if (confirm(ConfirmDel)) {			    

			    var url = "/DeskPlusEIP/Portlet/Ws/Package/SMS/SendSMS.asmx/SaveContent";

			    //파라미터값 배열에 담기
			    //auth 관리자페이지 : 1; 일반인페이지 : 0
			    ParamArray = {
			        'id': escape(temp_tr),
			        'gubun': 'delete',
			        'content': ''
			    };

			    var obj = eval("Save_Content_Success");
			    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
			    data = "{p:'" + GetEncryptValue + "'}";
			    AjaxService(url, data, obj);
			    			
                
				for (ri=0; ri<temp_tr_arr.length; ri++) {
					
					var opencontent_tr_i = "opencontent_tr_" + temp_tr_arr[ri];									
					var opencontent_tr =ifrm_openContent.document.all[opencontent_tr_i];				
					opencontent_tr.style.display = "none";
				}
			}
		}
    }

    //==================================================================    
    // Save_Content() : 문자저장 완료
    //==================================================================
    function Save_Content_Success(data, status) {
        if (data.d == true) {
            alert(SMS_SMSConfig_Success);
        }
        else {
            alert("error");
        }
    }


	//템플릿 글 의 선택
    function ifr_openContent_sel() {
        
		var f = ifrm_openContent.document.rf;
		//var myselobj = document.VisualPhone;
		var openContentidx = $('#ifrm_openContent').contents().find('#openContentidx').val();
		var content = $('#ifrm_openContent').contents().find('#content').val();
		
		if (openContentidx.value == '') {
			alert(SelectInputContent);
		} else {
			var temp = content.substr(1,content.length);
			var temp_arr = temp.split('#');

			for (ri=0; ri<temp_arr.length; ri++) {
				var temp_data = temp_arr[ri].split('@');
				if (temp_data[0]=='') {
					temp_name = temp_data[1];
				} else {
					temp_name = temp_data[0];
				}
				
				
				document.getElementById("txtMessage").value = temp_data[1];
				checklen(); //글자length
		
				//document.all("ExList").options[document.all("ExList").options.length] = new Option(temp_name,temp_data[1]);
				//document.all("sendlen").value = parseInt(document.all("sendlen").value) + 1;
			}
			Close_Content();
		}
	}
	
	
	//템플릿 글 선택 (더블클릭) - 입력
	function ifr_openContent_sel_dblclk(content, name) {    
		
		if (name=='') {
			temp_name = AjaxCallDecode(content);
		} else {
			temp_name = name;
		}
		//parent.document.all("ExList").options[parent.document.all("ExList").options.length] = new Option(temp_name,content);
//parent.document.all("sendlen").value = parseInt(parent.document.all("sendlen").value) + 1;
        var txtMessage = parent.document.getElementById("txtMessage").value;	
		parent.document.getElementById("txtMessage").value = txtMessage + temp_name;		
		parent.checklen(); //글자length
	}
	
	
	//템플릿 글 선택
	function ifr_inputCell(cell, newcolor, idx, tr, content) {
	    
		temp_idx	    = '#' + idx;
		temp_tr		    = '#' + tr;
		temp_content = '#' + content;
			
		var openContentidx = $("#openContentidx").val();
		var tr_idx = $("#tr_idx").val();
		var content = $("#content").val();		

		if ((cell.bgColor != '#bbbbbb')) {
		    $("#openContentidx").val(openContentidx + temp_idx);
		    $("#tr_idx").val(tr_idx + temp_tr);
		    $("#content").val(content + temp_content);
		    cell.bgColor = newcolor;
		} else {

		    $("#openContentidx").val(openContentidx.replace(temp_idx, ''));
		    $("#tr_idx").val(tr_idx.replace(temp_tr, ''));
		    $("#content").val(content.replace(temp_content, ''));
		    
		    cell.bgColor = '#ffffff';
		}
	}	
