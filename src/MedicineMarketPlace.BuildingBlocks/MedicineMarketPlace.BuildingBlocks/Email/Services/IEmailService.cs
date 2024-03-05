using MedicineMarketPlace.BuildingBlocks.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineMarketPlace.BuildingBlocks.Email.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailData emailData);
    }
}
