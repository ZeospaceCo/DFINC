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
<OBJECT RUNAT=server PROGID=ADODB.RecordSet ID=adoBeRS ></OBJECT> 
<OBJECT RUNAT=server PROGID=ADODB.RecordSet ID=adoInsRS ></OBJECT> 
<OBJECT RUNAT=server PROGID=ADODB.RecordSet ID=adoTable ></OBJECT> 

<% 
	 
	Response.Expires = -1000 

	ProjectName = "TABLE INFORMATION" 
	YourName = "심재운" '##작성자 이름
	

'####### Db 연결 (각자의 DB명에 맞게 연결코드를 완성하면 됩니다.) #### 
	adoConn.open "Provider=SQLOLEDB.1;Data Source=zeospace-20;Initial Catalog=DeskPluseip40;User ID=deskpluseip;Password=deskplus;Persist Security Info=True"  

   
    itableName = request("tablename")


    SQL = "SELECT  " &_
    "    MAX(ORDINAL_POSITION) AS ORDINAL_POSITION, TABLE_CATALOG, TABLE_NAME, COLUMN_NAME,  " &_
    "    DATA_TYPE, LENGTH, IS_NULLABLE, " &_
    "    MAX(PK) AS PK, MAX(CHARACTER) AS CHARACTER, MAX(COLLATION_NAME) AS COLLATION_NAME " &_
    "   FROM ( " &_
    "       SELECT  " &_
	    "    A.ORDINAL_POSITION, " &_
	    "    A.TABLE_CATALOG , " &_
	    "    A.TABLE_NAME , " &_
	    "    A.COLUMN_NAME , " &_
	    "    A.DATA_TYPE , " &_
	    "    ISNULL(A.CHARACTER_MAXIMUM_LENGTH,'')AS LENGTH , " &_
	    "    A.IS_NULLABLE , " &_
	    "    ISNULL(B.CONSTRAINT_NAME,'')AS PK , " &_
	    "    ISNULL(A.CHARACTER_SET_NAME,'')AS CHARACTER , " &_
	    "    ISNULL(A.COLLATION_NAME,'')AS COLLATION_NAME  " &_
        "   FROM INFORMATION_SCHEMA.COLUMNS A  " &_
        "   LEFT OUTER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE B  " &_
        "   ON A.TABLE_NAME = B.TABLE_NAME  " &_
        "   AND A.COLUMN_NAME = B.COLUMN_NAME LEFT OUTER JOIN syscolumns C  " &_
        "   ON C.ID = object_id(A.TABLE_NAME)  " &_
        "   AND A.COLUMN_NAME = C.NAME AND C.COLSTAT & 1 = 1  " &_
        "   where a.table_name = '"&itableName&"' " &_
    "   ) AS X " &_
    "   group by TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, DATA_TYPE, LENGTH, IS_NULLABLE " &_
    "   ORDER BY ORDINAL_POSITION ASC "
        
        'response.write SQL & "<br>"  

	adoRS.open SQL, adoConn, 0 
    adoBeRS.open SQL, adoConn, 0 
    adoInsRS.open SQL, adoConn, 0 
	
%> 


