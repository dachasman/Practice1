namespace VehicleListApp
{
    public class ListNode
    {
        public VehicleRecord Value { get; set; }
        public ListNode Next { get; set; }
        public ListNode Prev { get; set; }

        public ListNode(VehicleRecord value)
        {
            Value = value;
            Next = null;
            Prev = null;
        }
    }
}