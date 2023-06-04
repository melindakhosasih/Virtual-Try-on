using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class DatabaseManagement : MonoBehaviour
{
    public InputField username;
    public InputField fullname;
    public InputField email;
    public InputField password;
    private string userID;
    private DatabaseReference dbReference;
    // Start is called before the first frame update
    void Start()
    {
        this.userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        password.contentType = InputField.ContentType.Password;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUser() {
        User newUser = new User(username.text, fullname.text, email.text, password.text, userID);
        string json = JsonUtility.ToJson(newUser);
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
}
