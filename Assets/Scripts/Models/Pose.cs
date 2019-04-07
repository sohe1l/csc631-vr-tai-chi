using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

public class Pose
{

    public const int DIFF_EASY = 0;
    public const int DIFF_MEDIUM = 1;
    public const int DIFF_HARD = 2;



    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Difficulty { get; set; }

    //public int LeftHand { get; set; }
    //public int RightHand { get; set; }
    //public int Head { get; set; }
    //public int Waist { get; set; }
    //public int LeftFoot { get; set; }
    //public int RightFoot { get; set; }

    public string Video { get; set; }

    public override string ToString()
    {
        return string.Format("[Pose: Id={0}, Name={1},  Difficulty={2}]", Id, Name, Difficulty);
    }


    public static void GetPose()
    {

    }
}
