using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ActorsApplication
{

    public partial class AddPlace : Window
    {
        private string SelectedAction { get; set; }
        private Place SelectedPlace { get; set; }
        private DB db { get; set; }
        private ConcertsWindow concertWw;

        public delegate void MethodNotify();
        public event MethodNotify OnMethodNotify;
        public AddPlace(object db, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            concertWw = (ConcertsWindow)concertWindow;

            SelectedAction = "add";
        }
        public AddPlace(object db, object place, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            SelectedPlace = (Place)place;
            concertWw = (ConcertsWindow)concertWindow;

            SelectedAction = "change";
            FillTextBlocks();
        }

        private void FillTextBlocks()
        {
            TitleTextBlock.Text = SelectedPlace.Title;
            AddressTextBlock.Text = SelectedPlace.Addres;
            PlacesCountTextBlock.Text = SelectedPlace.PlaceCount.ToString();
        }

        private void InitDB(DB db) => this.db = db;

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if(TitleTextBlock.Text == "" || AddressTextBlock.Text == "" || PlacesCountTextBlock.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string title = TitleTextBlock.Text;
            string address = AddressTextBlock.Text;
            int placesCount = 0;
            if (!int.TryParse(PlacesCountTextBlock.Text, out placesCount))
            {
                MessageBox.Show("Количество мест не может быть строкой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (SelectedAction)
            {
                case "change":
                    db.UpdatePlaceFromDB(new Place(title, address, placesCount, SelectedPlace.Id));
                    break;
                case "add":
                    long lastId = db.GetLastId(Concert.dbtable);
                    db.InsertPlaceToDB(new Place(title, address, placesCount, ++lastId));
                    break;
            }
            OnMethodNotify();
            this.Close();
        }
    }
}
