﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domain.Intefaces
{
    public interface IEntityBase<T>
    {
        [Key]
        T Id { get; set; }
    }
}
