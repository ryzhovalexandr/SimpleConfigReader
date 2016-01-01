namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithDouble
    {
        public SettingsWithDouble(double doubleValue)
        {
            DoubleValue = doubleValue;
        }

        public double DoubleValue { get; private set; }
    }
}