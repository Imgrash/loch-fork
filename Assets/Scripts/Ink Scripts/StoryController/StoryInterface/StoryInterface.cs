using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Ink.Runtime;
using System.Threading;

namespace StorySpace
{
    public class knot
    {
        public string knotname;
        public List<string> stitches;
        public knot(string knotname_, List<string> stitches_)
        {
            knotname = knotname_;
            stitches = stitches_;
        }
    }

    /// <summary>
    /// Облегчённый интерфейс для работы с объектом Story
    /// </summary>
    public class StoryInterface : MonoBehaviour, IStoryInterface
    {
        public GameObject DialogueInterfaceDefault;
        public IDialogueInterface dialogueInterface;
        public TextAsset js;
        private Story st;
        /// <param name="js">json, скомпиленный из Ink-а</param>
        public void Start()
        {
            st = new Story(js.text);
            Knots = st.mainContentContainer.namedContent.Keys.Select(knot =>
                new knot(
                    knot.ToString()
                    , st.KnotContainerWithName(knot).namedContent.Keys.ToList())
            ).ToList();
            vars = new StoryVariables(st, true);
            if(DialogueInterfaceDefault != null)
                DialogueInterfaceDefault.TryGetComponent<IDialogueInterface>(out dialogueInterface);
            if (dialogueInterface == null)
            {
                Debug.Log("Ищем instance DialogueInterface");
                var a = DialogueInterface.instance;
                if(a != null)
                {
                    dialogueInterface = a;
                    DialogueInterfaceDefault = a.gameObject;
                }
            }
            if (dialogueInterface == null)
            {
                Debug.Log("Ищем FindFirstObjectByType DialogueInterface");
                var a = FindFirstObjectByType<DialogueInterface>(FindObjectsInactive.Include);
                if (a != null)
                {
                    dialogueInterface = a;
                    DialogueInterfaceDefault = a.gameObject;
                }
            }
        }

        public void activateDialogueWindow()
        {
            dialogueInterface.activate(this);
        }
        public void activateDialogueWindow(IDialogueInterface di)
        {
            this.dialogueInterface = di;
            activateDialogueWindow();
        }

        #region NavigateStory

        #region NavigateToKnotStitch
        public List<knot> Knots;

        /// <summary>
        /// Переводит состояние истории на указанный путь
        /// </summary>
        /// <param name="KnotName">Имя Knot-а</param>
        /// <param name="StitchName">Имя Stitch-а, если указан</param>
        public bool goToKnot(string KnotName, string StitchName = "")
        {
            try
            {
                if (Knots.Any(x => x.knotname == KnotName && string.IsNullOrEmpty(StitchName) || x.stitches.Contains(StitchName)))
                {
                    st.ChoosePathString(KnotName + (string.IsNullOrEmpty(StitchName) ? "" : "." + StitchName));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e) { Debug.Log(e.Message); return false; }
        }
        #endregion NavigateToKnotStitch

        #region Choices
        public Dictionary<int, string> CurrentDialogueChoices() => st.currentChoices.ToDictionary(x =>x.index, x =>  x.text);
        /// <summary>
        /// Если есть варианты выбора диалогов, здесь можно по индексу перейти на ветку диалога
        /// </summary>
        /// <param name="choiceIndex">Индекс ветки</param>
        /// <exception cref="Exception">Исключение, если ветки с таким индексом нет</exception>
        public void MakeChoice(int choiceIndex)
        {
            CurrentDialogueChoices().TryGetValue(choiceIndex, out var choice);
            if (CurrentDialogueChoices().ContainsKey(choiceIndex))
                st.ChooseChoiceIndex(choiceIndex);
            else
                throw new Exception("Нет такого выбора");
        }

        /// <summary>
        /// Здесь по названию ветки диалога можно получить её индекс
        /// </summary>
        /// <param name="choiceName">Название ветки</param>
        /// <returns>Индекс ветки</returns>
        /// <exception cref="Exception">Исключение, если ветки с таким названием нет</exception>
        public string ChoiceValue(int choiceIndex)
        {
            string val;
            bool contains = CurrentDialogueChoices().TryGetValue(choiceIndex, out val);
            if (!contains)
                throw new Exception("Нет такого выбора");
            else
                return val;
        }
        #endregion Choices

        #region ContinueStory
        /// <summary>
        /// Вызывать чтобы получить следующий по диалогу кусок текста
        /// </summary>
        /// <returns>следующий кусок текста</returns>
        public bool nextChunkOfStory(out string chunkOfStory)
        {
            chunkOfStory = null;
            if (st.canContinue)
            {
                chunkOfStory = st.Continue();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion ContinueStory

        #endregion NavigateStory

        #region StoryProgramming

        #region Variables
        public StoryVariables vars;
        public void changeVariable(string name, Ink.Runtime.Object val) => vars.changeVariable(name, val);
        public bool TryGetVariable(string name, out Ink.Runtime.Object val) => vars.TryGetVariable(name, out val);
        #endregion Variables

        #region StoryFunctions
        public bool CallStoryFunction(string functionName, out object result, object arguments = null)
        {
            result = null;
            if (st.HasFunction(functionName))
            {
                object[] args = null;
                if (arguments is List<object> lo)
                    args = lo.ToArray();
                else if (arguments is object[] oa)
                    args = oa;
                else if (arguments == null)
                    args = null;
                else
                    args = new List<object>() { arguments }.ToArray();
                result = args == null ? st.EvaluateFunction(functionName) : st.EvaluateFunction(functionName, arguments);
                return true;
            }
            else
                return false;
        }
        #endregion StoryFunctions

        #region HandleTags
        public List<string> getCurrentTags() => st.currentTags;

        #endregion HandleTags

        #endregion StoryProgramming
    }
}