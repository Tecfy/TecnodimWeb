using Helper.Utility;

namespace Model
{
    public class PaginateIn
    {
        public PaginateIn()
        {
            qtdEntries = int.Parse(Ready.AppSettings["Pagination.qtdEntries"]);
            qtdActionNumber = int.Parse(Ready.AppSettings["Pagination.qtdActionNumber"]);
            amount = null;
            currentPage = 1;
            sort = "CreatedDate";
            sortdirection = "asc";
            filter = "";
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
