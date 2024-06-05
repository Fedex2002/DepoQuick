using Logic.DTOs;

namespace Logic.Interfaces;

public interface IDateRangeController
{
    public void CheckIfDateStartAndDateEndAreIncludedInDateRange(DateTime dateStart, DateTime dateEnd,
        DateRangeDto dateRangeDto);
}