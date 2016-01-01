namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithOptionalBool
    {
        public const bool DefaultValue = true;

        public SettingsWithOptionalBool(bool? boolValue)
        {
            BoolValue = boolValue ?? DefaultValue;
        }

        public bool BoolValue { get; private set; }
    }
}