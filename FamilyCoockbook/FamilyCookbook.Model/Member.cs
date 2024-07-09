﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FamilyCookbook.Model
{
    public class Member
    {
        public int Id { get; set; }

        public Guid UniqueId { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string Bio { get; set; }

        public bool IsActive { get; set; }

        public DateOnly DateCreated { get; set; }

        public DateOnly DateUpdated { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("PictureId")]
        public Picture? Picture { get; set; }

        public IList<Recipe> Recipes { get; set; }

    }
}
