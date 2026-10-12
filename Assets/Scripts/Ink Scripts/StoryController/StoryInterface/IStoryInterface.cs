using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorySpace
{
    /// <summary>
    /// Облегчённый интерфейс для работы с объектом Story
    /// </summary>
    public interface IStoryInterface
    {
        public void activateDialogueWindow();
        public void activateDialogueWindow(IDialogueInterface di);

        #region NavigateStory

        #region NavigateToKnotStitch

        /// <summary>
        /// Переводит состояние истории на указанный путь
        /// </summary>
        /// <param name="KnotName">Имя Knot-а</param>
        /// <param name="StitchName">Имя Stitch-а, если указан</param>
        public bool goToKnot(string KnotName, string StitchName = "");
        #endregion NavigateToKnotStitch

        #region Choices
        public Dictionary<int, string> CurrentDialogueChoices();
        /// <summary>
        /// Если есть варианты выбора диалогов, здесь можно по индексу перейти на ветку диалога
        /// </summary>
        /// <param name="choiceIndex">Индекс ветки</param>
        /// <exception cref="Exception">Исключение, если ветки с таким индексом нет</exception>
        public void MakeChoice(int choiceIndex);

        /// <summary>
        /// Здесь по названию ветки диалога можно получить её индекс
        /// </summary>
        /// <param name="choiceName">Название ветки</param>
        /// <returns>Индекс ветки</returns>
        /// <exception cref="Exception">Исключение, если ветки с таким названием нет</exception>
        public string ChoiceValue(int choiceIndex);
        #endregion Choices

        #region ContinueStory
        /// <summary>
        /// Вызывать чтобы получить следующий по диалогу кусок текста
        /// </summary>
        /// <returns>следующий кусок текста</returns>
        public bool nextChunkOfStory(out string chunkOfStory);
        #endregion ContinueStory

        #endregion NavigateStory

        #region StoryProgramming

        #region Variables
        public void changeVariable(string name, Ink.Runtime.Object val);
        public bool TryGetVariable(string name, out Ink.Runtime.Object val);
        #endregion Variables

        #region StoryFunctions
        public bool CallStoryFunction(string functionName, out object result, object arguments = null);
        #endregion StoryFunctions

        #region HandleTags;
        public List<string> getCurrentTags();
        #endregion HandleTags

        #endregion StoryProgramming
    }
}
