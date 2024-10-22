using System.ComponentModel;

namespace Domain.Enumerables;

public enum TruckModel
{
    [Description("Modelo de Caminhão FH")]
    FH = 1,

    [Description("Modelo de Caminhão FM")]
    FM = 2,

    [Description("Modelo de Caminhão VM")]
    VM = 3,
}
