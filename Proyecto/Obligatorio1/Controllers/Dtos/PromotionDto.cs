namespace Logic.DTOs;

public class PromotionDto
{
    public string Label { get; set; }
    public int Discount { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
        
    public PromotionDto()
    {
            
    }

    public PromotionDto(string label, int discount, DateTime dateStart, DateTime dateEnd)
    {
        Label = label;
        Discount = discount;
        DateStart = dateStart;
        DateEnd = dateEnd;
    }
}