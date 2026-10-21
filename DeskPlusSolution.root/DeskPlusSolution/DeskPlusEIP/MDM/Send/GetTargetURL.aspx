<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetTargetURL.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.Send.GetTargetURL" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="/DeskPlusEIP/Board/Common/Admin/BoardTreeScript.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("BoardList")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
                <br />
                <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="80px;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><asp:literal id="lblSelected" runat="server"></asp:literal></th>
                            <td><cc3:ClientSmartTextBox ID="tbTitle" ReadOnly="true" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style1">                
                <div>
                    <div class="whd_layout_left">   
                        <div class="layout_scroll" style="min-height:425px;height:425px;outline:1px solid rgba(0,0,0,.1);">   
                            <componentart:TreeView ID="tvBoardTreeScript" runat="server" 
                                Width="100%" 
                                Height="98%" 
            
                                EnableViewState="false"
                                AutoPostBackOnSelect="False" 
                                DragAndDropEnabled="false" 
                                NodeEditingEnabled="false" 
                                AutoScroll="false" 
            
                                CssClass="TreeView" 
                                NodeCssClass="TreeNode"
                                HoverNodeCssClass="HoverTreeNode" 
                                SelectedNodeCssClass="SelectedTreeNode" 
                                MultipleSelectedNodeCssClass="SelectedTreeNode"
                                SelectedNodeRowCssClass="SelectedTreeNodeRow"
                                NodeRowCssClass="TreeNodeRow" 
                                LoadingFeedbackCssClass="LoadingFeedback"
             
                                NodeIndent="15" 
                                ShowLines="True" 
                                LineImageWidth="19" 
                                LineImageHeight="20" 
            
                                NoExpandImageUrl="noexpand.gif" 
                                ExpandCollapseImagewidth="14" 
                                ExpandCollapseImageHeight="11" 

            
                                EnableClientScript="true"
                                ClientSideOnCallbackComplete="onCallbackComplete"  
                                ClientSideOnNodeSelect="onNodeSelect">
                            </componentart:TreeView>                                                               
                        </div>
                    </div>

                    <div class="whd_layout_right">    
                        <!-- 테이블 시작 //-->
                        <div style="padding:10px 0px 0px 0px;display:inline-block;width:100%;min-height:530px;float:right;">
                        <div id="phBoard"></div>
                        </div>
                        <!-- 테이블 끝 //-->

                        <!-- 페이징 시작 //-->                            
                        <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                            <h3><span id="pageInfo"></span></h3>
	                        <span id="pager"></span>
                        </div>                            
                        <!-- 페이징 끝 //-->
                    </div>   
                </div>
            </div>
        </div>
    </div>
    <!-- //layout_info -->
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnCategoryID" type="hidden" name="hdnCategoryID" runat="server">
        <input id="hdnListID" type="hidden" name="hdnListID" runat="server">
    </div>
    <!-- ==================================================================================== -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "10"; //보여줄게시물수
        var CatID = "<%=CatID %>";
     
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";    
        var R_Number = "<%=GetResource("Number") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_DocView = "<%=GetResource("DocView") %>";
        var R_Register = "<%=GetResource("Register") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_NoContent = "<%=GetResource("NoContent")%>";    
        var R_Notice  = "<%=GetResource("Notice")%>";    
        var R_SelectSubject = "<%=GetResource("SelectSubject")%>";  

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/Send/GetTargetURL.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/Send/GetTargetURL.js");
    </script>
</body>
</html>
