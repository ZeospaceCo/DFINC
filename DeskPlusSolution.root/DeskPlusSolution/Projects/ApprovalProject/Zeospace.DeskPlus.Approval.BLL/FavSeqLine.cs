using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class FavSeqLine : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #endregion GET

        #region ADD

        #endregion ADD

        #region MOD

        /// <summary>
        /// 항목순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sortlist"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, string _sortlist)
        {
            try
            {
                if (_companyID < 0 || _sortlist.Equals("")) return false;

                string[] arr = _sortlist.TrimEnd(',').Split(',');

                DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!fsDal.Mod(_companyID, arr[i].ToNumber<int>(), i + 1))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.FavSeqLine[ModSeq] : " + ex);
                return false;
            }
        }

        #endregion MOD

        #region DEL

        #endregion DEL
    }
}