using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IStorageUnitController
{
    public void CreateStorageUnit(StorageUnitDto storageUnitDto);
    public void RemoveStorageUnit(StorageUnitDto storageUnitDto);
    public List<StorageUnitDto> GetStorageUnitsDto();
    public StorageUnitDto GetStorageUnitDtoFromId(string id);
    public void DeletePromotionFromAllStorageUnits(PromotionDto promotionDto);
    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto);
    public void AddAvailableDateRangeToStorageUnit(string id, DateRangeDto dateRangeDto);
    public List<StorageUnitDto> SearchAvailableStorageUnits(DateRangeDto dateRangeDto);
    public void EliminateDateRangeFromStorageUnit(string id, DateRangeDto dateRangeDto);
}