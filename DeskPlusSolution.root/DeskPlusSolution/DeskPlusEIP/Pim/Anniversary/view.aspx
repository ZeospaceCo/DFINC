<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Anniversary.view" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
       <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AnniversaryDay")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
              <cc3:ClientSmartButton ID="CbtnPrint" runat="server"  BeginEndTag="li"></cc3:ClientSmartButton>            

              <cc3:ClientSmartButton ID="CbtnEdit" runat="server"  BeginEndTag="li" Class="btn btn-secondary"></cc3:ClientSmartButton>
              <cc3:ClientSmartButton ID="cbtnDel" runat="server"  BeginEndTag="li" Class="btn btn-danger"></cc3:ClientSmartButton>
              <cc3:ClientSmartButton ID="CbtnCancel" runat="server"  BeginEndTag="li"></cc3:ClientSmartButton>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   

                    <div id="toPrint">
                    <table class="table_typeB">
			            <caption><%=GetResource("AnniversaryDay")%></caption>
			            <colgroup>
                            <col width="80px" />
                            <col />
                            <col width="80px" />
				            <col />
			            </colgroup>
			            <tbody>
				            <tr>
					            <th><%=GetResource("Title")%></th> 
					            <td colspan="3"><asp:Label ID="lblTitle" runat="server"></asp:Label></td> 
				            </tr> 
				            <tr>
                                <th><%=GetResource("Date")%></th> 
					            <td ><asp:Label ID="lblDateStr" runat="server"></asp:Label></td> 
					            <th><%=GetResource("WRITER")%></th>
					            <td><asp:Label ID="lblWriter" runat="server"></asp:Label></td>
				            </tr> 
			            </tbody>
		            </table>        

                    <!--본문정보-->
                    <div class="content_box"  style="overflow:auto;">
                        <table cellspacing="0" style="min-height:300px;width:100%;">
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="lblContent" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--본문정보-->
                    </div>
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"> 
                    <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AttachName")%>  
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                    </ul>
                </div>
            </div>             
         </div>
       </div>
    </div>

    <div style="display:none;">
        <asp:LinkButton ID="lbtnEdit" runat="server" ></asp:LinkButton>        
        <asp:LinkButton ID="lbtnCancel" runat="server"></asp:LinkButton>        
        <asp:LinkButton ID="lbtnDel" runat="server"></asp:LinkButton>        
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var ScheduleID = '<%=ScheduleID%>';
        var CompanyID = '<%=CompanyID%>';
        var R_FileName = '<%=GetResource("FileName") %>';
        var R_FileType = '<%=GetResource("FileType") %>';
        var R_FileLen = '<%=GetResource("FileLen") %>';
        var R_FileDown = '<%=GetResource("FileDownload") %>';
        var R_Name = '<%=GetResource("Name") %>';
        var R_Contents = '<%=GetResource("Contents") %>';
        var R_RegDate = '<%=GetResource("RegDate") %>';
        var R_ApplyDelete = '<%=GetResource("ApplyDelete") %>';
        var R_ReallyApplyDelete = '<%=GetResource("ReallyApplyDelete") %>';
        var Searchkey = '<%=Searchkey%>';
        var SearchValue = '<%=SearchValue%>';
        var CurrentPage = '<%=CurrentPage%>';
        var Starttime = '<%=Starttime%>';
        var Endtime = '<%=Endtime%>';
        var Order = '<%=Order%>';
        var PreLang = "<%=PreLang%>";
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_TextEmpty = '<%=GetResource("TextEmpty") %>';
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Anniversary/view.js"></script>
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Pim/Js/Anniversary/view.js");
    </script>    
</body>
</html>
