namespace FriendsApi.Models
{
    public class Friend
    {
        public string FullName { get; set; }
        public string Blog { get; set; }
        public string Slug { get; set; }
        public Company Workplace { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }
        public string Web { get; set; }
    }
}