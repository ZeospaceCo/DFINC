<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Anniversary.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />        
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
        var SwfUploadGUID = '<%=SwfUploadGUID %>'; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = '<%=GetResource("ConfirmEditorTab") %>';
        var NonActiveX = '<%=NonActiveX%>';
        var ClientQueryString = '<%=Page.ClientQueryString %>';
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var R_TextEmpty = '<%=GetResource("TextEmpty") %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        var PreLang = "<%=PreLang%>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Anniversary/Add.js" ></script>   
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Anniversary/Add.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body" class="write">
     <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AnniversaryDay")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnContinue" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
             <div class="row">
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >

                    <table class="table_typeB">
			        <caption>
			        <%=GetResource("AnniversaryReg")%>
			        </caption>
			        <colgroup>
                        <col width="80px" />
                        <col />
			        </colgroup>
			        <tbody>
				        <tr>
					        <th><%=GetResource("Title")%></th>
					        <td>
                              <input type="text" id="txttitle" runat="server" style="width:350px" maxlength="20"  />
                              <asp:DropDownList ID="ddlCategory" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                            </td>
				        </tr> 
				        <tr id="tdDate">
					        <th><%=GetResource("Date")%></th>
					        <td>
                              <div id="startEndDate">
					          <input type="text" id="startDate" size="12"  runat="server"  readonly="readonly" /> 
					          <input type="radio" class="searchradio" name="rbluna" id="rdSun" runat="server" value=""  checked="true" /><%=GetResource("sunday")%>
					          <input type="radio" class="searchradio" name="rbluna" id="rbmoon" runat="server" value="" /><%=GetResource("lunalday")%>
					          </div>
                            </td>
				        </tr> 
				        <tr style="display:none">
					        <th><%=GetResource("PimAlarm")%></th>
					        <td>
                                <asp:DropDownList ID="ddlnotikind" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
				                <input name="idEmail" type="text" class="text required" value="" id="idEmail" runat="server" style="width:100px !important;" maxlength="50" />

					            <asp:DropDownList ID="ddltime" runat="server" CssClass="searchselect form-control">                            
                                  <asp:ListItem Text="Time" Value=""></asp:ListItem>                    
		                        </asp:DropDownList><%=GetResource("BeforeNoti")%>
                            </td>
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
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">  
                    <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AttachName")%>  
                            <div class="float-right">                            
                            <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
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
</body>
</html>
