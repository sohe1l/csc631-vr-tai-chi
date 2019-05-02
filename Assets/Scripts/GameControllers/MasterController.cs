using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MasterController : MonoBehaviour
{
    public GameObject Head;
    public GameObject Right;
    public GameObject Left;

    Vector3 InitialHeadPos;

    // Start is called before the first frame update
    void Start()
    {
        InitialHeadPos = Head.transform.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        


        //Head.transform.SetPositionAndRotation(
        //    new Vector3(InitialHeadPos.x, InputTracking.GetLocalPosition(XRNode.Head).y, InitialHeadPos.z),
        //    InputTracking.GetLocalRotation(XRNode.Head)
        //);

        //Right.transform.SetPositionAndRotation(
        //    InputTracking.GetLocalPosition(XRNode.RightHand),
        //    InputTracking.GetLocalRotation(XRNode.RightHand)
        //);

        //Left.transform.SetPositionAndRotation(
        //    InputTracking.GetLocalPosition(XRNode.LeftHand),
        //    InputTracking.GetLocalRotation(XRNode.LeftHand)
        //);

        //Head.transform.Translate(0, 0, 2, Space.World);
        //Right.transform.Translate(0, 0, 2, Space.World);
        //Left.transform.Translate(0, 0, 2, Space.World);



        //Vector3 vrHeadPos = InputTracking.GetLocalPosition(XRNode.Head);
        //Master.transform.position = new Vector3(m.x, 0, m.z);

        //TargetHandLeft.transform.Translate(0, 0, 2, Space.World);
        //TargetHandRight.transform.Translate(0, 0, 2, Space.World);
        //Master.transform.Translate(0, 0, 2, Space.World);



    }
}
