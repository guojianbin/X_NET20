﻿using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using NewLife.Serialization;

namespace NewLife.Messaging
{
    /// <summary>经过压缩的消息</summary>
    /// <remarks>因为写入一个字节的对象引用，所以<see cref="Message"/>为空时后面不怕有其它包</remarks>
    public class CompressionMessage : Message//, IAccessor
    {
        /// <summary>消息类型</summary>
        [XmlIgnore]
        public override MessageKind Kind { get { return MessageKind.Compression; } }

        [NonSerialized]
        private Message _Message;
        /// <summary>内部消息对象</summary>
        public Message Message { get { return _Message; } set { _Message = value; } }

        /// <summary>已重载。</summary>
        /// <param name="stream">数据流</param>
        /// <param name="rwkind">序列化类型</param>
        protected override void OnWrite(Stream stream, RWKinds rwkind)
        {
            if (Message == null)
            {
                // 对象引用
                stream.WriteByte(0);
                return;
            }

            // 对象引用。this为1，所以这里为2
            stream.WriteByte(2);

            // 写入消息。把消息写入压缩流，压缩后写入到输出流
            using (var ds = new DeflateStream(stream, CompressionMode.Compress, true))
            {
                Message.Write(ds, rwkind);
            }
        }

        /// <summary>已重载。</summary>
        /// <param name="stream">数据流</param>
        /// <param name="rwkind">序列化类型</param>
        protected override bool OnRead(Stream stream, RWKinds rwkind)
        {
            if (stream.Position == stream.Length) return true;

            // 读取对象引用
            var r = stream.ReadByte();
            if (r <= 0) return true;

            var ms = new MemoryStream();

            // 读取消息。对剩下的数据流，进行解压缩后，读取成为另一个消息
            using (var ds = new DeflateStream(stream, CompressionMode.Decompress, true))
            {
                //Message = Read(stream);
                // 必须全部复制到内存流，然后再读取，否则可能因为加密流不能读取位置和长度而导致消息读取失败
                ds.CopyTo(ms);
            }
            Message = Read(ms, rwkind);

            return true;
        }

        #region IAccessor 成员
        //Boolean IAccessor.Read(IReader reader)
        //{
        //    if (reader.Stream.Position == reader.Stream.Length) return true;

        //    var ms = new MemoryStream();

        //    // 读取消息。对剩下的数据流，进行解压缩后，读取成为另一个消息
        //    using (var stream = new DeflateStream(reader.Stream, CompressionMode.Decompress, true))
        //    {
        //        //Message = Read(stream);
        //        // 必须全部复制到内存流，然后再读取，否则可能因为加密流不能读取位置和长度而导致消息读取失败
        //        stream.CopyTo(ms);
        //    }
        //    Message = Read(ms, reader.GetKind());

        //    return true;
        //}

        //Boolean IAccessor.ReadComplete(IReader reader, Boolean success) { return success; }

        //Boolean IAccessor.Write(IWriter writer)
        //{
        //    if (Message == null) return true;

        //    // 写入消息。把消息写入压缩流，压缩后写入到输出流
        //    using (var stream = new DeflateStream(writer.Stream, CompressionMode.Compress, true))
        //    {
        //        Message.Write(stream, writer.GetKind());
        //    }

        //    return true;
        //}

        //Boolean IAccessor.WriteComplete(IWriter writer, Boolean success) { return success; }
        #endregion

        #region 辅助
        /// <summary>已重载。</summary>
        /// <returns></returns>
        public override string ToString()
        {
            var msg = Message;
            if (msg != null)
                return String.Format("{0} {1}", base.ToString(), msg);
            else
                return base.ToString();
        }
        #endregion
    }
}