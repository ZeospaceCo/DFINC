<%
  response.expires = 0

  p = request("p")

  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '                 
  '       location  : 
  '       call      : 
  '       target    : 
  '       argument  : 
  '       programer : 
  '       date      : 
  '       copyrightⓒ2001 zeospace inc. all rights reserved.   
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
<IFRAME name="Detail" width="100%" height="768px" frameborder="0" align="center" src="/DeskPlusEDM/Authentication/LoginOutSide.aspx?p=<%=p%>"></IFRAME>
</body>
</html>