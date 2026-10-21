<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reply.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Board.Reply" %>
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
                <h1><%=GetResource("Reply")%></h1>
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
                                    <th class="last-child"><%=GetResource("Title")%></th>
                                    <td>
                                        <asp:TextBox ID="tbTitle" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="tabs" class="layout_subbox">
                            <ul id="tab" class="tab" style="display:none">
                                <li><a id="ActiveX" ><span>ActiveX</span> </a></li>
                                <li><a id="NonActiveX" class="tab_on"><span>NonActiveX</span> </a></li>
                            </ul>
                            <div class="divNonEditor">
                                <div id="divXFE" style="height: 5px;">
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
                   </div>
                   <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">    
                      <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AttachFile")%>
                            <div class="float-right">                            
                            <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("FileAdd")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                      </ul>
                    </div>
                  </div>
            </div>
        </div>
    </div>
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <!-- ==================================================================================== -->
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
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>"; //정렬
        var TrySaveCnt = 0;
        var Grade = "<%=Grade %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수 
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
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
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID 
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Board/Reply.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Board/Reply.js");
    </script>
</body>
</html>
