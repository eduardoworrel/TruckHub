using System.ComponentModel;

namespace Domain.Enumerables;

public enum TruckModel
{
    [Description("Caminhão FH")]
    FH = 1,

    [Description("Caminhão FM")]
    FM = 2,

    [Description("Caminhão VM")]
    VM = 3,
}
