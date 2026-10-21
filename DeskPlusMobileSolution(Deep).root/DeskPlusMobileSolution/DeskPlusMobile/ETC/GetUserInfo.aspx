<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserInfo.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.ETC.GetUserInfo" %>
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
    <form id="form1" runat="server" onsubmit="return GetUserInfo.SetSearch();">
    <div class="layer_popup">
        <div style="min-height:220px;">
            <div class="introcontentblock bottomcontent" >
                <ul class="mobliecontent" >
                    <li class="poptitle text-primary">
                        <span>
                            <i class="fas dropbtn "></i><%=GetResource("Target")%>
                        </span>
                        <div class="float-right date text-dark">
                            <button type="button" class="btn btn-xs btn-primary" id="btClean"><%=GetResource("DelPerson")%></button>
                            <button type="button" class="btn btn-xs btn-primary" id="btConfirm"><%=GetResource("ConfirmZR")%></button>
                            <button type="button" class="btn btn-xs btn-primary" id="btClose"><%=GetResource("Close")%></button>
                        </div>
                    </li> 
                </ul> 
                <ul class="mobilecontent">
                    <li class="contentblock">  
                        <div class="container-fluid"> 
                            <span><%=GetResource("Target")%> : </span>
                            <span id="ReceivePerson"></span>
                        </div>
                        <div class="container-fluid">   
                            <div class="float-left">
                                <div class="search-container float-right">
                                    <input type="text" id="tbSearch" class="keyword form-control" maxlength="100" runat="server" />
                                    <button type="button" class="btn btn-xs btn-primary" id="search" style="float:none;">
                                        <%=GetResource("Search")%>
                                    </button>
                                </div>
                            </div>                                
                        </div>
                        <div class="container-fluid">  
                                <div id="lbUser" class="form-control" style="height:80px;width:250px;overflow-y:scroll"></div>      
                        </div>  
                    </li>                             
                </ul>
             </div>
        </div>
    </div>
    <div style="display: none">
        <input id="hdnUserID" type="hidden" name="hdnUserID" runat="server">
        <input id="hdnDeptID" type="hidden" name="hdnDeptID" runat="server">
        <input id="hdnDeptName" type="hidden" name="hdnDeptName" runat="server">
    </div>
</form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var R_TagDic_UPNM = "<%=GetResource("TagDic_UPNM") %>";
        var R_MsgPerson = "<%=GetResource("MsgPerson") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/ETC/GetUserInfo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/ETC/GetUserInfo.js");
    </script>
</body>
</html>
