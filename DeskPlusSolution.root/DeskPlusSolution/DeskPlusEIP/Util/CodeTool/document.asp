<%
'###################################################################################################
'## 만든이				: 심재운 대리
'## 테이블				: 그룹웨어
'## 작성일자			: 
'## 사용용도			: 테이블 파라미터 만들기
'## 보안위험도			: 上 (sql 2005 용입니다.)
'###################################################################################################


%>

<OBJECT RUNAT=server PROGID=ADODB.Connection ID=adoConn ></OBJECT> 
<OBJECT RUNAT=server PROGID=ADODB.RecordSet ID=adoRS ></OBJECT> 

<% 
	 
	Response.Expires = -1000 

	ProjectName = "TABLE INFORMATION" 
	YourName = "심재운" '##작성자 이름
	

'####### Db 연결 (각자의 DB명에 맞게 연결코드를 완성하면 됩니다.) #### 
	adoConn.open "Provider=SQLOLEDB.1;Data Source=zeospace-20;Initial Catalog=DeskPluseip40;User ID=deskpluseip;Password=deskplus;Persist Security Info=True"  

   
    docID = request("docID")
    docID = Replace(docID, "'","")

    SQL = "SELECT ContentHTML FROM DP_APP_DOC WHERE COMPANYID = 1 AND DOCUMENTID = " & docID
        
        'response.write SQL & "<br>"  

	adoRS.open SQL, adoConn, 0 
     
	
%> 


<% 
cnt = 0
	Do Until adoRS.EOF
	
	    ContentHTML = adoRS("ContentHTML")
    
        'response.Write "parms["&cnt&"].Value = _be."&adoRS("column_name")&";<br>"
        response.Write ContentHTML
     	
	
	    'response.Write "new SqlParameter(PARM_" & UCase(adoRS("column_name")) & ", SqlDbType." & sqldb & sqllen  &"),<br>"
    	adoRS.MoveNext  '다음레코드로 이동
    	cnt = cnt + 1
	Loop   



adoRS.close 
adoConn.close 

%>
