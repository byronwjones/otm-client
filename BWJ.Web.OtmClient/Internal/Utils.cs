using AngleSharp.Dom;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BWJ.Web.OTM.Internal
{
    internal static class Utils
    {
        public static byte ToBooleanNumber(this bool value) => value ? 1 : 0;

        public static string EnumToDescriptiveString<T>(T enumValue)
            where T: Enum
        {
            var type = typeof(T);

            var value = type.GetMember(enumValue.ToString()).First();
            if (value != null)
            {
                var descAttribute = value.GetCustomAttribute<DescriptionAttribute>();
                if (descAttribute != null)
                {
                    return descAttribute.Description;
                }
                else
                {
                    var webOptionAttribute = value.GetCustomAttribute<WebOptionAttribute>();
                    if (webOptionAttribute != null)
                    {
                        return webOptionAttribute.Description;
                    }
                }
            }

            return enumValue.ToString(); // should never happen
        }

        public static string EnumToOptionValue<T>(T enumValue)
            where T : Enum
        {
            var type = typeof(T);

            var value = type.GetMember(enumValue.ToString()).First();
            if (value != null)
            {
                var webOptionAttribute = value.GetCustomAttribute<WebOptionAttribute>();
                if (webOptionAttribute != null)
                {
                    return webOptionAttribute.Value;
                }
                else
                {
                    throw new ArgumentException("This method may only be used for enumerations where all values are decorated with BWJ.Web.OTM.Internal.Attributes.WebOptionAttribute");
                }
            }

            return enumValue.ToString(); // should never happen
        }

        public static T OptionValueToEnumValue<T>(string value)
            where T : Enum
        {
            var type = typeof(T);

            var values = type.GetEnumValues();
            foreach(var v in values)
            {
                var enumVal = (T)v;

                try
                {
                    var valInfo = type.GetMember(enumVal.ToString()).First();
                    var webOptionAttribute = valInfo.GetCustomAttribute<WebOptionAttribute>();
                    if (webOptionAttribute.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        return enumVal;
                    }
                }
                catch
                {
                    throw new ArgumentException("This method may only be used for enumerations where all values are decorated with BWJ.Web.OTM.Internal.Attributes.WebOptionAttribute");
                }
            }

            return (T)Enum.ToObject(typeof(T), 0);
        }

        public static T OptionDescriptionToEnumValue<T>(string description)
            where T : Enum
        {
            var type = typeof(T);

            var values = type.GetEnumValues();
            foreach (var v in values)
            {
                var enumVal = (T)v;

                try
                {
                    var valInfo = type.GetMember(enumVal.ToString()).First();
                    var webOptionAttribute = valInfo.GetCustomAttribute<WebOptionAttribute>();
                    var descriptionAttribute = valInfo.GetCustomAttribute<DescriptionAttribute>();
                    var attributeValue = webOptionAttribute?.Description ??
                        descriptionAttribute?.Description ?? null;
                    if (attributeValue.Equals(description, StringComparison.OrdinalIgnoreCase))
                    {
                        return enumVal;
                    }
                }
                catch
                {
                    throw new ArgumentException($"This method may only be used for enumerations where all values are decorated with either {typeof(WebOptionAttribute).FullName}, or {typeof(DescriptionAttribute).FullName}");
                }
            }

            return (T)Enum.ToObject(typeof(T), 0);
        }

        public static IElement GetChildElementByAttribute(IElement parentElement, string attribute, string value)
        {
            var element = parentElement.QuerySelector($"[{attribute}='{value}']");
            if (element is null)
            {
                throw new HtmlParsingException($"Expected element with {attribute} {value}");
            }

            return element;
        }

        public static IElement GetChildElementById(IElement parentElement, string childElementId)
            => GetChildElementByAttribute(parentElement, "id", childElementId);
        public static IElement GetChildElementByName(IElement parentElement, string childElementName)
            => GetChildElementByAttribute(parentElement, "name", childElementName);

        public static string GetValueByAttribute(IElement formElement, string attribute, string value)
        {
            var element = GetChildElementByAttribute(formElement, attribute, value);
            return GetValue(element);
        }

        public static string GetValueById(IElement formElement, string childElementId)
            => GetValueByAttribute(formElement, "id", childElementId);

        public static string GetValueByName(IElement formElement, string childElementName)
            => GetValueByAttribute(formElement, "name", childElementName);

        public static string GetValue(IElement element)
            => WebUtility.HtmlDecode(element.GetAttribute("value"))?.Trim() ?? string.Empty;

        public static string GetText(IElement element, string elementDescriptor, bool optional = false)
        {
            var text = WebUtility.HtmlDecode(element.TextContent)?.Trim();
            if (string.IsNullOrEmpty(text) && optional == false)
            {
                throw new HtmlParsingException($"Expected text for element {elementDescriptor}");
            }

            return text ?? string.Empty;
        }

        public static IEnumerable<string> GetSelectedValues(IElement formElement, string childElementId)
        {
            var selectElement = GetChildElementById(formElement, childElementId);
            return GetSelectedValues(selectElement);
        }
        public static IEnumerable<string> GetSelectedValues(IElement selectElement)
        {
            var allSelections = selectElement.QuerySelectorAll("option[selected]");
            if (allSelections is null || allSelections.Any() == false)
            {
                return new string[0];
            }

            return allSelections.Select(e => e.GetAttribute("value") ?? string.Empty);
        }

        public static string GetSelectedValue(IElement formElement, string childElementId)
            => GetSelectedValues(formElement, childElementId).FirstOrDefault();
        public static string GetSelectedValue(IElement selectElement)
            => GetSelectedValues(selectElement).FirstOrDefault();

        public static string ToFormString(object obj)
        {
            if(obj is null) { return string.Empty; }

            var result = obj.ToString();
            return string.IsNullOrWhiteSpace(result) ? string.Empty : result;
        }

        public static DateTime? GetDate(IElement e, string dataDescription, bool optional = false)
        {
            var text = GetText(e, dataDescription, optional);
            if (string.IsNullOrWhiteSpace(text)) { return null; }

            if (false == Regex.IsMatch(text, @"^[0-9]{1,2}/[0-9]{1,2}/([0-9]{2}|[0-9]{4})$"))
            {
                throw new HtmlParsingException($"{dataDescription} is in an unexpected format. Expected 'mm/dd/yy' or 'mm/dd/yyyy', got '{text}'");
            }

            var dateParts = text.Split('/');
            var year = Convert.ToInt32(dateParts[2]);
            return new DateTime(
                year: year < 100 ? (2000 + year) : year,
                month: Convert.ToInt32(dateParts[0]),
                day: Convert.ToInt32(dateParts[1]));
        }

        public static DateTime? GetDate(string dateString, string dataDescription, bool optional = false)
        {
            if (string.IsNullOrWhiteSpace(dateString)) { return null; }

            if (false == Regex.IsMatch(dateString, @"^[0-9]{1,2}/[0-9]{1,2}/([0-9]{2}|[0-9]{4})$"))
            {
                throw new HtmlParsingException($"{dataDescription} is in an unexpected format. Expected 'mm/dd/yy' or 'mm/dd/yyyy', got '{dateString}'");
            }

            var dateParts = dateString.Split('/');
            var year = Convert.ToInt32(dateParts[2]);
            return new DateTime(
                year: year < 100 ? (2000 + year) : year,
                month: Convert.ToInt32(dateParts[0]),
                day: Convert.ToInt32(dateParts[1]));
        }

        public static bool? GetBoolean(IElement e, string dataDescription, bool optional = false)
        {
            var text = GetText(e, dataDescription, optional);
            if (string.IsNullOrWhiteSpace(text)) { return null; }

            var affirmative = new string[] { "yes", "y", "true", "t", "1" };
            return affirmative.Contains(text.ToLower());
        }
    }
}
