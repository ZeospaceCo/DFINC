<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorDefault.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Content.Html.ErrorMessage.ErrorDefault" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="uft8" />
    <title></title>
    
		<script language="javascript">
			function GoBack()
			{
				history.back();
			}
		</script>    
</head>
<body>
    <form id="form1" runat="server">
			<div>
				<table border="0" cellspacing="0" cellpadding="0" align="center">
					<thead>
						<tr>
							<td><img src="images/err_top.jpg" width="384" height="97"></td>
						</tr>
						<tr>
							<td background="images/errbg_main.jpg" align="center" valign="top">
								<table width="332" border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td width="332" class="td12"></td>
									</tr>
									<tr>
										<td align="center"><span class="SBOLD_err"><span id="lblDESC">예상치 않은 오류입니다.</span>
											</span>
											<br>
											<br>
											<img src="images/ico_help.gif" width="14" height="14" align="absMiddle"></td>
									</tr>
									<tr>
										<td class="td12"></td>
									</tr>
								</table>
							</td>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td height="45" background="images/err_bottom.jpg">
								<table border="0" cellspacing="0" cellpadding="0" width="100%">
									<tr>
										<td width="10%"></td>
										<td width="34%" align="right"><!--a href="javascript:showLayer('divDetail');"><img src="images/btn_detail.gif" id="ibtnDetail" align="absMiddle" border="0" /></a--></td>
										<td width="2%"><A href="javascript:GoBack();"><img src="images/btn_list_fb.gif" border="0"></A></td>
										<td width="34%" align="left"></td>
										<td width="10%"></td>
									</tr>
								</table>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
    </form>
</body>
</html>
