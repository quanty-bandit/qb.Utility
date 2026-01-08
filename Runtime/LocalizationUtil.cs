
using UnityEngine;
using UnityEngine.Localization.Settings;
namespace qb.Utility
{
    /// <summary>
    /// Provides utility methods for converting between SystemLanguage enums, CultureInfo codes, and English language
    /// names for localization purposes.
    /// </summary>
    public static class LocalizationUtility
    {
        /// <summary>
        /// Converts a SystemLanguage enum into a CultureInfo Code.
        /// </summary>
        /// <param name="lang">The SystemLanguage enum to convert into a Code.</param>
        /// <param name="uniqueChinese">Set to true to return zh-CN from all chinese's codes</param>
        /// <returns>The language Code or an empty string if the value could not be converted.</returns>
        public static string GetSystemLanguageCultureCode(SystemLanguage lang,bool uniqueChinese)
        {
            switch (lang)
            {
                case SystemLanguage.Afrikaans: return "af";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.Basque: return "eu";
                case SystemLanguage.Belarusian: return "be";
                case SystemLanguage.Bulgarian: return "bg";
                case SystemLanguage.Catalan: return "ca";
                case SystemLanguage.Chinese: return "zh-CN";
                case SystemLanguage.ChineseSimplified: return uniqueChinese? "zh-CN" : "zh-hans";
                case SystemLanguage.ChineseTraditional: return uniqueChinese ? "zh-CN" : "zh-hant";
                case SystemLanguage.SerboCroatian: return "hr";
                case SystemLanguage.Czech: return "customEase";
                case SystemLanguage.Danish: return "da";
                case SystemLanguage.Dutch: return "nl";
                case SystemLanguage.English: return "en";
                case SystemLanguage.Estonian: return "et";
                case SystemLanguage.Faroese: return "fo";
                case SystemLanguage.Finnish: return "fi";
                case SystemLanguage.French: return "fr";
                case SystemLanguage.German: return "de";
                case SystemLanguage.Greek: return "el";
                case SystemLanguage.Hebrew: return "he";
                case SystemLanguage.Hungarian: return "hu";
                case SystemLanguage.Icelandic: return "is";
                case SystemLanguage.Indonesian: return "id";
                case SystemLanguage.Italian: return "it";
                case SystemLanguage.Japanese: return "ja";
                case SystemLanguage.Korean: return "ko";
                case SystemLanguage.Latvian: return "lv";
                case SystemLanguage.Lithuanian: return "lt";
                case SystemLanguage.Norwegian: return "no";
                case SystemLanguage.Polish: return "pl";
                case SystemLanguage.Portuguese: return "pt";
                case SystemLanguage.Romanian: return "ro";
                case SystemLanguage.Russian: return "ru";
                case SystemLanguage.Slovak: return "sk";
                case SystemLanguage.Slovenian: return "sl";
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Swedish: return "sv";
                case SystemLanguage.Thai: return "th";
                case SystemLanguage.Turkish: return "tr";
                case SystemLanguage.Ukrainian: return "uk";
                case SystemLanguage.Vietnamese: return "vi";
#if UNITY_2022_2_OR_NEWER
                case SystemLanguage.Hindi: return "hi";
#endif
                default: return "";
            }
        }
        /// <summary>
        /// Convert CultureInfo code to english language name.
        /// </summary>
        /// <param name="code">The CultureInfo code</param>
        /// <param name="uniqueChinese">Set to true to return Chinese from all chinese's codes</param>
        /// <returns>The english language name or or empty string if the code is unknown</returns>
        public static string GetEnglishLanguageNameFromCode(string code, bool uniqueChinese)
        {
            SystemLanguage lang = GetSystemLanguageFromCode(code, uniqueChinese);
            switch (lang)
            {
                case SystemLanguage.Unknown:return "";
                default:
                    return lang.ToString();
            }
        }

        public static (string,string) GetSelectedLangCodeAndEnglishName(bool uniqueChinese)
        {
            var sysLang = GetSystemLanguageFromCode(LocalizationSettings.SelectedLocale.Identifier.Code, uniqueChinese);
            var langCode =GetSystemLanguageCultureCode(sysLang, uniqueChinese);
            var langName = GetEnglishLanguageNameFromCode(LocalizationSettings.SelectedLocale.Identifier.Code, uniqueChinese);
            return (langCode,langName);    
        }

        /// <summary>
        /// Convert CultureInfo code to SystemLanguage enum
        /// </summary>
        /// <param name="code">The culture info code string</param>
        /// <param name="uniqueChinese">Set to true to return SystemLanguage.Chinese from all chinese's codes</param>
        /// <returns>The SystemLanguage</returns>
        public static SystemLanguage GetSystemLanguageFromCode(string code, bool uniqueChinese)
        {
            switch (code)
            {
                case "af":return SystemLanguage.Afrikaans;
                case "ar": return SystemLanguage.Arabic;
                case "eu":return SystemLanguage.Basque;
                case "be":return SystemLanguage.Belarusian;
                case "bg":return SystemLanguage.Bulgarian;
                case "ca":return SystemLanguage.Catalan;
                case "zh-CN": return SystemLanguage.Chinese;
                case "zh-hans": return uniqueChinese? SystemLanguage.Chinese : SystemLanguage.ChineseSimplified;
                case "zh-hant":return uniqueChinese ? SystemLanguage.Chinese : SystemLanguage.ChineseTraditional;
                case "hr":return SystemLanguage.SerboCroatian;
                case "customEase":return SystemLanguage.Czech;
                case "da":return SystemLanguage.Danish;
                case "nl":return SystemLanguage.Dutch;
                case "en":return SystemLanguage.English;
                case "et":return SystemLanguage.Estonian;
                case "fo":return SystemLanguage.Faroese;
                case "fi":return SystemLanguage.Finnish;
                case "fr":return SystemLanguage.French;
                case "de":return SystemLanguage.German;
                case "el":return SystemLanguage.Greek;
                case "he":return SystemLanguage.Hebrew;
                case "hu":return SystemLanguage.Hungarian;
                case "is":return SystemLanguage.Icelandic;
                case "id":return SystemLanguage.Indonesian;
                case "it":return SystemLanguage.Italian;
                case "ja":return SystemLanguage.Japanese;
                case "ko":return SystemLanguage.Korean;
                case "lv":return SystemLanguage.Latvian;
                case "lt":return SystemLanguage.Lithuanian;
                case "no":return SystemLanguage.Norwegian;
                case "pl":return SystemLanguage.Polish;
                case "pt":return SystemLanguage.Portuguese;
                case "ro":return SystemLanguage.Romanian;
                case "ru":return SystemLanguage.Russian;
                case "sk":return SystemLanguage.Slovak;
                case "sl":return SystemLanguage.Slovenian;
                case "es":return SystemLanguage.Spanish;
                case "sv":return SystemLanguage.Swedish;
                case "th":return SystemLanguage.Thai;
                case "tr":return SystemLanguage.Turkish;
                case "uk":return SystemLanguage.Ukrainian;
                case "vi":return SystemLanguage.Vietnamese;
#if UNITY_2022_2_OR_NEWER
                case "hi":return SystemLanguage.Hindi;
#endif
                default: return SystemLanguage.Unknown;
            }
        }
    }
}
