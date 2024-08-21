﻿using System.ComponentModel.DataAnnotations;

namespace FamilyCookbook.REST_Models.Picture
{
    public sealed class PictureCreate
    {
        [StringLength(100, ErrorMessage ="Maximum allowed number of characters :50")]
        public string Name { get; set; }

    }
}
