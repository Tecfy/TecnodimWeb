using System;

namespace Model
{
    public class BaseIn
    {
        public BaseIn()
        {
            this.userId = null;
            this.key = null;
        }

        public string userId { get; set; }

        public string key { get; set; }

    }
}
