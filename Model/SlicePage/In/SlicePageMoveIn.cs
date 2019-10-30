namespace Model.In
{
    public class SlicePageMoveIn : BaseIn
    {
        public int sliceNewId { get; set; }

        public int sliceOldId { get; set; }

        public int page { get; set; }
    }
}
