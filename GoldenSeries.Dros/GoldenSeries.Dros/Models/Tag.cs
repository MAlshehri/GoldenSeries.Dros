using System;
using System.Collections.Generic;

namespace GoldenSeries.Dros.Models
{
    public class Tag : ITrackable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MaterialTag> Materials { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
