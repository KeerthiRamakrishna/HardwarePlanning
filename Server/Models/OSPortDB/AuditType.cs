using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Osporting.Server.Models.OSPortDB
{
    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}