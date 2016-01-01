namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithBool
    {
        public SettingsWithBool(bool? boolValue)
        {
            BoolValue = boolValue;
        }

        public bool? BoolValue { get; private set; }
    }
}