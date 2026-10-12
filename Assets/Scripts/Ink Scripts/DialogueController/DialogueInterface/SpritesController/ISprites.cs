using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySpace
{
    public interface ISprites
    {
        public void AddPlaces(List<GameObject> places);
        public void AddPlaces(GameObject RootPlace);
        public void DeletePlace(string namePlace);
        public void ClearPlaces();

        public void AddSpriteField(string namePlace, string nameSprite);
        public void MoveSpriteField(string namePlace, string nameSprite);
        public void DestroySpriteField(string name);
        public void ClearSpriteFields();

        public void SetActiveSpriteField(string name);
        public void SetSprite(string nameSprite, string nameCharacter = null);

        public void Clear();
    }
}