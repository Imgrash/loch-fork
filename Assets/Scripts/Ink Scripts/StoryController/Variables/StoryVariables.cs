using Ink.Runtime;
using StorySpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StorySpace
{
    public class StoryVariables
    {
        private bool useGlobalVariables = false;
        private GlobalVariablesManager gvm { 
            get => GlobalVariablesManager.getInstance();
        }
        private VariablesState state;
        private Dictionary<string, Ink.Runtime.Object> variables = new Dictionary<string, Ink.Runtime.Object>();
        public Dictionary<string, Ink.Runtime.Object> Variables { get { return variables; } }
        public StoryVariables(Story st_, bool useGlobalVariables = false)
        {
            this.useGlobalVariables = useGlobalVariables;
            if(!useGlobalVariables) 
                gvm.AddLocal(this);
            state = st_.variablesState;
            state.variableChangedEvent += changedVariable;
            foreach (string name in state)
            {
                var val = state.GetVariableWithName(name);
                variables.Add(name, val);
            }

        }
        public bool variableExists(string name) {
            var a = variables.ContainsKey(name);
            return a; 
        }
        public bool TryGetVariable(string name, out Ink.Runtime.Object val)
        {
            //если нет в локальных переменных
            return !variables.TryGetValue(name, out val) ?
                //и родительские (глобальные) переменные не назначены
                (!useGlobalVariables ?
                    //возвращаем false
                    false :
                    //иначе смотрим в глобальных переменных
                    gvm.TryGetVariable(name, out val)
                )
                //или, если в локальных переменных всё же есть, возвращаем true
                : true;
        }

        public void changeVariable(string name, Ink.Runtime.Object val, bool explicitlyHere = false)
        {
            if (useGlobalVariables && gvm.variableExists(name) && !explicitlyHere)
                gvm.changeVariable(name, val);
            if (variableExists(name))
                state.SetGlobal(name, val);
        }
        private void changedVariable(string name, Ink.Runtime.Object val)
        {
            if (variables.ContainsKey(name))
                variables[name] = val;
            else
                variables.Add(name, val);
        }
        ~StoryVariables() { state.variableChangedEvent -= changedVariable; }
    }
}