namespace VehicleListApp
{
    public class VehicleRecord
    {
        public VehicleType Type { get; set; }
        public double EngineCapacity { get; set; }
        public bool IsElectricOrHybrid { get; set; }

        public VehicleRecord() { } 

        public VehicleRecord(VehicleType type, double engineCapacity, bool isElectricOrHybrid)
        {
            Type = type;
            EngineCapacity = engineCapacity;
            IsElectricOrHybrid = isElectricOrHybrid;
        }

        public override string ToString()
        {
            return $"{Type,-12} | {EngineCapacity,14:F1} L | {(IsElectricOrHybrid ? "Yes" : "No"),-10}";
        }
    }
}