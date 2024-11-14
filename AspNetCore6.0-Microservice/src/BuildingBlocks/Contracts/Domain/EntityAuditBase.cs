using Contracts.Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domain
{
    public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LasrModifiedDate { get; set; }
    }
}
