using DataEF.DataAccess;
using Model.In;
using Model.Out;

namespace Repository
{
    public partial class RatingRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        #region .: Methods :.

        public ClippingOut SaveRating(RatingIn ratingIn)
        {
            ClippingOut clippingOut = new ClippingOut();

            registerEventRepository.SaveRegisterEvent(ratingIn.userId.Value, ratingIn.key.Value, "Log - Start", "Repository.RatingRepository.SaveRatings", "");

            using (var db = new DBContext())
            {

            }

            registerEventRepository.SaveRegisterEvent(ratingIn.userId.Value, ratingIn.key.Value, "Log - End", "Repository.RatingRepository.SaveRatings", "");
            return clippingOut;
        }

        #endregion
    }
}
