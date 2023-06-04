using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

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
}
