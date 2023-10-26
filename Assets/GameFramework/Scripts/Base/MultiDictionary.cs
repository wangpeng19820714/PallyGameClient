using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// ��Ϸ��ܶ�ֵ�ֵ��ࡣ
    /// </summary>
    /// <typeparam name="TKey">ָ����ֵ�ֵ���������͡�</typeparam>
    /// <typeparam name="TValue">ָ����ֵ�ֵ��ֵ���͡�</typeparam>
    public sealed class MultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, LinkedListRange<TValue>>>, IEnumerable
    {
        private readonly GameFrameworkLinkedList<TValue> m_LinkedList;
        private readonly Dictionary<TKey, LinkedListRange<TValue>> m_Dictionary;

        /// <summary>
        /// ��ʼ����Ϸ��ܶ�ֵ�ֵ������ʵ����
        /// </summary>
        public MultiDictionary()
        {
            m_LinkedList = new GameFrameworkLinkedList<TValue>();
            m_Dictionary = new Dictionary<TKey, LinkedListRange<TValue>>();
        }

        /// <summary>
        /// ��ȡ��ֵ�ֵ���ʵ�ʰ���������������
        /// </summary>
        public int Count
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// ��ȡ��ֵ�ֵ���ָ�������ķ�Χ��
        /// </summary>
        /// <param name="key">ָ����������</param>
        /// <returns>ָ�������ķ�Χ��</returns>
        public LinkedListRange<TValue> this[TKey key]
        {
            get
            {
                LinkedListRange<TValue> range = default(LinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }

        /// <summary>
        /// �����ֵ�ֵ䡣
        /// </summary>
        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }

        /// <summary>
        /// ����ֵ�ֵ����Ƿ����ָ��������
        /// </summary>
        /// <param name="key">Ҫ����������</param>
        /// <returns>��ֵ�ֵ����Ƿ����ָ��������</returns>
        public bool Contains(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// ����ֵ�ֵ����Ƿ����ָ��ֵ��
        /// </summary>
        /// <param name="key">Ҫ����������</param>
        /// <param name="value">Ҫ����ֵ��</param>
        /// <returns>��ֵ�ֵ����Ƿ����ָ��ֵ��</returns>
        public bool Contains(TKey key, TValue value)
        {
            LinkedListRange<TValue> range = default(LinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                return range.Contains(value);
            }

            return false;
        }

        /// <summary>
        /// ���Ի�ȡ��ֵ�ֵ���ָ�������ķ�Χ��
        /// </summary>
        /// <param name="key">ָ����������</param>
        /// <param name="range">ָ�������ķ�Χ��</param>
        /// <returns>�Ƿ��ȡ�ɹ���</returns>
        public bool TryGetValue(TKey key, out LinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }

        /// <summary>
        /// ��ָ������������ָ����ֵ��
        /// </summary>
        /// <param name="key">ָ����������</param>
        /// <param name="value">ָ����ֵ��</param>
        public void Add(TKey key, TValue value)
        {
            LinkedListRange<TValue> range = default(LinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_LinkedList.AddBefore(range.Terminal, value);
            }
            else
            {
                LinkedListNode<TValue> first = m_LinkedList.AddLast(value);
                LinkedListNode<TValue> terminal = m_LinkedList.AddLast(default(TValue));
                m_Dictionary.Add(key, new LinkedListRange<TValue>(first, terminal));
            }
        }

        /// <summary>
        /// ��ָ�����������Ƴ�ָ����ֵ��
        /// </summary>
        /// <param name="key">ָ����������</param>
        /// <param name="value">ָ����ֵ��</param>
        /// <returns>�Ƿ��Ƴ��ɹ���</returns>
        public bool Remove(TKey key, TValue value)
        {
            LinkedListRange<TValue> range = default(LinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                for (LinkedListNode<TValue> current = range.First; current != null && current != range.Terminal; current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        if (current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            if (next == range.Terminal)
                            {
                                m_LinkedList.Remove(next);
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                m_Dictionary[key] = new LinkedListRange<TValue>(next, range.Terminal);
                            }
                        }

                        m_LinkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// ��ָ�����������Ƴ����е�ֵ��
        /// </summary>
        /// <param name="key">ָ����������</param>
        /// <returns>�Ƿ��Ƴ��ɹ���</returns>
        public bool RemoveAll(TKey key)
        {
            LinkedListRange<TValue> range = default(LinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_Dictionary.Remove(key);

                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    LinkedListNode<TValue> next = current != range.Terminal ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        IEnumerator<KeyValuePair<TKey, LinkedListRange<TValue>>> IEnumerable<KeyValuePair<TKey, LinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// ѭ�����ʼ��ϵ�ö������
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, LinkedListRange<TValue>>>, IEnumerator
        {
            private Dictionary<TKey, LinkedListRange<TValue>>.Enumerator m_Enumerator;

            internal Enumerator(Dictionary<TKey, LinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new ArgumentException("Dictionary is invalid.");
                }

                m_Enumerator = dictionary.GetEnumerator();
            }

            /// <summary>
            /// ��ȡ��ǰ��㡣
            /// </summary>
            public KeyValuePair<TKey, LinkedListRange<TValue>> Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// ��ȡ��ǰ��ö������
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// ����ö������
            /// </summary>
            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            /// <summary>
            /// ��ȡ��һ����㡣
            /// </summary>
            /// <returns>������һ����㡣</returns>
            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            /// <summary>
            /// ����ö������
            /// </summary>
            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, LinkedListRange<TValue>>>)m_Enumerator).Reset();
            }
        }
    }
}
