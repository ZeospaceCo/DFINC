<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Board.Mod" %>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
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
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchTerm = "<%=SearchTerm %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var R_Add_InputTitle = "<%=GetResource("Add_InputTitle") %>";        
        var ImagePath = "<%=BaseImgPath %>";
        var R_Del = "<%=GetResource("Unit_Register_Delete") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";   
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Unit_Register_Delete") %>";
        var R_NoAttachData = "<%=GetResource("NoFile") %>"; 
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DelAttach = "<%= GetResource("DelAttach") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Board/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Board/Mod.js");
    </script>
    <script type="text/javascript">
        setTimeout(function () {
            Mod.GetAttachFileList();
            Mod.GetSpoolFileList();
        }, 200);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li"  Class="btn btn-secondary"  />
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
                                    <th>
                                        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbTitle" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
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
                                <%=GetResource("AttachName")%>
                            </li>
                            <li><div id="placeholder_fileList" style="clear: both;"></div></li>
                            <!-- 첨부파일 리스트 -->

                            <!-- 첨부파일 -->
                            <li class="title">
                                <%=GetResource("Additional")%> <%=GetResource("AttachName")%>
                                <div class="float-right">                            
                                <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                                </div>
                            </li>
                            <li><div id="placeholder_file" style="clear: both;"></div></li>
                            <!-- 첨부파일 리스트 -->
                       </ul>   
                    </div>   
                  </div>             
            </div>
        </div>
    </div>
   <!-- ================================== Hidden Input ===================================== -->
        <div style="display: none">
            <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
            <asp:TextBox ID="tbHiddenSelectedContent" runat="server" Width="0px"></asp:TextBox>
            <asp:TextBox ID="tbHiddenMime_contents" TextMode="MultiLine" Width="0" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbHiddenText_contents" Width="0" runat="server"></asp:TextBox>

            <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
            <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"
                style="display: none" rows="1" cols="20"></textarea>
        </div>
        <!-- ==================================================================================== -->
    </form>
</body>
</html>
