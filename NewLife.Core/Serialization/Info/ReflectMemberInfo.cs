﻿using System;
using System.Reflection;
using System.Xml.Serialization;
using NewLife.Reflection;

namespace NewLife.Serialization
{
    /// <summary>反射成员信息</summary>
    class ReflectMemberInfo : IObjectMemberInfo
    {
        #region 属性
        private FieldInfo _Field;
        private PropertyInfo _Property;
        /// <summary>成员</summary>
        public MemberInfo Member { get { return (MemberInfo)_Field ?? _Property; } private set { _Field = value as FieldInfo; _Property = value as PropertyInfo; } }

        //private MemberInfoX _Mix;
        ///// <summary>快速反射</summary>
        //private MemberInfoX Mix { get { return _Mix ?? (_Mix = Member); } }
        #endregion

        #region 构造
        /// <summary>实例化</summary>
        /// <param name="member"></param>
        public ReflectMemberInfo(MemberInfo member)
        {
            Member = member;
        }
        #endregion

        #region IObjectMemberInfo 成员
        private String _Name;
        /// <summary>名称</summary>
        public String Name { get { return _Name ?? (_Name = GetName()); } }

        /// <summary>类型</summary>
        public Type Type
        {
            get
            {
                if (_Field != null) return _Field.FieldType;
                if (_Property != null) return _Property.PropertyType;
                return null;
            }
        }

        /// <summary>对目标对象取值赋值</summary>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public object this[object target]
        {
            get
            {
                //return Mix.GetValue(target);
                if (_Field != null) return target.GetValue(_Field);
                if (_Property != null) return target.GetValue(_Property);
                return null;
            }
            set
            {
                //Mix.SetValue(target, value);
                if (_Field != null) target.SetValue(_Field, value);
                else if (_Property != null) target.SetValue(_Property, value);
            }
        }

        ///// <summary>是否可读</summary>
        //public bool CanRead
        //{
        //    get
        //    {
        //        if (Member.MemberType == MemberTypes.Field) return true;
        //        if (Member.MemberType == MemberTypes.Property) return (Member as PropertyInfo).CanRead;
        //        return false;
        //    }
        //}

        ///// <summary>是否可写</summary>
        //public bool CanWrite
        //{
        //    get
        //    {
        //        if (Member.MemberType == MemberTypes.Field) return true;
        //        if (Member.MemberType == MemberTypes.Property) return (Member as PropertyInfo).CanWrite;
        //        return false;
        //    }
        //}
        #endregion

        #region 方法
        String GetName()
        {
            String name = null;
            if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlAttributeAttribute>(Member, "AttributeName");
            //if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlArrayItemAttribute>(Member, "ElementName");
            if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlArrayAttribute>(Member, "ElementName");
            if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlElementAttribute>(Member, "ElementName");
            if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlAnyElementAttribute>(Member, "Name");
            if (String.IsNullOrEmpty(name)) name = GetCustomAttributeValue<XmlRootAttribute>(Type, "ElementName");

            if (String.IsNullOrEmpty(name)) name = Member.Name;
            return name;
        }

        static String GetCustomAttributeValue<TAttribute>(MemberInfo member, String name)
        {
            var att = AttributeX.GetCustomAttribute<TAttribute>(member, true);
            if (att == null) return null;

            //var pix = PropertyInfoX.Create(typeof(TAttribute), name);
            //if (pix == null) return null;

            //return (String)pix.GetValue(att);

            Object value = null;
            if (att.TryGetValue(name, out value)) return (String)value;
            return null;
        }
        #endregion

        #region 已重载
        /// <summary>已重载。</summary>
        /// <returns></returns>
        public override string ToString() { return Name; }
        #endregion
    }
}