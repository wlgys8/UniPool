using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.CommonUtils{

    /// <summary>
    /// List类型的Pool实现。 用以重复利用List类型对象，减少GC开销。
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public class ListPool<T>
    {
    
        private static AutoAllocatePool<List<T>> _cache = new AutoAllocatePool<List<T>>();

        /// <value>当前池中的List对象数量</value>
        public static int Count{
            get{
                return _cache.Count;
            }
        }

        public static List<T> Request(){
            return _cache.Request();
        }

        /// <summary>
        /// List will be cleared and put back into the pool
        /// 将一个List对象放回池中。List.Clear会被调用。
        /// </summary>
        public static void Release(List<T> list){
            list.Clear();
            _cache.Release(list);
        }
    }
}
