<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetBaseDoc.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.GetBaseDoc" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div style="min-height:597px;">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent" >
                <li class="poptitle text-primary">
                    <span>
                        <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("BaseDoc")%>
                    </span>
                    <div class="float-right date text-dark">
                        <button type="button" class="btn btn-xs btn-primary" id="btConfirm" style="float:none;"><%=GetResource("Add")%></button>
                        <button type="button" class="btn btn-xs btn-primary" id="btClose" style="float:none;"><%=GetResource("Close")%></button>
                    </div>
                </li> 
                </ul>
                <ul class="mobliecontent" style="width:100%;height:85px;overflow-y:scroll" >
                    <li class="contentblock" style="background-color:#eeeeee" >
                        <div class="title">
                        <input type="radio" id="rdFolderPublic" name="rdFolder" class="rd" value="Dept" checked /><label for=""><%=GetResource("PublicBase")%></label>
                        <input type="radio" id="rdFolderPrivate" name="rdFolder" class="rd" value="Private" /><label for=""><%=GetResource("PrivateBase")%></label>                            
                        <input type="radio" id="rdFolderApproval" name="rdFolder" class="rd" value="Approval" /><label for=""><%=GetResource("ApprovalBase")%></label>                            
                        <input type="radio" id="rdFolderArbitrary" name="rdFolder" class="rd" value="Arbitrary" /><label for=""><%=GetResource("ArbitraryBase")%></label>                            
                        <input type="radio" id="rdFolderCarboncopy" name="rdFolder" class="rd" value="Carboncopy" /><label for=""><%=GetResource("CarboncopyBase")%></label>                            
                        <input type="radio" id="rdFolderForward" name="rdFolder" class="rd" value="Forward" /><label for=""><%=GetResource("ForwardBase")%></label>
                        <input type="radio" id="rdFolderReport" name="rdFolder" class="rd" value="Report" /><label for=""><%=GetResource("Report")%></label>
                        </div> 
                    </li>
                </ul>
                <ul class="mobliecontent" id="phApproval" style="height:350px;overflow-y:scroll" >
                </ul>  
                <!-- page -->
                <ul class="mobliecontent">
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
                                        <span class="bg-light text-black" id="PageNum"></span> / <span class="bg-light text-black" id="TotalNum"></span>
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
                 <!-- //page -->  
                 <ul class="mobliecontent" id="baseList" style="height:120px;overflow-y:scroll" >
                 </ul>            
             </div>
          </div>
    </div>

    <div style="display:none">
        <input name="tbAddDocID" type="text" id="tbAddDocID" value="" size="150"> 
    </div>  
</form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var PageSize = "5", PageNo = "1";
        var FolderName = "";
        var RowMax = 0;
        var BaseDocIDs = "<%=BaseDocIDs %>"; //부모창에 추가된 documentid들
        var BaseDocPopUpIDs = ""; //팝업창에서 추가한 documentid들

        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Msg_ErrBaseDoc = "<%=GetResource("Msg_ErrBaseDoc") %>";
        var R_Msg_ErrBaseDocList = "<%=GetResource("Msg_ErrBaseDocList") %>";
        var R_Msg_ErrBaseSameDoc = "<%=GetResource("Msg_ErrBaseSameDoc") %>";
        var R_PublicBase  = "<%=GetResource("PublicBase") %>";
        var R_PrivateBase = "<%=GetResource("PrivateBase") %>";
        var R_ApprovalBase  = "<%=GetResource("ApprovalBase") %>";
        var R_ArbitraryBase = "<%=GetResource("ArbitraryBase") %>";
        var R_CarboncopyBase  = "<%=GetResource("CarboncopyBase") %>";
        var R_ForwardBase  = "<%=GetResource("ForwardBase") %>";
        var R_Report = "<%=GetResource("Report") %>";
        var R_NoSelList = "<%=GetResource("NoSelList") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/GetBaseDoc.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/GetBaseDoc.js");
    </script>
</body>
</html>
