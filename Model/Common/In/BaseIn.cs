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

        public Guid? userId { get; set; }

        public Guid? key { get; set; }

    }
}
