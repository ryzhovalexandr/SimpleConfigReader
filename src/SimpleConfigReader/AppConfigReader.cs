using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Configuration = System.Configuration.Configuration;

namespace SimpleConfigReader
{
    /// <summary>
    /// Читает конфигурационные данные.
    /// </summary>
    public static class AppConfigReader
    {
        /// <summary>
        /// Чтение настроек из заданной секции app.config в класс настроек.
        /// </summary>
        /// <param name="sectionName">Имя секции.</param>
        /// <typeparam name="T">Класс настроек.</typeparam>
        /// <returns>Прочитанные настройки.</returns>
        public static T ReadFromSection<T>(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            var configuration = GetConfiguration();
            var section = GetSection(configuration, sectionName);
            return ConfigurationReader<T>.ReadFromCollection(section.Settings);
        }

        /// <summary>
        /// Чтение настроек из секции appSettings app.config в класс настроек.
        /// </summary>
        /// <typeparam name="T">Класс настроек.</typeparam>
        /// <returns>Прочитанные настройки.</returns>
        public static T ReadFromAppSettings<T>()
        {
            var configuration = GetConfiguration();
            return ConfigurationReader<T>.ReadFromCollection(configuration.AppSettings.Settings);
        }

        private static Configuration GetConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        }

        private static AppSettingsSection GetSection(Configuration configuration, string sectionName)
        {
            var section = (AppSettingsSection)configuration.GetSection(sectionName);
            if (section == null)
            {
                throw new KeyNotFoundException($"Секция с именем \"{sectionName}\" не найдена");
            }

            return section;
        }
    }
}