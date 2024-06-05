using Logic.DTOs;

namespace Logic.Interfaces;

public interface IDateRangeController
{
    public void CheckIfDateStartAndDateEndAreIncludedInDateRange(DateTime dateStart, DateTime dateEnd, DateRangeDto dateRangeDto);
    public void ModifyOrRemoveDateRange(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto);
    public void EliminateDateRangeFromStorageUnit(string id, DateRangeDto dateRangeDto);
}