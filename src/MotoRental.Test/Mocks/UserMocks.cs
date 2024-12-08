using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using MotoRental.Core.Entities;

namespace MotoRental.Test.Mocks
{
    public class UserMocks
    {
        public static User GetValidUser()
        {
            var faker = new Faker<User>("pt_BR")
                .CustomInstantiator(f => new User(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Date.Past(30, DateTime.UtcNow.AddYears(-18)),
                    f.Internet.Password(),
                    "user"
                ));

            var fakeUser = faker.Generate();
            return fakeUser;
        }

    }
}