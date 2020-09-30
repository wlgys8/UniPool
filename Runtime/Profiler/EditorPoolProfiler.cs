using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace MS.CommonUtils.Profiler{

    public static class EditorPoolProfiler
    {

        private static Dictionary<int,PoolStatics> _staticsMap = new Dictionary<int, PoolStatics>();
        private static bool _dirty = false;

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
            _dirty = true;
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TrackRequest(object pool){
            GetStatics(pool).freeCount --;
             _dirty = true;
        }

        [Conditional("UNITY_EDITOR")]
        internal static void TrackRelease(object pool){
            GetStatics(pool).freeCount ++;
             _dirty = true;
        }

        public static bool isDirty{
            get{
                return _dirty;
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void CleanDirty(){
            _dirty = false;
        }

        public static void ListPoolStatics(List<PoolStatics> outList, bool gcInvoke = false){
            if(gcInvoke){
                GC.Collect();
            }
            outList.Clear();
            foreach(var kv in _staticsMap){
                var v = kv.Value;
                outList.Add(v);
            }
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
