using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IStorageUnitController
{
    public StorageUnit CreateStorageUnit(StorageUnitDto storageUnitDto);
    public void AddStorageUnit(StorageUnit storageUnit);
    public void DeleteStorageUnit(StorageUnitDto storageUnitDto);
    public List<StorageUnitDto> GetStorageUnitsDto();
    public StorageUnitDto GetStorageUnitDtoFromId(string id);
    public void DeletePromotionFromAllStorageUnits(PromotionDto promotionDto);
    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto);
}