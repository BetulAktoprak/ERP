using Ardalis.SmartEnum;

namespace ERP.Server.Domain.Enums;

public sealed class StockMovementTypeEnum : SmartEnum<StockMovementTypeEnum>
{
    public static StockMovementTypeEnum AlisFaturasi = new("Alış Faturası", 1);
    public static StockMovementTypeEnum SatisFaturasi = new("Satış Faturası", 2);
    public static StockMovementTypeEnum Uretim = new("Üretim", 3);
    public static StockMovementTypeEnum IadeFaturasi = new("İade Faturası", 4);
    public static StockMovementTypeEnum Devir = new("Devir", 5);
    public StockMovementTypeEnum(string name, int value) : base(name, value)
    {
    }
}
