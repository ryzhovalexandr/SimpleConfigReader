using System.Configuration;
using System.Linq;
using NUnit.Framework;
using SimpleConfigReader.Tests.Fixture;

namespace SimpleConfigReader.Tests
{
    public static class AppSettingsReader
    {
        public static int? ReadInt(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            int result;
            return (value != null && int.TryParse(value, out result))
                ? result
                : (int?)null;
        }

        public static string ReadString(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        public static bool? ReadBool(string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            switch (value)
            {
                case "1":
                    return true;

                case "0":
                    return false;

                default:
                    bool result;
                    return (value != null && bool.TryParse(value, out result))
                        ? result
                        : (bool?) null;
            }
        }
    }

    [TestFixture]
    internal class ConfigurationReaderTests
    {
        private void Test()
        {
            bool useSomeFunction = AppSettingsReader.ReadBool("UseSomeFunction") ?? false;
            string serverAddress = AppSettingsReader.ReadString("ServerAddress");

            int timeoutInSeconds = AppSettingsReader.ReadInt("TimeoutInSeconds") ?? 1000;
        }

        [Test]
        public void ReadStringValueFromConfig()
        {
            var settings = ConfigurationReader<SettingsWithString>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "StringValue", "stringValue" } });

            Assert.AreEqual("stringValue", settings.StringValue);
        }

        [Test]
        public void ReadStringArrayFromConfig()
        {
            var settings = ConfigurationReader<SettingsWithStringArray>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "StringArrayValues", "ss34dd;30;40;dfdfa" } });

            var stringArray = settings.StringArrayValues.ToArray();
            Assert.AreEqual("ss34dd", stringArray[0]);
            Assert.AreEqual("30", stringArray[1]);
            Assert.AreEqual("40", stringArray[2]);
            Assert.AreEqual("dfdfa", stringArray[3]);
        }

        [Test]
        public void ReadIntValue()
        {
            var settings = ConfigurationReader<SettingsWithInt>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "IntValue", "34" } });

            Assert.AreEqual(34, settings.IntValue);
        }

        [Test]
        public void ReadIntArrayValues()
        {
            var settings = ConfigurationReader<SettingsWithIntArray>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "IntArrayValues", "34;30;40;aa;f444" } });

            var intArray = settings.IntArrayValues.ToArray();

            Assert.AreEqual(intArray.Length, 5);

            Assert.AreEqual(34, intArray[0]);
            Assert.AreEqual(30, intArray[1]);
            Assert.AreEqual(40, intArray[2]);
            Assert.AreEqual(0, intArray[3]);
            Assert.AreEqual(0, intArray[4]);
        }

        [Test]
        public void ReadDefaultIntValue()
        {
            var settings = ConfigurationReader<SettingsWithInt>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(0, settings.IntValue);
        }

        [Test]
        public void ReadDefaultIntValueFromConstructor()
        {
            var settings = ConfigurationReader<SettingsWithOptionalInt>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(SettingsWithOptionalInt.DefaultValue, settings.IntValue);
        }


        [Test]
        public void ReadBoolArrayValues()
        {
            var settings = ConfigurationReader<SettingsWithBoolArray>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "BoolArrayValue", "true;1;0;false;fff;truefdf" } });

            var boolArray = settings.BoolArrayValue.ToArray();

            Assert.AreEqual(boolArray.Length, 6);

            Assert.AreEqual(true, boolArray[0]);
            Assert.AreEqual(true, boolArray[1]);
            Assert.AreEqual(false, boolArray[2]);
            Assert.AreEqual(false, boolArray[3]);
            Assert.AreEqual(false, boolArray[4]);
            Assert.AreEqual(false, boolArray[5]);

        }

        [Test]
        public void ReadBoolValue()
        {
            var settings = ConfigurationReader<SettingsWithBool>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "BoolValue", "true" } });

            Assert.AreEqual(true, settings.BoolValue);
        }

        [Test]
        public void ReadOneAsTrueBoolValue()
        {
            var settings = ConfigurationReader<SettingsWithBool>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "BoolValue", "1" } });

            Assert.AreEqual(true, settings.BoolValue);
        }

        [Test]
        public void ReadZeroAsFalseBoolValue()
        {
            var settings = ConfigurationReader<SettingsWithBool>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "BoolValue", "0" } });

            Assert.AreEqual(false, settings.BoolValue);
        }

        [Test]
        public void ReadDefaultBoolValue()
        {
            var settings = ConfigurationReader<SettingsWithBool>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(null, settings.BoolValue);
        }

        [Test]
        public void ReadDefaultBoolValueFromConstructor()
        {
            var settings = ConfigurationReader<SettingsWithOptionalBool>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(SettingsWithOptionalBool.DefaultValue, settings.BoolValue);
        }

        [Test]
        public void ReadDoubleValue()
        {
            var settings = ConfigurationReader<SettingsWithDouble>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "DoubleValue", "10.3" } });

            Assert.AreEqual(10.3, settings.DoubleValue, 0.001);
        }


        [Test]
        public void ReadDoubleArrayValues()
        {
            var settings = ConfigurationReader<SettingsWithDoubleArray>.ReadFromCollection(
                new KeyValueConfigurationCollection { { "DoubleArrayValue", "1.0;1.5;1;01.98;-5.7;fdf;a44" } });

            var doubleArray = settings.DoubleArrayValue.ToArray();

            Assert.AreEqual(doubleArray.Length, 7);

            Assert.AreEqual(1d, doubleArray[0]);
            Assert.AreEqual(1.5, doubleArray[1]);
            Assert.AreEqual(1, doubleArray[2]);
            Assert.AreEqual(1.98, doubleArray[3]);
            Assert.AreEqual(-5.7, doubleArray[4]);
            Assert.AreEqual(default(double), doubleArray[5]);
            Assert.AreEqual(default(double), doubleArray[6]);
        }


        [Test]
        public void ReadDefaultDoubleValue()
        {
            var settings = ConfigurationReader<SettingsWithDouble>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(0.0, settings.DoubleValue, 0.001);
        }

        [Test]
        public void ReadDefaultDoubleValueFromConstructor()
        {
            var settings = ConfigurationReader<SettingsWithOptionalDouble>.ReadFromCollection(new KeyValueConfigurationCollection());

            Assert.AreEqual(SettingsWithOptionalDouble.DefaultValue, settings.DoubleValue);
        }

        [Test]
        public void ReadIntoStruct()
        {
            var settings = ConfigurationReader<SettingsStruct>.ReadFromCollection(new KeyValueConfigurationCollection());
            Assert.NotNull(settings);
        }

        [Test]
        public void ReadIntoNestedClass()
        {
            var settings = ConfigurationReader<NestedSettings>.ReadFromCollection(new KeyValueConfigurationCollection());
            Assert.NotNull(settings);
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private class NestedSettings // ReSharper restore ClassNeverInstantiated.Local
        {
            // ReSharper disable UnusedParameter.Local
            public NestedSettings(string fakeValue) // ReSharper restore UnusedParameter.Local
            {
            }
        }
    }
}