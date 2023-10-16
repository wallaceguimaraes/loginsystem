using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginSystem.Models.EntityModels;

namespace LoginSystem.Models.Interfaces
{
    public interface IAuthentication
    {
        (bool, User) SignIn(string login, string password);

    }
}