<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>신규등록</h1>
                <!-- 버튼 //-->
                <ul class="btn_right">              
                    <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                </ul>
            </div>
            <div id="layout_content">
               <div class="row"> 
                  <div class="col-12 col-lg-9 col-xl-8 content-area-body-left">
                    <div class="layout_Tbox">
                    <table cellspacing="0" class="table_typeB">
                        <colgroup>
                            <col width="80px" />
                            <col />
                            <col width="80px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>분류</th>
                                <td colspan="3"><asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>  </td>
                            </tr>
                            <tr>
                                <th>대상자</th>
                                <td colspan="3">   
                                    <asp:HiddenField ID="hdReceiveID" runat="Server"></asp:HiddenField>
                                    <input type="hidden" id="hdAllTargetYN" name="hdAllTargetYN" value="N" />
                                    <div id="InputTO" style="width: 75%; padding-left: 2px; padding-right:2px;display: inline-block;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>                                    
                                    <span class="btn_right"><cc3:ClientSmartButton ID="cbtnWhole" runat="server"></cc3:ClientSmartButton>&nbsp;<cc3:ClientSmartButton ID="cbtnReceive" runat="server"></cc3:ClientSmartButton></span>
                                </td>                               
                            </tr>                                                       
                            <tr>
                                <th>기간</th>
                                <td colspan="3"><cc3:ClientSmartTextBox ID="tbxStartDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                ~ <cc3:ClientSmartTextBox ID="tbxEndDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                </td>                              
                            </tr>
                            <tr>
                                <th>팝업설정</th>
                                <td>
                                   <input id="cbPopUp" type="checkbox" name="cbPopUp" runat="server" />팝업사용
                                </td>
                                <th>알람</th>
                                <td><asp:DropDownList ID="ddlAlarmDay" runat="server" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;(일전 미완료자 재알람 발송)</td>                                
                            </tr>
                            <tr>
                                <th>과정명</th>
                                <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="tbxTitle" Width="98%" MaxLength="100" Required="true"></cc3:ClientSmartTextBox></td>
                            </tr>
                        </tbody>
                    </table>
                    </div>
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
                  <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                    <ul>
                        <!-- 첨부파일 -->
                        <li class="title">
                             첨부파일                 
                             <a href="#" id="cbtnSwfFileAdd"><i class="fas fa-plus-circle text-secondary" title="파일추가"></i></a>                   
                        </li>
                        <!-- 첨부파일 리스트 -->                
                        <li><div id="placeholder_file" style="clear: both;"></div></li>                       
                    </ul>

                  </div><!-- end col  -->
    
               </div><!-- end row  -->
            </div><!-- end layout_content  -->
		</div><!-- end layout_full  -->
	</div><!-- end layout_body  -->
    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"   runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  style="display: none" rows="1" cols="20"></textarea>
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchDateValue = "<%=SearchDateValue %>";
        var SearchTarget = "<%=SearchTarget %>";
        var SearchItem = "<%=SearchItem %>"; 

        var ReceiveName = "";
        var BtnCheck = "N";     
      
        var SwfUploadGUID = "<%=SwfUploadGUID %>";        
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";        
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=(PreLangReal == "en-gb") ? "en-GB" : PreLang%>.js"></script>    
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Admin/Add.js");
    </script>
</body>
</html>
