using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class Leaderboard
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Player_id { get; set; }
    public int Level { get; set; }
    public int Score { get; set; }
    public int Timestamp; 

    public override string ToString()
    {
        return string.Format("[Score: Id={0}, Player_ID={1}, Level={2}, Value={3}]", Id, Player_id, Level, Score);
    }
}
