using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Tecnodim.Models
{
    /// <summary>
    /// Classe padrão de retorno da Api
    /// </summary>
    [Serializable, DataContract]
    public class Return<T>
    {
        #region .: Constructors :.

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Return()
        {
            _Key = Guid.NewGuid();
        }

        /// <summary>
        /// Construtor padrão Log
        /// </summary>
        /// <param name="request"></param>
        public Return(HttpRequestMessage request)
        {
            if (request.Headers.Contains("LogKey") || request.Headers.Contains("logkey"))
            {
                try
                {
                    string LogKey = request.Headers.GetValues("LogKey").FirstOrDefault();
                    _Key = Guid.Parse(LogKey);
                }
                catch
                {
                    _Key = Guid.NewGuid();
                }
            }
            else
            {
                _Key = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Construtor padrão sucesso
        /// </summary>
        /// <param name="message">Mensagem do retorno</param>
        /// <param name="success">Booleano utilizado para identificar se é um retorno de sucesso ou falha.</param>
        public Return(T Obj, Guid key, Boolean success)
        {
            _Key = key;
            _Success = success;
            _Obj = Obj;
        }

        /// <summary>
        /// Construtor padrão error
        /// </summary>
        /// <param name="key">Chave unica de identificação, pode ser utilizada para geração de logs</param>
        /// <param name="exception">Classe do tipo Exception, utilizada para retorno do catch ou para erro customizados.</param>
        /// <param name="success">Booleano utilizado para identificar se é um retorno de sucesso ou falha.</param>
        /// <param name="Api">Nome da Api.</param>
        /// <param name="ApiArea">Nome da Área da Api.</param>
        /// <param name="ApiController">Nome do Controller da Api.</param>
        /// <param name="ApiControllerMethod">Nome do Método do Controller da Api.</param>
        /// <param name="ApiControllerMethodParams">Pâmetros passados no Método do Controller da Api.</param>
        public Return(Exception exception, Guid key, string Api, string ApiArea, string ApiController, string ApiControllerMethod, string ApiControllerMethodParams)
        {
            this._Key = key;
            this._Success = false;
            this.Api = Api;
            this.ApiArea = ApiArea;
            this.ApiController = ApiController;
            this.ApiControllerMethod = ApiControllerMethod;
            this.ApiControllerMethodParams = ApiControllerMethodParams;
            this.Exception = exception;
        }

        #endregion

        #region .: Properties :.

        /// <summary>
        /// Chave unica de identificação, pode ser utilizada para geração de logs
        /// </summary>
        [DataMember]
        public Guid Key { get { return _Key; } }
        private Guid _Key;

        /// <summary>
        /// Classe do tipo Exception, utilizada para retorno do catch ou para erro customizados.
        /// </summary>        
        [DataMember]
        private Exception Exception
        {
            set
            {
                setMessageErros(value);
            }
        }

        /// <summary>
        /// Booleano utilizado para identificar se é um retorno de sucesso ou falha.
        /// </summary>
        [DataMember]
        public bool Success { get { return _Success; } }
        private bool _Success;

        /// <summary>
        /// Objeto de Retorno
        /// </summary>
        [DataMember]
        public T ObjReturn { get { return _Obj; } }
        private T _Obj;

        /// <summary>
        /// Retorna Mensagem de erro
        /// </summary>
        [DataMember]
        private List<string> MessageErros { get; set; }

        /// <summary>
        /// Retorna codigo de erro padronizado
        /// </summary>
        [DataMember]
        public string Code { get { return _Code; } }
        private string _Code;

        private string Api { get; set; }
        private string ApiArea { get; set; }
        private string ApiController { get; set; }
        private string ApiControllerMethod { get; set; }
        private string ApiControllerMethodParams { get; set; }

        #endregion

        #region .: Methods :.

        private void setMessageErros(Exception ex)
        {
            if (ex != null)
            {
                MessageErros = new List<string>();
                getInnerException(ex);
            }
        }

        private void getInnerException(Exception ex)
        {
            if (ex is System.Web.HttpException)
            {
                _Code = ((System.Web.HttpException)ex).GetHttpCode().ToString();
            }
            MessageErros.Add(ex.Message);
            if (ex.InnerException != null)
            {
                getInnerException(ex.InnerException);
            }
        }

        #endregion
    }
}