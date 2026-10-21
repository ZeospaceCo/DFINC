<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Temp.Mod" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("SaveTemp")%> <%=GetResource("Modify")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                   <div class="col-12 col-lg-9 col-xl-8 content-area-body-left">
                
                        <table cellspacing="0" class="table_typeB">
                            <colgroup>
			                    <col width="80px" />
                                <col />
		                    </colgroup>
                            <tr>
                                <th>
                                    <%=GetResource("Title")%>
                                </th>
                                <td>
                                    <cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server"
                                        Required="true" ScriptSubmitFunName="Mod.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="last-child">
                                    <%=GetResource("Gubun")%>
                                </th>
                                <td class="last-child">
                                    <select name="ddlFontColor" id="ddlFontColor" runat="server">
                                    </select>
                                    <select name="ddlFontSize" id="ddlFontSize" runat="server">
                                    </select>
                                    <input id="cbxBold" type="checkbox" name="cbxBold" runat="server" /><label for="cbxBold"><%=GetResource("Bold") %></label>
                                    <input id="cbOptionModify" type="checkbox" name="cbOptionModify" runat="server" /><label
                                        for="cbOptionModify"><%=GetResource("ModifyEveryone")%></label>
                                </td>
                            </tr>
                        </table>
                        <div id="tabs" class="layout_subbox">
                           <ul id="tab" class="tab" style="display:none">
                                <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                                <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                            </ul>
                            <div class="divNonEditor">                    
                                <div id="divXFE" style="height:5px;">  
                                    <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>                       
                                    <script type="text/javascript">
                                        xfe = new XFE({
                                            basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                            width: '100%',
                                            height: '550px',
                                            ignoreMinHeight: true
                                        });
                                        xfe.render('divXFE');
                                    </script>
                                </div>
                            </div>
                        </div> 

                    </div><!-- end col content-area-body-right -->
                    <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">              

                        <ul> 
                            <!-- 첨부파일 -->
                            <li class="title">
                                 <%=GetResource("AttachName")%>
                            </li>
                            <li>
                                <!-- 첨부파일 리스트 -->
                                <div id="placeholder_fileList" style="clear: both;"></div>
                                <!-- 첨부파일 리스트 -->
                            </li>
                         </ul>
                         <ul>
                            <!-- 첨부파일 -->
                            <li class="title">
                                  <%=GetResource("Additional")%> <%=GetResource("AttachName")%>
                                  <div class="float-right">   
                                     <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                                  </div>
                            </li>
                            <li>
                                <!-- 첨부파일 리스트 -->
                                <div id="placeholder_file" style="clear: both;"></div>
                                <!-- 첨부파일 리스트 -->
                            </li>                           
                         </ul>                          
                      </div><!-- end col content-area-body-right -->
                    
			    </div><!-- end row  -->
		    </div><!-- end layout_content  -->
        
	    </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
     <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbHiddenSelectedContent" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" TextMode="MultiLine" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"></textarea>
        <!--  파일 첨부시 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>
    </div>
    <!-- ==================================================================================== -->
    </form>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">        
        var CategoryID = "<%=CategoryID %>"; //선택한 트리 카테고리
        var ListID = "<%=ListID %>"; //리스트아이디   
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort        
        var ScreenShot = "<%=ScreenShot %>";
        var GoPage = "<%=GoPage %>";  
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var Add_InputTitle = "<%= GetResource("Add_InputTitle") %>";
        var Add_SelectOptionTop = "<%= GetResource("Add_SelectOptionTop") %>";
        var Add_InputContent = "<%= GetResource("Add_InputContent") %>";
        var DelAttach = "<%= GetResource("DelAttach") %>";
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_NotImageFormat = "<%=GetResource("NotImageFormat") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_ScreenShot = "<%=GetResource("ScreenShot") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID   
        var FileSizeLimit = "<%=AttachLimitSize %>"; //upload 사이즈 제한 kb
        var FileUploadLimit = 0; //업로드 개수
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Temp/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Temp/Mod.js");
    </script>
    <script type="text/javascript">
        setTimeout(function () {
            Mod.GetAttachFileList();
            Mod.GetSpoolFileList();
        }, 200);
    </script>
</body>
</html>
