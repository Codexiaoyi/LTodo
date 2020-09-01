﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model
{
    public class UserModel
    {
        public UserModel()
        {
            Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
        }

        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public int Level { get; set; } = 1;
    }
}
