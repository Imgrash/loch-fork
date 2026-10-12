===begin===
{DoorLocked:
    ->DoorCheck
    }
{not DoorLocked:
    ->DoorLocked
}

===DoorLocked===
Дверь заело... Не могу открыть.
Придётся идти в обход.
бу#putEmptySprite:Стул1-Char1#changeSprite:Char1_neutral 1-Char1#CharName:Абоба
абее#changeSprite:Char1_small#HideName
бебу#deleteSprite:Char1#CharName
боба#HideName#LoadScene:level_boss_big
абябя
->END

===DoorCheck===
Я уже проверял эту дверь.
бу#putEmptySprite:Стул1-Char1#changeSprite:Char1_neutral 1-Char1#CharName:Абоба
абее#changeSprite:Char1_small-Char1#HideName
бебу#deleteSprite:Char1#CharName
боба#HideName#LoadScene:level_boss_big
абябя
->END
