using System.Collections.Generic;

namespace Model
{
    public class ResultServiceVM
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResultServiceVM()
        {
            messages = new List<string>();
            successMessage = null;
        }

        /// <summary>
        /// Operation Result Validation
        /// </summary>
        public bool success
        {
            get
            {
                return messages.Count == 0;
            }
        }
      
        /// <summary>
        /// Error Messages
        /// </summary>
        public List<string> messages { get; set; }

        /// <summary>
        /// Success Message
        /// </summary>
        public string successMessage { get; set; }
    }
}
