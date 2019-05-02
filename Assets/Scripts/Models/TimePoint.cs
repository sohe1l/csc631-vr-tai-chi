using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SQLite4Unity3d;

public class TimePoint
{
    public const int TYPE_HAND_LEFT = 0;
    public const int TYPE_HAND_RIGHT = 1;
    public const int TYPE_FOOT_LEFT = 2;
    public const int TYPE_FOOT_RIGHT = 3;
    public const int TYPE_HEAD = 4;
    public const int TYPE_WAIST = 5;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int PoseID { get; set; }

    public int Type { get; set; }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public int Time { get; set; }

    public Vector3 getV3()
    {
        return new Vector3(X, Y, Z);
    }
}
