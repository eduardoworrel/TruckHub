using System.ComponentModel;

namespace Domain.Enumerables;

public enum PlantLocation
{
    [Description("Brasil")]
    BR = 1,

    [Description("Suécia")]
    SE = 2,

    [Description("Estados Unidos")]
    US = 3,

    [Description("França")]
    FR = 4
}