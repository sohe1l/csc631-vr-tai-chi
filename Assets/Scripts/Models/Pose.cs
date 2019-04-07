using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class Pose
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public string Boundary { get; set; }
    public string Video { get; set; }

    public override string ToString()
    {
        return string.Format("[Pose: Id={0}, Name={1},  Difficulty={2}]", Id, Name, Difficulty);
    }
}
