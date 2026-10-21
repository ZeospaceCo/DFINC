<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.WebHard.List" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server" name="form1" action="/DeskPlusMobile/Common/Upload/UploadAttach.ashx"
    method="POST" enctype="multipart/form-data">
    <input type="text" name="folderID" value="<%=folderID%>" style="display: none" />
    <input type="text" name="userID" value="<%=UserID%>" style="display: none" />
    <input type="text" name="companyID" value="<%=CompanyID%>" style="display: none" />
    <input type="text" name="publicGubun" value="<%=publicGubun%>" style="display: none" />
    <div id="divprg" align="center" class="loadingH">
    </div>
    <div data-role="page" id="MailList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-folder-open dropbtn "></i>
                            <%=FolderTitle%>
                        </span>
                        <div class="float-right date text-dark">
                            <asp:DropDownList ID="ddlOrder" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" id="btMain" runat="server">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <button type="button" class="btn btn-lg btn-success" id="btAll" runat="server">
                                    <span class="btnb" id="allcheck"><%=GetResource("AllSelect")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-danger" id="btDel" runat="server">
                                    <span class="btnr"><%=GetResource("Del")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btSend" runat="server">
                                    <span class="btnr"><%=GetResource("MailSend")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAddFile" runat="server">
                                    <span class="btnb"><%=GetResource("SaveFile")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btPCAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <input type="file" name="upload" id="upload" onchange="fileChange(this);" style="display:none;" />
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent " id="phWebHard" style="clear: both;">
                </ul>
                <ul class="mobliecontent ">
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="float-right">
                                <a href="#" class="ubtst">▲<%=GetResource("Top")%></a>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent " id="PageNumMain" runat="server">
                    <li class="contentblock">
                        <nav aria-label="Page navigation  row">
                            <ul class="pagination date left justify-content-left">
                                <li class="page-item">
                                    <a class="page-link" href="#" id="pvNum">
                                        <span><%=GetResource("BeforePage")%></span>
                                    </a>
                                </li>
                                <li class="page-item">
                                    <a class="page-link">
                                        <span class="bg-light text-black" id="PageNum" runat="server"></span> / <span class="bg-light text-black" id="TotalNum" runat="server"></span>
                                    </a>
                                </li>
                                <li class="page-item">
                                    <a class="page-link" href="#" id="nxNum">
                                        <span><%=GetResource("NextPage")%></span>
                                    </a>
                                </li>
                            </ul>
                        </nav>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">           
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var PrivateQuota = "<%=PrivateQuota %>";
        var UseCount = "<%=UseCount %>";
        var FileLength = "<%=FileLength%>";
        var ParentFolderID = "<%=parentFolderID%>";
        var FolderID = "<%=folderID%>";
        var ShareGubun = "<%=shareGubun%>";
        var PublicGubun = "<%=publicGubun%>";        
        var DownCheck = "<%=downCheck%>";    
        var UploadCheck = "<%=uploadCheck%>";     
        var PageSize = "10", PageNo = "<%=pageNo%>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var UUID = "<%=this.UUID%>"; 
        var AttachFileID = "<%=fileID %>";
        var BlockExt = "<%=blockExt %>"; 
        var UpFileSize = "<%=upFileSize %>"; 

        var R_DelNoFile = "<%=GetResource("DelNoFile") %>";  
        var R_DelList = "<%=GetResource("DelList") %>";       
        var R_SelAll = "<%=GetResource("SelAll") %>";
        var R_SelNoAll = "<%=GetResource("SelNoAll") %>";  
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";  
        var R_DownloadAuthority = "<%=GetResource("DownloadAuthority") %>";   
        var R_SendNoFile = "<%=GetResource("SendNoFile") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_UploadCount = "<%=GetResource("UploadCount") %>";
        var R_MaxFile = "<%=GetResource("MaxFile") %>";
        var R_CheckFileType = "<%=GetResource("CheckFileType") %>";               
        var R_DeleteAuthority = "<%=GetResource("DeleteAuthority") %>";     
        
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/WebHard/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/WebHard/List.js");
    </script>
</body>
</html>
