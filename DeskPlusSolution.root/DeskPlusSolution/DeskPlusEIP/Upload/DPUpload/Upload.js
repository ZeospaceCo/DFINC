
$(function () {
    $(document).ready(function () {

        //다중 파일 업로드
        $('#cbtnApply').click(function (e) {
            e.preventDefault();

            if (Upload() == true) {
                //                var obj = eval("parent." + callBackEvent);
                //                obj();
                //parent.disablePopup();
                if (parent.disablePopup) {
                    var obj = eval("parent." + callBackEvent);
                    obj();
                    parent.disablePopup();
                } else {
                    var obj = eval("opener." + callBackEvent);
                    obj();
                    self.close();
                }
            }

        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //silverlight upload
        $('#RIAUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            //var iPath = iPageUrl.attr('path');
            var iQuery = iPageUrl.attr('query');

            //location = "/DeskPlusEIP/Upload/SilverUpload/Main.aspx?" + iQuery;
            location = "/DeskPlusEIP/Upload/JSUpload/Main.aspx?" + iQuery;
        });

        //web hard
        $('#FolderUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            var iQuery = iPageUrl.attr('query');

            location = "/DeskPlusEIP/WebHard/Upload/WebHardAttach.aspx?" + iQuery;
        });

        //standard
        $('#NETUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            //var iPath = iPageUrl.attr('path');
            var iQuery = iPageUrl.attr('query');

            location = "/DeskPlusEIP/Upload/Standard/Main.aspx?" + iQuery;
        });

        init();
    });
});

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
function init() {

    if (limitTotalSize < 0) {
        alert(R_ExistPrivateTotalLimitSize);
        if (parent.disablePopup) {parent.disablePopup();} else {self.close();}
        return;
    }

    setTimeout("InitDpUpload();", 30);

    //클라이언트 버전 정보를 확인 하여 다국어 지원 여부 확인
    var userAgentInfo = navigator.userAgent;
    var userAgentArr = userAgentInfo.split(";");
    var userAgentOSVersion = userAgentArr[2];

    if (userAgentOSVersion.indexOf("95") > -1 || userAgentOSVersion.indexOf("98") > -1) {
        uploadETC = "&unicode=N";
    }

    if (limitSize == "") {
        limitSize = "5120";
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 폴더 노드 이름 설정하기
***************************************************/
function InitDpUpload() {

    if (fileType != "") {
        form1.DPUpload.FileViewFilter = fileType; //filter
    }


    //업로드 파일 리스트 Title
    form1.DPUpload.SetTitleHeaderList(R_UploadName, 200, 0);  //이름
    form1.DPUpload.SetTitleHeaderList(R_UploadSize, 60, 1);   //크기
    form1.DPUpload.SetTitleHeaderList(R_UploadType, 75, 0);   //유형
    form1.DPUpload.SetTitleHeaderList(R_UploadDate, 80, 0);   //수정한 날짜

    //업로드 패널 리스트 Title
    form1.DPUpload.SetTitleHeaderListFile(R_UploadName, 80, 0);
    form1.DPUpload.SetTitleHeaderListFile(R_UploadSize, 40, 1);
    form1.DPUpload.SetTitleHeaderListFile(R_UploadType, 40, 0);
    form1.DPUpload.SetTitleHeaderListFile(R_UploadDate, 130, 0);

    form1.DPUpload.SetTitleDesktop = R_RootFolder;
    form1.DPUpload.SetTitleMyDocuments = R_MyDocument;
    form1.DPUpload.SetTitleMyComputer = R_MyComputer;
    form1.DPUpload.SetTitleMyNetworkPlaces = "My Network Places";
    form1.DPUpload.SetStartFolder = "z";
    form1.DPUpload.RunCreateInit();

    form1.DPUpload.SetTitleSelected = R_UploadTitleSelected;   //업로드 파일목록(삭제시 우클릭)
    form1.DPUpload.SetTitleListHelp = R_UploadTitleListHelp;   //파일을 선택 후 위 선택창으로 Drag 또는 '파일선택'버튼을 클릭하세요.
    form1.DPUpload.SetTitleSelectButton = R_UploadTitleSelectButton;  //파일 선택
    form1.DPUpload.SetTitleSearchButton = R_UploadSearchButton;   //파일 찾기
    form1.DPUpload.SetTitleFolder = R_UploadTitleFolder;      //폴더
    form1.DPUpload.SetPostFilePath(R_UploadPostFilePath);     //폴더
}

		
/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 다중 파일 업로드 기술
***************************************************/
function Upload() {

    var fileUploadCount = form1.DPUpload.AppendFileCount();
    if (fileCount != "0") {
        if (fileUploadCount > fileCount) {
            alert(fileCount + R_NoUploadOver);
            return false;
        }
    }

    if (fileUploadCount > 0)
	{
		form1.DPUpload.SetFileNotUpload = 0;
		form1.DPUpload.MaxFileSize = limitSize;		// 업로드 파일 크기 제한
		form1.DPUpload.MaxTotalSize = 0;//limitTotalSize; // 업로드 가능한 총파일 크기
		form1.DPUpload.Host = uploadHost;
		form1.DPUpload.Port = uploadPort;
				
		var iFileCount = form1.DPUpload.AppendFileCount;//업로드된 파일 개수
        var sFileResult = "";
        var bUploadFileList = "";
        var bUpload = true;
        var iTotalFileSize = 0; //현재 업로드한 파일들의 전체 용량(Byte)
        var bFileFilter = true;
            
        limitSizeByte = limitSize * 1024;//byte 치환
        
        for( i=0; i<iFileCount; ++i)
        {
            sFileName = form1.DPUpload.AppendFileName(i);//파일 당 파일명
            sFileSize = form1.DPUpload.AppendFileSize(i); //파일 당 파일사이즈 byte
            sFilePath = form1.DPUpload.AppendFilePath(i); //파일 당 파일 경로
            var abileExtFile = false; //가능 확장자 여부

            $.each(fileType.split('|'), function (idx) {
                if (sFileName.toLowerCase().indexOf("." + fileType.split('|')[idx]) != -1) {
                    abileExtFile = true;
                }
            });

            if (abileExtFile == false) {
                alert(R_UploadAllowList + " [" + fileTypeDesc + "]");
                bFileFilter = false;
            }

            $.each(BlockExt.split(','), function (idx) {
                if (sFileName.toLowerCase().indexOf("." + BlockExt.split(',')[idx]) > 0) {
                    alert(R_UploadBlockList + " [" + BlockExt + "]");
                    bFileFilter = false;
                    return false;
                }
            });

            //파일명 특수문자 제한 ByFreeman 20131030
            $.each(BlockFileName.split(','), function (idx) {
                if (sFileName.indexOf(BlockFileName.split(',')[idx]) > 0) {
                    alert(R_UploadBlockFileName);
                    bFileFilter = false;
                    return false;
                }
            });

            //첨부파일 불가 확장자 일경우 false 
            if (bFileFilter == false) return false;



            sFileResult = sFileResult + sFileName + sFileSize + sFilePath + "\r\n";
               
            iTotalFileSize += sFileSize;

            //### 파일 하나의 사이즈가 클 경우,
            if ( limitSizeByte < sFileSize)
            {
                bUpload = false;
                if(bUploadFileList.length > 0 ) bUploadFileList += "\n";
                bUploadFileList += sFileName + " (" + (sFileSize /1024) + "KB)";
            }
        }
            
        if (!bUpload && limitSize > 0)
        {
            alert(R_ExistLimitSize + "\n\n (" + (limitSize / 1024) + "MB) " + R_NoUploadOver + "\n\n" + bUploadFileList);
            return false;
        }

        //### 웹하드와 일정관리는 개인용량이므로 개인별 제한된 용량 체크한다.
        if ((ModuleName == "WEBHARD") || (ModuleName == "PIM")) {
            //현재 업로드한 파일들의 총 용량 :iTotalFileSize 사이즈 체크 (byte)
            if (iTotalFileSize > limitTotalSize && limitTotalSize > 0)
            {
                alert(R_ExistPrivateTotalLimitSize + "\n\n" + " TOTAL (" + ((limitTotalSize / 1024) / 1024) + " MB) " + R_NoUploadOver);
                return false;
            }
        }        
				
		//지정한 경로에 있는 전부의 파일들을 저장한다.
        //Windows98일 경우 파일 업로드시 서버에 유니코드가 지원돼지 않는다는 정보를 보낸다

        form1.DPUpload.PostAcceptor = uploadURL + "?p=" + uploadPath; //  + "&cID=" + cID + "&uID=" + uID + uploadETC;
		//alert(form1.DPUpload.PostAcceptor);
		form1.DPUpload.UploadStart = 1;
		form1.DPUpload.Post();
		var rtnVal = form1.DPUpload.ServerRespones;

		return true;

		/*

		if (rtnVal == "")
		{
			var fileList = FileList();
			//alert("uploadPath : " + uploadPath);
			//alert("fileList : " + fileList);

			alert("Upload rtnVal : " + rtnVal);
			return;

			if(top.opener.AttachOK) 
			{ 
				top.opener.AttachOK("1<" + uploadPath  + "<" + fileList); 
			}

			setTimeout("top.close();", 300);

			//window.returnValue = "1<" + uploadPath  + "<" + fileList;	
			//window.close();
		}else
		{
			if(top.opener.AttachOK) 
			{ 
				top.opener.AttachOK("-1"); 
						
			}
					
			setTimeout("top.close();", 300);
			//window.returnValue = "-1";
		}

        */
	}else
	{
	    alert("No Selected File(s)");
	    return false;
	}
}