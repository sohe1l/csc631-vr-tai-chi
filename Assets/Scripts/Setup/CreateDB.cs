using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var conn = DataService.Instance.GetConnection();

        conn.DropTable<Player>();
        conn.CreateTable<Player>();

        conn.DropTable<Level>();
        conn.CreateTable<Level>();

        conn.DropTable<Pose>();
        conn.CreateTable<Pose>();

        conn.DropTable<TimePoint>();
        conn.CreateTable<TimePoint>();

        conn.DropTable<Leaderboard>();
        conn.CreateTable<Leaderboard>();

        InsertPoses();
        InsertLevels();
    }



    void InsertPoses()
    {
        var db = DataService.Instance.GetConnection();

        // 1
        db.Insert(new Pose()
        {
            Name = "Starting",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://giphy.com/gifs/cbs-h50-l2Sqb3eAKkP06JkRy"
        });

        // 2
        db.Insert(new Pose()
        {
            Name = "Pull the bow",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://youtu.be/cwlvTcWR3Gs?t=233"
        });

        // 3
        db.Insert(new Pose()
        {
            Name = "Reach for sky",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://youtu.be/cwlvTcWR3Gs?t=107"
        });

        // 4
        db.Insert(new Pose()
        {
            Name = "Hug a barrel",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://youtu.be/cEOS2zoyQw4?t=185"
        });

        // 5
        db.Insert(new Pose()
        {
            Name = "Push the oceans",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://youtu.be/cEOS2zoyQw4?t=220"
        });

        // 6
        db.Insert(new Pose()
        {
            Name = "Carress the moon",
            Difficulty = Pose.DIFF_EASY,
            Video = "https://youtu.be/cEOS2zoyQw4?t=292"
        });

        // 7
        db.Insert(new Pose()
        {
            Name = "Spread the wings",
            Difficulty = Pose.DIFF_MEDIUM,
            Video = "https://youtu.be/cwlvTcWR3Gs?t=414"
        });

        // 8
        db.Insert(new Pose()
        {
            Name = "Push the clouds",
            Difficulty = Pose.DIFF_MEDIUM,
            Video = "https://youtu.be/PNtWqDxwwMg?t=443"
        });

        // 9
        db.Insert(new Pose()
        {
            Name = "Reach for heaven",
            Difficulty = Pose.DIFF_MEDIUM,
            Video = "https://youtu.be/cwlvTcWR3Gs?t=852"
        });

        // 10
        db.Insert(new Pose()
        {
            Name = "Strong iron guard",
            Difficulty = Pose.DIFF_MEDIUM,
            Video = "https://youtu.be/cwlvTcWR3Gs?t=960"
        });

    }

    private void InsertLevels()
    {
        var db = DataService.Instance.GetConnection();

        db.Insert(new Level()
        {
            Name = "Starting",
            Mode = Level.MODE_TRANING,
            Poses = "1,1,1,1,1"
        });

        db.Insert(new Level()
        {
            Name = "Pull the bow",
            Mode = Level.MODE_TRANING,
            Poses = "2,2,2,2,2"
        });

        db.Insert(new Level()
        {
            Name = "Reach for sky",
            Mode = Level.MODE_TRANING,
            Poses = "3,3,3,3,3"
        });

        db.Insert(new Level()
        {
            Name = "Hug a barrel",
            Mode = Level.MODE_TRANING,
            Poses = "4,4,4,4,4"
        });

        db.Insert(new Level()
        {
            Name = "Push the oceans",
            Mode = Level.MODE_TRANING,
            Poses = "5,5,5,5,5"
        });

        db.Insert(new Level()
        {
            Name = "Carress the moon",
            Mode = Level.MODE_TRANING,
            Poses = "6,6,6,6,6"
        });

        db.Insert(new Level()
        {
            Name = "Spread the wings",
            Mode = Level.MODE_TRANING,
            Poses = "7,7,7,7,7"
        });

        db.Insert(new Level()
        {
            Name = "Push the clouds",
            Mode = Level.MODE_TRANING,
            Poses = "8,8,8,8,8"
        });

        db.Insert(new Level()
        {
            Name = "Reach for heaven",
            Mode = Level.MODE_TRANING,
            Poses = "9,9,9,9,9"
        });

        db.Insert(new Level()
        {
            Name = "Strong iron guard",
            Mode = Level.MODE_TRANING,
            Poses = "10,10,10,10,10"
        });


        db.Insert(new Level()
        {
            Name = "Level 1",
            Mode = Level.MODE_SCORED,
            Poses = "1,1,3,3,9,9,4,4,6,6"
        });

        db.Insert(new Level()
        {
            Name = "Level 2",
            Mode = Level.MODE_SCORED,
            Poses = "1,1,2,2,10,10,8,8,5,5"
        });

        db.Insert(new Level()
        {
            Name = "Level 3",
            Mode = Level.MODE_SCORED,
            Poses = "1,1,8,8,7,7,5,5,6,6"
        });

    }
}
