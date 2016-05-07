﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Configuration = System.Configuration.Configuration;

namespace SimpleConfigReader
{
    /// <summary>
    /// Читатель параметров из app.config.
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

            var configuration = GetConfiguration(Assembly.GetCallingAssembly());
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
            var configuration = GetConfiguration(Assembly.GetCallingAssembly());
            return ConfigurationReader<T>.ReadFromCollection(configuration.AppSettings.Settings);
        }

        private static Configuration GetConfiguration(Assembly configurationAssembly)
        {
            // для получения сборки, в которой непосредственно используется библиотека, пробрасываем assembly
            return ConfigurationManager.OpenExeConfiguration(configurationAssembly.Location);
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