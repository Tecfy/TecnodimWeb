using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class resultServiceVM
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public resultServiceVM()
        {
            this.messages = new List<string>();
            this.successMessage = null;
        }

        /// <summary>
        /// Operation Result Validation
        /// </summary>
        public bool success
        {
            get
            {
                return this.messages.Count == 0;
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
