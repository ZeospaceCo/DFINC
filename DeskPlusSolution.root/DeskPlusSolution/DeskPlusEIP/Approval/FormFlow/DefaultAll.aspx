<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultAll.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.DefaultAll" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript">
        var ListID = "<%=ListID %>";
        var DocID = "<%=DocID %>";
        var AccountTag = "<%=AccountTag %>";
        var ApprovalState = "<%=ApprovalState %>";
        var GroupID = "<%=GroupID %>";
        var IsReject = "<%=IsReject %>";

        $(function () {
            $(document).ready(function () {

                var leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/Tree.aspx";

                switch (ListID.toUpperCase()) {
                    case "LISTPROCESS":
                        //상신한문서
                        if (DocID > 0) {
                            // AccountTag에 따라 분기.
                            leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/";
                            leftUrl += ((AccountTag == "ZD") && (DocID == GroupID)) ? "TreeEdit.aspx" : "TreeReceiveEdit.aspx"
                        }
                        break;
                    case "LISTRECEIVE":
                    case "LISTRECEIVEALL":
                        //담당자결재문서(수신문서)
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeReceiveAll.aspx";
                        break;
                    case "LISTRECEIVEDEPT":
                        //담당자결재문서(부서수신문서)
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeReceiveAll.aspx";
                        break;
                    case "LISTTRANSFER":
                        //담당자결재문서(이관문서)
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeReceiveAll.aspx";
                        break;
                    case "APPROVE": //결재대상문서(LISTAPPROVE)
                        // 결재페이지
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeMod.aspx";
                        break;
                    case "LEGACY":  //외부시스템호출
                        // 문서작성
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeLegacy.aspx";
                        break;
                    case "LISTRECEIVEMOD": //수신문서수정(LISTRECEIVEMOD)
                        // 수신문서상신 페이지
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeModReceive.aspx";
                        break;
                    case "HOME":    //홈화면
                        // 수신문서상신 페이지
                        leftUrl = "/DeskPlusEIP/Approval/FormFlow/Menu/TreeModReceive.aspx";
                        break;
                }

                leftUrl += "?p=" + '<%=this.GetParm("p", "")%>';
                frmLeft.location.href = leftUrl;
            });
        });
    </script>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="about:blank"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="about:blank"></frame>
</frameset>
</html>
