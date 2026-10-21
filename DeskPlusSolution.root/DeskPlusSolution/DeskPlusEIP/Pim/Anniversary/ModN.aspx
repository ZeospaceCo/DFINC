<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Anniversary.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_body" class="write"><!-- layout_body --> 
    <div class="layout_full"><!-- layout_full --> 
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AnniversaryEdit")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server" ></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >
                    <!-- 테이블 시작 -->
 		            <table cellspacing="0" class="table_typeB"> 
                    <caption><%=GetResource("AnniversaryEdit")%></caption>
			        <colgroup>
                        <col width="80px" />
                        <col />
			        </colgroup>
                    <tr> 
                      <th><%=GetResource("Title")%></th> 
                      <td>
                      <input type="text" id="txttitle" runat="server" style="width:350px" maxlength="20"   />
                      <asp:DropDownList ID="ddlCategory" runat="server" CssClass="searchselect" >
                       </asp:DropDownList>
                      </td> 
                    </tr> 
                    <tr id="tdDate" style="vertical-align:top;"> 
                      <th><%=GetResource("Date")%></th> 
                      <td >
                      <div id="startEndDate">
                      <input type="text" id="startDate" size="12"  runat="server" readonly="readonly" /> 
                      <input type="radio" class="searchradio" name="rbluna" id="rdSun" runat="server" value=""  checked="true" /><%=GetResource("sunday")%> 
                      <input type="radio" class="searchradio" name="rbluna" id="rbmoon" runat="server" value="" /><%=GetResource("lunalday")%>
                      </div>
                      </td> 
                    </tr>  
                    <tr style="display:none">
                      <th class="last-child"><%=GetResource("PimAlarm")%></th> 
                      <td class="last-child"> 
                       <asp:DropDownList ID="ddlnotikind" runat="server" CssClass="searchselect" runat="server">                                                
                       </asp:DropDownList>
                        <input name="idEmail" type="text" class="text required" value="" id="idEmail" runat="server" style="width:100px !important;" maxlength="50" >
                        <asp:DropDownList ID="ddltime" runat="server" CssClass="searchselect" runat="server">                            
                            <asp:ListItem Text="Time" Value=""></asp:ListItem>
                       </asp:DropDownList><%=GetResource("BeforeNoti")%>
                      </td> 
                    </tr> 
                    </table> 
        
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab" style="display:none">
                            <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divNonEditor">
                            <div id="divXFE" style="height: 5px;">                        
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
                        <li><div id="placeholder_fileListmod" style="clear: both;"></div></li>
                    </ul> 
                    <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AddTO")%> <%=GetResource("AttachName")%>
                            <div class="float-right">                            
                            <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><div id="placeholder_fileList" style="clear: both;"></div></li>
                    </ul>
                 </div>
              </div>
        </div>
    </div>
    </div>

    <div style="display:none;">
    <!-- 에디터에 필요한 필드 //-->
    <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
    <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
    <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>        

    <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
        <!--  파일 첨부시 필요한 필드 //-->
	<asp:TextBox id="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox id="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
	<asp:TextBox id="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>        
        	<iframe id="ifrmDelFile" style="DISPLAY: none" name="ifrmDelFile" src="about:blank" frameBorder="0"></iframe>
    <!-- ==================================================================================== -->
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>
    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript">
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
        var Add_SelectOptionTop = '<%= GetResource("Add_SelectOptionTop") %>';
        var Add_InputContent = '<%= GetResource("Add_InputContent") %>';
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var R_ApplyDelete = '<%= GetResource("ApplyDelete") %>';
        var R_FileName = '<%=GetResource("FileName") %>';
        var R_FileType = '<%=GetResource("FileType") %>';
        var R_FileLen = '<%=GetResource("FileLen") %>';
        var R_FileDel = '<%=GetResource("Del") %>';
        var ScheduleID = '<%=ScheduleID%>';
        var CompanyID = '<%=CompanyID%>';
        var Searchkey = '<%=Searchkey%>';
        var SearchValue = '<%=SearchValue%>';
        var CurrentPage = '<%=CurrentPage%>';
        var Starttime = '<%=Starttime%>';
        var Endtime = '<%=Endtime%>';
        var Order = '<%=Order%>';
        var SwfUploadGUID = '<%=SwfUploadGUID %>'; //flash upload GUID   
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = '<%=GetResource("ConfirmEditorTab") %>';
        var NonActiveX = '<%=NonActiveX%>';
        var ClientQueryString = '<%=Page.ClientQueryString %>';
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var PreLang = "<%=PreLang%>";
        var WaitMent = '<%=GetResource("waitment")%>';
        var R_TextEmpty = '<%=GetResource("TextEmpty") %>';
        var BtnCheck = 'N';
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Anniversary/Mod.js" ></script>           
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Anniversary/Mod.js");
    </script>
        
</body>
</html>
