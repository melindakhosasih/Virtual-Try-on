using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using static Firebase.Extensions.TaskExtension;
using Newtonsoft.Json;


public class DatabaseManagement : MonoBehaviour
{
    private DatabaseReference dbReference;

    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUser(string userID, string json) {
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
    
    public void updateInfo(string userID, string json) {
        Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        dbReference.Child("users").Child(userID).UpdateChildrenAsync(data);
    }

    public async Task<DataSnapshot> getUserInfos(string info, string uid) {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        return await dbReference.Child("users").Child(uid).Child(info).GetValueAsync();
    }
}
