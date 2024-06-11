using System;
using System.Collections.Generic;
using System.Windows;

namespace ActorsApplication
{
    public partial class ChangeConcert : Window
    {
        private string SelectedAction { get; set; }
        private Concert SelectedConcert { get; set; }
        private DB db { get; set; }
        private ConcertsWindow concertWw;
        public delegate void MethodNotify();
        public event MethodNotify OnMethodNotify;
        public ChangeConcert(object db, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            SelectedAction = "add";
            FillComboBoxes();
            concertWw = (ConcertsWindow)concertWindow;
        }
        public ChangeConcert(object db, object concert, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            SelectedAction = "change";
            SelectedConcert = (Concert)concert;
            FillComboBoxes(SelectedConcert);
            concertWw = (ConcertsWindow)concertWindow;
        }

        private void InitDB(DB db) => this.db = db;

        private void FillComboBoxes(Concert selectedConcert = null)
        {
            List<Actor> concerts = db.GetActorsFromDB();
            List<Place> places = db.GetPlacesFromDB();

            foreach(var c in concerts)
            {
                ComboBoxActors.Items.Add(c);
            }
            foreach(var c in places)
            {
                ComboBoxPlaces.Items.Add(c);
            }

            if(selectedConcert != null) DatePickerDt.SelectedDate = selectedConcert.Dt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxActors.SelectedItem == null || ComboBoxPlaces.SelectedItem == null || DatePickerDt.SelectedDate == null)
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Actor actor = (Actor)ComboBoxActors.SelectedItem;
            Place place = (Place)ComboBoxPlaces.SelectedItem;
            DateTime dt = (DateTime)DatePickerDt.SelectedDate;
            switch (SelectedAction)
            {
                case "add":
                    long lastId = db.GetLastId(Concert.dbtable);
                    Concert c = new Concert(actor, place, dt, ++lastId);
                    db.InsertConcertsToDB(c);
                    
                    break;
                case "change":
                    db.UpdateConcertsFromDB(new Concert(actor, place, dt, SelectedConcert.Id));
                    break;
            }
            OnMethodNotify();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
