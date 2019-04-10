using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PoseRecorderVC : MonoBehaviour
{

    private IEnumerator SetVRDevice(string device, bool vrEnabled)
    {
        XRSettings.LoadDeviceByName(device);
        yield return null;
        XRSettings.enabled = vrEnabled;




    }


    // Start is called before the first frame update
    void Start()
    {
        // Activate VR
        StartCoroutine(SetVRDevice("OpenVR", true));





    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(InputTracking.GetLocalPosition(XRNode.Head));



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

    }
}
