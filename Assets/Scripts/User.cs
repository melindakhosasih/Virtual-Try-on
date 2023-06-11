using Newtonsoft.Json;
using System.Collections.Generic;
public class User
{
    public string username;
    public string fullname;
    public string email;
    public string uid;
    public Dictionary<string, string> carts;

    public User(string username, string fullname, string email, string uid) {
        this.username = username;
        this.fullname = fullname;
        this.email = email;
        this.uid = uid;
        this.carts = new Dictionary<string, string>() {
            {"1", 0},
            {"2", 0},
            {"3", 0},
            {"4", 0},
            {"5", 0},
            {"6", 0},
            {"7", 0},
            {"8", 0},
            {"9", 0},
        };
    }
}
