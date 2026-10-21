<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Board.Add" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
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
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
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
                            <ul id="tab" class="tab" style="display:none">
                                <li><a id="ActiveX"><span>ActiveX</span> </a></li>
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
                    </div><!-- end col content-area-body-right -->
                    <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">  
                        <ul> 
                            <!-- 첨부파일 -->
                            <li class="title">
                                <%=GetResource("Board_List_Attach")%>
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
            <!-- 에디터에 필요한 필드 //-->
            <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
            <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>            
        
            <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
            <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        </div>
        <!-- ==================================================================================== -->
    </form>
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
        var ProjParam           = "<%=ProjParam %>"; 
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchTerm = "<%=SearchTerm %>";
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
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Board/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Board/Add.js");
    </script>
</body>
</html>
