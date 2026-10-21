<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Upload.JSUpload.Main" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<%=csrf_token%>">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" href="css/bootstrap.css" />
    <link rel="stylesheet" href="css/bootstrap-responsive.css" />
    <link rel="stylesheet" href="css/style.css" />    
    <link rel="stylesheet" href="<%=BaseCssPath%>/Dstyle/style.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/stylecolorvariation.css"/> 
    <!--<link rel="stylesheet" href="css/blueimp-gallery.min.css">-->
    <link rel="stylesheet" href="css/jquery.fileupload-ui.css" />
    <noscript><link rel="stylesheet" href="css/jquery.fileupload-ui-noscript.css"></noscript>

    <script type="text/javascript">
        var guid = "<%=GUID %>";
        var callBackEvent = "<%=CallBackEvent %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var slCtl = null;

        var limitSize = "<%=FileSizePerFile %>"; //KB (하나의 업로드파일 사이즈 제한)
        var limitTotalSize = "<%=FileSizePerUser %>"; //개인별 총 업로드 용량 제한(웹하드, 일정관리), 0 이면 제한없이 업로드 가능
        var uploadPath = "<%=FilePathUser %>";
        var fileType = "<%= FileType %>";  //"<%= FileType %>";
        var fileTypeDesc = "<%=FileTypeDesc %>";
        var fileCount = "<%=FileCount %>"; //파일 개수 제한
        var ModuleName = "<%=ModuleName %>";
        var BlockExt = "<%=BlockExt%>";

        var ImagePath = "<%=ImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>

</head>
<body style="vertical-align:top;">
<form id="fileupload" action="Default.aspx" method="POST" enctype="multipart/form-data">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1 small><%=GetResource("Upload")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnApply" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3"  style="min-height:430px;height:455px;">
            <div id="tabs">
                <ul id="tab" class="tab">                    
                    <li><a id="RIAUpload" class="tab_on"><%=GetResource("Files")%></a></li>
                    <li><a id="FolderUpload"><%=GetResource("WebHardTitle")%></a></li>
                </ul>
                <div id="tabs-2" style="margin-top:20px;">
                    <div style="width:510px;height:330px;border:1px solid #e9e9e9;float:left;overflow:scroll;">       
                        <div class="overFile" id="over">
                        <i class="fas fa-file-upload h1 text-secondary"></i><span> <%=GetResource("AttachFileDrag") %></span>
                        </div>             
                        <table class="table table-striped">
                        <colgroup>
                            <col width="250px" />
                            <col width="80px" />
                            <col width="140px" />
                        </colgroup>
                        <tbody id="ulWidgets" class="files ui-sortable">
                        <tr>
                            <td><%=GetResource("FileName")%></td>
                            <td><%=GetResource("FileLength")%></td>
                            <td><%=GetResource("Upload")%></td>
                        </tr>
                        </tbody>
                        </table>

                    </div>
                    <div style="padding:5px 0 0 3px;width:100px;border:0px solid #ff0;float:left;">
                        <div class="fileupload-buttonbar">
                            <div class="span7">
                                <span class="btn btn-secondary fileinput-button" style="width:87px;">
                                    <span><%=GetResource("UploadTitleSelectButton")%></span>
                                    <input type="file" name="files[]" id="btnfileupload" multiple>
                                </span>
                                <br />
                                <button type="submit" class="btn start  btn-secondary" style="width:87px;">
                                    <span><%=GetResource("TotalUpload")%></span>
                                </button>
                                <br />
                                <button type="reset" class="btn cancel  btn-secondary" style="width:87px;display:none;">
                                    <span><%=GetResource("UploadCancel")%></span>
                                </button>
                            </div>
                            <span class="fileupload-loading"></span>
                        </div>
                    </div>
                    <div style="clear:both;border:0px solid #f9f;width:600px;">
                        <div class="span5 fileupload-progress fade">
                            <div class="progress progress-success progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                <div class="bar" style="width:0%;"></div>
                            </div>
                            <div class="progress-extended">&nbsp;</div>
                        </div>
                        <input type="hidden" name="p" value="<%=FilePathUser %>">
                    </div>   
                </div>
            </div>
        </div>
    </div>
</div>
</form>

<!-- The template to display files available for upload -->
<script>
    var fileSequence = 1;
</script>
<script id="template-upload" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-upload fade">
        <td class="name" style="width: 250px;word-break:normal;">
            <span>{%=file.name%}</span>
            {% if (file.error) { %}
                <div><span class="label label-important">Error</span> {%=file.error%}</div>
            {% } %}
        </td>
        <td class="size">
            <span>{%=o.formatFileSize(file.size)%}</span>
            {% if (!o.files.error) { %}
                <div class="progress progress-success progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="bar" style="width:0%;"></div></div>
            {% } %}
        </td>
        <td>
            {% if (!o.files.error && !i && !o.options.autoUpload) { %}
                <button class="btn start">
                    <%=GetResource("Upload")%>
                </button>
            {% } %}
            {% if (!i) { %}
                <button class="btn cancel">
                    <%=GetResource("Alert_Cancel")%>
                </button>
            {% } %}
        </td>
    </tr>
{% } %}
</script>
<!-- The template to display files available for download -->
<script id="template-download" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-download fade">
        <td class="name"><span>{%=file.name%}</span></td>
        <td class="size"><span>{%=o.formatFileSize(file.size)%}</span></td>
        <td>
        {% if (!file.error) { %}
        <%=GetResource("UploadComplete")%>
        {% } else { %}
            <div><span class="label label-important">Error</span> {%=file.error%}</div>
        {% } %}
        </td>
    </tr>
{% } %}
</script>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
<script type="text/javascript" src="js/vendor/jquery.ui.widget.js"></script>
<script type="text/javascript" src="js/tmpl.min.js"></script>
<script type="text/javascript" src="js/load-image.min.js"></script>
<script type="text/javascript" src="js/canvas-to-blob.min.js"></script>
<script type="text/javascript" src="js/bootstrap.min.js"></script>
<script type="text/javascript" src="js/blueimp-gallery.min.js"></script>
<script type="text/javascript" src="js/jquery.blueimp-gallery.min.js"></script>
<script type="text/javascript" src="js/jquery.iframe-transport.js"></script>
<script type="text/javascript" src="js/jquery.fileupload.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-process.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-image.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-audio.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-video.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-validate.js"></script>
<script type="text/javascript" src="js/jquery.fileupload-ui.js"></script>
<script type="text/javascript" src="js/main.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
</body>
</html>
