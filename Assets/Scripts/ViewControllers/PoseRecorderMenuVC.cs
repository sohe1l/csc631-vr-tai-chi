using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PoseRecorderMenuVC : MonoBehaviour
{

    public GameObject ScrollViewContent;
    public GameObject PoseRowPrefab;
    private const int LEVEL_ROW_OFFSET = 55;


    // Start is called before the first frame update
    void Start()
    {
        LoadPoses();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadPoses()
    {
        var db = DataService.Instance.GetConnection();

        var query = db.Table<Pose>();

        int counter = 0;
        foreach (Pose pose in query)
        {
            GameObject Row = Instantiate(PoseRowPrefab);

            Text levelName = Row.transform.Find("Name").GetComponent<Text>();
            levelName.text = pose.Name;

            Button StartBtn = Row.transform.Find("StartBtn").GetComponent<Button>();
            StartBtn.onClick.AddListener(() => LoadPose(pose.Id));

            Row.transform.SetParent(ScrollViewContent.transform, false);
            Row.transform.Translate(Vector3.down * LEVEL_ROW_OFFSET * counter);
            counter++;
        }

    }

    private void LoadPose(int id)
    {
        Prefs.SetPoseID(id);
        SceneManager.LoadScene("PoseRecorder");
    }
}
