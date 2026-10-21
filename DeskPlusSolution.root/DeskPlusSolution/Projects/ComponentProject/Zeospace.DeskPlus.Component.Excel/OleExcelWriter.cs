//원본 http://www.codeproject.com/office/excel_using_oledb.asp?df=100&forumid=115836&exp=0&select=1937095
//수정본 http://www.bangsil.pe.kr/Board/Read.aspx?BoardId=da05ba1b-3e9b-4d16-b8f8-ff2e4f7514c5&PostId=bb46fa1f-d300-4ad2-aebb-9321f492b343

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace Zeospace.DeskPlus.Component.Excel
{
    /// <summary>
    /// OleExcelWriter V1.2
    /// OLEDB를 이용한 엑셀 핸들러
    /// </summary>
    public class OleExcelWriter
    {
        public delegate void RowCount(object sender, RowCountEventArgs e);
        public event RowCount OnRowCount;

        #region Variables
        private DataSet curDs;
        private int pkColIndex = 0;
        private string fileName;
        private int imex = 0;
        private bool hasHeader = true;
        private string sheetRange;
        private bool isConnection = false;
        private OleDbConnection con;
        private OleDbCommand selectCmd;
        private List<string[]> colTypeList;
        private int rowCount = 0;
        #endregion

        #region 소멸자
        public void Dispose()
        {
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
            if (selectCmd != null)
            {
                selectCmd.Dispose();
                selectCmd = null;
            }
            // Dispose of remaining objects.
        }
        #endregion

        #region properties

        public List<string[]> ColTypeList
        {
            set
            {
                this.colTypeList = value;
            }
        }
        /// <summary>
        /// 바인딩할 테이블
        /// </summary>
        public DataSet CurDs
        {
            set
            {
                this.curDs = value;
            }
            get
            {
                return this.curDs;
            }
        }

        /// <summary>
        /// 엑셀 파일명
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 구간
        /// 전체를 선택할때는 ""이다.
        /// </summary>
        public string SheetRange
        {
            get { return sheetRange; }
            set { sheetRange = value; }
        }

        /// <summary>
        /// 연결유지
        /// </summary>
        public bool KeepConnectionOpen
        {
            get { return isConnection; }
            set { isConnection = value; }
        }

        /// <summary>
        /// 해더가 있는가.
        /// </summary>
        public bool HasHeader
        {
            get { return this.hasHeader; }
            set { this.hasHeader = value; }
        }


        /// <summary>
        /// 기본키
        /// </summary>
        public int PkColIndex
        {
            set
            {
                this.pkColIndex = value;
            }
        }
        #endregion

        #region 엑셀 파일 연결 설정(Private메소드)
        private string ExcelConnectionOptions()
        {
            string strOpts = "Imex=" + this.imex + ";";
            strOpts += "HDR=Yes;";
            return strOpts;
        }

        private string ExcelConnection()
        {
            return
                @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                @"Data Source=" + this.fileName + ";" +
                @"Extended Properties=" + Convert.ToChar(34).ToString() +
                @"Excel 8.0;" + ExcelConnectionOptions() + Convert.ToChar(34).ToString();
        }
        #endregion

        #region 파일열기 / 파일닫기
        public void Open()
        {
            try
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con = null;
                }

                if (System.IO.File.Exists(this.fileName))
                {
                    System.IO.File.Delete(this.fileName);
                    //throw new Exception("파일이 이미 존재 합니다.");
                }
                con = new OleDbConnection(ExcelConnection());
                con.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            if (con != null)
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
                con.Dispose();
                con = null;
            }
        }
        #endregion

        #region 기본키 생성
        /// <summary>
        /// 기본키 설정
        /// </summary>
        /// <param name="dt"></param>
        private void SetPrimaryKey(System.Data.DataTable dt)
        {
            DataColumn[] dc;
            dc = new DataColumn[this.pkColIndex];
            dt.PrimaryKey = dc;
        }
        #endregion

        public void CreateSheet()
        {
            for (int i = 0; i < this.curDs.Tables.Count; i++)
            {
                this.Command("CREATE TABLE [" + this.curDs.Tables[i].TableName + "] (" + this.CreatColName(this.curDs.Tables[i], i) + ")");
            }
        }

        private string CreatColName(DataTable dt, int index)
        {
            string[] colType = this.colTypeList[index];
            if (dt.Columns.Count != colType.Length)
            {
                throw new Exception("컬럼의 이름과 타입의 수가 다릅니다.");
            }
            string result = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                result += "[" + dt.Columns[i].ColumnName + "] " + colType[i] + ",";
            }
            if (result.EndsWith(","))
            {
                result.Trim().Remove(result.Length - 1, 1);
            }
            return result.Substring(0, result.Length - 1);
        }

        private void Command(string sql)
        {
            //Open and query
            if (con == null) Open();
            if (con.State != ConnectionState.Open)
                throw new Exception("Connection cannot open error.");

            OleDbCommand command = new OleDbCommand(sql, con);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!this.KeepConnectionOpen)
                {
                    this.Close();
                }
            }
        }

        public void Binding()
        {
            foreach (System.Data.DataTable dt in this.curDs.Tables)
            {
                string col = "";
                string resultCol = "";
                foreach (DataColumn c in dt.Columns)
                {
                    col += "[" + c.ColumnName + "],";
                }
                if (col.EndsWith(","))
                {
                    resultCol = col.Substring(0, col.Length - 1);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Command(@"Insert Into [" + dt.TableName + "] (" + resultCol + ") values (" + this.CreateRowForInsert(dt, i) + ")");
                    this.OnRowCount(this, new RowCountEventArgs(++this.rowCount));
                }
            }
        }

        /// <summary>
        /// Creates the row for insert.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="rowCount">The row count.</param>
        /// <returns></returns>
        private string CreateRowForInsert(System.Data.DataTable dt, int rowCount)
        {
            string result = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[rowCount][i].ToString() == "NULL")
                {
                    result += "'',";
                }
                else
                {
                    result += "'" + this.Decoding(dt.Rows[rowCount][i].ToString()) + "',";
                }
            }
            if (result.EndsWith(","))
            {
                return result.Substring(0, result.Length - 1);
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Html을 디코딩 합니다.
        /// </summary>
        /// <param name="outputString">변환할 html 문자열</param>
        /// <returns></returns>
        private string Decoding(string inputString)
        {
            StringBuilder retVal = new StringBuilder(inputString.Length);
            if (!String.IsNullOrEmpty(inputString))
            {
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '\'':
                            retVal.Append("''");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
            }

            return retVal.ToString();
        }
    }
}
