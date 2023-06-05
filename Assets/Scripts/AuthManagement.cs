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
    public enum Mode {
        Read,
        Write
    };
    public InputField username;
    public InputField fullname;
    public InputField email;
    public InputField password;
    public Mode mode;
    private string userID;
    public DatabaseManagement database;
    private FirebaseAuth auth;
    private FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (mode == Mode.Read) {
            loadUserProfile();
        } else {
            password.contentType = InputField.ContentType.Password;
        }
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

    public Task login() {
        return auth.SignInWithEmailAndPasswordAsync(email.text, password.text);
    }

    public async void handleLogin() {
        try {
            await login();
            user = auth.CurrentUser;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
            SceneManager.LoadScene("Catalogue");
        } catch (System.Exception error) {
            Debug.LogError("Error login: " + error.Message);
        }
    }

    private async void loadUserProfile() {
        user = auth.CurrentUser;
        email.text = user.Email;
        fullname.text = user.DisplayName;
        var usernameData = await database.getUserInfos("username", user.UserId);
        print(usernameData);
        if(usernameData != null) {
            username.text = usernameData.Value.ToString();
        }
    }
}
