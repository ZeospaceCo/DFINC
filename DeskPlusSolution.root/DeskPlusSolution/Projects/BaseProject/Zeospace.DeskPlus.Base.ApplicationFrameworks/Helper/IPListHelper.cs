using System;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// 동일한 IP 마스크를 저장하기 위한 클래스 
    /// </summary>
    internal class IPArrayList
    {
        private bool isSorted = false;
        private ArrayList ipNumList = new ArrayList();
        private uint ipmask;

        /// <summary>
        /// 생성자
        /// </summary>
        public IPArrayList(uint mask)
        {
            ipmask = mask;
        }

        /// <summary>
        /// 추가
        /// </summary>
        public void Add(uint IPNum)
        {
            isSorted = false;
            ipNumList.Add(IPNum & ipmask);
        }

        /// <summary>
        /// IP 번호 목록이 포함된 범위 내인지 체크
        /// </summary>
        public bool Check(uint IPNum)
        {
            bool found = false;
            if (ipNumList.Count > 0)
            {
                if (!isSorted)
                {
                    ipNumList.Sort();
                    isSorted = true;
                }
                IPNum = IPNum & ipmask;
                if (ipNumList.BinarySearch(IPNum) >= 0) found = true;
            }
            return found;
        }

        /// <summary>
        /// 리스트 삭제
        /// </summary>
        public void Clear()
        {
            ipNumList.Clear();
            isSorted = false;
        }

        /// <summary>
        /// The ToString is overriden to generate a list of the IP numbers
        /// </summary>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (uint ipnum in ipNumList)
            {
                if (buf.Length > 0) buf.Append("\r\n");
                buf.Append(((int)ipnum & 0xFF000000) >> 24).Append('.');
                buf.Append(((int)ipnum & 0x00FF0000) >> 16).Append('.');
                buf.Append(((int)ipnum & 0x0000FF00) >> 8).Append('.');
                buf.Append(((int)ipnum & 0x000000FF));
            }
            return buf.ToString();
        }

        /// <summary>
        /// The IP mask for this list of IP numbers
        /// </summary>
        public uint Mask
        {
            get
            {
                return ipmask;
            }
        }
    }

    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class IPList
    {
        private ArrayList ipRangeList = new ArrayList();
        private SortedList maskList = new SortedList();
        private ArrayList usedList = new ArrayList();

        public IPList()
        {
            // IP 마스크 목록을 초기화하고 ipRangeList에 IPArrayList를 만듬
            uint mask = 0x00000000;
            for (int level = 1; level < 33; level++)
            {
                mask = (mask >> 1) | 0x80000000;
                maskList.Add(mask, level);
                ipRangeList.Add(new IPArrayList(mask));
            }
        }

        // System.Net.IPAddress 를 사용 할수 있게 uint 로 IP주소를 분석
        private uint parseIP(string IPNumber)
        {
            uint res = 0;
            string[] elements = IPNumber.Split(new Char[] { '.' });
            if (elements.Length == 4)
            {
                res = (uint)Convert.ToInt32(elements[0]) << 24;
                res += (uint)Convert.ToInt32(elements[1]) << 16;
                res += (uint)Convert.ToInt32(elements[2]) << 8;
                res += (uint)Convert.ToInt32(elements[3]);
            }
            return res;
        }

        /// <summary>
        /// IP 리스트에 IP 추가, ex. 10.1.1.1
        /// </summary>
        public void Add(string ipNumber)
        {
            this.Add(parseIP(ipNumber));
        }

        /// <summary>
        /// IP 리스트에 IP 추가 ex. 0x0A010101
        /// </summary>
        public void Add(uint ip)
        {
            ((IPArrayList)ipRangeList[31]).Add(ip);
            if (!usedList.Contains((int)31))
            {
                usedList.Add((int)31);
                usedList.Sort();
            }
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 172.16.0.0 255.255.0.0 => 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(string ipNumber, string mask)
        {
            this.Add(parseIP(ipNumber), parseIP(mask));
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 0xAC1000 0xFFFF0000 => 172.16.0.0 - 172.16.255.255
        /// </summary>
        public void Add(uint ip, uint umask)
        {
            object Level = maskList[umask];
            if (Level != null)
            {
                ip = ip & umask;
                ((IPArrayList)ipRangeList[(int)Level - 1]).Add(ip);
                if (!usedList.Contains((int)Level - 1))
                {
                    usedList.Add((int)Level - 1);
                    usedList.Sort();
                }
            }
        }

        /// <summary>
        /// 지정 범위의 마스크를 사용하여 IP 번호를 추가
        /// ex. 192.168.1.0/24 => 192.168.1.0 - 192.168.1.255
        /// </summary>
        public void Add(string ipNumber, int maskLevel)
        {
            this.Add(parseIP(ipNumber), (uint)maskList.GetKey(maskList.IndexOfValue(maskLevel)));
        }

        /// <summary>
        /// IP 범위를 추가  범위 확인하여 정상 IP / 마스크 블록 으로 변환
        /// </summary>
        public void AddRange(string fromIP, string toIP)
        {
            this.AddRange(parseIP(fromIP), parseIP(toIP));
        }

        /// <summary>
        /// IP 범위를 추가  범위 확인하여 정상 IP / 마스크 블록 으로 변환
        /// </summary>
        public void AddRange(uint fromIP, uint toIP)
        {
            if (fromIP > toIP)
            {
                uint tempIP = fromIP;
                fromIP = toIP;
                toIP = tempIP;
            }
            if (fromIP == toIP)
            {
                this.Add(fromIP);
            }
            else
            {
                uint diff = toIP - fromIP;
                int diffLevel = 1;
                uint range = 0x80000000;
                if (diff < 256)
                {
                    diffLevel = 24;
                    range = 0x00000100;
                }
                while (range > diff)
                {
                    range = range >> 1;
                    diffLevel++;
                }
                uint mask = (uint)maskList.GetKey(maskList.IndexOfValue(diffLevel));
                uint minIP = fromIP & mask;
                if (minIP < fromIP) minIP += range;
                if (minIP > fromIP)
                {
                    this.AddRange(fromIP, minIP - 1);
                    fromIP = minIP;
                }
                if (fromIP == toIP)
                {
                    this.Add(fromIP);
                }
                else
                {
                    if ((minIP + (range - 1)) <= toIP)
                    {
                        this.Add(minIP, mask);
                        fromIP = minIP + range;
                    }
                    if (fromIP == toIP)
                    {
                        this.Add(toIP);
                    }
                    else
                    {
                        if (fromIP < toIP) this.AddRange(fromIP, toIP);
                    }
                }
            }
        }

        /// <summary>
        /// IP 리스트에 해당 IP가 잇는지 확인  ex. 10.0.0.1
        /// </summary>
        public bool CheckNumber(string ipNumber)
        {
            return this.CheckNumber(parseIP(ipNumber)); ;
        }

        /// <summary>
        /// IP 리스트에 해당 IP가 잇는지 확인  ex) 0x0A000001
        /// </summary>
        public bool CheckNumber(uint ip)
        {
            bool found = false;
            int i = 0;
            while (!found && i < usedList.Count)
            {
                found = ((IPArrayList)ipRangeList[(int)usedList[i]]).Check(ip);
                i++;
            }
            return found;
        }

        /// <summary>
        /// 리스트 초기화
        /// </summary>
        public void Clear()
        {
            foreach (int i in usedList)
            {
                ((IPArrayList)ipRangeList[i]).Clear();
            }
            usedList.Clear();
        }

        /// <summary>
        /// 모든 IP 범위 리스트 포멧
        /// </summary>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (int i in usedList)
            {
                buffer.Append("\r\nRange with mask of ").Append(i + 1).Append("\r\n");
                buffer.Append(((IPArrayList)ipRangeList[i]).ToString());
            }
            return buffer.ToString();
        }


    }
}
