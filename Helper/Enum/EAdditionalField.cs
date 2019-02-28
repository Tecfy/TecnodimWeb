using System.ComponentModel;

namespace Helper.Enum
{
    public enum EAdditionalField
    {
        [Description("Identificador")]
        Identifier = 1,
        [Description("Competência")]
        Competence = 2,
        [Description("Validade")]
        Validity = 3,
        [Description("Visualização do documento")]
        DocumentView = 4,
        [Description("Observação")]
        Note = 5,
    }
}
