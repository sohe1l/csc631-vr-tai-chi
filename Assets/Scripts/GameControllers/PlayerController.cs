using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
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
        Vector3 HeadPos = new Vector3(InitialHeadPos.x, InputTracking.GetLocalPosition(XRNode.Head).y, InitialHeadPos.z);
        Vector3 RightPos = HeadPos - (InputTracking.GetLocalPosition(XRNode.Head) - InputTracking.GetLocalPosition(XRNode.RightHand));
        Vector3 LeftPos = HeadPos - (InputTracking.GetLocalPosition(XRNode.Head) - InputTracking.GetLocalPosition(XRNode.LeftHand));
        
        Head.transform.SetPositionAndRotation(HeadPos, InputTracking.GetLocalRotation(XRNode.Head));
        Right.transform.SetPositionAndRotation(RightPos, InputTracking.GetLocalRotation(XRNode.RightHand));
        Left.transform.SetPositionAndRotation(LeftPos, InputTracking.GetLocalRotation(XRNode.LeftHand));
    }
}
