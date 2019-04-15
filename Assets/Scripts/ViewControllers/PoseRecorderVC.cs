using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PoseRecorderVC : MonoBehaviour
{
    public Text StatusLabel;
    public Text PoseLabel;
    bool ended = false;
    bool started = false;

    private Recorder HeadRecorder;
    private Recorder LeftRecorder;
    private Recorder RightRecorder;

    int last_elapsed = -1;
    int count = 0;
    double x_sum = 0, y_sum = 0, z_sum = 0;
    const string TRIGGER = "joystick button 14";



    // Start is called before the first frame update
    void Start()
    {
        // Activate VR
        //StartCoroutine(SetVRDevice("OpenVR", true));
        StatusLabel.text = "Hold the trigger to start recording";
        int poseId = Prefs.GetPoseID();

        // Load pose name
        var db = DataService.Instance.GetConnection();
        var query = db.Table<Pose>()
            .Where(v => v.Id.Equals(poseId));

        if (query.Count() == 0)
        {
            StatusLabel.text = "Error! Invalid pose ID.";
        }
        Pose pose = query.First();
        PoseLabel.text = pose.Name;

        HeadRecorder = new Recorder(poseId, XRNode.Head, TimePoint.TYPE_HEAD);
        LeftRecorder = new Recorder(poseId, XRNode.LeftHand, TimePoint.TYPE_HAND_LEFT);
        RightRecorder = new Recorder(poseId, XRNode.RightHand, TimePoint.TYPE_HAND_RIGHT);
    }

    void Update()
    {
        try
        {
            //SteamVR_Action_Pose pose = SteamVR_Input.GetPose
            if (SteamVR_Input.GetBooleanAction("GrabPinch").state)
            {
                // player holding trigger for the first time
                if (!ended)
                {
                    StatusLabel.text = "Recording data points";
                    HeadRecorder.Record();
                    LeftRecorder.Record();
                    RightRecorder.Record();
                }
                // player holding trigger for the second time - validation
                if (ended)
                {
                    StatusLabel.text = "Recording data points for validation";
                    HeadRecorder.RecordValidate();
                    LeftRecorder.RecordValidate();
                    RightRecorder.RecordValidate();
                }
            }
            else
            {
                if (!ended && started)
                {
                    ended = true;
                    HeadRecorder.Score();
                    LeftRecorder.Score();
                    RightRecorder.Score();
                }
            }
        }
        catch
        {
            StatusLabel.text = "Error occured while getting input from device";
        }

    }


    private IEnumerator SetVRDevice(string device, bool vrEnabled)
    {
        XRSettings.LoadDeviceByName(device);
        yield return null;
        XRSettings.enabled = vrEnabled;
    }


    static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        // lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        lr.startColor = color;

        lr.startWidth = 0.1f;
        lr.startWidth = 0.01f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }

    private class Recorder
    {
        #region vars
        private DateTime start_time;
        private TimeSpan time;
        private int last_elapsed = -1;
        private  int count = 0;
        double x_sum = 0, y_sum = 0, z_sum = 0;
        private bool started = false;
        bool started_validation = false;
        XRNode Node;
        int poseId;
        int poseType;
        public Dictionary<int, double[]> record_points = new Dictionary<int, double[]>();
        public Dictionary<int, double[]> validate_points = new Dictionary<int, double[]>();
        #endregion  

        public Recorder(int poseId, XRNode Node, int poseType)
        {
            this.Node = Node;
            this.poseId = poseId;
            this.poseType = poseType;
        }

        public void Record()
        {
            if (!started)
            {
                start_time = DateTime.Now;
                started = true;
            }

            time = DateTime.Now - start_time;
            int elapsed = (int)(Math.Round(time.Seconds + time.Milliseconds / 1000.0, 1) * 10);
            if (last_elapsed != elapsed && count != 0)
            {
                record_points.Add(elapsed, new double[] { x_sum / count, y_sum / count, z_sum / count });
                
                if (record_points.Count != 0)
                {
                    double[] start = record_points[elapsed - 1];
                    DrawLine(new Vector3((float)start[0], (float)start[1], (float)start[2]), 
                            new Vector3((float)x_sum / count, (float)y_sum / count, (float)z_sum / count), Color.blue);
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

        public void RecordValidate()
        {
            if (!started_validation)
            {
                start_time = DateTime.Now;
                started_validation = true;
            }
            time = DateTime.Now - start_time;
            int elapsed = (int)(Math.Round(time.Seconds + time.Milliseconds / 1000.0, 1) * 10);
            if (last_elapsed != elapsed && count != 0)
            {
                validate_points.Add(elapsed, new double[] { x_sum / count, y_sum / count, z_sum / count });
                count = 0;
                x_sum = y_sum = z_sum = 0;
                last_elapsed = elapsed;
            }
            //Debug.Log(elapsed);
            Vector3 input = InputTracking.GetLocalPosition(Node);
            x_sum += input.x;
            y_sum += input.y;
            z_sum += input.z;
            count++;
        }

        public double Score()
        {
            double total_diff = 0;
            for(int i = 0; i < record_points.Count; i++)
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
                catch{}
            }
            Debug.Log("Total difference " + total_diff);
            return total_diff;
        }

        public void Save()
        {
            var db = DataService.Instance.GetConnection();

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

                db.Insert(tp);
            }
        }
    }

    public void SaveExit()
    {
        HeadRecorder.Save();
        LeftRecorder.Save();
        RightRecorder.Save();
        Exit();
    }

    public void Exit()
    {
        SceneManager.LoadScene("PoseRecorder");
    }
}


//var value = Input.GetButton("Trigger");
//Debug.Log(value);

//var value2 = Input.GetAxis("Trigger");
//Debug.Log(value2);




//Debug.Log(InputTracking.GetLocalPosition(XRNode.Head));
//Debug.Log(InputTracking.GetLocalRotation(XRNode.Head));

//Debug.Log(InputTracking.GetLocalPosition(XRNode.RightHand));
//Debug.Log(InputTracking.GetLocalRotation(XRNode.RightHand));



//// vibrate device
//InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
//HapticCapabilities capabilities;
//if (device.TryGetHapticCapabilities(out capabilities))
//{
//    if (capabilities.supportsImpulse)
//    {
//        uint channel = 0;
//        float amplitude = 0.5f;
//        float duration = 1.0f;
//        device.SendHapticImpulse(channel, amplitude, duration);
//    }
//}