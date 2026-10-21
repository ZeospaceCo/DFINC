<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Upload.Standard.Main" %>

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
        var guid = "<%=GUID %>";
        var callBackEvent = "<%=CallBackEvent %>";
        var PageUrl = "<%= Request.Url.ToString() %>";        
        var slCtl = null;

        var baseHttp = "<%=BaseHttp%>";
        var fullDomain = "<%=FullDomain%>";
        var uploadHost = baseHttp + fullDomain;
        var uploadPort = "<%=BaseHttpPort%>";
        var uploadURL = "/DeskPlusEIP/Common/Upload/UploadFileAttach.ashx";
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

        var R_ExistLimitSize = "<%=GetResource("ExistLimitSize") %>";
        var R_ExistPrivateTotalLimitSize = "<%=GetResource("ExistPrivateTotalLimitSize") %>";
        var R_NoUploadOver = "<%=GetResource("NoUploadOver") %>";
        var R_RootFolder = "<%=GetResource("UploadRootFolder") %>";
        var R_MyDocument = "<%=GetResource("UploadMyDocument") %>";
        var R_MyComputer = "<%=GetResource("UploadMyComputer") %>";

        var R_UploadName = "<%=GetResource("UploadName") %>";
        var R_UploadSize = "<%=GetResource("UploadSize") %>";
        var R_UploadType = "<%=GetResource("UploadType") %>";
        var R_UploadDate = "<%=GetResource("UploadDate") %>";
        var R_UploadTitleSelected = "<%=GetResource("UploadTitleSelected") %>";
        var R_UploadTitleListHelp = "<%=GetResource("UploadTitleListHelp") %>";
        var R_UploadTitleSelectButton = "<%=GetResource("UploadTitleSelectButton") %>";
        var R_UploadSearchButton = "<%=GetResource("UploadSearchButton") %>";
        var R_UploadTitleFolder = "<%=GetResource("UploadTitleFolder") %>";
        var R_UploadPostFilePath = "<%=GetResource("UploadPostFilePath") %>";

        //업로드시 추가 정보
        var uploadETC = "";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Upload/Standard/Upload.js");
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
                        <li><a id="DPUpload" class="tab_on"><span>ActiveX</span> </a></li>
                        <li><a id="RIAUpload"><span>NonActiveX</span> </a></li>
                        <li><a id="FolderUpload"><span>WebHard</span> </a></li>
                        <li><a id="NETUpload"><span>Standard</span> </a></li>
                    </ul>
                    <div id="tabs-1">
                        <script src="/DeskPlusEIP/Html/Js/IEActiveX/activexDPUploadFileAttach.js" type="text/javascript"></script>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- //layout_info -->
    </form>
</body>
</html>
