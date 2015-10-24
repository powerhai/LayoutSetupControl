using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using WpfCnxLayoutSetControlDemo.Annotations;

namespace WpfCnxLayoutSetControlDemo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LcDevice : DependencyObject, INotifyPropertyChanged
    {
        public string Label
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LcLaneDevice : LcDevice
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class LcNonLaneDevice : LcDevice
    {
        public LcNonLaneDevice () 
        {
            DeviceType= DeviceType.None;
            AvailableDeviceTypes = new List<DeviceType>(){ DeviceType };
        }
        public List<DeviceType> AvailableDeviceTypes
        {
            get
            {
                return mAvailableDeviceTypes;
            }
            set
            {
                mAvailableDeviceTypes = value;
                OnPropertyChanged(); 
            }
        }
        public DeviceType DeviceType
        {
            get;
            protected set;
        }
        public LcTLane ParentTLane
        {
            get;
            set;
        }

        public static readonly DependencyProperty IsReverseProperty =
          DependencyProperty.Register("IsReverse", typeof(bool), typeof(LcNonLaneDevice),
              new PropertyMetadata(false));
        private List<DeviceType> mAvailableDeviceTypes;

        public bool IsReverse
        {
            get
            {
                return (bool)this.GetValue(IsReverseProperty);
            }
            set
            {
                this.SetValue(IsReverseProperty, value);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class LcLaneAssign : LcLaneDevice
    {
        public static readonly DependencyProperty IsReverseProperty =
           DependencyProperty.Register("IsReverse", typeof(bool), typeof(LcLaneAssign),
               new PropertyMetadata(false));

        public bool IsReverse
        {
            get
            {
                return (bool)this.GetValue(IsReverseProperty);
            }
            set
            {
                this.SetValue(IsReverseProperty,value);
            }
        }
        public List<LcNonLaneDevice> Devices
        {
            get;
            set;
        }
        protected LcLaneAssign ()
        {
            Devices = new List<LcNonLaneDevice>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class LcLaneNonAssign : LcLaneDevice
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class LcInlet : LcLaneNonAssign
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class LcErrorLane : LcLaneNonAssign
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class LcTLane : LcLaneAssign
    {
        public static readonly DependencyProperty IsSelectedProperty =
             DependencyProperty.Register("IsSelected", typeof(bool), typeof(LcTLane),
                 new PropertyMetadata(false));

        public bool IsSelected
        {
            get
            {
                return (bool)this.GetValue(IsSelectedProperty);
            }
            set
            {
                this.SetValue(IsSelectedProperty, value);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class LcTestInstrument : LcNonLaneDevice
    {
       
    }
    /// <summary>
    /// 
    /// </summary>
    public   class LcOnlineTestInstrument : LcTestInstrument
    {
        public LcOnlineTestInstrument ()
        {
            this.DeviceType = DeviceType.Instrument;
            AvailableDeviceTypes = new List<DeviceType>() { DeviceType };
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public   class LcOfflineTestInstrument : LcTestInstrument
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LcStoreDevice : LcNonLaneDevice
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class LcProcessDevice : LcNonLaneDevice
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class LcOutlet : LcStoreDevice
    {
        public LcOutlet ()
        {
            DeviceType = DeviceType.Outlet;
            AvailableDeviceTypes = new List<DeviceType>() { DeviceType };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LcStorage : LcStoreDevice
    {
        public LcStorage ()
        {
            this.DeviceType = DeviceType.Storage;
            AvailableDeviceTypes = new List<DeviceType>() { DeviceType };
        }
    }


    public class LcNoneDevice : LcNonLaneDevice
    {
        public LcNoneDevice ()
        {
            this.DeviceType = DeviceType.None;
            AvailableDeviceTypes = new List<DeviceType>() { DeviceType };
        }
    }

}
