using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Sensors;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace App4
{
    class RkaziProvider
    {
        public bool IsACCNotSupported { get; set; }
        public Action<int> Rakza;
        Accelerometer accelerometer;

        internal async void  Init()
        {
            accelerometer = Accelerometer.GetDefault();
            if (accelerometer == null)
            {
                IsACCNotSupported = true;
                MessageDialog msgbox = new MessageDialog("Accelerometer not supported or deactivated!");
                //Calling the Show method of MessageDialog class  
                //which will show the MessageBox  
                await msgbox.ShowAsync();
                return;
            }
            accelerometer.ReadingChanged += OnNewValues;

        }

        private void OnNewValues(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            
                if (Rakza != null && !IsACCNotSupported)
                {
                    AccelerometerReading reading = args.Reading;

                    var r = 255 - Convert.ToInt32((1 - Math.Abs(reading.AccelerationX)) * 255); 
                     Rakza(r);
                }
        }
    }
}
