using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ActorsApplication
{
    public partial class ConcertsWindow : Window
    {
        readonly private DB db = new DB();
        private User currentuser;
        public ConcertsWindow(object user)
        {
            InitializeComponent();
            currentuser = (User)user;

            ChangeSelectedConcertButton.Visibility = Visibility.Hidden;
            DeleteSelectedConcertButton.Visibility = Visibility.Hidden;
            if (currentuser.IsAdmin == false)
            {
                AddNewConcertButton.Visibility = Visibility.Hidden;
            }

            Error error = db.InitConnect();

            if (error != null) InfoLabel.Content = $"{error.message}\n{error.detail}";

            foreach (var c in new List<string>() { "Актёры", "Концерты", "Места проведения" })
            {
                ComboBoxFiltersCategories.Items.Add(c);
            }
            ComboBoxFiltersCategories.SelectedIndex = 1;
            FillConcertsLabel();
        }

        private void FillConcertsLabel()
        {
            ConcertsListBox.Items.Clear();
            switch (ComboBoxFiltersCategories.SelectedIndex)
            {
                case 1:
                    List<Concert> concerts = db.GetConcertsFromDB();
                    FillInfoLabel(concerts);
                    break;
                case 2:
                    List<Place> places = db.GetPlacesFromDB();
                    FillInfoLabel(places);
                    break;
                case 0:
                    List<Actor> actors = db.GetActorsFromDB();
                    FillInfoLabel(actors);
                    break;
            }
        }
        private void FillInfoLabel<T>(List<T> data)
        {
            foreach (var c in data)
            {
                ConcertsListBox.Items.Add(c);
            }
        }
        private void ConcertsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ConcertsListBox.SelectedItem == null) return;

            if(currentuser.IsAdmin == true)
            {
                ChangeSelectedConcertButton.Visibility = Visibility.Visible;
                DeleteSelectedConcertButton.Visibility = Visibility.Visible;
            }

            ListBox lb = (ListBox)sender;
            InfoLabel.Content = lb.SelectedItem;
        }

        private void OpenCreateWindowButton_click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch (ComboBoxFiltersCategories.SelectedIndex)
            {
                case 0:
                    AddActor addActor = new AddActor(db, this);
                    if ((string)btn.Content == "Изменить") addActor = new AddActor(db, ConcertsListBox.SelectedItem, this);
                    addActor.OnMethodNotify += this.FillConcertsLabel;
                    addActor.Show();
                    break;
                case 1:
                    ChangeConcert a = new ChangeConcert(db, this);
                    if ((string)btn.Content == "Изменить") a = new ChangeConcert(db, ConcertsListBox.SelectedItem, this);
                    a.OnMethodNotify += this.FillConcertsLabel;
                    a.Show();
                    break;
                case 2:
                    AddPlace addPlace = new AddPlace(db, this);
                    if ((string)btn.Content == "Изменить") addPlace = new AddPlace(db, ConcertsListBox.SelectedItem, this);
                    addPlace.OnMethodNotify += this.FillConcertsLabel;
                    addPlace.Show();
                    break;

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConcertsListBox.SelectedItem == null) return;
            switch (ComboBoxFiltersCategories.SelectedIndex)
            {
                case 0:
                    db.DeleteActorFromDB((Actor)ConcertsListBox.SelectedItem);
                    break;
                case 1:
                    db.DeleteConcertFromDB((Concert)ConcertsListBox.SelectedItem);
                    break;
                case 2:
                    db.DeletePlaceFromDB((Place)ConcertsListBox.SelectedItem);
                    break;
            }
            FillConcertsLabel();
        }

        private void ComboBoxFiltersCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillConcertsLabel();
        }
    }
}
