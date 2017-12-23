using System;

namespace GoldenSeries.Dros.Models
{
    public interface ITrackable
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset UpdatedOn { get; set; }
    }
}