using System.ComponentModel;

namespace Helper.Enum
{
    public enum EDocumentStatus
    {
        [Description("Novo")]
        New = 1,
        [Description("Parcialmente Recortado")]
        PartiallySlice = 2,
        [Description("Recortado")]
        Slice = 3,
        [Description("Parcialmente Classificado")]
        PartiallyClassificated = 4,
        [Description("Classificado")]
        Classificated = 5,
        [Description("Finalizado")]
        Finished = 6,
        [Description("Enviado")]
        Sent = 7,
        [Description("Cancelado")]
        Canceled = 8,
    }
}
