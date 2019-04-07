using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var conn = DataService.Instance.GetConnection();

        conn.DropTable<Player>();
        conn.CreateTable<Player>();

        conn.DropTable<Level>();
        conn.CreateTable<Level>();

        conn.DropTable<Pose>();
        conn.CreateTable<Pose>();


    }
}
