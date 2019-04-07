using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class Player
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Height { get; set; }
    public int Score { get; set; }
    public int Level { get; set; }

    public override string ToString()
    {
        return string.Format("[Player: Id={0}, Name={1},  Height={2}, Score={3}, Level={4}]", Id, Name, Height, Score, Level);
    }
}
