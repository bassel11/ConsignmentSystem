using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Success, string Token, string ErrorMessage)> LoginAsync(string email, string password);
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(string fullName, string email, string password, string role);
    }
}
