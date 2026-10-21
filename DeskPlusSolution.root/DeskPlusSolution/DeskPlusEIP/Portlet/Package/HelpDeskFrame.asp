<%
  response.expires = 0

  id = request("id")
   pass = request("pass")

  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '                 
  '       location  : 
  '       call      : 
  '       target    : 
  '       argument  : 
  '       programer : 
  '       date      : 
  '       copyright¨Ï2001 zeospace inc. all rights reserved.   
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''  
   randomize 
%>

<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title></title>

</head>
<body <%'oncontextmenu="JAVAscript:return false;"%> topmargin="0" leftmargin="0" marginwidth="0" marginheight="0">
<IFRAME name="Detail" width="100%" height="768px" frameborder="0" align="center" src="http://help.deskplus.co.kr/techTeam/Frame_Main.asp?p=ZGNwYXJtJTNkaGVscGRlc2s=&id=<%=id%>&pass=<%=pass%>&go=dec"></IFRAME>
</body>
</html>