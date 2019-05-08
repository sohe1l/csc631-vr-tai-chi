using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMasterVC : MonoBehaviour
{

    public GameObject M1, M2, M3, M4, M5, M6, M7, M8;
    private MasterController MC1, MC2, MC3, MC4, MC5, MC6, MC7, MC8;

    private Pose[] Poses; // poses for the current level
    private int currentPoseIndex = -1; // current pose index. -1 means game not started.
    PoseLoader PL = PoseLoader.Instance;
    float delta = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Utils.SetVRDevice("OpenVR", true));


        MC1 = M1.GetComponent<MasterController>();
        MC2 = M2.GetComponent<MasterController>();
        MC3 = M3.GetComponent<MasterController>();
        MC4 = M4.GetComponent<MasterController>();
        MC5 = M5.GetComponent<MasterController>();
        MC6 = M6.GetComponent<MasterController>();
        MC7 = M7.GetComponent<MasterController>();
        MC8 = M8.GetComponent<MasterController>();

        loadLevel();
        currentPoseIndex = 0;

    }

    void loadLevel()
    {
        var db = DataService.Instance.GetConnection();
        var currentLevel = db.Table<Level>()
            .Where(v => v.Id.Equals(4))
            .First();

        string[] poses = currentLevel.Poses.Split(',');
 
        Poses = new Pose[poses.Length];

        for (int i = 0; i < poses.Length; i++)
        {
            int poseId = int.Parse(poses[i]);
            var poseQuery = db.Table<Pose>()
                .Where(v => v.Id.Equals(poseId)); // convert to int

            Poses[i] = poseQuery.First();
        }
    }


    // Update is called once per frame
    void Update()
    {


        delta += Time.deltaTime;
        if (delta > 0.1)
        {

            if (currentPoseIndex != -1 && !PL.NextFrame())
            {
                currentPoseIndex++;
                Debug.Log("Changed to the pose " + currentPoseIndex);
                PL.SwitchPose(Poses[currentPoseIndex].Id);
            }
            MC1.UpdateMaster();
            MC2.UpdateMaster();
            MC3.UpdateMaster();
            MC4.UpdateMaster();
            MC5.UpdateMaster();
            MC6.UpdateMaster();
            MC7.UpdateMaster();
            MC8.UpdateMaster();
            delta = 0;
        }

    }
}
