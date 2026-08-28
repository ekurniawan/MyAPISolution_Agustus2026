namespace MyAPISolution.SampleAPI.DTO
{
    public class UsersRoleDTO
    {
        public string RoleName { get; set; } = string.Empty;
        public List<string> Usernames { get; set; } = new List<string>();

    }
}
