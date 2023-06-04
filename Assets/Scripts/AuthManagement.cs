using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;

public class AuthManagement : MonoBehaviour
{
    public InputField username;
    public InputField fullname;
    public InputField email;
    public InputField password;
    private string userID;
    public DatabaseManagement database;
    private FirebaseAuth auth;
    private FirebaseUser user;
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

    private Task<Firebase.Auth.AuthResult> createAccount() {
        return auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text);
    }

    private Task setProfile() {
        user = auth.CurrentUser;
        if (user != null) {
            UserProfile profile = new UserProfile {
                DisplayName = fullname.text
            };
            return user.UpdateUserProfileAsync(profile);;
        }
        return null;
    }

    private void storeProfileInfo() {
        User newUser = new User(username.text, fullname.text, email.text, userID);
        string json = JsonUtility.ToJson(newUser);
        database.CreateUser(userID, json);
    }

    public async void createUser() {
        try {
            AuthResult result = await createAccount();
            userID = result.User.UserId;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", result.User.DisplayName, userID);
            await setProfile();
            storeProfileInfo();
            SceneManager.LoadScene("Reg_Succcess");
        } catch (System.Exception error) {
            Debug.LogError("Error creating user: " + error.Message);
        }
    }
}