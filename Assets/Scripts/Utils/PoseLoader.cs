using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoseLoader
{
    private int PoseID = -1;

    private TableQuery<TimePoint> QueryLeft;
    private TableQuery<TimePoint> QueryRight;
    private TableQuery<TimePoint> QueryHead;

    public IEnumerator<TimePoint> EQ_Left;
    public IEnumerator<TimePoint> EQ_Right;
    public IEnumerator<TimePoint> EQ_Head;
    
    public Vector3 InitialHeadPos;
    public Quaternion InitialHeadQ;

    Vector3 nullV3 = new Vector3(0, 0, 0);
    Quaternion nullQ = new Quaternion(0,0,0,0);

    public Vector3 LeftV3 { get { try { return EQ_Left.Current.getV3(); } catch { return nullV3; } } }
    public Vector3 RightV3 { get { try { return EQ_Right.Current.getV3(); } catch { return nullV3; } } }
    public Vector3 HeadV3 { get { try {  return  EQ_Head.Current.getV3(); } catch { return nullV3; } } }

    public Quaternion LeftQ { get { try { return EQ_Left.Current.getQ(); } catch { return nullQ; } } }
    public Quaternion RightQ { get { try { return EQ_Right.Current.getQ(); } catch { return nullQ; } } }
    public Quaternion HeadQ { get { try { return EQ_Head.Current.getQ(); } catch { return nullQ; } } }

    public bool IsLoaded()
    {
        return PoseID != -1;
    }

    public void SetNotLoaded()
    {
        PoseID = -1;
    }
    public void SwitchPose(int PoseID)
    {
        this.PoseID = -1;
        
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
        InitialHeadPos = HeadV3;
        InitialHeadQ = HeadQ;
        EQ_Head.Reset();

        this.PoseID = PoseID;
    }

    public bool NextFrame()
    {
        if (PoseID == -1) return false;
        if (!EQ_Left.MoveNext()) return false;
        if (!EQ_Right.MoveNext()) return false;
        if (!EQ_Head.MoveNext()) return false;
        return true;
    }

    public void Reset()
    {
        if (PoseID == -1) return;
        EQ_Left.Reset();
        EQ_Right.Reset();
        EQ_Head.Reset();
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
