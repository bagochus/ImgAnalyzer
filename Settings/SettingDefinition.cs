using ImgAnalyzer.Properties;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImgAnalyzer
{


    /// <summary>
    /// Класс, описывающий одну настройку приложения
    /// </summary>
    public class SettingDefinition
    {
        private static Type[] supportedTypes =
            { 
            typeof(bool),
            typeof(int),
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(string) 
        };

        /// <summary>
        /// Уникальное имя настройки (ключ)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип значения настройки
        /// </summary>
        public Type ValueType { get; set; }

        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        /// 

        public string Owner { get; set; }

        public bool IsDefault { get { return Value == DefaultValue; } }

        public object DefaultValue { get; set; }

        public object Value { get; set; }

        /// <summary>
        /// Комментарий/описание настройки
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// 

        private bool _global = false;
        public bool Global => _global;

        public bool ConvertFromString(string s)
        {
            bool result = true;
            try
            {
                
            if (ValueType == typeof(bool))
                Value = bool.Parse(s);

            if (ValueType == typeof(int))
                Value = int.Parse(s);

            if (ValueType == typeof(double))
                Value = double.Parse(s);

            if (ValueType == typeof(decimal))
                Value = decimal.Parse(s);

            if (ValueType == typeof(DateTime))
                Value = DateTime.Parse(s);

            if (ValueType == typeof(string))
                Value = s;
            }
            catch { result = false; }

            return result;

        }

        public void SetDefault()
        {
            Value = DefaultValue;
        }

        /// <summary>
        /// Упрощенный конструктор с автоматическим определением типа
        /// </summary>
        public SettingDefinition(string name, object defaultValue, string comment = "")
        {
            if (!supportedTypes.Contains(defaultValue.GetType()))
                throw new ArgumentException("Type not supported");

            Name = name;
            ValueType = defaultValue.GetType();
            DefaultValue = defaultValue;
            Value = defaultValue;   
            Comment = comment;
            _global = true;
            Owner = "global";

        }

        private SettingDefinition(string name, object defaultValue, object owner, string comment = "" )
        {
            if (!supportedTypes.Contains(defaultValue.GetType()))
                throw new ArgumentException("Type not supported");

            Name = name;
            ValueType = defaultValue.GetType();
            DefaultValue = defaultValue;
            Value = defaultValue;
            Comment = comment;
            Owner = owner.GetType().Name;
        }

        public static SettingDefinition CreateGlobal(string name, object defaultValue, string comment = "")
        { 
            SettingDefinition sd = new SettingDefinition(name, defaultValue, comment);
            return sd;
        }

        public static SettingDefinition CreateLocal(string name, object defaultValue, object owner, string comment = "")
        { 
            SettingDefinition sd = new SettingDefinition(name, defaultValue, owner ,comment);
            return sd;
        }

        public T GetValue<T>()
        {
            return (T)Value;
        
        
        
        }


    }
}
