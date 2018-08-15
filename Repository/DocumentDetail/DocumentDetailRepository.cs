using ApiTecnodim;
using Model.In;
using Model.Out;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentDetailApi documentDetailApi = new DocumentDetailApi();

        public DocumentDetailOut GetDocumentDetail(DocumentDetailIn documentDetailIn)
        {
            DocumentDetailOut documentDetailOut = new DocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId.Value, documentDetailIn.key.Value, "Log - Start", "Repository.DocumentDetailRepository.GetDocumentDetail", "");

            documentDetailOut = documentDetailApi.GetDocumentDetail(documentDetailIn);

            registerEventRepository.SaveRegisterEvent(documentDetailIn.userId.Value, documentDetailIn.key.Value, "Log - End", "Repository.DocumentDetailRepository.GetDocumentDetail", "");
            return documentDetailOut;
        }
    }
}
