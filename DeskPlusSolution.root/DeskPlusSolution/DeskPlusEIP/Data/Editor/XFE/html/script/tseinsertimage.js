var tseName = parent.document.getElementById("iframe_name").value;
var oFrame = parent.document.getElementById(tseName);
var tseBrowserKind = parent.document.getElementById("tseDlgBrowserKind").value;
var oRng;
var oImage = null;

if (window.getSelection)
{
	var oSelect = oFrame.contentWindow.window.getSelection();
	
	if( parent.select_imageElement != false && parent.select_imageElement != undefined)
	{
		oImage = parent.select_imageElement;
	}
			
}
else if (document.getSelection)
{
	var oSelect = oFrame.contentWindow.document.getSelection();
	
	if( parent.select_imageElement != false && parent.select_imageElement != undefined)
	{
		oImage = parent.select_imageElement;
	}
}
else if (document.selection)
{
	var selected_obj = oFrame.contentWindow.document.selection;
	if(selected_obj.type == "Control"){
		var strTagName = selected_obj.createRange().item(0).tagName;
		if(strTagName.toLowerCase() == "img")
			oImage = selected_obj.createRange().item(0);
		else
			oImage = null;
	}
	
/*
	if(selected_obj.type != "Control" || oImage.toLowerCase() != "img"){
		
	}
*/
}


function getSelectRng()
{
	if (parent.tseBrowserKind == "msie")
	{
		if(parent.m_ClickRag != undefined )
		{
			oRng = parent.m_ClickRag
		}
		else
		{
			parent.document.getElementById(tseName).contentWindow.document.body.focus();
			oRng = parent.document.getElementById(tseName).contentWindow.document.selection.createRange();
		}
	}
};


function setImgAttribute(oImage){ 
	if(oImage == null) return;
	var str = "";

	if(oImage.src != "")
	{
		document.getElementById("url").value = oImage.src;
	}
	if(oImage.style.height != "")
	{
		str = oImage.style.height;
		document.getElementById("height").value = str.substring(0, str.lastIndexOf('p'));
	}
	if(oImage.style.width != "")
	{
		str = oImage.style.width;
		document.getElementById("width").value = str.substring(0, str.lastIndexOf('p'));
	}
	
	if(oImage.style.verticalAlign == "" || oImage.style.verticalAlign  == "기본값" || oImage.style.verticalAlign  == "auto")
	{
		document.getElementById("align").defaultSelected = true;			
	}
	else
	{		
		document.getElementById("align").value = oImage.style.verticalAlign;			
	}
	
	if(oImage.style.borderWidth != "")
	{
		str = oImage.style.borderWidth;
		document.getElementById("border").value = str.substring(0, str.indexOf('p'));
	}
	if(oImage.style.marginRight != "")
	{
		str = oImage.style.marginRight;
		document.getElementById("hspace").value = str.substring(0, str.lastIndexOf('p'));
	}	
	if(oImage.style.marginTop != "")
	{
		str = oImage.style.marginTop;
		document.getElementById("vspace").value = str.substring(0, str.lastIndexOf('p'));
	}
	if(oImage.alt != "")
	{
		document.getElementById("alt").value = oImage.alt;
	}
};

function FileCheck()
{
   var chk = true;
	var str = document.getElementById('FILE_PATH').value;
	var ext = str.substring(str.lastIndexOf(".")+1, str.length);
	var file_Name = str.substring(str.lastIndexOf("\\")+1, str.length);

	if (ext.toLowerCase() == "jpg" || ext.toLowerCase() == "jpeg" ||
	    ext.toLowerCase() == "gif" || ext.toLowerCase() == "png")
	{
		document.getElementById('url').value = str;
		chk = true;
	}
	else
	{
		alert(parent.getString("STRID_UPLOAD_NOT_SUPPORTED_IMAGE"));
		chk = false;
	}
	return chk;
};

