using System.Collections.Generic;

namespace SimpleConfigReader.Tests.Fixture
{
    internal class SettingsWithDoubleArray
    {
        public SettingsWithDoubleArray(IEnumerable<double> doubleArrayValue)
        {
            DoubleArrayValue = doubleArrayValue;
        }

        public IEnumerable<double> DoubleArrayValue { get; private set; }
    }
}