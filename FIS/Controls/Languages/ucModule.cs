using System;
using System.Drawing;
using DevExpress.XtraEditors;
using FIS.Entities;
using FIS.Utils;

namespace FIS.AppClient.Controls
{
    public partial class ucModule
    {
        public class ModuleLanguage
        {
            public ModuleInfo ModuleInfo { get; private set; }
            public string Title { get; set; }
            public string ExecutingStatus { get; set; }
            public Size Size { get; set; }

            public ModuleLanguage(ModuleInfo moduleInfo)
            {
                ModuleInfo = moduleInfo;
            }

            public void FormatButton(SimpleButton button)
            {
                button.Text = GetButtonCaption(button.Name);
                button.Image = GetButtonIcon16(button.Name);                
            }

            public void FormatButton(SimpleButton button, string buttonName)
            {
                button.Text = GetButtonCaption(buttonName);
                button.Image = GetButtonIcon16(buttonName);
            }

            public Size GetSize()
            {
                var strSize = LangUtils.TranslateModuleItem(LangType.SIZE, ModuleInfo);
                var arrSize = strSize.Split(new[] {",", ";", "x"}, StringSplitOptions.RemoveEmptyEntries);
                return new Size(int.Parse(arrSize[0]), int.Parse(arrSize[1]));
            }

            public string GetButtonCaption(string buttonName)
            {
                return LangUtils.TranslateModuleItem(LangType.BUTTON_CAPTION, ModuleInfo, buttonName);
            }

            public Image GetButtonIcon16(string buttonName)
            {
                var imageName = LangUtils.TranslateModuleItem(LangType.BUTTON_ICON, ModuleInfo, buttonName);

                if (string.IsNullOrEmpty(imageName))
                    return null;
                return ThemeUtils.Image16.Images[imageName];
            }

            public Image GetButtonIcon24(string buttonName)
            {
                var imageName = LangUtils.TranslateModuleItem(
                         LangType.BUTTON_ICON,
                         ModuleInfo,
                         buttonName);

                if (string.IsNullOrEmpty(imageName))
                    return null;
                return ThemeUtils.Image24.Images[imageName];
            }

            public string GetButtonHotkey(string buttonName)
            {
                return LangUtils.TranslateModuleItem(LangType.BUTTON_HOTKEY, ModuleInfo, buttonName);
            }

            public string GetButtonToolTip(string buttonName)
            {
                return LangUtils.TranslateModuleItem(LangType.BUTTON_TIP, ModuleInfo, buttonName);
            }

            public string GetSpecialStatus(string status)
            {
                return LangUtils.TranslateModuleItem(LangType.MODULE_STATUS, ModuleInfo, status);
            }

            public string GetLabelText(string fieldName)
            {
                return LangUtils.TranslateModuleItem(LangType.LABEL_FIELD, ModuleInfo, fieldName);
            }

            public string GetLayout(string layoutType)
            {
                if (layoutType == null)
                    return LangUtils.TranslateModuleItem(LangType.MODULE_LAYOUT, ModuleInfo);
                return LangUtils.TranslateModuleItem(LangType.MODULE_LAYOUT, ModuleInfo, layoutType);
            }

            public string GetRegExValidateText(string validateName, string defaulLabel)
            {
                return string.Format(LangUtils.Translate(LangType.VALID_ERROR, validateName, "RegEx"), defaulLabel);
            }

            public string GetValidateNumber(string validateName, string defaulLabel)
            {
                return string.Format(LangUtils.Translate(LangType.VALID_ERROR, validateName, "Number"), defaulLabel);
            }

            public string GetValidateProcedure(string validateName, string defaulLabel)
            {
                return string.Format(LangUtils.Translate(LangType.VALID_ERROR, validateName, "Procedure"), defaulLabel);
            }

            public string GetNullValidateText(string defaultLabel)
            {
                return string.Format(LangUtils.TranslateBasic("{0} can not empty", "VALUE_NOT_NULL_ALLOW"), defaultLabel);
            }

            public string GetDateValidateText(string defaultLabel)
            {
                return string.Format(LangUtils.TranslateBasic("{0} can not greater than now", "VALUE_NOT_EQUAL_NOW"), defaultLabel);
            }   

            public string GetRoleName(string roleName)
            {
                return LangUtils.Translate(LangType.ROLE_NAME, roleName, roleName);
            }            
        }
    }
}
