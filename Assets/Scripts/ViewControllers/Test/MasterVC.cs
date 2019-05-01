using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class MasterVC : MonoBehaviour
{
    public Transform[] FootTarget;
    public Transform LookTarget;
    public Transform HandTarget;
    public Transform HandPole;
    public Transform Step;
    public Transform Attraction;
    public Transform Head;


    public Transform TargetHandLeft;
    public Transform TargetHandRight;
    

    int counter = 0;
    TableQuery<TimePoint> QueryLeft;
    IEnumerator<TimePoint> EQ_Left;
    TableQuery<TimePoint> QueryRight;
    IEnumerator<TimePoint> EQ_Right;
    //TableQuery<TimePoint> QueryHead;
    //IEnumerator<TimePoint> EQ_Head;

    float delta = 0;

    public void SwitchPose1()
    {
        SwitchPose(1);
    }

    public void SwitchPose2()
    {
        SwitchPose(2);
    }

    public void SwitchPose3()
    {
        SwitchPose(3);
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

    //    QueryHead = db.Table<TimePoint>()
    //.Where(v => v.PoseID.Equals(PoseID))
    //.Where(v => v.Type.Equals(TimePoint.TYPE_HEAD));
        //foreach (TimePoint tp in Query)
        //{
        //    Debug.Log(tp.X);
        //}

        EQ_Left = QueryLeft.GetEnumerator();
        EQ_Right = QueryRight.GetEnumerator();
        //EQ_Head = QueryHead.GetEnumerator();
    }

    public void LateUpdate()
    {
        ////move step & attraction
        //Step.Translate(Vector3.forward * Time.deltaTime * 0.7f);
        //if (Step.position.z > 1f)
        //    Step.position = Step.position + Vector3.forward * -2f;
        //Attraction.Translate(Vector3.forward * Time.deltaTime * 0.5f);
        //if (Attraction.position.z > 1f)
        //    Attraction.position = Attraction.position + Vector3.forward * -2f;

        ////footsteps
        //foreach (var foot in FootTarget)
        //{
        //    var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
        //    var hitInfo = new RaycastHit();
        //    if (Physics.SphereCast(ray, 0.05f, out hitInfo, 0.50f))
        //        foot.position = hitInfo.point + Vector3.up * 0.05f;
        //}

        //hand and look
        //var normDist = Mathf.Clamp((Vector3.Distance(LookTarget.position, Attraction.position) - 0.3f) / 1f, 0, 1);
        //HandTarget.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), HandTarget.rotation, normDist);
        //HandTarget.position = Vector3.Lerp(Attraction.position, HandTarget.position, normDist);
        //HandPole.position = Vector3.Lerp(HandTarget.position + Vector3.down * 2, HandTarget.position + Vector3.forward * 2f, normDist);
        //LookTarget.position = Vector3.Lerp(Attraction.position, LookTarget.position, normDist);


    }


    // Start is called before the first frame update
    void Start()
    {
        SwitchPose1();
    }

    // Update is called once per frame
    void Update()
    {

        try
        {

            delta += Time.deltaTime;
            if (delta > 0.1)
            {
                EQ_Left.MoveNext();
                TimePoint tp = EQ_Left.Current;
                TargetHandLeft.transform.position = new Vector3((float)tp.X, (float)tp.Y, (float)tp.Z);


                EQ_Right.MoveNext();
                TimePoint tpr = EQ_Right.Current;
                TargetHandRight.transform.position = new Vector3((float)tpr.X, (float)tpr.Y, (float)tpr.Z);

                //EQ_Head.MoveNext();
                //TimePoint tprt = EQ_Head.Current;
                //Head.transform.position = new Vector3((float)tprt.X, (float)tprt.Y, (float)tprt.Z);




                delta = 0;
            }

        }
        catch
        {

        }



    }
}
