using ApiTecnodim;
using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Linq;

namespace Repository
{
    public partial class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentApi documentApi = new DocumentApi();

        #region .: Methods :.

        public DocumentsOut GetDocuments(DocumentsIn documentsIn)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetDocuments", "");

            documentsOut = documentApi.GetDocuments(documentsIn);

            registerEventRepository.SaveRegisterEvent(documentsIn.userId.Value, documentsIn.key.Value, "Log - End", "Repository.DocumentRepository.GetDocuments", "");
            return documentsOut;
        }

        #endregion
    }
}
