using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class PortletInfo
    {
        public PortletInfo() { }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }

        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public int PortletID { get; set; }
        /// <summary>
        /// 포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
        /// </summary>
        public string PortletCode { get; set; }
        public string PortletType { get; set; }
        public string PortletTypeName { get; set; }
        /// <summary>
        /// UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)
        /// </summary>
        public string UIType { get; set; }
        /// <summary>
        /// UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
        /// </summary>
        public string UITemplate { get; set; }
        public string UIURL { get; set; }
        /// <summary>
        /// Data 호출 형식 (R : RSS 호출 / S : SQL Storeprocedure Name)
        /// </summary>
        public string DataType { get; set; }
        public string DataURL { get; set; }
        /// <summary>
        /// More Link 형식 (P : 팝업창 /  N : 새창 / C : 하단 프레임)
        /// </summary>
        public string LinkType { get; set; }
        public string LinkPopupSize { get; set; }
        public string LinkURL { get; set; }
        /// <summary>
        /// 개인화허용유무(1 : 허용 / 0 : 불가)
        /// </summary>
        public string IsPesonalization { get; set; }
        public string ActivePortlet { get; set; }
        public string SendMethod { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Description { get; set; }
        public string PhotoName { get; set; }
        public string IsAdmin { get; set; }

        //public string CatName { get; set; }

        public string CatNameBase { get; set; }
        public string CatNameKo { get; set; }
        public string CatNameEn { get; set; }
        public string CatNameJa { get; set; }
        public string CatNameFr { get; set; }
        public string CatNameZhcn { get; set; }
        public string CatNameZhtw { get; set; }

        //public string PathName { get; set; }

        public string PathNameBase { get; set; }
        public string PathNameKo { get; set; }
        public string PathNameEn { get; set; }
        public string PathNameJa { get; set; }
        public string PathNameFr { get; set; }
        public string PathNameZhcn { get; set; }
        public string PathNameZhtw { get; set; }

        public string TitleName { get; set; }

        public int PortletIDCnt { get; set; }
        public int RoleIDCnt { get; set; }
        public string ParamValue { get; set; }
        public int TPortletID { get; set; }

        public string FileDel { get; set; }


    }
}
