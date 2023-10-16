using LoginSystem.Models.EntityModels;

namespace LoginSystem.Factories
{
    public class UserFactory
    {
        public List<User> Users { get; set; } = new List<User>();

        public UserFactory()
        {
            Users.AddRange(new List<User>{
                new User{
                    Id = 1,
                    Name = "João Santos",
                    Login = "joao",
                    Password = "@1ty11d5"
                },
                new User{
                    Id = 2,
                    Name = "Wallace Guimarães",
                    Login = "wallace",
                    Email = "wallace@gmail.com",
                    Password = "123"
                }
            });
        }
    }
}