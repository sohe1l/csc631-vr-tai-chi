using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLite4Unity3d;

public class TimePoint {
    public const int TYPE_HAND_LEFT = 0;
    public const int TYPE_HAND_RIGHT = 1;
    public const int TYPE_FOOT_LEFT = 2;
    public const int TYPE_FOOT_RIGHT = 3;
    public const int TYPE_HEAD = 4;
    public const int TYPE_WAIST = 5;
     [PrimaryKey, AutoIncrement]     public int Id { get; set; }

    public int PoseID { get; set; }

    public int Type { get; set; }
     public double X { get; set; }     public double Y { get; set; }     public double Z { get; set; }      public double Time { get; set; }

    public static void GetPosePoints(SQLiteConnection db, int PoseId, int Type)
    {
        var query = db.Table<TimePoint>()
            .Where(v => v.PoseID.Equals(PoseId))
            .Where(v => v.Type.Equals(Type));

        Debug.Log("Count of points: " + query.Count());

        // times are saved in 0.1 accuracy so we will multiply everything by 10
        // to get indexes: 0, 1, 2, 3, 4, 5

        double[,] points = new double[query.Count(), 3];

        foreach (var tp in query)
        {
            Debug.Log("Got points: " + tp.X + "," + tp.Y + "," + tp.Z);

            //points[Math.Floor(tp.Time * 10), 1] = 1;
            int i = (int)Math.Floor(tp.Time * 10);
            points[i, 0] = tp.X;
            points[i, 1] = tp.Y;
            points[i, 2] = tp.Z;
        }


        Debug.Log("test points[0,1] : " + points[0, 0]);
        Debug.Log("test points[1,2] : " + points[0, 1]);
        Debug.Log("test points[2,3] : " + points[0, 2]);

        Debug.Log("test points[0,1] : " + points[1, 0]);
        Debug.Log("test points[1,2] : " + points[1, 1]);
        Debug.Log("test points[2,3] : " + points[1, 2]);

    }

    public static void TestAdd(SQLiteConnection db)
    {
        var pose = new Pose()
        {
            Name = "Test Pose",
            Difficulty = Pose.DIFF_MEDIUM
        };

        db.Insert(pose);
        Debug.Log(("Added Pose#{0} {1}", pose.Id, pose.Name));

        // Add points
        var tp = new TimePoint()
        {
            PoseID = pose.Id,
            Type = TYPE_FOOT_LEFT,
            X = 1,
            Y = 1.1,
            Z = 1.2,
            Time = 0
        };
        db.Insert(tp);
        db.Insert(tp);
        db.Insert(tp);
    }




    //var tp2 = new TimePoint()
    //{
    //    PoseID = pose.Id,
    //    Type = TYPE_FOOT_LEFT,
    //    X = 2,
    //    Y = 2.1,
    //    Z = 2.2,
    //    Time = 0.1
    //};
    //db.Insert(tp2);

    //var tp3 = new TimePoint()
    //{
    //    PoseID = pose.Id,
    //    Type = TYPE_FOOT_LEFT,
    //    X = 3,
    //    Y = 3.1,
    //    Z = 3.2,
    //    Time = 0.2
    //};
    //db.Insert(tp3);
    // Console.WriteLine("{0} == {1}", stock.Symbol, stock.Id);

} 