using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class Level 
{
    public const int MODE_TRANING = 0;
    public const int MODE_SCORED = 1;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Accuracy { get; set; }
    public int Mode { get; set; }
    public string Poses { get; set; }

    public override string ToString()
    {
        return string.Format("[Level: Id={0}, Name={1}, Accuracy={2}, Mode={3}]", Id, Name, Accuracy, Mode);
    }



}
