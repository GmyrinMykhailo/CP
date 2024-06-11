using System;
using System.Collections.Generic;
using System.Windows;

namespace ActorsApplication
{
    class User
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public User(string name, string lastname, bool isadmin, string username, string password)
        {
            Name = name;
            LastName = lastname;
            IsAdmin = isadmin;
            Username = username;
            Password = password;
        }

    }
    public abstract class BaseModel
    {
        public abstract long Id { get; set; }
    }
    internal class Place : BaseModel
    {
        public override long Id { get; set; }
        public string Title { get; set; }
        public string Addres { get; set; }
        public long PlaceCount { get; set; }
        public static string dbtable = "places";

        public Place(string title, string addres, long placeCount)
        {
            Title = title;
            Addres = addres;
            PlaceCount = placeCount;
        }
        public Place(string title, string addres, long placeCount, long id) : this(title, addres, placeCount)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Title} {Addres}";
        }
    }

    internal class Actor : BaseModel
    {
        public override long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string ActorName { get; set; }
        public static string dbtable = "actors";

        public Actor(string name, string lastName, string actorName)
        {
            Name = name;
            LastName = lastName;
            ActorName = actorName;
        }
        public Actor(string name, string lastName, string actorName, long id) : this(name, lastName, actorName)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Name} '{ActorName}' {LastName}";
        }
    }

    internal class Concert : BaseModel
    {
        public override long Id { get; set; }
        public Actor Actor { get; set; }
        public Place Place { get; set; }
        public DateTime Dt { get; set; }
        public static string dbtable = "concerts";
        public Concert(Actor actor, Place place, DateTime dt)
        {
            Actor = actor;
            Place = place;
            Dt = dt;
        }
        public Concert(Actor actor, Place place, DateTime dt, long id) : this(actor, place, dt)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"Концерт {Id}\nИсполнитель: {Actor.Name} {Actor.ActorName} {Actor.LastName}\nМесто проведения: {Place.Title}\nАдрес: {Place.Addres}\nДата: {Dt.Date}";
        }

        public string FormatDt()
        {
            string month = $"{Dt.Month}";
            string day = $"{Dt.Day}";
            if (Dt.Month < 10) month = $"0{month}";
            if (Dt.Day < 10) day = $"0{day}"; 
            return $"{Dt.Year}-{month}-{day}";
        }
    }
}
