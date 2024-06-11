using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Windows.Documents;

namespace ActorsApplication
{
    internal class DBErrors
    {
        public static Error DBEngineIsNullError => new Error("Ошибка подключения!", "DB ENGINE NOT INITIALIZED");
        public static Error DBEngineInitError => new Error("Ошибка создания подключения", "Возможно вы забыли создать БД?");
        public static Error DBTableIsNotExist(string table) => new Error("Ошибка получения данных из бд", $"Таблица {table} существует?");
        public static Error DBTableInsertError(string message) => new Error("Ошибка при попытке добавления в таблицу", message);
        public static Error DBTableDeleteError(string message) => new Error("Ошибка при попытке удаления данных из таблицы", message);
        public static Error DBTableUpdateError(string message) => new Error("Ошибка при попытке обновления данных в таблице", message);
    }
    internal class DB
    {
        public string connectString = "Data Source=concerts_app_db.db";
        public SQLiteConnection connection;
        public Error InitConnect()
        {
            connection = new SQLiteConnection(connectString);
            connection.Open();
            return null;
        }
        public List<Concert> GetConcertsFromDB(string filter = null)
        {
            string dbtable = Concert.dbtable;
            string sqlexpression_concerts = $"SELECT * FROM {dbtable}";
            if (filter != null)
            {
                sqlexpression_concerts = $"SELECT * FROM {dbtable} WHERE id = {filter}";
            }

            DataTable data = FetchSelectQuery(sqlexpression_concerts);
            List<Concert> concerts = new List<Concert>();

            foreach (DataRow c in data.Rows)
            {
                concerts.Add(new Concert(GetActorsFromDB(c.Field<long>("actor_id").ToString())[0], GetPlacesFromDB(c.Field<long>("place_id").ToString())[0], c.Field<DateTime>("dt"), c.Field<long>("id")));
            }

            return concerts;

        }
        public List<Actor> GetActorsFromDB(string filter = null)
        {
            string dbtable = Actor.dbtable;
            string sqlexpression_actors = $"SELECT * FROM {dbtable}";
            if(filter != null)
            {
                sqlexpression_actors = $"SELECT * FROM {dbtable} WHERE id = {filter}";
            }

            DataTable data = FetchSelectQuery(sqlexpression_actors);
            List<Actor> actors = new List<Actor>();

            foreach (DataRow c in data.Rows)
            {
                actors.Add(new Actor(c.Field<string>("name"), c.Field<string>("last_name"), c.Field<string>("actor_name"), c.Field<long>("id")));
            }
            return actors;
        }
        public List<Place> GetPlacesFromDB(string filter = null)
        {
            string dbtable = Place.dbtable;
            string sqlexpression_places = $"SELECT * FROM {dbtable}";
            if (filter != null)
            {
                sqlexpression_places = $"SELECT * FROM {dbtable} WHERE id = {filter}";
            }

            DataTable data = FetchSelectQuery(sqlexpression_places);
            List<Place> places = new List<Place>();

            foreach(DataRow c in data.Rows)
            {
                places.Add(new Place(c.Field<string>("title"), c.Field<string>("addres"), c.Field<long>("place_count"), c.Field<long>("id")));
            }
            return places;
        }
        public long GetLastId(string dbtable)
        {
            string sqlexpression = $"SELECT * FROM {dbtable}";
            DataTable data = FetchSelectQuery(sqlexpression);
            int i = 0;
            foreach(DataRow c in data.Rows)
            {
                if (i + 1 == data.Rows.Count) return c.Field<long>("id");
                i++;
            }
            return 0;
        }
        public Error InsertConcertsToDB(Concert concert)
        {
            string sqlexpression = $"INSERT INTO {Concert.dbtable} (id,actor_id,place_id,dt) VALUES ({concert.Id},{concert.Actor.Id},{concert.Place.Id},'{concert.FormatDt()}')";
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return DBErrors.DBTableInsertError(e.Message);
            }
            return null;
        }
        public Error UpdateConcertsFromDB(Concert concert)
        {
            string sqlexpression = $"UPDATE {Concert.dbtable} SET actor_id = {concert.Actor.Id}, place_id = {concert.Place.Id}, dt = '{concert.FormatDt()}' WHERE id = {concert.Id}";
            Console.WriteLine(sqlexpression);
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            } catch(Exception e)
            {
                return DBErrors.DBTableUpdateError(e.Message);
            }
            return null;
        }
        public Error DeleteConcertFromDB(Concert concert)
        {
            string sqlexpression = $"DELETE FROM {Concert.dbtable} WHERE id = {concert.Id}";
            return FetchDeleteQuery(sqlexpression);
        }
        public Error InsertPlaceToDB(Place place)
        {
            string sqlexpression = $"INSERT INTO {Place.dbtable} (id,title,addres,place_count) VALUES ({place.Id}, '{place.Title}', '{place.Addres}', {place.PlaceCount})";
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return DBErrors.DBTableInsertError(e.Message);
            }
            return null;
        }
        public Error UpdatePlaceFromDB(Place place)
        {
            string sqlexpression = $"UPDATE {Place.dbtable} SET title = '{place.Title}', addres = '{place.Addres}', place_count = {place.PlaceCount} WHERE id = {place.Id}";
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return DBErrors.DBTableUpdateError(e.Message);
            }
            return null;
        }
        public Error DeletePlaceFromDB(Place place)
        {
            string sqlexpression = $"DELETE FROM {Place.dbtable} WHERE id = {place.Id}";
            return FetchDeleteQuery(sqlexpression);
        }
        public Error InsertActorToDB(Actor actor)
        {
            string sqlexpression = $"INSERT INTO {Actor.dbtable} (id,name,last_name,actor_name) VALUES ({actor.Id}, '{actor.Name}','{actor.LastName}','{actor.ActorName}')";
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return DBErrors.DBTableInsertError(e.Message);
            }
            return null;
        }
        public Error UpdateActorFromDB(Actor actor)
        {
            string sqlexpression = $"UPDATE {Actor.dbtable} SET name = '{actor.Name}', last_name = '{actor.LastName}', actor_name = '{actor.ActorName}' WHERE id = {actor.Id}";
            SQLiteCommand command = new SQLiteCommand(sqlexpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return DBErrors.DBTableUpdateError(e.Message);
            }
            return null;
        }
        public Error DeleteActorFromDB(Actor actor)
        {
            string sqlexpression = $"DELETE FROM {Actor.dbtable} WHERE id = {actor.Id}";
            return FetchDeleteQuery(sqlexpression);
        }
        private Error FetchDeleteQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return DBErrors.DBTableDeleteError(e.Message);
            }
            return null;
        }
        private DataTable FetchSelectQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            return data;
        }

    }
}
