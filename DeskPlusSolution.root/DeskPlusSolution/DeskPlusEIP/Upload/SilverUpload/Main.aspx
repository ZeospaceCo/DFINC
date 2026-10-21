<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Upload.SilverUpload.Main" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript">
        var R_DelMsg = "삭제하시겠습니까?";
        var guid = "<%=GUID %>";
        var fileSizeLimit = "<%=FileTotalLimitSize %>"; //총 upload 사이즈 제한 (KB)
        var fileUploadLimit = "<%=FileCount %>"; //업로드 개수
        var fileType = "<%=FileType %>";
        var fileTypeDesc = "<%=FileTypeDesc %>";
        var callBackEvent = "<%=CallBackEvent %>";
        var companyID = "<%=CompanyID %>";
        var userID = "<%=UserID %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var slCtl = null;

        var ImagePath = "<%=ImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_ExistPrivateTotalLimitSize = "<%=GetResource("ExistPrivateTotalLimitSize") %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Upload/SilverUpload/Upload.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Upload")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnApply" runat="server"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3"  style="min-height:430px;height:455px;">
                <div id="tabs">
                    <ul id="tab" class="tab">
                        <li><a id="DPUpload"><span>ActiveX</span> </a></li>
                        <li><a id="RIAUpload" class="tab_on"><span>NonActiveX</span> </a></li>
                        <li><a id="FolderUpload"><span>WebHard</span> </a></li>
                    </ul>
                    <div id="tabs-2">
                        <div id="silverlightControlHost" >
                            <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2"
                                width="98%" height="420px">
                                <param name="source" value="ClientBin/mpost.SilverlightMultiFileUpload.ko.xap" />
                                <param name="onerror" value="onSilverlightError" />
                                <param name="initParams" value="MaxFileSizeKB=<%=FileTotalLimitSize %>,MaxUploads=2,FileFilter=,ChunkSize=4194304,CustomParams=yourparameters,DefaultColor=White" />
                                <param name="background" value="white" />
                                <param name="onload" value="pluginLoaded" />
                                <param name="minRuntimeVersion" value="4.0.50401.0" />
                                <param name="autoUpgrade" value="true" />
                                <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration: none">
                                    <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                                        style="border-style: none" />
                                </a>
                            </object>
                            <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- //layout_info -->
    </form>
</body>
</html>
