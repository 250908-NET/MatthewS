


namespace todoapi.API.users;

public class UserInfo
{
    public int userId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserInfo()
    {

    }
    public UserInfo(int id,string UserName, string Password)
    {
        this.userId = id;
        this.UserName = UserName;
        this.Password = Password;
    }


}