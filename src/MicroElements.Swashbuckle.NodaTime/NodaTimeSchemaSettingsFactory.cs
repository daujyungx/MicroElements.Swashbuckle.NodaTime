﻿using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;

namespace MicroElements.Swashbuckle.NodaTime
{
    /// <summary>
    /// Factory methods for <see cref="NodaTimeSchemaSettings"/>.
    /// </summary>
    public static class NodaTimeSchemaSettingsFactory
    {
        /// <summary>
        /// Creates <see cref="NodaTimeSchemaSettings"/> for NewtonsoftJson.
        /// </summary>
        /// <param name="serializerSettings"><see cref="JsonSerializerSettings"/>.</param>
        /// <param name="shouldGenerateExamples">Should the example node be generated.</param>
        /// <param name="dateTimeZoneProvider">Optional <see cref="IDateTimeZoneProvider"/>.</param>
        /// <param name="example"></param>
        /// <returns><see cref="NodaTimeSchemaSettings"/>.</returns>
        public static NodaTimeSchemaSettings CreateNodaTimeSchemaSettingsForNewtonsoftJson(
            this JsonSerializerSettings serializerSettings,
            bool shouldGenerateExamples = true,
            IDateTimeZoneProvider dateTimeZoneProvider = null,
            DateTimeOffset? example = null)
        {
            string FormatToJson(object value)
            {
                string formatToJson = JsonConvert.SerializeObject(value, serializerSettings);
                if (formatToJson.StartsWith("\"") && formatToJson.EndsWith("\""))
                    formatToJson = formatToJson.Substring(1, formatToJson.Length - 2);
                return formatToJson;
            }

            string ResolvePropertyName(string propertyName)
            {
                return (serializerSettings.ContractResolver as DefaultContractResolver)?.GetResolvedPropertyName(propertyName) ?? propertyName;
            }

            return new NodaTimeSchemaSettings(ResolvePropertyName, FormatToJson, shouldGenerateExamples, dateTimeZoneProvider, example);
        }

        /// <summary>
        /// Creates <see cref="NodaTimeSchemaSettings"/> for SystemTextJson.
        /// </summary>
        /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/>.</param>
        /// <param name="shouldGenerateExamples">Should the example node be generated.</param>
        /// <param name="dateTimeZoneProvider">Optional <see cref="IDateTimeZoneProvider"/>.</param>
        /// <param name="example"></param>
        /// <returns><see cref="NodaTimeSchemaSettings"/>.</returns>
        public static NodaTimeSchemaSettings CreateNodaTimeSchemaSettingsForSystemTextJson(
            this JsonSerializerOptions jsonSerializerOptions,
            bool shouldGenerateExamples = true,
            IDateTimeZoneProvider dateTimeZoneProvider = null,
            DateTimeOffset? example = null)
        {
            string FormatToJson(object value)
            {
                string formatToJson = System.Text.Json.JsonSerializer.Serialize(value, jsonSerializerOptions);
                if (formatToJson.StartsWith("\"") && formatToJson.EndsWith("\""))
                    formatToJson = formatToJson.Substring(1, formatToJson.Length - 2);
                return formatToJson;
            }

            string ResolvePropertyName(string propertyName)
            {
                return jsonSerializerOptions.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
            }

            return new NodaTimeSchemaSettings(ResolvePropertyName, FormatToJson, shouldGenerateExamples, dateTimeZoneProvider, example);
        }
    }
}
