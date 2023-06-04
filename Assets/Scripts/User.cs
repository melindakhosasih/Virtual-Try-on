public class User
{
    private string username;
    private string fullname;
    private string email;
    private string password;
    private string uid;

    public User(string username, string fullname, string email, string password, string uid) {
        this.username = username;
        this.fullname = fullname;
        this.email = email;
        this.password = password;
        this.uid = uid;
    }
}
