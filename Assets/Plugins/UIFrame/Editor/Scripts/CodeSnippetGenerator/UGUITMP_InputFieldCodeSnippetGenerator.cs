using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Feif.Extensions;
using System.Linq;
using TMPro;

namespace Feif.UIFramework.Editor
{
    public class UGUITEMP_InputFieldCodeSnippetGenerator : CodeSnippetGenerator
    {
        public override int GetPriority()
        {
            return 140;
        }

        public override List<GameObject> GetGameObjects(GameObject prefab)
        {
            return prefab.transform.BreadthTraversal(t => t != prefab.transform && t.GetComponent<UIBase>() != null)
                .Where(item => item.GetComponent<TMP_InputField>() != null && item.name.StartsWith("@"))
                .Select(item => item.gameObject)
                .ToList();
        }

        public override List<string> GenerateField(GameObject gameObject)
        {
            var name = gameObject.name;
            var fieldName = name.Replace("@", "");
            fieldName = fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1);
            return new List<string>()
            {
                $"[SerializeField] private TMP_InputField {fieldName};"
            };
        }
    }
}