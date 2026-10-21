<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPT.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.ViewPT" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
    <style type="text/css">
        .list_add_style1
        {
            clear: both;
            position: relative;
            overflow: hidden;
            height: 25px;
            margin-bottom: 5px;
        }
        .date
        {
            position: absolute;
            display: inline-block;
            left: 48%;
            overflow: hidden;
            width: 300px;
            margin: 0 0 0 -100px;
        }
        .date .b_prev
        {
            float: left;
            margin-top: 1px;
            margin-right: 10px;
        }
        .date span
        {
            float: left;
            font-family: "맑은 고딕" , "돋움" ,Dotum, "굴림" ,Gulim,AppleGothic,Sans-serif;
            font-weight: bold;
            font-size: 1.5em;
            color: #258ee9;
        }
        .date .b_next
        {
            float: left;
            margin-top: 1px;
            margin-left: 10px;
        }
        
        .table_typeC td
        {           
            word-break:break-all;
        }        
    </style>
</head>
<body>
    <form id="form1" runat="server">
                <div id="layout_body">
                    <div class="layout_full">
                        <!-- START HEADER -->
                        <div id="layout_header">
                            <!-- 페이지타이틀 //-->
                            <h1><%=GetResource("ViewPT")%></h1>
                            <!-- 버튼 //-->
                            <ul class="btn_left">
                                
                            </ul>
                            <ul class="btn_right">
                                <li><asp:DropDownList ID="ddlFolder" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                                <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                                <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                            </ul>
                        </div>
                        <div class="list_add_style1">
                            <div class="date">
                                <a class="b_prev" id="pvWeek"><i class="fas fa-angle-left"></i></a> 
                                <span id="TimeList" runat="server"></span>
                                <span>~ </span>
                                <span id="EtimeList" runat="server"></span>
                                <a class="b_next" id="nxWeek"><i class="fas fa-angle-right"></i></a>
                            </div>
                        </div>
                        <div id="layout_content">
                            <div  id="toPrint" class="row">
                	        <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!--  -->
                                <!-- 테이블 시작 //-->
                                <table cellspacing="0" class="table_typeC">
                                    <colgroup>
                                        <col width="15%" />
                                        <col width="42%" />
                                        <col width="42%" />
                                    </colgroup>
                                    <tbody>
                                        <tr>
                                            <th><%=GetResource("Gubun")%></th>
                                            <th><asp:Label ID="lblThisWeek" runat="server"></asp:Label></th>
                                            <th><asp:Label ID="lblNextWeek" runat="server"></asp:Label></th>
                                        </tr>
                                        <tr>
                                            <th><%=GetResource("WorkContent")%></th>
                                            <td valign="top">
                                                <asp:Label title="" ID="lblThisContent" runat="server"></asp:Label>
                                            </td>
                                            <td valign="top">
                                                <asp:Label title="" ID="lblNextContent" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th><%=GetResource("IssueWork")%></th>
                                            <td valign="top" colspan="2">
                                                <asp:Label title="" ID="lblIssueContent" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                   
                                </div><!-- width 100%(col-12) -->
                                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-12) -->
                                <ul> 
                                <!--첨부파일-->
                                <li class="title">
                                    <div class="float-left">
                                        <%=GetResource("Attach")%></h2>
                                    </div>
                                </li>
                                <li><div id="placeholder_file" style="clear: both;"></div></li>
                                
                                <!--짧은댓글-->
                                <li class="title">
                                    <div class="float-left">
                                        <%=GetResource("Short")%>
                                    </div>
                                    <div class="float-right">
                                        <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary" />
                                    </div>
                                </li>
                                
                                <li class="reply"><div id="placeholder" style="clear: both;"></div></li>
                               </ul>         
                            </div><!-- end col-3 content-area-body-right -->
                        </div>
                    
                    </div>
                    </div>
                    <!-- 테이블 끝 //-->
                </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnClick" runat="server" OnClick="lbtnClick_Click"></asp:LinkButton>        
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">    </div>
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var ListID = "<%=ListID%>";
        var BeforeStartDate = "<%=BeforeStartDate%>";
        var NextStartDate = "<%=NextStartDate%>";
        var StartDate = "<%=StartDate%>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDown") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>";        
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/ViewPT.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Report/Js/Content/ViewPT.js");
    </script>
</body>
</html>
