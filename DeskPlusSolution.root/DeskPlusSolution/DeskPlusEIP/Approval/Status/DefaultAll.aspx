<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultAll.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.DefaultAll" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <script type="text/javascript">
        var approvalStatusContentLoaded = false;
        var approvalStatusLastContentUrl = "";
        var approvalStatusTreeRefreshTimer = null;

        function refreshApprovalStatusTreeCount() {
            var currentUrl = "";

            try {
                currentUrl = window.frmContent && window.frmContent.location ? window.frmContent.location.href : "";
            }
            catch (e) {
                currentUrl = "";
            }

            if (!approvalStatusContentLoaded) {
                approvalStatusContentLoaded = true;
                approvalStatusLastContentUrl = currentUrl;
                return;
            }

            try {
                if (!window.frmLeft || !window.frmLeft.TreeAll) return;

                if (window.frmLeft.TreeAll.SkipContentLoadRefresh) {
                    window.frmLeft.TreeAll.SkipContentLoadRefresh = false;
                    approvalStatusLastContentUrl = currentUrl;
                    return;
                }

                if (!isApprovalStatusTreeRefreshTarget(approvalStatusLastContentUrl, currentUrl)) {
                    approvalStatusLastContentUrl = currentUrl;
                    return;
                }

                approvalStatusLastContentUrl = currentUrl;

                if (approvalStatusTreeRefreshTimer != null) {
                    window.clearTimeout(approvalStatusTreeRefreshTimer);
                }

                approvalStatusTreeRefreshTimer = window.setTimeout(function () {
                    try {
                        if (window.frmLeft && window.frmLeft.TreeAll && typeof window.frmLeft.TreeAll.RefreshAllCount == "function") {
                            window.frmLeft.TreeAll.RefreshAllCount();
                        }
                    }
                    catch (e) {
                    }
                }, 100);
            }
            catch (e) {
            }
        }

        function isApprovalStatusTreeRefreshTarget(previousUrl, currentUrl) {
            var prev = (previousUrl || "").toLowerCase();
            var curr = (currentUrl || "").toLowerCase();

            if (prev == "" || curr == "" || prev == curr) return false;
            if (!isApprovalStatusListPage(curr)) return false;

            return isApprovalStatusProcessPage(prev);
        }

        function isApprovalStatusListPage(url) {
            return url.indexOf("/deskpluseip/approval/status/list") >= 0
                || url.indexOf("/deskpluseip/approval/folder/") >= 0;
        }

        function isApprovalStatusProcessPage(url) {
            return url.indexOf("/deskpluseip/approval/process/") >= 0
                || url.indexOf("/deskpluseip/approval/formflow/") >= 0
                || url.indexOf("/deskpluseip/approval/status/common/view") >= 0
                || url.indexOf("/deskpluseip/approval/folder/common/view") >= 0
                || url.indexOf("/deskpluseip/approval/folder/carboncopy/view") >= 0;
        }
    </script>
</head>
<frameset cols="240,*" border="0">
    <frame name="frmLeft" id="frmLeft" frameborder="0" src="<%=LeftURL%>"></frame>
    <frame name="frmContent" id="frmContent" frameborder="0" src="<%=ContentURL%>" class="frmContent" onload="refreshApprovalStatusTreeCount();"></frame>
</frameset><noframes></noframes>
</html>
