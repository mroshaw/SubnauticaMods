namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    public enum RecallMoveMethod
    {
        Instant,
        Teleport,
        Smooth,
        Fixed
    };

    public static class ConfigFile
    {
        public static RecallMoveMethod RecallMoveMethod =  RecallMoveMethod.Instant;
        public static float MaximumRange = 100f;
    }
}