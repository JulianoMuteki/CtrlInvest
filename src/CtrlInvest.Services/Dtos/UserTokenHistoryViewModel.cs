using System;

namespace CtrlInvest.Services.Dtos
{
    public class UserTokenHistoryViewModel
    {
        public Guid JwtId { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
