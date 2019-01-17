using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobStatusIn : BaseIn
    {
        [Required]
        [Display(Name = "Job", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobId { get; set; }

        [Required]
        [Display(Name = "JobStatus", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobStatusId { get; set; }
    }
}