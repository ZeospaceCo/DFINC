<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebHardAttach.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Upload.WebHardAttach" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Upload")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style1">      
                <div id="tabs">
                    <ul id="tab" class="tab">
                        <li><a id="RIAUpload"><span><%=GetResource("Files")%></span></a></li>
                        <li><a id="FolderUpload" class="tab_on"><span><%=GetResource("WebHardTitle")%></span> </a></li>
                    </ul>
                    <div>
                      <div class="whd_layout_left">   
                            <div class="layout_scroll" style="min-height:341px;height:341px;outline:1px solid rgba(0,0,0,.1);">   
                                <COMPONENTART:TREEVIEW 
                                id="tvFolderTree" 
                                runat="server" 
                                Width="203px" 
                                Height="260px" 
                                        
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
                                </COMPONENTART:TREEVIEW>                                
                            </div>
                      </div>                            
  
                      <div class="whd_layout_right">     
                            <!-- 테이블 시작 //-->
                            <div style="padding:10px 0px 0px 0px;width:100%;min-height:355px;height:355px;float:right;overflow:auto;">
                            <div id="phWebAttach"></div>
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
    </div>    
    <!-- //layout_info -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var callBackEvent = "<%=CallBackEvent %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "7"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var FolderID = "";
        var GUID = "<%=GUID %>";
        var ModuleName = "<%=ModuleName %>";
        var UID = "<%=UserID %>";
        var FileType = "<%=FileType %>";
        var FileTypeDesc = "<%=FileTypeDesc %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_Download = "<%=GetResource("File") %>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_SelectAttach = "<%=GetResource("SelectAttach") %>";
        var R_NoWebHardUpload = "<%=GetResource("NoWebHardUpload") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Upload/WebHardAttach.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Upload/WebHardAttach.js");
    </script>
</body>
</html>
