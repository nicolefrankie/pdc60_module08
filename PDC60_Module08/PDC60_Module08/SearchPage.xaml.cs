using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;

namespace PDC60_Module08
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class Post2
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    public partial class SearchPage : ContentPage
    {
        private const string url_search = "http://172.16.26.173/pdc60/api-search.php";
        private HttpClient _Client = new HttpClient();
        private ObservableCollection<Post2> _posts;
        public SearchPage()
        {
            InitializeComponent();
        }
        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }
        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", "Are you sure you want to delete ID No: " + mi.CommandParameter + " delete context action", "OK");
        }
        public class ResponseObject
        {
            public bool status { get; set; }
            public JToken data { get; set; }
            public string message { get; set; }
        }
        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {

            }
            else
            {
                try
                {
                    var searchUrl = $"{url_search}?username={searchQuery}";
                    System.Diagnostics.Debug.WriteLine($"Search URL: {searchUrl}");
                    var content = await _Client.GetStringAsync(searchUrl);
                    var responseObject =
                    JsonConvert.DeserializeObject<ResponseObject>(content);
                    if (responseObject.status)
                    {
                        var searchResult =
                        JsonConvert.DeserializeObject<List<Post2>>(responseObject.data.ToString());
                        _posts = new ObservableCollection<Post2>(searchResult);
                        Post_list2.ItemsSource = _posts;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Error: {responseObject.message}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"An error occured: {ex.Message} ");

                }
            }
        }
    }
}