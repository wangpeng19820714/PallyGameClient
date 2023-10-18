using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// ��Ϸ�������Χ��
    /// </summary>
    /// <typeparam name="T">ָ������Χ��Ԫ�����͡�</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct LinkedListRange<T> : IEnumerable<T>, IEnumerable
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Terminal;

        /// <summary>
        /// ��ʼ����Ϸ�������Χ����ʵ����
        /// </summary>
        /// <param name="first">����Χ�Ŀ�ʼ��㡣</param>
        /// <param name="terminal">����Χ���ս��ǽ�㡣</param>
        public LinkedListRange(LinkedListNode<T> first, LinkedListNode<T> terminal)
        {
            if (first == null || terminal == null || first == terminal)
            {
                throw new ArgumentException("Range is invalid.");
            }

            m_First = first;
            m_Terminal = terminal;
        }

        /// <summary>
        /// ��ȡ����Χ�Ƿ���Ч��
        /// </summary>
        public bool IsValid
        {
            get
            {
                return m_First != null && m_Terminal != null && m_First != m_Terminal;
            }
        }

        /// <summary>
        /// ��ȡ����Χ�Ŀ�ʼ��㡣
        /// </summary>
        public LinkedListNode<T> First
        {
            get
            {
                return m_First;
            }
        }

        /// <summary>
        /// ��ȡ����Χ���ս��ǽ�㡣
        /// </summary>
        public LinkedListNode<T> Terminal
        {
            get
            {
                return m_Terminal;
            }
        }

        /// <summary>
        /// ��ȡ����Χ�Ľ��������
        /// </summary>
        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 0;
                for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// ����Ƿ����ָ��ֵ��
        /// </summary>
        /// <param name="value">Ҫ����ֵ��</param>
        /// <returns>�Ƿ����ָ��ֵ��</returns>
        public bool Contains(T value)
        {
            for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
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
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly LinkedListRange<T> m_GameFrameworkLinkedListRange;
            private LinkedListNode<T> m_Current;
            private T m_CurrentValue;

            internal Enumerator(LinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new ArgumentException("Range is invalid.");
                }

                m_GameFrameworkLinkedListRange = range;
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }

            /// <summary>
            /// ��ȡ��ǰ��㡣
            /// </summary>
            public T Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// ��ȡ��ǰ��ö������
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_CurrentValue;
                }
            }

            /// <summary>
            /// ����ö������
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// ��ȡ��һ����㡣
            /// </summary>
            /// <returns>������һ����㡣</returns>
            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_GameFrameworkLinkedListRange.m_Terminal)
                {
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }

            /// <summary>
            /// ����ö������
            /// </summary>
            void IEnumerator.Reset()
            {
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }
        }
    }
}
