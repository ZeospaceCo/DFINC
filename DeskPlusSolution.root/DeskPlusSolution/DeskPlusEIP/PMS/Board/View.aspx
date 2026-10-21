<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Board.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=GetResource("ViewBoard")%></h1>

                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnListView" BeginEndTag="li" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnPrint" BeginEndTag="li" runat="server" />

                    <cc3:ClientSmartButton ID="cbtnReply" BeginEndTag="li" Class="btn btn-secondary" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" Class="btn btn-secondary" runat="server" />                    
                    <cc3:ClientSmartButton ID="cbtnMod" BeginEndTag="li" Class="btn btn-secondary" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" Class="btn btn-danger" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
				  <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                    <div id="toPrint">
                    <table class="table_typeB">
                        <caption>
                        </caption>
                        <colgroup>
                            <col width="100px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>
                                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                                </th>
                                <td>
                                    <asp:Label ID="lbTitle" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <%=GetResource("View_RegInfo")%>
                                </th>
                                <td>
                                    <asp:Literal ID="ltName" runat="server"></asp:Literal>
                                    :
                                    <asp:HyperLink ID="hlName" runat="server"></asp:HyperLink>
                                    &nbsp;/&nbsp;
                                    <asp:Literal ID="ltWriteDate" runat="server"></asp:Literal>
                                    :
                                    <asp:Label ID="lbWriteDate" runat="server"></asp:Label>
                                    &nbsp;/&nbsp;
                                    <asp:Literal ID="ltReadCount" runat="server"></asp:Literal>
                                    :
                                    <asp:Label ID="lbReadCount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="content_box"  style="overflow:auto;">
                    <table cellspacing="0" style="min-height:300px;width:100%;">
                        <tr>
                            <td valign="top">
                                <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    </div>
                    </div>
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"> 
                    <ul> 
                        <!--첨부파일-->
                        <li class="title"><%=GetResource("AttachName")%></li>  
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        <!--첨부파일-->

                        <li class="title"><asp:Literal ID="ltlShortReplyCount" runat="server"></asp:Literal>
							<cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                        </li>
                        <li class="reply"><div id="placeholder" style="clear: both;"></div></li>
                     </ul>
                 </div>           
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <!-- ================================== Hidden Input ===================================== -->
        <asp:TextBox ID="tbMailID" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tbAttachInfo" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tbSubject" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tbBody" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tbServer" runat="server" Width="0" Style="display: none"></asp:TextBox>
        <!-- ==================================================================================== -->
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank" title=""></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/  
        var ProjectID           = "<%=ProjectID %>";        
        var MenuID              = "<%=MenuID %>";  
        var MenuType            = "<%=MenuType %>";          
        var RqProjectID = "<%=RqProjectID %>"; 
        var BoardID       = "<%=BoardID %>";
        var ProjParam           = "<%=ProjParam %>"; 
        var Popup = "0";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchTerm = "<%=SearchTerm %>";
		var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var PageName = "<%= System.IO.Path.GetFileName(Request.Path) %>";
		var GoUrl               = "<%=GoUrl%>";
		var AttachFilesInfo     = "<%=AttachFilesInfo %>";			
		var BoardListViewUrl    = "<%=BoardListViewUrl%>";
		var GroupID             = "<%=GroupID%>";
		var ListOrder           = "<%=ListOrder%>";
		var Depth               = "<%=Depth%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";	
        var FullDomainPath = "<%=FullDomainPath %>";
        var DeleteMsg = "<%=GetResource("ApplyDelete") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Schedule_Header3") %>";
        var R_RegDate = "<%=GetResource("Menu_Header4") %>";
        var R_ApplyDelete = "<%=GetResource("MSG_CONFIRM_DEL") %>";
        var R_ReallyApplyDelete = "<%=GetResource("DeleteReal") %>";
        var R_Del = "<%=GetResource("Unit_Register_Delete") %>";
        var R_Mod = "<%=GetResource("Mod") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Unit_Register_Delete") %>";
        var R_FileDown = "<%=GetResource("FileDown") %>";
        var R_Board_List_Name = "<%=GetResource("Board_List_Name") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Board/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Board/View.js");
    </script>
</body>
</html>
