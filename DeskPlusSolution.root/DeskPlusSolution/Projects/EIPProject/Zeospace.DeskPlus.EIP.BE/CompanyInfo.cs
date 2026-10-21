using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class CompanyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_nameKo">The _name ko.</param>
        /// <param name="_nameEn">The _name en.</param>
        /// <param name="_nameJa">The _name ja.</param>
        /// <param name="_nameFr">The _name fr.</param>
        /// <param name="_nameZhcn">The _name ZHCN.</param>
        /// <param name="_nameZhtw">The _name ZHTW.</param>
        /// <param name="_companyEnglishName">Name of the _company english.</param>
        /// <param name="_homePage">The _home page.</param>
        public CompanyInfo(
        int	_companyID ,string	_nameKo ,string	_nameEn ,string	_nameJa ,string	_nameFr 
        ,string	_nameZhcn ,string	_nameZhtw ,string	_companyEnglishName ,string	_homePage )
  
         {
	         this.CompanyID	=	_companyID;
	         this.NameKo	=	_nameKo;
	         this.NameEn	=	_nameEn;
	         this.NameJa	=	_nameJa;
	         this.NameFr	=	_nameFr;
	         this.NameZhcn	=	_nameZhcn;
	         this.NameZhtw	=	_nameZhtw;
	         this.CompanyEnglishName	=	_companyEnglishName;
	         this.HomePage	=	_homePage;

        } 
        // DP_EIP_Company
        ///<summary>
        ///CompanyID
        ///</summary>
        public 	int	CompanyID	{ get; set; } 

        ///<summary>
        ///NameKo
        ///</summary>
        public 	string	NameKo	{ get; set; } 

        ///<summary>
        ///NameEn
        ///</summary>
        public 	string	NameEn	{ get; set; } 

        ///<summary>
        ///NameJa
        ///</summary>
        public 	string	NameJa	{ get; set; } 

        ///<summary>
        ///NameFr
        ///</summary>
        public 	string	NameFr	{ get; set; } 

        ///<summary>
        ///NameZhcn
        ///</summary>
        public 	string	NameZhcn	{ get; set; } 

        ///<summary>
        ///NameZhtw
        ///</summary>
        public 	string	NameZhtw	{ get; set; } 

        ///<summary>
        ///CompanyEnglishName
        ///</summary>
        public 	string	CompanyEnglishName	{ get; set; } 

        ///<summary>
        ///HomePage
        ///</summary>
        public 	string	HomePage	{ get; set; } 
    }
}
