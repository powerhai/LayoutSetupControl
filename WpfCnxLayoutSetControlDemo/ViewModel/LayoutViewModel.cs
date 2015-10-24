using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using WpfCnxLayoutSetControlDemo.Annotations;
using WpfCnxLayoutSetControlDemo.Models;

namespace WpfCnxLayoutSetControlDemo.ViewModel
{
    public class LayoutViewModel :INotifyPropertyChanged
    {



        private ICommand mSetTLaneReserceCommand;
        private ObservableCollection<LcDevice> mDevices;
        private ICommand mSetDeviceReserceCommand;
        private ICommand mSetTLaneCheckedCommand;
        private ICommand mSetDeviceTypeCommand;
        private List<DeviceType> mDeviceTypes;
        private CollectionViewSource mDevicesViewSource;
        private ICommand mSelectDeviceTypeCommand;

        public CollectionViewSource DevicesViewSource
        {
            get
            {
                if(mDevicesViewSource == null)
                {
                    mDevicesViewSource = new CollectionViewSource(){ Source = Devices };
                    mDevicesViewSource.View.MoveCurrentToFirst();
                }
                return mDevicesViewSource;
            } 
        }
        public List<DeviceType> DeviceTypes
        {
            get
            {
                return mDeviceTypes;
            }
            set
            {
                mDeviceTypes = value;
                OnPropertyChanged();
            }
        }
        private void ReplaceDevice (LcNonLaneDevice oldDevice, LcNonLaneDevice newDevice)
        {
            var oldIndex = Devices.IndexOf(oldDevice);

            var lane = Devices.OfType<LcTLane>().FirstOrDefault(a => a.Devices.Contains(oldDevice));
            newDevice.IsReverse = oldDevice.IsReverse;
            newDevice.ParentTLane = lane;
            Devices[oldIndex] = newDevice;
            lane.Devices.Remove(oldDevice);
            lane.Devices.Add(newDevice);
            
        }
        public ICommand SelectDeviceTypeCommand
        {
            get
            {
                return mSelectDeviceTypeCommand??(mSelectDeviceTypeCommand = new DelegateCommand<DeviceType?>( (s)=>
                {
                    
                    if(!s.HasValue)
                        return;
                    Debug.WriteLine( s.Value +   "vvvv " + DevicesViewSource.View.CurrentItem.ToString());
                    if(s.Value == DeviceType.Storage)
                        ReplaceDevice(DevicesViewSource.View.CurrentItem as LcNonLaneDevice, new LcStorage());
                    if (s.Value == DeviceType.Instrument)
                        ReplaceDevice(DevicesViewSource.View.CurrentItem as LcNonLaneDevice, new LcOnlineTestInstrument());
                    
                }));
            } 
        }
        public ICommand SetDeviceTypeCommand
        {
            get
            {
                return mSetDeviceTypeCommand ?? (mSetDeviceTypeCommand = new DelegateCommand<LcNonLaneDevice>(a =>
                {
                     
                    if(a.DeviceType == DeviceType.Storage)
                    {
                        a.AvailableDeviceTypes = new List<DeviceType> { DeviceType.None, DeviceType.Storage, DeviceType.Instrument, }; 
                    } 
                    if(a.DeviceType == DeviceType.Instrument)
                    {
                        a.AvailableDeviceTypes = new List<DeviceType> { DeviceType.Outlet, DeviceType.Instrument, DeviceType.None , DeviceType.Storage}; 
                    }
                    if (a.DeviceType == DeviceType.None)
                    {
                        a.AvailableDeviceTypes = new List<DeviceType> { DeviceType.Outlet, DeviceType.Instrument, DeviceType.None, DeviceType.Storage,DeviceType.Recapper, DeviceType.Decapper };
                    }
                   
                }));
            } 
        }
        public ICommand SetTLaneReserceCommand
        {
            get
            {
                return mSetTLaneReserceCommand??(mSetTLaneReserceCommand = new DelegateCommand<LcLaneAssign>(a =>
                {
                    a.IsReverse = !a.IsReverse; 
                    Devices.Add(null);
                }));
            } 
        }

        public ICommand SetDeviceReserceCommand
        {
            get
            {
                return mSetDeviceReserceCommand??(mSetDeviceReserceCommand = new DelegateCommand<LcNonLaneDevice>(
                    a =>
                    {
                        a.IsReverse = !a.IsReverse;
                        Devices.Add(null);
                    }));
            } 
        }

        public ICommand SetTLaneCheckedCommand
        {
            get
            {
                return mSetTLaneCheckedCommand ?? (mSetTLaneCheckedCommand = new DelegateCommand<LcTLane>(a =>
                {
                    if(!a.IsSelected)
                    {
                        foreach(var item in a.Devices)
                            this.Devices.Remove(item);
                        a.Devices.Clear();
                    }
                    if(a.IsSelected)
                    {
                        var newDevice = new LcNoneDevice();
                        newDevice.ParentTLane = a;
                        a.Devices.Add(newDevice);
                        this.Devices.Add(newDevice);

                        var lanes = Devices.OfType<LcTLane>();
                        var lastLane = lanes.LastOrDefault();
                        if(a == lastLane)
                        {
                            Devices.Add(new LcTLane()
                            {
                                Label="3"
                            });
                        }
                    }
                }));
            } 
        }

        public ObservableCollection<LcDevice> Devices
        {
            get
            {
                return mDevices;
            }
            set
            {
                mDevices = value;
               OnPropertyChanged();
            }
        }
        public LayoutViewModel ()
        {
            var tl1 = new LcTLane(){ IsReverse = true ,IsSelected = true};
            tl1.Devices.Add(new LcStorage(){IsReverse = true, ParentTLane = tl1 , Label="1" });
            tl1.Devices.Add(new LcOutlet(){ ParentTLane = tl1 });

            var tl2 = new LcTLane(){ IsReverse = true, IsSelected = true }; 
            tl2.Devices.Add(new LcStorage() { IsReverse = false , Label="2", ParentTLane = tl2 });
            tl2.Devices.Add(new LcOutlet() { IsReverse = false , Label="3",  ParentTLane = tl2}); 
            tl2.Devices.Add(new LcOnlineTestInstrument() { IsReverse = true , Label="2" , ParentTLane = tl2  }); 

            var tl3 = new LcTLane() { IsReverse = false, IsSelected = true  };
            tl3.Devices.Add(new LcOutlet(){ ParentTLane = tl3 });
            tl3.Devices.Add(new LcStorage() { IsReverse = true , ParentTLane = tl3 });


             Devices = new ObservableCollection<LcDevice>();
            {
                Devices.Add(new LcInlet());
                Devices.Add(new LcErrorLane());
                Devices.Add(tl1);
                Devices.Add(new LcTLane());
                Devices.Add(tl2); 
                Devices.Add(new LcTLane());
                Devices.Add(tl3);
                 
                Devices.Add(new LcTLane()); 
            }

            foreach (var item in tl1.Devices)
                Devices.Add(item);

            foreach (var item in tl2.Devices)
                Devices.Add(item);


            foreach (var item in tl3.Devices)
                Devices.Add(item);

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
}
