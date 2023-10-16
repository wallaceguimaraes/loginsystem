using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginSystem.Factories;
using LoginSystem.Models.EntityModels;
using LoginSystem.Models.Interfaces;

namespace LoginSystem.Models.ServiceModel
{
    public class Authentication : IAuthentication
    {
        public (bool, User) SignIn(string login, string password)
        {
            var user = new UserFactory().Users.Where(x => x.Login == login && x.Password == password).SingleOrDefault();

            if (user is null)
                return (false, null);

            return (true, user);
        }
    }
}