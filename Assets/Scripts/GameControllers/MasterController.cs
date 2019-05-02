using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MasterController : MonoBehaviour
{
 
    int counter = 0;
    TableQuery<TimePoint> QueryLeft;
    IEnumerator<TimePoint> EQ_Left;
    TableQuery<TimePoint> QueryRight;
    IEnumerator<TimePoint> EQ_Right;
    TableQuery<TimePoint> QueryHead;
    IEnumerator<TimePoint> EQ_Head;

    float delta = 0;

    public GameObject Head;
    public GameObject Right;
    public GameObject Left;

    Vector3 InitialHeadPos;
    Vector3 InitialRecordedHeadPos;

    Vector3 eyeOffset = new Vector3(0,0.1f,0);


    // Start is called before the first frame update
    void Start()
    {
        InitialHeadPos = Head.transform.transform.position;
        SwitchPose(1);
 
    }


    public void SwitchPose(int PoseID)
    {
        var db = DataService.Instance.GetConnection();

        QueryLeft = db.Table<TimePoint>()
            .Where(v => v.PoseID.Equals(PoseID))
            .Where(v => v.Type.Equals(TimePoint.TYPE_HAND_LEFT));


        QueryRight = db.Table<TimePoint>()
            .Where(v => v.PoseID.Equals(PoseID))
            .Where(v => v.Type.Equals(TimePoint.TYPE_HAND_RIGHT));

        QueryHead = db.Table<TimePoint>()
            .Where(v => v.PoseID.Equals(PoseID))
            .Where(v => v.Type.Equals(TimePoint.TYPE_HEAD));


        EQ_Left = QueryLeft.GetEnumerator();
        EQ_Right = QueryRight.GetEnumerator();
        EQ_Head = QueryHead.GetEnumerator();

        EQ_Head.MoveNext();
        InitialRecordedHeadPos = EQ_Head.Current.getV3();
        EQ_Head.Reset();
  
    }

    void Update()
    {

        try
        {

            delta += Time.deltaTime;
            if (delta > 0.1)
            {
                if (!EQ_Left.MoveNext())
                {
                    EQ_Left.Reset();
                    EQ_Right.Reset();
                    EQ_Head.Reset();
                }
                Vector3 tpLeft = EQ_Left.Current.getV3();
                //Left.transform.position = new Vector3((float)tp.X, (float)tp.Y, (float)tp.Z);


                EQ_Right.MoveNext();
                Vector3 tpRight = EQ_Right.Current.getV3();
                //Right.transform.position = new Vector3((float)tpr.X, (float)tpr.Y, (float)tpr.Z);

                EQ_Head.MoveNext();
                Vector3 tpHead = EQ_Head.Current.getV3();
                //Head.transform.position = new Vector3((float)tprt.X, (float)tprt.Y, (float)tprt.Z);


                Vector3 HeadPos = InitialHeadPos - (InitialRecordedHeadPos - tpHead);
                Vector3 RightPos = HeadPos - (tpHead - tpRight);
                Vector3 LeftPos = HeadPos - (tpHead - tpLeft);


                //Head.transform.position = HeadPos;
                //Right.transform.position = RightPos;
                //Left.transform.position = LeftPos;

                Head.transform.SetPositionAndRotation(HeadPos - eyeOffset, EQ_Head.Current.getQ());
                Right.transform.SetPositionAndRotation(RightPos, EQ_Right.Current.getQ());
                Left.transform.SetPositionAndRotation(LeftPos, EQ_Left.Current.getQ());



                Debug.Log(EQ_Left.Current);
                
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
