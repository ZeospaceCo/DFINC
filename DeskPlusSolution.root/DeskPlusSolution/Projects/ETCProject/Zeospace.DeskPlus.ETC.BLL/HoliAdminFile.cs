using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.ETC.BLL
{
    public class HoliAdminFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : ellepooky
        /// 작성일자 : 2018-11-28
        /// 작성목적 : 엑셀 업로드
        /// </summary>
        /// <returns></returns>
        public bool Add(List<BE.HoliInfo> _hl, string _spoolPath, string _holiPath)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    string tmpHoli = FileHelper.GetServerMapPath(_holiPath);
                    if (!Directory.Exists(tmpHoli))
                    {
                        Directory.CreateDirectory(tmpHoli);
                    }

                    string tmpSpool = FileHelper.GetServerMapPath(_spoolPath);

                    foreach (BE.HoliInfo info in _hl)
                    {
                        // DB 처리
                        DAL.HoliAdmin dal = new DAL.HoliAdmin();

                        string tmpFilePath = dal.Add(info);

                        if (tmpFilePath == "-1")
                        {
                            return false;
                        }
                        else
                        {
                            //이전파일 삭제
                            if (tmpFilePath != "")
                            {
                                System.IO.File.Delete(FileHelper.GetServerMapPath(tmpFilePath));
                            }
                        }

                        // File 처리                       
                        System.IO.File.Move(tmpSpool + info.FileName, tmpHoli + info.FileGUID);
                    }

                    if (Directory.Exists(tmpSpool))
                    {
                        Directory.Delete(tmpSpool);
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return false;
            }
        }
        /// <summary>
        /// 사용자 UserID 
        /// </summary>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public int GetUserID(string _loginID)
        {
            Zeospace.DeskPlus.Account.DAL.User dal = new Zeospace.DeskPlus.Account.DAL.User();
            int userID = 0;

            DataTableReader dtr = dal.GetUserByLogin(1, _loginID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        userID = dtr["UserID"].ToString().ToNumber<int>();
                    }
                }
            }

            return userID;
        }

        #endregion
    }
}
