using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MasterDummyVC : MonoBehaviour
{

    public GameObject MasterDummyHead;

    public GameObject MasterDummyLeftHand;

    public GameObject MasterDummyRightHand;



    // Start is called before the first frame update
    void Start()
    {

        // Start VR
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

    }

    // Update is called once per frame
    void Update()
    {



        //InputTracking.ge
        //    XRNode.RightHand

        MasterDummyHead.transform.SetPositionAndRotation(
            InputTracking.GetLocalPosition(XRNode.Head),
            InputTracking.GetLocalRotation(XRNode.Head)
        );

        //MasterDummyRightHand.transform.SetPositionAndRotation(
        //    InputTracking.GetLocalPosition(XRNode.RightHand),
        //    InputTracking.GetLocalRotation(XRNode.RightHand)
        //);

        //MasterDummyLeftHand.transform.SetPositionAndRotation(
        //    InputTracking.GetLocalPosition(XRNode.LeftHand)        );



    }
}
