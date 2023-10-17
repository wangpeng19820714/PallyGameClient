using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// �ַ���ص�ʵ�ú�����
        /// </summary>
        public static partial class Text
        {

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T">�ַ������������͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg">�ַ���������</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T>(string format, T arg)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2>(string format, T1 arg1, T2 arg2)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                 return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <typeparam name="T12">�ַ������� 12 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <param name="arg12">�ַ������� 12��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <typeparam name="T12">�ַ������� 12 �����͡�</typeparam>
            /// <typeparam name="T13">�ַ������� 13 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <param name="arg12">�ַ������� 12��</param>
            /// <param name="arg13">�ַ������� 13��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <typeparam name="T12">�ַ������� 12 �����͡�</typeparam>
            /// <typeparam name="T13">�ַ������� 13 �����͡�</typeparam>
            /// <typeparam name="T14">�ַ������� 14 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <param name="arg12">�ַ������� 12��</param>
            /// <param name="arg13">�ַ������� 13��</param>
            /// <param name="arg14">�ַ������� 14��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <typeparam name="T12">�ַ������� 12 �����͡�</typeparam>
            /// <typeparam name="T13">�ַ������� 13 �����͡�</typeparam>
            /// <typeparam name="T14">�ַ������� 14 �����͡�</typeparam>
            /// <typeparam name="T15">�ַ������� 15 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <param name="arg12">�ַ������� 12��</param>
            /// <param name="arg13">�ַ������� 13��</param>
            /// <param name="arg14">�ַ������� 14��</param>
            /// <param name="arg15">�ַ������� 15��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            }

            /// <summary>
            /// ��ȡ��ʽ���ַ�����
            /// </summary>
            /// <typeparam name="T1">�ַ������� 1 �����͡�</typeparam>
            /// <typeparam name="T2">�ַ������� 2 �����͡�</typeparam>
            /// <typeparam name="T3">�ַ������� 3 �����͡�</typeparam>
            /// <typeparam name="T4">�ַ������� 4 �����͡�</typeparam>
            /// <typeparam name="T5">�ַ������� 5 �����͡�</typeparam>
            /// <typeparam name="T6">�ַ������� 6 �����͡�</typeparam>
            /// <typeparam name="T7">�ַ������� 7 �����͡�</typeparam>
            /// <typeparam name="T8">�ַ������� 8 �����͡�</typeparam>
            /// <typeparam name="T9">�ַ������� 9 �����͡�</typeparam>
            /// <typeparam name="T10">�ַ������� 10 �����͡�</typeparam>
            /// <typeparam name="T11">�ַ������� 11 �����͡�</typeparam>
            /// <typeparam name="T12">�ַ������� 12 �����͡�</typeparam>
            /// <typeparam name="T13">�ַ������� 13 �����͡�</typeparam>
            /// <typeparam name="T14">�ַ������� 14 �����͡�</typeparam>
            /// <typeparam name="T15">�ַ������� 15 �����͡�</typeparam>
            /// <typeparam name="T16">�ַ������� 16 �����͡�</typeparam>
            /// <param name="format">�ַ�����ʽ��</param>
            /// <param name="arg1">�ַ������� 1��</param>
            /// <param name="arg2">�ַ������� 2��</param>
            /// <param name="arg3">�ַ������� 3��</param>
            /// <param name="arg4">�ַ������� 4��</param>
            /// <param name="arg5">�ַ������� 5��</param>
            /// <param name="arg6">�ַ������� 6��</param>
            /// <param name="arg7">�ַ������� 7��</param>
            /// <param name="arg8">�ַ������� 8��</param>
            /// <param name="arg9">�ַ������� 9��</param>
            /// <param name="arg10">�ַ������� 10��</param>
            /// <param name="arg11">�ַ������� 11��</param>
            /// <param name="arg12">�ַ������� 12��</param>
            /// <param name="arg13">�ַ������� 13��</param>
            /// <param name="arg14">�ַ������� 14��</param>
            /// <param name="arg15">�ַ������� 15��</param>
            /// <param name="arg16">�ַ������� 16��</param>
            /// <returns>��ʽ������ַ�����</returns>
            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
            {
                if (format == null)
                {
                    throw new ArgumentException("Format is invalid.");
                }

                return string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            }
        }
    }

}

