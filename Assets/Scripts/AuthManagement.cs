using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class AuthManagement : MonoBehaviour
{
    public InputField username;
    public InputField fullname;
    public InputField email;
    public InputField password;
    public DatabaseManagement database;
    private FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
        password.contentType = InputField.ContentType.Password;
        auth = FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createUser() {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            
            User newUser = new User(username.text, fullname.text, email.text, result.User.UserId);
            string json = JsonUtility.ToJson(newUser);
            database.CreateUser(result.User.UserId, json);
        });
    }
}
