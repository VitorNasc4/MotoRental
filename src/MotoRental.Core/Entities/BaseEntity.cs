using System;
using System.Collections.Generic;
using System.Text;

namespace MotoRental.Core.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity() { }
        public int Id { get; private set; }
    }
}
