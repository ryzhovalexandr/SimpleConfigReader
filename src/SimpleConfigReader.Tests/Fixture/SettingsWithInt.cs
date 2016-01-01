namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithInt
    {
        public SettingsWithInt(int intValue)
        {
            IntValue = intValue;
        }

        public int IntValue { get; private set; }
    }
}