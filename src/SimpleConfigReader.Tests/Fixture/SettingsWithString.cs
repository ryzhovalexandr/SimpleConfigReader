namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithString
    {
        public SettingsWithString(string stringValue)
        {
            StringValue = stringValue;
        }

        public string StringValue { get; private set; }
    }
}