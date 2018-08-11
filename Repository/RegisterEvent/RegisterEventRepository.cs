using DataEF.DataAccess;
using System;

namespace Repository.RegisterEvent
{
    public class RegisterEventRepository
    {
        #region .: Methods :.

        public void SaveRegisterEvent(Guid userId, Guid identifier, string type, string source, string text)
        {
            using (var context = new DBContext())
            {
                RegisterEvents registerEvent = new RegisterEvents() { UserId = userId, Identifier = identifier, Type = type, Source = source, Text = text };
                context.RegisterEvents.Add(registerEvent);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
