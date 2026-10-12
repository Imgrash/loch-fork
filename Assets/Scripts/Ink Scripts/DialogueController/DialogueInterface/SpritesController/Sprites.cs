using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySpace
{
    public class Sprites : MonoBehaviour, ISprites
    {
        public GameObject prefabSpriteField;
        private Dictionary<string, GameObject> Places = new();
        private Dictionary<string, string> takenPlaces = new();
        public void AddPlaces(List<GameObject> places)
        {
            foreach (var item in places)
            {
                if (!this.Places.ContainsKey(item.name))
                    this.Places.Add(item.name, item);
                else
                    Debug.Log("Место \"" + item.name + "\" уже есть в списке");
            }
        }
        public void AddPlaces(GameObject RootPlace)
        {
            if (RootPlace.transform.childCount > 0)
            {
                List<GameObject> places = new();
                for (int i = 0; i < RootPlace.transform.childCount; i++)
                {
                    places.Add(RootPlace.transform.GetChild(i).gameObject);
                }
                AddPlaces(places);
            }
            else
                AddPlaces(new List<GameObject> { RootPlace });
        }
        private bool tryGetPlace(string name, out GameObject place) => Places.TryGetValue(name, out place);
        public void DeletePlace(string namePlace)
        {
            Places.Remove(namePlace);
            freePlace(namePlace);
        }
        public void ClearPlaces()
        {
            Places.Clear();
            takenPlaces.Clear();
        }
        private bool isPlaceTaken(string namePlace) => takenPlaces.ContainsKey(namePlace);
        private void takePlace(string namePlace, string name) => takenPlaces.Add(namePlace, name);
        private void freePlace(string namePlace) => takenPlaces.Remove(namePlace);
        private void freePlaceBySpriteFieldName(string name) => takenPlaces.Remove(SpriteFields.GetValueOrDefault(name).GetPlace());
        private void freePlaceBySpriteField(ISpriteField sf) => takenPlaces.Remove(sf.GetPlace());

        private Dictionary<string, ISpriteField> SpriteFields = new();
        private bool spriteNameExists(string name) => SpriteFields.ContainsKey(name);
        public void AddSpriteField(string namePlace, string nameSprite)
        {
            GameObject place = null;
            if (!tryGetPlace(namePlace, out place))
            {
                Debug.Log("Не найдено место \"" + namePlace + "\"");
                return;
            }
            if (spriteNameExists(nameSprite))
            {
                Debug.Log("Поле \"" + name + "\" уже существует");
                return;
            }

            ISpriteField spriteField = null;
            if (!isPlaceTaken(namePlace))
            {
                spriteField = Instantiate(prefabSpriteField).GetComponent<ISpriteField>();
                spriteField.Set(place, nameSprite);
                AddSpriteField(spriteField);
                takePlace(namePlace, nameSprite);
            }
            else
            {
                Debug.Log("Место \"" + namePlace + "\" занято");
                return;
            }

        }
        public void MoveSpriteField(string namePlace, string nameSprite)
        {
            GameObject place = null;
            if (!tryGetPlace(namePlace, out place))
            {
                Debug.Log("Не найдено место \"" + namePlace + "\"");
                return;
            }
            if (!spriteNameExists(nameSprite))
            {
                Debug.Log("Поля \"" + name + "\" не существует");
                return;
            }

            ISpriteField spriteField = null;
            if (!isPlaceTaken(namePlace))
            {
                freePlaceBySpriteField(spriteField);
                spriteField.SetPlace(place);
                takePlace(namePlace, nameSprite);
            }
            else
            {
                Debug.Log("Место \"" + namePlace + "\" занято");
                return;
            }

        }

        private void AddSpriteField(ISpriteField spriteField)
        {
            SpriteFields.Add(spriteField.GetName(), spriteField);
        }
        public void DestroySpriteField(string name)
        {
            if (SpriteFields.TryGetValue(name, out ISpriteField sf))
            {
                if (activeSpriteField == sf)
                {
                    activeSpriteField = null;
                }
                freePlace(sf.GetPlace());
                SpriteFields.Remove(name);
                sf.DestroySelf();
            }
            else
            {
                Debug.Log("Не найдено поля \"" + name + "\"");
                return;
            }
        }
        public void ClearSpriteFields()
        {
            activeSpriteField = null;
            foreach (var item in SpriteFields)
            {
                freePlace(item.Value.GetPlace());
                item.Value.DestroySelf();
            }
            SpriteFields.Clear();
        }
        public void Clear()
        {
            ClearPlaces();
            ClearSpriteFields();
        }

        private ISpriteField activeSpriteField = null;

        public void SetActiveSpriteField(string name)
        {
            if (SpriteFields.TryGetValue(name, out ISpriteField sf))
            {
                activeSpriteField = sf;
            }
            else
            {
                Debug.Log("Не найдено поля \"" + name + "\"");
                return;
            }
        }
        public void SetSprite(string nameSprite, string nameCharacter = null)
        {
            ISpriteField sf = null;
            if (!string.IsNullOrEmpty(nameCharacter))
            {
                if (!SpriteFields.TryGetValue(nameCharacter, out sf))
                {
                    Debug.Log("Не найдено поля \"" + nameCharacter + "\"");
                    return;
                }
                SetActiveSpriteField(nameCharacter);
            }
            else if (activeSpriteField == null)
                Debug.Log("Активное поле пусто");
            else
                sf = activeSpriteField;
            sf.ShowSprite(nameSprite);
        }
    }
}