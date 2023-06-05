using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using static Firebase.Extensions.TaskExtension;


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

    public async Task<DataSnapshot> getUserInfos(string info, string uid) {
            // print(dbReference.Child("users").Child(uid).Child(info).GetValueAsync().Value.ToString());
            print(dbReference.Child("users").Child(uid).Child(info).GetValueAsync().ToString());
        // try {
            return await dbReference.Child("users").Child(uid).Child(info).GetValueAsync();
        // }
        // catch (Exception e) {
        //     // Handle the error or log the exception
        //     Debug.LogError($"Error getting user info: {e.Message}");
        //     return null;
        // }
    }
}
