using System.ComponentModel;

namespace API_Rest_ASP_Core.Models.Enums
{
    public enum AvailableEnum : int
    {
        [Description("Disponible")]
        True = 1,
        [Description("No disponible")]
        False = 0
    }
}
