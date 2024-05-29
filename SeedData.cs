// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity;

// namespace storeInventoryApi
// {
//     public class SeedData
//     {
//         public static async Task InitializeAsync(IServiceProvider serviceProvider)
//         {
//             var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
//             if (_roleManager != null)
//             {
//                 // IdentityRole<Guid>? role = await _roleManager.FindByNameAsync("Admin");
//                 // if (role == null)
//                 // {
//                 //     var results = await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
//                 // }

//                 List<IdentityRole<string>> roleRoles = new()
//                 {
//                     new IdentityRole<string>("Admin"),
//                     new IdentityRole<string>("Manager"),
//                     new IdentityRole<string>("Cashiers")

//                 };
//                 foreach (var role in roleRoles)
//                 {
//                     IdentityRole<string>? existingRole = await _roleManager.FindByNameAsync(role.Name!);
//                     if (existingRole == null)
//                     {
//                         _ = await _roleManager.CreateAsync(role);
//                     }

//                 }
//             }
//         }
//     }
// }