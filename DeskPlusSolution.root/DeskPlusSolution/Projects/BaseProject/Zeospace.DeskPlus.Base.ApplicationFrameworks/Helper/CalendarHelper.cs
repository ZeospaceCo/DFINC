using System;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    public class CalendarHelper
    {
        private string[] yearMonthArr;
        private string[] weekArr;
        private int[] monthDayArr;

        private int lDAY;
        private int lMONTH;
        private int lYEAR;

        private int sDAY;
        private int sMONTH;
        private int sYEAR;

        private bool YoonYN;

        private string yearMonthData;

        public CalendarHelper()
        {
            this.yearMonthArr = YearToMonthData.Split(',');
            this.weekArr = new string[7] { "\uc77c", "\uc6d4", "\ud654", "\uc218", "\ubaa9", "\uae08", "\ud1a0" };
            this.monthDayArr = new int[12] { 0x1f, 0, 0x1f, 30, 0x1f, 30, 0x1f, 0x1f, 30, 0x1f, 30, 0x1f };
        }

        public string FromLunToSol(DateTime dt)
        {
            return this.FromLunToSol(dt.Year, dt.Month, dt.Day);
        }

        public string FromLunToSol(string yyyymmdd)
        {
            return this.FromLunToSol(DateTime.Parse(yyyymmdd));
        }

        public string FromLunToSol(int y, int m, int d)
        {
            int num1 = y;
            int num2 = m;
            int num3 = d;
            int num7 = num1 - 0x731;
            int num8 = num2 - 1;
            int num9;
            bool flag1 = false;
            int num12 = Convert.ToInt32(this.yearMonthArr[(num7 * 12) + num8]);
            if (num12 > 2)
            {
                this.siYoon(num1);
                flag1 = this.YoonYN;
            }
            int num11 = 0;
            for (int num13 = 0; num13 < num7; num13++)
            {
                for (int num14 = 0; num14 < 12; num14++)
                {
                    switch (Convert.ToInt32(this.yearMonthArr[(num13 * 12) + num14]))
                    {
                        case 1:
                            {
                                num11 += 0x1d;
                                goto Label_00C0;
                            }
                        case 2:
                            {
                                num11 += 30;
                                goto Label_00C0;
                            }
                        case 3:
                            {
                                num11 += 0x3a;
                                goto Label_00C0;
                            }
                        case 4:
                            {
                                num11 += 0x3b;
                                goto Label_00C0;
                            }
                        case 5:
                            {
                                num11 += 0x3b;
                                goto Label_00C0;
                            }
                        case 6:
                            {
                                break;
                            }
                        default:
                            {
                                goto Label_00C0;
                            }
                    }
                    num11 += 60;
                Label_00C0: ;
                }
            }
            for (int num15 = 0; num15 < num8; num15++)
            {
                switch (Convert.ToInt32(this.yearMonthArr[(num7 * 12) + num15]))
                {
                    case 1:
                        {
                            num11 += 0x1d;
                            goto Label_014F;
                        }
                    case 2:
                        {
                            num11 += 30;
                            goto Label_014F;
                        }
                    case 3:
                        {
                            num11 += 0x3a;
                            goto Label_014F;
                        }
                    case 4:
                        {
                            num11 += 0x3b;
                            goto Label_014F;
                        }
                    case 5:
                        {
                            num11 += 0x3b;
                            goto Label_014F;
                        }
                    case 6:
                        {
                            break;
                        }
                    default:
                        {
                            goto Label_014F;
                        }
                }
                num11 += 60;
            Label_014F: ;
            }
            num11 += (num3 + 0x16);
            num7 = 0x730;
        Label_016D:
            num7++;
            this.siYoon(num7);
            if (this.YoonYN)
            {
                num9 = 0x16e;
            }
            else
            {
                num9 = 0x16d;
            }
            if (num11 > num9)
            {
                num11 -= num9;
                goto Label_016D;
            }
            int num4 = num7;
            this.monthDayArr[1] = num9 - 0x151;
            num8 = 0;
        Label_01C0:
            num8++;
            if (num11 > this.monthDayArr[num8 - 1])
            {
                num11 -= this.monthDayArr[num8 - 1];
                goto Label_01C0;
            }
            int num5 = num8;
            int num6 = num11;
            int num10 = num4;
            num11 = (((num10 * 0x16d) + Convert.ToInt32((int)(num10 / 4))) - Convert.ToInt32((int)(num10 / 100))) + Convert.ToInt32((int)(num10 / 400));
            for (int num16 = 0; num16 < num5; num16++)
            {
                num11 += this.monthDayArr[num16];
            }
            num11 += num6;
            this.sYEAR = num10;
            this.sMONTH = num5;
            this.sDAY = num6;
            object[] objArray1 = new object[5] { this.sYEAR, "-", this.sMONTH, "-", this.sDAY };
            return string.Concat(objArray1);
        }

        public string FromSolToLun(DateTime dt)
        {
            return this.FromSolToLun(dt.Year, dt.Month, dt.Day);
        }

        public string FromSolToLun(string yyyymmdd)
        {
            return this.FromSolToLun(DateTime.Parse(yyyymmdd));
        }

        public string FromSolToLun(int y, int m, int d)
        {
            int num1 = 0;
            int num3 = 0xa4145;
            int num5 = y;
            int num6 = m;
            int num7 = d;
            int num14;

            this.siYoon(num5);

            if (this.YoonYN)
            {
                this.monthDayArr[1] = 0x1d;
            }
            else
            {
                this.monthDayArr[1] = 0x1c;
            }
            int num4 = ((((num5 - 1) * 0x16d) + Convert.ToInt32((int)((num5 - 1) / 4))) - Convert.ToInt32((int)((num5 - 1) / 100))) + Convert.ToInt32((int)((num5 - 1) / 400));
            num6 -= 2;
            for (int num11 = 0; num11 <= num6; num11++)
            {
                num4 += this.monthDayArr[num11];
            }
            num4 += num7;
            int num2 = (num4 - num3) + 1;
            num5 -= 0x731;
            num5++;
            int[] numArray1 = new int[num5];
            for (int num12 = 0; num12 < num5; num12++)
            {
                numArray1[num12] = 0;
                for (int num13 = 0; num13 <= 11; num13++)
                {
                    int[] numArray2;
                    IntPtr ptr1;
                    switch (Convert.ToInt32(this.yearMonthArr[(num12 * 12) + num13]))
                    {
                        case 1:
                            {
                                num1 = 0x1d;
                                goto Label_0121;
                            }
                        case 2:
                            {
                                num1 = 30;
                                goto Label_0121;
                            }
                        case 3:
                            {
                                num1 = 0x3a;
                                goto Label_0121;
                            }
                        case 4:
                            {
                                num1 = 0x3b;
                                goto Label_0121;
                            }
                        case 5:
                            {
                                num1 = 0x3b;
                                goto Label_0121;
                            }
                        case 6:
                            {
                                break;
                            }
                        default:
                            {
                                goto Label_0121;
                            }
                    }
                    num1 = 60;
                Label_0121:
                    (numArray2 = numArray1)[(int)(ptr1 = (IntPtr)num12)] = numArray2[(int)ptr1] + num1;
                }
            }
            int num8 = 0;
            while (num2 > numArray1[num8])
            {
                num2 -= numArray1[num8];
                num8++;
            }
            int num9 = 0;

        Label_0175:
            num14 = Convert.ToInt32(this.yearMonthArr[(num8 * 12) + num9]);
            if (num14 <= 2)
            {
                num1 = num14 + 0x1c;
                if (num2 <= num1)
                {
                    goto Label_0223;
                }
                num2 -= num1;
                num9++;
                goto Label_0175;
            }
            int num15 = 0;
            int num16 = 0;
            switch (num14)
            {
                case 3:
                    {
                        num15 = 0x1d;
                        num16 = 0x1d;
                        break;
                    }
                case 4:
                    {
                        num15 = 0x1d;
                        num16 = 30;
                        break;
                    }
                case 5:
                    {
                        num15 = 30;
                        num16 = 0x1d;
                        break;
                    }
                case 6:
                    {
                        num15 = 30;
                        num16 = 30;
                        break;
                    }
            }
            if (num2 > num15)
            {
                num2 -= num15;
                if (num2 > num16)
                {
                    num2 -= num16;
                    num9++;
                    goto Label_0175;
                }

                //tmpStr = "\uc724\ub2ec";
                goto Label_0175;
            }
        Label_0223:
            num8 += 0x731;
            num9++;
            int num10 = num2;
            this.lYEAR = num8;
            this.lMONTH = num9;
            this.lDAY = num10;
            object[] objArray1 = new object[5] { this.lYEAR, "-", this.lMONTH, "-", this.lDAY };
            return string.Concat(objArray1);
        }

        private void siYoon(int yy)
        {
            if ((yy % 4) != 0)
            {
                this.YoonYN = false;
            }
            else if ((yy % 100) != 0)
            {
                this.YoonYN = true;
            }
            else if ((yy % 400) != 0)
            {
                this.YoonYN = false;
            }
            else
            {
                this.YoonYN = true;
            }
        }

        private string YearToMonthData
        {
            get
            {
                this.yearMonthData = "1,2,4,1,1,2,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,4,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,5,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,2,1,5,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,5,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,6,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,2,3,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,4,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,3,2,1,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,5,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,4,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,5,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,4,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,5,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,5,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,1,2,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,5,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,5,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,5,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,5,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,3,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,4,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,5,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,5,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,3,2,1,2,2,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,5,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,3,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,5,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,1,2,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,1,5,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,6,1,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,4,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,4,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,4,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,4,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,5,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,3,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,4,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,4,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,5,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,5,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,5,2,1,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,5,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,5,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,5,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,6,1,2,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,3,2,1,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,1,2,1,1,5,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,5,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,5,1,2,1,2,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,1,2,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,5,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,1,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,2,1,5,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,2,1,1,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,2,3,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,2,1,2,1,2,2,1,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,2,1,5,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,1,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,1,5,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,6,2,1,2,1,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,2,1,2,5,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,1,2,1,1,2,1,2,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,3,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,1,2,5,2,1,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,2,2,1,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,2,1,2,1,2,2,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,2,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,5,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,2,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,1,1,2,1,1,2,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,2,1,5,1,2,1,1,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,2,1,1,2,1,1,2,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,2,2,1,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,5,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,5,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,2,1,2,2,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,1,1,2,1,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,4,1,1,2,1,2,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,1,2,1,1,2,1,2,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,1,2,1,2,1,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,2,1,2,5,2,1,2,1,2,1,1,";
                this.yearMonthData = this.yearMonthData + "2,1,2,2,1,2,2,1,2,1,2,1,";
                this.yearMonthData = this.yearMonthData + "2,1,1,2,1,2,2,1,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,5,1,2,1,2,1,2,2,2,1,2,";
                this.yearMonthData = this.yearMonthData + "1,2,1,1,2,1,1,2,2,1,2,2";
                return this.yearMonthData;
            }
        }
    }
}