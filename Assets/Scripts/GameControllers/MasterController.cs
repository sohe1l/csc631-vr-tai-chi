using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MasterController : MonoBehaviour
{
 
    int counter = 0;

    float delta = 0;

    public GameObject Head;
    public GameObject Right;
    public GameObject Left;

    Vector3 InitialHeadPos;
    Quaternion InitialQ;

    Vector3 eyeOffset = new Vector3(0,0.1f,0);
    PoseLoader PL = PoseLoader.Instance;

    // Start is called before the first frame update
    void Start()
    {
        InitialHeadPos = Head.transform.position;
        InitialQ = Head.transform.rotation;
    }


    public void UpdateMaster()
    {
        if (PL.IsLoaded())
        {


            Quaternion diff = Quaternion.Inverse(PL.InitialHeadQ) * InitialQ;
            
            float angle = Quaternion.Angle(PL.InitialHeadQ, InitialQ);

            Debug.Log(diff.eulerAngles.y);
            Debug.Log(angle);

            Vector3 HeadPos = InitialHeadPos - (PL.InitialHeadPos - PL.HeadV3) - eyeOffset;
            Head.transform.SetPositionAndRotation(HeadPos, PL.HeadQ * diff);


            Vector3 RightPos = HeadPos - (PL.HeadV3 - PL.RightV3);
            Vector3 LeftPos = HeadPos - (PL.HeadV3 - PL.LeftV3);


            Right.transform.SetPositionAndRotation(RightPos, PL.RightQ);
            Left.transform.SetPositionAndRotation(LeftPos, PL.LeftQ);


            //Right.transform.position = RightPos;
            //Left.transform.position = LeftPos;

            //Head.transform.RotateAround(HeadPos, angle);
            //Left.transform.RotateAround(HeadPos, angle);
            //Right.transform.RotateAround(HeadPos, angle);

            Left.transform.RotateAround(Head.transform.position, Vector3.up, diff.eulerAngles.y);
            Right.transform.RotateAround(Head.transform.position, Vector3.up, diff.eulerAngles.y);



            // Head.transform.Rotate(diff.eulerAngles);
            //Left.transform.Rotate( 180);
            //Right.transform.Rotate(Head.transform.up, 180);
            //Right.transform.Rotate(diff.eulerAngles);



        }

    }



    void Update()
    {
        //try
        //{
        //    delta += Time.deltaTime;
        //    if (delta > 0.1)
        //    {

        //        if (!PL.NextFrame()) PL.Reset();
       
        //        Vector3 HeadPos = InitialHeadPos - (PL.InitialHeadPos - PL.HeadV3) - eyeOffset;
        //        Vector3 RightPos = HeadPos - (PL.HeadV3 - PL.RightV3);
        //        Vector3 LeftPos = HeadPos - (PL.HeadV3 - PL.LeftV3);

        //        Head.transform.SetPositionAndRotation(HeadPos, PL.HeadQ);
        //        Right.transform.SetPositionAndRotation(RightPos, PL.RightQ);
        //        Left.transform.SetPositionAndRotation(LeftPos, PL.LeftQ);
                
        //        delta = 0;
        //    }

        //}
        //catch
        //{

        //}

        


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
