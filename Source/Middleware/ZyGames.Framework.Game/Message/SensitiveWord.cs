﻿//------------------------------------------------------------------------------
// <auto-generated>
// 此代码由Codesmith工具生成。
// 此文件的更改可能会导致不正确的行为，如果
// 重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 
    /// </summary>
    /// <![CDATA[
    /// @periodTime:设置生存周期(秒)
    /// @personalName: 映射UserId对应的字段名,默认为"UserId"
    /// ]]>
    /// </remarks>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, "", "SensitiveWord")]
    public class SensitiveWord : ShareEntity
    {

        /// <summary>
        /// </summary>
        public SensitiveWord()
            : base(AccessLevel.ReadOnly)
        {

        }
        /// <summary>
        /// </summary>
        public SensitiveWord(int code)
            : this()
        {
            _code = code;
        }

        #region 自动生成属性
        private int _code;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField("Code", IsKey = true)]
        public int Code
        {
            get
            {
                return _code;
            }

        }
        private string _word;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField("Word")]
        public string Word
        {
            get
            {
                return _word;
            }

        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "Code": return _code;
                    case "Word": return _word;
                    default: throw new ArgumentException(string.Format("SensitiveWord index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "Code":
                        _code = value.ToInt();
                        break;
                    case "Word":
                        _word = value.ToNotNullString();
                        break;
                    default: throw new ArgumentException(string.Format("SensitiveWord index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion


    }
}