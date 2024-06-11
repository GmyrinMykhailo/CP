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

namespace ActorsApplication
{
    public partial class AddActor : Window
    {
        private string SelectedAction { get; set; }
        private Actor SelectedActor { get; set; }
        private DB db { get; set; }
        private ConcertsWindow concertWw;

        public delegate void MethodNotify();
        public event MethodNotify OnMethodNotify;
        public AddActor(object db, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            concertWw = (ConcertsWindow)concertWindow;

            SelectedAction = "add";
        }
        public AddActor(object db, object actor, object concertWindow)
        {
            InitializeComponent();
            InitDB((DB)db);
            SelectedActor = (Actor)actor;
            concertWw = (ConcertsWindow)concertWindow;

            SelectedAction = "change";
            FillTextBlocks();
        }

        private void FillTextBlocks()
        {
            NameTextBlock.Text = SelectedActor.Name;
            LastNameTextBlock.Text = SelectedActor.LastName;
            ActorNameCountTextBlock.Text = SelectedActor.ActorName;
        }

        private void InitDB(DB db) => this.db = db;

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBlock.Text == "" || LastNameTextBlock.Text == "" || ActorNameCountTextBlock.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = NameTextBlock.Text;
            string lastName = LastNameTextBlock.Text;
            string actorName = ActorNameCountTextBlock.Text;
            switch (SelectedAction)
            {
                case "change":
                    db.UpdateActorFromDB(new Actor(name, lastName, actorName, SelectedActor.Id));
                    break;
                case "add":
                    long lastId = db.GetLastId(Actor.dbtable);
                    db.InsertActorToDB(new Actor(name, lastName, actorName, ++lastId));
                    break;
            }
            OnMethodNotify();
            this.Close();
        }
    }
}
