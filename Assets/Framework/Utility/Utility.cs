using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    public static class Utility
    {
        public static class Text
        {
            private const int StringBuilderCapacity = 1024;

            [ThreadStatic]
            private static StringBuilder s_CachedStringBuilder;

            public static string Format(string format, object arg0)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format(string format, object arg0, object arg1)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0, arg1);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format(string format, object arg0, object arg1, object arg2)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0, arg1, arg2);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format(string format, params object[] args)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                if (args == null)
                {
                    throw new ArgumentException("Args is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, args);
                return s_CachedStringBuilder.ToString();
            }

            private static void CheckCachedStringBuilder()
            {
                if (s_CachedStringBuilder == null)
                {
                    s_CachedStringBuilder = new StringBuilder(1024);
                }
            }
        }

        public static class Verifier
        {
            private sealed class Crc32
            {
                private const int TableLength = 256;

                private const uint DefaultPolynomial = 3988292384u;

                private const uint DefaultSeed = uint.MaxValue;

                private readonly uint m_Seed;

                private readonly uint[] m_Table;

                private uint m_Hash;

                public Crc32()
                    : this(3988292384u, uint.MaxValue)
                {
                }

                public Crc32(uint polynomial, uint seed)
                {
                    m_Seed = seed;
                    m_Table = InitializeTable(polynomial);
                    m_Hash = seed;
                }

                public void Initialize()
                {
                    m_Hash = m_Seed;
                }

                public void HashCore(byte[] bytes, int offset, int length)
                {
                    m_Hash = CalculateHash(m_Table, m_Hash, bytes, offset, length);
                }

                public uint HashFinal()
                {
                    return ~m_Hash;
                }

                private static uint CalculateHash(uint[] table, uint value, byte[] bytes, int offset, int length)
                {
                    int num = offset + length;
                    for (int i = offset; i < num; i++)
                    {
                        value = (value >> 8) ^ table[bytes[i] ^ (value & 0xFF)];
                    }

                    return value;
                }

                private static uint[] InitializeTable(uint polynomial)
                {
                    uint[] array = new uint[256];
                    for (int i = 0; i < 256; i++)
                    {
                        uint num = (uint)i;
                        for (int j = 0; j < 8; j++)
                        {
                            num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ polynomial));
                        }

                        array[i] = num;
                    }

                    return array;
                }
            }

            private const int CachedBytesLength = 4096;

            private static readonly byte[] s_CachedBytes = new byte[4096];

            private static readonly Crc32 s_Algorithm = new Crc32();

            public static int GetCrc32(byte[] bytes)
            {
                if (bytes == null)
                {
                    throw new ArgumentException("Bytes is invalid.");
                }

                return GetCrc32(bytes, 0, bytes.Length);
            }

            public static int GetCrc32(byte[] bytes, int offset, int length)
            {
                if (bytes == null)
                {
                    throw new ArgumentException("Bytes is invalid.");
                }

                if (offset < 0 || length < 0 || offset + length > bytes.Length)
                {
                    throw new ArgumentException("Offset or length is invalid.");
                }

                s_Algorithm.HashCore(bytes, offset, length);
                uint result = s_Algorithm.HashFinal();
                s_Algorithm.Initialize();
                return (int)result;
            }

            public static int GetCrc32(Stream stream)
            {
                if (stream == null)
                {
                    throw new ArgumentException("Stream is invalid.");
                }

                while (true)
                {
                    int num = stream.Read(s_CachedBytes, 0, 4096);
                    if (num <= 0)
                    {
                        break;
                    }

                    s_Algorithm.HashCore(s_CachedBytes, 0, num);
                }

                uint result = s_Algorithm.HashFinal();
                s_Algorithm.Initialize();
                Array.Clear(s_CachedBytes, 0, 4096);
                return (int)result;
            }

            public static byte[] GetCrc32Bytes(int crc32)
            {
                return new byte[4]
                {
                    (byte)((uint)(crc32 >> 24) & 0xFFu),
                    (byte)((uint)(crc32 >> 16) & 0xFFu),
                    (byte)((uint)(crc32 >> 8) & 0xFFu),
                    (byte)((uint)crc32 & 0xFFu)
                };
            }

            //
            // 摘要:
            //     获取 CRC32 数值的二进制数组。
            //
            // 参数:
            //   crc32:
            //     CRC32 数值。
            //
            //   bytes:
            //     要存放结果的数组。
            public static void GetCrc32Bytes(int crc32, byte[] bytes)
            {
                GetCrc32Bytes(crc32, bytes, 0);
            }

            public static void GetCrc32Bytes(int crc32, byte[] bytes, int offset)
            {
                if (bytes == null)
                {
                    throw new ArgumentException("Result is invalid.");
                }

                if (offset < 0 || offset + 4 > bytes.Length)
                {
                    throw new ArgumentException("Offset or length is invalid.");
                }

                bytes[offset] = (byte)((uint)(crc32 >> 24) & 0xFFu);
                bytes[offset + 1] = (byte)((uint)(crc32 >> 16) & 0xFFu);
                bytes[offset + 2] = (byte)((uint)(crc32 >> 8) & 0xFFu);
                bytes[offset + 3] = (byte)((uint)crc32 & 0xFFu);
            }

            internal static int GetCrc32(Stream stream, byte[] code, int length)
            {
                if (stream == null)
                {
                    throw new ArgumentException("Stream is invalid.");
                }

                if (code == null)
                {
                    throw new ArgumentException("Code is invalid.");
                }

                int num = code.Length;
                if (num <= 0)
                {
                    throw new ArgumentException("Code length is invalid.");
                }

                int num2 = (int)stream.Length;
                if (length < 0 || length > num2)
                {
                    length = num2;
                }

                int num3 = 0;
                while (true)
                {
                    int num4 = stream.Read(s_CachedBytes, 0, 4096);
                    if (num4 <= 0)
                    {
                        break;
                    }

                    if (length > 0)
                    {
                        for (int i = 0; i < num4 && i < length; i++)
                        {
                            s_CachedBytes[i] ^= code[num3++];
                            num3 %= num;
                        }

                        length -= num4;
                    }

                    s_Algorithm.HashCore(s_CachedBytes, 0, num4);
                }

                uint result = s_Algorithm.HashFinal();
                s_Algorithm.Initialize();
                Array.Clear(s_CachedBytes, 0, 4096);
                return (int)result;
            }
        }
    }
}

