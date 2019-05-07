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
    Vector3 InitialRecordedHeadPos;

    Vector3 eyeOffset = new Vector3(0,0.1f,0);
    PoseLoader PL = PoseLoader.Instance;

    // Start is called before the first frame update
    void Start()
    {
        InitialHeadPos = Head.transform.transform.position;
        
        PL.SwitchPose(1); 
    }


    

    void Update()
    {


        try
        {

            delta += Time.deltaTime;
            if (delta > 0.1)
            {



                if (!PL.EQ_Left.MoveNext())
                {
                    PL.EQ_Left.Reset();
                    PL.EQ_Right.Reset();
                    PL.EQ_Head.Reset();
                }
                Vector3 tpLeft = PL.EQ_Left.Current.getV3();


                PL.EQ_Right.MoveNext();
                PL.EQ_Head.MoveNext();
                Vector3 tpRight = PL.EQ_Right.Current.getV3();
                Vector3 tpHead = PL.EQ_Head.Current.getV3();


                Vector3 HeadPos = InitialHeadPos - (InitialRecordedHeadPos - tpHead);
                Vector3 RightPos = HeadPos - (tpHead - tpRight);
                Vector3 LeftPos = HeadPos - (tpHead - tpLeft);


                Head.transform.SetPositionAndRotation(HeadPos - eyeOffset, PL.EQ_Head.Current.getQ());
                Right.transform.SetPositionAndRotation(RightPos, PL.EQ_Right.Current.getQ());
                Left.transform.SetPositionAndRotation(LeftPos, PL.EQ_Left.Current.getQ());



                Debug.Log(PL.EQ_Left.Current);
                
                delta = 0;
            }

        }
        catch
        {

        }

        


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
