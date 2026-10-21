using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class Status
    {

        private int josaMaxStart = 0;

        private int eomiMaxStart = 0;

        private int maxStart = 0;

        public virtual void Apply(int num)
        {
            if (maxStart < num)
            {
                maxStart = num;
            }
        }

        public virtual int MaxStart
        {
            get
            {
                return maxStart;
            }
            set
            {
                this.maxStart = value;
            }
        }


        public virtual int JosaMaxStart
        {
            get
            {
                return josaMaxStart;
            }
            set
            {
                this.josaMaxStart = value;
            }
        }



        public virtual int EomiMaxStart
        {
            get
            {
                return eomiMaxStart;
            }
            set
            {
                this.eomiMaxStart = value;
            }
        }

    }
}
