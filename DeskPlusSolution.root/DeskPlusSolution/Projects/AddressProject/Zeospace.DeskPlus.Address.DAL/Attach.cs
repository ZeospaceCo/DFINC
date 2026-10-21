using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Address.BE;
using System.Data;

namespace Zeospace.DeskPlus.Address.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        
        private BE.AttachInfo GetAttach(IDataReader _reader)
        {
            return new AttachInfo(
            (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
            (_reader["OwnerID"] == DBNull.Value) ? -1 : (int)_reader["OwnerID"],
            (_reader["AttachFileID"] == DBNull.Value) ? -1 : (int)_reader["AttachFileID"],
            _reader["Gubun"].ToString(),
            _reader["FilePath"].ToString(),
            _reader["FileName"].ToString(),
            _reader["FileType"].ToString(),
            Convert.ToInt64(_reader["FileLength"]),
            _reader["FileGUID"].ToString()
            );
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<AttachInfo> GetAttachList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttach(reader));
            return list;
        }
    }
}
