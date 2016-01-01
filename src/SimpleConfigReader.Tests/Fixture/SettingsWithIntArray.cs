using System.Collections.Generic;

namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithIntArray
    {
        public SettingsWithIntArray(IEnumerable<int> intArrayValues)
        {
            IntArrayValues = intArrayValues;
        }

        public IEnumerable<int> IntArrayValues { get; private set; }
    }
}