<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Temp.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%= GetResource("SaveTemp")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnMove" runat="server"></cc3:ClientSmartButton></li>

                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        
        <div id="layout_content">        
            <div class="row">
				<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                    <!-- print start -->
                    <div id="toPrint">
                    <table cellspacing="0" class="table_typeB">
		            <colgroup>
			            <col width="80px" />
                        <col />
		            </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("Title") %></th>
                            <td><asp:Label ID="lbTitle" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="last-child"><%=GetResource("View_RegInfo") %></th>
                            <td class="last-child"><%=GetResource("Register")%>:<asp:Label ID="lbWriteInfo" runat="server"></asp:Label>
                                &nbsp;/&nbsp;
                                <%=GetResource("RegDate")%>
                                :
                                <asp:Label ID="lbWriteDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                    </table>

                    <!-- 본문정보 -->
                    <div class="content_box"  style="overflow:auto;">
                        <table cellspacing="0" style="min-height:300px;width:100%;">
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="lbContent" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- 본문정보 -->
                    </div>
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"> 

                    <ul> 
                        <!--첨부파일-->
                        <li class="title"><%=GetResource("AttachName")%></li>                      
        
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
						<!--첨부파일-->
                     </ul>                    
                                       
	             </div><!-- end col content-area-body-right --> 
                    
			    </div><!-- end row  -->          
		    </div><!-- end layout_content  -->
        
	    </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->

    <div style="display: none;">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%= ListID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var GoPage = "<%=GoPage %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var Option_Reple = ""; //리플 사용여부
        var OptionShowViewer = ""; //조회자사용여부        
        var WindowObjectReference; //새창용
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Temp/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Temp/View.js");
    </script>
</body>
</html>
