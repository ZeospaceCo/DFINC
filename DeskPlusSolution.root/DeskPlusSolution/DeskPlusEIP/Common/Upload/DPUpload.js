/*=================================================================================
 첨부파일 처리 시 추가 하는 스크립트 시작
=================================================================================*/		
function attachFile(where)
{
	var ranNum;
	var uploadURL;
	var spoolPath;
	
	ranNum = Math.random();
	ranNum = ranNum.toString();
	ranNum = ranNum.substring(2,ranNum.length);


	var beforeSpoolPath = Form1.tbHiddenSpoolPath.value;
	
	if (beforeSpoolPath != "") // 이전에 첨부를 했는지 확인
	{
		uploadURL = "/DeskPlusEIP/Common/Upload/AttachFile.aspx?where=" + where + "&ran=" + ranNum + "&uploadPath=" + escape(beforeSpoolPath) + "&CID=" + DelFileCID + "&UID=" + DelFileUID;
	}else
	{
		uploadURL = "/DeskPlusEIP/Common/Upload/AttachFile.aspx?where=" + where + "&ran=" + ranNum + "&CID=" + DelFileCID + "&UID=" + DelFileUID;		
	}

	var rtnVal = window.open (uploadURL, "", "toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=610px,height=490px");
}

/*=================================================================================
 첨부된 파일 리스트를 tbHiddenFileList 담기
=================================================================================*/	
function AttachOK(rtnVal)
{
	var rtnArr = rtnVal.split("<");

	if (rtnArr[0] != "-1") // 업로드를 함
	{
		if (rtnArr[1] != "")Form1.tbHiddenSpoolPath.value = rtnArr[1]; // 임시 파일 업로드 경로

		var oldFileList = Form1.tbHiddenFileList.value;

		if (oldFileList.length > 0) // 이전에 업로드한 파일들이 존재한다면
		{
			Form1.tbHiddenFileList.value += "|" + rtnArr[2]; // 새로 업로드한 파일 추가
		}else
		{
			Form1.tbHiddenFileList.value = rtnArr[2];				
		}

		// 새롭게 추가한 파일을 리스트에 추가
		var rtnFileList = rtnArr[2].split("|");
		

		for (i=0; i < rtnFileList.length; i++)
		{
		    //alert("rtnFileList : " + rtnFileList[i]);
			if (rtnFileList[i] != "") AddFileList(rtnFileList[i]);
		}
	}
	
	//alert(Form1.tbHiddenFileList.value);
}

/*=================================================================================
 리스트 박스에 파일 추가 하기
=================================================================================*/	
function AddFileList(filePath)
{

//alert(filePath)
// 1</DeskPlusEIP/FileData/1/Users/11/Spool/Board<L>C:\test.gif|L>c:\test2.gif|
// Key < 임시 폴더 < 파일리스트 ( 타입 : L, S, W ... >파일 정보 |

    var fileName = "";
    var fileInfo = filePath.split(">");
	
    var oOption = document.createElement("OPTION"); //List Item 생성
    
    
    if (fileInfo[0] == "L") //개인 pc에서 업로드 한것  ex) L>c:\Imsi\test.exe
    {
	    var fileIdx = 0;
	    fileName = fileInfo[1];  //c:\Imsi\test.exe
	    
	    if (fileName.indexOf("/") > -1)
	    {
		    fileIdx = fileName.lastIndexOf("/");
	    }else
	    {
		    fileIdx = fileName.lastIndexOf("\\");
	    }
    	
	    fileName = fileName.substr(fileIdx + 1, fileName.length - fileIdx - 1);    
    }else  //서버에 저장된것  fileInfo[0] == "S" , "W"
    {
        fileName = fileInfo[3]; //test.exe  //ex) S>첨부파일 ID>경로>파일명>GUID  : S>1>/DeskPlusEIP/Test/2/>test.exe>GUID
    }
    
	var oOption = document.createElement("OPTION");
	oOption.text=fileName;
	oOption.value=filePath;

    //alert("filePath Full : " + filePath + " ----  fileName : " + fileName + " ----  filePath : " + filePath);

    Form1.lbxFileList.add(oOption);
}		

/*
function AddFileList(filePath)
{
	var fileName = "";
	var fileIdx = 0;
	if (filePath.indexOf("/") > -1)
	{
		fileIdx = filePath.lastIndexOf("/");
	}else
	{
		fileIdx = filePath.lastIndexOf("\\");
	}
	
	fileName = filePath.substr(fileIdx + 1, filePath.length - fileIdx - 1);

	var oOption = document.createElement("OPTION");
	oOption.text=fileName;
	oOption.value=filePath;

	Form1.lbxFileList.add(oOption);						
}		
*/


