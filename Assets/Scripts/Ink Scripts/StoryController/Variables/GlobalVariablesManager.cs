using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

namespace StorySpace
{
    public class GlobalVariablesManager
    {
        private static GlobalVariablesManager instance;

        private string globalsFilePath = "Assets//Ink_docs//variables.ink";
        private StoryVariables globalVariables;
        private List<StoryVariables> localVariables = new List<StoryVariables>();
        private GlobalVariablesManager()
        {
            string inkFileContents = File.ReadAllText(globalsFilePath);
            Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
            globalVariables = new StoryVariables(compiler.Compile());
        }
        public static GlobalVariablesManager getInstance() => instance ??= new GlobalVariablesManager();

        public void AddLocal(StoryVariables sv)
        {
            localVariables.Add(sv);
            foreach (var item in globalVariables.Variables)
            {
                if(sv.variableExists(item.Key))
                    sv.changeVariable(item.Key, item.Value);
            }
        }

        public void RemoveLocal(StoryVariables sv) => localVariables.Remove(sv);
        public bool TryGetVariable(string name, out Ink.Runtime.Object val) => globalVariables.TryGetVariable(name, out val);
        public bool variableExists(string name) => globalVariables.variableExists(name);
        public void changeVariable(string name, Ink.Runtime.Object val)
        {
            globalVariables.changeVariable(name, val, true);
            localVariables.ForEach(x => x.changeVariable(name, val, true));
        }
    }
}