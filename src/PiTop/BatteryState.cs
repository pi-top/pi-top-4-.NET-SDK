using System;
using System.Linq;

namespace PiTop
{
    
    public class BatteryState
    {
        public static BatteryState FromMessage(PiTopMessage message)
        {
            var args = message.Parameters.ToArray();

            var chargingState = (BatteryChargingState)int.Parse(args[0]);
            var capacity = double.Parse(args[1]);
            var timeRemaining = TimeSpan.FromMinutes(int.Parse(args[2]));
            var wattage = double.Parse(args[3]);
            var state = new BatteryState(chargingState, capacity, timeRemaining, wattage);
            return state
        }

        public BatteryState(BatteryChargingState chargingState, double capacity, TimeSpan timeRemaining, double wattage)
        {
            ChargingState = chargingState;
            Capacity = capacity;
            TimeRemaining = timeRemaining;
            Wattage = wattage;
        }

        public static BatteryState Empty => new BatteryState(BatteryChargingState.Undefined, double.NaN, TimeSpan.MinValue, double.NaN);

        public BatteryChargingState ChargingState { get; }
        public double Capacity { get; }
        public TimeSpan TimeRemaining { get; }
        public double Wattage { get; }
    }
}