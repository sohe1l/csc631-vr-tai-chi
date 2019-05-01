using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;

public class VisualVC : MonoBehaviour
{
    public Transform[] FootTarget;
    public Transform LookTarget;
    public Transform HandTarget;
    public Transform HandPole;
    public Transform Step;
    public Transform Attraction;

    public Camera camera;



    public Transform TargetHandLeft;
    public Transform TargetHandRight;

    public GameObject RealHandLeft;
    public GameObject RealHandRight;
    public GameObject RealHead;
    public GameObject Master;

    public GameObject leadSphereLeft;
    public GameObject leadSphereRight;

    public GameObject handSphereLeft;
    public GameObject handSphereRight;


    int counter = 0;
    TableQuery<TimePoint> QueryLeft;
    IEnumerator<TimePoint> EQ_Left;
    TableQuery<TimePoint> QueryRight;
    IEnumerator<TimePoint> EQ_Right;

    IEnumerator<TimePoint> EQ_LeftLead;
    IEnumerator<TimePoint> EQ_RightLead;

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

        EQ_Left = QueryLeft.GetEnumerator();
        EQ_Right = QueryRight.GetEnumerator();
        EQ_LeftLead = QueryLeft.GetEnumerator();
        EQ_RightLead = QueryRight.GetEnumerator();

        for (int i = 0; i < 5; i++)
        {
            EQ_LeftLead.MoveNext();
            EQ_RightLead.MoveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        handSphereLeft = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        handSphereLeft.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer = handSphereLeft.GetComponent<Renderer>();
        objectRenderer.material.color = Color.green;

        handSphereRight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        handSphereRight.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer2 = handSphereRight.GetComponent<Renderer>();
        objectRenderer2.material.color = Color.green;

        leadSphereLeft = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leadSphereLeft.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer3 = leadSphereLeft.GetComponent<Renderer>();
        objectRenderer3.material.color = Color.cyan;

        leadSphereRight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leadSphereRight.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer4 = leadSphereRight.GetComponent<Renderer>();
        objectRenderer4.material.color = Color.cyan;


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


                RealHandLeft.transform.position = UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.LeftHand);
                RealHandRight.transform.position = UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.RightHand);


                EQ_Left.MoveNext();
                TimePoint tp = EQ_Left.Current;
                TargetHandLeft.transform.position = new Vector3((float)tp.X, (float)tp.Y, (float)tp.Z);


                EQ_Right.MoveNext();
                TimePoint tpr = EQ_Right.Current;
                TargetHandRight.transform.position = new Vector3((float)tpr.X, (float)tpr.Y, (float)tpr.Z);

                //first input is database value, second input is player input value
                StartCoroutine(DrawVisual(TargetHandLeft.transform.position, RealHandLeft.transform.position, handSphereLeft));

                StartCoroutine(DrawVisual(TargetHandRight.transform.position, RealHandRight.transform.position, handSphereRight));


                EQ_LeftLead.MoveNext();
                TimePoint tpLeadLeft = EQ_LeftLead.Current;
                Vector3 leftLeadPoint = new Vector3((float)tpLeadLeft.X, (float)tpLeadLeft.Y, (float)tpLeadLeft.Z);

                EQ_RightLead.MoveNext();
                TimePoint tpLeadRight = EQ_RightLead.Current;
                Vector3 rightLeadPoint = new Vector3((float)tpLeadRight.X, (float)tpLeadRight.Y, (float)tpLeadRight.Z);

                MoveSphere(leadSphereLeft, leftLeadPoint);
                MoveSphere(leadSphereRight, rightLeadPoint);
                delta = 0;
            }

        }
        catch
        {

        }
    }

    //first input is database value, second input is player input value
    IEnumerator DrawVisual(Vector3 real, Vector3 trial, GameObject sphere)
    {

        Color c1;

        sphere.transform.position = real;

        if (validatePoint(real, trial) == true)
        {
            c1 = Color.green;
        }
        else
        {
            c1 = Color.red;
        }

        Renderer objectRenderer = sphere.GetComponent<Renderer>();
        objectRenderer.material.color = c1;


        //GameObject.Destroy(sphere, (float) 0.1);

        DrawLine(real, trial, Color.red);

        yield return null;
    }

    void MoveSphere(GameObject sphere, Vector3 leadPoint)
    {
        sphere.transform.position = leadPoint;
        return;
    }


    void DrawLine(Vector3 real, Vector3 trial, Color c1)
    {
        //Debug.Log("called from guideline @@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        GameObject myLine = new GameObject();
        myLine.transform.position = real;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        if (validatePoint(real, trial) == true)
        {
            c1 = Color.green;
        }
        else
        {
            c1 = Color.red;
        }

        float duration = (float) .1;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.SetColors(c1, c1);
        lr.SetWidth(.02f, .02f);
        lr.SetPosition(0, trial);
        lr.SetPosition(1, real);
        GameObject.Destroy(myLine, duration);
    }


    bool validatePoint(Vector3 positionTrue, Vector3 positionTrial, double acceptableDistance = .8)
    {
        //Debug.Log("distance between points: " + Vector3.Distance(positionTrue, positionTrial));
        if (Vector3.Distance(positionTrue, positionTrial) <= acceptableDistance)
        {
            //Debug.Log("green");
            return true;
        }
        else
        {
            //Debug.Log("red");
            return false;
        }
    }

    void DrawLine2(Vector3 real, Vector3 trial, Color c1)
    {


        //Debug.Log("called from guideline @@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        GameObject myLine = new GameObject();
        myLine.transform.position = real;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();

        float duration = (float).1;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.SetColors(c1, c1);
        lr.SetWidth(.02f, .02f);
        //lr.SetPositions();
;
        GameObject.Destroy(myLine, duration);
    }
}
