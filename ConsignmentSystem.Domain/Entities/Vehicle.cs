using ConsignmentSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Entities
{
    public class Vehicle : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid VendorId { get; private set; } = default!;
        public string LicensePlate { get; private set; } = default!;
        public string DriverName { get; private set; } = default!;

        protected Vehicle() { }

        public Vehicle(Guid vendorId, string licensePlate, string driverName, string createdBy)
        {
            Id = Guid.NewGuid();
            VendorId = vendorId;
            LicensePlate = licensePlate;
            DriverName = driverName;
            CreatedBy = createdBy;
        }
    }
}
