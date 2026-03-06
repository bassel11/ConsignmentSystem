using ConsignmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface IConsignmentItemRepository
    {
        Task<ConsignmentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
