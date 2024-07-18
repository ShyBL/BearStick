//PickupData class inherits from ScriptableObject
class PickupData : ScriptableObject
{
    //Declaring variables to be used
    int gainValue;
    int rarity;
    int originalValue;
    int originalRarity;
    string name;

    //Sprite variable
    Sprite pickupSprite;

    //GainValue function
    //getter and setter
    int GainValue
    {
        get { return gainValue; }
        set { gainValue = value; }
    }

    //Rarity function
    //getter and setter
    int Rarity
    {
        get { return rarity; }
        set { rarity = value; }
    }

    //I believe this is similar to a C++ class constructor
    //On initialization, object will have gainValue, rarity, and name set
    Initialize()
    {
        originalValue = gainValue;
        originalRarity = rarity;

        SetSprite();
        name = pickupSprite.sprite.name;
    }

    //Sets value back to default on reset
    ResetData()
    {
        gainValue = originalValue;
        rarity = originalValue;
    }

    SetSprite()
    {
        //Load sprites from the "Pickups" folder
        Sprite[] sprites = Resources.LoadAll<Sprite>("Pickups");

        string rarityName = GetRarityName(rarity);

        //LINQ (?) function to shuffle list and find the first sprite with the matching name
        sprites selectedSprite = sprites
            .OrderBy(sprites => Random.value)
            .FirstOrDefault(sprite => sprite.name.Contains(rarityName));

        if (selectedSprite != null)
        {
            pickupSprite = selectedSprite;
        }
        else
        {
            selectedSprite = sprites
                .OrderBy(sprite => Random.value)
                .FirstOrDefault(sprites => sprite.name.Contains(rarityName));

            if (selectedSprite != null)
            {
                pickupSprite = selectedSprite;
            }
        }
    }

    //Pseudo randomization of pickup rarity
    string GetRarityName(int i)
    {
        if (i > 5)
        {
            return "Rare";
        }
        else if (i > 2)
        {
            return "Uncommon";
        }
        else
        {
            return "Common";
        }
    }

}