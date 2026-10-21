<%
'###################################################################################################
'## 만든이				: 심재운 대리
'## 테이블				: 그룹웨어
'## 작성일자			: 
'## 사용용도			: 테이블 BE 객체 생성하기
'## 보안위험도			: 上 (sql 2008 용입니다.)
'###################################################################################################



%>
<object runat="server" progid="ADODB.Connection" id="adoConn" viewastext>
</object>
<object runat="server" progid="ADODB.RecordSet" id="adoRS" viewastext>
</object>
<% 
	 
	Response.Expires = -1000 
	Dim SQL, i,tmpLen, j, k , MaxK, aryRef, M, aryFK, MaxFK, ProjectName, YourName,tmpTABLEName, aryPk, TABLE_des, aryTblist, aryIndex, s, u, tmpIndexkeys,aryTABLEInfo 


	ProjectName = "TABLE INFORMATION" 
	YourName = "심재운" '##작성자 이름
	

'####### Db 연결 (각자의 DB명에 맞게 연결코드를 완성하면 됩니다.) #### 
	adoConn.open "Provider=SQLOLEDB.1;Data Source=zeospace-20;Initial Catalog=SiteKsnetEIP;User ID=deskpluseip;Password=deskplus;Persist Security Info=True" 
   
    itableName = request("tablename")

	'# 테이블 정보조회 
		SQL = "SELECT A.TABLE_CATALOG ,A.TABLE_NAME ,A.COLUMN_NAME " &_
        ",A.DATA_TYPE ,ISNULL(A.CHARACTER_MAXIMUM_LENGTH,'')AS LENGTH  " &_
        ",A.IS_NULLABLE ,ISNULL(B.CONSTRAINT_NAME,'')AS PK ,ISNULL(A.CHARACTER_SET_NAME,'')AS CHARACTER  " &_
        ",ISNULL(A.COLLATION_NAME,'')AS COLLATION_NAME FROM  " &_
        "INFORMATION_SCHEMA.COLUMNS A  LEFT OUTER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE B  " &_
        "ON A.TABLE_NAME = B.TABLE_NAME   AND A.COLUMN_NAME = B.COLUMN_NAME LEFT OUTER JOIN  " &_
        "syscolumns C ON C.ID = object_id(A.TABLE_NAME) AND A.COLUMN_NAME = C.NAME AND C.COLSTAT & 1 = 1  " &_
        "where a.table_name = '"&itableName&"' " &_
        "order by a.table_name "  
        
        'response.Write SQL 

	adoRS.open SQL, adoConn, 0  
	if not adoRS.Eof then 
		aryTABLEInfo = adoRS.GetRows() 
		end if 
	adoRS.close 
%>
<meta http-equiv="Content-Type" content="text/html; charset=ks_c_5601-1987">
<style type="text/css"> 
	<!-- 
		/* BODY { 
		FONT-SIZE: 9pt 
		} 
		*/ 
		TABLE { 
		FONT-SIZE: 9pt 
		} 
		TD { 
		FONT-FAMILY: Gulimche; 
		FONT-SIZE: 9pt 
	} 
	--> 
	</style>
<%
	for i = 0 to Ubound(aryTABLEInfo,2) 

		tmpTABLEName = aryTABLEInfo(2,i) 

		
%>
<table border="1" width="800" cellspacing="0" bordercolordark="white" bordercolorlight="#959595">

    <% 
		'##################################################################

		for j = 0 to Ubound(aryTABLEInfo,2) 

			'1##--
			if i > Ubound(aryTABLEInfo,2) then 
				exit for 
			end if
			'1##--
			
			'2##--
			if tmpTABLEName = aryTABLEInfo(2,i) then 
				tmpTABLEName = aryTABLEInfo(2,i) 
			else 
				exit for 
			end if 
			'2##--		
    %>
    <tr height="20">
        <td align='LEFT' bgcolor='#DFDFDF'>
            <%
