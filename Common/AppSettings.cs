using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Hydrate.Common
{
    public class AppSettings
    {
        IsolatedStorageSettings isolatedStore;

        const string GoalSettingKeyName = "Goal";
        const string SizeSettingKeyName = "Size";
        const string ReminderSettingKeyName = "Reminder";

        const string HistorySettingKeyName = "History";

        public AppSettings()
        {
            try
            {
                // Get the settings for this application.
                isolatedStore = IsolatedStorageSettings.ApplicationSettings;

                if (History == null)
                    History = new Dictionary<DateTime, double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        private bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            try
            {
                // if new value is different, set the new value.
                if (isolatedStore[Key] != value)
                {
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            catch (KeyNotFoundException)
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }
            catch (ArgumentException)
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }

            return valueChanged;
        }

        private valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            try
            {
                value = (valueType)isolatedStore[Key];
            }
            catch (KeyNotFoundException)
            {
                value = defaultValue;
            }
            catch (ArgumentException)
            {
                value = defaultValue;
            }

            return value;
        }

        public void Save()
        {
            isolatedStore.Save();
        }

        public double Goal
        {
            get
            {
                return GetValueOrDefault<double>(GoalSettingKeyName, 75);
            }
            set
            {
                AddOrUpdateValue(GoalSettingKeyName, value);
                Save();
            }
        }

        public double Size
        {
            get
            {
                return GetValueOrDefault<double>(SizeSettingKeyName, 16);
            }
            set
            {
                AddOrUpdateValue(SizeSettingKeyName, value);
                Save();
            }
        }

        public int Reminder
        {
            get
            {
                return GetValueOrDefault<int>(ReminderSettingKeyName, 1);
            }
            set
            {
                AddOrUpdateValue(ReminderSettingKeyName, value);
                Save();
            }
        }

        public Dictionary<DateTime, double> History
        {
            get
            {
                return GetValueOrDefault<Dictionary<DateTime, double>>(HistorySettingKeyName, null);
            }
            set
            {
                AddOrUpdateValue(HistorySettingKeyName, value);
                Save();
            }
        }

        public double Current
        {
            get
            {
                if (History.ContainsKey(DateTime.Today) == true)
                {
                    return History[DateTime.Today];
                }
                else
                {
                    return 0.0;
                }
            }

            set
            {
                if (History.ContainsKey(DateTime.Today) == true)
                {
                    History[DateTime.Today] = value;
                }
                else
                {
                    History.Add(DateTime.Today, value);
                }
            }
        }
    }
}