// ***********************************************************
// ��װ��ϵͳ�������йصķ�������
// Creator:YangMingkun  Date:2013-7-24
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Windy.Common.Libraries
{
    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ��ϵͳ�����йصķ���
        /// </summary>
        public struct Reflect
        {
            /// <summary>
            /// ����ָ������Ԫ�ض����ʵ��
            /// </summary>
            /// <param name="type">��������</param>
            /// <param name="args">�������</param>
            /// <returns>����ʵ��</returns>
            public static object CreateInstance(Type type, object[] args)
            {
                try
                {
                    Type[] types = new Type[0];
                    return type.GetConstructor(types).Invoke(args);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.CreateInstance", null, null, ex);
                    return null;
                }
            }

            /// <summary>
            /// ��ȡָ�������ָ�����Ե�����ֵ
            /// </summary>
            /// <param name="instance">ָ������</param>
            /// <param name="property">ָ������</param>
            /// <returns>����ֵ</returns>
            public static object GetPropertyValue(object instance, PropertyInfo property)
            {
                if (property == null || instance == null || !property.CanRead)
                    return null;
                try
                {
                    return property.GetValue(instance, null);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetPropertyValue", null, null, ex);
                    return null;
                }
            }

            /// <summary>
            /// ����ָ�������ָ�����Ե�����ֵ
            /// </summary>
            /// <param name="instance">ָ������</param>
            /// <param name="property">ָ������</param>
            /// <param name="value">����ֵ</param>
            /// <returns>�Ƿ�ɹ�</returns>
            public static bool SetPropertyValue(object instance, PropertyInfo property, object value)
            {
                if (property == null || instance == null || !property.CanWrite)
                    return false;

                try
                {
                    if (value == null || value.GetType() == property.PropertyType)
                    {
                        property.SetValue(instance, value, null);
                        return true;
                    }

                    TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                    }
                    property.SetValue(instance, value, null);
                    return true;
                }
                catch { return false; }
            }

            /// <summary>
            /// ����ͬ���͵��������������
            /// </summary>
            /// <param name="source">Դ����</param>
            /// <param name="target">Ŀ�����</param>
            /// <returns>�Ƿ�ɹ�</returns>
            public static bool CopyProperties(object source, object target)
            {
                if (source == null || target == null || source.GetType() != target.GetType())
                    return false;
                PropertyInfo[] elementProperties = source.GetType().GetProperties();
                foreach (PropertyInfo property in elementProperties)
                {
                    MethodInfo method = property.GetSetMethod();
                    if (method == null || !method.IsPublic)
                        continue;
                    if (!property.CanRead || !property.CanWrite)
                        continue;

                    Type propertyType = property.PropertyType;
                    object propertyValue = GetPropertyValue(source, property);
                    ICloneable cloneValue = propertyValue as ICloneable;
                    if (cloneValue != null)
                    {
                        SetPropertyValue(target, property, cloneValue.Clone());
                        continue;
                    }

                    //֧��IList��IDictionary�ӿ�����������
                    IDictionary dictionary = propertyValue as IDictionary;
                    if (dictionary != null)
                    {
                        IDictionary instance = null;
                        if (!property.CanWrite)
                            instance = GetPropertyValue(target, property) as IDictionary;
                        else
                            instance = CreateInstance(propertyType, null) as IDictionary;
                        if (instance == null)
                            continue;
                        foreach (DictionaryEntry entry in dictionary)
                        {
                            ICloneable clone = entry.Key as ICloneable;
                            object key = (clone == null) ? entry.Key : clone.Clone();
                            if (key == null)
                                continue;
                            object value = null;
                            if (entry.Value != null)
                            {
                                clone = entry.Value as ICloneable;
                                value = (clone == null) ? entry.Value : clone.Clone();
                            }
                            instance.Add(key, value);
                        }
                        GlobalMethods.Reflect.SetPropertyValue(target, property, instance);
                        continue;
                    }

                    //֧��IList��IDictionary�ӿ�����������
                    IList list = propertyValue as IList;
                    if (list != null)
                    {
                        IList instance = null;
                        if (!property.CanWrite)
                            instance = GetPropertyValue(target, property) as IList;
                        else
                            instance = CreateInstance(propertyType, null) as IList;
                        if (instance == null)
                            continue;
                        foreach (object item in list)
                        {
                            ICloneable clone = item as ICloneable;
                            if (clone == null)
                                instance.Add(item);
                            else
                                instance.Add(clone.Clone());
                        }
                        GlobalMethods.Reflect.SetPropertyValue(target, property, instance);
                        continue;
                    }
                    GlobalMethods.Reflect.SetPropertyValue(target, property, propertyValue);
                }
                return true;
            }
        }
    }
}