'response.Write aryTABLEInfo(4,i)
		Response.write "/// &lt summary>"
		Response.write "<br>"
		Response.write "/// " & aryTABLEInfo(2,i)
		Response.write "<br>"
		Response.write "/// &lt/summary>"
		Response.write "<br>"
		Response.write "public "
		'if instr("bit,tinyint,smallint,int,int identity,bigint,smalldatetime,datetime,text,ntext",aryTABLEInfo(4,i)) = 0 then
		if instr("nchar,char,text,varchar,ntext,nvarchar",aryTABLEInfo(3,i)) > 0 then 
				Response.Write "string" 
		end if 
		if instr("smallint,int,tinyint,bigint,int identity",aryTABLEInfo(3,i)) > 0 then 
				Response.Write "int" 
		end if 
		if instr("datetime",aryTABLEInfo(3,i)) > 0 then 
				Response.Write "DateTime" 
		end if 
		
		Dim FirstString, LastString
		FirstString = UCase(left(aryTABLEInfo(2,i),1))
		LastString	= mid(aryTABLEInfo(2,i),2,len(aryTABLEInfo(2,i)))
		Response.write "&nbsp;" & FirstString & LastString &" {get;set;}"
		'Response.write "&nbsp;" & aryTABLEInfo(3,i) &";"
            %>
        </td>
    </tr>
    <% 
	
	
		i = i + 1 		
		
		next 

		
		'#####################################

		i = i - 1


    %>
</table>
<table width="800" border="0">
    <tr>
        <td height="20" colspan="7">
            &nbsp;</td>
    </tr>
    <tr>
        <td height="20" colspan="7">
            &nbsp;</td>
    </tr>
</table>
<% 
	next 







	for i = 0 to Ubound(aryTABLEInfo,2) 

		tmpTABLEName = aryTABLEInfo(2,i) 

		
%>
<table border="1" width="800" cellspacing="0" bordercolordark="white" bordercolorlight="#959595">
    
    <% 
		'##################################################################

		for j = 0 to Ubound(aryTABLEInfo,2) 

			'1##--
			if i > Ubound(aryTABLEInfo,2) then 
				exit for 
			end if
			'1##--
			
			'2##--
			if tmpTABLEName = aryTABLEInfo(2,i) then 
				tmpTABLEName = aryTABLEInfo(2,i) 
			else 
				exit for 
			end if 
			'2##--		
    %>
    <tr height="20">
        <td align='LEFT' bgcolor='#DFDFDF'>
            <%

		Response.write "/// &lt summary>"
		Response.write "<br>"
		Response.write "/// " & aryTABLEInfo(2,i)
		Response.write "<br>"
		Response.write "/// &lt/summary>"
		Response.write "<br>"
		Response.write "public "
		'if instr("bit,tinyint,smallint,int,int identity,bigint,smalldatetime,datetime,text,ntext",aryTABLEInfo(4,i)) = 0 then
		if instr("char,nchar,text,varchar,ntext,nvarchar",aryTABLEInfo(3,i)) > 0 then 
				Response.Write "string" 
		end if 
		if instr("smallint,bigint,int,tinyint,int identity",aryTABLEInfo(3,i)) > 0 then 
				Response.Write "int" 
		end if 
		if instr("datetime",aryTABLEInfo(4,i)) > 0 then 
				Response.Write "DateTime" 
		end if 
		
		FirstString = LCase(left(aryTABLEInfo(2,i),1))
		LastString	= mid(aryTABLEInfo(2,i),2,len(aryTABLEInfo(2,i)))
				
		Response.write "&nbsp;" & aryTABLEInfo(2,i)
		Response.write "<br>"
		Response.write "{"
		Response.write "<br>"
		Response.write "&nbsp;&nbsp;&nbsp; get"
		Response.write "{return " & FirstString & LastString & ";"
		Response.write "}"
		Response.write "<br>"
		Response.write "&nbsp;&nbsp;&nbsp; set"
		Response.write "{" & FirstString & LastString & " = value;}"
		Response.write "<br>"
		Response.write "}"
            %>
        </td>
    </tr>
    <% 
	
	
		i = i + 1 		
		
		next 

		
		'#####################################

		i = i - 1


    %>
</table>
<table width="800" border="0">
    <tr>
        <td height="20" colspan="7">
            &nbsp;</td>
    </tr>
    <tr>
        <td height="20" colspan="7">
            &nbsp;</td>
    </tr>
</table>
<% 
	next 
'------------------------------------------------------------------------------ ' // 디비관련 객체 반환 
'------------------------------------------------------------------------------ SET adoRS = Nothing 
'SET adoConn = nothing 
adoConn.close 

%>
