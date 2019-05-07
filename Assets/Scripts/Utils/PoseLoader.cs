using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoseLoader : MonoBehaviour
{

    private TableQuery<TimePoint> QueryLeft;
    private TableQuery<TimePoint> QueryRight;
    private TableQuery<TimePoint> QueryHead;

    public IEnumerator<TimePoint> EQ_Left;
    public IEnumerator<TimePoint> EQ_Right;
    public IEnumerator<TimePoint> EQ_Head;

    Vector3 InitialRecordedHeadPos;

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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    private PoseLoader()
    {
    }

    public static PoseLoader Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly PoseLoader instance = new PoseLoader();
    }
}
