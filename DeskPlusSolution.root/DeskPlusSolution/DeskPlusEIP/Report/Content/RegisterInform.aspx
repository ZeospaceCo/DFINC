<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterInform.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.RegisterInform" %>


<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
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
</head>
<body>
    <form id="form1" runat="server">
	<div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1> <%=GetResource("InformBox")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">     
                    <cc3:ClientSmartButton ID="cbtnPT" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnBringNow" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnBringNext" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnBringReporter" runat="server" BeginEndTag="li" />               
                    <li><cc3:ClientSmartButton ID="cbtnReport" runat="server" class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>
                </ul>
            </div>
            <div id="layout_content">
              <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!--  -->
                <!-- 테이블 시작 //-->
                <table cellspacing="0" class="table_typeC">
                    <colgroup>
                        <col width="10%" />
                        <col width="45%" />
                        <col width="45%" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th rowspan="2">
                                <%=GetResource("Gubun")%>
                            </th>
                            <th colspan="2">
                                <%=GetResource("InformSystem")%> : <asp:DropDownList ID="ddlFolder" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            </th>
                        </tr>
                        <tr>                            
                            <th>
                                <asp:Label ID="lblThisWeek" runat="server"></asp:Label>
                            </th>
                            <th>
                                <asp:Label ID="lblNextWeek" runat="server"></asp:Label>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("WorkContent")%>
                            </th>
                            <td>
                                <textarea id="tbthisWeek" name="tbthisWeek" runat="server" style="min-height:350px;"></textarea>
                            </td>
                            <td>
                                <textarea id="tbNextWeek" name="tbNextWeek" runat="server" style="min-height:350px;"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("IssueWork")%>
                            </th>
                            <td colspan="2">
                                <textarea id="tbIssue" name="tbIssue" runat="server"></textarea>
                            </td>
                        </tr>
                    </tbody>
                 </table>

                 </div><!-- end col content-area-body-right -->                                
                 <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!--  -->
                    
                     <ul>       
                        <!-- 첨부파일 -->
                         <li class="title">
                                <%=GetResource("Attach")%>
                         </li>                
                         <li><div id="placeholder_fileList" style="clear: both;"></div></li>
                
                         <li class="title">
                            <div class="float-left">
                                <%=GetResource("AddAttach")%>
                            </div>
                            <div class="float-right">                            
                                <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                         </li>
                         <li><div id="placeholder_file" style="clear: both;"></div></li>
                    
                
                        <!-- 첨부파일 -->
                        <li class="title">
                            <div class="float-left">
                                <%=GetResource("Short")%>
                            </div>
                           <div class="float-right">
                                <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary"/>
                            </div>
                        </li>
                
                        <li class='reply'><div id="placeholder" style="clear: both;"></div></li>
                    </ul>
                </div>
           </div><!-- end col content-area-body-right -->
        </div>
        <!-- 테이블 끝 //-->
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <asp:RadioButton ID="rbOpen" GroupName="rbkind" runat="server" Checked="true" />&nbsp;<%=GetResource("WorkOpen")%>
        <asp:RadioButton ID="rbClose" GroupName="rbkind" runat="server" />&nbsp;<%=GetResource("WorkClose")%>
        <!--  파일 첨부시 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
        <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank"
            frameborder="0"></iframe>
        <!--  파일 첨부시 필요한 필드 //-->
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
    <script type="text/javascript">
        var Years = "<%=Years %>";
        var Months = "<%=Months %>";
        var Days = "<%=Days %>";
        var TargetUserID = "<%=TargetUserID %>";
        var TargetDeptID = "<%=TargetDeptID %>";
        var Weeks = "<%=Weeks %>";
        var Flag = "<%=Flag %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var ListType = "<%=ListType %>";
        var ListID = "<%=ListID %>";
        var Order = "";
        var NowStartDate = "<%=NowStartDate %>";;//금주업무 시작일
        var NextStartDate = "<%=NextStartDate %>";; //다음주 업무 시작일
        var NowNext = "";//금주,차주 구분
        var StartDate = "<%=StartDate %>";;//금주업무 시작일
        var EndDate = "<%=EndDate %>";; //금주업무 종료일
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
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_ThisWeek = "<%=GetResource("ThisWeek") %>";
        var R_NextWeek = "<%=GetResource("NextWeek") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_DelAttach = "<%=GetResource("DelAttach") %>";
        var R_ShallAddNowJob = "<%=GetResource("ShallAddNowJob") %>";
        var R_ShallAddNextJob = "<%=GetResource("ShallAddNextJob") %>";
        var R_CheckAuthUser = "<%=GetResource("CheckAuthUser") %>";
        var R_SelInformSystem = "<%=GetResource("SelInformSystem") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID
        var FileSizeLimit = "<%=AttachLimitSize %>"; //upload 사이즈 제한 kb
        var FileUploadLimit = 0; //업로드 개수
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/RegisterInform.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Report/Js/Content/RegisterInform.js");
    </script>
</body>
</html>
