<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Main.Mail.Default" %>
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
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("SendMail")%></h1>
            <span class="cm_subtxt">* <%=GetResource("MailDesc")%></span>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnSend" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
			  <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >

                <!-- 메일발송 시작 //-->
                <table class="table_typeB cm_mail">
			    <caption></caption>
			    <colgroup>
                    <col width="80px" />
                    <col />
			    </colgroup>
			    <tbody>
                    <tr>
                        <th><%=GetResource("Sender")%></th>
                        <td><cc3:ClientSmartTextBox ID="CTBSendName" ReadOnly="true" MaxLength="250" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
					    <th><%=GetResource("Receiver")%></th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctbTo" InputMode="None" TextMode="MultiLine" runat="server" Required="true" CssStyle="float:left;width:85% !important; height:40px"></cc3:ClientSmartTextBox>
                            &nbsp;<ul class="btn btn-sm"><li><a href="#" id="imgTo"><span><%= GetResource("Address")%></span></a></li></ul>                        
                        </td>
				    </tr>
                    <tr>
                        <th><%=GetResource("Cc")%></th>
                        <td>
                            <cc3:ClientSmartTextBox ID="ctbCc" InputMode="None" TextMode="MultiLine" runat="server" CssStyle="float:left;width:85% !important; height:40px"></cc3:ClientSmartTextBox>
                            &nbsp;<ul class="btn btn-sm"><li><a href="#" id="imgCc"><span><%= GetResource("Address")%></span></a></li></ul>                        
                        </td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Bcc")%></th>
                        <td>
                            <cc3:ClientSmartTextBox ID="ctbBcc" InputMode="None" TextMode="MultiLine" runat="server" CssStyle="float:left;width:85% !important; height:40px"></cc3:ClientSmartTextBox>
                            &nbsp;<ul class="btn btn-sm"><li><a href="#" id="imgBcc"><span><%= GetResource("Address")%></span></a></li></ul>                        
                        </td>
                    </tr>
                    <tr>
                        <th class="last-child"><%=GetResource("Title")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbTitle" InputMode="None" runat="server" Required="true" CssStyle="width:94% !important;"></cc3:ClientSmartTextBox></td>
                    </tr>
			    </tbody>
		        </table> 

                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab">
                        <li><a id="ActvieX"  class="tab_on"><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div class="divEditor">
                        <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                    </div>
                </div>
            </div>
            <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">     
                 <ul> 
                    <!-- 첨부파일 -->
                    <li class="title">
                        <%=GetResource("AttachFile")%>
                        <div class="float-right">                            
                        <a href="#" id="cbtnAddFile" title="<%=GetResource("FileAdd")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                        </div>
                    </li>
                    <li><div id="phFile" style="clear: both;"></div></li>
                </ul>                 
            </div>
          </div>
        </div>   
        
        <div style="display:none;">        
        <asp:TextBox ID="tbTo" Style="display: none" runat="server" Width="0px"></asp:TextBox>   
        <asp:TextBox ID="tbCc" Style="display: none" runat="server" Width="0px"></asp:TextBox>   
        <asp:TextBox ID="tbBcc" Style="display: none" runat="server" Width="0px"></asp:TextBox>   
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>                 
        </div>

    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.autogrow-textarea.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var BtnCheck = "N";
        var CommunityID = "<%=CommunityID %>";
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>"; //정렬
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수        
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_InputTitle = "<%=GetResource("InputTitle") %>";   
        var R_InputContent = "<%=GetResource("InputContent") %>";     
        var R_InputTo = "<%=GetResource("InputTo") %>";   
        var R_ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel ="<%=GetResource("FileDel") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";    
        var R_ErrorMailType = "<%=GetResource("ErrorMailType") %>";            
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Main/Mail/Default.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Main/Mail/Default.js");
    </script>
</body>
</html>
