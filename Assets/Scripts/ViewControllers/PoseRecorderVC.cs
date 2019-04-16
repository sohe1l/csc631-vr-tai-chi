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
    enum State
    {
        Not_Started,
        Recording_Record,
        Record_Done,
        Recording_Validation,
        Validation_Done
    }

    State state = State.Not_Started;
    public Text StatusLabel;
    public Text PoseLabel;
    bool record_ended = false;
    bool record_started = false;

    private Recorder HeadRecorder;
    private Recorder LeftRecorder;
    private Recorder RightRecorder;

    public Material MaterialReocrd;
    public Material MaterialValidate;


    int last_elapsed = -1;
    int count = 0;
    double x_sum = 0, y_sum = 0, z_sum = 0;
    const string TRIGGER = "joystick button 14";



    // Start is called before the first frame update
    void Start()
    {
        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

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

        HeadRecorder = new Recorder(poseId, XRNode.Head, TimePoint.TYPE_HEAD, MaterialReocrd, MaterialValidate);
        LeftRecorder = new Recorder(poseId, XRNode.LeftHand, TimePoint.TYPE_HAND_LEFT, MaterialReocrd, MaterialValidate);
        RightRecorder = new Recorder(poseId, XRNode.RightHand, TimePoint.TYPE_HAND_RIGHT, MaterialReocrd, MaterialValidate);
    }

    void Update()
    {
        //try
        //{
            //SteamVR_Action_Pose pose = SteamVR_Input.GetPose
            if(Input.GetKey(TRIGGER))
            //if (SteamVR_Input.GetBooleanAction("GrabPinch").state)
            {
                record_started = true;

            if (state == State.Not_Started)
            {
                state = State.Recording_Record;
            }

            if (state == State.Record_Done)
            {
                state = State.Recording_Validation;
            }

            // player holding trigger for the first time
            if (state == State.Recording_Record)
                {
                    StatusLabel.text = "Recording data points";
                    HeadRecorder.Record();
                    LeftRecorder.Record();
                    RightRecorder.Record();
                }
                // player holding trigger for the second time - validation
                if (state == State.Recording_Validation)
                {
                    StatusLabel.text = "Recording data points for validation";
                    HeadRecorder.RecordValidate();
                    LeftRecorder.RecordValidate();
                    RightRecorder.RecordValidate();
                }
            }
            else
            {
                if (state == State.Recording_Record)
                {
                    StatusLabel.text = "Recording done. Hold trigger for validation.";
                    state = State.Record_Done;
                }

                if (state == State.Recording_Validation)
                {
                    double total = 0;
                    total += HeadRecorder.Score();
                    total += LeftRecorder.Score();
                    total += RightRecorder.Score();

                    StatusLabel.text = "Validation done. " + "Total diff " + total.ToString();

                HeadRecorder.Save();
                LeftRecorder.Save();
                RightRecorder.Save();


                state = State.Validation_Done;
                }

            }
        //}
        //catch
        //{
        //    StatusLabel.text = "Error occured while getting input from device";
        //}
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
        SceneManager.LoadScene("PoseRecorderMenu");
    }
}