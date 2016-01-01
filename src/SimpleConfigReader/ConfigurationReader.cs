using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SimpleConfigReader
{
    /// <summary>
    /// Считыватель настроек key-value в иммутабельный объект.
    /// </summary>
    /// <typeparam name="T">Класс настроек.</typeparam>
    public class ConfigurationReader<T>
    {
        /// <summary>
        /// Символы разделения для массива в строковом представлении.
        /// </summary>
        private const char ItemsDelimeter = ';';

        private readonly KeyValueConfigurationCollection _keyValueCollection;

        /// <summary>
        /// Создание экземпляра класса <see cref="ConfigurationReader{T}"/>.
        /// </summary>
        /// <param name="keyValueCollection">Коллекция прочитанных настроек.</param>
        public ConfigurationReader(KeyValueConfigurationCollection keyValueCollection)
        {
            if (keyValueCollection == null)
            {
                throw new ArgumentNullException(nameof(keyValueCollection));
            }

            _keyValueCollection = keyValueCollection;
        }

        /// <summary>
        /// Создание класса настроек с инициализацией считанных параметров в конструктор.
        /// </summary>
        /// <param name="keyValueCollection">Коллекция прочитанных настроек.</param>
        /// <returns>Объект класса настроек с прочитанными параметрами из конфига.</returns>
        /// <typeparam name="T">Класс настроек.</typeparam>
        public static T ReadFromCollection(KeyValueConfigurationCollection keyValueCollection)
        {
            var configurationReader = new ConfigurationReader<T>(keyValueCollection);
            return configurationReader.ReadFromCollection();
        }

        /// <summary>
        /// Создание класса настроек с инициализацией считанных параметров в конструктор.
        /// </summary>
        /// <returns>Объект класса настроек с прочитанными параметрами из конфига.</returns>
        private T ReadFromCollection()
        {
            var constructorInfo = GetConstructorInfo();
            var args = constructorInfo.GetParameters().Select(GetParameterValue).ToArray();

            return (T)constructorInfo.Invoke(args);
        }

        private static ConstructorInfo GetConstructorInfo()
        {
            var constructors = typeof(T).GetConstructors();

            if (constructors.Length != 1)
            {
                throw new ArgumentException(
                    $"В классе настроек {typeof(T).Name} должен быть единственный конструктор (найдено {constructors.Length}), принимающий значения всех настроек в виде параметров.");
            }

            return constructors.First();
        }

        /// <summary>
        /// Преобразуем из дробного строкового значения в double.
        /// Если парсинг не удался, то возвращаем default(double) == 0.
        /// </summary>
        /// <param name="parameterValue">Дробное значение в виде строки.</param>
        /// <returns>Полученный результат парсинга.</returns>
        private static double ConvertDouble(string parameterValue)
        {
            double value;
            return double.TryParse(parameterValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                       ? value
                       : default(double);
        }

        /// <summary>
        /// Преобразуем из булевского строкового значения (true, false, 1, 0) в bool.
        /// Если парсинг не удался, то возвращаем false.
        /// </summary>
        /// <param name="parameterValue">Булевское строкового значение(true, false, 1, 0).</param>
        /// <returns>Полученный результат парсинга.</returns>
        private static bool ConvertBool(string parameterValue)
        {
            // оставляем возможность читать настройки, заданные цифрами
            if (parameterValue == "1")
            {
                return true;
            }

            if (parameterValue == "0")
            {
                return false;
            }

            bool value;
            return bool.TryParse(parameterValue, out value) && value;
        }

        /// <summary>
        /// Преобразуем из целочисленного строкового значения в int.
        /// Если парсинг не удался, то возвращаем default(int) == 0.
        /// </summary>
        /// <param name="value">Целочисленное строковое значение.</param>
        /// <returns>Полученный результат парсинга.</returns>
        private static int ConvertInt(string value)
        {
            int intValue;
            return int.TryParse(value, out intValue) ? intValue : default(int);
        }

        private object GetParameterValue(ParameterInfo parameter)
        {
            KeyValueConfigurationElement setting = _keyValueCollection[parameter.Name];
            var parameterValue = setting?.Value;

            // сразу возвращаем результат по умолчанию, если значения нет в конфиге.
            if (parameterValue == null)
            {
                return parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null;
            }

            // анализируем Nullable интерфейс, чтобы уменьшить количество проверок по типам.
            var parameterType = parameter.ParameterType.IsGenericType
                                 && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>)
                                     ? parameter.ParameterType.GetGenericArguments().First()
                                     : parameter.ParameterType;

            // анализируем по типам
            if (parameterType == typeof(string))
            {
                return parameterValue;
            }

            if (parameterType == typeof(int))
            {
                return ConvertInt(parameterValue);
            }

            if (parameterType == typeof(bool))
            {
                return ConvertBool(parameterValue);
            }

            if (parameterType == typeof(double))
            {
                return ConvertDouble(parameterValue);
            }

            if (parameterType == typeof(IEnumerable<string>))
            {
                return parameterValue.Split(ItemsDelimeter).ToArray();
            }

            if (parameterType == typeof(IEnumerable<int>))
            {
                return parameterValue.Split(ItemsDelimeter).Select(ConvertInt).ToArray();
            }

            if (parameterType == typeof(IEnumerable<bool>))
            {
                return parameterValue.Split(ItemsDelimeter).Select(ConvertBool).ToArray();
            }

            if (parameterType == typeof(IEnumerable<double>))
            {
                return parameterValue.Split(ItemsDelimeter).Select(ConvertDouble).ToArray();
            }

            throw new ArgumentException(
                $"Не возможно создать объект {typeof (T).Name}. Параметр \"{parameter.Name}\" имеет неподдерживаемый тип {parameterType}");
        }
    }
}