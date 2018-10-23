using System;

namespace Model
{
    public class BaseIn : PaginateIn
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
