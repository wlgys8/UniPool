using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.CommonUtils{
    public static class SetPool<T>
    {
        private static AutoAllocatePool<HashSet<T>> _cache = new AutoAllocatePool<HashSet<T>>();

        /// <value>当前池中的Set对象数量</value>
        public static int Count{
            get{
                return _cache.Count;
            }
        }

        public static HashSet<T> Request(){
            return _cache.Request();
        }

        /// <summary>
        /// Set will be cleared and put back into the pool
        /// </summary>
        public static void Release(HashSet<T> set){
            set.Clear();
            _cache.Release(set);
        }
    }
}
