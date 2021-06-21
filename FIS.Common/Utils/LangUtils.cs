using DevExpress.XtraBars;
using FIS.Common;
using FIS.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FIS.Utils
{
    public enum LangType
    {
        #region Define
        DEFINE_CODE,
        DEFINE_ERROR,
        #endregion
        #region Menu
        MENU_CAPTION,
        MENU_ICON,
        MENU_HOTKEY,
        #endregion
        #region Module
        MODULE_TITLE,
        MODULE_ICON,
        MODULE_TEXT,
        MODULE_STATUS,
        MODULE_LAYOUT,
        #endregion
        #region Button
        BUTTON_CAPTION,
        BUTTON_ICON,
        BUTTON_HOTKEY,
        BUTTON_TIP,
        #endregion
        #region Validate
        VALID_ERROR,
        VALID_NULL,
        #endregion
        PAGE_INFO,
        LABEL_FIELD,
        ICON_FIELD,
        ROLE_NAME,
        SIZE
    }

    public static class LangUtils
    {
        //#if DEBUG
        public static List<string> CaptureLanguage { get; set; }
        //#endif

        static LangUtils()
        {
            //#if DEBUG
            CaptureLanguage = new List<string>();
            //#endif
        }

        public static void LoadLangFromCache()
        {
            try
            {
                AllCaches.LanguageInfo = (Dictionary<string, string>)CommonUtils.GetCache(CONSTANTS.CACHED_LANG_FILENAME);
            }
            catch
            {
                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_GET_DEFINED_LANGUAGE_FAIL);
            }
        }

        //#if DEBUG
        public static void RefreshLanguage()
        {
            App.Environment.InitializeLanguage();

            foreach (var language in AllCaches.LanguageInfo)
            {
                if (CaptureLanguage.Contains(language.Key))
                {
                    CaptureLanguage.Remove(language.Key);
                }
            }
        }
        //#endif

        public static string TranslateBasic(string @default, string languageName)
        {
            if (AllCaches.LanguageInfo.ContainsKey(languageName))
            {
                return AllCaches.LanguageInfo[languageName];
            }
#if DEBUG
            if (!CaptureLanguage.Contains(languageName))
                CaptureLanguage.Add(languageName);
#endif

            return @default;
        }

        public static Image Get16x16Image(LangType type, params string[] @params)
        {
            var imageName = Translate(type, true, @params);
            if (!string.IsNullOrEmpty(imageName))
            {
                return ThemeUtils.Image16.Images[imageName];
            }
            return null;
        }

        public static Image Get24x24Image(LangType type, params string[] @params)
        {
            var imageName = Translate(type, true, @params);
            if (!string.IsNullOrEmpty(imageName))
            {
                return ThemeUtils.Image24.Images[imageName];
            }
            return null;
        }

        public static Image Get32x32Image(LangType type, params string[] @params)
        {
            var imageName = Translate(type, true, @params);
            if (!string.IsNullOrEmpty(imageName))
            {
                return ThemeUtils.Image32.Images[imageName];
            }
            return null;
        }

        public static Image Get48x48Image(LangType type, params string[] @params)
        {
            var imageName = Translate(type, true, @params);
            if (!string.IsNullOrEmpty(imageName))
            {
                return ThemeUtils.Image48.Images[imageName];
            }
            return null;
        }

        public static BarShortcut GetShortcut(LangType type, params string[] @params)
        {
            var hotkey = Translate(type, true, @params);
            if (!string.IsNullOrEmpty(hotkey))
            {
                var shortcut = (Shortcut)Enum.Parse(typeof(Shortcut), hotkey);
                return new BarShortcut(shortcut);
            }
            return null;
        }

        public static string Translate(LangType type, params string[] @params)
        {
            return Translate(type, false, @params);
        }

        private static string Translate(LangType type, bool returnEmpty, params string[] @params)
        {
            string langName = null;
            var defaultValue = @params[0];
            switch (type)
            {
                case LangType.DEFINE_ERROR:
                    langName = string.Format("DEFERROR${0}", @params);
                    defaultValue = "ERR{0}: " + @params[0];
                    break;
                case LangType.ROLE_NAME:
                    langName = string.Format("ROLE${0}", @params);
                    break;
                case LangType.DEFINE_CODE:
                    langName = string.Format("DEFCODE${0}${1}.{2}", @params);
                    defaultValue = string.Format("{1}.{2}", @params);
                    break;
                case LangType.MENU_CAPTION:
                    langName = string.Format("MENU${0}.Caption", @params);
                    break;
                case LangType.MENU_ICON:
                    langName = string.Format("MENU${0}.Icon", @params);
                    break;
                case LangType.MENU_HOTKEY:
                    langName = string.Format("MENU${0}.Hotkey", @params);
                    break;
                case LangType.BUTTON_CAPTION:
                    langName = string.Format("{0}${1}.Caption", @params);
                    defaultValue = string.Format("{1}", @params);
                    break;
                case LangType.BUTTON_ICON:
                    langName = string.Format("{0}${1}.Icon", @params);
                    break;
                case LangType.BUTTON_HOTKEY:
                    langName = string.Format("{0}${1}.Hotkey", @params);
                    break;
                case LangType.BUTTON_TIP:
                    langName = string.Format("{0}${1}.Tip", @params);
                    defaultValue = string.Format("{1}.Tip", @params);
                    break;
                case LangType.LABEL_FIELD:
                    langName = string.Format("{0}${1}.Label", @params);
                    defaultValue = string.Format("{1}", @params);
                    break;
                case LangType.ICON_FIELD:
                    langName = string.Format("{0}${1}.Icon", @params);
                    break;
                case LangType.MODULE_TITLE:
                    langName = string.Format("{0}.Title", @params);
                    defaultValue = string.Format("{0}.Title", @params);
                    break;
                case LangType.MODULE_STATUS:
                    if (@params.Length == 1)
                    {
                        langName = string.Format("{0}.Status", @params);
                    }
                    else
                        langName = string.Format("{0}.Status[{1}]", @params);
                    defaultValue = langName;
                    break;
                case LangType.PAGE_INFO:
                    if (@params.Length == 1)
                    {
                        langName = string.Format("{0}.PageInfo", @params);
                    }
                    else
                        langName = string.Format("{0}.PageInfo[{1}]", @params);
                    defaultValue = langName;
                    break;
                case LangType.MODULE_ICON:
                    if (@params.Length == 1)
                        langName = string.Format("{0}.Icon", @params);
                    else
                        langName = string.Format("{0}.Icon[{1}]", @params);
                    break;
                case LangType.MODULE_TEXT:
                    langName = string.Format("{0}.Text[{1}]", @params);
                    defaultValue = langName;
                    break;
                case LangType.MODULE_LAYOUT:
                    if (@params.Length == 1)
                        langName = string.Format("{0}.Layout", @params);
                    else
                        langName = string.Format("{0}.Layout[{1}]", @params);
                    break;
                case LangType.VALID_ERROR:
                    langName = string.Format("{0}.Validate[{1}]", @params);
                    defaultValue = langName;
                    break;
                case LangType.VALID_NULL:
                    langName = string.Format("{0}.IsNull", @params);
                    defaultValue = langName;
                    break;
                case LangType.SIZE:
                    langName = string.Format("{0}.Size", @params);
                    defaultValue = "640,480";
                    break;
            }

            switch (type)
            {
                case LangType.DEFINE_ERROR:
                case LangType.ROLE_NAME:
                case LangType.DEFINE_CODE:
                case LangType.MENU_CAPTION:
                case LangType.BUTTON_CAPTION:
                case LangType.LABEL_FIELD:
                case LangType.MODULE_TITLE:
                case LangType.MODULE_STATUS:
                case LangType.PAGE_INFO:
                case LangType.MODULE_TEXT:
                case LangType.VALID_ERROR:
                case LangType.VALID_NULL:
                case LangType.SIZE:
                    if (returnEmpty)
                        return TranslateBasic(null, langName);
                    return TranslateBasic(defaultValue, langName);
                case LangType.MENU_ICON:
                case LangType.MENU_HOTKEY:
                case LangType.BUTTON_ICON:
                case LangType.BUTTON_HOTKEY:
                case LangType.BUTTON_TIP:
                case LangType.ICON_FIELD:
                case LangType.MODULE_ICON:
                case LangType.MODULE_LAYOUT:
                    return TranslateBasic(null, langName);
            }

            throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_LANGUAGE_NOT_SUPPORTED);
        }

        public static string TranslateModuleItem(LangType type, ModuleInfo moduleInfo, params string[] @params)
        {
            var modParams = new List<string>(@params);
            modParams.Insert(0, moduleInfo.ModuleName);

            var langValue = Translate(type, true, modParams.ToArray());
            if (langValue != null) return langValue;

            var modTypeParams = new List<string>(@params);
            modTypeParams.Insert(0, moduleInfo.ModuleTypeName);

            langValue = Translate(type, true, modTypeParams.ToArray());
            if (langValue != null) return langValue;

            return Translate(type, false, modParams.ToArray());
        }

        public static T Translate<T>(LangType type, string name)
            where T : class
        {
            return (T)Convert.ChangeType(Translate(type, name), typeof(T));
        }
    }
}
