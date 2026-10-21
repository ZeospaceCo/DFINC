
/*

site : www.michielpost.nl/Silverlight/MultiFileUploader/

-MaxFileSizeKB: File size in KBs. (파일 개당 용량 제한 걸기, 대신 전체 용량을 제한 걸지는 못함)
-MaxUploads: Maximum number of simultaneous uploads (2 설정시, 업로드 할 파일 6개를 올릴 경우 2개씩 파일을 나눠서 3번 올림)
-FileFilter:	File filter, for example ony jpeg use: FileFilter=Jpeg (*.jpg) |*.jpg (필터)
-CustomParam: Your custom parameter, anything here will be available in the WCF webservice (ashx 에 넘길 파라미터 정의)
-DefaultColor: The default color for the control, for example: LightBlue (스킨)
-ChunkSize: Size of each uploaded chunk in bytes (minimum is 4096, default is 4194304) (only for the HttpUploader) (몇 byte 씩 짤라서 socket 으로 서버에 전송할 양을 지정)

*/


$(function () {
    $(document).ready(function () {
        //적용
        $('#cbtnApply').click(function (e) {
            e.preventDefault();
//            var obj = eval("parent." + callBackEvent);
//            obj();
//            parent.disablePopup();

            if (parent.disablePopup) {
                var obj = eval("parent." + callBackEvent);
                obj();
                parent.disablePopup();
            } else {
                var obj = eval("opener." + callBackEvent);
                obj();
                self.close();
            }
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //dp upload
        $('#DPUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            //var iPath = iPageUrl.attr('path');
            var iQuery = iPageUrl.attr('query');            

            location = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?" + iQuery;
        });

        //folder upload
        $('#FolderUpload').click(function () {

            var iPageUrl = $.url(PageUrl);
            var iQuery = iPageUrl.attr('query');

            location = "/DeskPlusEIP/WebHard/Upload/WebHardAttach.aspx?" + iQuery;            
            
        });

        //KB
        if (fileSizeLimit < 0) {
            alert(R_ExistPrivateTotalLimitSize);
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
            return;
        }
        
    });
});

//DO NOT FORGET TO REGISTER THIS FUNCTION WITH THE SILVERIGHT CONTROL
// <param name="onload" value="pluginLoaded" />
function pluginLoaded(sender) {

    //IMPORTANT: Make sure this is the same ID as the ID in your <OBJECT tag (<object id="MultiFileUploader" etc)
    slCtl = document.getElementById("MultiFileUploader");

    //Images (*.jpg;*.jpeg*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*";
    //Bilder(*.jpg *.png *.gif*)|*.jpg;*.png;*.gif|Dokumente(*.pdf)|*.pdf|Videos(*.mpeg *.avi *.wma)|*.mpeg;*.avi;*.wma|Audio(*.mp3)|*.mp3
    //slCtl.Content.Configuration.FileFilter = "Images (*.jpg *.jpeg *.png *.gif)|*.jpg;*.jpeg;*.png;*.gif";
    //slCtl.Content.Configuration.FileFilter = "All Files (*.*)|*.*";    
    slCtl.Content.Configuration.FileFilter = fileTypeDesc + "|" + fileType;


    //Set your custom parameter using javascript
    //This parameter will be available in the webservice and you can use it for your business logic
    //Or use it to identity the upload to a sinle row in your database
    slCtl.Content.Files.CustomParams = encodeURIComponent(guid + "|" + companyID + "|" + userID + "|" + fileType);


    //slCtl.Content.Files.MaxFileSizeKB = 102400; //kb(100mb)
}

function onSilverlightError(sender, args) {

    var appSource = "";
    if (sender != null && sender != 0) {
        appSource = sender.getHost().Source;
    }
    var errorType = args.ErrorType;
    var iErrorCode = args.ErrorCode;

    var errMsg = "Unhandled Error in Silverlight 2 Application " + appSource + "\n";

    errMsg += "Code: " + iErrorCode + "    \n";
    errMsg += "Category: " + errorType + "       \n";
    errMsg += "Message: " + args.ErrorMessage + "     \n";

    if (errorType == "ParserError") {
        errMsg += "File: " + args.xamlFile + "     \n";
        errMsg += "Line: " + args.lineNumber + "     \n";
        errMsg += "Position: " + args.charPosition + "     \n";
    }
    else if (errorType == "RuntimeError") {
        if (args.lineNumber != 0) {
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " + args.charPosition + "     \n";
        }
        errMsg += "MethodName: " + args.methodName + "     \n";
    }

    throw new Error(errMsg);
}