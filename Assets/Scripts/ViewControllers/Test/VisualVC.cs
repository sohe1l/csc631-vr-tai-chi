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


    public GameObject handSphereLeft;
    public GameObject handSphereRight;

    public GameObject[] leadingSpheresLeft;
    public GameObject[] leadingSpheresRight;

    public LineRenderer validationLineLeft;
    public LineRenderer validationLineRight;
    public int leadingNumber;


    int counter = 0;

    //db queries for pose data 
    TableQuery<TimePoint> QueryLeft;
    TableQuery<TimePoint> QueryRight;

    //current time point enumerators
    IEnumerator<TimePoint> EQ_Left;
    IEnumerator<TimePoint> EQ_Right;

    //leading time point enumerators
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

        for (int i = 0; i < leadingNumber; i++)
        {
            EQ_LeftLead.MoveNext();
            EQ_RightLead.MoveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));

        leadingNumber = 7;

        handSphereLeft = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        handSphereLeft.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer = handSphereLeft.GetComponent<Renderer>();
        objectRenderer.material.color = Color.green;

        handSphereRight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        handSphereRight.transform.localScale = new Vector3((float).1, (float).1, (float).1);
        Renderer objectRenderer2 = handSphereRight.GetComponent<Renderer>();
        objectRenderer2.material.color = Color.green;

        validationLineLeft = CreateLineRenderer();
        validationLineRight = CreateLineRenderer();
        leadingSpheresLeft = CreateLeadingSpheres(leadingNumber);
        leadingSpheresRight = CreateLeadingSpheres(leadingNumber);

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

                //this is the VR input positions I think
                RealHandLeft.transform.position = UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.LeftHand);
                RealHandRight.transform.position = UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.RightHand);

                //this first section handles the current time enumerators and the validation line
                EQ_Left.MoveNext();
                TimePoint tp = EQ_Left.Current;
                TargetHandLeft.transform.position = new Vector3((float)tp.X, (float)tp.Y, (float)tp.Z);

                EQ_Right.MoveNext();
                TimePoint tpr = EQ_Right.Current;
                TargetHandRight.transform.position = new Vector3((float)tpr.X, (float)tpr.Y, (float)tpr.Z);

                //first input is database value, second input is player input value
                StartCoroutine(UpdateVisual(TargetHandLeft.transform.position, RealHandLeft.transform.position, handSphereLeft, validationLineLeft));
                StartCoroutine(UpdateVisual(TargetHandRight.transform.position, RealHandRight.transform.position, handSphereRight, validationLineRight));


                //this second section handles the leading visual trail using separate enumerators which have been pre-incremented
                EQ_LeftLead.MoveNext();
                TimePoint tpLeadLeft = EQ_LeftLead.Current;
                Vector3 leftLeadPoint = new Vector3((float)tpLeadLeft.X, (float)tpLeadLeft.Y, (float)tpLeadLeft.Z);

                EQ_RightLead.MoveNext();
                TimePoint tpLeadRight = EQ_RightLead.Current;
                Vector3 rightLeadPoint = new Vector3((float)tpLeadRight.X, (float)tpLeadRight.Y, (float)tpLeadRight.Z);

                //MoveSphere(leadSphereLeft, leftLeadPoint);
                StartCoroutine(MoveLeadingSpheres(leadingSpheresLeft, leftLeadPoint));
                StartCoroutine(MoveLeadingSpheres(leadingSpheresRight, rightLeadPoint));

                //potential error point! if this try body of the update() method takes too long and the 
                //next frame updates before the line below is called, delta is not reset and time based events are affected
                delta = 0;
            }

        }
        catch
        {

        }
    }

    //first input is database value, second input is player input value
    IEnumerator UpdateVisual(Vector3 real, Vector3 trial, GameObject handSphere, LineRenderer lr)
    {
        Color c1 = validatePoint(real, trial);

        handSphere.transform.position = real;
        Renderer objectRenderer = handSphere.GetComponent<Renderer>();
        objectRenderer.material.color = c1;

        StartCoroutine(MoveLine(lr, real, trial, c1));

        yield return null;
    }

    void MoveSphere(GameObject sphere, Vector3 leadPoint)
    {
        sphere.transform.position = leadPoint;
        return;
    }

    //used for moving the leading trail of spheres for visual guide
    IEnumerator MoveLeadingSpheres(GameObject[] spheres, Vector3 leadPoint)
    {
        for (int i = 0; i < spheres.Length - 1; i++)
        {
            spheres[i].transform.position = spheres[i + 1].transform.position;
        }

        spheres[spheres.Length - 1].transform.position = leadPoint;

        yield return null;
    }

    IEnumerator MoveLine(LineRenderer lr , Vector3 real, Vector3 trial, Color c1)
    {
        lr.SetColors(c1, c1);
        lr.SetWidth(.01f, .01f);
        lr.SetPosition(0, trial);
        lr.SetPosition(1, real);

        yield return null;
    }


    //creates the sphere objects for the leading trail visuals
    GameObject[] CreateLeadingSpheres(int numSpheres)
    {
        GameObject[] spheres = new GameObject[numSpheres];

        for (int i = 0; i < numSpheres; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3((float).02, (float).02, (float).02);
            Renderer objectRenderer = sphere.GetComponent<Renderer>();
            objectRenderer.material.color = Color.cyan;
            spheres[i] = sphere;
        }

        //leading sphere is the final index of array
        GameObject sphere7 = spheres[numSpheres - 1];
        sphere7.transform.localScale = new Vector3((float).06, (float).06, (float).06);
        Renderer objectRenderer7 = sphere7.GetComponent<Renderer>();
        objectRenderer7.material.color = Color.cyan;

        return spheres;
    }

    LineRenderer CreateLineRenderer()
    {
        GameObject myLine = new GameObject();
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.SetWidth(.01f, .01f);

        return lr;
    }

    Color validatePoint(Vector3 positionTrue, Vector3 positionTrial, double acceptableDistance = .1)
    {
        //Debug.Log("distance between points: " + Vector3.Distance(positionTrue, positionTrial));
        if (Vector3.Distance(positionTrue, positionTrial) <= acceptableDistance)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

}
