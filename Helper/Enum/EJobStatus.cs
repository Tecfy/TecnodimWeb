using System.ComponentModel;

namespace Helper.Enum
{
    public enum EJobStatus
    {
        [Description("Novo")]
        New = 1,
        [Description("Parcialmente Digitalizado")]
        PartiallyDigitalized = 2,
        [Description("Digitalizado")]
        Digitalized = 3,
        [Description("Finalizado")]
        Finished = 4,
        [Description("Enviado")]
        Sent = 5
    }
}
