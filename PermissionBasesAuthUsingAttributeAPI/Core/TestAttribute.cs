using PermissionBasesAuthUsingAttributeAPI.Controllers;
using PermissionBasesAuthUsingAttributeAPI.Enum;
using PermissionBasesAuthUsingAttributeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasesAuthUsingAttributeAPI
{
    public class TestAttribute : TypeFilterAttribute
    {
        public TestAttribute(PermissionEnum.Rights[] item) : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { item };
        }

        public class AuthorizeActionFilter : IAuthorizationFilter
        {
            private readonly PermissionEnum.Rights[] _item;

            public AuthorizeActionFilter(PermissionEnum.Rights[] item)
            {
                _item = item;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                string userId = context.HttpContext.Request?.Headers["UserId"].ToString();
                
                var userList = UserList();  // we can load these from db
                var _right = _item[0].ToString();
                bool isUserPermission = userList.Where(w => w.Id == Convert.ToInt32(userId) && w.Right == _right).Any();
                if (!isUserPermission)
                {
                    var _res = new { status = 401, Message = "Unauthorized Access", Data = "Unauthorized Access" };
                    var result = new JsonResult(_res);
                    result.StatusCode = 401;
                    context.Result = result;
                    return;
                }
                
            }

            public List<Users> UserList()
            {
                List<Users> users = new List<Users>
                {
                    new Users { Id = 1, Name = "AAA", Right ="ADD"},
                    new Users { Id = 2, Name = "BBB" , Right ="EDIT"},
                    new Users { Id = 10, Name = "CCC", Right ="VIEW"},
                    new Users { Id=22, Name = "DDD" , Right ="ADD"},
                    new Users { Id=15, Name = "EEE", Right ="DELETE" }
                };
                return users;
            }
        }
    }



}