function AddImage(oImage) {
    
	if(oImage == null) 
	{
		if (parent.tseUploadAction() != undefined && parent.tseUploadAction() != "")
			document.forms[0].action = parent.tseUploadAction();
		else
		    document.forms[0].action = "../upload/upload.aspx"; //default는 asp로..

		if (document.getElementById("url").value != "")
		{
			var html = 'style="';
			//html += '<img src="' + document.getElementById("url").value + '"';
			if (document.getElementById("border").value != "") {
				html += ' border:' + document.getElementById("border").value + 'px solid #000000;';
			}
			if (document.getElementById("width").value != "0" && document.getElementById("width").value != "") {
				html += ' width:' + document.getElementById("width").value + 'px;';
			}
			if (document.getElementById("height").value != "0" && document.getElementById("height").value != "") {
				html += ' height:' + document.getElementById("height").value + 'px;';
			}
			if (document.getElementById("hspace").value != "") {
				html += ' margin-left:' + document.getElementById("hspace").value + 'px; margin-right:' + document.getElementById("hspace").value + 'px;';
			}
			if (document.getElementById("vspace").value != "") {
				html += ' margin-top:' + document.getElementById("vspace").value + 'px; margin-bottom:' + document.getElementById("vspace").value + 'px;';
			}
			if (document.getElementById("align").value != "" && document.getElementById("align").value != "Default" ) {
				html += ' vertical-align:' + document.getElementById("align").value + ';';
			}	else	{
				html += ' vertical-align:auto;';
			}
			html += '"';
			if (document.getElementById("alt").value != "") {
				html += ' alt="' + document.getElementById("alt").value + '"';
				html += ' title="' + document.getElementById("alt").value + '"';
			} else {
				var strFileName = document.getElementById("url").value;
				var tmpIndex = strFileName.lastIndexOf("\\");
				if (tmpIndex > 0)
					strFileName = strFileName.substring(tmpIndex+1, strFileName.length);
				html += ' alt="' + strFileName + '"';
			}

	
			
			if (parent.tseBrowserKind == "msie") {
				//oRng.pasteHTML('<span id="xfe_insertImage"></span>');
			}
			document.getElementById("html_holder").value = html;
			parent.tseSetTempAttrs(html);
			parent.setImgRag(oRng);
			document.forms[0].submit();
			document.getElementById("progress_img").src = "images/progress.gif";
			document.getElementById("progress_div").style.display = "";
			//parent.tseInsertHTML(html, oRng);
			//parent.document.getElementById("pop_Frame").src = "";
			//parent.document.getElementById("pop_layer").style.visibility = "hidden";

		} else {
			alert(parent.getString("STRID_IMAGE_NEEDED_SELECTED"));
		}
	}
	else	// 이미지 정보 수정.
	{
		if(document.getElementById("height").value != "")
		{
			oImage.style.height = document.getElementById("height").value;
		}
		if(document.getElementById("width").value != "")
		{
			oImage.style.width = document.getElementById("width").value;
		}
		
		if(document.getElementById("align").value == "" || document.getElementById("align").value  == "기본값" || document.getElementById("align").value  == "Default" )
		{
			oImage.style.verticalAlign = "auto";			
		}
		else
		{	
			oImage.style.verticalAlign = document.getElementById("align").value;	
		}
		
		if(document.getElementById("border").value != "")
		{				
			oImage.style.borderWidth = document.getElementById("border").value;			
		}
		if(document.getElementById("hspace").value != "")
		{
			oImage.style.marginRight = document.getElementById("hspace").value;
		}	
		if(document.getElementById("vspace").value != "")
		{
			oImage.style.marginTop = 	document.getElementById("vspace").value;
		}
		if(document.getElementById("alt").value != "")
		{
			oImage.alt = document.getElementById("alt").value;
		}		
		
		onCancel();		
	}
};

function onCancel()
{
    var layer = parent.document.getElementById("pop_layer");
    var frame = parent.document.getElementById("pop_Frame");

    layer.removeChild(frame);
    layer.style.visibility = "hidden";
};

function tseSpinUp(strName) {
	document.getElementById(strName).value++;
};

function tseSpinDown(strName) {
	var minValue = 0;
	if (document.getElementById(strName).value > minValue)
		document.getElementById(strName).value--;
};
function getSkinPath(strName)
{
	return parent.tseDlgSkinPath + strName;
};

setImgAttribute(oImage);