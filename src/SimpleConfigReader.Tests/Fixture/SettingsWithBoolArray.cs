using System.Collections.Generic;

namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithBoolArray
    {
        public SettingsWithBoolArray(IEnumerable<bool> boolArrayValue)
        {
            BoolArrayValue = boolArrayValue;
        }

        public IEnumerable<bool> BoolArrayValue { get; private set; }
    }
}