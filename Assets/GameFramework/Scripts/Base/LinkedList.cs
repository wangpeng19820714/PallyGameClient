using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// ��Ϸ��������ࡣ
    /// </summary>
    /// <typeparam name="T">ָ�������Ԫ�����͡�</typeparam>
    public sealed class GameFrameworkLinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private readonly LinkedList<T> m_LinkedList;
        private readonly Queue<LinkedListNode<T>> m_CachedNodes;

        /// <summary>
        /// ��ʼ����Ϸ������������ʵ����
        /// </summary>
        public GameFrameworkLinkedList()
        {
            m_LinkedList = new LinkedList<T>();
            m_CachedNodes = new Queue<LinkedListNode<T>>();
        }

        /// <summary>
        /// ��ȡ������ʵ�ʰ����Ľ��������
        /// </summary>
        public int Count
        {
            get
            {
                return m_LinkedList.Count;
            }
        }

        /// <summary>
        /// ��ȡ�����㻺��������
        /// </summary>
        public int CachedNodeCount
        {
            get
            {
                return m_CachedNodes.Count;
            }
        }

        /// <summary>
        /// ��ȡ����ĵ�һ����㡣
        /// </summary>
        public LinkedListNode<T> First
        {
            get
            {
                return m_LinkedList.First;
            }
        }

        /// <summary>
        /// ��ȡ��������һ����㡣
        /// </summary>
        public LinkedListNode<T> Last
        {
            get
            {
                return m_LinkedList.Last;
            }
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ ICollection`1 �Ƿ�Ϊֻ����
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)m_LinkedList).IsReadOnly;
            }
        }

        /// <summary>
        /// ��ȡ������ͬ���� ICollection �ķ��ʵĶ���
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return ((ICollection)m_LinkedList).SyncRoot;
            }
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵָʾ�Ƿ�ͬ���� ICollection �ķ��ʣ��̰߳�ȫ����
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)m_LinkedList).IsSynchronized;
            }
        }

        /// <summary>
        /// ��������ָ�������н�����Ӱ���ָ��ֵ���½�㡣
        /// </summary>
        /// <param name="node">ָ�������н�㡣</param>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>����ָ��ֵ���½�㡣</returns>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        /// <summary>
        /// ��������ָ�������н������ָ�����½�㡣
        /// </summary>
        /// <param name="node">ָ�������н�㡣</param>
        /// <param name="newNode">ָ�����½�㡣</param>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddAfter(node, newNode);
        }

        /// <summary>
        /// ��������ָ�������н��ǰ��Ӱ���ָ��ֵ���½�㡣
        /// </summary>
        /// <param name="node">ָ�������н�㡣</param>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>����ָ��ֵ���½�㡣</returns>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }

        /// <summary>
        /// ��������ָ�������н��ǰ���ָ�����½�㡣
        /// </summary>
        /// <param name="node">ָ�������н�㡣</param>
        /// <param name="newNode">ָ�����½�㡣</param>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddBefore(node, newNode);
        }

        /// <summary>
        /// ������Ŀ�ͷ����Ӱ���ָ��ֵ���½�㡣
        /// </summary>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>����ָ��ֵ���½�㡣</returns>
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddFirst(node);
            return node;
        }

        /// <summary>
        /// ������Ŀ�ͷ�����ָ�����½�㡣
        /// </summary>
        /// <param name="node">ָ�����½�㡣</param>
        public void AddFirst(LinkedListNode<T> node)
        {
            m_LinkedList.AddFirst(node);
        }

        /// <summary>
        /// ������Ľ�β����Ӱ���ָ��ֵ���½�㡣
        /// </summary>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>����ָ��ֵ���½�㡣</returns>
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddLast(node);
            return node;
        }

        /// <summary>
        /// ������Ľ�β�����ָ�����½�㡣
        /// </summary>
        /// <param name="node">ָ�����½�㡣</param>
        public void AddLast(LinkedListNode<T> node)
        {
            m_LinkedList.AddLast(node);
        }

        /// <summary>
        /// ���������Ƴ����н�㡣
        /// </summary>
        public void Clear()
        {
            LinkedListNode<T> current = m_LinkedList.First;
            while (current != null)
            {
                ReleaseNode(current);
                current = current.Next;
            }

            m_LinkedList.Clear();
        }

        /// <summary>
        /// ��������㻺�档
        /// </summary>
        public void ClearCachedNodes()
        {
            m_CachedNodes.Clear();
        }

        /// <summary>
        /// ȷ��ĳֵ�Ƿ��������С�
        /// </summary>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>ĳֵ�Ƿ��������С�</returns>
        public bool Contains(T value)
        {
            return m_LinkedList.Contains(value);
        }

        /// <summary>
        /// ��Ŀ�������ָ����������ʼ�����������Ƶ����ݵ�һά���顣
        /// </summary>
        /// <param name="array">һά���飬���Ǵ������Ƶ�Ԫ�ص�Ŀ�ꡣ���������д��㿪ʼ��������</param>
        /// <param name="index">array �д��㿪ʼ���������Ӵ˴���ʼ���ơ�</param>
        public void CopyTo(T[] array, int index)
        {
            m_LinkedList.CopyTo(array, index);
        }

        /// <summary>
        /// ���ض��� ICollection ������ʼ���������Ԫ�ظ��Ƶ�һ�������С�
        /// </summary>
        /// <param name="array">һά���飬���Ǵ� ICollection ���Ƶ�Ԫ�ص�Ŀ�ꡣ���������д��㿪ʼ��������</param>
        /// <param name="index">array �д��㿪ʼ���������Ӵ˴���ʼ���ơ�</param>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)m_LinkedList).CopyTo(array, index);
        }

        /// <summary>
        /// ���Ұ���ָ��ֵ�ĵ�һ����㡣
        /// </summary>
        /// <param name="value">Ҫ���ҵ�ָ��ֵ��</param>
        /// <returns>����ָ��ֵ�ĵ�һ����㡣</returns>
        public LinkedListNode<T> Find(T value)
        {
            return m_LinkedList.Find(value);
        }

        /// <summary>
        /// ���Ұ���ָ��ֵ�����һ����㡣
        /// </summary>
        /// <param name="value">Ҫ���ҵ�ָ��ֵ��</param>
        /// <returns>����ָ��ֵ�����һ����㡣</returns>
        public LinkedListNode<T> FindLast(T value)
        {
            return m_LinkedList.FindLast(value);
        }

        /// <summary>
        /// ���������Ƴ�ָ��ֵ�ĵ�һ��ƥ���
        /// </summary>
        /// <param name="value">ָ��ֵ��</param>
        /// <returns>�Ƿ��Ƴ��ɹ���</returns>
        public bool Remove(T value)
        {
            LinkedListNode<T> node = m_LinkedList.Find(value);
            if (node != null)
            {
                m_LinkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }

            return false;
        }

        /// <summary>
        /// ���������Ƴ�ָ���Ľ�㡣
        /// </summary>
        /// <param name="node">ָ���Ľ�㡣</param>
        public void Remove(LinkedListNode<T> node)
        {
            m_LinkedList.Remove(node);
            ReleaseNode(node);
        }

        /// <summary>
        /// �Ƴ�λ������ͷ���Ľ�㡣
        /// </summary>
        public void RemoveFirst()
        {
            LinkedListNode<T> first = m_LinkedList.First;
            if (first == null)
            {
                throw new ArgumentException("First is invalid.");
            }

            m_LinkedList.RemoveFirst();
            ReleaseNode(first);
        }

        /// <summary>
        /// �Ƴ�λ�������β���Ľ�㡣
        /// </summary>
        public void RemoveLast()
        {
            LinkedListNode<T> last = m_LinkedList.Last;
            if (last == null)
            {
                throw new ArgumentException("Last is invalid.");
            }

            m_LinkedList.RemoveLast();
            ReleaseNode(last);
        }

        /// <summary>
        /// ����ѭ�����ʼ��ϵ�ö������
        /// </summary>
        /// <returns>ѭ�����ʼ��ϵ�ö������</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_LinkedList);
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> node = null;
            if (m_CachedNodes.Count > 0)
            {
                node = m_CachedNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }

            return node;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            m_CachedNodes.Enqueue(node);
        }

        /// <summary>
        /// ��ֵ��ӵ� ICollection`1 �Ľ�β����
        /// </summary>
        /// <param name="value">Ҫ��ӵ�ֵ��</param>
        void ICollection<T>.Add(T value)
        {
            AddLast(value);
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
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                if (linkedList == null)
                {
                    throw new ArgumentException("Linked list is invalid.");
                }

                m_Enumerator = linkedList.GetEnumerator();
            }

            /// <summary>
            /// ��ȡ��ǰ��㡣
            /// </summary>
            public T Current
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
                ((IEnumerator<T>)m_Enumerator).Reset();
            }
        }
    }
}
