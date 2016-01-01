namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithOptionalDouble
    {
        public const double DefaultValue = -123.12;

        public SettingsWithOptionalDouble(double? doubleValue)
        {
            DoubleValue = doubleValue ?? DefaultValue;
        }

        public double DoubleValue { get; private set; }
    }
}