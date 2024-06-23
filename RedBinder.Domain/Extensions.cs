using System;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Domain;

public static class Extensions
{
    public static Result<Measurement> AddSameMeasurement(this Measurement measurement1, Measurement measurement2) =>
        Result.SuccessIf(string.Equals(measurement1.Name, measurement2.Name, StringComparison.CurrentCultureIgnoreCase), "Measurements must be the same")
            .Bind(() => Measurement.Create(measurement1.Name, measurement1.Quantity + measurement2.Quantity));
}