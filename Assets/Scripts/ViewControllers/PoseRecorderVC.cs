using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class PoseRecorderVC : MonoBehaviour
{



    bool started = false;
    bool ended = false;
    bool started_validation = false;

    DateTime start_time = DateTime.Now;
    TimeSpan time;
    public Dictionary<int, double[]> dict = new Dictionary<int, double[]>();
    public Dictionary<int, double[]> dictV = new Dictionary<int, double[]>();

    int last_elapsed = -1;
    int count = 0;
    double x_sum = 0, y_sum = 0, z_sum = 0;
    const string TRIGGER = "joystick button 14";


    // Start is called before the first frame update
    void Start()
    {
        // Activate VR
        //StartCoroutine(SetVRDevice("OpenVR", true));

    }

    void Update()
    {
            
        if (SteamVR_Input.GetBooleanAction("GrabPinch").state)
        {
            Vector3 right = InputTracking.GetLocalPosition(XRNode.RightHand);
            Vector3 left = InputTracking.GetLocalPosition(XRNode.LeftHand);
            Vector3 head = InputTracking.GetLocalPosition(XRNode.Head);


            // player holding trigger for the first time
            if (!ended)
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
                    dict.Add(elapsed, new double[] { x_sum / count, y_sum / count, z_sum / count });

                    if(dict.Count != 0)
                    {

                        double[] start = dict[elapsed - 1];
                        DrawLine(new Vector3((float)start[0], (float)start[1], (float)start[2]), new Vector3((float)x_sum / count, (float)y_sum / count, (float)z_sum / count), Color.blue);
                    }


                    count = 0;
                    x_sum = 0;
                    y_sum = 0;
                    z_sum = 0;
                    last_elapsed = elapsed;
                }
                Debug.Log(elapsed);
                count++;
                double x = right.x;
                double y = right.y;
                double z = right.z;
                x_sum += x;
                y_sum += y;
                z_sum += z;
                //SteamVR_Action_Pose pose = SteamVR_Input.GetPose
                Debug.Log(right);
            }
            // player holding trigger for the second time - validation
            if (ended)
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
                    dictV.Add(elapsed, new double[] { x_sum / count, y_sum / count, z_sum / count });
                    count = 0;
                    x_sum = 0;
                    y_sum = 0;
                    z_sum = 0;
                    last_elapsed = elapsed;
                    double off = Math.Sqrt(
                        (Math.Pow(dict[elapsed][0] - dictV[elapsed][0], 2)
                        + Math.Pow(dict[elapsed][1] - dictV[elapsed][1], 2)
                        + Math.Pow(dict[elapsed][2] - dictV[elapsed][2], 2))
                        );
                    Debug.Log("Result " + elapsed + "  " + off);
                }
                //Debug.Log(elapsed);
                count++;
                
                
                double x = right.x;
                double y = right.y;
                double z = right.z;
                x_sum += x;
                y_sum += y;
                z_sum += z;

            }
        }
        else
        {
            if (!ended && started)
            {
                ended = true;
                //Debug.Log("else");
                foreach (KeyValuePair<int, double[]> kvp in dict)
                {
                    Debug.Log(("{0}: {1},{2},{3}", kvp.Key, kvp.Value[0], kvp.Value[1], kvp.Value[2]));
                }
            }
        }
    }


    private IEnumerator SetVRDevice(string device, bool vrEnabled)
    {
        XRSettings.LoadDeviceByName(device);
        yield return null;
        XRSettings.enabled = vrEnabled;
    }



    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
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