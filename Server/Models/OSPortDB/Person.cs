using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    [Table("Person", Schema = "public")]
    public partial class Person
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
        public int PersonID { get; set; }

        //public Person Person1 { get; set; }

        [ConcurrencyCheck]
        public string PersonFirstName { get; set; }

        [ConcurrencyCheck]
        public string PersonLastName { get; set; }

        //public ICollection<Person> People1 { get; set; }
        public ICollection<HardwarePlanning> HardwarePlannings { get; set; }

    }
}