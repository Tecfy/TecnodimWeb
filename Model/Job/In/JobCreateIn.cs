namespace Model.In
{
    public class JobCreateIn : BaseIn
    {
        public int userId { get; set; }

        public int jobStatusId { get; set; }

        public string registration { get; set; }

        public string name { get; set; }

        public string course { get; set; }

        public int unityId { get; set; }
    }
}
