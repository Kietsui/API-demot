using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public Player player;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetData();
            Debug.Log("Button pressed");
        }
    }

    public void GetData()
    {
        string url = "http://localhost:5256/questitems";

        Quest quest = new Quest();
        StartCoroutine(quest.LoadDataFromDatabase(url, player));
    }
}
