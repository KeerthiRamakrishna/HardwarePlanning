using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    public partial class AuditableEntity
    {

        //[Display(Name = "Created By")]
        public string? CreatedBy { get; set; }
  
        //[Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedAt { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedAt { get; set; }

        //[JsonProperty(PropertyName = "UserId", NullValueHandling = NullValueHandling.Ignore)]
        //public int? UserId { get; set; }

        ////[JsonProperty(PropertyName = "User", NullValueHandling = NullValueHandling.Ignore)]
        ////public virtual User User { get; set; }
        //[JsonProperty(PropertyName = "User", NullValueHandling = NullValueHandling.Ignore)]
        //public virtual User User { get; set; }

    }
}