namespace MBRS_API.Models.ViewModels
{
    public class NumberRoomStatusViewModel
    {
        public int numberOfRoomCheckIn { get; set; }
        public int numberOfRoomHaveOrder { get; set; }
        public int numberOfRoomEmpty { get; set; }

        public int numberTotalRoom
        {
            get
            {
                return (numberOfRoomCheckIn + numberOfRoomHaveOrder + numberOfRoomEmpty);
            }
        }
        public NumberRoomStatusViewModel(int numberOfRoomCheckIn, int numberOfRoomHaveOrder, int numberOfRoomEmpty)
        {
            this.numberOfRoomCheckIn = numberOfRoomCheckIn;
            this.numberOfRoomHaveOrder = numberOfRoomHaveOrder;
            this.numberOfRoomEmpty = numberOfRoomEmpty;
        }
    }
}
