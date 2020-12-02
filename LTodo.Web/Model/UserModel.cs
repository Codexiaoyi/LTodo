using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace LTodo.Web.Model
{
    public class UserModel
    {
        public UserModel()
        {
            Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
        }

        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public Platform Platform { get; set; }

        public int Level { get; set; } = 1;
    }
}
