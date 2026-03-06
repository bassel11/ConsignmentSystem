using ConsignmentSystem.Domain.Common;

namespace ConsignmentSystem.Domain.Entities
{
    public class Vendor : AuditableEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string ContactEmail { get; private set; } = default!;

        private readonly List<Vehicle> _vehicles = new();
        public decimal DefaultCommissionPercentage { get; private set; } = 0m;
        public IReadOnlyCollection<Vehicle> Vehicles => _vehicles.AsReadOnly();
        protected Vendor() { }

        public Vendor(string name, string contactEmail, string createdBy, decimal defaultCommissionPercentage)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vendor name cannot be empty.");

            Id = Guid.NewGuid();
            Name = name;
            ContactEmail = contactEmail;
            CreatedBy = createdBy;
            DefaultCommissionPercentage = defaultCommissionPercentage;
        }

        public void UpdateContactInfo(string newEmail, string updatedBy)
        {
            ContactEmail = newEmail;
            UpdateAudit(updatedBy);
        }

        public void UpdateDetails(string name, string contactEmail, decimal defaultCommissionPercentage, string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Vendor name cannot be empty.");

            Name = name;
            ContactEmail = contactEmail;
            DefaultCommissionPercentage = defaultCommissionPercentage;
            UpdateAudit(updatedBy);
        }

    }
}
