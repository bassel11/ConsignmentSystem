using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public string? CreatedBy { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;

        public void MarkAsDeleted(string deletedBy)
        {
            if (IsDeleted) return;

            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = deletedBy;
        }

        public void UpdateAudit(string updatedBy)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }
    }
}
