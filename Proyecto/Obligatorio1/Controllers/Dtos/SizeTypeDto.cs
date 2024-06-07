using Model.Enums;

namespace Logic.DTOs
{
    public class SizeTypeDto
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public SizeTypeDto(SizeType sizeType)
        {
            Value = (int)sizeType;
            Name = sizeType.ToString();
        }
    }
}