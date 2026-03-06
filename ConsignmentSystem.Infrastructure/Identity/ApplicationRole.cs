using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
