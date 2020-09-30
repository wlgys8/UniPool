using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace MS.CommonUtils.Profiler{

    public static class EditorPoolProfiler
    {

        private static Dictionary<int,PoolStatics> _staticsMap = new Dictionary<int, PoolStatics>();

        private static PoolStatics GetStatics(object pool){
            var hash = pool.GetHashCode();
            if(!_staticsMap.ContainsKey(hash)){
                _staticsMap.Add(hash,new PoolStatics(pool));
            }
            return _staticsMap[hash];
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TrackAllocate(object pool){
            GetStatics(pool).totalAllocateCount ++;
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TrackRequest(object pool){
            GetStatics(pool).freeCount --;
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TrackRelease(object pool){
            GetStatics(pool).freeCount ++;
        }

        public static List<PoolStatics> ListPoolStatics(){
            GC.Collect();
            List<PoolStatics> statics = new List<PoolStatics>();
            var keys = _staticsMap.Keys.ToArray();
            foreach(var key in keys){
                var v = _staticsMap[key];
                if(v.Target == null){
                    _staticsMap.Remove(key);
                    continue;
                }
                statics.Add(v);
            }
            return statics;
        }



        public class PoolStatics{

            public readonly WeakReference poolReference;
            public readonly string name;
            public int totalAllocateCount;
            public int freeCount;

            public PoolStatics(object pool){
                poolReference = new WeakReference(pool);
                this.name = pool.ToString();
            }

            public object Target{
                get{
                    return poolReference.Target;
                }
            }
        }


        
    }
}
