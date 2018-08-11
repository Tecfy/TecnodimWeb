using System;

namespace Model
{
    public class baseIn
    {
        public baseIn()
        {
            this.userId = null;
            this.key = null;
        }

        public Guid? userId { get; set; }

        public Guid? key { get; set; }

    }
}
