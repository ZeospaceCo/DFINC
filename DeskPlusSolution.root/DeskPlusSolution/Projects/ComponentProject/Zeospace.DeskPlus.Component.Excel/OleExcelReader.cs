//원본 http://www.codeproject.com/office/excel_using_oledb.asp?df=100&forumid=115836&exp=0&select=1937095
//수정본 http://www.bangsil.pe.kr/Board/Read.aspx?BoardId=da05ba1b-3e9b-4d16-b8f8-ff2e4f7514c5&PostId=bb46fa1f-d300-4ad2-aebb-9321f492b343

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Zeospace.DeskPlus.Component.Excel
{

    /// <summary>
    /// OleExcelHandler V1.1
    /// OLEDB를 이용한 엑셀 핸들러
    /// </summary>
	public class OleExcelReader : IDisposable
	{
		#region Variables
        private int? pkColIndex;
		private string fileName;
		private int imex = 1;
		private bool hasHeaders = false;
		private string[] sheetName;
		private string[] sheetRange;
		private bool isConnection = false;
		private OleDbConnection con;
		private OleDbCommand selectCmd;
		private OleDbCommand updateCmd;
		#endregion

		#region properties
		/// <summary>
		/// 엑셀 파일명
		/// </summary>
		public string Filename
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		/// <summary>
		/// 시트명
		/// </summary>
		public string[] SheetName
		{
			get
			{
				return this.sheetName;
			}
			set
			{
				this.sheetName = value;
			}
		}

		/// <summary>
		/// 구간
		/// 전체를 선택할때는 ""이다.
		/// </summary>
		public string[] SheetRange
		{
			get
			{
				if (this.sheetRange == null)
				{
					return new string[this.sheetName.Length];
				}
				return this.sheetRange;
			}
			set
			{
				this.sheetRange = value;
			}
		}

		/// <summary>
		/// 연결유지
		/// </summary>
		public bool KeepConnectionOpen
		{
			get
			{
				return this.isConnection;
			}
			set
			{
				this.isConnection = value;
			}
		}

		/// <summary>
		/// 헤더(첫줄이 컬럼명이 있으면 true)
		/// </summary>
		public bool HasHeaders
		{
			get
			{
				return this.hasHeaders;
			}
			set
			{
				this.hasHeaders = value;
			}
		}

		/// <summary>
		/// You can add the option IMEX=1; to the Excel connect string in the OpenDatabase method. For example: 
		///	Set Db = OpenDatabase("C:\Temp\Book1.xls", _False, True, "Excel 8.0; HDR=NO; IMEX=1;")
		///	NOTE: Setting IMEX=1 tells the driver to use Import mode. In this state, the registry setting ImportMixedTypes=Text will be noticed. This forces mixed data to be converted to text. For this to work reliably, you may also have to modify the registry setting, TypeGuessRows=8. The ISAM driver by default looks at the first eight rows and from that sampling determines the datatype. If this eight row sampling is all numeric, then setting IMEX=1 will not convert the default datatype to Text; it will remain numeric. 
		///		You must be careful that IMEX=1 not be used indiscriminately. This is IMPORT mode, so the results may be unpredictable if you try to do appends or updates of data in this mode. 
		///			0 is Export mode
		///			1 is Import mode
		///			2 is Linked mode (full update capabilities)
		/// </summary>
		public int Imex
		{
			get
			{
				return this.imex;
			}
			set
			{
				this.imex = value;
			}
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

		#region 정리된 부분

		#region 헤더가 없으면 컬럼명을 임의로 설정해준다.
		public string ColName(int intCol)
		{
			string sColName = "";
			if (intCol < 26)
			{
				sColName = Convert.ToString(Convert.ToChar((Convert.ToByte((char)'A') + intCol)));
			}
			else
			{
				int intFirst = ((int)intCol / 26);
				int intSecond = ((int)intCol % 26);
				sColName = Convert.ToString(Convert.ToByte((char)'A') + intFirst);
				sColName += Convert.ToString(Convert.ToByte((char)'A') + intSecond);
			}
			return sColName;
		}

		public int ColNumber(string strCol)
		{
			strCol = strCol.ToUpper();
			int intColNumber = 0;
			if (strCol.Length > 1)
			{
				intColNumber = Convert.ToInt16(Convert.ToByte(strCol[1]) - 65);
				intColNumber += Convert.ToInt16(Convert.ToByte(strCol[1]) - 64) * 26;
			}
			else
			{
				intColNumber = Convert.ToInt16(Convert.ToByte(strCol[0]) - 65);
			}
			return intColNumber;
		}
		#endregion

		#region 시트의 이름 얻기
		/// <summary>
		/// 시트의 이름들의 배열
		/// </summary>
		/// <returns></returns>
		public String[] GetExcelSheetNames()
		{
			DataTable dt = null;

			try
			{
				if (con == null)
				{
					this.Open();
				}

				// Get the data table containing the schema
				dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

				if (dt == null)
				{
					return null;
				}

				String[] excelSheets = new String[dt.Rows.Count];
				int i = 0;

				// Add the sheet name to the string array.
				foreach (DataRow row in dt.Rows)
				{
					string strSheetTableName = row["TABLE_NAME"].ToString();
					if (strSheetTableName.EndsWith("$"))
					{
						excelSheets[i] = strSheetTableName.Substring(0, strSheetTableName.Length - 1);
					}
					else
					{
						excelSheets[i] = strSheetTableName;
					}
					i++;
				}
				return excelSheets;
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				// Clean up.
				if (this.KeepConnectionOpen == false)
				{
					this.Close();
				}
				if (dt != null)
				{
					dt.Dispose();
					dt = null;
				}
			}
		}
		#endregion

		#region 엑셀 파일 연결 설정(Private메소드)
		private string ExcelConnectionOptions()
		{
			string strOpts = "Imex=" + this.imex + ";";
			if (this.hasHeaders)
			{
				strOpts += "HDR=Yes;";
			}
			else
			{
				strOpts += "HDR=No;";
			}
			return strOpts;
		}

        private string ExcelConnection()
        {
            string tmpStr = Path.GetExtension(this.fileName);

            if (tmpStr == "xls")
            {

                return
                    @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    @"Data Source=" + this.fileName + ";" +
                    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                    @"Excel 8.0;" + ExcelConnectionOptions() + Convert.ToChar(34).ToString();
            }
            else
            {
                return
                    @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    @"Data Source=" + this.fileName + ";" +
                    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                    @"Excel 12.0;" + ExcelConnectionOptions() + Convert.ToChar(34).ToString();
            } 
        }
        #endregion

		#region 파일열기 / 파일닫기
		private void Open()
		{
			try
			{
				if (this.con != null)
				{
					if (this.con.State == ConnectionState.Open)
					{
						this.con.Close();
					}
					this.con = null;
				}

				if (System.IO.File.Exists(this.fileName) == false)
				{
					throw new Exception("Excel file " + this.fileName + "could not be found.");
				}
				this.con = new OleDbConnection(ExcelConnection());
				this.con.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Close()
		{
			if (this.con != null)
			{
				if (this.con.State != ConnectionState.Closed)
				{
					this.con.Close();
				}
				this.con.Dispose();
				this.con = null;
			}
		}
		#endregion

		#region Select 쿼리
		private bool SetSheetQuerySelect()
		{
			try
			{
				if (this.con == null)
				{
					throw new Exception("Connection is unassigned or closed.");
				}

				if (this.sheetName.Length == 0)
				{
					throw new Exception("Sheetname was not assigned.");
				}

				string cmd = "";
				for (int i = 0; i < this.sheetName.Length; i++)
				{
					cmd += @"SELECT * FROM [" + this.sheetName[i] + "$" + this.SheetRange[i] + "]" + System.Environment.NewLine;
				}
				this.selectCmd = new OleDbCommand(cmd, con);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region 기본키 생성
		/// <summary>
		/// 기본키 설정
		/// </summary>
		/// <param name="dt"></param>
		private void SetPrimaryKey(DataTable dt)
		{
			if (this.pkColIndex != null && dt.Rows.Count > 0)
			{
				DataColumn[] dc;
				dc = new DataColumn[1] { dt.Columns[(int)this.pkColIndex] };
				dt.PrimaryKey = dc;
			}
		}
		#endregion

		#region DataTable을 만든다
		/// <summary>
		/// DataTable을 만든다.
		/// </summary>
		/// <param name="strTableName"></param>
		/// <returns></returns>
		public DataTable GetTable(string strTableName)
		{
			try
			{
				//Open and query
				if (this.con == null)
				{
					this.Open();
				}
				if (this.con.State != ConnectionState.Open)
				{
					throw new Exception("Connection cannot open error.");
				}
				if (this.SetSheetQuerySelect() == false)
				{
					return null;
				}

				//Fill table
				OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
				this.selectCmd.CommandTimeout = 200;
				oleAdapter.SelectCommand = selectCmd;
				DataTable dt = new DataTable(strTableName);
				oleAdapter.FillSchema(dt, SchemaType.Source);
				oleAdapter.Fill(dt);
				if (this.hasHeaders == false)	//헤더가 없으면 임의로 컬럼명을 생성한다.
				{
					if (this.sheetRange[0].IndexOf(":") > 0)
					{
						string FirstCol = this.sheetRange[0].Substring(0, this.sheetRange[0].IndexOf(":") - 1);
						int intCol = this.ColNumber(FirstCol);
						for (int intI = 0; intI < dt.Columns.Count; intI++)
						{
							dt.Columns[intI].Caption = ColName(intCol + intI);
						}
					}
				}
				this.SetPrimaryKey(dt);
				//Cannot delete rows in Excel workbook
				dt.DefaultView.AllowDelete = false;

				//Clean up
				this.selectCmd.Dispose();
				this.selectCmd = null;
				oleAdapter.Dispose();
				oleAdapter = null;
				if (this.KeepConnectionOpen == false)
				{
					this.Close();
				}
				return dt;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region DataSet을 만든다
		/// <summary>
		/// DataSet을 만든다.
		/// </summary>
		/// <param name="strTableName"></param>
		/// <returns></returns>
		public DataSet GetDataSet()
		{
			try
			{
				//Open and query
				if (this.con == null)
				{
					this.Open();
				}
				if (this.con.State != ConnectionState.Open)
				{
					throw new Exception("Connection cannot open error.");
				}

				//Fill table
				OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
				DataSet ds = new DataSet();
				for (int i = 0; i < this.sheetName.Length; i++)
				{
					oleAdapter.SelectCommand = new OleDbCommand(@"SELECT * FROM [" + this.sheetName[i] + "$" + this.SheetRange[i] + "]", this.con);
					DataTable dt = new DataTable(this.sheetName[i]);
					oleAdapter.FillSchema(dt, SchemaType.Source);
					oleAdapter.Fill(dt);
					ds.Tables.Add(dt);
				}

				if (this.hasHeaders == false)	//헤더가 없으면 임의로 컬럼명을 생성한다.
				{
					for (int i = 0; i < ds.Tables.Count; i++)
					{
						if (this.sheetRange[i].IndexOf(":") > 0)
						{
							string FirstCol = this.sheetRange[i].Substring(0, this.sheetRange[i].IndexOf(":") - 1);
							int intCol = this.ColNumber(FirstCol);
							for (int intI = 0; intI < ds.Tables[i].Columns.Count; intI++)
							{
								ds.Tables[i].Columns[intI].Caption = this.ColName(intCol + intI);
							}
						}
					}
				}

				foreach (DataTable dt in ds.Tables)
				{
					this.SetPrimaryKey(dt);
				}

				//Clean up
				oleAdapter.Dispose();
				oleAdapter = null;
				if (this.KeepConnectionOpen == false)
				{
					this.Close();
				}
				return ds;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#endregion


		public DataTable SetTable(DataTable dt)
		{
			try
			{
				DataTable dtChanges = dt.GetChanges();
				if (dtChanges == null)
				{
					throw new Exception("There are no changes to be saved!");
				}
				this.CheckPKExists(dt);
				//Open and query
				if (this.con == null)
				{
					this.Open();
				}
				if (this.con.State != ConnectionState.Open)
				{
					throw new Exception("Connection cannot open error.");
				}
				if (this.SetSheetQuerySelect() == false)
				{
					return null;
				}

				//Fill table
				OleDbDataAdapter oleAdapter = SetSheetQueryAdapter(dtChanges);

				oleAdapter.Update(dtChanges);
				//Clean up
				this.selectCmd.Dispose();
				this.selectCmd = null;
				oleAdapter.Dispose();
				oleAdapter = null;
				if (this.KeepConnectionOpen == false)
				{
					Close();
				}
				return dt;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void CheckPKExists(DataTable dt)
		{
			if (dt.PrimaryKey.Length == 0)
			{
				this.SetPrimaryKey(dt);
			}
			else
			{
				throw new Exception("Provide an primary key to the datatable");
			}
		}

		private OleDbDataAdapter SetSheetQueryAdapter(DataTable dt)
		{
			// Deleting in Excel workbook is not possible
			//So this command is not defined
			try
			{
				if (this.con == null)
				{
					throw new Exception("Connection is unassigned or closed.");
				}


				if (this.sheetName.Length == 0)
				{
					throw new Exception("Sheetname was not assigned.");
				}

				OleDbDataAdapter oleda = new OleDbDataAdapter(selectCmd);
				string strUpdate = "";
				string strInsertPar = "";
				string strInsert = "";
				string strWhere = this.AddWithAnd("", dt.Columns[(int)this.pkColIndex].ColumnName + "=?");

				strWhere = " Where " + strWhere;

				for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
				{
					strInsert = AddWithComma(strInsert, dt.Columns[iCol].ColumnName);
					strInsertPar = AddWithComma(strInsertPar, "?");
					strUpdate = AddWithComma(strUpdate, dt.Columns[iCol].ColumnName) + "=?";
				}

				string strTable = "[" + this.SheetName + "$" + this.SheetRange + "]";
				strInsert = "INSERT INTO " + strTable + "(" + strInsert + ") Values (" + strInsertPar + ")";
				strUpdate = "Update " + strTable + " Set " + strUpdate + strWhere;


				oleda.InsertCommand = new OleDbCommand(strInsert, con);
				oleda.UpdateCommand = new OleDbCommand(strUpdate, con);
				OleDbParameter oleParIns = null;
				OleDbParameter oleParUpd = null;
				for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
				{
					oleParIns = new OleDbParameter("?", dt.Columns[iCol].DataType.ToString());
					oleParUpd = new OleDbParameter("?", dt.Columns[iCol].DataType.ToString());
					oleParIns.SourceColumn = dt.Columns[iCol].ColumnName;
					oleParUpd.SourceColumn = dt.Columns[iCol].ColumnName;
					oleda.InsertCommand.Parameters.Add(oleParIns);
					oleda.UpdateCommand.Parameters.Add(oleParUpd);
					oleParIns = null;
					oleParUpd = null;
				}

				oleParUpd = new OleDbParameter("?", dt.Columns[(int)this.pkColIndex].DataType.ToString());
				oleParUpd.SourceColumn = dt.Columns[(int)this.pkColIndex].ColumnName;
				oleParUpd.SourceVersion = DataRowVersion.Original;
				oleda.UpdateCommand.Parameters.Add(oleParUpd);
				return oleda;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		#region simple utilities
		private string AddWithComma(string strSource, string strAdd)
		{
			if (strSource != "")
			{
				strSource = strSource += ", ";
			}
			return strSource + strAdd;
		}

		private string AddWithAnd(string strSource, string strAdd)
		{
			if (strSource != "")
			{
				strSource = strSource += " and ";
			}
			return strSource + strAdd;
		}
		#endregion



		#region command Singe Value Update
		private bool SetSheetQuerySingelValUpdate(string strVal)
		{
			try
			{
				if (this.con == null)
				{
					throw new Exception("Connection is unassigned or closed.");
				}

				if (this.sheetName.Length == 0)
				{
					throw new Exception("Sheetname was not assigned.");
				}

				this.updateCmd = new OleDbCommand(
					@" Update ["
					+ this.sheetName
					+ "$" + this.sheetRange
					+ "] set F1=" + strVal, con);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}


		}
		#endregion


		#region Get/Set Single Value

		public void SetSingleCellRange(int sheetIndex, string strCell)
		{
			this.sheetRange[sheetIndex] = strCell + ":" + strCell;
		}

		public object GetValue(int sheetIndex, string strCell)
		{
			this.SetSingleCellRange(sheetIndex, strCell);
			object objValue = null;
			//Open and query
			if (this.con == null)
			{
				this.Open();
			}
			if (this.con.State != ConnectionState.Open)
			{
				throw new Exception("Connection is not open error.");
			}

			if (this.SetSheetQuerySelect() == false)
			{
				return null;
			}
			objValue = this.selectCmd.ExecuteScalar();

			selectCmd.Dispose();
			selectCmd = null;
			if (KeepConnectionOpen == false) Close();
			return objValue;
		}

		public void SetValue(int sheetIndex, string strCell, object objValue)
		{
			try
			{
				SetSingleCellRange(sheetIndex, strCell);
				//Open and query
				if (con == null) Open();
				if (con.State != ConnectionState.Open)
					throw new Exception("Connection is not open error.");

				if (SetSheetQuerySingelValUpdate(objValue.ToString()) == false) return;
				objValue = this.updateCmd.ExecuteNonQuery();

				this.updateCmd.Dispose();
				this.updateCmd = null;
				if (KeepConnectionOpen == false) Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (this.updateCmd != null)
				{
					this.updateCmd.Dispose();
					this.updateCmd = null;
				}
			}

		}
		#endregion

		#region IDisposable 멤버

		void IDisposable.Dispose()
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
		}

		#endregion
	}
}
