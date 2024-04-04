using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    [Table("HardwarePlanning", Schema = "public")]
    public partial class HardwarePlanning
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
        public int HardwarePlanningID { get; set; }

        [ConcurrencyCheck]
        public string ProgramIncrement { get; set; }

        [ConcurrencyCheck]
        public int? StatusID { get; set; }
        public Status Status { get; set; }

        [ConcurrencyCheck]
        public int? ArchitectureID { get; set; }

        public Architecture Architecture { get; set; }

        [ConcurrencyCheck]
        public int? DerivativeID { get; set; }

        public Derivative Derivative { get; set; }

        [ConcurrencyCheck]
        public int? TestPCID { get; set; }

        public TestPc TestPc { get; set; }

        [ConcurrencyCheck]
        public string HWAssetNo { get; set; }

        [ConcurrencyCheck]
        public string HWevaluationBoard { get; set; }

        [ConcurrencyCheck]
        public string MCU { get; set; }

        [ConcurrencyCheck]
        public int? PersonID { get; set; }
        
        public Person Person { get; set; }

        [ConcurrencyCheck]
        public string StartWeek { get; set; }

        [ConcurrencyCheck]
        public string EndWeek { get; set; }

        [ConcurrencyCheck]
        public DateTime? StartDate { get; set; }

        [ConcurrencyCheck]
        public DateTime? EndDate { get; set; }

    }
}