using CtrlInvest.Domain.Identity;
using System;

namespace CtrlInvest.Domain.Entities.Aggregates
{
    public class UserTokenHistory
    {
        public Guid UserTokenHistoryID { get; set; }
        public Guid JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public Guid UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }

        public ApplicationUser User { get; set; }

        public UserTokenHistory()
        {
            UserTokenHistoryID = Guid.NewGuid();
            AddedDate = DateTime.Now;
            IsRevorked = false;
            IsUsed = false;
        }

        public void SetUserToken(Guid userId, Guid jwtId, DateTime expiration, string token)
        {
            UserId = userId;
            JwtId = jwtId;
            ExpireDate = expiration;
            Token = token;
        }

        public void Update()
        {
            IsRevorked = true;
            IsUsed = true;           
        }
    }
}
