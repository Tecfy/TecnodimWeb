using Model.VM;

namespace Model.Out
{
    public class ClassificationOut : ResultServiceVM
    {
        public ClassificationOut()
        {
            this.result = new ClassificationVM();
        }

        public ClassificationVM result { get; set; }
    }
}
