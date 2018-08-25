using Helper.Utility;

namespace Model
{
    public class PaginateIn
    {
        public PaginateIn()
        {
            this.qtdEntries = int.Parse(Ready.AppSettings["Pagination.qtdEntries"]);
            this.qtdActionNumber = int.Parse(Ready.AppSettings["Pagination.qtdActionNumber"]);
            this.amount = null;
            this.currentPage = 1;
            this.sort = "CreatedDate";
            this.sortdirection = "asc";
            this.filter = "";
        }

        public int? qtdEntries { get; set; }
        public int? qtdActionNumber { get; set; }
        public int? amount { get; set; }
        public int? currentPage { get; set; }
        public string sort { get; set; }
        public string sortdirection { get; set; }
        public string filter { get; set; }
    }
}
