using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCnxLayoutSetControlDemo.Models;

namespace WpfCnxLayoutSetControlDemo.Controls
{
     
    public class LcLayoutPanel : Panel
    {

        private const double INTERVAL_Y = 20;
        private const double INTERVAL_X = 10;
        private const double BUS_WIDTH = 6;
        private const double LANE_WIDTH = 2;
        private readonly Brush mBusBrush = Brushes.DarkGray;
        private readonly Brush mLaneBrush = Brushes.DarkGray;
        
        private const double BLACK_MINI_HEIGHT = 20;


        private Size mCanvasSize;
        private double mCenterLineY;
        public LaneData[] mLanes;
        public DeviceData[] mDevices;
        protected override Size MeasureOverride (Size availableSize)
        { 
            foreach(UIElement item in this.Children)
                item.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        
        public class DeviceData
        {
            public FrameworkElement Element
            {
                get;
                set;
            }
            public LcNonLaneDevice Data
            {
                get;
                set;
            }
            public Rect Rect
            {
                get;
                set;
            }
        }
        public class LaneData
        {
            public FrameworkElement Element
            {
                get;
                set;
            }
            public LcLaneDevice Data
            {
                get;
                set;
            }
            public Rect Rect
            {
                get;
                set;
            }
        }

        public double GetCenterLineY(LaneData[] lanes, DeviceData[] devs, Size panelSize)
        {
            if (lanes == null || lanes.Length <= 0)
                return 200;

            return panelSize.Height /2;
        }
        public void CalculateVariables( )
        { 
            var objs = this.Children.OfType<FrameworkElement>().Select(a => new   { Element = a, Data = a.DataContext });
            mLanes = objs.Where(a => a.Data is LcLaneDevice).Select(a=> new LaneData(){ Data = a.Data as LcLaneDevice, Element = a.Element}).ToArray();
            mDevices = objs.Where(a => a.Data is LcNonLaneDevice).Select(a=> new DeviceData(){Data = a.Data as LcNonLaneDevice, Element = a.Element }).ToArray();

            mCenterLineY = 300;
        }

        private LaneData GetBeforeLane(LaneData lane)
        {
             var index = -1;
            for(var i = 0; i < mLanes.Length; i++)
            {
                if(mLanes[i] == lane)
                {
                    index = i;
                    break;
                }
            }
            if(index == -1)
            {
                return null;
            }
            return mLanes[index];
        }
 
        protected override Size ArrangeOverride (Size finalSize)
        { 
            CalculateVariables(  );
               
            double lastLaneR = 0;
            double downRight = 0;
            double topRight = 0;

            foreach (var lane in mLanes)
            { 
                var assLane = lane.Data as LcLaneAssign;
                if(assLane == null)
                { 
                    lane.Rect = new Rect(lastLaneR + INTERVAL_X, mCenterLineY - lane.Element.DesiredSize.Height / 2,
                        lane.Element.DesiredSize.Width, lane.Element.DesiredSize.Height);
                    lane.Element.Arrange(lane.Rect); 

                } else
                {
                    var laneDevs = mDevices.Where(a => assLane.Devices.Contains(a.Data)).ToArray();
                    var sameSidedevs = laneDevs.Where(a => a.Data.IsReverse == assLane.IsReverse);
                    var maxDevWidth = sameSidedevs.Count() > 0 ? sameSidedevs.Max(a => a.Element.DesiredSize.Width) : 0;
                    
                    var x = lastLaneR + INTERVAL_X ;
                    if(assLane.IsReverse)
                    {
                        if(downRight + maxDevWidth > x && laneDevs.Length > 0)
                        {
                            x = downRight + maxDevWidth;
                        }
                    } else
                    {
                        if (topRight + maxDevWidth > x && laneDevs.Length > 0)
                        {
                            x = topRight + maxDevWidth;
                        }
                    }

                     
                    #region Arrange TLane
                    lane.Rect = new Rect(x, mCenterLineY - lane.Element.DesiredSize.Height / 2,
                        lane.Element.DesiredSize.Width, lane.Element.DesiredSize.Height);
                    lane.Element.Arrange(lane.Rect); 
                    #endregion
                    #region Arrange Devices
                    //左下最低处
                    double downLY = mCenterLineY + lane.Element.DesiredSize.Height / 2;
                    //右下最低处
                    double downRY = downLY;
                    //最低者的TOP
                    double down = downLY; 

                    //左上最高处
                    double topLY = lane.Rect.Top;
                    //右上最高处
                    double topRY = topLY;
                    //最高者的TOP
                    double top = topLY;
                    
                    foreach(var dev in laneDevs)
                    {
                        double devx = 0;
                        double devy = 0;

                        if(assLane.IsReverse  )
                        {
                            if( dev.Data.IsReverse)
                            {
                                devx = lane.Rect.Left + lane.Rect.Width / 2 - dev.Element.DesiredSize.Width - INTERVAL_X;
                                devy = down > downLY ? down + INTERVAL_Y : downLY + INTERVAL_Y;
                                
                            } else
                            {
                                devx = lane.Rect.Left + lane.Rect.Width / 2 + INTERVAL_X;
                                devy = down > downRY ? down + INTERVAL_Y : downRY + INTERVAL_Y;
                            }
                        } else
                        {
                            if (dev.Data.IsReverse)
                            {
                                devx = lane.Rect.Left + lane.Rect.Width / 2 + INTERVAL_X;
                                devy = top < topRY - dev.Element.DesiredSize.Height - INTERVAL_Y ? top - INTERVAL_Y : topRY - dev.Element.DesiredSize.Height - INTERVAL_Y;
                            }
                            else
                            {
                                devx = lane.Rect.Left + lane.Rect.Width / 2 - dev.Element.DesiredSize.Width - INTERVAL_X;
                                devy = top < topLY - dev.Element.DesiredSize.Height - INTERVAL_Y ? top   - INTERVAL_Y : topLY - dev.Element.DesiredSize.Height - INTERVAL_Y;
                            }
                        }
                       

                        dev.Rect = new Rect(devx, devy, dev.Element.DesiredSize.Width,dev.Element.DesiredSize.Height);
                        dev.Element.Arrange(dev.Rect);
                        if(assLane.IsReverse)
                        {
                            if(dev.Data.IsReverse)
                            {
                                downLY = dev.Rect.Bottom;
                                topRY = dev.Rect.Top;

                            } else
                            {
                                downRY = dev.Rect.Bottom;
                                topRY = dev.Rect.Top;
                            }
                        } else
                        {
                            if (dev.Data.IsReverse)
                            {
                                downRY = dev.Rect.Bottom;
                                topRY = dev.Rect.Top;
                            }
                            else
                            {
                                downLY = dev.Rect.Bottom;
                                topLY = dev.Rect.Top;
                            }
                        } 

                        down = dev.Rect.Top;
                        top = dev.Rect.Top;
                        if(assLane.IsReverse)
                        {
                            if (dev.Rect.Right > downRight)
                            {
                                downRight = dev.Rect.Right;
                            }                           
                        } else
                        {
                            if (dev.Rect.Right > topRight)
                            {
                                topRight = dev.Rect.Right;
                            }
                        } 
                    }
                }
                lastLaneR = lane.Rect.Right;
                #endregion

            }
            this.InvalidateVisual();
            return base.ArrangeOverride(finalSize);
        }


        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            LaneData lastLane = null;
            foreach(var lane in mLanes)
            {
                if(lastLane == null)
                {
                    lastLane = lane;
                    continue;
                }
                var rec = new Rect()
                {
                    Height = BUS_WIDTH,
                    Width = lane.Rect.Left - lastLane.Rect.Right,
                    X = lastLane.Rect.Right,
                    Y = mCenterLineY - BUS_WIDTH / 2
                };
                dc.DrawRectangle(mBusBrush, new Pen(mBusBrush,1),  rec); 
            }
            foreach(var item in mLanes)
            {
                var lane = item.Data as LcLaneAssign;
                if(lane == null)
                    continue;
                var laneDevs = mDevices.Where(a => lane.Devices.Contains(a.Data)).ToArray();
                foreach(var dev in laneDevs)
                {
                    Rect rec;
                    if(lane.IsReverse)
                    {
                        if(dev.Data.IsReverse )
                        {
                            
                            rec = new Rect()
                                {
                                    X = dev.Rect.Right ,
                                    Y = dev.Rect.Top + BLACK_MINI_HEIGHT/2,
                                    Width = INTERVAL_X ,
                                    Height = LANE_WIDTH
                                }; 
                        } else
                        {
                            rec = new Rect()
                            {
                                X = dev.Rect.Left - INTERVAL_X,
                                Y = dev.Rect.Top + BLACK_MINI_HEIGHT/2,
                                Width = INTERVAL_X +1 ,
                                Height = LANE_WIDTH
                            };
                           
                        }
                    } else
                    {
                        if (dev.Data.IsReverse )
                        {
                            rec = new Rect()
                            {
                                X = dev.Rect.Left - INTERVAL_X,
                                Y = dev.Rect.Top + BLACK_MINI_HEIGHT / 2,
                                Width = INTERVAL_X + 1,
                                Height = LANE_WIDTH
                            };

                        }
                        else
                        {
                            rec = new Rect()
                            {
                                X = dev.Rect.Right,
                                Y = dev.Rect.Top + BLACK_MINI_HEIGHT / 2,
                                Width = INTERVAL_X,
                                Height = LANE_WIDTH
                            };
                        }
                    }

                    dc.DrawRectangle(mLaneBrush, new Pen(mLaneBrush, 1), rec);
                }

                var lastDev = laneDevs.LastOrDefault();
                if(lastDev != null)
                {
                    Rect rec;
                    if(lane.IsReverse)
                    {
                        rec = new Rect()
                        {
                            X = item.Rect.Left + item.Rect.Width/2 - LANE_WIDTH/2,
                            Y = item.Rect.Bottom,
                            Width = LANE_WIDTH,
                            Height = lastDev.Rect.Top - item.Rect.Bottom + BLACK_MINI_HEIGHT / 2 + LANE_WIDTH
                        };
                    } else
                    {
                        rec = new Rect()
                        {
                            X = item.Rect.Left + item.Rect.Width / 2 - LANE_WIDTH / 2,
                            Y = lastDev.Rect.Top + BLACK_MINI_HEIGHT /2 ,
                            Width = LANE_WIDTH,
                            Height = item.Rect.Top - lastDev.Rect.Top  
                        };
                    }
                     
                    dc.DrawRectangle(mLaneBrush, new Pen(mLaneBrush, 1), rec);
                  
                }
            }
            base.OnRender(dc);
        }
    }
}