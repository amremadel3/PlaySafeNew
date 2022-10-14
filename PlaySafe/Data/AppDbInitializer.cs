using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlaySafe.Models;
using System.Security.Cryptography;
using System.Text;

namespace PlaySafe.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
               
                var context = serviceScope.ServiceProvider.GetService<dbContext>();
                if (context == null) return;
                if (!context.userType.Any())
                {
                    context.userType.AddRange(
                    new userType()
                    {
                        id = Guid.NewGuid(),
                        usersType = "Admin"
                    },
                    new userType()
                    {
                        id = Guid.NewGuid(),
                        usersType = "Player"
                    },
                    new userType()
                    {
                        id = Guid.NewGuid(),
                        usersType = "Guard"
                    },
                    new userType()
                    {
                        id = Guid.NewGuid(),
                        usersType = "Owner"
                    });
                    context.SaveChanges();
                }
                
                var ownerId = context.userType.Where(x => x.usersType == "Owner").FirstOrDefault();
                
                
                if (ownerId != null) {
                        if (!context.user.Where(x => x.userTypeId == ownerId.id).Any())
                        {
                            using (HashAlgorithm algorithm = SHA256.Create())
                            {
                                byte[] password = algorithm.ComputeHash(Encoding.UTF8.GetBytes("Admin"));
                                context.user.Add(
                                new user()
                                {
                                    id = Guid.NewGuid(),
                                    userTypeId = ownerId.id,
                                    name = "defaultOwner",
                                    userName = "Admin",
                                    password = password,
                                    createdDate = DateTime.Now,
                                    phoneNum = "123",
                                });
                            }
                    context.SaveChanges();
                        }
                }
                if (!context.entry.Any())
                {
                    context.entry.AddRange(
                    new entry()
                    {
                        id = Guid.NewGuid(),
                        price = 10,
                        points = 20

                    },
                    new entry()
                    {
                        id = Guid.NewGuid(),
                        price = 5,
                        points = 10

                    },
                    new entry()
                    {
                        id = Guid.NewGuid(),
                        price = 20,
                        points = 40

                    }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
