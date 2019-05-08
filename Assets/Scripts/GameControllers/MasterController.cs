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
           
            Vector3 HeadPos = InitialHeadPos - (PL.InitialHeadPos - PL.HeadV3) - eyeOffset;
            Head.transform.SetPositionAndRotation(HeadPos, PL.HeadQ * diff);
        
            Vector3 RightPos = HeadPos - (PL.HeadV3 - PL.RightV3);
            Vector3 LeftPos = HeadPos - (PL.HeadV3 - PL.LeftV3);
        
            Right.transform.SetPositionAndRotation(RightPos, PL.RightQ);
            Left.transform.SetPositionAndRotation(LeftPos, PL.LeftQ);

            Left.transform.RotateAround(Head.transform.position, Vector3.up, diff.eulerAngles.y);
            Right.transform.RotateAround(Head.transform.position, Vector3.up, diff.eulerAngles.y);



        }

    }

    void Update()
    {
        
    }
}
