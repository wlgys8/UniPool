using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace MS.CommonUtils.Profiler.Editor{
    internal class ProfilerWindow : EditorWindow
    {
        [MenuItem("Window/UniPool/Profiler")]
        public static void Open(){

            var win = EditorWindow.GetWindow<ProfilerWindow>();
            win.titleContent = new GUIContent("PoolProfiler");
            win.ShowPopup();
        }
        private List<EditorPoolProfiler.PoolStatics> _statics = new List<EditorPoolProfiler.PoolStatics>();
        void OnEnable(){
            EditorApplication.update += EditorUpdate;
        }
        void OnDisable(){
            EditorApplication.update -= EditorUpdate;
        }

        private void EditorUpdate(){
            if(EditorPoolProfiler.isDirty){
                EditorPoolProfiler.CleanDirty();
                this.Repaint();
            }
        }

        private void OnGUI() {
            DrawToolButton();
            EditorPoolProfiler.ListPoolStatics(_statics);
            var headerRect = EditorGUILayout.GetControlRect(GUILayout.Height(15));
            DrawHeader(headerRect);
            foreach(var s in _statics){
                var rect = EditorGUILayout.GetControlRect(GUILayout.Height(15));
                DrawStaticsItem(rect,s);
            }
        }

        private void DrawToolButton(){
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("GC")){
                System.GC.Collect();
            }
            var trackName = Profiler.EditorPoolProfiler.isTrackOn ? "Disable Track":"Enable Track";
            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
            if(GUILayout.Button(trackName)){
                if(Profiler.EditorPoolProfiler.isTrackOn){
                    DisableTrack();
                }else{
                    EnableTrack();
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private void EnableTrack(){
            var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,defineSymbols + ";" + "UNIPOOL_TRACK");
        }

        private void DisableTrack(){
            var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var array = defineSymbols.Split(';').Where((value)=>{
                return value != "UNIPOOL_TRACK";
            });
            defineSymbols = string.Join(";",array);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,defineSymbols);
        }

        private void DrawStaticsItem(Rect rect, EditorPoolProfiler.PoolStatics staticsItem){
            var x = rect.x;
            var y = rect.y;
            var width = rect.width / 3;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),staticsItem.name);
            x += width;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),staticsItem.freeCount + "/" + staticsItem.totalAllocateCount);
            x += width;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),staticsItem.Target == null ? "Dead":"Alive");
            x += width;
        }

        private void DrawHeader(Rect rect){
            var x = rect.x;
            var y = rect.y;
            var width = rect.width / 3;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),"Name");
            x += width;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),"Free/Alloc");
            x += width;
            EditorGUI.LabelField(new Rect(x,y,width,rect.height),"Status");
            x += width;
        }
    }
}
