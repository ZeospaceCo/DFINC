<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Board.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/> 
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=GetResource("Mod")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >
                        <table class="table_typeB">
                            <caption>
                            </caption>
                            <colgroup>
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th class="last-child"><%=GetResource("Title")%>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbTitle" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <!--tr>
                                    <th>구분</th>
                                    <td>
                                    <select name="ddlCatHeader" id="ddlCatHeader"  runat="server">
                                    </select>
                                    <select name="ddlFontColor" id="ddlFontColor"  runat="server">
                                    </select>
                                    <select name="ddlFontSize" id="ddlFontSize"  runat="server">
                                    </select>
                                    <input id="cbxBold" type="checkbox" name="cbxBold" runat="server" /><label for="cbxBold">굵게</label>            
                                    <input id="cbOptionModify" type="checkbox" name="cbOptionModify" runat="server" /><label for="cbOptionModify">타인수정가능</label>                        
                                    <input id="cbOptionTop" type="checkbox" name="cbOptionTop"   runat="server" /><label for="cbOptionTop">공지사항등록</label>
                                    <select name="ddlDay" id="ddlDay" style="display:none"  runat="server">
	                                    <option value="">---</option>
	                                    <option value="5">5</option>
	                                    <option value="10">10</option>
	                                    <option value="15">15</option>
	                                    <option value="20">20</option>
                                        <option value="25">25</option>
                                        <option value="30">30</option>
                                        <option value="35">35</option>
                                        <option value="40">40</option>
                                        <option value="45">45</option>
                                        <option value="50">50</option>
                                        <option value="55">55</option>
                                        <option value="60">60</option>
                                        <option value="65">65</option>
                                        <option value="70">70</option>
                                        <option value="75">75</option>
                                        <option value="80">80</option>
                                        <option value="85">85</option>
                                        <option value="90">90</option>
                                        <option value="95">95</option>
                                        <option value="100">100</option>
                                    </select>            
                                    <asp:CheckBox ID="cbOptionAnonymity" runat="server" />
                                    </td>
                                </tr-->
                            </tbody>
                        </table>
                        <div id="tabs" class="layout_subbox">
                            <ul id="tab" class="tab">
                                <li><a id="ActvieX" class="tab_on"><span>ActiveX</span> </a></li>
                                <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                            </ul>
                            <div class="divEditor">
                                <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                            </div>
                        </div>
                   </div><!-- end col content-area-body-right -->
                   <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">     
                        <ul> 
                            <!-- 첨부파일 -->
                            <li class="title">
                                <%=GetResource("AttachFile")%>                                
                            </li>
                            <li><div id="phFileList" style="clear: both;"></div></li>  
                        </ul>  
                        <ul>
                            <!-- 첨부파일 -->
                            <li class="title">
                                  <%=GetResource("AddAttachFile")%>
                                  <div class="float-right">   
                                     <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("FileAdd")%>"><i class="fas fa-plus-circle text-secondary"></i></a> 
                                  </div>
                            </li>
                            <li><div id="phFile" style="clear: both;"></div></li>
                         </ul>  
                       </div>
                    </div>
            </div>
        </div>
    </div>
   <!-- 첨부파일 리스트 -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CommunityID = "<%=CommunityID %>";
        var MenuID = "<%=MenuID %>";
        var BoardID = "<%=BoardID %>";
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>"; //정렬
        var ListType = "<%=ListType %>";
        var ScreenShot = "<%=ScreenShot %>";
        var Grade = "<%=Grade %>"; 
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var R_EditorScript = "<%=GetResource("EditorScript") %>";
        var R_InputTitle = "<%=GetResource("InputTitle") %>";
        var R_InputContent = "<%=GetResource("InputContent") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_DelAttach = "<%=GetResource("DelAttach") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileDel = "<%=GetResource("FileDel") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_NotImageFormat = "<%=GetResource("NotImageFormat") %>";
        var R_ScreenShot = "<%=GetResource("ScreenShot") %>";
        var R_SelScreenShot = "<%=GetResource("SelScreenShot") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID   
        var FileSizeLimit = "<%=AttachLimitSize %>"; //upload 사이즈 제한 kb
        var FileUploadLimit = 0; //업로드 개수
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";        
        var FileGallery = "*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pjpeg;*.pjpg;*.tif;";
        var FileGalleryDesc = "Images (*.jpg *.jpeg *.png *.gif *.bmp *.pjpeg *.pjpg *.tif)";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Board/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Board/Mod.js");
    </script>
    <script type="text/javascript">
        setTimeout(function () {
            Mod.GetAttachFileList();
            Mod.GetSpoolFileList();
        }, 200);
    </script>  
</body>
</html>
