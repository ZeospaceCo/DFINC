using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    /**
     * 명사의 유형별 분류
     * @author user
     *
     */
    public class NounProperty
    {
	    // 위치, 장소
	    public const String NP_LOCATION = "L";
	
	    // 물리 측정량 (속도, 각도)
        public const String NP_MEASURE = "M";
    }
}
