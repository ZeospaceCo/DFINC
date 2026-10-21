<%@ Page Language="C#" AutoEventWireup="true" Codebehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Unit.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
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
<div id="layout_body">
<div class="layout_full">
    <div id="layout_header">
        <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server" /></li>   

            <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>    
            <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>    
        </ul>
    </div>

    <div id="layout_content">
        <div class="row">
			<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                <div id="toPrint">
                <table class="table_typeB">
                <caption></caption>
                <colgroup>
                    <col width="100px" />
                    <col />
                    <col width="100px" />
                    <col />
                </colgroup>
                <tbody>
                <tr>
                    <th><asp:Literal ID="ltlPlanStartDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanStartDate" runat="Server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlPlanEndDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanEndDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblStartDate" runat="Server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblEndDate" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlPlanMoney" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanMoney" runat="Server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlPlanDay" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblPlanDay" runat="Server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></th>
                    <td colspan="3">
                        <asp:Label ID="lblTitle" runat="Server"></asp:Label>
                    </td>          
                </tr> 
                <tr>
                    <th><asp:Literal ID="ltlExecuteMoney" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblExecuteMoney" runat="Server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlCharger" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblCharger" runat="Server"></asp:Label></td>
                </tr> 
                <tr>
                    <th><asp:Literal ID="ltlProcess" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblProcess" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlChargeDeptName" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblChargeDeptName" runat="Server"></asp:Label></td>
                </tr> 
                <tr>
                    <th><asp:Literal ID="ltlRelateDeptName" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblRelateDeptName" runat="Server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlWeightPercent" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWeightPercent" runat="server"></asp:Label></td>
                </tr> 
                <tr>
                    <th><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblStatus" runat="server"></asp:Label></td>          
                    <th><asp:Literal ID="ltlCode" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblCode" runat="Server"></asp:Label></td>
                </tr>  
                <tr>
                    <th><asp:Literal ID="ltlWriteName" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteName" runat="server"></asp:Label></td>
                    <th><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></th>
                    <td><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
                </tr> 
                </tbody>
                </table>

                <div class="content_box"  style="overflow:auto;">
                <table cellspacing="0" style="min-height:300px;width:100%;">
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                </div>
                </div>
        </div><!-- end col content-area-body-right -->
        <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"> 
             <ul> 
                <!--첨부파일-->
                <li class="title">
                    <%=GetResource("AttachName")%>        
                </li>
                <li><div id="placeholder_file" style="clear: both;"></div></li>
                <li class="title"><%=GetResource("Board_List_ShortReply")%>
					<cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                </li>
                <li class="reply"><div id="placeholder" style="clear: both;"></div></li>
            </ul>
        </div>
    </div>
    </div>    
</div>
</div>

<div style="display: none">
<!-- ================================== Hidden Input ===================================== -->
<asp:TextBox ID="tbMailID" runat="server" Width="0" Style="display: none"></asp:TextBox>
<asp:TextBox ID="tbAttachInfo" runat="server" Width="0" Style="display: none"></asp:TextBox>
<asp:TextBox ID="tbSubject" runat="server" Width="0" Style="display: none"></asp:TextBox>
<asp:TextBox ID="tbBody" runat="server" Width="0" Style="display: none"></asp:TextBox>
<asp:TextBox ID="tbServer" runat="server" Width="0" Style="display: none"></asp:TextBox>
<asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ProjectID = "<%=ProjectID %>";
        var MenuID = "<%=MenuID %>";  
        var MenuType = "<%=MenuType %>";   
        var RqProjectID = "<%=RqProjectID %>";
        var Status = "<%=Status %>"; 
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var UnitID       = "<%=UnitID %>"; 
		var AttachFilesInfo     = "<%=AttachFilesInfo %>";
		var projParam           = "<%=ProjParam%>";
        var ImagePath = "<%=BaseImgPath%>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Schedule_Header3") %>";
        var R_RegDate = "<%=GetResource("Menu_Header4") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ReallyApplyDelete = "<%=GetResource("DeleteReal") %>";
        var R_Del = "<%=GetResource("Unit_Register_Delete") %>";
        var R_Modify = "<%=GetResource("Mod") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_FileName =  "<%=GetResource("FileName") %>"; 
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("FileDel") %>";
        var R_FileDown = "<%=GetResource("FileDown") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Unit/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Unit/View.js");
    </script>
</body>
</html>
