using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Quest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Reward { get; set; }

    public Quest() {}

    public Quest(int id, string name, string description, int reward)
    {
        Id = id;
        Name = name;
        Description = description;
        Reward = reward;
    }

    public IEnumerator LoadDataFromDatabase(string url, Player player)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Network error: {request.error}");
        }
        else
        {
            string json = request.downloadHandler.text;
            Quest[] quests = JsonConvert.DeserializeObject<Quest[]>(json);
            
            foreach (var quest in quests)
            {
                Debug.Log($"Id: {quest.Id}, Name: {quest.Name}, Description: {quest.Description}, Reward: {quest.Reward}");
            }
        }
    }
}
