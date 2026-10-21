using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    /// <summary>
    /// 복합명사의 개별단어에 대한 정보를 담고있는 클래스 
    /// </summary>
    public class CompoundEntry
    {
        private string word;
        private int offset = -1;
        private bool exist = true;
        private char pos = PatternConstants.POS_NOUN;

        public CompoundEntry()
        {

        }

        public CompoundEntry(string w)
        {
            this.word = w;
        }

        public CompoundEntry(string w, int o) : this(w)
        {
            this.offset = o;
        }

        public CompoundEntry(string w, int o, bool @is) : this(w, o)
        {
            this.exist = @is;
        }

        public CompoundEntry(string w, int o, bool @is, char p)
            : this(w, o, @is)
        {
            this.pos = p;
        }

        public virtual string Word
        {
            set
            {
                this.word = value;
            }
            get
            {
                return this.word;
            }
        }

        public virtual int Offset
        {
            set
            {
                this.offset = value;
            }
            get
            {
                return this.offset;
            }
        }



        public virtual bool Exist
        {
            get
            {
                return exist;
            }
            set
            {
                this.exist = value;
            }
        }


        public virtual char Pos
        {
            get
            {
                return pos;
            }
            set
            {
                this.pos = value;
            }
        }
    }
}
