using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Menus.Settings
{
    public class LanguageSelector : MonoBehaviour
    {
        private bool _active;
        private int _var;

        public void ChangeLocale()
        {
            if (!_active)
            {
                StartCoroutine(SetLocale());
            }
        }

        private IEnumerator SetLocale()
        {
            _active = true;
            _var++;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_var % 2];
            _active = false;
        }
        
    }
}