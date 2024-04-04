using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    [Table("Derivative", Schema = "public")]
    public partial class Derivative
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DerivativeID { get; set; }

        [ConcurrencyCheck]
        public string DerivativeName { get; set; }

        public ICollection<HardwarePlanning> HardwarePlannings { get; set; }

    }
}