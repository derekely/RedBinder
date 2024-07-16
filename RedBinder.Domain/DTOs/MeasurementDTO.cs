using RedBinder.Domain.Entities;

namespace RedBinder.Domain.DTOs;

public record MeasurementDto(int Id, string Name, double Quantity)
{
    public MeasurementDto(Measurement measurement) : this(measurement.Id, measurement.Name, measurement.Quantity)
    {
    }
}