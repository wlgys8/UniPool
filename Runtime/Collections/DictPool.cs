using System.Collections;
using System.Collections.Generic;
namespace MS.CommonUtils{
    public static class DictPool<K,V>{
        
        private static AutoAllocatePool<Dictionary<K,V>> _cache = new AutoAllocatePool<Dictionary<K,V>>();
   
        public static int Count{
            get{
                return _cache.Count;
            }
        }

        public static Dictionary<K,V> Request(){
            return _cache.Request();
        }

        /// <summary>
        /// dict will be cleared and put back into the pool
        /// </summary>
        public static void Release(Dictionary<K,V> dict){
            dict.Clear();
            _cache.Release(dict);
        }
    }
}
