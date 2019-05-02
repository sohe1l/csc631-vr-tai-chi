using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Recorder
{
    #region vars
    private DateTime start_time;
    private TimeSpan time;
    private int last_elapsed = -1;
    private int count = 0;
    float x_sum = 0, y_sum = 0, z_sum = 0;
    private bool started_record = false;
    bool started_validation = false;
    XRNode Node;
    int poseId;
    int poseType;
    public Dictionary<int, float[]> record_points = new Dictionary<int, float[]>();
    public Dictionary<int, float[]> validate_points = new Dictionary<int, float[]>();
    Material MaterialRecord;
    Material MaterialValidate;
    #endregion

    public Recorder(int poseId, XRNode Node, int poseType, Material MaterialRecord, Material MaterialValidate)
    {
        this.Node = Node;
        this.poseId = poseId;
        this.poseType = poseType;
        this.MaterialRecord = MaterialRecord;
        this.MaterialValidate = MaterialValidate;
    }

    public void Record()
    {
        if (!started_record)
        {
            start_time = DateTime.Now;
            started_record = true;
        }
        RecordHelper(record_points, MaterialRecord);
    }

    public void RecordValidate()
    {
        if (!started_validation)
        {
            start_time = DateTime.Now;
            count = 0;
            x_sum = y_sum = z_sum = 0;
            started_validation = true;
        }

        RecordHelper(validate_points, MaterialValidate);
    }

    private void RecordHelper(Dictionary<int, float[]> dict, Material material)
    {
        time = DateTime.Now - start_time;
        int elapsed = (int)(Math.Round(time.Seconds + time.Milliseconds / 1000.0, 1) * 10);
        if (last_elapsed != elapsed && count != 0)
        {
            dict.Add(elapsed, new float[] { x_sum / count, y_sum / count, z_sum / count });

            if (dict.Count != 0 && dict.ContainsKey(elapsed - 1))
            {
                float[] start = dict[elapsed - 1];
                Utils.DrawLine(new Vector3((float)start[0], (float)start[1], (float)start[2]),
                        new Vector3((float)x_sum / count, (float)y_sum / count, (float)z_sum / count), material);
            }
            count = 0;
            x_sum = y_sum = z_sum = 0;
            last_elapsed = elapsed;
        }

        Vector3 input = InputTracking.GetLocalPosition(Node);
        x_sum += input.x;
        y_sum += input.y;
        z_sum += input.z;
        count++;
    }

    public double Score()
    {
        double total_diff = 0;
        for (int i = 0; i < record_points.Count; i++)
        {
            try
            {
                double off = Math.Sqrt(
                    (Math.Pow(validate_points[i][0] - record_points[i][0], 2)
                    + Math.Pow(validate_points[i][1] - record_points[i][1], 2)
                    + Math.Pow(validate_points[i][2] - record_points[i][2], 2))
                    );
                total_diff += off;
            }
            catch { }
        }

        Debug.Log("Total difference " + total_diff);
        return total_diff;
    }

    public void Save()
    {
        var db = DataService.Instance.GetConnection();

        //// delete previous rows
        //var res = db.Table<TimePoint>()
        //            .Where(v => v.PoseID.Equals(poseId))
        //            .Where(v => v.Type.Equals(poseType));
        //foreach(var row in res)
        //{

        //}



        foreach (var tp in record_points)
        {
            var timePoint = new TimePoint()
            {
                PoseID = poseId,
                Type = poseType,
                X = tp.Value[0],
                Y = tp.Value[1],
                Z = tp.Value[2],
                Time = tp.Key
            };

            db.Insert(timePoint);
        }
    }
}
