using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobsWebRequestIn : BaseIn
    {
        public int registration { get; set; }
                
        public List<JobPageInfoIn> jobCategoryList { get; set; } //lista com informações das categorias a  serem inseridas

    }
}
