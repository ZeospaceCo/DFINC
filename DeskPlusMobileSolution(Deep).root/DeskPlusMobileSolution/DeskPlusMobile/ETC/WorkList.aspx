<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkList.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.ETC.WorkList" %>
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
        <div style="min-height:330px;">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent" >
                <li class="poptitle text-primary">
                    <span>
                        <i class="fas dropbtn "></i>특근내역
                    </span>
                    <div class="float-right date text-dark">
                        <button type="button" class="btn btn-xs btn-primary" id="btConfirm" style="float:none;"><%=GetResource("ConfirmZR")%></button>
                        <button type="button" class="btn btn-xs btn-primary" id="btClose" style="float:none;"><%=GetResource("Close")%></button>
                    </div>
                </li> 
                </ul>
                <ul class="mobilecontent">
                    <li class="contentblock">  
                        <div class="container-fluid"> 
                            <span><%=GetResource("Target")%> : </span>
                            <span id="TargetDocNumber"></span>
                        </div>                       
                    </li>                             
                </ul>
                <ul class="mobliecontent" id="phList" style="height:232px;overflow-y:scroll" >
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
             </div>
          </div>
    </div>
    <div style="display: none">
        <input id="hdWorkID" type="hidden" name="hdnUserID" runat="server">
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
        var TotalItemCount = "";

        var R_NoContent = "<%=GetResource("NoContent") %>";
 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/ETC/WorkList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/ETC/WorkList.js");
    </script>
</body>
</html>
