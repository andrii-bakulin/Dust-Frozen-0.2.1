using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    [InitializeOnLoad]
    public static class DuFactoryEditorController
    {
        private static HashSet<DuFactory> m_QueueUpdateFactory;

        static DuFactoryEditorController()
        {
            m_QueueUpdateFactory = new HashSet<DuFactory>();

            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdated;
            EditorApplication.update += EditorUpdate;
        }

        static void OnPrefabInstanceUpdated(GameObject instance)
        {
            // Notice:
            // Yes. DuFactoryInstance has [DisallowMultipleComponent] flag, but if user ?SOMEHOW? added few components
            // to a single prefab, then require analyze of all this components!
            var duFactoryInstances = instance.GetComponents<DuFactoryInstance>();

            foreach (var duFactoryInstance in duFactoryInstances)
            {
                if (Dust.IsNull(duFactoryInstance) || Dust.IsNull(duFactoryInstance.parentFactory))
                    continue;

                var duFactory = duFactoryInstance.parentFactory;

                if (m_QueueUpdateFactory.Contains(duFactory))
                    continue;

                if (duFactory.autoRebuildOnPrefabUpdated == false)
                {
#if DUST_DEBUG_FACTORY_BUILDER
                    Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                        string.Format("Skip factory rebuilding [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif
                    continue;
                }

                m_QueueUpdateFactory.Add(duFactory);

#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "OnPrefabInstanceUpdated",
                    string.Format("Require rebuild factory [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif
            }
        }

        static void EditorUpdate()
        {
            if (m_QueueUpdateFactory.Count == 0)
                return;

            // Why need this?
            // I had situation when I edit prefab but DuFactory object already not existed in the Scene.
            // As a result, I tried to call RebuildInstances() method for non-existed objects and had Error in console.
            // After that m_QueueUpdateFactory.Clear() not called (it was after the loop).
            // So, I decide to move Clear() call before the loop.
            //
            // Hm... Hello to Garbage Collector?!

            DuFactory[] duFactories = m_QueueUpdateFactory.ToArray();
            m_QueueUpdateFactory.Clear();

            foreach (var duFactory in duFactories)
            {
#if DUST_DEBUG_FACTORY_BUILDER
                Dust.Debug.CheckpointWarning("Factory.Controller", "UpdateParentFactory",
                    string.Format("Rebuild [{0} (#{1})]", duFactory.gameObject.name, duFactory.transform.GetInstanceID()));
#endif

                duFactory.RebuildInstances();
            }
        }
    }
}
