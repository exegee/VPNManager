using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VPNManager.Core;

namespace Business
{
    public class VpnRemoteHost : INotifyPropertyChanged
    {
        public int? Id { get; set; }
        public string HostName { get; set; }
        public string HostDescription { get; set; }
        public string NetworkDst { get; set; }
        public string NetworkMask { get; set; }
        public string Gateway { get; set; }
        private bool _isNoOtherConnectionActive = true;
        public bool IsNoOtherConnectionActive
        {
            get
            {
                return _isNoOtherConnectionActive;
            }
            set
            {
                _isNoOtherConnectionActive = value;
                OnPropertyChanged();
            }
        }
        private string _latency;
        public string Latency
        {
            get
            {
                return _latency;
            }
            set
            {
                _latency = value;
                OnPropertyChanged();
            }
        }
        public DateTime? CreatedDate { get; set; }
        private VPNConnectionStatus _status = VPNConnectionStatus.Disconnected;
        public VPNConnectionStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
