using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Common.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";               // كل الصلاحيات
        public const string Storekeeper = "Storekeeper";   // ادارة المركبات واستلام البضائه
        public const string Accountant = "Accountant";     // ادارة المبيعات وتوليد الفواتير
    }
}
