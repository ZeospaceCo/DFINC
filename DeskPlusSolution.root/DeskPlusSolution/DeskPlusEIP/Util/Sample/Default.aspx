<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.SWFUpload.Sample.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head><meta charset="UTF-8">
<title>SWFUpload Demo by Chintan Jajal</title>
<link href="default.css"rel="stylesheet" type="text/css" />
<script type="text/javascript" src="swfupload.js"></script>
<script type="text/javascript" src="swfupload.queue.js"></script>
<script type="text/javascript" src="fileprogress.js"></script>
<script type="text/javascript" src="handlers.js"></script>
<script type="text/javascript" src=jquery-1.3.1.min.js></script>
<script type="text/javascript">

    var R_DelMsg = "삭제하시겠습니까?";
    var swfu;

    window.onload = function () {
        var settings = {
            flash_url: "swfupload.swf",
            upload_url: "handler.ashx", // Relative to the SWF file
            post_params: {},
            file_size_limit: "10000 MB",
            file_types: "*.*",
            file_types_description: "All Files",
            file_upload_limit: 100,
            file_queue_limit: 0,
            custom_settings: {
                progressTarget: "fsUploadProgress",
                cancelButtonId: "btnCancel"
            },
            debug: false,

            // Button settings
            button_image_url: "XPButtonUploadText_61x22.png", // Relative to the Flash file
            button_width: "61",
            button_height: "22",
            button_placeholder_id: "spanButtonPlaceHolder",
            //button_text: '파일업로드',
            //button_text_style: ".theFont { font-size: 11; }",
            button_text_left_padding: 12,
            button_text_top_padding: 3,

            // The event handler functions are defined in handlers.js
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_start_handler: uploadStart,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: uploadSuccess,
            upload_complete_handler: uploadComplete //,
            //queue_complete_handler : queueComplete	// Queue plugin event
        };

        swfu = new SWFUpload(settings);
    };
	</script>
</head>

<body>
<div id="content">
	
	<form id="form1" runat=server >
            <div>
				<span id="spanButtonPlaceHolder"></span>
				<input id="btnCancel" type="button" value="업로드중단" onclick="swfu.cancelQueue();" disabled="disabled" style="margin-left: 2px; font-size: 8pt; height: 23px;" />

			</div>

			<div class="fieldset flash" id="fsUploadProgress">
			    <span class="legend">Upload Queue</span>
			</div>
		
			

	</form>
</div>
</body>
</html>
