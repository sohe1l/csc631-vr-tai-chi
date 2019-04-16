using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class Player
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Height { get; set; }
    public int Level { get; set; }

    public override string ToString()
    {
        return string.Format("[Player: Id={0}, Name={1},  Height={2}, Level={3}]", Id, Name, Height, Level);
    }


    public static Player GetOrCreatePlayer(string name)
    {
        var db = DataService.Instance.GetConnection();

        var query = db.Table<Player>()
            .Where(v => v.Name.Equals(name));

        if(query.Count() == 0)
        {
            var player = new Player
            {
                Name = name
            };

            db.Insert(player);
            return player;
        }
        return query.First();
    }


    // Validations
    public static string ValidateName(string name)
    {
        if (name == null) return "Name cannot be empty";
        if (name.Length < 3) return "Name should be at least 3 characters";
        if (name.Length > 10) return "Name should not be at more than 10 characters";
        return null;
    }

}
