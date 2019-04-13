using Model.VM;

namespace Model.Out
{
    public class ClassificationOut : ResultServiceVM
    {
        public ClassificationOut()
        {
            result = new ClassificationVM();
        }

        public ClassificationVM result { get; set; }
    }
}
