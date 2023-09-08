using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFFrontend.Models;
using Microsoft.Win32;

namespace WPFFrontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] imageBytes;
        List<Game> games;

        HttpClient client = new HttpClient();
        public MainWindow()
        {
            client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            GetGames();
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            GetGames();
        }

        private async void GetGames()
        {
            guid_textbox.Text ="Refreshing List!";
            await Task.Delay(1100);
            guid_textbox.Text = "";
            name_textbox.Text = "";
            category_textbox.Text = "";
            rating_textbox.Text = "";
            Pic.Source = null;
            search_textbox.Text = "";

            try { 
            var response = await client.GetStringAsync("games");
            games = JsonConvert.DeserializeObject<List<Game>>(response);
            dgGames.DataContext = games;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private async void SearchGame()
        {
            if (search_textbox.Text != string.Empty) { 
                guid_textbox.Text = "Searching Game!";
                await Task.Delay(500);
                guid_textbox.Text = "";
                name_textbox.Text = "";
                category_textbox.Text = "";
                rating_textbox.Text = "";
                Pic.Source = null;

                try {
                    var response = await client.GetStringAsync("games/search/" + search_textbox.Text);
                    games = JsonConvert.DeserializeObject<List<Game>>(response);
                    dgGames.DataContext = games;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
            }
        }

        private async void SaveGame(Game game)
        {
            try {                
                if (name_textbox.Text != string.Empty)
                {
                    game.Picture = imageBytes;
                    await client.PostAsJsonAsync("games", game);
                    guid_textbox.Text = "Game Saved!";
                    await Task.Delay(1100);
                    GetGames();
                } else
                {
                    guid_textbox.Text = "Empty, not saving..";
                    await Task.Delay(1800);
                    GetGames();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private async void UpdateGame(Game game)
        {
        try { 
            game.Picture = imageBytes;
            await client.PutAsJsonAsync("games/" + game.Id, game);
            guid_textbox.Text = "Game Updated!";
            await Task.Delay(1100);
            GetGames();
        }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private async void DeleteGame(string gameId)
        {
            try {
                await client.DeleteAsync("games/" + gameId);
                guid_textbox.Text = "Deleted Game!";
                await Task.Delay(1100);
                GetGames();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            var game = new Game()
            {
                Id = Guid.NewGuid(),
                Name = name_textbox.Text,
                Category = category_textbox.Text,
                Rating = rating_textbox.Text,
            };

            if (guid_textbox.Text == string.Empty)
            {
                game.Id = Guid.NewGuid();
                SaveGame(game);
            }
            else
            {
                game.Id = Guid.Parse(guid_textbox.Text);
                UpdateGame(game);
            }

            name_textbox.Text = "";
            category_textbox.Text = "";
            rating_textbox.Text = "";
        }

        void Edit_Game_Button(object sender, RoutedEventArgs e)
        {
            try {
                Game game = ((FrameworkElement)sender).DataContext as Game;
                guid_textbox.Text = game.Id.ToString();
                name_textbox.Text = game.Name;
                category_textbox.Text = game.Category;
                rating_textbox.Text = game.Rating;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        void Delete_Game_Button(object sender, RoutedEventArgs e)
        {
            try {
                Game game = ((FrameworkElement)sender).DataContext as Game;
                DeleteGame(game.Id.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        public void ShowImg(Byte[] bytes)
        {
            try
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(bytes);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ImageSource imgSrc = biImg as ImageSource;

                Pic.Source = imgSrc;
            }
            catch (Exception ex)
            {

            }
        }

        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Image";
            dialog.DefaultExt = ".png" + ".bmp" + ".jpg" + ".jpeg";
            dialog.Filter = "Image Files (*.bmp, *.jpg, *.png, *.jpeg)|*.bmp;*.jpg;*.png;*.jpeg";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                using (FileStream stream = File.Open(filename, FileMode.Open))
                {
                    imageBytes = new byte[stream.Length];
                    await stream.ReadAsync(imageBytes, 0, (int)stream.Length);
                }
                ShowImg(imageBytes);
            }
        }    

        private void dgGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dgGames.SelectedItem;
            int id = dgGames.Items.IndexOf(dgGames.CurrentItem);

            try
            {
                Byte[] bytes = games[id].Picture;
                if (bytes != null)
                {
                    ShowImg(bytes);
                }
                else
                {
                    Pic.Source = null;
                }
            }catch (Exception ex)
            {

            }
        }
        private void btnSearchGame_Click(object sender, RoutedEventArgs e)
        {
            SearchGame();
        }
    }
}
