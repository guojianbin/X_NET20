﻿/*
 * XCoder v4.8.4548.28140
 * 作者：nnhy/NEWLIFE
 * 时间：2012-06-18 11:52:47
 * 版权：版权所有 (C) 新生命开发团队 2012
*/
﻿using System;
using System.ComponentModel;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace NewLife.CommonEntity
{
    /// <summary>手册</summary>
    [Serializable]
    [DataObject]
    [Description("手册")]
    [BindIndex("IX_Manual_Url", true, "Url")]
    [BindIndex("PK_Manual", true, "ID")]
    [BindTable("Manual", Description = "手册", ConnName = "Common", DbType = DatabaseType.SqlServer)]
    public partial class Manual<TEntity> : IManual
    {
        #region 属性
        private Int32 _ID;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "ID", "编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging("ID", value)) { _ID = value; OnPropertyChanged("ID"); } }
        }

        private String _Url;
        /// <summary>资源</summary>
        [DisplayName("资源")]
        [Description("资源")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(2, "Url", "资源", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging("Url", value)) { _Url = value; OnPropertyChanged("Url"); } }
        }

        private String _Summary;
        /// <summary>摘要</summary>
        [DisplayName("摘要")]
        [Description("摘要")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(3, "Summary", "摘要", null, "nvarchar(200)", 0, 0, true)]
        public virtual String Summary
        {
            get { return _Summary; }
            set { if (OnPropertyChanging("Summary", value)) { _Summary = value; OnPropertyChanged("Summary"); } }
        }

        private String _Content;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn(4, "Content", "内容", null, "ntext", 0, 0, true)]
        public virtual String Content
        {
            get { return _Content; }
            set { if (OnPropertyChanging("Content", value)) { _Content = value; OnPropertyChanged("Content"); } }
        }

        private String _UserName;
        /// <summary>作者</summary>
        [DisplayName("作者")]
        [Description("作者")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "UserName", "作者", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging("UserName", value)) { _UserName = value; OnPropertyChanged("UserName"); } }
        }

        private DateTime _CreateTime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(6, "CreateTime", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateTime
        {
            get { return _CreateTime; }
            set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } }
        }

        private DateTime _UpdateTime;
        /// <summary>更新时间</summary>
        [DisplayName("更新时间")]
        [Description("更新时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(7, "UpdateTime", "更新时间", null, "datetime", 3, 0, false)]
        public virtual DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { if (OnPropertyChanging("UpdateTime", value)) { _UpdateTime = value; OnPropertyChanged("UpdateTime"); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "ID": return _ID;
                    case "Url": return _Url;
                    case "Summary": return _Summary;
                    case "Content": return _Content;
                    case "UserName": return _UserName;
                    case "CreateTime": return _CreateTime;
                    case "UpdateTime": return _UpdateTime;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "ID": _ID = Convert.ToInt32(value); break;
                    case "Url": _Url = Convert.ToString(value); break;
                    case "Summary": _Summary = Convert.ToString(value); break;
                    case "Content": _Content = Convert.ToString(value); break;
                    case "UserName": _UserName = Convert.ToString(value); break;
                    case "CreateTime": _CreateTime = Convert.ToDateTime(value); break;
                    case "UpdateTime": _UpdateTime = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得手册字段信息的快捷方式</summary>
        public class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName("ID");

            ///<summary>资源</summary>
            public static readonly Field Url = FindByName("Url");

            ///<summary>摘要</summary>
            public static readonly Field Summary = FindByName("Summary");

            ///<summary>内容</summary>
            public static readonly Field Content = FindByName("Content");

            ///<summary>作者</summary>
            public static readonly Field UserName = FindByName("UserName");

            ///<summary>创建时间</summary>
            public static readonly Field CreateTime = FindByName("CreateTime");

            ///<summary>更新时间</summary>
            public static readonly Field UpdateTime = FindByName("UpdateTime");

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }
        #endregion
    }

    /// <summary>手册接口</summary>
    public partial interface IManual
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>资源</summary>
        String Url { get; set; }

        /// <summary>摘要</summary>
        String Summary { get; set; }

        /// <summary>内容</summary>
        String Content { get; set; }

        /// <summary>作者</summary>
        String UserName { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreateTime { get; set; }

        /// <summary>更新时间</summary>
        DateTime UpdateTime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}