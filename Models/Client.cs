using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BatUtils.Models
{
    public class Client : INotifyPropertyChanged
    {
        private bool _enabled = true;
        private string _code;
        private string _name;
        private string _rkServerAddress;
        private ushort _rkServerPort;
        private string _shServerAddress;
        private ushort _shServerPort;

        public bool Enabled
        {
            get => _enabled;
            set => SetField(ref _enabled, value);
        }

        public string Code
        {
            get => _code;
            set => SetField(ref _code, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string RKServerAddress
        {
            get => _rkServerAddress;
            set
            {
                if (SetField(ref _rkServerAddress, value))
                    OnPropertyChanged(nameof(RKEndpoint));
            }
        }

        public ushort RKServerPort
        {
            get => _rkServerPort;
            set
            {
                if (SetField(ref _rkServerPort, value))
                    OnPropertyChanged(nameof(RKEndpoint));
            }
        }

        public string SHServerAddress
        {
            get => _shServerAddress;
            set
            {
                if (SetField(ref _shServerAddress, value))
                    OnPropertyChanged(nameof(SHEndpoint));
            }
        }

        public ushort SHServerPort
        {
            get => _shServerPort;
            set
            {
                if (SetField(ref _shServerPort, value))
                    OnPropertyChanged(nameof(SHEndpoint));
            }
        }

        // Combined "address:port". null when there is no address,
        // so the XAML TargetNullValue='—' shows a dash instead of ":0".
        public string RKEndpoint =>
            string.IsNullOrWhiteSpace(_rkServerAddress)
                ? null
                : $"{_rkServerAddress}:{_rkServerPort}";

        public string SHEndpoint =>
            string.IsNullOrWhiteSpace(_shServerAddress)
                ? null
                : $"{_shServerAddress}:{_shServerPort}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Returns true only when the value actually changed, so we don't
        // raise (and re-render) on a no-op assignment.
        protected bool SetField<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);   // propertyName = the setter's name, via CallerMemberName
            return true;
        }
    }
}