using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    [Table("AuditTrail", Schema = "public")]
    public partial class AuditTrail
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        //public int Id { get; set; }
        //public string TableName { get; set; }
        //public string AffectedColumn { get; set; }
        //public string OldValue { get; set; }
        //public string NewValue { get; set; }
        //public string ActionType { get; set; } // Create, Update, Delete
        //public string UserId { get; set; }
        //public DateTime Timestamp { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditTrailId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}