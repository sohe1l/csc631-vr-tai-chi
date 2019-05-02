using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerVC : MonoBehaviour
{

    public Transform[] FootTarget;
    public Transform LookTarget;
    public Transform HandTarget;
    public Transform HandPole;
    public Transform Step;
    public Transform Attraction;

    public Transform TargetHandLeft;
    public Transform TargetHandRight;
    public GameObject Master;


    // Start is called before the first frame update
    void Start()
    {
        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));
    }

    // Update is called once per frame
    void Update()
    {
        TargetHandLeft.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        TargetHandRight.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);

        LookTarget.SetPositionAndRotation(InputTracking.GetLocalPosition(XRNode.Head), InputTracking.GetLocalRotation(XRNode.Head));

        var res = InputTracking.GetLocalRotation(XRNode.Head) *  new Vector3(0, (float)-0.2, 1);
        LookTarget.transform.position += res;
        
        

        Vector3 headLoc = InputTracking.GetLocalPosition(XRNode.Head);
        Quaternion headRotation = InputTracking.GetLocalRotation(XRNode.Head);
        headRotation.Set(0, headRotation.y, 0, headRotation.w);

        Master.transform.position = new Vector3(headLoc.x, 0, headLoc.z);
        Master.transform.rotation = headRotation;
    }
}
