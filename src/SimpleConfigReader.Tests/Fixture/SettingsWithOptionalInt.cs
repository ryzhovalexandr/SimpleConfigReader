namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithOptionalInt
    {
        public const int DefaultValue = 1234;

        public SettingsWithOptionalInt(int? intValue)
        {
            IntValue = intValue ?? DefaultValue;
        }

        public int IntValue { get; private set; }
    }
}