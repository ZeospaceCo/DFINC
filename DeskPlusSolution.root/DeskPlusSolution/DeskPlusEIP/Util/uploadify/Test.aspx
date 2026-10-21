<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Util.uploadify.Test" %>

<!DOCTYPE html>

<html>
<head runat="server"><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>    
    <script src="../../Html/Js/jQuery/jquery.js" type="text/javascript"></script>    
    <script src="jUpload/swfobject.js" type="text/javascript"></script>
    <script src="jUpload/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    

    <style type="text/css">
            #custom-demo .uploadifyQueueItem
            {
                background-color: #FFFFFF;
                border: none;
                border-bottom: 1px solid #E5E5E5;
                font: 11px Verdana, Geneva, sans-serif;
                height: 50px;
                margin-top: 0;
                padding: 10px;
                width: 350px;
            }
            #custom-demo .uploadifyError
            {
                background-color: #FDE5DD !important;
                border: none !important;
                border-bottom: 1px solid #FBCBBC !important;
            }
            #custom-demo .uploadifyQueueItem .cancel
            {
                float: right;
            }
            #custom-demo .uploadifyQueue .completed
            {
                color: #C5C5C5;
            }
            #custom-demo .uploadifyProgress
            {
                background-color: #E5E5E5;
                margin-top: 10px;
                width: 100%;
            }
            #custom-demo .uploadifyProgressBar
            {
                background-color: #0099FF;
                height: 3px;
                width: 1px;
            }
            #custom-demo #custom-queue
            {
                border: 1px solid #E5E5E5;
                height: 213px;
                margin-bottom: 10px;
                width: 370px;
            }
        </style>
    <script type="text/javascript">
        $(function () {
            //싱글 업로드 화면만 제공하며, 버튼을 선택할때 선택한 파일이 서버로 전송된다.
            $('#file_upload').uploadify({
                'uploader': 'jUpload/uploadify.swf',
                'script': 'uploadify.ashx',
                'cancelImg': 'jUpload/cancel.png',
                'folder': 'uploads',
                'removeCompleted': true
            });

            //멀티로 업로드 화면을 제공해주며, 업로드할 파일을 선택하자마자 자동으로 서버로 전송된다.
            $('#custom_file_upload').uploadify({
                'uploader': 'jUpload/uploadify.swf',
                'script': 'uploadify.ashx',
                'cancelImg': 'jUpload/cancel.png',
                'folder': 'uploads',
                'multi': true,
                'auto': true,
                'fileExt': '*.jpg;*.gif;*.png',
                'fileDesc': 'Image Files (.JPG, .GIF, .PNG)',
                'queueID': 'custom-queue',
                'queueSizeLimit': 3,
                'simUploadLimit': 3,
                'removeCompleted': false,
                'onSelectOnce': function (event, data) {
                    $('#status-message').text(data.filesSelected + ' files have been added to the queue.');
                },
                'onAllComplete': function (event, data) {
                    $('#status-message').text(data.filesUploaded + ' files uploaded, ' + data.errors + ' errors.');
                }
            });

            //멀티로 업로드 화면을 제공하지만, 버튼을 눌러야 파일이 서버로 전송된다.
            $('#file_upload1').uploadify({
                'uploader': 'jUpload/uploadify.swf',
                'script': 'uploadify.ashx',
                'cancelImg': 'jUpload/cancel.png',
                'folder': 'uploads',
                'multi': true
            });


        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <input id="file_upload" type="file" name="Filedata" />
        <br />
        <a href="javascript:$('#file_upload').uploadifyUpload()">Upload Files</a>
        <br />
        <br />
        <br />
    </div>
    <div id="custom-demo" class="demo">
        
        <div class="demo-box">
            <div id="status-message">
                Select some files to upload:</div>
            <div id="custom-queue">
            </div>
            <input id="custom_file_upload" type="file" name="Filedata" />
        </div>
    </div>
    <br />
    <div id="demo">
        <input type="file" id="file_upload1" name="file_upload1" />
        <a href="javascript:$('#file_upload1').uploadifyUpload();">Upload Files</a> | 
        <a href="javascript:$('#file_upload1').uploadifyCancel($('.uploadifyQueueItem').first().attr('id').replace('file_upload1',''))">
            Cancel First File</a>
    </div>      
    </form>
</body>
</html>
