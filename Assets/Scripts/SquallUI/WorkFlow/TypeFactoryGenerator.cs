using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace SquallUI
{
    public class TypeFactorGenerator
    {
        private static object _generator;

        [UnityEditor.InitializeOnLoadMethod]
        private static void OnInitializeOnLoad()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            _generator = new object();
            EditorCoroutineUtility.StartCoroutine(DoCompareGenerate(), _generator);
        }

        private static IEnumerator DoCompareGenerate()
        {
            yield return null;
            CompareGenerate();
            _generator = null;
        }

        private static void CompareGenerate()
        {
            List<Type> generateTypes = new List<Type>();
            List<Type> configTypes = new List<Type>();
            CollectHotUpdate(generateTypes,configTypes);

            bool generated = false;
            EditorApplication.LockReloadAssemblies();
            try
            {
                bool newType = false;
                foreach(var t in generateTypes)
                {
                    if (!TypeFactory.Contain(t))
                    {
                        newType = true;
                        break;
                    }
                }
                foreach (var t in configTypes)
                {
                    if (!TypeFactory.Contain(t))
                    {
                        newType = true;
                        break;
                    }
                }
                if (newType)
                {
                    Generate(generateTypes,configTypes);
                    generated = true;
                }
            }
            catch (Exception err)
            {
                Debug.LogError(err);
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
                if (generated)
                {
                    AssetDatabase.Refresh();
                }
            }
        }

        private static void Generate(List<Type> generateTypes,List<Type> configTypes)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine();
            builder.AppendLine("public partial class TypeFactory");
            builder.AppendLine("{");
            builder.AppendLine("    static TypeFactory()");
            builder.AppendLine("    {");

            for(int i = 0; i < configTypes.Count; ++i)
            {
                string typeFullName = configTypes[i].FullName.Replace('+', '.');
                builder.AppendLine($"        RegisterCreator<{typeFullName}>();");
            }
        
            for(int i = 0; i < generateTypes.Count; ++i)
            {
                string typeFullName = generateTypes[i].FullName.Replace('+', '.');
                builder.AppendLine($"        RegisterCreator<{typeFullName}>();");
            }
        
            builder.AppendLine("    }");
            builder.AppendLine("}");
            File.WriteAllText("Assets/Scripts/SquallUI/Utils/TypeFactory/TypeFactory.Data.cs", builder.ToString());
        }

        private static void CollectHotUpdate(List<Type> generateTypes,List<Type> configTypes)
        {
            // var containerType = typeof(YourClass);
            var viewType = typeof(IView);
            var groupType = typeof(IGroup);

            // 预先取出三类派生类型
            // var containers = TypeCache.GetTypesDerivedFrom(containerType);
            var views = TypeCache.GetTypesDerivedFrom(viewType);
            var groups = TypeCache.GetTypesDerivedFrom(groupType);

            // 用 HashSet 去重，避免类型同时命中多个集合（理论上可能）
            var visited = new HashSet<Type>();

            void TryAdd(Type t, bool isContainer)
            {
                if (!visited.Add(t))
                    return;

                // 抽象类、嵌套 private、无无参构造 -> 跳过
                if (t.IsAbstract || t.IsNestedPrivate || t.GetConstructor(Type.EmptyTypes) == null)
                    return;

                if (isContainer)
                    configTypes.Add(t);
                else
                    generateTypes.Add(t);
            }

            // foreach (var t in containers) TryAdd(t, true);
            foreach (var t in views) TryAdd(t, false);
            foreach (var t in groups) TryAdd(t, false);
        }
    }
}
