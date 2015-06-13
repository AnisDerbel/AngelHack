using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace App4
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool RegistationStarted;
        private RkaziProvider mzRakzaProvider;

        List<Couleurs> listcouleurs = new List<Couleurs>();
        public MainPage()
        {
            this.InitializeComponent();

            //sdgf

            map1.MapServiceToken = " AiYzOMksI-ddkdcBMYlc6l5syQtbQJY4fNH2GOaTI7nDszWtPba6bNiRlNWz3Hi3";
            this.Loaded += MainPage_Loaded;
            
          
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
          //  var list = await App.MobileService.GetTable<Item>().ToListAsync();
            listcouleurs.Add(new Couleurs() { Id = 10, couleur = "#FF0000" });
            listcouleurs.Add(new Couleurs() { Id = 9, couleur = "#FF0000" });
            listcouleurs.Add(new Couleurs() { Id = 8, couleur = "#FF3A00" });
            listcouleurs.Add(new Couleurs() { Id = 7, couleur = "#FF6100" });
            listcouleurs.Add(new Couleurs() { Id = 6, couleur = "#FFB600" });
            listcouleurs.Add(new Couleurs() { Id = 5, couleur = "#FFE400" });
            listcouleurs.Add(new Couleurs() { Id = 4, couleur = "#D9E400" });
            listcouleurs.Add(new Couleurs() { Id = 3, couleur = "#46E400" });
            listcouleurs.Add(new Couleurs() { Id = 2, couleur = "#46BE00" });
            listcouleurs.Add(new Couleurs() { Id = 1, couleur = "#326800" });

            listbox.ItemsSource = listcouleurs;

          

        }
        Geoposition geoposition;
       async private void getposition()
        {
            MessageDialog msg = new MessageDialog("erreur");
            try
            {
                Geolocator geolocator = new Geolocator();
                
                geolocator.DesiredAccuracyInMeters = 50;


                geoposition = await geolocator.GetGeopositionAsync(
                   maximumAge: TimeSpan.FromMinutes(5),
                   timeout: TimeSpan.FromSeconds(10)
                   );


                geolocator.PositionChanged += geolocator_PositionChanged;
                this.NavigationCacheMode = NavigationCacheMode.Required;
                mzRakzaProvider = new RkaziProvider();
                mzRakzaProvider.Init();
                mzRakzaProvider.Rakza += OnNewRakza;
           
            }
            catch (Exception ex)
            {

                msg.ShowAsync();
            }
            

           
               latitude = geoposition.Coordinate.Latitude.ToString();
               longitude = geoposition.Coordinate.Longitude.ToString();
           
        }

       string longitude, latitude;

    /*   private void GetCoordinate()
       {
           var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High)
           {
               MovementThreshold = 1
           };
           watcher.PositionChanged += this.watcher_PositionChanged;
           watcher.Start();
       }

       private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
       {
           //Get position data
           var pos = e.Position.Location;
           //Update mypos object
           mypos.update(pos.Latitude, pos.Longitude);
           //Update data on the main interface
           MainMap.SetView(mypos.getCoordinate(), MainMap.ZoomLevel, MapAnimationKind.Parabolic);
       }
     */

       async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
       {
           try
           {
             //  await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              //  {
                    latitude = args.Position.Coordinate.Latitude.ToString();
                    longitude = args.Position.Coordinate.Longitude.ToString();

                    
                    
             //   });
           }
           catch (Exception ex)
           {
               
               throw;
           }



       }



        async private void OnNewRakza(int intensity)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                intensity = Math.Min(255, Math.Max(0, intensity));
                //if(intensity>80)
                //{ 
                int value = intensity / 10;

                listbox.ItemsSource = null;
                listbox.ItemsSource = listcouleurs.Where(x=> x.Id<=value).ToList();
                if(intensity>=100)
                {
                     media.AutoPlay = true;
                     media.Play();

                }
               // var b = new SolidColorBrush(Color.FromArgb((byte)255, (byte)intensity, (byte)0, (byte)0));
                
              //  this.Background = b;
              
                //sendrakza();
               // }
              //  else
               // {
                    //
               // }
            });
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        /// 
        Geolocator geolocator;
        DispatcherTimer dt = new DispatcherTimer();
       async protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            
            dt.Interval = new TimeSpan(0,0,1);
            dt.Tick += dt_Tick;
            // TODO: préparer la page pour affichage ici.
        //  getposition();

         //  if(gl!=null)
          //  gl.PositionChanged += geolocator_PositionChanged;
         /*   geolocator = new Geolocator();

            // Get cancellation token
             var cts = new CancellationTokenSource();
             CancellationToken token = cts.Token;

             var pos = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

            latitude =  pos.Coordinate.Latitude.ToString();
            longitude = pos.Coordinate.Longitude.ToString();
            //geolocator = new Geolocator();

           */

            //  geolocator.StatusChanged += geolocator_StatusChanged;
            //  geolocator.PositionChanged += geolocator_PositionChanged;
          //  this.NavigationCacheMode = NavigationCacheMode.Required;


           mzRakzaProvider = new RkaziProvider();
         mzRakzaProvider.Init();
          mzRakzaProvider.Rakza += OnNewRakza;
          
            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }

       async void dt_Tick(object sender, object e)
       {
         /*  gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.Default, MovementThreshold = 0.1 };
           var location = await gl.GetGeopositionAsync(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
           longitudes.Text = location.Coordinate.Longitude.ToString() ;
           latitudes.Text = location.Coordinate.Latitude.ToString();
          */
       }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendrakza();
           
        }

        private void StartRegistration()
        {
            RegistationStarted = true;
           
        }

        async private void sendrakza()
        {
            try
            {
                //var request = HttpWebRequest.Create("http://atlas-voyage.site88.net/index.php?Longitude=" + geoposition.Coordinate.Longitude.ToString().Replace(',', '.') + "&Latitude=" + geoposition.Coordinate.Latitude.ToString().Replace(',', '.') + "&Name=Anis&Cin=08895243&notif_text=rakza") as HttpWebRequest;
          /*      var request = HttpWebRequest.Create("http://atlas-voyage.site88.net/index.php?Longitude=35.4544&Latitude=10.3454&Name=Anis&Cin=08895243&notif_text=rakza") as HttpWebRequest;
                //request.Accept = "application/json"; <- No longer valid in WCF Data Services 5.0, use syntax shown below

                var response = await request.GetResponseAsync();


            

            //  Debug.WriteLine(response.ContentType);
            // Read the response into a Stream object.
            System.IO.Stream responseStream = response.GetResponseStream();
            string data;
            using (var reader = new System.IO.StreamReader(responseStream))
            {
                data = WebUtility.HtmlDecode(reader.ReadToEnd());

            }
           */

              
                /*HttpClient http = new HttpClient();

                var response = await http.GetByteArrayAsync("http://atlas-voyage.site88.net/index.php?Longitude=" + geoposition.Coordinate.Longitude.ToString().Replace(',', '.') + "&Latitude=" + geoposition.Coordinate.Latitude.ToString().Replace(',', '.') + "&Name=Anis&Cin=08895243&notif_text=rakza");

                // Encoding permet de décoder les caractères speciaux (é,è,à..)
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);

                source = WebUtility.HtmlDecode(source);
                HtmlDocument document = new HtmlDocument(); // creation d'un document html pour lire le code html obtenu par le site web
                document.LoadHtml(source);
           

            this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                 */
            }
            catch (Exception ex)
            {


            }

            Random rand = new Random();
            i = rand.Next(1, 100000);
           
            try
            {
                Item item = new Item
                 {
                     id = i.ToString(),
                     Cin = "08895243",
                     Name = "Anis",
                     notif_text = "rakza",
                     Longitude = longitude.Replace(',', '.'),
                     Latitude = latitude.Replace(',', '.')
                 };
                await App.MobileService.GetTable<Item>().InsertAsync(item);

                this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
               
            }
            catch (Exception ex)
            {
                
             
            }
        }

        int i = 1;

        private void StopRegistration()
        {
            RegistationStarted= false;
        }

        void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {



            string status = "";

            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "location is disabled in phone settings";
                    break;
                case PositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "initializing";
                    break;
                case PositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "no data";
                    break;
                case PositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "ready";
                    break;
                case PositionStatus.NotAvailable:
                    status = "not available";
                    // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                    break;
                case PositionStatus.NotInitialized:
                    // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state

                    break;
            }


            //StatusTextBlock.Text = status;

        }

        Geolocator gl = null;
        MapIcon pin=null,oldpin;
        private async void map1_Loaded(object sender, RoutedEventArgs e)
        {
             gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High, MovementThreshold = 1 };
           
           // geolocator.DesiredAccuracy = PositionAccuracy.High;
           // geolocator.MovementThreshold = 1; // The units are meters.

           
           var location = await gl.GetGeopositionAsync(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
            // var location = await gl.GetGeopositionAsync();

             
             pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "You are here!",

                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };
            map1.MapElements.Add(pin);
            map1.TrySetViewAsync(location.Coordinate.Point, 16, 0, 0, MapAnimationKind.Bow);


            /*var feed = await App.MobileService.GetTable<Item>().ToListAsync();

            foreach (var item in feed)
            {


                var   pushpin = new MapIcon()
            {
                Location = new Geopoint(),
                Title = "You are here!",

                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 }
            };
                            
            }
             */

                /*
                 * listcouleurs.Add(new Couleurs() { Id = 10, couleur = "#FF0000" });
            listcouleurs.Add(new Couleurs() { Id = 9, couleur = "#FF0000"  });
            listcouleurs.Add(new Couleurs() { Id = 8, couleur = "#FF3A00" });
            listcouleurs.Add(new Couleurs() { Id = 7, couleur = "#FF6100" });
            listcouleurs.Add(new Couleurs() { Id = 6, couleur = "#FFB600" });
            listcouleurs.Add(new Couleurs() { Id = 5, couleur = "#FFE400" });
            listcouleurs.Add(new Couleurs() { Id = 4, couleur = "#D9E400" });
            listcouleurs.Add(new Couleurs() { Id = 3, couleur = "#46E400" });
            listcouleurs.Add(new Couleurs() { Id = 2, couleur = "#46BE00" });
            listcouleurs.Add(new Couleurs() { Id = 1, couleur = "#326800" });
                 */
              /*  map1.Children.Add(new Ellipse()
                {
                   
                    Fill = new SolidColorBrush(Color.FromArgb()),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 50,
                    Height = 50
                });
               */
            



          gl.PositionChanged += gl_PositionChanged;
           // dt.Start();
        }


        async void gl_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            try
            {
               
                  await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                  {
                latitudes.Text = args.Position.Coordinate.Latitude.ToString();
                longitudes.Text = args.Position.Coordinate.Longitude.ToString();

               if (pin != null)
                {
                    oldpin = new MapIcon();
                    oldpin = pin;
                }

                 pin = new MapIcon()
                {
                    Location = args.Position.Coordinate.Point,
                    Title = "You are here!",

                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Pin.png")),
                    NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
                };

                 if (map1.MapElements.Count >= 1)
                     map1.MapElements.RemoveAt(0);

                     map1.MapElements.Insert(0,pin);
                   
                  });
               

                  map1.TrySetViewAsync(args.Position.Coordinate.Point, 16, 0, 0, MapAnimationKind.Bow);
               
            }

            catch (Exception ex)
            {

                throw;
            }

        }
    }

}
