<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Address.List" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server" autocomplete="off" onsubmit="return List.SetSearch();">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="AddressList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span>
                            <i class="fas fa-address-book dropbtn "></i><%=GetResource("Address")%>
                        </span>
                        <div class="float-right text-primary">
                            <span class="dy" style="cursor: pointer"><%=GetResource("GoDirect")%>▼&nbsp;&nbsp;</span>
                        </div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock padding0">
                        <div class="ht">
                            <div class="nv1 margin0">
                                <ul>
                                    <li class="l1"><a href="#" id="gubunG"><span>
                                        <%=GetResource("Company")%></span></a></li>
                                    <li class="l1"><a href="#" id="gubunC"><span>
                                        <%=GetResource("Common")%></span></a></li>
                                    <li class="l1 "><a href="#" id="gubunP"><span>
                                        <%=GetResource("Private")%></span></a></li>
                                </ul>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="addisrc" name="addisrc">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <div class="search-container">
                                    <input type="text" id="tbSearchValue" class="keyword form-control" maxlength="80" runat="server"/>
                                    <button type="button" class="btn btn-xs btn-primary" id="search" style="float:none;">
                                        <%=GetResource("Search")%>
                                    </button>
                                </div>
                                <div class="float-right">
                                    <button type="button" class="btn btn-xs btn-primary" id="confirm">
                                        <span class="btnb"><%=GetResource("MailSendStr")%></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="addirec" name="addirec" style="display: none;">
                        <div class="full-width">
                            <div class="asc">
                                <ul class="abt">
                                    <li class="ft"><a href="#" class="btbg" onclick="List.LoadList('f1')" id="f1">ㄱ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f2')" id="f2">ㄴ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f3')" id="f3">ㄷ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f4')" id="f4">ㄹ</a></li>
                                    <li class="ft"><a href="#" class="btbg" onclick="List.LoadList('f5')" id="f5">ㅁ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f6')" id="f6">ㅂ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f7')" id="f7">ㅅ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f8')" id="f8">ㅇ</a></li>
                                    <li class="ft"><a href="#" class="btbg" onclick="List.LoadList('f9')" id="f9">ㅈ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f10')" id="f10">ㅊ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f11')" id="f11">ㅋ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f12')" id="f12">ㅌ</a></li>
                                    <li class="ft"><a href="#" class="btbg" onclick="List.LoadList('f13')" id="f13">ㅍ</a></li>
                                    <li><a href="#" class="btbg" onclick="List.LoadList('f14')" id="f14">ㅎ</a></li>
                                    <li><a href="#" class="btbg en" onclick="List.LoadList('f15')" id="f15">A-Z</a></li>
                                    <li><a href="#" class="btbg kr" onclick="List.LoadList('f16')" id="f16">0-9</a></li>
                                </ul>
                            </div>
                        </div>                        
                    </li>
                    <li class="contentblock padding0" id="phAddress" style="clear: both;">
                    </li>
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
                <ul class="mobliecontent ">
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
        var Gubun = "<%=gubun%>";
        var FirstChar = "<%=firstChar%>";
        var PageSize = "10", PageNo = "<%=pageNo%>";
        var Action = "<%=action%>";
        var MailID = "<%=mailID%>";
        var Code = "<%=code%>";
        var Mail_sn = "<%=mail_sn%>";
        var Hearder = "<%=hearder%>";
        var History = "<%=history%>";
        var SearchValue = "<%=searchValue%>";
        var FastPath = "<%=fastPath%>";
        var FileID = "<%=fileID%>";
        var CallPage = "<%=callPage%>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>";
        var FileCheck = "<%=FileCheck %>";
        var FolderID = "<%=folderID%>";
        var PublicGubun = "<%=publicGubun%>";        
        var ShareGubun = "<%=shareGubun%>";
        var Order = "<%=order%>";
        var R_GoDirect = "<%= GetResource("GoDirect") %>";
        var R_NoMail = "<%= GetResource("NoMail") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_NoSelList = "<%=GetResource("NoSelList")%>";   
        var R_InputAdd = "<%=GetResource("InputAdd")%>";  
        var R_ConfirmMail = "<%=GetResource("ConfirmMail")%>";  
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Address/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Address/List.js");
    </script>
</body>
</html>
