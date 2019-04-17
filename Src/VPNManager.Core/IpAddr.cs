using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNManager.Core
{
    public class IpAddr
    {
        public string ASegment { get; set; }
        public string BSegment { get; set; }
        public string CSegment { get; set; }
        public string DSegment { get; set; }

        private string _setIpAddress;
        public string SetIpAddress {
            get
            {
                return _setIpAddress;
            }
            set
            {
                _setIpAddress = value;
                ASegment = _setIpAddress.Split('.').ToList()[0];
                BSegment = _setIpAddress.Split('.').ToList()[1];
                CSegment = _setIpAddress.Split('.').ToList()[2];
                DSegment = _setIpAddress.Split('.').ToList()[3];
            }
        }

        public IpAddr()
        {
        }

        public string GetIpAddress()
        {
            return ASegment + '.' + BSegment + '.' + CSegment + '.' + DSegment;
        }

    }
}