/*=================================================================================
 파일 삭제
=================================================================================*/	
function DelFile(where)
{
    var len = Form1.lbxFileList.options.length - 1; // 첨부파일 리스트 사이즈
	var tmpFile = "";   // 파일 정보 
	var fileInfo;       // 파일 정보 Array
	
	var tmpFileIdx = "";
	var fileName = "";
	
	for (i=len; i >= 0; i--)
	{
		if (Form1.lbxFileList.options[i].selected)
		{
		    tmpFile = Form1.lbxFileList.options[i].value;
			fileInfo = tmpFile.split(">");

            if (fileInfo[0] == "L") //개인 pc에서 업로드 한것  ex) L>c:\Imsi\test.exe  (임시 저장소)
            {
                var fileIdx = 0;
                fileName = fileInfo[1];  //c:\Imsi\test.exe
        	    
                if (fileName.indexOf("/") > -1)
                {
	                fileIdx = fileName.lastIndexOf("/");
                }else
                {
	                fileIdx = fileName.lastIndexOf("\\");
                }
            	
                fileName = fileName.substr(fileIdx + 1, fileName.length - fileIdx - 1);  
                
			    ifrmDelFile.location.href = "/DeskPlusEIP/Common/Upload/DelFile.ashx?fileName=" + encodeURIComponent(fileName) + "&where=" + where + "&companyID=" + DelFileCID + "&userID=" + DelFileUID;

			    Form1.lbxFileList.remove(i);

            }else //서버에 저장된것  ex) S>/DeskPlusEIP/Test/2/>test.exe>GUID
            {
                Form1.lbxFileList.remove(i); //서버파일은 BLL 단에서 지우도록 한다
            }
           		
		}
	}
	
	len = Form1.lbxFileList.options.length;
	
	var filePathList = "";
	
	for (j=0; j < len; j++)
	{
		if (filePathList.length > 0) filePathList += "|";
		filePathList += Form1.lbxFileList.options[j].value;
	}
	
	//alert(Form1.tbHiddenFileList.value);
	//alert(filePathList)

	Form1.tbHiddenFileList.value = filePathList; // 삭제가 않된 파일들의 정보를 저장 
	
}

/*
function DelFile(where, cID, uID)
{
	var len = Form1.lbxFileList.options.length - 1;
	var tmpFile = "";
	var tmpFileIdx = "";
	var fileName = "";

	for (i=len; i >= 0; i--)
	{
		if (Form1.lbxFileList.options[i].selected)
		{
			tmpFile = Form1.lbxFileList.options[i].value;
			tmpFileIdx = tmpFile.lastIndexOf("\\");
			fileName += "/" + tmpFile.substr(tmpFileIdx + 1, tmpFile.length - tmpFileIdx - 1);
			
			ifrmDelFile.location.href = "/DeskPlusEIP/Common/Upload/DelFile.ashx?fileName=" + escape(fileName) + "&where=" + where + "&companyID=" + DelFileCID + "&userID=" + DelFileUID;
			Form1.lbxFileList.remove(i);
		}
	}
	
	len = Form1.lbxFileList.options.length;
	
	var filePathList = "";
	
	for (j=0; j < len; j++)
	{
		if (filePathList.length > 0) filePathList += "|";
		filePathList += Form1.lbxFileList.options[j].value;
	}

	Form1.tbHiddenFileList.value = filePathList; // 삭제가 않된 파일들의 정보를 저장 
}
*/


/*=================================================================================
 수정 모드시 기존 파일 정보 가져오기

=================================================================================*/	
function window::onload()
{
	var fileList = Form1.tbHiddenFileList.value;

	if (fileList.length > 0)
	{
		Form1.lbxFileList.clear;

		var fileListArr = fileList.split("|");
		for (i=0; i<fileListArr.length; i++)
		{
		    if (fileListArr[i].length)AddFileList(fileListArr[i]);
		}			
	}
}

/*=================================================================================
 작성 창에 첨부중이 파일 열기
arg1 : 위치 , arg2 : 선택된 값
=================================================================================*/	
function openFile(arg1, arg2)
{
    var fileName = "";
    var listBox = arg2; // 첨부파일 리스트 박스
    
    if (listBox.value)
    {
        var fileInfo = listBox.value.split(">");
    	
        if (fileInfo[0] == "L") //개인 pc에서 업로드 한것  ex) L>c:\Imsi\test.exe
        {        
	        var fileIdx = 0;
	        fileName = fileInfo[1];  //c:\Imsi\test.exe
    	    
	        if (fileName.indexOf("/") > -1)
	        {
		        fileIdx = fileName.lastIndexOf("/");
	        }else
	        {
		        fileIdx = fileName.lastIndexOf("\\");
	        }
        	
	        fileName = fileName.substr(fileIdx + 1, fileName.length - fileIdx - 1);    

            var url = uploadHost + "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?filePath=" + escape("/DeskPlusEIP/FileData/" + DelFileCID + "/Users/" + DelFileUID + "/Spool/" + arg1 + "/" + fileName) + "&fileName=" + escape(fileName);

	        window.open(url);    
        }else
        {
            fileName = fileInfo[3];
            
            var url = uploadHost + "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?filePath=" + escape(fileInfo[2] + fileInfo[4]) + "&fileName=" + escape(fileInfo[3]);
	        window.open(url);              
        }
    }
}    