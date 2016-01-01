using System.Collections.Generic;

namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithStringArray
    {
        public SettingsWithStringArray(IEnumerable<string> stringArrayValues)
        {
            StringArrayValues = stringArrayValues;
        }

        public IEnumerable<string> StringArrayValues { get; private set; }
    }
}