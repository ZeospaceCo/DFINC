<%
  response.expires = 0

   p = request("p")
   cmd = request("cmd")
   id = request("id")
   pass = request("pass")
   user_account = request("user_account")
   mailUrl = request("mailUrl")
   language = request("language")

   'Response.write "mailUrl" &  mailUrl
   'Response.end

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
<IFRAME name="Detail" width="100%" height="800px" frameborder="0" align="center" src="http://<%=mailUrl%>/zmail/login.nvd?p=<%=p%>&cmd=<%=cmd%>&user_account=<%=user_account%>&language=<%=language%>"></IFRAME>
</body>
</html>