<% 
cnt = 0
	Do Until adoRS.EOF
	
	    sqldb = adoRS("data_type")
	    sqllen = adoRS("length")
        sqlcolmn = adoRS("COLUMN_NAME")
        isnulls = ""
    	
	    if sqldb = "varchar" or sqldb = "nvarchar" or sqldb = "char" or sqldb = "nchar" then
            sqllen = "," & sqllen
        else
            sqllen = ""
        end if
    	
	    if sqldb = "int" Or sqldb = "int identity" then
	        sqldb = "Int"
        elseif sqldb = "datetime" then
	        sqldb = "DateTime"
        elseif sqldb = "ntext" then
	        sqldb = "NText"
            isnulls = ".ToNull()"
        elseif sqldb = "nvarchar" then
	        sqldb = "NVarChar"	
            isnulls = ".ToNull()"
        elseif sqldb = "nchar" then
	        sqldb = "NChar"	
            isnulls = ".ToNull()"
        elseif sqldb = "varchar" then
	        sqldb = "VarChar"	
            isnulls = ".ToNull()"
        elseif sqldb = "char" then
	        sqldb = "Char"
            isnulls = ".ToNull()"
        elseif sqldb = "bigint" then
	        sqldb = "BigInt"
        elseif sqldb = "tinyint" then
		        sqldb = "TinyInt"
        end if 
    
    
    
    'response.Write "parms["&cnt&"].Value = _be."&adoRS("column_name")&";<br>"
    response.Write "cmd.Parameters.Add(""@"+sqlcolmn+""", SqlDbType."&sqldb&""&sqllen&").Value = _info."&sqlcolmn&""&isnulls&";<br>"
     	
	
	    'response.Write "new SqlParameter(PARM_" & UCase(adoRS("column_name")) & ", SqlDbType." & sqldb & sqllen  &"),<br>"
    	adoRS.MoveNext  '다음레코드로 이동
    	cnt = cnt + 1
	Loop
    
    response.Write "<br>"
    Do Until adoBeRS.EOF
	
	    sqldb = adoBeRS("data_type")
	    sqllen = adoBeRS("length")
        sqlcolmn = adoBeRS("COLUMN_NAME")
    	
	    if sqldb = "varchar" or sqldb = "nvarchar" or sqldb = "char" or sqldb = "nchar" then
            sqllen = "," & sqllen
        else
            sqllen = ""
        end if
    	
	    if sqldb = "int" Or sqldb = "int identity" then
	        sqldb = "int"
        elseif sqldb = "datetime" then
	        sqldb = "DateTime"
        elseif sqldb = "bigint" then
	        sqldb = "long"
        elseif sqldb = "tinyint" then
		        sqldb = "int"
        else
            sqldb = "string"
        end if 
    
    response.Write "public " & sqldb & " " & sqlcolmn & " {get;set;}"
    response.Write "<br>"
     	
	
	    'response.Write "new SqlParameter(PARM_" & UCase(adoRS("column_name")) & ", SqlDbType." & sqldb & sqllen  &"),<br>"
    	adoBeRS.MoveNext  '다음레코드로 이동
    	cnt = cnt + 1
	Loop	


adoBeRS.close 

response.Write "<br>"

    parameters = ""
    parameters1 = ""
    parameters2 = ""
    parameters3 = ""
    parameters4 = ""
    parameters5 = ""
    parameters6 = ""
    parameters7 = ""
    parameters8 = ""
    '*******************   

    Do Until adoInsRS.EOF
	
	    sqldb = adoInsRS("data_type")
	    sqllen = adoInsRS("length")
        sqlcolmn = adoInsRS("COLUMN_NAME")    
    
    
    'response.Write "parms["&cnt&"].Value = _be."&adoRS("column_name")&";<br>"
    'response.Write "cmd.Parameters.Add(""@"+sqlcolmn+""", SqlDbType."&sqldb&""&sqllen&").Value = _info."&sqlcolmn&""&isnulls&";<br>"
     	

        
        if sqldb <> "int" and sqldb <> "datetime" and sqldb <> "ntext" then
            parameters = parameters &  "@" & sqlcolmn & " " & sqldb & "(" & sqllen & ")"                        
        else
            parameters = parameters & "@" & sqlcolmn & " " & sqldb
        end if 


        if sqldb <> "int" and sqldb <> "datetime" and sqldb <> "bigint" then            
            parameters8 = parameters8 & "" & sqlcolmn & " = _reader[""" & sqlcolmn & """].ToString().ToNull(),"        
        elseif  sqldb = "datetime" then            
            parameters8 = parameters8 & "" & sqlcolmn & " = (_reader[""" & sqlcolmn & """] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader[""" & sqlcolmn & """],"        
        else            
            parameters8 = parameters8 & "" & sqlcolmn & " = (_reader[""" & sqlcolmn & """] == DBNull.Value) ? -1 : _reader[""" & sqlcolmn & """].ToString().ToNumber&ltint>()," 
        end if 

        parameters1 = parameters1 & "@" & sqlcolmn
        parameters2 = parameters2 & sqlcolmn
        parameters3 = parameters3 & sqlcolmn & " = " & "@" & sqlcolmn
        parameters4 = parameters4 & "be." & sqlcolmn & " = " & "reader[""" & sqlcolmn & """].ToString().ToNull();"
        parameters5 = parameters5 & "info." & sqlcolmn & " = " & "be." & sqlcolmn & ";"
        parameters6 = parameters6 & "info." & sqlcolmn & " = " & "dtr[""" & sqlcolmn & """].ToString();"
        parameters7 = parameters7 & "info." & sqlcolmn & " = " & "ctb" & sqlcolmn & ".Text;"
        

        parameters = parameters & ",<br>"
        parameters1 = parameters1  & ",<br>"
        parameters2 = parameters2  & ",<br>"
        parameters3 = parameters3  & ",<br>"
        parameters4 = parameters4  & "<br>"
        parameters5 = parameters5  & "<br>"
        parameters6 = parameters6  & "<br>"
        parameters7 = parameters7  & "<br>"
        parameters8 = parameters8  & "<br>"
	
	    'response.Write "new SqlParameter(PARM_" & UCase(adoRS("column_name")) & ", SqlDbType." & sqldb & sqllen  &"),<br>"
    	adoInsRS.MoveNext  '다음레코드로 이동
    	cnt = cnt + 1
	Loop
    
    adoInsRS.close 
    response.write parameters
    response.Write "<br>"
    response.write parameters1
    response.Write "<br>"
    response.write parameters2
    response.Write "<br>"
    response.write parameters3
    response.Write "<br>"
    response.write parameters4
    response.Write "<br>"
    response.write parameters5
    response.Write "<br>"
    response.write parameters6
    response.Write "<br>"
    response.write parameters7
    response.Write "<br>"
    response.write parameters8
    response.Write "<br>"

'********************




SQL = "select table_name from INFORMATION_SCHEMA.tables order by table_name "   
    adoTable.open SQL, adoConn, 0 

    Do Until adoTable.EOF
        response.Write "<a href='/DeskPlusEIP/Util/CodeTool/dal_parameter.asp?tablename=" & adoTable("table_name")&"'>"
        response.Write adoTable("table_name")
        response.Write "</a>"
        response.Write "<br>"
    	adoTable.MoveNext  '다음레코드로 이동
	Loop	


    adoTable.close 

adoConn.close 

%>
