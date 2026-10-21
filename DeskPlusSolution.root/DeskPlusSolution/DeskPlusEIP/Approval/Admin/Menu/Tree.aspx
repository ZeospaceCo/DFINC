<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Menu.Tree" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin"> 
    	<ul class="left-menu-container">
			<li>
                <div id="layout_treetop">
                    <span class="treetop_title"><%=GetResource("ApprovalAdmin")%></span>
                </div>
            
                <div id="layout_treeboxAcc" style="height:90%;">
            
                    <div class="accordion">
                        <div class="menusub">
                            <ul class="menu_line">
                                <li><h2><a onclick="Tree.GoListPage('Form')"><%=GetResource("ManageForm")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('Folder')"><%=GetResource("ManageFolder")%></a></h2></li>  
                                <li><h2><a onclick="Tree.GoListPage('Doc')"><%=GetResource("ManageDoc")%></a></h2></li>                  
                                <li><h2><a onclick="Tree.GoListPage('ContactR')"><%=GetResource("ReceiveSender")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('ContactS')"><%=GetResource("ManageApprover")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('Proxy')"><%=GetResource("ManageProxy")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('EnforceSeal')"><%=GetResource("EnforceTitle")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('FolderAuth')"><%=GetResource("OtherDeptFolder")%></a></h2></li>                    
                                <li><h2><a onclick="Tree.GoListPage('AccessGrade')"><%=GetResource("AccessGrade")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('Config')"><%=GetResource("Setting2")%></a></h2></li>                    
                                <%--<li><h2><a onclick="Tree.GoListPage('Transfer')"><%=GetResource("TransferDoc")%></a></h2></li>--%>
                                <li><h2><a onclick="Tree.GoListPage('TransferDept')"><%=GetResource("TransferDocDept")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('TransferUser')"><%=GetResource("TransferDocUser")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('StatMonth')"><%=GetResource("StatTitle")%></a></h2></li>
                                <li><h2><a onclick="Tree.GoListPage('StatTop')"><%=GetResource("StatTop1")%></a></h2></li>
                                <li style="display:none"><h2><a onclick="Tree.GoListPage('')">상태별 문서건수</a></h2></li>
                                <li style="display:none"><h2><a onclick="Tree.GoListPage('')">처리부서,양식별 현황</a></h2></li>
                            </ul>
                        </div>
                    </div>
                
                </div>
            
                <div id="layout_treebuttom"></div>
                <!-- 버튼>
                <div id="layout_treebutton">
                    <ul class="btn_small">
                        <li class="btn_small_red"><a href="#"><span>삭제</span></a></li>
                        <li><a href="#"><span>수정</span></a></li>
                        <li class="btn_small_blue"><a href="#"><span>추가</span></a></li>
                    </ul>
                </div //-->
			</li>
		</ul>
	</div>
    </form>
</body>
</html>

